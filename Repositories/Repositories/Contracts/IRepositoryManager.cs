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
        IBookRepository Book { get; }
        //kayıt işlemi
        Task SaveAsync();

    }
}
