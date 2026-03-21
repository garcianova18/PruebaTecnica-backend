using AutoMapper;
using PruebaTecnica.Application.Common.Models;
using PruebaTecnica.Application.DTOs.Auth;
using PruebaTecnica.Application.DTOs.Product;
using PruebaTecnica.Domain.Entities;

namespace PruebaTecnica.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Product
        CreateMap<PageResults<Product>, PageResults<ProductResponse>>().ReverseMap();
        CreateMap<Product, UpdateProductRequest>().ReverseMap();
        CreateMap<Product, CreateProductRequest>().ReverseMap();
        CreateMap<Product, ProductResponse>().ReverseMap();

        // User
        CreateMap<User, RegisterRequest>().ReverseMap();
            
    }
}
