using PruebaTecnica.Domain.Common;

namespace PruebaTecnica.Domain.Entities;

public class User : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public Guid RolId { get; set; }
    public Role Role { get; set; } = new();
}
