using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.DTO;
using Talabat.APIs.Errors;
using Talabat.Core;
using Talabat.Core.Entities.OrderAggregation;
using Talabat.Core.Services;
using Talabat.Service;

namespace Talabat.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : APIBaseController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWOrk;

        public OrdersController(IOrderService orderService, IMapper mapper, IUnitOfWork unitOfWOrk)
        {
            _orderService = orderService;
            _mapper = mapper;
            _unitOfWOrk = unitOfWOrk;
        }
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderdto)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var mappedAddress = _mapper.Map<AddressDto, Address>(orderdto.ShippingAddress);
            var order = await _orderService.CreateOrderAsync(buyerEmail, orderdto.BasketId, orderdto.DeliveryMethodId, mappedAddress);
            if (order is null)
                return BadRequest(new ApiResponse(400));
            return order;
        }
        [ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var orders = await _orderService.GetOrdersForSpecificUser(buyerEmail);
            if (orders is null) return NotFound(new ApiResponse(404));
            var MappedOrders = _mapper.Map< IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(orders);
            return Ok(MappedOrders);
        }
        [ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderByIdForUser(int id)
        {
            var buyeremail = User.FindFirstValue(ClaimTypes.Email);
            var order = await _orderService.GetOrderByIdForSpecificUserAsync(buyeremail, id);
            if (order is null) return NotFound(new ApiResponse(404));
            var mappedOrder = _mapper.Map<Order,OrderToReturnDto>(order);
            return Ok(mappedOrder);

        }


        [HttpGet("DeliverMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            var result =await _unitOfWOrk.Repository<DeliveryMethod>().GetAllAsync();
            return Ok(result);
        }

    }
}
