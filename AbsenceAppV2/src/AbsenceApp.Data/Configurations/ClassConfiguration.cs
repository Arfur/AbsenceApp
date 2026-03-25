using AbsenceApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AbsenceApp.Data.Configurations;

public class ClassConfiguration : IEntityTypeConfiguration<Class>
{
    public void Configure(EntityTypeBuilder<Class> builder)
    {
        builder.Property(c => c.Name).IsRequired();
        builder.Property(c => c.Code).IsRequired();
    }
}
