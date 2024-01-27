using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NWI
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Product, GetProductResponseDto>();
            CreateMap<GetProductResponseDto, Product>();
            CreateMap<AddProductRequestDto, Product>();
            CreateMap<AddProductRequestDto, GetProductResponseDto>();
            CreateMap<UpdateProductRequestDto, GetProductResponseDto>();
            CreateMap<UpdateProductRequestDto, Product>();
            CreateMap<Product, UpdateProductRequestDto>();

            CreateMap<User, UserRequestDto>();
        }
    }
}