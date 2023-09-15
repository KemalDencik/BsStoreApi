using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.LinkModels
{
    public class Link
    {
        public string? Href { get; set; } //link bilgisini nereye vereceksek bu amaçla kullanılır
        public string? Rel { get; set; } //linki tanımlayacak ifade silme mi güncellememi 
        public string? Method { get; set; } // "GET", "POST", "PUT", "DELETE"
        //jsona çevirilirken boş ctor a ihtiyaç olacak 
        //Muhtemelen bu şekilde bir örnek oluşturulduğunda, ilgili özelliklere daha sonra değer atanacak demektir.
        public Link()
        {
            
        }
        //Yani bu yapıcıyı kullanarak direkt olarak bir bağlantının özelliklerini belirterek örnek oluşturabilirsiniz.
        public Link(string? href, string? rel, string? method)
        {
            Href = href;
            Rel = rel;
            Method = method;
        }
    }
}
