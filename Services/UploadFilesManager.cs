﻿using Entities.Exceptions;
using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class UploadFilesManager : IUploadFilesService
    {
        private readonly IRepositoryManager _manager;

        public UploadFilesManager(IRepositoryManager manager)
        {
            _manager = manager;
        }
        public async Task<IEnumerable<UploadedFile>> GetAllFilesAsync(bool trackChanges)
        {
            return await _manager
               .UploadFiles
               .GetAllFilesAsync(trackChanges);
        }

        public async Task<UploadedFile> GetOneFilesByIdAsync(int id, bool trackChanges)
        {
            var uploadFiles = await _manager
                .UploadFiles
                .GetOneFilesByIdAsync(id, trackChanges);

            if (uploadFiles is null)
                throw new UploadFileException(id);
            return uploadFiles;
        }

        public async Task DeleteOneUploadFiles(int id, bool trackChanges)
        {
            var file = await GetOneFileByIdCheckExistsAsync(id, trackChanges);

            _manager.UploadFiles.DeleteOneFiles(file);
            await _manager.SaveAsync();
        }

        private async Task<UploadedFile> GetOneFileByIdCheckExistsAsync(int id, bool trackChanges)
        {
            //check entity?
            var file = await _manager.UploadFiles.GetOneFilesByIdAsync(id, trackChanges);

            if (file is null)
                throw new UploadFileException(id);
            return file;
        }
    }
}
