using PruebaTecnica.Domain.Common;

namespace PruebaTecnica.Domain.Entities;

public class Role : BaseEntity
{
    public string Name { get; set; } = null!;
    public ICollection<User> Users { get; set; } = [];
}
