using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Specifications
{
    public class ProductSpecParams
    {
        public string? sort { get; set; }
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }
        private int pgeSize=5;

        public int PageSize
        {
            get { return pgeSize; }
            set { pgeSize = value > 10 ? 10 : value; }
        }
        public int PageIndex { get; set; } = 1;
        public string Search { get; set; }

    }
}
