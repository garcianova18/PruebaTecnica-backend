using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Application.Contracts.Services;
using PruebaTecnica.Infrastructure.Persistence;

namespace PruebaTecnica.Api.Extensions
{
    public static class WebApplicationExtensions
    {
        public static async Task SeedInitialDataAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateAsyncScope();
            var seedData = scope.ServiceProvider.GetRequiredService<ISeedDataService>();
            await seedData.SeedAsync();
        }

        public static void ApplyMigrations(this WebApplication app)
        {
            using var scope = app.Services.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.Migrate();
        }
    }
}
