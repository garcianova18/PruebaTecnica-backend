using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Application.Contracts.Repositories;
using PruebaTecnica.Domain.Entities;

namespace PruebaTecnica.Infrastructure.Persistence.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {

        public ApplicationDbContext context { get; }

        public UserRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
        public async Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default)
        {

            return await context.Users.FirstOrDefaultAsync(u => u.UserName.ToLower() == userName.ToLower(), cancellationToken);
        }

        public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
        {
            return await context.Users.AnyAsync(u => u.Email == email, cancellationToken);
        }
        public async Task<bool> UserNameExistsAsync(string userName, CancellationToken cancellationToken = default)
        {
            return await context.Users
                .AnyAsync(u => u.UserName.ToLower() == userName.ToLower(), cancellationToken);
        }

        //public async Task<User?> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
        //{
        //    return await context.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken, cancellationToken);
        //}

        public async Task<User?> GetWithRolesAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        }

        public async Task<User?> GetByUserNameWithRolesAsync(string userName, CancellationToken cancellationToken = default)
        {
            return await context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserName.ToLower() == userName.ToLower(), cancellationToken);
        }

    }
}
