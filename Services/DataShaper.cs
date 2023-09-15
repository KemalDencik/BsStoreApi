using Entities.DataTransferObject;
using Entities.Models;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class DataShaper<T> : IDataShaper<T>
        where T : class
    {
        public PropertyInfo[] Properties { get; set; }
        public DataShaper()
        {
            Properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);//bu ifadenin bir array dönemsini sağladım 
            //hangi propları istediğimi binding üzerinden çözüyorum
        }
    
        
        private IEnumerable<PropertyInfo> GetRequiredProperties(string fieldsString)
        {
            var requiredFields = new List<PropertyInfo>(); //seçilen alanları içeren bir liste yaptık 
            if (!string.IsNullOrWhiteSpace(fieldsString)) 
            { 
                //virgülle ayrılma işlemi
                var fields= fieldsString.Split(',',  //bir array elde ettim 
                    StringSplitOptions.RemoveEmptyEntries);//boş olanları kaldır dolu olanları al 

                foreach(var field in fields)
                {
                    var property = Properties
                        .FirstOrDefault(pi => pi.Name.Equals(field.Trim(),StringComparison.InvariantCultureIgnoreCase
                        ));
                    if (property is null)
                        continue;
                    requiredFields.Add(property);
                }
            }
            else
            {
                requiredFields = Properties.ToList(); //datashaping yok ise tüm listeyi döndür
            }
            return requiredFields;
        } 

        //verileri çekme
        private ShapedEntity FetchDataForEntity(T entity , IEnumerable<PropertyInfo> requiredProperties)
        {
            var shapedObject = new ShapedEntity();
            foreach (var property in requiredProperties)
            {
                var objectPropertyValue = property.GetValue(entity);//id değeri al
                shapedObject.Entity.TryAdd(property.Name, objectPropertyValue);
            }

            var objectProperty = entity.GetType().GetProperty("Id");//ıd için manuel süreç
            shapedObject.Id = (int)objectProperty.GetValue(entity);
            return shapedObject;
        }
        //liste için tanım
        private IEnumerable<ShapedEntity> FetchData(IEnumerable<T> entities,IEnumerable<PropertyInfo> requiredProperties)
        {
            var shapedData = new List<ShapedEntity>();
            foreach (var entity in entities)
            {
                var shapedObject = FetchDataForEntity(entity, requiredProperties);
                shapedData.Add(shapedObject);
            }   
            return shapedData;
        }

        public IEnumerable<ShapedEntity> ShapeData(IEnumerable<T> entities, string fieldsString)
        {
            var requiredFields = GetRequiredProperties(fieldsString);
            return FetchData(entities, requiredFields);
        }

        public ShapedEntity ShapeData(string fieldsString, T entities)
        {
            var requiredProperties = GetRequiredProperties(fieldsString);
            return FetchDataForEntity(entities, requiredProperties);//şekillendirmiş olduğu veriyi vermiş oldu çalışma zamanında
        }
    }
}
