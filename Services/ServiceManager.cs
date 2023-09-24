using AutoMapper;
using Entities.DataTransferObject;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//bütün servisler burda toplanır
namespace Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IBookService> _bookService;
        private readonly Lazy<ICategoryService> _categoryService;
        private readonly Lazy<IAuthenticationService> _authenticationService;
        public ServiceManager(IRepositoryManager repositoryManager,
            ILoggerService logger,
            IMapper mapper,
            IConfiguration configuration,
            UserManager<User> userManager,
            IBookLinks bookLinks)
        {
            //MANAGER İFADESİ İMPLEMANTASİONONUNYANİ DETAYLARIN OLDUĞU BİR NESNE BU NESNE İÇİNDE MAPPER KULLANDIĞIMIZ İÇİN
            //BİR ENJEKSİYON YAPTIK BU SEBEPLE GELİP BURDA MAPPER VERMEK ZORUNDAYIZ
            _bookService = new Lazy<IBookService>(() => 
            new BookManager(repositoryManager, logger, mapper,bookLinks));

            _categoryService=new Lazy<ICategoryService>(()=>new CategoryManager(repositoryManager));

            _authenticationService = new Lazy<IAuthenticationService>(() =>
            new AuthenticationManager(logger,mapper,userManager,configuration));
        }
        public IBookService BookService => _bookService.Value;

        public IAuthenticationService AuthenticationService => _authenticationService.Value;

        //ne zaman biri bizden bu nesneyi isterse value dönecez public kısım burası 
        public ICategoryService CategoryService => _categoryService.Value;
    }
}
