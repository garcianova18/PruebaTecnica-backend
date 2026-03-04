

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PruebaTecnica.Domain.Entities;

namespace PruebaTecnica.Infrastructure.Persistence.Configurations
{
    public class RoleEntityConfigurations : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.HasIndex(x => x.Name).IsUnique();

            builder.HasMany(x => x.Users)
                   .WithOne(u => u.Role)
                   .HasForeignKey(u => u.RolId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
