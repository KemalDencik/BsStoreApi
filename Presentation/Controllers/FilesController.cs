using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.EFCore;
using Services.Contracts;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/files")]
    public class FilesController : ControllerBase
    {
        private readonly RepositoryContext _context; // RepositoryContext'ini bağımlılık enjeksiyonuyla ekledik
        private readonly IServiceManager _services;
        public FilesController(RepositoryContext context, IServiceManager services)
        {
            _context = context;
            _services = services;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Geçersiz dosya");
            }

            // Dosyanın kaydedileceği klasörü belirle
            var folder = Path.Combine(Directory.GetCurrentDirectory(), "Files");

            // Klasör yoksa oluştur
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            // Dosyanın kaydedileceği yol
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var path = Path.Combine(folder, fileName);

            // Dosyayı diske kaydet
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Veritabanına kaydet
            var fileUrl = GenerateFileUrl(fileName); // Dosyanın URL'sini oluştur
            var uploadedFile = new UploadedFile
            {
                FileName = fileName,
                Url = fileUrl,
                Size = file.Length
            };

            _context.UploadedFiles.Add(uploadedFile);
            await _context.SaveChangesAsync();

            // Yanıt döndür
            return Ok(new
            {
                file = fileName,
                url = fileUrl,
                size = file.Length
            });
        }

        private string GenerateFileUrl(string fileName)
        {
            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            return $"{baseUrl}/api/files/download/{fileName}";
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFilesAsync()
        {
            return Ok(await _services.UploadFilesService.GetAllFilesAsync(false));

        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOneFilesByIdAsync([FromRoute] int id)
        {
            return Ok(await _services
                .UploadFilesService
                .GetOneFilesByIdAsync(id, false));
        }
    }
}