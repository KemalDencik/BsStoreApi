using Entities.DataTransferObject;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    //veri şekillendirme
    public interface IDataShaper<T>
    {
        //bir kaynak var elimde bu kayankdan hangisini seçiyorsam liste olarak dönüyorum
        IEnumerable<ShapedEntity> ShapeData(IEnumerable<T> entities,string fieldsString);
        //tek bir nesne üzerinde arama
        ShapedEntity ShapeData(string fieldsString, T entity);
      
    }
}
