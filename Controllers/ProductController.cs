using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace NWI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet()]
        public async Task<ActionResult<ServiceResponse<List<GetProductResponseDto>>>> GetProducts() {
            return Ok(await _productService.GetAllProducts());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetProductResponseDto>>> Get(int id) {
            return Ok(await _productService.GetProductById(id));
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<GetProductResponseDto>>>> AddProduct(AddProductRequestDto product) {
            return Ok(await _productService.AddProduct(product));
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<List<GetProductResponseDto>>>> UpdateProduct(UpdateProductRequestDto updateProduct) {
            var response = await _productService.UpdateProduct(updateProduct);
            if(response.Data is null)
                return NotFound(response);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<GetProductResponseDto>>> DeleteProduct(int id) {
            var response = await _productService.DeleteProduct(id);
            if(response.Data is null)
                return NotFound(response);
            return Ok(response);
        }
    }
}