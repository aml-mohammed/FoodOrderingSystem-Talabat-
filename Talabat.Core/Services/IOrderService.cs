using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.OrderAggregation;

namespace Talabat.Core.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(string buyerEmail, string basketId, int deliverMethodId, Address ShippingAddress);
        Task<IReadOnlyList<Order?>> GetOrdersForSpecificUser(string buyerEmail);
        Task<Order> GetOrderByIdForSpecificUserAsync(string buyerEmail,int orderId);

    }
}
