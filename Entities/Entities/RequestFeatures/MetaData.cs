using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public class MetaData
    {
        public int CurrentPage {get; set; }
        public int TotalPage { get; set; } //toplam sayfa
        public int PageSize { get; set; } //kaç kayıt gösterileceği her sayfada
        public int TotalCount { get; set; } //toplam kayıt

        public bool HasPrevious => CurrentPage > 1; //HasPrevious yani bundan önce başka sayfa varmı  currentpage 1 den büyük ise sayfa var
        public bool HasNextPage => CurrentPage < TotalPage; //sonrasında sayfa varmı
    }
}
