using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Application.Contracts.Repositories;
using PruebaTecnica.Domain.Entities;

namespace PruebaTecnica.Infrastructure.Persistence.Repositories
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        private readonly ApplicationDbContext context;

        public RoleRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
        public async Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await context.Roles.FirstOrDefaultAsync(r => r.Name == name, cancellationToken);
        }

    }
}

