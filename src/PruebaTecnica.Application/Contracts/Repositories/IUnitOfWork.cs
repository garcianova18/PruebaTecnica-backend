namespace PruebaTecnica.Application.Contracts.Repositories;

public interface IUnitOfWork
{
    IProductRepository Products { get; }
    IUserRepository Users { get; }
    IRoleRepository Roles { get; }
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
