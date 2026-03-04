namespace PruebaTecnica.Domain.Common;

public abstract class BaseEntity
{
    public Guid Id { get; private set; } = Guid.CreateVersion7();
    public string CreatedBy { get; set; } = "admin";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
