using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.OrderAggregation;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Core.Specifications.OrdereSepc;

namespace Talabat.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        public OrderService(IBasketRepository basketRepository,
            IUnitOfWork unitOfWork,IPaymentService paymentService)
        {
           _basketRepository = basketRepository;
           _unitOfWork = unitOfWork;
            _paymentService = paymentService;
        }

      
        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliverMethodId, Address ShippingAddress)
        {
            var basket =await _basketRepository.GetBasketAsync(basketId);
            var OrderItems = new List<OrderItem>();
            if (basket?.Items.Count > 0)
            {
                foreach (var item in basket.Items)
                {
                    var product =await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    var productItemOrder = new ProductItemOrder(product.Id, product.Name, product.PictureUrl);
                    var OrderItem = new OrderItem(productItemOrder,product.Price, item.Quantity);
                    OrderItems.Add(OrderItem);
                }
            }

            var SubTotal = OrderItems.Sum(item => item.Price * item.Quantity);
            var DeliverMethod =await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliverMethodId);

            var spec = new OrderPaymentIntentSpec(basket.PaymentIntentId);
            var ExOrder =await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);
            if(ExOrder is not null)
            {
                _unitOfWork.Repository<Order>().Delete(ExOrder);
                await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            }
            var order = new Order(buyerEmail, ShippingAddress, DeliverMethod, OrderItems, SubTotal,basket.PaymentIntentId);
           await _unitOfWork.Repository<Order>().Add(order);
         var result=   await _unitOfWork.CompleteAsync();
            if (result <= 0) return null;
            return order;

        }
        public async Task<Order> GetOrderByIdForSpecificUserAsync(string buyerEmail,int orderId)
        {
            var spec = new OrderSecifications(buyerEmail);
            var Order = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);
            return Order;
        }

        
        public  async Task<IReadOnlyList<Order>> GetOrdersForSpecificUser(string buyerEmail)
        {
            var spec = new OrderSecifications(buyerEmail);
            var Orders =await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);
            return Orders;
        }
    }
}
