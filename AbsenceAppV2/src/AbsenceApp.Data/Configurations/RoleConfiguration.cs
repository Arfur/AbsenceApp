using AbsenceApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AbsenceApp.Data.Configurations;

// Legacy stub — Role entity is retained only for backward compatibility.
public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(r => r.RoleId);
        builder.Property(r => r.RoleName).IsRequired();
    }
}
