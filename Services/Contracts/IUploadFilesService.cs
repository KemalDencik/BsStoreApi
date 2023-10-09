using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IUploadFilesService
    {
        Task<IEnumerable<UploadedFile>> GetAllFilesAsync(bool trackChanges);
        Task<UploadedFile> GetOneFilesByIdAsync(int id, bool trackChanges);
    }
}
