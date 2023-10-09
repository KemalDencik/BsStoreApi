using AutoMapper;
using Entities.DataTransferObject;
using Entities.Exceptions;
using Entities.LinkModels;
using Entities.Models;
using Entities.RequestFeatures;
using Repositories.Contracts;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class BookManager : IBookService
    {
        //herşey manager üzerinde dönecek
        //newlemeleri IRepositoryManager da yapıyoruz o sebeple bunu kullanıyoruz
        private readonly ICategoryService _categoryService;
        private readonly IRepositoryManager _manager;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;
        private readonly IBookLinks _bookLinks;
        //bu kodla çözümlüyorum DI 
        public BookManager(
            IRepositoryManager manager,
            ILoggerService logger,
            IMapper mapper,
            IBookLinks bookLinks,
            ICategoryService categoryService)
        {
            _manager = manager;
            _logger = logger;
            _mapper = mapper;
            _bookLinks = bookLinks;
            _categoryService = categoryService;
        }

        public async Task<BookDto> CreateOneBookAsync(BookDtoForInsertion bookDto)
        {
            var category = await _categoryService.GetOneCategoryByIdAsnyc(bookDto.CategoryId, false);

            var entity =  _mapper.Map<Book>(bookDto);

            _manager.Book.CreateOneBook(entity);
            await _manager.SaveAsync();
            return _mapper.Map<BookDto>(entity);    
        }

        public async Task DeleteOneBookAsync(int id, bool trackChanges)
        {
            var entity = await GetOneBookByIdCheckExistsAsync(id, trackChanges);

            _manager.Book.DeleteOneBook(entity);
            await _manager.SaveAsync();
        }

        public async Task<(LinkResponse linkResponse, MetaData metaData)> GetAllBooksAsync
            (LinkParameters linkParameters,bool trackChanges)
        {
            if(!linkParameters.BookParameters.ValidPriceRange)
                throw new PriceOutofRangeBadRequestException();

            //repoya manager üzerinden geçiş yaptık 
            var booksWithMetaData= await _manager.Book.GetAllBooksAsync(linkParameters.BookParameters, trackChanges);

            var booksDto= _mapper.Map<IEnumerable<BookDto>>(booksWithMetaData);
            //var shapedData = _shaper.ShapeData(booksDto, bookParameters.Fields);
            var links = _bookLinks.TryGenerateLinks(booksDto, linkParameters.BookParameters.Fields,linkParameters.HttpContext);//LİNK ÜRETECEZ
            return (linkResponse: links, metaData: booksWithMetaData.MetaData);
        }

        //v2 nin bu 
        public async Task<List<Book>> GetAllBooksAsync(bool trackChanges)
        {
            var books =await _manager.Book.GetAllBooksAsync(trackChanges);
            return books;
        }

        public async Task<IEnumerable<Book>> GetAllBooksWithDetailsAsync(bool trackChanges)
        {
            return await _manager
                .Book
                .GetAllBooksWithDetailsAsync(trackChanges);
        }

        public async Task<BookDto> GetOneBookByIdAsync(int id, bool trackChanges)
        {
          var book = await GetOneBookByIdCheckExistsAsync(id,trackChanges);

            if (book is null)
                throw new BookNotFoundException(id);
            return _mapper.Map<BookDto>(book);
        }
        //hem kitap nesnesini hem de ilgili dtoyu service katmanından sunum katmanına iletmek için 
        public async Task<(BookDtoForUpdate bookDtoForUpdate, Book book)> GetOneBookForPatchAsync(int id, bool trackChanges)
        {
            var book = await GetOneBookByIdCheckExistsAsync(id, trackChanges);

            var bookDtoForUpdate = _mapper.Map<BookDtoForUpdate>(book);

            return (bookDtoForUpdate, book);
        }

        public async Task SaveChangesForPatchAsync(BookDtoForUpdate bookDtoForUpdate, Book book)
        {
            _mapper.Map(bookDtoForUpdate, book);
            await _manager.SaveAsync();

        }

        public async Task UpdateOneBookAsync(int id,
            BookDtoForUpdate bookDto,
            bool trackChanges)
        {
            var category = await _manager.Book.GetOneBookByIdAsync(bookDto.CategoryId, false);
            var entity = await GetOneBookByIdCheckExistsAsync(id, trackChanges);

            if (category is null)
                throw new CategoryNotFoundException(bookDto.CategoryId);

            entity.CategoryId = bookDto.CategoryId;
            //mapping işlemi var burda

            entity = _mapper.Map<Book>(bookDto);
            _manager.Book.Update(entity);
            await _manager.SaveAsync();
        }

        //varlığı kontrol et
        private async Task<Book> GetOneBookByIdCheckExistsAsync(int id , bool trackChanges)
        {
            //check entity?
            var entity = await _manager.Book.GetOneBookByIdAsync(id, trackChanges);

            if (entity is null)
                throw new BookNotFoundException(id);
            return entity;
        }
        
    }
}
