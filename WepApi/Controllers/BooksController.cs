using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Repositories.EFCore;


namespace WepApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        // RepositoriContex bu classın newlenmesi lazım instance olması lazım bu işlemi otomatik olarak aşşağıdaki context fonksiyonunda kendisi 
        //yapıyor
        private readonly RepositoryContext _context;

        public BooksController(RepositoryContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllBooks()
        {
            try
            {
                var books = _context.Books.ToList();
                return Ok(books);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpGet("{id=int}")]
        public IActionResult GetBook([FromRoute (Name = "id")] int id)
        {
            try
            {

                var book = _context.Books.Where(b => b.Id.Equals(id)).SingleOrDefault();
                if (book is null)

                    return NotFound();
                return Ok(book);
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
            
        }
        [HttpPost]
        public IActionResult CreateOneBooks([FromBody] Book book)
        {
            try
            {
                if (book is null)
                    return BadRequest();

                _context.Books.Add(book);
                //savechanges kalıcı hale getirir
                _context.SaveChanges();

                return StatusCode(201, book);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpPut("{id:int}")]

        public IActionResult UpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] Book book)
        {
            try
            {
                var entitiy = _context
                .Books
                .Where(b => b.Id.Equals(id)).SingleOrDefault();

                if (entitiy is null)

                    return NotFound();

                if (id != book.Id)
                    return BadRequest();

                entitiy.Title = book.Title;
                entitiy.Price = book.Price;

                _context.SaveChanges();

                return Ok(book);

            }catch(Exception ex)

            {
                throw new Exception(ex.Message);
            }

        }


        [HttpDelete ("{id:int}")]
        public IActionResult DeleteAllBook([FromRoute(Name = "id")]int id)
        {
            try
            {
              var entity = _context.Books.Where(b=>b.Id.Equals(id)).SingleOrDefault();
                if (entity is null)
                    return NotFound(new
                    {
                        StatusCode = 404,
                        message = $"Book with id:{id} could not found"
                    });
                _context.Books.Remove(entity);
                _context.SaveChanges();
                
                return NoContent();
                
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
           
        }

        [HttpPatch("{id:int}")]
        public IActionResult PartiallyUpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] JsonPatchDocument<Book> bookPatch)
        {
            try
            {
                var entity = _context.Books.Where(b => b.Id.Equals(id)).SingleOrDefault();
                if (entity is null)
                    return NotFound();

                bookPatch.ApplyTo(entity);
                _context.SaveChanges();
                return NoContent();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}", ex);
            }
        }
    }
}
