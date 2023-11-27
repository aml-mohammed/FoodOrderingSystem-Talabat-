using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class ProductWithBrandAndTypeSpecifications:BaseSpecifications<Product>
    {
        public ProductWithBrandAndTypeSpecifications(ProductSpecParams Params) :base(
            p=>(!Params.BrandId.HasValue || p.ProductBrandId== Params.BrandId) &&
            (!Params.TypeId.HasValue || p.ProductTypeId==Params.TypeId)  && (p.Name.Contains(Params.Search)))
        {
            Includes.Add(P => P.ProductType);
            Includes.Add(P => P.ProductBrand);
            if (!string.IsNullOrEmpty(Params.sort))
            {
                switch(Params.sort)
                {
                    case "priceAsc":
                        AddOrderby(p => p.Price);
                        break;

                    case "priceDesc":
                        AddOrderbyDesc(p=>p.Price);
                        break;
                    default:
                        AddOrderby(p => p.Name);
                        break;

                }

            }

           ApplyPagination(Params.PageSize * (Params.PageIndex - 1), Params.PageSize); 
           
        }
        public ProductWithBrandAndTypeSpecifications(int id):base(P=>P.Id==id)
        {
            Includes.Add(P => P.ProductType);
            Includes.Add(P => P.ProductBrand);

        }
    }
}
