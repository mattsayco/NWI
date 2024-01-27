using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NWI.Services.ProductService
{
    public class ProductService : IProductService
    {
        private static List<GetProductResponseDto> products = new();
        public readonly IMapper _mapper;
        private readonly DataContext _context;

        public ProductService(IMapper mapper, DataContext context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<GetProductResponseDto>>> AddProduct(AddProductRequestDto addProduct)
        {
            var serviceResponse = new ServiceResponse<List<GetProductResponseDto>>();
            try
            {
                var test = await _context.Products.AddAsync(_mapper.Map<Product>(addProduct));
                await _context.SaveChangesAsync();
                var dbProducts = await _context.Products.ToListAsync();
                serviceResponse.Data = dbProducts.Select(_mapper.Map<GetProductResponseDto>).ToList();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetProductResponseDto>>> GetAllProducts()
        {

            var serviceResponse = new ServiceResponse<List<GetProductResponseDto>>();
            try
            {
                var dbProducts = await _context.Products.ToListAsync();
                if (dbProducts is null)
                    throw new Exception($"No Products are found.");
                serviceResponse.Data = dbProducts.Select(_mapper.Map<GetProductResponseDto>).ToList();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetProductResponseDto>> GetProductById(int id)
        {
            var serviceResponse = new ServiceResponse<GetProductResponseDto>();
            try
            {
                var dbProduct = await _context.Products.FirstOrDefaultAsync(product => product.Id == id);
                if (dbProduct is null)
                    throw new Exception($"Product with Id '{id}' not found.");
                serviceResponse.Data = _mapper.Map<GetProductResponseDto>(dbProduct);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<string>> UpdateProduct(UpdateProductRequestDto updateProduct)
        {
            var serviceResponse = new ServiceResponse<string>();
            try
            {
                var dbProduct = await _context.Products.FirstOrDefaultAsync(product => product.Id == updateProduct.Id);
                if (dbProduct is null)
                    throw new Exception($"Product with Id '{updateProduct.Id}' not found.");
                await _context.Products
                    .Where(product => product.Id == updateProduct.Id)
                    .ExecuteUpdateAsync(setters => setters
                    .SetProperty(product => product.ProductCode, updateProduct.ProductCode)
                    .SetProperty(product => product.ProductName, updateProduct.ProductName)
                    .SetProperty(product => product.UnitPrice, updateProduct.UnitPrice)
                    .SetProperty(product => product.UpdatedBy, "Admin")
                    .SetProperty(product => product.DateUpdated, DateTime.Now));
                await _context.SaveChangesAsync();
                serviceResponse.Data = "";
                serviceResponse.Message = $"Successfully updated product {updateProduct.ProductName}";
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<string>> DeleteProduct(int id)
        {
            var serviceResponse = new ServiceResponse<string>();
            try
            {
                var dbProduct = await _context.Products.FirstOrDefaultAsync(product => product.Id == id);
                if (dbProduct is null)
                    throw new Exception($"Product with Id '{id}' not found.");
                await _context.Products.Where(product => product.Id == id).ExecuteDeleteAsync();
                await _context.SaveChangesAsync();
                serviceResponse.Data = "";
                serviceResponse.Message = $"Successfully deleted product {dbProduct.ProductName}";
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }
    }
}