
using Microsoft.Extensions.Logging;
using PruebaTecnica.Application.Contracts.Repositories;
using PruebaTecnica.Application.Contracts.Services;
using PruebaTecnica.Domain.Entities;

namespace PruebaTecnica.Infrastructure.Persistence;

public  class SeedDataService : ISeedDataService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly ILogger logger;
    private readonly IPasswordHasherService passwordHasher;

    public SeedDataService(IUnitOfWork unitOfWork, ILogger<SeedDataService> logger, IPasswordHasherService passwordHasher)
    {
        this.unitOfWork = unitOfWork;
        this.logger = logger;
        this.passwordHasher = passwordHasher;
    }
    public async Task SeedAsync()
    {

        try
        {
            var roleAdmin = await unitOfWork.Roles.GetByNameAsync("admin");

            if (roleAdmin is null)
            {
                roleAdmin = new Role { Name = "admin" };

                await unitOfWork.Roles.AddAsync(roleAdmin);
                await unitOfWork.SaveChangesAsync();
                logger.LogInformation("Roles seeded.");
            }

            var userAdmin = await unitOfWork.Users.GetByUserNameAsync("admin");

            if (userAdmin is null)
            {
                var admin = new User
                {
                    Email = "admin@pruebatecnica.com",
                    UserName = "admin",
                    PasswordHash = passwordHasher.HashPassword("admin"),
                    FirstName = "System",
                    LastName = "Admin",
                    RolId = roleAdmin.Id
                };
                await unitOfWork.Users.AddAsync(admin);
                await unitOfWork.SaveChangesAsync();
                logger.LogInformation("Admin user seeded.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
        }
    }
}
