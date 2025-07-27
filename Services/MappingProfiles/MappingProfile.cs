using AutoMapper;
using Domain.Models;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            // Customer Mappings
            CreateMap<Customer, CustomerDto>().ReverseMap();

            // Invoice Mappings
            CreateMap<Invoice, InvoiceDto>()
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount));

            // Product Mappings
            CreateMap<Product, ProductDto>().ReverseMap();

            // Order Mappings
            CreateMap<OrderRequestDto, Order>();
            CreateMap<OrderItemDetailDto, OrderItem>();
            CreateMap<OrderItem, OrderItemDetailDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name));
            CreateMap<Order, OrderResponseDto>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderItems));

            // User Mappings (optional, based on needs)
            CreateMap<RegisterUserDto, User>();
        }
    }
}
