using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
	//base bir class old için abstract yaptık
    public abstract class RequestParameters
    {
		//maximum 50 kayıt ver
		const int maxPageSize = 50;
		//auto-implemented property
        public int PageNumber { get; set; }

		//full-property
		private int _pageSize;

		public int PageSize
		{
			get { return _pageSize; }
			//50 den büyük bi şey se maxsize ı dön küçükse yani 10 kayıt sa buna destek ver demiş oldum 
			set { _pageSize = value > maxPageSize ? maxPageSize : value; }
		}
        public String? OrderBy { get; set; }
        public String? Fields{ get; set; }

    }
}
