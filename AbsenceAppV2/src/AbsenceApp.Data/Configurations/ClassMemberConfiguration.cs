using AbsenceApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AbsenceApp.Data.Configurations;

// Legacy stub — ClassMembers entity is retained only for backward compatibility.
public class ClassMemberConfiguration : IEntityTypeConfiguration<ClassMembers>
{
    public void Configure(EntityTypeBuilder<ClassMembers> builder)
    {
        builder.HasKey(cm => new { cm.ClassId, cm.UserId });
    }
}
