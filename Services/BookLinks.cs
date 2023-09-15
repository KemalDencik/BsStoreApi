using Entities.DataTransferObject;
using Entities.LinkModels;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Net.Http.Headers;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class BookLinks : IBookLinks
    {
        private readonly LinkGenerator _linkGenerator;
        private readonly IDataShaper<BookDto> _dataShaper;

        public BookLinks(LinkGenerator linkGenerator, IDataShaper<BookDto> dataShaper)
        {
            _linkGenerator = linkGenerator;
            _dataShaper = dataShaper;

        }
        public LinkResponse TryGenerateLinks(IEnumerable<BookDto> booksDto, string fields, HttpContext httpContext)
        {
            //ŞEKİLLENDİRİLMİŞ OLAN VERİLERİ OLUŞTUR SHAPEDBOOKS A AT
            var shapedBooks = ShapeData(booksDto, fields);
            if (ShouldGenerateLinks(httpContext))
                return ReturnLinkedBooks(booksDto , fields , httpContext , shapedBooks); //link ifadelerini üreteceğimiz yapı
            return ReturnShapedBooks(shapedBooks);
            //eğer link oluşursa oluşturduğun linkleri içeren bir veri yapısıyla dönüş yapacam 
            //eğer oluşturamadıysan şekillendirilmiş veri tipiyle dönüş yapacam
        }

        //link ifadelerini üreteceğimiz yapı
        private LinkResponse ReturnLinkedBooks(IEnumerable<BookDto> booksDto,
            string fields, 
            HttpContext httpContext,
            List<Entity> shapedBooks)
        {
            var bookDtoList= booksDto.ToList(); //elimde bookdto listesi var
            for (int index = 0; index < bookDtoList.Count(); index++) //link üretmek üzere hazırladığım metoda gidiyoru for ile
            {
                var bookLinks = CreateForBook(httpContext, bookDtoList[index], fields);
                shapedBooks[index].Add("Links", bookLinks);//shapedBooks bir varlığı ifade ediyor
            }
            //link var
            var bookCollection = new LinkCollectionWrapper<Entity>(shapedBooks);
            CreateForBooks(httpContext, bookCollection);
            return new LinkResponse { HasLinks = true, LinkedEntities = bookCollection };
        }
        //bilginin nerden geldiğini öğreniyoruz postmande en altta bulunuyor
        private LinkCollectionWrapper<Entity> CreateForBooks(HttpContext httpContext, LinkCollectionWrapper<Entity> bookCollectionWrapper) 
        {
            bookCollectionWrapper.Links.Add(new Link()
            {
                Href = $"/api/{httpContext.GetRouteData().Values["controller"].ToString().ToLower()}",
                Rel="self",
                Method="GET"
            });
            return bookCollectionWrapper;
        }
        //liste oluşturacaz link listesi
        private List<Link> CreateForBook(HttpContext httpContext, BookDto bookDto, string fields)
        {
            var links = new List<Link>()
            {
                new Link()
                {
                    Href=$"/api/{httpContext.GetRouteData().Values["controller"].ToString().ToLower()}" + $"/{bookDto.Id}",
                    Rel="self",
                    Method="GET",
                },
                new Link()
                {
                    Href = $"api/{httpContext.GetRouteData().Values["controller"].ToString().ToLower()}" + $"/{bookDto.Id}",
                    Rel="create",
                    Method="POST",
                }

            };
            return links;
        }

        private LinkResponse ReturnShapedBooks(List<Entity> shapedBooks)
        {
            return new LinkResponse() { ShapedEntities = shapedBooks};
        }

        //link oluşturabiliyormusun oluşturamıyormusun
        private bool ShouldGenerateLinks(HttpContext httpContext)
        {
            var mediaType = (MediaTypeHeaderValue)httpContext.Items["AcceptHeaderMediaType"];
            return mediaType
                .SubTypeWithoutSuffix
                .EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);//InvariantCultureIgnoreCase büyük küçük harf kalktı
        }

        private List<Entity> ShapeData(IEnumerable<BookDto> booksDto, string fields)
        {
            return _dataShaper.ShapeData(booksDto, fields)
                .Select(b => b.Entity)
                .ToList();
        }
    }
}
