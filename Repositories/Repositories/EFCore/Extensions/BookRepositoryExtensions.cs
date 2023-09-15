using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.EFCore.Extensions
{
    public static class BookRepositoryExtensions
    {
        public static IQueryable<Book> FilterBooks(this IQueryable<Book> books,
            uint minPrice,
            uint maxPrice) => books.Where(book => book.Price >= minPrice && book.Price <= maxPrice);

        public static IQueryable<Book> Search(this IQueryable<Book> books,string searhTerm)
        {
            if (string.IsNullOrWhiteSpace(searhTerm))
                return books; //boşsa books u getir 

            var lowerCaseTerm = searhTerm.Trim().ToLower(); //büyük küçük harf ne şekilde gelirse gelsin getirecek trim ise boşlukları alıyor
                return books
                    .Where(b => b.Title
                    .ToLower()
                    .Contains(searhTerm));
        }

        public static IQueryable<Book> Sort(this IQueryable<Book> books, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return books.OrderBy(b => b.Id);

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Book>(orderByQueryString); //oluşturulan sorgu elimizde

            if (orderQuery is null)
                return books.OrderBy(b => b.Id);
            return books.OrderBy(orderQuery);
        }
    }
}
