using AbsenceApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AbsenceApp.Data.Configurations;

public class ClassConfiguration : IEntityTypeConfiguration<TeachingGroup>
{
    public void Configure(EntityTypeBuilder<TeachingGroup> builder)
    {
        builder.Property(c => c.Name).IsRequired();
        builder.Property(c => c.Code).IsRequired();
    }
}
