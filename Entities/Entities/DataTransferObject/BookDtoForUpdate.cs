using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObject
{
    //record da referans tipli bir ifade ve classlara son derece benzeyen bir ifade
    //set yazma get okuma yapılabilir
    //zaten varlığımızda değişkenler olduğu için şu alttaki yöntem uygulanır
    //kayıt (record) tipi tanımlar. Bu tip, verileri taşımak için kullanılan bir veri transfer nesnesidir.
    public record BookDtoForUpdate : BookDtoForManipulation
    {
        [Required]
        public int Id { get; set; }

        public int CategoryId { get; set; }
    }
}
