using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.OrderAggregation;

namespace Talabat.Core.Specifications.OrdereSepc
{
    public class OrderSecifications:BaseSpecifications<Order>
    {
        public OrderSecifications(string email):base(o=>o.BuyerEmail==email)
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);
            AddOrderbyDesc(o => o.OrderDate);
        }

        public OrderSecifications(string email,int id):base(o => o.BuyerEmail == email && o.Id==id)
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);
        }
    }
}
