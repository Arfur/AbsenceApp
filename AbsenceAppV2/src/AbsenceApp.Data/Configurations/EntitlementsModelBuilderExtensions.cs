/*
===============================================================================
 File        : EntitlementsModelBuilderExtensions.cs
 Namespace   : AbsenceApp.Data.Configurations
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-04-05
 Updated     : 2026-04-05
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
-------------------------------------------------------------------------------
 Notes       :
   - Table names are snake_case to match the Phase 2 schema conventions.
   - The Feature.Key column has a unique index for fast entitlement lookup.
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
    // applies table mappings, keys, indexes, and FK constraints.
    // ---------------------------------------------------------------------------

    public static ModelBuilder ConfigureEntitlements(this ModelBuilder modelBuilder)
    {
        // ── Feature ──────────────────────────────────────────────────────────
        modelBuilder.Entity<Feature>(b =>
        {
            b.ToTable("features");
            b.HasKey(e => e.FeatureId);
            b.Property(e => e.FeatureId).ValueGeneratedOnAdd();
            b.Property(e => e.Key).IsRequired().HasMaxLength(200);
            b.Property(e => e.IsActive).HasDefaultValue(true);
            b.HasIndex(e => e.Key).IsUnique();
        });

        // ── RoleFeature ───────────────────────────────────────────────────────
        modelBuilder.Entity<RoleFeature>(b =>
        {
            b.ToTable("role_features");
            b.HasKey(e => e.RoleFeatureId);
            b.Property(e => e.RoleFeatureId).ValueGeneratedOnAdd();
            b.Property(e => e.IsAllowed).HasDefaultValue(true);
            b.HasOne(e => e.Feature)
                .WithMany()
                .HasForeignKey(e => e.FeatureId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ── UserFeatureOverride ───────────────────────────────────────────────
        modelBuilder.Entity<UserFeatureOverride>(b =>
        {
            b.ToTable("user_feature_overrides");
            b.HasKey(e => e.UserFeatureOverrideId);
            b.Property(e => e.UserFeatureOverrideId).ValueGeneratedOnAdd();
            b.HasOne(e => e.Feature)
                .WithMany()
                .HasForeignKey(e => e.FeatureId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        return modelBuilder;
    }
}
