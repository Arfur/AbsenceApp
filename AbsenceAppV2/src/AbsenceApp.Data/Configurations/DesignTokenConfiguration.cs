/*
===============================================================================
 File        : DesignTokenConfiguration.cs
 Namespace   : AbsenceApp.Data.Configurations
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-06-03
 Updated     : 2026-06-03
-------------------------------------------------------------------------------
 Purpose     : Pure EF Core configuration for the DesignToken entity. Ensures
               correct column mappings, constraints, and indexes for the
               DesignTokens table.

               Phase 4.1 aligns the EF model with the updated domain model:
               - Promotes ComponentGroup → Component (domain-level)
               - Adds Family, Variant, GroupName as first-class fields
               - Retains DB compatibility by mapping Component → "ComponentGroup"
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-06-03  Initial creation. Extracted EF configuration from the
                        legacy seeding file and converted to a clean, modern
                        Phase 4.1 configuration with no seed data.
-------------------------------------------------------------------------------
 Notes       :
   - Physical DB column names remain unchanged for compatibility.
   - Unique index: (Component, TokenKey) — enforced at DB and EF level.
   - This file contains *only* EF configuration. All token data now lives in
     the DesignTokens database table.
===============================================================================
*/

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AbsenceApp.Data.Models;

namespace AbsenceApp.Data.Configurations;

public class DesignTokenConfiguration : IEntityTypeConfiguration<DesignToken>
{
    public void Configure(EntityTypeBuilder<DesignToken> entity)
    {
        entity.ToTable("DesignTokens");

        entity.HasKey(e => e.Id);

        // Component (domain) → ComponentGroup (DB)
        entity.Property(e => e.Component)
              .HasColumnName("ComponentGroup")
              .HasMaxLength(100)
              .IsRequired();

        entity.Property(e => e.TokenKey)
              .HasMaxLength(200)
              .IsRequired();

        entity.Property(e => e.CssVariable)
              .HasMaxLength(200)
              .IsRequired();

        entity.Property(e => e.DefaultValue)
              .HasMaxLength(500)
              .IsRequired();

        entity.Property(e => e.CurrentValue)
              .HasMaxLength(500);

        entity.Property(e => e.Category)
              .HasMaxLength(100)
              .IsRequired();

        entity.Property(e => e.Description)
              .HasMaxLength(500);

        entity.Property(e => e.SortOrder)
              .IsRequired();

        // Phase 4 fields
        entity.Property(e => e.Family)
              .HasMaxLength(100)
              .IsRequired();

        entity.Property(e => e.Variant)
              .HasMaxLength(100)
              .IsRequired();

        entity.Property(e => e.GroupName)
              .HasMaxLength(100)
              .IsRequired();

        // Unique index
        entity.HasIndex(e => new { e.Component, e.TokenKey })
              .IsUnique()
              .HasDatabaseName("UX_DesignTokens_Component_TokenKey");
    }
}
