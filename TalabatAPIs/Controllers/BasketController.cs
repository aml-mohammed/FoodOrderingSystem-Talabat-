using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTO;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;

namespace Talabat.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepository,IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }
        [HttpGet("{BasketId}")]
        public async Task<CustomerBasket>GetBasket(string BasketId)
        {
            var basket =await _basketRepository.GetBasketAsync(BasketId);
            return basket is null ? new CustomerBasket(BasketId) : basket;
            //if (basket is null) return new CustomerBasket(BasketId);
            //else 
            //    return Ok(basket);

        }
        [HttpPost]
        public async Task<ActionResult<CustomerBasket>>UpdateBasket(CustomerBasketDto basket)
        {
            var MappedBasket = _mapper.Map<CustomerBasketDto, CustomerBasket>(basket);
            var updatedOrCeatedBasket =await _basketRepository.UpdateBasketAsync(MappedBasket);
            if (updatedOrCeatedBasket is null)
                return BadRequest(new ApiResponse(400));
            return Ok(updatedOrCeatedBasket);

        }
        [HttpDelete]
        public async Task<ActionResult<bool>>DeleteBasket(string BasketId)
        {
           return await _basketRepository.DeleteBasketAsync(BasketId);
        }

    }
}
