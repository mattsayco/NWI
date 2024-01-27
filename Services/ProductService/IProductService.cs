using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NWI.Services.ProductService
{
    public interface IProductService
    {
        Task<ServiceResponse<List<GetProductResponseDto>>> GetAllProducts();
        Task<ServiceResponse<GetProductResponseDto>> GetProductById(int id);
        Task<ServiceResponse<List<GetProductResponseDto>>> AddProduct(AddProductRequestDto product);
        Task<ServiceResponse<string>> UpdateProduct(UpdateProductRequestDto product);
        Task<ServiceResponse<string>> DeleteProduct(int id);

    }
}