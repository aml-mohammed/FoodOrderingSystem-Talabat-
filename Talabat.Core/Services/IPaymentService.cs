using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Services
{
    public interface IPaymentService
    {
        //create or update payment intent
        Task<CustomerBasket> CreateOrUpdatePaymentIntent(string BasketId);
    }
}
