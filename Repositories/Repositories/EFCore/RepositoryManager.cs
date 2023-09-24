using Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.EFCore
{
    public class RepositoryManager : IRepositoryManager
    {
        //save işlemi context üzerinde yapma
        private readonly RepositoryContext _context;
        //eager loading yerine lazy yi seçme sebebi gereksiz kaynak kullanımından kaçınma
        private readonly Lazy<IBookRepository> _bookRepository;
        private readonly Lazy<ICategoryRepository> _categoryRepository;
        //context e erişim 
        public RepositoryManager(RepositoryContext context)
        {
            _context = context;
            _bookRepository = new Lazy<IBookRepository>(() => new BookRepository(_context));
            _categoryRepository= new Lazy<ICategoryRepository>(() => new CategoryRepository(_context));
        }
        //IOC kaydını yapıyoruz context i çözmek için
        //bu sayede book ifadesi bir repoya karşılık geliyor
        //bizden bookrepoyu istediği anda _bookRepository.Value bu şekilde dönmüş olacaz nesne ancak ve ancak kullanılınca ilgili ifade new lenecek

        //bu alan tanımlanmasaydı bir üst sınıftan categoryrepoya erişemezdin çnükü ilk başta private tanımladım 
        public IBookRepository Book => _bookRepository.Value;
        public ICategoryRepository Category => _categoryRepository.Value;

        public async Task SaveAsync()
        {
           await _context.SaveChangesAsync();
        }
    }
}
