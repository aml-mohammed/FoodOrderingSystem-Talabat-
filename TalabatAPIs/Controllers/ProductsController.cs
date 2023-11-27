using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTO;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;

namespace Talabat.APIs.Controllers
{
   // [Route("~/GetAllProducts")]
     [Route("api/[controller]")]
    //[Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductsController : APIBaseController
    {
        
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
      

        public ProductsController(IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            
        }
        [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [Authorize/*(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)*/]
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetAllProducts([FromQuery]ProductSpecParams Params)
        {
            var Spec = new ProductWithBrandAndTypeSpecifications(Params);
            var products = await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(Spec);
            var mappedProduct = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);
            var countspec = new ProdcutWithFilterationForCountAsync(Params);
            var count =await _unitOfWork.Repository<Product>().GetCountWithSpecAsync(countspec);
            var returnedobject = new Pagination<ProductToReturnDto>()
            {
                PageIndex = Params.PageIndex,
                PageSize = Params.PageSize,
                Data = mappedProduct,
                Count=count
               
            };
            return Ok(returnedobject);
            //return Ok(new Pagination<ProductToReturnDto>(Params.PageIndex,Params.PageSize,mappedProduct,count
            //    ));
        }
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductToReturnDto),200)]
        [ProducesResponseType(typeof(ApiResponse),404)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductWithBrandAndTypeSpecifications(id);
            var product = await _unitOfWork.Repository<Product>().GetEntityWithSpecAsync(spec);
            if (product is null) return NotFound(new ApiResponse(404));
            var mappedProduct=_mapper.Map<Product, ProductToReturnDto>(product);
            return Ok(mappedProduct);
        }


        [HttpGet("Types")]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetTypesOfProduct()
        {
            var types =await _unitOfWork.Repository<ProductType>().GetAllAsync();
            return Ok(types);
        }

        [HttpGet("Brands")]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetBrandsOfProduct()
        {
            var brands = await _unitOfWork.Repository<ProductBrand>().GetAllAsync();
            return Ok(brands);
        }
    }
}
