using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    //birden çok repo muz olabilir bu repolara manager üzerinden hepsine erişim verecez
    public interface IRepositoryManager
    {
        //kayıt işlemi
        IBookRepository Book { get; }
        ICategoryRepository Category { get; }
        IUploadFilesRepository UploadFiles { get; }
        Task SaveAsync();

    }
}
