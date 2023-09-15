using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Entities.LogModel
{
    public class LogDetails
    {
        //obje olarak tanılmadık çünkü context üzerinden tanımlıyor olacaz 
        public Object? ModelName { get; set; }
        public Object? Controller { get; set; }
        public Object? Action { get; set; }
        public Object? Id { get; set; }
        //bu log ne zaman meydana geliyorsa onu tutatacak 
        public Object? CreateAt { get; set; }
        //zaman bilgisini de aldık 
        public LogDetails()
        {
            CreateAt = DateTime.UtcNow;
        }
        //Bu, türetilmiş bir sınıfın, temel sınıfın davranışını değiştirmek veya genişletmek istediği durumlarda kullanılır.
        //LogDetails nesnesini bir metne dönüştürmek için
        public override string ToString() => JsonSerializer.Serialize(this);
    }
}
