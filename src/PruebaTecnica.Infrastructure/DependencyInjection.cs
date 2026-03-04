using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PruebaTecnica.Application.Contracts.Repositories;
using PruebaTecnica.Application.Contracts.Services;
using PruebaTecnica.Infrastructure.External;
using PruebaTecnica.Infrastructure.External.Geo;
using PruebaTecnica.Infrastructure.External.Weather;
using PruebaTecnica.Infrastructure.Persistence;
using PruebaTecnica.Infrastructure.Persistence.Repositories;
using PruebaTecnica.Infrastructure.Security;
using System.Text;
using IJwtService = PruebaTecnica.Application.Contracts.Services.IJwtService;

namespace PruebaTecnica.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Database
        services.AddDbContext<ApplicationDbContext>(opts =>
            opts.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Repositories & UoW
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Security
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        services.AddScoped<IPasswordHasherService, PasswordHasherService>();
        services.AddScoped<IJwtService, JwtService>();

        // JWT Authentication
        var jwtSettings = configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()!;

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opts =>
            {
                opts.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
                };
            });

        // External HTTP Clients - BaseAddress from settings
        services.Configure<WeatherApiSettings>(configuration.GetSection(WeatherApiSettings.SectionName));
        services.Configure<GeocodingApiSettings>(configuration.GetSection(GeocodingApiSettings.SectionName));

        services.AddHttpClient<IGeoClient, GeoClient>((sp, client) =>
        {
            var settings = sp.GetRequiredService<IOptions<GeocodingApiSettings>>().Value;
            client.BaseAddress = new Uri(settings.BaseAddress.TrimEnd('/') + "/");
        });

        services.AddHttpClient<IWeatherClient, WeatherClient>((sp, client) =>
        {
            var settings = sp.GetRequiredService<IOptions<WeatherApiSettings>>().Value;
            client.BaseAddress = new Uri(settings.BaseAddress.TrimEnd('/') + "/");
        });

        return services;
    }
}
