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
        private readonly IBookRepository _bookRepository;
        private readonly ICategoryRepository _categoryRepository;

        public RepositoryManager(
            RepositoryContext context,
            IBookRepository bookRepository,
            ICategoryRepository categoryRepository)
        {
            _context = context;
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
        }

        //context e erişim 

        //IOC kaydını yapıyoruz context i çözmek için
        //bu sayede book ifadesi bir repoya karşılık geliyor
        //bizden bookrepoyu istediği anda _bookRepository.Value bu şekilde dönmüş olacaz nesne ancak ve ancak kullanılınca ilgili ifade new lenecek

        //bu alan tanımlanmasaydı bir üst sınıftan categoryrepoya erişemezdin çnükü ilk başta private tanımladım 
        public IBookRepository Book => _bookRepository;
        public ICategoryRepository Category => _categoryRepository;

        public async Task SaveAsync()
        {
           await _context.SaveChangesAsync();
        }
    }
}
