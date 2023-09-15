using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//IRepositoryBase de CRUD tanımlamış olmamıza rağmen burada tanımladık
// sebebi ise BookRepositoryde daha esnek işlemeler yapabilmek için
//kısaca işlemleri sınıflara göre özelleştirmiş olduk
//trackChanges parametresi, değişiklikleri izleyip izlememeyi belirten bir parametredir.
namespace Repositories.Contracts
{
    public interface IBookRepository : IRepositoryBase<Book>
    {
        Task<PagedList<Book>> GetAllBooksAsync(BookParameters bookParameters ,bool trackChanges);
        //tek bir kayıt için alttaki ifadeyi onun altındakiyle değiştirdik
        //IQueryable<Book> GetOneBooksById(int id,bool trackChanges);

        //v2 için 
        Task<List<Book>> GetAllBooksAsync(bool trackChanges);
        Task<Book> GetOneBookByIdAsync(int id,bool trackChanges);
        void CreateOneBook(Book book);
        void UpdateOneBook(Book book);
        void DeleteOneBook(Book book);
        
    }
}
