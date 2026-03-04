using PruebaTecnica.Domain.Entities;

namespace PruebaTecnica.Application.Contracts.Services;

public interface IPasswordHasherService
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string passwordHash);
}

