using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IUploadFilesRepository : IRepositoryBase<UploadedFile>
    {
        Task<IEnumerable<UploadedFile>> GetAllFilesAsync(bool trackChanges);
        Task<UploadedFile> GetOneFilesByIdAsync(int id, bool trackChanges);
        void DeleteOneFiles(UploadedFile uploadedFile);

    }
}
