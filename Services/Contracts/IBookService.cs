using Entities.DataTransferObject;
using Entities.LinkModels;
using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    /* IBookService arabirimi, kitap işlemlerini gerçekleştirmek için bir dizi metod içerir.
     Bu metodlar:
     * kitap verileri üzerinde işlemler yapmayı sağlar ve bir veri kaynağına erişim sağlamadan önce iş mantığını uygulamak için kullanılır. */
    public interface IBookService
    {
        //foreach ile dolaşabileceğim bir ifade tanımladım
        //imzalar ve veri transfer kullanacağımız için ayrı ayrı kullanmamız önemli
        Task<(LinkResponse linkResponse, MetaData metaData)> GetAllBooksAsync(LinkParameters linkParameters,bool trackChanges);
        Task<BookDto> GetOneBookByIdAsync(int id,bool trackChanges);
        Task<BookDto> CreateOneBookAsync(BookDtoForInsertion book);
        Task UpdateOneBookAsync(int id,BookDtoForUpdate bookDto,bool trackChanges);
        Task DeleteOneBookAsync(int id,bool trackChanges);
        Task< (BookDtoForUpdate bookDtoForUpdate, Book book)> GetOneBookForPatchAsync(int id,bool trackChanges);
        Task SaveChangesForPatchAsync(BookDtoForUpdate bookDtoForUpdate, Book book);
        Task<List<Book>> GetAllBooksAsync(bool trackChanges);
    }
}
