/*
===============================================================================
 File        : EntitlementsModelBuilderExtensions.cs
 Namespace   : AbsenceApp.Data.Configurations
 Author      : Michael
 Version     : 2.0.0
 Created     : 2026-04-05
 Updated     : 2026-04-19
-------------------------------------------------------------------------------
 Purpose     : ModelBuilder extension that configures the three entitlement
               entities (Feature, RoleFeature, UserFeatureOverride) in the
               AppDbContext model. Called from AppDbContext.OnModelCreating
               via modelBuilder.ConfigureEntitlements().

               Kept in a dedicated extension to preserve modularity: all
               entitlement schema changes are isolated to this file.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-05  Initial implementation. Closes the build error
                         introduced when AppDbContext.cs called the then-missing
                         ConfigureEntitlements() extension.
   - 2.0.0  2026-04-19  Schema alignment: updated table names (feature,
                         rolefeature, userfeatureoverride), updated PK names
                         (Id), updated column names to match CSV schema.
                         Removed FK navigation from RoleFeature→Feature and
                         UserFeatureOverride→Feature (joins are by FeatureCode
                         string, not by FK integer).
-------------------------------------------------------------------------------
 Notes       :
   - Table names match the production MySQL schema from the CSV source of truth.
   - FeatureCode in rolefeature/userfeatureoverride is a string code matching
     feature.Code — there is no FK constraint enforced by EF.
===============================================================================
*/

using AbsenceApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AbsenceApp.Data.Configurations;

// ===========================================================================
// EntitlementsModelBuilderExtensions
// ===========================================================================

public static class EntitlementsModelBuilderExtensions
{
    // ---------------------------------------------------------------------------
    // ConfigureEntitlements
    // Registers Feature, RoleFeature, and UserFeatureOverride entities and
    // applies table mappings, keys, indexes, and column constraints.
    // ---------------------------------------------------------------------------

    public static ModelBuilder ConfigureEntitlements(this ModelBuilder modelBuilder)
    {
        // ── Feature ──────────────────────────────────────────────────────────
        modelBuilder.Entity<Feature>(b =>
        {
            b.ToTable("features");
            b.HasKey(e => e.Id);
            b.Property(e => e.Id).ValueGeneratedOnAdd();
            b.Property(e => e.Code).IsRequired().HasMaxLength(200);
            b.Property(e => e.IsEnabled).HasDefaultValue(true);
            b.HasIndex(e => e.Code).IsUnique();
        });

        // ── RoleFeature ───────────────────────────────────────────────────────
        modelBuilder.Entity<RoleFeature>(b =>
        {
            b.ToTable("rolefeatures");
            b.HasKey(e => e.Id);
            b.Property(e => e.Id).ValueGeneratedOnAdd();
            b.Property(e => e.FeatureCode).IsRequired().HasMaxLength(200);
            b.Property(e => e.IsEnabled).HasDefaultValue(true);
        });

        // ── UserFeatureOverride ───────────────────────────────────────────────
        modelBuilder.Entity<UserFeatureOverride>(b =>
        {
            b.ToTable("userfeatureoverrides");
            b.HasKey(e => e.Id);
            b.Property(e => e.Id).ValueGeneratedOnAdd();
            b.Property(e => e.FeatureCode).IsRequired().HasMaxLength(200);
        });

        return modelBuilder;
    }
}
