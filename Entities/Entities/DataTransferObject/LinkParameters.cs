using Entities.RequestFeatures;
using Microsoft.AspNetCore.Http;


namespace Entities.DataTransferObject
{
    // record: bir controller var presenttation layer da burdan veriyi alacaz service katmanına doğru iletecez aradaki geçişi sağlamak için 
    //verinin değişmemesni sağlamak için record type kullandık 
    public record LinkParameters
    {
        public  BookParameters BookParameters { get; init; } //init değişmiyor yazma işlemi yok 
        public HttpContext HttpContext { get; init; }

    }
}
