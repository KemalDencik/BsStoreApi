using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.EFCore.Extensions
{
    public static class OrderQueryBuilder
    {
        public static String CreateOrderQuery<T>(String orderByQueryString)
        {
            /*örneğin books?OrderBy = title,price önce title sonra price a göre arama bu sayede bir dizi elde edecez ve virgülle ayıracaz
          * o ıncı id title
          * 1. id price olur
          */
            var orderParams = orderByQueryString.Trim().Split(',');
            //book daki verilere erişim 
            var propertyInfos = typeof(T)//reflection için
                .GetProperties(BindingFlags.Public | BindingFlags.Instance);//public ifadeler ya da new lenebilen ifade erişimi

            var orderQueryBuilder = new StringBuilder(); //bir sorgu ifadesi yani bir string tanımlıyorum

            //sorgudan aldığım ifadeyle property yi eşleştirecem 
            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                    continue; //param için boş ise bir sonraki ifadeye geç

                var propertyFromQueryName = param.Split(" ")[0];

                var objectProperty = propertyInfos
                    .FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName
                    //Büyük küçük harf uyumuna dikkat etmez
                    , StringComparison.InvariantCultureIgnoreCase)); //büyük küçük ayrımı olmasın istiyoruz

                if (objectProperty is null)
                    continue;

                var direction = param.EndsWith(" desc") ? "descending" : "ascending";

                orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {direction},");//bütün nesneler için çalışacak 

            }

            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

            return orderQuery;
        }

    }
}
