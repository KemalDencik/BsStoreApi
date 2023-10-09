using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.EFCore
{
    public class UploadFilesRepository : RepositoryBase<UploadedFile>, IUploadFilesRepository
    {
        public UploadFilesRepository(RepositoryContext context) : base(context)
        {
        }
        public void DeleteOneFiles(UploadedFile uploadedFile)=>Delete(uploadedFile);

        public async Task<IEnumerable<UploadedFile>> GetAllFilesAsync(bool trackChanges)
        {
            return await FindAll(trackChanges).ToListAsync();
        }

        public async Task<UploadedFile> GetOneFilesByIdAsync(int id, bool trackChanges)
        {
            return await FindByCondition(c => c.Id.Equals(id), trackChanges)
                .SingleOrDefaultAsync();
        }
    }
}
