using Entities.DataTransferObject;
using Entities.RequestFeatures;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Services.Contracts;
using System.Text.Json;


namespace Presentation.Controllers
{
    //[ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ServiceFilter(typeof(LogFilterAttribute))]
    [ApiController]
    [Route("api/books")]
    
    //[ResponseCache(CacheProfileName ="5mins")]
    public class BooksController : ControllerBase
    {
        //controller yapısı doğrudan dbContext i tüketiyor bizim isteğimiz manager üzerinden kayıtlara erişim olsun çünkü logicler kullanılabilir
        //bu sebepren bu komutu yazıyoruz
    
        // Services ı apiye bağlıyoruz 
        private readonly IServiceManager _manager;

        public BooksController(IServiceManager manager)
        {
            _manager = manager;
        }
        //[Authorize(Roles = "admin")]
        [HttpHead]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        [HttpGet(Name ="GetAllBooksAsync")]
        //[HttpCacheExpiration(CacheLocation = CacheLocation.Public,MaxAge = 80)]
        //[ResponseCache (Duration =60)]
        public async Task<IActionResult> GetAllBooksAsync([FromQuery]BookParameters bookParameters)
        {
            var linkParameters = new LinkParameters()
            {
                BookParameters = bookParameters,
                HttpContext = HttpContext
            };
            //service gönderdik serviste repoya gönderiyor
            var result =await _manager
                .BookService
                .GetAllBooksAsync(linkParameters,false);

            Response.Headers.Add("X-Pagination",JsonSerializer.Serialize(result.metaData));

            return result.linkResponse.HasLinks ?
                Ok(result.linkResponse.LinkedEntities) :
                Ok(result.linkResponse.ShapedEntities); //link üretildiyse linkle yoksa shaped entities dön 
        }

        //[Authorize(Roles = "editor")]
        [HttpGet("{id=int}")]
        public async Task<IActionResult> GetOneBookAsync([FromRoute(Name = "id")] int id)
        {
            var book = await _manager
                        .BookService
                        .GetOneBookByIdAsync(id, false);
               
            return Ok(book);
        }

        //[Authorize]
        [HttpGet("details")]
        public async Task<IActionResult> GetAllBooksWithDetailsAsync()
        {
            return Ok(await _manager
                .BookService
                .GetAllBooksWithDetailsAsync(false));
        }

        //[Authorize(Roles = "admin")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [HttpPost(Name = "CreateOneBooksAsync")]
        public async Task<IActionResult> CreateOneBooksAsync([FromBody] BookDtoForInsertion bookDto)
        {

            var book = await _manager.BookService.CreateOneBookAsync(bookDto);
                return StatusCode(201, book);
         
        }
        
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateOneBookAsync([FromRoute(Name = "id")] int id, [FromBody] BookDtoForUpdate bookDto)
        {
            await _manager.BookService.UpdateOneBookAsync(id, bookDto, false);
            return NoContent();//204
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAllBookAsync([FromRoute(Name = "id")] int id)
        {           

            await _manager.BookService.DeleteOneBookAsync(id, false);

            return NoContent();

        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> PartiallyUpdateOneBookAsync([FromRoute(Name = "id")] int id,
            [FromBody] JsonPatchDocument<BookDtoForUpdate> bookPatch)
        {

        if(bookPatch is null)
            return BadRequest();//400

            var result = await _manager.BookService.GetOneBookForPatchAsync(id,false);  

                bookPatch.ApplyTo(result.bookDtoForUpdate,ModelState);

            TryValidateModel(result.bookDtoForUpdate);

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

            await _manager.BookService.SaveChangesForPatchAsync(result.bookDtoForUpdate,result.book);
            return NoContent();
            
        }

        [HttpOptions]
        public IActionResult GetBooksOptions()
        {
            Response.Headers.Add("Allow", "GET, PUT, POST, PATCH, DELETE, HEAD, OPTIONS"); //ALLOW ARTIK Bİ KEY DİEĞR KISIM VALUE YE KARŞILIK GELİYOR
            //HANGİ METHODLAR KULLANILABİLİR ONLARI SÖYLEDİK
            return Ok();//200 
        }

    }
}
