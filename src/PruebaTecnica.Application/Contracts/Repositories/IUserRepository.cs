using PruebaTecnica.Domain.Entities;


namespace PruebaTecnica.Application.Contracts.Repositories
{
    public interface IUserRepository:IRepository<User>
    {
        Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default);
        Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);
        Task<bool> UserNameExistsAsync(string userName, CancellationToken cancellationToken = default);
        //Task<User?> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
        Task<User?> GetWithRolesAsync(Guid id, CancellationToken cancellationToken = default);
        Task<User?> GetByUserNameWithRolesAsync(string userName, CancellationToken cancellationToken = default);
    }
}
