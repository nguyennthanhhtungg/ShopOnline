using AutoMapper;
using Microsoft.AspNetCore.Http;
using ShopOnlineAPI.Models;
using ShopOnlineAPI.Ultilities;
using ShopOnlineAPI.ViewModels;
using System.IO;

namespace TaskAssignment.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductViewModel, Product>()
                 .ForMember(dest => dest.ProductNameNoSign, source => source.MapFrom(source => ConvertToUnSign.Convert(source.ProductName)))
                 .ForMember(dest => dest.ImageName, source => source.MapFrom(source => GenerateNewImageFileName.Generate(source.Image)))
                 .ForMember(dest => dest.ImageType, source => source.MapFrom(source => source.Image.ContentType))
                 .ForMember(dest => dest.ImageData, source => source.MapFrom(source => ConvertIFormFileToByteArray.Convert(source.Image)));

            CreateMap<Product, ProductViewModel>()
                .ForMember(dest => dest.Image, source => source.MapFrom(source => new FormFile(new MemoryStream(source.ImageData), 0, source.ImageData.Length, "Image", source.ImageName) { Headers = new HeaderDictionary(), ContentType = source.ImageType }));;         

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
