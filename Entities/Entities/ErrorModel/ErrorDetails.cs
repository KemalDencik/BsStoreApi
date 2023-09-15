using System.Text.Json;

namespace Entities.ErrorModel
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }

        public override string ToString()
        {
            //serilaze kullanıcıya derli toplu bir hata gösterir
            return JsonSerializer.Serialize(this);
        }
    }
}
