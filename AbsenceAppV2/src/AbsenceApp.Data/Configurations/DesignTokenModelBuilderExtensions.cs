/*
===============================================================================
 File        : DesignTokenModelBuilderExtensions.cs
 Namespace   : AbsenceApp.Data.Configurations
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-05-12
 Updated     : 2026-05-12
-------------------------------------------------------------------------------
 Purpose     : ModelBuilder extension that configures the DesignToken entity
               and seeds the canonical set of button design tokens.
               Called from AppDbContext.OnModelCreating via
               modelBuilder.ConfigureDesignTokens().

               All 28 seed rows cover:
                 - 6 colour variant groups (primary, secondary, success,
                   danger, warning, info) × 4 tokens each = 24 colour tokens
                 - 4 structural tokens (border-radius, font-size,
                   padding-y, padding-x)
               IDs follow the spacing convention 10–13, 20–23, … 73.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-05-12  Initial creation (Phase A — Design Token System).
-------------------------------------------------------------------------------
 Notes       :
   - DefaultValue strings exactly match the values used in global-config.css
     so that the runtime CSS output is visually identical to the static file
     before any admin overrides are applied.
   - SeedDate is a compile-time constant required by HasData().
   - The unique index on (ComponentGroup, TokenKey) mirrors the DB constraint
     that prevents duplicate token addresses.
===============================================================================
*/

using System;
using AbsenceApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AbsenceApp.Data.Configurations;

public static class DesignTokenModelBuilderExtensions
{
    // -------------------------------------------------------------------------
    // Fixed seed timestamp — must be a compile-time constant for HasData().
    // -------------------------------------------------------------------------
    private static readonly DateTime SeedDate =
        new(2026, 5, 12, 0, 0, 0, DateTimeKind.Utc);

    // =========================================================================
    // ConfigureDesignTokens
    // =========================================================================

    public static ModelBuilder ConfigureDesignTokens(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DesignToken>(b =>
        {
            b.ToTable("DesignTokens");
            b.HasKey(e => e.Id);

            b.Property(e => e.Id).ValueGeneratedOnAdd();
            b.Property(e => e.ComponentGroup).IsRequired().HasMaxLength(100);
            b.Property(e => e.TokenKey).IsRequired().HasMaxLength(200);
            b.Property(e => e.CssVariable).IsRequired().HasMaxLength(200);
            b.Property(e => e.DefaultValue).IsRequired().HasMaxLength(500);
            b.Property(e => e.CurrentValue).HasMaxLength(500);
            b.Property(e => e.Category).IsRequired().HasMaxLength(100);
            b.Property(e => e.Description).HasMaxLength(500);
            b.Property(e => e.IsActive).HasDefaultValue(true);

            // Unique composite index: one token address per component group
            b.HasIndex(e => new { e.ComponentGroup, e.TokenKey })
             .IsUnique()
             .HasDatabaseName("IX_DesignTokens_ComponentGroup_TokenKey");

            // -----------------------------------------------------------------
            // Seed data — 28 button tokens matching global-config.css values
            // -----------------------------------------------------------------
            b.HasData(

                // ── Primary (IDs 10–13) ──────────────────────────────────────
                new DesignToken
                {
                    Id             = 10,
                    ComponentGroup = "btn",
                    TokenKey       = "primary-bg",
                    CssVariable    = "--ds-btn-primary-bg",
                    DefaultValue   = "#0d6efd",
                    Category       = "color",
                    Description    = "Primary button background colour",
                    IsActive       = true,
                    SortOrder      = 10,
                    CreatedAt      = SeedDate,
                    UpdatedAt      = SeedDate,
                },
                new DesignToken
                {
                    Id             = 11,
                    ComponentGroup = "btn",
                    TokenKey       = "primary-text",
                    CssVariable    = "--ds-btn-primary-text",
                    DefaultValue   = "#ffffff",
                    Category       = "color",
                    Description    = "Primary button text colour",
                    IsActive       = true,
                    SortOrder      = 11,
                    CreatedAt      = SeedDate,
                    UpdatedAt      = SeedDate,
                },
                new DesignToken
                {
                    Id             = 12,
                    ComponentGroup = "btn",
                    TokenKey       = "primary-border",
                    CssVariable    = "--ds-btn-primary-border",
                    DefaultValue   = "#0d6efd",
                    Category       = "color",
                    Description    = "Primary button border colour",
                    IsActive       = true,
                    SortOrder      = 12,
                    CreatedAt      = SeedDate,
                    UpdatedAt      = SeedDate,
                },
                new DesignToken
                {
                    Id             = 13,
                    ComponentGroup = "btn",
                    TokenKey       = "primary-hover-bg",
                    CssVariable    = "--ds-btn-primary-hover-bg",
                    DefaultValue   = "#0b5ed7",
                    Category       = "color",
                    Description    = "Primary button hover background colour",
                    IsActive       = true,
                    SortOrder      = 13,
                    CreatedAt      = SeedDate,
                    UpdatedAt      = SeedDate,
                },

                // ── Secondary (IDs 20–23) ────────────────────────────────────
                new DesignToken
                {
                    Id             = 20,
                    ComponentGroup = "btn",
                    TokenKey       = "secondary-bg",
                    CssVariable    = "--ds-btn-secondary-bg",
                    DefaultValue   = "transparent",
                    Category       = "color",
                    Description    = "Secondary button background colour",
                    IsActive       = true,
                    SortOrder      = 20,
                    CreatedAt      = SeedDate,
                    UpdatedAt      = SeedDate,
                },
                new DesignToken
                {
                    Id             = 21,
                    ComponentGroup = "btn",
                    TokenKey       = "secondary-text",
                    CssVariable    = "--ds-btn-secondary-text",
                    DefaultValue   = "#212529",
                    Category       = "color",
                    Description    = "Secondary button text colour",
                    IsActive       = true,
                    SortOrder      = 21,
                    CreatedAt      = SeedDate,
                    UpdatedAt      = SeedDate,
                },
                new DesignToken
                {
                    Id             = 22,
                    ComponentGroup = "btn",
                    TokenKey       = "secondary-border",
                    CssVariable    = "--ds-btn-secondary-border",
                    DefaultValue   = "#dee2e6",
                    Category       = "color",
                    Description    = "Secondary button border colour",
                    IsActive       = true,
                    SortOrder      = 22,
                    CreatedAt      = SeedDate,
                    UpdatedAt      = SeedDate,
                },
                new DesignToken
                {
                    Id             = 23,
                    ComponentGroup = "btn",
                    TokenKey       = "secondary-hover-bg",
                    CssVariable    = "--ds-btn-secondary-hover-bg",
                    DefaultValue   = "#f8f9fa",
                    Category       = "color",
                    Description    = "Secondary button hover background colour",
                    IsActive       = true,
                    SortOrder      = 23,
                    CreatedAt      = SeedDate,
                    UpdatedAt      = SeedDate,
                },

                // ── Success (IDs 30–33) ──────────────────────────────────────
                new DesignToken
                {
                    Id             = 30,
                    ComponentGroup = "btn",
                    TokenKey       = "success-bg",
                    CssVariable    = "--ds-btn-success-bg",
                    DefaultValue   = "#198754",
                    Category       = "color",
                    Description    = "Success button background colour",
                    IsActive       = true,
                    SortOrder      = 30,
                    CreatedAt      = SeedDate,
                    UpdatedAt      = SeedDate,
                },
                new DesignToken
                {
                    Id             = 31,
                    ComponentGroup = "btn",
                    TokenKey       = "success-text",
                    CssVariable    = "--ds-btn-success-text",
                    DefaultValue   = "#ffffff",
                    Category       = "color",
                    Description    = "Success button text colour",
                    IsActive       = true,
                    SortOrder      = 31,
                    CreatedAt      = SeedDate,
                    UpdatedAt      = SeedDate,
                },
                new DesignToken
                {
                    Id             = 32,
                    ComponentGroup = "btn",
                    TokenKey       = "success-border",
                    CssVariable    = "--ds-btn-success-border",
                    DefaultValue   = "#198754",
                    Category       = "color",
                    Description    = "Success button border colour",
                    IsActive       = true,
                    SortOrder      = 32,
                    CreatedAt      = SeedDate,
                    UpdatedAt      = SeedDate,
                },
                new DesignToken
                {
                    Id             = 33,
                    ComponentGroup = "btn",
                    TokenKey       = "success-hover-bg",
                    CssVariable    = "--ds-btn-success-hover-bg",
                    DefaultValue   = "#157347",
                    Category       = "color",
                    Description    = "Success button hover background colour",
                    IsActive       = true,
                    SortOrder      = 33,
                    CreatedAt      = SeedDate,
                    UpdatedAt      = SeedDate,
                },

                // ── Danger (IDs 40–43) ───────────────────────────────────────
                new DesignToken
                {
                    Id             = 40,
                    ComponentGroup = "btn",
                    TokenKey       = "danger-bg",
                    CssVariable    = "--ds-btn-danger-bg",
                    DefaultValue   = "#dc3545",
                    Category       = "color",
                    Description    = "Danger button background colour",
                    IsActive       = true,
                    SortOrder      = 40,
                    CreatedAt      = SeedDate,
                    UpdatedAt      = SeedDate,
                },
                new DesignToken
                {
                    Id             = 41,
                    ComponentGroup = "btn",
                    TokenKey       = "danger-text",
                    CssVariable    = "--ds-btn-danger-text",
                    DefaultValue   = "#ffffff",
                    Category       = "color",
                    Description    = "Danger button text colour",
                    IsActive       = true,
                    SortOrder      = 41,
                    CreatedAt      = SeedDate,
                    UpdatedAt      = SeedDate,
                },
                new DesignToken
                {
                    Id             = 42,
                    ComponentGroup = "btn",
                    TokenKey       = "danger-border",
                    CssVariable    = "--ds-btn-danger-border",
                    DefaultValue   = "#dc3545",
                    Category       = "color",
                    Description    = "Danger button border colour",
                    IsActive       = true,
                    SortOrder      = 42,
                    CreatedAt      = SeedDate,
                    UpdatedAt      = SeedDate,
                },
                new DesignToken
                {
                    Id             = 43,
                    ComponentGroup = "btn",
                    TokenKey       = "danger-hover-bg",
                    CssVariable    = "--ds-btn-danger-hover-bg",
                    DefaultValue   = "#bb2d3b",
                    Category       = "color",
                    Description    = "Danger button hover background colour",
                    IsActive       = true,
                    SortOrder      = 43,
                    CreatedAt      = SeedDate,
                    UpdatedAt      = SeedDate,
                },

                // ── Warning (IDs 50–53) ──────────────────────────────────────
                new DesignToken
                {
                    Id             = 50,
                    ComponentGroup = "btn",
                    TokenKey       = "warning-bg",
                    CssVariable    = "--ds-btn-warning-bg",
                    DefaultValue   = "#ffc107",
                    Category       = "color",
                    Description    = "Warning button background colour",
                    IsActive       = true,
                    SortOrder      = 50,
                    CreatedAt      = SeedDate,
                    UpdatedAt      = SeedDate,
                },
                new DesignToken
                {
                    Id             = 51,
                    ComponentGroup = "btn",
                    TokenKey       = "warning-text",
                    CssVariable    = "--ds-btn-warning-text",
                    DefaultValue   = "#212529",
                    Category       = "color",
                    Description    = "Warning button text colour",
                    IsActive       = true,
                    SortOrder      = 51,
                    CreatedAt      = SeedDate,
                    UpdatedAt      = SeedDate,
                },
                new DesignToken
                {
                    Id             = 52,
                    ComponentGroup = "btn",
                    TokenKey       = "warning-border",
                    CssVariable    = "--ds-btn-warning-border",
                    DefaultValue   = "#ffc107",
                    Category       = "color",
                    Description    = "Warning button border colour",
                    IsActive       = true,
                    SortOrder      = 52,
                    CreatedAt      = SeedDate,
                    UpdatedAt      = SeedDate,
                },
                new DesignToken
                {
                    Id             = 53,
                    ComponentGroup = "btn",
                    TokenKey       = "warning-hover-bg",
                    CssVariable    = "--ds-btn-warning-hover-bg",
                    DefaultValue   = "#ffca2c",
                    Category       = "color",
                    Description    = "Warning button hover background colour",
                    IsActive       = true,
                    SortOrder      = 53,
                    CreatedAt      = SeedDate,
                    UpdatedAt      = SeedDate,
                },

                // ── Info (IDs 60–63) ─────────────────────────────────────────
                new DesignToken
                {
                    Id             = 60,
                    ComponentGroup = "btn",
                    TokenKey       = "info-bg",
                    CssVariable    = "--ds-btn-info-bg",
                    DefaultValue   = "#0dcaf0",
                    Category       = "color",
                    Description    = "Info button background colour",
                    IsActive       = true,
                    SortOrder      = 60,
                    CreatedAt      = SeedDate,
                    UpdatedAt      = SeedDate,
                },
                new DesignToken
                {
                    Id             = 61,
                    ComponentGroup = "btn",
                    TokenKey       = "info-text",
                    CssVariable    = "--ds-btn-info-text",
                    DefaultValue   = "#212529",
                    Category       = "color",
                    Description    = "Info button text colour",
                    IsActive       = true,
                    SortOrder      = 61,
                    CreatedAt      = SeedDate,
                    UpdatedAt      = SeedDate,
                },
                new DesignToken
                {
                    Id             = 62,
                    ComponentGroup = "btn",
                    TokenKey       = "info-border",
                    CssVariable    = "--ds-btn-info-border",
                    DefaultValue   = "#0dcaf0",
                    Category       = "color",
                    Description    = "Info button border colour",
                    IsActive       = true,
                    SortOrder      = 62,
                    CreatedAt      = SeedDate,
                    UpdatedAt      = SeedDate,
                },
                new DesignToken
                {
                    Id             = 63,
                    ComponentGroup = "btn",
                    TokenKey       = "info-hover-bg",
                    CssVariable    = "--ds-btn-info-hover-bg",
                    DefaultValue   = "#31d2f2",
                    Category       = "color",
                    Description    = "Info button hover background colour",
                    IsActive       = true,
                    SortOrder      = 63,
                    CreatedAt      = SeedDate,
                    UpdatedAt      = SeedDate,
                },

                // ── Structural (IDs 70–73) ───────────────────────────────────
                new DesignToken
                {
                    Id             = 70,
                    ComponentGroup = "btn",
                    TokenKey       = "border-radius",
                    CssVariable    = "--ds-btn-border-radius",
                    DefaultValue   = "6px",
                    Category       = "structure",
                    Description    = "Button border radius",
                    IsActive       = true,
                    SortOrder      = 70,
                    CreatedAt      = SeedDate,
                    UpdatedAt      = SeedDate,
                },
                new DesignToken
                {
                    Id             = 71,
                    ComponentGroup = "btn",
                    TokenKey       = "font-size",
                    CssVariable    = "--ds-btn-font-size",
                    DefaultValue   = "0.875rem",
                    Category       = "structure",
                    Description    = "Button font size",
                    IsActive       = true,
                    SortOrder      = 71,
                    CreatedAt      = SeedDate,
                    UpdatedAt      = SeedDate,
                },
                new DesignToken
                {
                    Id             = 72,
                    ComponentGroup = "btn",
                    TokenKey       = "padding-y",
                    CssVariable    = "--ds-btn-padding-y",
                    DefaultValue   = "7px",
                    Category       = "structure",
                    Description    = "Button vertical padding",
                    IsActive       = true,
                    SortOrder      = 72,
                    CreatedAt      = SeedDate,
                    UpdatedAt      = SeedDate,
                },
                new DesignToken
                {
                    Id             = 73,
                    ComponentGroup = "btn",
                    TokenKey       = "padding-x",
                    CssVariable    = "--ds-btn-padding-x",
                    DefaultValue   = "16px",
                    Category       = "structure",
                    Description    = "Button horizontal padding",
                    IsActive       = true,
                    SortOrder      = 73,
                    CreatedAt      = SeedDate,
                    UpdatedAt      = SeedDate,
                },

                // ── Card (IDs 100–105) ───────────────────────────────────────────────
                new DesignToken
                {
                    Id             = 100,
                    ComponentGroup = "card",
                    TokenKey       = "bg",
                    CssVariable    = "--ds-card-bg",
                    DefaultValue   = "#ffffff",
                    Category       = "color",
                    Description    = "Card background colour",
                    IsActive       = true,
                    SortOrder      = 100,
                    CreatedAt      = SeedDate,
                    UpdatedAt      = SeedDate,
                },
                new DesignToken
                {
                    Id             = 101,
                    ComponentGroup = "card",
                    TokenKey       = "border-color",
                    CssVariable    = "--ds-card-border-color",
                    DefaultValue   = "#dee2e6",
                    Category       = "color",
                    Description    = "Card border colour",
                    IsActive       = true,
                    SortOrder      = 101,
                    CreatedAt      = SeedDate,
                    UpdatedAt      = SeedDate,
                },
                new DesignToken
                {
                    Id             = 102,
                    ComponentGroup = "card",
                    TokenKey       = "radius",
                    CssVariable    = "--ds-card-radius",
                    DefaultValue   = "8px",
                    Category       = "structure",
                    Description    = "Card border radius",
                    IsActive       = true,
                    SortOrder      = 102,
                    CreatedAt      = SeedDate,
                    UpdatedAt      = SeedDate,
                },
                new DesignToken
                {
                    Id             = 103,
                    ComponentGroup = "card",
                    TokenKey       = "shadow",
                    CssVariable    = "--ds-card-shadow",
                    DefaultValue   = "0 1px 4px rgba(0,0,0,0.06)",
                    Category       = "structure",
                    Description    = "Card box shadow",
                    IsActive       = true,
                    SortOrder      = 103,
                    CreatedAt      = SeedDate,
                    UpdatedAt      = SeedDate,
                },
                new DesignToken
                {
                    Id             = 104,
                    ComponentGroup = "card",
                    TokenKey       = "header-bg",
                    CssVariable    = "--ds-card-header-bg",
                    DefaultValue   = "#f8f9fa",
                    Category       = "color",
                    Description    = "Card header background colour",
                    IsActive       = true,
                    SortOrder      = 104,
                    CreatedAt      = SeedDate,
                    UpdatedAt      = SeedDate,
                },
                new DesignToken
                {
                    Id             = 105,
                    ComponentGroup = "card",
                    TokenKey       = "padding",
                    CssVariable    = "--ds-card-padding",
                    DefaultValue   = "1.25rem",
                    Category       = "structure",
                    Description    = "Card body padding",
                    IsActive       = true,
                    SortOrder      = 105,
                    CreatedAt      = SeedDate,
                    UpdatedAt      = SeedDate,
                }
            );
        });

        return modelBuilder;
    }
}
