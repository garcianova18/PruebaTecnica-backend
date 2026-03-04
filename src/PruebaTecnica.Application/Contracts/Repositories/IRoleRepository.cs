using PruebaTecnica.Domain.Entities;

namespace PruebaTecnica.Application.Contracts.Repositories
{
    public interface IRoleRepository:IRepository<Role>
    {
        Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

    }
}
