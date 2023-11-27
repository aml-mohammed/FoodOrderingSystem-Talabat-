using System.ComponentModel.DataAnnotations;
using Talabat.Core.Entities.OrderAggregation;

namespace Talabat.APIs.DTO
{
    public class OrderDto
    {
        [Required]
        public string BasketId { get; set; }
        public int DeliveryMethodId { get; set; }
        public AddressDto ShippingAddress { get; set; }
        
    }
}
