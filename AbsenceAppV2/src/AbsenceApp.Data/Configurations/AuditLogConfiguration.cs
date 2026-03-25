using AbsenceApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AbsenceApp.Data.Configurations;

// Legacy stub — AuditLog entity is retained only for backward compatibility.
// Active audit trail is split across dedicated audit entities.
public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.HasKey(a => a.AuditId);
        // UserId is int but User.Id is long — ignore the navigation to avoid
        // FK type mismatch and identity-tracking conflicts in EF Core.
        builder.Ignore(a => a.User);
    }
}
