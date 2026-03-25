/*
===============================================================================
 File        : TablePageSettingConfiguration.cs
 Namespace   : AbsenceApp.Data.Configurations
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-17
 Updated     : 2026-03-17
-------------------------------------------------------------------------------
 Purpose     : EF Core Fluent API configuration for the TablePageSetting
               entity.  Overrides the global ValueGeneratedNever() convention
               so the Id column uses SQL Server IDENTITY auto-increment.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-17  Initial implementation.
===============================================================================
*/

using AbsenceApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AbsenceApp.Data.Configurations;

public class TablePageSettingConfiguration : IEntityTypeConfiguration<TablePageSetting>
{
    public void Configure(EntityTypeBuilder<TablePageSetting> builder)
    {
        builder.ToTable("table_page_settings");

        builder.HasKey(x => x.Id);

        // Explicitly enable IDENTITY — this is applied AFTER the global
        // ValueGeneratedNever() loop in AppDbContext.OnModelCreating, which
        // is why AppDbContext also excludes this entity type from that loop.
        builder.Property(x => x.Id)
               .UseIdentityColumn()
               .ValueGeneratedOnAdd();

        builder.Property(x => x.PageName)    .IsRequired().HasMaxLength(100);
        builder.Property(x => x.FieldName)   .IsRequired().HasMaxLength(100);
        builder.Property(x => x.DisplayLabel).IsRequired().HasMaxLength(200);
        builder.Property(x => x.IsVisible)   .IsRequired();
        builder.Property(x => x.IsSortable)  .IsRequired();
        builder.Property(x => x.IsFilterable).IsRequired();
        builder.Property(x => x.IsSearchable).IsRequired();
        builder.Property(x => x.DisplayOrder).IsRequired();

        // Composite unique index — one row per (page, field) pair
        builder.HasIndex(x => new { x.PageName, x.FieldName }).IsUnique();
    }
}
