using Talabat.Core.Entities.OrderAggregation;

namespace Talabat.APIs.DTO
{
    public class OrderToReturnDto
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } 
        public string Status { get; set; } 
        public Address ShippingAddress { get; set; }
       
        public string DeliveryMethod { get; set; }
        public string DeliveryMethodCost { get; set; }
        public ICollection<OrderItemDto> Items { get; set; } = new HashSet<OrderItemDto>();
        public decimal Total { get; set; }
        public decimal SubTotal { get; set; }

       
        public string PaymentIntentId { get; set; } 
    }
}
