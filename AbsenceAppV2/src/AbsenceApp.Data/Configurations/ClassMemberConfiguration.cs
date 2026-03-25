using AbsenceApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AbsenceApp.Data.Configurations;

// Legacy stub — ClassMember entity is retained only for backward compatibility.
public class ClassMemberConfiguration : IEntityTypeConfiguration<ClassMember>
{
    public void Configure(EntityTypeBuilder<ClassMember> builder)
    {
        builder.HasKey(cm => new { cm.ClassId, cm.UserId });
    }
}
