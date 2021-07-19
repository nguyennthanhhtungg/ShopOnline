using AutoMapper;
using ShopOnlineAPI.Models;
using ShopOnlineAPI.Ultilities;
using ShopOnlineAPI.ViewModels;

namespace TaskAssignment.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductViewModel, Product>()
                 .ForMember(dest => dest.ProductNameNoSign, source => source.MapFrom(source => ConvertToUnSign.Convert(source.ProductName)));

            CreateMap<Product, ProductViewModel>();

            CreateMap<CategoryViewModel, Category>().ReverseMap();
            CreateMap<SupplierViewModel, Supplier>().ReverseMap();
            CreateMap<CustomerViewModel, Customer>().ReverseMap();
            CreateMap<EmployeeViewModel, Employee>().ReverseMap();
            CreateMap<OrderDetailViewModel, OrderDetail>().ReverseMap();

            CreateMap<OrderViewModel, Order>()
                .ForMember(dest => dest.OrderDetails, source => source.MapFrom(source => source.OrderId == 0 ? null : source.OrderDetails));
            CreateMap<Order, OrderViewModel>();
        }
    }
}
