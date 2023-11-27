using AutoMapper;
using Talabat.APIs.DTO;
using Talabat.Core.Entities;
using Talabat.Core.Entities.OrderAggregation;
using Talabat.Core.Entities.Identity;

namespace Talabat.APIs.Helpers
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d=>d.ProductType,o=>o.MapFrom(s=>s.ProductType.Name))
                .ForMember(d=>d.ProductBrand,o=>o.MapFrom(s=>s.ProductBrand.Name)).
                ForMember(d=>d.PictureUrl,o=>o.MapFrom<ProductPictureUrlResolver>());
            CreateMap<Core.Entities.Identity.Address, AddressDto>().ReverseMap();
            CreateMap<AddressDto, Core.Entities.OrderAggregation.Address>();
           CreateMap<CustomerBasketDto, CustomerBasket>().ReverseMap();
            CreateMap<BasketItemDto, BasketItem>().ReverseMap();
            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d=>d.DeliveryMethod,o=>o.MapFrom(s=>s.DeliveryMethod.ShortName))
                .ForMember(d => d.DeliveryMethodCost, o => o.MapFrom(s => s.DeliveryMethod.Cost));
            CreateMap<OrderItem, OrderItemDto>().
                ForMember(p => p.ProductId, o => o.MapFrom(s => s.Product.ProductId)).
                ForMember(p => p.ProductName, o => o.MapFrom(s => s.Product.ProductName)).
                ForMember(p => p.PictureUrl, o => o.MapFrom(s => s.Product.PictureUrl)).
                 ForMember(p => p.PictureUrl, o => o.MapFrom<OrderItemPictureUrlResolver>());
        //   CreateMap<CustomerBasket, CustomerBasketDto>();
            



        }
    }
}
