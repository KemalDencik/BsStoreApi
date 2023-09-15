using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using Repositories.EFCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* BookRepository sınıfı, RepositoryBase<Book> sınıfından kalıtım aldığı için temel 
 * CRUD (Create, Read, Update, Delete) işlemlerini otomatik olarak gerçekleştirebilir.
 * Book tipine özgü işlemleri ve sorguları tanımlamak için IBookRepository arabirimini uygular.*/
namespace Repositories.EFCore
{
    //bir class yalnızca bir tek class dan alınır ama ınterface de bu durum geçerli değil
    //sealed la zırhladık kullanılamayacak devir alınamayacak 
    public sealed class BookRepository : RepositoryBase<Book>, IBookRepository
    {
        public BookRepository(RepositoryContext context) : base(context)
        {
        }

        public void CreateOneBook(Book book) => Create(book);
        public void DeleteOneBook(Book book) => Delete(book);
        public async Task<PagedList<Book>> GetAllBooksAsync(
            BookParameters bookParameters,
            bool trackChanges)
        {
          var books =  await
                        FindAll(trackChanges)
                       .FilterBooks(bookParameters.MinPrice,bookParameters.MaxPrice)
                       .Search(bookParameters.SearchTerm)
                       .Sort(bookParameters.OrderBy)
                       .ToListAsync();

            return PagedList<Book>
                    .ToPagedList(books,bookParameters.PageNumber,bookParameters.PageSize);
        }

        //v2 için
        public async Task<List<Book>> GetAllBooksAsync(bool trackChanges)
        {
            return await FindAll(trackChanges)
                .OrderBy(b => b.Id)
                .ToListAsync();
        }

        public async Task<Book> GetOneBookByIdAsync(int id, bool trackChanges) =>
           await FindByCondition(b => b.Id.Equals(id), trackChanges).SingleOrDefaultAsync();
        public void UpdateOneBook(Book book) => Update(book);
    }
}
