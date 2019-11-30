using AutoMapper;
using Billing.API.Models;
using Billing.API.Models.ViewModels;

namespace Billing.API.Mappings
{
    public class BIllingProfile : Profile
    {
        public BIllingProfile()
        {
            CreateMap<Purchase, PurchaseViewModel>()
                   .ForMember(dest => dest.SKU, opt => opt.MapFrom(src => src.Product.SKU))
                   .ForMember(dest => dest.OriginalPrice, opt => opt.MapFrom(src => src.Product.Price))
                   .ForMember(dest => dest.Discount, opt => opt.MapFrom(src => src.Discount))
                   .ForMember(dest => dest.FinalPrice, opt => opt.MapFrom(src => src.FinalPrice));

            CreateMap<Product, ProductViewModel>();
        }
    }
}
