using PruebaTecnica.Application.Contracts.Repositories;

namespace PruebaTecnica.Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork,IDisposable
{
    private IProductRepository? products;
    private IUserRepository? users;
    private IRoleRepository? roles;

    private readonly ApplicationDbContext context;

    public UnitOfWork(ApplicationDbContext context)
    {
        this.context = context;
    }

    public IProductRepository Products
    {
        get
        {
          return  products ??= new ProductRepository(context);
        }
    }
    public IUserRepository Users
    {
        get
        {
           return users ??= new UserRepository(context);
        }
    }
    public IRoleRepository Roles
    {
        get
        {
            return roles ??= new RoleRepository(context);
        }
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
       
    public void Dispose()
    {
        context.Dispose();
    }
}
