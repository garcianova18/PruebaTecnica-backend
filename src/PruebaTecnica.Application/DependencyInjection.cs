using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PruebaTecnica.Application.Common.Mappings;
using PruebaTecnica.Application.DTOs.Product;
using System.Reflection;

namespace PruebaTecnica.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        services.AddAutoMapper(typeof(MappingProfile));


        services.AddValidatorsFromAssemblyContaining<CreateProductRequest>();
        return services;
    }
}
