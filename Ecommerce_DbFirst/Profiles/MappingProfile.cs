using AutoMapper;
using Ecommerce_DBFirst.Dtos;
using Ecommerce_DBFirst.Models;

namespace Ecommerce_DBFirst.Profiles
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductDto, Product>();
            CreateMap<Product, ProductDto>();
        }
    }
}