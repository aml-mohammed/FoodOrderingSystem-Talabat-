using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.OrderAggregation;

namespace Talabat.Core.Specifications.OrdereSepc
{
    public class OrderPaymentIntentSpec:BaseSpecifications<Order>
    {
        public OrderPaymentIntentSpec(string paymentIntentId):base(o=>o.PaymentIntentId==paymentIntentId)
        {

        }
    }
}
