using AutoMapper;
using Core.DTOs;
using Core.Entities;

namespace NetCoreUrunSitesi.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Category, CategorySelectDto>().ReverseMap();
            CreateMap<Product, ProductListViewDto>().ReverseMap();
        }
    }
}
