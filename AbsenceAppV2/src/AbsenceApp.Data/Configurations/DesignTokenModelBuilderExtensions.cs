/*
===============================================================================
 File        : DesignTokenModelBuilderExtensions.cs
 Namespace   : AbsenceApp.Data.Configurations
 Author      : Michael
 Version     : 2.1.0
 Created     : 2026-05-12
 Updated     : 2026-05-19
-------------------------------------------------------------------------------
 Purpose     : ModelBuilder extension that configures the DesignToken entity
               and seeds the canonical set of design tokens.
               Called from AppDbContext.OnModelCreating via
               modelBuilder.ConfigureDesignTokens().

               Phase A (v1.0.0): 28 button + 6 card tokens (IDs 10–105).
               Phase D (v2.0.0): 17 token families added (IDs 200–1010).
               Phase 1 (v2.1.0): 16 action-btn tokens added (IDs 1100–1115).
                 - 7 foundation families (text, surface, border, radius, shadow,
                   spacing, layout)
                 - 10 semantic component families (nav-header, nav-sidebar,
                   form-field, form-shell, table, alert, icon, icon-btn,
                   badge-status, chart)
               IDs follow the spacing convention 10–13, 20–23, … for btn;
               100–105 for card; 200+ for Phase D families.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-05-12  Initial creation (Phase A — Design Token System).
   - 2.0.0  2026-05-15  Phase D: added 17 token families (190 rows).
   - 2.1.0  2026-05-19  Phase 1: added action-btn component tokens (IDs 1100–1115).
     Families: text, surface, border, radius, shadow, spacing, layout,
     nav-header, nav-sidebar, form-field, form-shell, table, alert, icon,
     icon-btn, badge-status, chart. All DefaultValues set to "TBD" per
     audit requirements (no guessing). Family definitions and default
     values will be extracted and finalized in subsequent audit phase.
-------------------------------------------------------------------------------
 Notes       :
   - DefaultValue strings for Phase A (btn/card) match global-config.css;
     Phase D tokens use "TBD" placeholder per audit protocol.
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
                },

                // ── Text (IDs 200–213) ────────────────────────────────────────────────
                new DesignToken { Id = 200, ComponentGroup = "text", TokenKey = "size-xs", CssVariable = "--ds-text-size-xs", DefaultValue = "TBD", Category = "typography", Description = "Extra small text size", IsActive = true, SortOrder = 200, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 201, ComponentGroup = "text", TokenKey = "size-sm", CssVariable = "--ds-text-size-sm", DefaultValue = "TBD", Category = "typography", Description = "Small text size", IsActive = true, SortOrder = 201, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 202, ComponentGroup = "text", TokenKey = "size-md", CssVariable = "--ds-text-size-md", DefaultValue = "TBD", Category = "typography", Description = "Medium text size", IsActive = true, SortOrder = 202, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 203, ComponentGroup = "text", TokenKey = "size-lg", CssVariable = "--ds-text-size-lg", DefaultValue = "TBD", Category = "typography", Description = "Large text size", IsActive = true, SortOrder = 203, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 204, ComponentGroup = "text", TokenKey = "size-xl", CssVariable = "--ds-text-size-xl", DefaultValue = "TBD", Category = "typography", Description = "Extra large text size", IsActive = true, SortOrder = 204, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 205, ComponentGroup = "text", TokenKey = "size-2xl", CssVariable = "--ds-text-size-2xl", DefaultValue = "TBD", Category = "typography", Description = "2x large text size", IsActive = true, SortOrder = 205, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 206, ComponentGroup = "text", TokenKey = "font-regular", CssVariable = "--ds-text-font-regular", DefaultValue = "TBD", Category = "typography", Description = "Regular font weight", IsActive = true, SortOrder = 206, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 207, ComponentGroup = "text", TokenKey = "font-medium", CssVariable = "--ds-text-font-medium", DefaultValue = "TBD", Category = "typography", Description = "Medium font weight", IsActive = true, SortOrder = 207, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 208, ComponentGroup = "text", TokenKey = "font-semibold", CssVariable = "--ds-text-font-semibold", DefaultValue = "TBD", Category = "typography", Description = "Semibold font weight", IsActive = true, SortOrder = 208, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 209, ComponentGroup = "text", TokenKey = "font-bold", CssVariable = "--ds-text-font-bold", DefaultValue = "TBD", Category = "typography", Description = "Bold font weight", IsActive = true, SortOrder = 209, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 210, ComponentGroup = "text", TokenKey = "color-primary", CssVariable = "--ds-text-color-primary", DefaultValue = "TBD", Category = "color", Description = "Primary text color", IsActive = true, SortOrder = 210, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 211, ComponentGroup = "text", TokenKey = "color-secondary", CssVariable = "--ds-text-color-secondary", DefaultValue = "TBD", Category = "color", Description = "Secondary text color", IsActive = true, SortOrder = 211, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 212, ComponentGroup = "text", TokenKey = "color-muted", CssVariable = "--ds-text-color-muted", DefaultValue = "TBD", Category = "color", Description = "Muted text color", IsActive = true, SortOrder = 212, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 213, ComponentGroup = "text", TokenKey = "color-inverse", CssVariable = "--ds-text-color-inverse", DefaultValue = "TBD", Category = "color", Description = "Inverse text color for dark surfaces", IsActive = true, SortOrder = 213, CreatedAt = SeedDate, UpdatedAt = SeedDate },

                // ── Surface (IDs 250–256) ─────────────────────────────────────────────
                new DesignToken { Id = 250, ComponentGroup = "surface", TokenKey = "base", CssVariable = "--ds-surface-base", DefaultValue = "TBD", Category = "color", Description = "Base surface background", IsActive = true, SortOrder = 250, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 251, ComponentGroup = "surface", TokenKey = "raised", CssVariable = "--ds-surface-raised", DefaultValue = "TBD", Category = "color", Description = "Raised surface background", IsActive = true, SortOrder = 251, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 252, ComponentGroup = "surface", TokenKey = "subtle", CssVariable = "--ds-surface-subtle", DefaultValue = "TBD", Category = "color", Description = "Subtle surface background", IsActive = true, SortOrder = 252, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 253, ComponentGroup = "surface", TokenKey = "overlay", CssVariable = "--ds-surface-overlay", DefaultValue = "TBD", Category = "color", Description = "Overlay surface background", IsActive = true, SortOrder = 253, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 254, ComponentGroup = "surface", TokenKey = "hover", CssVariable = "--ds-surface-hover", DefaultValue = "TBD", Category = "color", Description = "Hover surface background", IsActive = true, SortOrder = 254, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 255, ComponentGroup = "surface", TokenKey = "active", CssVariable = "--ds-surface-active", DefaultValue = "TBD", Category = "color", Description = "Active surface background", IsActive = true, SortOrder = 255, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 256, ComponentGroup = "surface", TokenKey = "disabled", CssVariable = "--ds-surface-disabled", DefaultValue = "TBD", Category = "color", Description = "Disabled surface background", IsActive = true, SortOrder = 256, CreatedAt = SeedDate, UpdatedAt = SeedDate },

                // ── Border (IDs 300–307) ──────────────────────────────────────────────
                new DesignToken { Id = 300, ComponentGroup = "border", TokenKey = "default", CssVariable = "--ds-border-default", DefaultValue = "TBD", Category = "color", Description = "Default border color", IsActive = true, SortOrder = 300, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 301, ComponentGroup = "border", TokenKey = "muted", CssVariable = "--ds-border-muted", DefaultValue = "TBD", Category = "color", Description = "Muted border color", IsActive = true, SortOrder = 301, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 302, ComponentGroup = "border", TokenKey = "strong", CssVariable = "--ds-border-strong", DefaultValue = "TBD", Category = "color", Description = "Strong border color", IsActive = true, SortOrder = 302, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 303, ComponentGroup = "border", TokenKey = "focus", CssVariable = "--ds-border-focus", DefaultValue = "TBD", Category = "color", Description = "Focus border color", IsActive = true, SortOrder = 303, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 304, ComponentGroup = "border", TokenKey = "danger", CssVariable = "--ds-border-danger", DefaultValue = "TBD", Category = "color", Description = "Danger border color", IsActive = true, SortOrder = 304, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 305, ComponentGroup = "border", TokenKey = "warning", CssVariable = "--ds-border-warning", DefaultValue = "TBD", Category = "color", Description = "Warning border color", IsActive = true, SortOrder = 305, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 306, ComponentGroup = "border", TokenKey = "success", CssVariable = "--ds-border-success", DefaultValue = "TBD", Category = "color", Description = "Success border color", IsActive = true, SortOrder = 306, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 307, ComponentGroup = "border", TokenKey = "info", CssVariable = "--ds-border-info", DefaultValue = "TBD", Category = "color", Description = "Info border color", IsActive = true, SortOrder = 307, CreatedAt = SeedDate, UpdatedAt = SeedDate },

                // ── Radius (IDs 350–357) ──────────────────────────────────────────────
                new DesignToken { Id = 350, ComponentGroup = "radius", TokenKey = "xs", CssVariable = "--ds-radius-xs", DefaultValue = "TBD", Category = "radius", Description = "Extra small border radius", IsActive = true, SortOrder = 350, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 351, ComponentGroup = "radius", TokenKey = "sm", CssVariable = "--ds-radius-sm", DefaultValue = "TBD", Category = "radius", Description = "Small border radius", IsActive = true, SortOrder = 351, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 352, ComponentGroup = "radius", TokenKey = "md", CssVariable = "--ds-radius-md", DefaultValue = "TBD", Category = "radius", Description = "Medium border radius", IsActive = true, SortOrder = 352, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 353, ComponentGroup = "radius", TokenKey = "lg", CssVariable = "--ds-radius-lg", DefaultValue = "TBD", Category = "radius", Description = "Large border radius", IsActive = true, SortOrder = 353, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 354, ComponentGroup = "radius", TokenKey = "xl", CssVariable = "--ds-radius-xl", DefaultValue = "TBD", Category = "radius", Description = "Extra large border radius", IsActive = true, SortOrder = 354, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 355, ComponentGroup = "radius", TokenKey = "full", CssVariable = "--ds-radius-full", DefaultValue = "TBD", Category = "radius", Description = "Full border radius (pill)", IsActive = true, SortOrder = 355, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 356, ComponentGroup = "radius", TokenKey = "control", CssVariable = "--ds-radius-control", DefaultValue = "TBD", Category = "radius", Description = "Control border radius", IsActive = true, SortOrder = 356, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 357, ComponentGroup = "radius", TokenKey = "card", CssVariable = "--ds-radius-card", DefaultValue = "TBD", Category = "radius", Description = "Card border radius", IsActive = true, SortOrder = 357, CreatedAt = SeedDate, UpdatedAt = SeedDate },

                // ── Shadow (IDs 400–406) ──────────────────────────────────────────────
                new DesignToken { Id = 400, ComponentGroup = "shadow", TokenKey = "none", CssVariable = "--ds-shadow-none", DefaultValue = "TBD", Category = "shadow", Description = "No shadow", IsActive = true, SortOrder = 400, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 401, ComponentGroup = "shadow", TokenKey = "xs", CssVariable = "--ds-shadow-xs", DefaultValue = "TBD", Category = "shadow", Description = "Extra small shadow", IsActive = true, SortOrder = 401, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 402, ComponentGroup = "shadow", TokenKey = "sm", CssVariable = "--ds-shadow-sm", DefaultValue = "TBD", Category = "shadow", Description = "Small shadow", IsActive = true, SortOrder = 402, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 403, ComponentGroup = "shadow", TokenKey = "md", CssVariable = "--ds-shadow-md", DefaultValue = "TBD", Category = "shadow", Description = "Medium shadow", IsActive = true, SortOrder = 403, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 404, ComponentGroup = "shadow", TokenKey = "lg", CssVariable = "--ds-shadow-lg", DefaultValue = "TBD", Category = "shadow", Description = "Large shadow", IsActive = true, SortOrder = 404, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 405, ComponentGroup = "shadow", TokenKey = "dropdown", CssVariable = "--ds-shadow-dropdown", DefaultValue = "TBD", Category = "shadow", Description = "Dropdown shadow", IsActive = true, SortOrder = 405, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 406, ComponentGroup = "shadow", TokenKey = "alert", CssVariable = "--ds-shadow-alert", DefaultValue = "TBD", Category = "shadow", Description = "Alert shadow", IsActive = true, SortOrder = 406, CreatedAt = SeedDate, UpdatedAt = SeedDate },

                // ── Spacing (IDs 450–469) ─────────────────────────────────────────────
                new DesignToken { Id = 450, ComponentGroup = "spacing", TokenKey = "space-1", CssVariable = "--ds-space-1", DefaultValue = "TBD", Category = "spacing", Description = "1 unit spacing", IsActive = true, SortOrder = 450, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 451, ComponentGroup = "spacing", TokenKey = "space-2", CssVariable = "--ds-space-2", DefaultValue = "TBD", Category = "spacing", Description = "2 unit spacing", IsActive = true, SortOrder = 451, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 452, ComponentGroup = "spacing", TokenKey = "space-3", CssVariable = "--ds-space-3", DefaultValue = "TBD", Category = "spacing", Description = "3 unit spacing", IsActive = true, SortOrder = 452, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 453, ComponentGroup = "spacing", TokenKey = "space-4", CssVariable = "--ds-space-4", DefaultValue = "TBD", Category = "spacing", Description = "4 unit spacing", IsActive = true, SortOrder = 453, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 454, ComponentGroup = "spacing", TokenKey = "space-5", CssVariable = "--ds-space-5", DefaultValue = "TBD", Category = "spacing", Description = "5 unit spacing", IsActive = true, SortOrder = 454, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 455, ComponentGroup = "spacing", TokenKey = "space-6", CssVariable = "--ds-space-6", DefaultValue = "TBD", Category = "spacing", Description = "6 unit spacing", IsActive = true, SortOrder = 455, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 456, ComponentGroup = "spacing", TokenKey = "space-8", CssVariable = "--ds-space-8", DefaultValue = "TBD", Category = "spacing", Description = "8 unit spacing", IsActive = true, SortOrder = 456, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 457, ComponentGroup = "spacing", TokenKey = "space-10", CssVariable = "--ds-space-10", DefaultValue = "TBD", Category = "spacing", Description = "10 unit spacing", IsActive = true, SortOrder = 457, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 458, ComponentGroup = "spacing", TokenKey = "space-12", CssVariable = "--ds-space-12", DefaultValue = "TBD", Category = "spacing", Description = "12 unit spacing", IsActive = true, SortOrder = 458, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 459, ComponentGroup = "spacing", TokenKey = "gap-sm", CssVariable = "--ds-gap-sm", DefaultValue = "TBD", Category = "spacing", Description = "Small gap", IsActive = true, SortOrder = 459, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 460, ComponentGroup = "spacing", TokenKey = "gap-md", CssVariable = "--ds-gap-md", DefaultValue = "TBD", Category = "spacing", Description = "Medium gap", IsActive = true, SortOrder = 460, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 461, ComponentGroup = "spacing", TokenKey = "gap-lg", CssVariable = "--ds-gap-lg", DefaultValue = "TBD", Category = "spacing", Description = "Large gap", IsActive = true, SortOrder = 461, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 462, ComponentGroup = "spacing", TokenKey = "input-px", CssVariable = "--ds-input-px", DefaultValue = "TBD", Category = "spacing", Description = "Input horizontal padding", IsActive = true, SortOrder = 462, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 463, ComponentGroup = "spacing", TokenKey = "input-py", CssVariable = "--ds-input-py", DefaultValue = "TBD", Category = "spacing", Description = "Input vertical padding", IsActive = true, SortOrder = 463, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 464, ComponentGroup = "spacing", TokenKey = "table-cell-px", CssVariable = "--ds-table-cell-px", DefaultValue = "TBD", Category = "spacing", Description = "Table cell horizontal padding", IsActive = true, SortOrder = 464, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 465, ComponentGroup = "spacing", TokenKey = "table-cell-py", CssVariable = "--ds-table-cell-py", DefaultValue = "TBD", Category = "spacing", Description = "Table cell vertical padding", IsActive = true, SortOrder = 465, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 466, ComponentGroup = "spacing", TokenKey = "form-label-mb", CssVariable = "--ds-form-label-mb", DefaultValue = "TBD", Category = "spacing", Description = "Form label margin bottom", IsActive = true, SortOrder = 466, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 467, ComponentGroup = "spacing", TokenKey = "form-hint-mt", CssVariable = "--ds-form-hint-mt", DefaultValue = "TBD", Category = "spacing", Description = "Form hint margin top", IsActive = true, SortOrder = 467, CreatedAt = SeedDate, UpdatedAt = SeedDate },

                // ── Layout (IDs 500–504) ──────────────────────────────────────────────
                new DesignToken { Id = 500, ComponentGroup = "layout", TokenKey = "header-h", CssVariable = "--ds-layout-header-h", DefaultValue = "TBD", Category = "layout", Description = "Header height", IsActive = true, SortOrder = 500, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 501, ComponentGroup = "layout", TokenKey = "footer-h", CssVariable = "--ds-layout-footer-h", DefaultValue = "TBD", Category = "layout", Description = "Footer height", IsActive = true, SortOrder = 501, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 502, ComponentGroup = "layout", TokenKey = "sidebar-w", CssVariable = "--ds-layout-sidebar-w", DefaultValue = "TBD", Category = "layout", Description = "Sidebar width", IsActive = true, SortOrder = 502, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 503, ComponentGroup = "layout", TokenKey = "sidebar-w-collapsed", CssVariable = "--ds-layout-sidebar-w-collapsed", DefaultValue = "TBD", Category = "layout", Description = "Collapsed sidebar width", IsActive = true, SortOrder = 503, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 504, ComponentGroup = "layout", TokenKey = "shell-gap", CssVariable = "--ds-layout-shell-gap", DefaultValue = "TBD", Category = "layout", Description = "Shell region gap", IsActive = true, SortOrder = 504, CreatedAt = SeedDate, UpdatedAt = SeedDate },

                // ── Nav Header (IDs 550–557) ───────────────────────────────────────────
                new DesignToken { Id = 550, ComponentGroup = "nav-header", TokenKey = "bg", CssVariable = "--ds-nav-header-bg", DefaultValue = "TBD", Category = "color", Description = "Header background", IsActive = true, SortOrder = 550, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 551, ComponentGroup = "nav-header", TokenKey = "border", CssVariable = "--ds-nav-header-border", DefaultValue = "TBD", Category = "color", Description = "Header border color", IsActive = true, SortOrder = 551, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 552, ComponentGroup = "nav-header", TokenKey = "text", CssVariable = "--ds-nav-header-text", DefaultValue = "TBD", Category = "color", Description = "Header text color", IsActive = true, SortOrder = 552, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 553, ComponentGroup = "nav-header", TokenKey = "dropdown-bg", CssVariable = "--ds-nav-header-dropdown-bg", DefaultValue = "TBD", Category = "color", Description = "Header dropdown background", IsActive = true, SortOrder = 553, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 554, ComponentGroup = "nav-header", TokenKey = "dropdown-border", CssVariable = "--ds-nav-header-dropdown-border", DefaultValue = "TBD", Category = "color", Description = "Header dropdown border", IsActive = true, SortOrder = 554, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 555, ComponentGroup = "nav-header", TokenKey = "dropdown-shadow", CssVariable = "--ds-nav-header-dropdown-shadow", DefaultValue = "TBD", Category = "shadow", Description = "Header dropdown shadow", IsActive = true, SortOrder = 555, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 556, ComponentGroup = "nav-header", TokenKey = "badge-notification-bg", CssVariable = "--ds-nav-header-badge-notification-bg", DefaultValue = "TBD", Category = "color", Description = "Notification badge background", IsActive = true, SortOrder = 556, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 557, ComponentGroup = "nav-header", TokenKey = "badge-notification-text", CssVariable = "--ds-nav-header-badge-notification-text", DefaultValue = "TBD", Category = "color", Description = "Notification badge text", IsActive = true, SortOrder = 557, CreatedAt = SeedDate, UpdatedAt = SeedDate },

                // ── Nav Sidebar (IDs 600–609) ──────────────────────────────────────────
                new DesignToken { Id = 600, ComponentGroup = "nav-sidebar", TokenKey = "bg", CssVariable = "--ds-nav-sidebar-bg", DefaultValue = "TBD", Category = "color", Description = "Sidebar background", IsActive = true, SortOrder = 600, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 601, ComponentGroup = "nav-sidebar", TokenKey = "border", CssVariable = "--ds-nav-sidebar-border", DefaultValue = "TBD", Category = "color", Description = "Sidebar border color", IsActive = true, SortOrder = 601, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 602, ComponentGroup = "nav-sidebar", TokenKey = "text", CssVariable = "--ds-nav-sidebar-text", DefaultValue = "TBD", Category = "color", Description = "Sidebar text color", IsActive = true, SortOrder = 602, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 603, ComponentGroup = "nav-sidebar", TokenKey = "text-muted", CssVariable = "--ds-nav-sidebar-text-muted", DefaultValue = "TBD", Category = "color", Description = "Sidebar muted text color", IsActive = true, SortOrder = 603, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 604, ComponentGroup = "nav-sidebar", TokenKey = "hover-bg", CssVariable = "--ds-nav-sidebar-hover-bg", DefaultValue = "TBD", Category = "color", Description = "Sidebar hover background", IsActive = true, SortOrder = 604, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 605, ComponentGroup = "nav-sidebar", TokenKey = "active-bg", CssVariable = "--ds-nav-sidebar-active-bg", DefaultValue = "TBD", Category = "color", Description = "Sidebar active background", IsActive = true, SortOrder = 605, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 606, ComponentGroup = "nav-sidebar", TokenKey = "active-text", CssVariable = "--ds-nav-sidebar-active-text", DefaultValue = "TBD", Category = "color", Description = "Sidebar active text color", IsActive = true, SortOrder = 606, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 607, ComponentGroup = "nav-sidebar", TokenKey = "scrollbar-thumb", CssVariable = "--ds-nav-sidebar-scrollbar-thumb", DefaultValue = "TBD", Category = "color", Description = "Sidebar scrollbar thumb color", IsActive = true, SortOrder = 607, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 608, ComponentGroup = "nav-sidebar", TokenKey = "scrollbar-thumb-hover", CssVariable = "--ds-nav-sidebar-scrollbar-thumb-hover", DefaultValue = "TBD", Category = "color", Description = "Sidebar scrollbar thumb hover", IsActive = true, SortOrder = 608, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 609, ComponentGroup = "nav-sidebar", TokenKey = "category-text", CssVariable = "--ds-nav-sidebar-category-text", DefaultValue = "TBD", Category = "color", Description = "Sidebar category text color", IsActive = true, SortOrder = 609, CreatedAt = SeedDate, UpdatedAt = SeedDate },

                // ── Form Field (IDs 650–660) ───────────────────────────────────────────
                new DesignToken { Id = 650, ComponentGroup = "form-field", TokenKey = "bg", CssVariable = "--ds-form-field-bg", DefaultValue = "TBD", Category = "color", Description = "Form field background", IsActive = true, SortOrder = 650, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 651, ComponentGroup = "form-field", TokenKey = "text", CssVariable = "--ds-form-field-text", DefaultValue = "TBD", Category = "color", Description = "Form field text color", IsActive = true, SortOrder = 651, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 652, ComponentGroup = "form-field", TokenKey = "border", CssVariable = "--ds-form-field-border", DefaultValue = "TBD", Category = "color", Description = "Form field border color", IsActive = true, SortOrder = 652, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 653, ComponentGroup = "form-field", TokenKey = "placeholder", CssVariable = "--ds-form-field-placeholder", DefaultValue = "TBD", Category = "color", Description = "Form field placeholder color", IsActive = true, SortOrder = 653, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 654, ComponentGroup = "form-field", TokenKey = "focus-border", CssVariable = "--ds-form-field-focus-border", DefaultValue = "TBD", Category = "color", Description = "Form field focus border", IsActive = true, SortOrder = 654, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 655, ComponentGroup = "form-field", TokenKey = "focus-ring", CssVariable = "--ds-form-field-focus-ring", DefaultValue = "TBD", Category = "shadow", Description = "Form field focus ring shadow", IsActive = true, SortOrder = 655, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 656, ComponentGroup = "form-field", TokenKey = "error-border", CssVariable = "--ds-form-field-error-border", DefaultValue = "TBD", Category = "color", Description = "Form field error border", IsActive = true, SortOrder = 656, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 657, ComponentGroup = "form-field", TokenKey = "error-text", CssVariable = "--ds-form-field-error-text", DefaultValue = "TBD", Category = "color", Description = "Form field error text", IsActive = true, SortOrder = 657, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 658, ComponentGroup = "form-field", TokenKey = "label", CssVariable = "--ds-form-field-label", DefaultValue = "TBD", Category = "color", Description = "Form label text color", IsActive = true, SortOrder = 658, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 659, ComponentGroup = "form-field", TokenKey = "hint", CssVariable = "--ds-form-field-hint", DefaultValue = "TBD", Category = "color", Description = "Form hint text color", IsActive = true, SortOrder = 659, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 660, ComponentGroup = "form-field", TokenKey = "disabled", CssVariable = "--ds-form-field-disabled", DefaultValue = "TBD", Category = "color", Description = "Form field disabled state", IsActive = true, SortOrder = 660, CreatedAt = SeedDate, UpdatedAt = SeedDate },

                // ── Form Shell (IDs 700–704) ───────────────────────────────────────────
                new DesignToken { Id = 700, ComponentGroup = "form-shell", TokenKey = "bg", CssVariable = "--ds-form-shell-bg", DefaultValue = "TBD", Category = "color", Description = "Form shell background", IsActive = true, SortOrder = 700, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 701, ComponentGroup = "form-shell", TokenKey = "border", CssVariable = "--ds-form-shell-border", DefaultValue = "TBD", Category = "color", Description = "Form shell border color", IsActive = true, SortOrder = 701, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 702, ComponentGroup = "form-shell", TokenKey = "shadow", CssVariable = "--ds-form-shell-shadow", DefaultValue = "TBD", Category = "shadow", Description = "Form shell shadow", IsActive = true, SortOrder = 702, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 703, ComponentGroup = "form-shell", TokenKey = "muted", CssVariable = "--ds-form-shell-muted", DefaultValue = "TBD", Category = "color", Description = "Form shell muted text", IsActive = true, SortOrder = 703, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 704, ComponentGroup = "form-shell", TokenKey = "action-bg", CssVariable = "--ds-form-shell-action-bg", DefaultValue = "TBD", Category = "color", Description = "Form shell action area background", IsActive = true, SortOrder = 704, CreatedAt = SeedDate, UpdatedAt = SeedDate },

                // ── Table (IDs 750–757) ────────────────────────────────────────────────
                new DesignToken { Id = 750, ComponentGroup = "table", TokenKey = "header-bg", CssVariable = "--ds-table-header-bg", DefaultValue = "TBD", Category = "color", Description = "Table header background", IsActive = true, SortOrder = 750, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 751, ComponentGroup = "table", TokenKey = "header-text", CssVariable = "--ds-table-header-text", DefaultValue = "TBD", Category = "color", Description = "Table header text color", IsActive = true, SortOrder = 751, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 752, ComponentGroup = "table", TokenKey = "row-even-bg", CssVariable = "--ds-table-row-even-bg", DefaultValue = "TBD", Category = "color", Description = "Table even row background", IsActive = true, SortOrder = 752, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 753, ComponentGroup = "table", TokenKey = "row-hover-bg", CssVariable = "--ds-table-row-hover-bg", DefaultValue = "TBD", Category = "color", Description = "Table row hover background", IsActive = true, SortOrder = 753, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 754, ComponentGroup = "table", TokenKey = "border", CssVariable = "--ds-table-border", DefaultValue = "TBD", Category = "color", Description = "Table border color", IsActive = true, SortOrder = 754, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 755, ComponentGroup = "table", TokenKey = "surface", CssVariable = "--ds-table-surface", DefaultValue = "TBD", Category = "color", Description = "Table surface color", IsActive = true, SortOrder = 755, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 756, ComponentGroup = "table", TokenKey = "muted", CssVariable = "--ds-table-muted", DefaultValue = "TBD", Category = "color", Description = "Table muted text", IsActive = true, SortOrder = 756, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 757, ComponentGroup = "table", TokenKey = "action-primary-bg", CssVariable = "--ds-table-action-primary-bg", DefaultValue = "TBD", Category = "color", Description = "Table action primary background", IsActive = true, SortOrder = 757, CreatedAt = SeedDate, UpdatedAt = SeedDate },

                // ── Alert (IDs 800–813) ────────────────────────────────────────────────
                new DesignToken { Id = 800, ComponentGroup = "alert", TokenKey = "info-bg", CssVariable = "--ds-alert-info-bg", DefaultValue = "TBD", Category = "color", Description = "Alert info background", IsActive = true, SortOrder = 800, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 801, ComponentGroup = "alert", TokenKey = "info-border", CssVariable = "--ds-alert-info-border", DefaultValue = "TBD", Category = "color", Description = "Alert info border", IsActive = true, SortOrder = 801, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 802, ComponentGroup = "alert", TokenKey = "info-text", CssVariable = "--ds-alert-info-text", DefaultValue = "TBD", Category = "color", Description = "Alert info text", IsActive = true, SortOrder = 802, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 803, ComponentGroup = "alert", TokenKey = "success-bg", CssVariable = "--ds-alert-success-bg", DefaultValue = "TBD", Category = "color", Description = "Alert success background", IsActive = true, SortOrder = 803, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 804, ComponentGroup = "alert", TokenKey = "success-border", CssVariable = "--ds-alert-success-border", DefaultValue = "TBD", Category = "color", Description = "Alert success border", IsActive = true, SortOrder = 804, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 805, ComponentGroup = "alert", TokenKey = "success-text", CssVariable = "--ds-alert-success-text", DefaultValue = "TBD", Category = "color", Description = "Alert success text", IsActive = true, SortOrder = 805, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 806, ComponentGroup = "alert", TokenKey = "warning-bg", CssVariable = "--ds-alert-warning-bg", DefaultValue = "TBD", Category = "color", Description = "Alert warning background", IsActive = true, SortOrder = 806, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 807, ComponentGroup = "alert", TokenKey = "warning-border", CssVariable = "--ds-alert-warning-border", DefaultValue = "TBD", Category = "color", Description = "Alert warning border", IsActive = true, SortOrder = 807, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 808, ComponentGroup = "alert", TokenKey = "warning-text", CssVariable = "--ds-alert-warning-text", DefaultValue = "TBD", Category = "color", Description = "Alert warning text", IsActive = true, SortOrder = 808, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 809, ComponentGroup = "alert", TokenKey = "danger-bg", CssVariable = "--ds-alert-danger-bg", DefaultValue = "TBD", Category = "color", Description = "Alert danger background", IsActive = true, SortOrder = 809, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 810, ComponentGroup = "alert", TokenKey = "danger-border", CssVariable = "--ds-alert-danger-border", DefaultValue = "TBD", Category = "color", Description = "Alert danger border", IsActive = true, SortOrder = 810, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 811, ComponentGroup = "alert", TokenKey = "danger-text", CssVariable = "--ds-alert-danger-text", DefaultValue = "TBD", Category = "color", Description = "Alert danger text", IsActive = true, SortOrder = 811, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 812, ComponentGroup = "alert", TokenKey = "shadow", CssVariable = "--ds-alert-shadow", DefaultValue = "TBD", Category = "shadow", Description = "Alert shadow", IsActive = true, SortOrder = 812, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 813, ComponentGroup = "alert", TokenKey = "radius", CssVariable = "--ds-alert-radius", DefaultValue = "TBD", Category = "radius", Description = "Alert border radius", IsActive = true, SortOrder = 813, CreatedAt = SeedDate, UpdatedAt = SeedDate },

                // ── Icon (IDs 850–858) ─────────────────────────────────────────────────
                new DesignToken { Id = 850, ComponentGroup = "icon", TokenKey = "size-xs", CssVariable = "--ds-icon-size-xs", DefaultValue = "TBD", Category = "typography", Description = "Extra small icon size", IsActive = true, SortOrder = 850, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 851, ComponentGroup = "icon", TokenKey = "size-sm", CssVariable = "--ds-icon-size-sm", DefaultValue = "TBD", Category = "typography", Description = "Small icon size", IsActive = true, SortOrder = 851, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 852, ComponentGroup = "icon", TokenKey = "size-md", CssVariable = "--ds-icon-size-md", DefaultValue = "TBD", Category = "typography", Description = "Medium icon size", IsActive = true, SortOrder = 852, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 853, ComponentGroup = "icon", TokenKey = "size-lg", CssVariable = "--ds-icon-size-lg", DefaultValue = "TBD", Category = "typography", Description = "Large icon size", IsActive = true, SortOrder = 853, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 854, ComponentGroup = "icon", TokenKey = "size-xl", CssVariable = "--ds-icon-size-xl", DefaultValue = "TBD", Category = "typography", Description = "Extra large icon size", IsActive = true, SortOrder = 854, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 855, ComponentGroup = "icon", TokenKey = "size-2xl", CssVariable = "--ds-icon-size-2xl", DefaultValue = "TBD", Category = "typography", Description = "2x large icon size", IsActive = true, SortOrder = 855, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 856, ComponentGroup = "icon", TokenKey = "color", CssVariable = "--ds-icon-color", DefaultValue = "TBD", Category = "color", Description = "Icon color", IsActive = true, SortOrder = 856, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 857, ComponentGroup = "icon", TokenKey = "color-muted", CssVariable = "--ds-icon-color-muted", DefaultValue = "TBD", Category = "color", Description = "Muted icon color", IsActive = true, SortOrder = 857, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 858, ComponentGroup = "icon", TokenKey = "color-accent", CssVariable = "--ds-icon-color-accent", DefaultValue = "TBD", Category = "color", Description = "Accent icon color", IsActive = true, SortOrder = 858, CreatedAt = SeedDate, UpdatedAt = SeedDate },

                // ── Icon Button (IDs 900–914) ──────────────────────────────────────────
                new DesignToken { Id = 900, ComponentGroup = "icon-btn", TokenKey = "gap", CssVariable = "--ds-icon-btn-gap", DefaultValue = "TBD", Category = "spacing", Description = "Icon button content gap", IsActive = true, SortOrder = 900, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 901, ComponentGroup = "icon-btn", TokenKey = "radius", CssVariable = "--ds-icon-btn-radius", DefaultValue = "TBD", Category = "radius", Description = "Icon button border radius", IsActive = true, SortOrder = 901, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 902, ComponentGroup = "icon-btn", TokenKey = "font-size-sm", CssVariable = "--ds-icon-btn-font-size-sm", DefaultValue = "TBD", Category = "typography", Description = "Small icon button font size", IsActive = true, SortOrder = 902, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 903, ComponentGroup = "icon-btn", TokenKey = "font-size-md", CssVariable = "--ds-icon-btn-font-size-md", DefaultValue = "TBD", Category = "typography", Description = "Medium icon button font size", IsActive = true, SortOrder = 903, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 904, ComponentGroup = "icon-btn", TokenKey = "font-size-lg", CssVariable = "--ds-icon-btn-font-size-lg", DefaultValue = "TBD", Category = "typography", Description = "Large icon button font size", IsActive = true, SortOrder = 904, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 905, ComponentGroup = "icon-btn", TokenKey = "padding-sm", CssVariable = "--ds-icon-btn-padding-sm", DefaultValue = "TBD", Category = "spacing", Description = "Small icon button padding", IsActive = true, SortOrder = 905, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 906, ComponentGroup = "icon-btn", TokenKey = "padding-md", CssVariable = "--ds-icon-btn-padding-md", DefaultValue = "TBD", Category = "spacing", Description = "Medium icon button padding", IsActive = true, SortOrder = 906, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 907, ComponentGroup = "icon-btn", TokenKey = "padding-lg", CssVariable = "--ds-icon-btn-padding-lg", DefaultValue = "TBD", Category = "spacing", Description = "Large icon button padding", IsActive = true, SortOrder = 907, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 908, ComponentGroup = "icon-btn", TokenKey = "ghost-bg", CssVariable = "--ds-icon-btn-ghost-bg", DefaultValue = "TBD", Category = "color", Description = "Ghost icon button background", IsActive = true, SortOrder = 908, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 909, ComponentGroup = "icon-btn", TokenKey = "ghost-hover-bg", CssVariable = "--ds-icon-btn-ghost-hover-bg", DefaultValue = "TBD", Category = "color", Description = "Ghost icon button hover background", IsActive = true, SortOrder = 909, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 910, ComponentGroup = "icon-btn", TokenKey = "primary-bg", CssVariable = "--ds-icon-btn-primary-bg", DefaultValue = "TBD", Category = "color", Description = "Primary icon button background", IsActive = true, SortOrder = 910, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 911, ComponentGroup = "icon-btn", TokenKey = "primary-hover-bg", CssVariable = "--ds-icon-btn-primary-hover-bg", DefaultValue = "TBD", Category = "color", Description = "Primary icon button hover background", IsActive = true, SortOrder = 911, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 912, ComponentGroup = "icon-btn", TokenKey = "danger-bg", CssVariable = "--ds-icon-btn-danger-bg", DefaultValue = "TBD", Category = "color", Description = "Danger icon button background", IsActive = true, SortOrder = 912, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 913, ComponentGroup = "icon-btn", TokenKey = "danger-hover-bg", CssVariable = "--ds-icon-btn-danger-hover-bg", DefaultValue = "TBD", Category = "color", Description = "Danger icon button hover background", IsActive = true, SortOrder = 913, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 914, ComponentGroup = "icon-btn", TokenKey = "text", CssVariable = "--ds-icon-btn-text", DefaultValue = "TBD", Category = "color", Description = "Icon button text color", IsActive = true, SortOrder = 914, CreatedAt = SeedDate, UpdatedAt = SeedDate },

                // ── Badge Status (IDs 950–957) ──────────────────────────────────────────
                new DesignToken { Id = 950, ComponentGroup = "badge-status", TokenKey = "active-bg", CssVariable = "--ds-badge-active-bg", DefaultValue = "TBD", Category = "color", Description = "Active badge background", IsActive = true, SortOrder = 950, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 951, ComponentGroup = "badge-status", TokenKey = "active-text", CssVariable = "--ds-badge-active-text", DefaultValue = "TBD", Category = "color", Description = "Active badge text", IsActive = true, SortOrder = 951, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 952, ComponentGroup = "badge-status", TokenKey = "inactive-bg", CssVariable = "--ds-badge-inactive-bg", DefaultValue = "TBD", Category = "color", Description = "Inactive badge background", IsActive = true, SortOrder = 952, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 953, ComponentGroup = "badge-status", TokenKey = "inactive-text", CssVariable = "--ds-badge-inactive-text", DefaultValue = "TBD", Category = "color", Description = "Inactive badge text", IsActive = true, SortOrder = 953, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 954, ComponentGroup = "badge-status", TokenKey = "planned-bg", CssVariable = "--ds-badge-planned-bg", DefaultValue = "TBD", Category = "color", Description = "Planned badge background", IsActive = true, SortOrder = 954, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 955, ComponentGroup = "badge-status", TokenKey = "planned-text", CssVariable = "--ds-badge-planned-text", DefaultValue = "TBD", Category = "color", Description = "Planned badge text", IsActive = true, SortOrder = 955, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 956, ComponentGroup = "badge-status", TokenKey = "radius", CssVariable = "--ds-badge-radius", DefaultValue = "TBD", Category = "radius", Description = "Badge border radius", IsActive = true, SortOrder = 956, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 957, ComponentGroup = "badge-status", TokenKey = "font-size", CssVariable = "--ds-badge-font-size", DefaultValue = "TBD", Category = "typography", Description = "Badge font size", IsActive = true, SortOrder = 957, CreatedAt = SeedDate, UpdatedAt = SeedDate },

                // ── Chart (IDs 1000–1010) ──────────────────────────────────────────────
                new DesignToken { Id = 1000, ComponentGroup = "chart", TokenKey = "surface", CssVariable = "--ds-chart-surface", DefaultValue = "TBD", Category = "color", Description = "Chart surface background", IsActive = true, SortOrder = 1000, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 1001, ComponentGroup = "chart", TokenKey = "grid-line", CssVariable = "--ds-chart-grid-line", DefaultValue = "TBD", Category = "color", Description = "Chart grid line color", IsActive = true, SortOrder = 1001, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 1002, ComponentGroup = "chart", TokenKey = "axis-text", CssVariable = "--ds-chart-axis-text", DefaultValue = "TBD", Category = "color", Description = "Chart axis text color", IsActive = true, SortOrder = 1002, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 1003, ComponentGroup = "chart", TokenKey = "series-1", CssVariable = "--ds-chart-series-1", DefaultValue = "TBD", Category = "color", Description = "Chart series 1 color", IsActive = true, SortOrder = 1003, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 1004, ComponentGroup = "chart", TokenKey = "series-2", CssVariable = "--ds-chart-series-2", DefaultValue = "TBD", Category = "color", Description = "Chart series 2 color", IsActive = true, SortOrder = 1004, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 1005, ComponentGroup = "chart", TokenKey = "positive", CssVariable = "--ds-chart-positive", DefaultValue = "TBD", Category = "color", Description = "Chart positive value color", IsActive = true, SortOrder = 1005, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 1006, ComponentGroup = "chart", TokenKey = "warning", CssVariable = "--ds-chart-warning", DefaultValue = "TBD", Category = "color", Description = "Chart warning value color", IsActive = true, SortOrder = 1006, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 1007, ComponentGroup = "chart", TokenKey = "danger", CssVariable = "--ds-chart-danger", DefaultValue = "TBD", Category = "color", Description = "Chart danger value color", IsActive = true, SortOrder = 1007, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 1008, ComponentGroup = "chart", TokenKey = "neutral", CssVariable = "--ds-chart-neutral", DefaultValue = "TBD", Category = "color", Description = "Chart neutral value color", IsActive = true, SortOrder = 1008, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 1009, ComponentGroup = "chart", TokenKey = "tooltip-bg", CssVariable = "--ds-chart-tooltip-bg", DefaultValue = "TBD", Category = "color", Description = "Chart tooltip background", IsActive = true, SortOrder = 1009, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 1010, ComponentGroup = "chart", TokenKey = "tooltip-text", CssVariable = "--ds-chart-tooltip-text", DefaultValue = "TBD", Category = "color", Description = "Chart tooltip text color", IsActive = true, SortOrder = 1010, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                // ── Phase 1: ActionButtonV2 tokens (IDs 1100–1115) ─────────────────────
                new DesignToken { Id = 1100, ComponentGroup = "action-btn", TokenKey = "primary-bg",       CssVariable = "--ds-action-btn-primary-bg",       DefaultValue = "TBD", Category = "color",      Description = "Action button primary background",         IsActive = true, SortOrder = 1100, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 1101, ComponentGroup = "action-btn", TokenKey = "primary-hover-bg", CssVariable = "--ds-action-btn-primary-hover-bg", DefaultValue = "TBD", Category = "color",      Description = "Action button primary hover background",   IsActive = true, SortOrder = 1101, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 1102, ComponentGroup = "action-btn", TokenKey = "primary-text",     CssVariable = "--ds-action-btn-primary-text",     DefaultValue = "TBD", Category = "color",      Description = "Action button primary text",              IsActive = true, SortOrder = 1102, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 1103, ComponentGroup = "action-btn", TokenKey = "primary-icon",     CssVariable = "--ds-action-btn-primary-icon",     DefaultValue = "TBD", Category = "color",      Description = "Action button primary icon colour",       IsActive = true, SortOrder = 1103, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 1104, ComponentGroup = "action-btn", TokenKey = "secondary-bg",     CssVariable = "--ds-action-btn-secondary-bg",     DefaultValue = "TBD", Category = "color",      Description = "Action button secondary background",      IsActive = true, SortOrder = 1104, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 1105, ComponentGroup = "action-btn", TokenKey = "secondary-hover-bg", CssVariable = "--ds-action-btn-secondary-hover-bg", DefaultValue = "TBD", Category = "color", Description = "Action button secondary hover background", IsActive = true, SortOrder = 1105, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 1106, ComponentGroup = "action-btn", TokenKey = "secondary-text",   CssVariable = "--ds-action-btn-secondary-text",   DefaultValue = "TBD", Category = "color",      Description = "Action button secondary text",            IsActive = true, SortOrder = 1106, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 1107, ComponentGroup = "action-btn", TokenKey = "secondary-icon",   CssVariable = "--ds-action-btn-secondary-icon",   DefaultValue = "TBD", Category = "color",      Description = "Action button secondary icon colour",     IsActive = true, SortOrder = 1107, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 1108, ComponentGroup = "action-btn", TokenKey = "radius",           CssVariable = "--ds-action-btn-radius",           DefaultValue = "TBD", Category = "radius",     Description = "Action button border radius",             IsActive = true, SortOrder = 1108, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 1109, ComponentGroup = "action-btn", TokenKey = "padding-x",        CssVariable = "--ds-action-btn-padding-x",        DefaultValue = "TBD", Category = "spacing",    Description = "Action button horizontal padding",        IsActive = true, SortOrder = 1109, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 1110, ComponentGroup = "action-btn", TokenKey = "padding-y",        CssVariable = "--ds-action-btn-padding-y",        DefaultValue = "TBD", Category = "spacing",    Description = "Action button vertical padding",          IsActive = true, SortOrder = 1110, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 1111, ComponentGroup = "action-btn", TokenKey = "font-size",        CssVariable = "--ds-action-btn-font-size",        DefaultValue = "TBD", Category = "typography", Description = "Action button default font size",         IsActive = true, SortOrder = 1111, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 1112, ComponentGroup = "action-btn", TokenKey = "font-size-sm",     CssVariable = "--ds-action-btn-font-size-sm",     DefaultValue = "TBD", Category = "typography", Description = "Action button small font size",           IsActive = true, SortOrder = 1112, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 1113, ComponentGroup = "action-btn", TokenKey = "font-size-lg",     CssVariable = "--ds-action-btn-font-size-lg",     DefaultValue = "TBD", Category = "typography", Description = "Action button large font size",           IsActive = true, SortOrder = 1113, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 1114, ComponentGroup = "action-btn", TokenKey = "icon-size",        CssVariable = "--ds-action-btn-icon-size",        DefaultValue = "TBD", Category = "typography", Description = "Action button icon size",                 IsActive = true, SortOrder = 1114, CreatedAt = SeedDate, UpdatedAt = SeedDate },
                new DesignToken { Id = 1115, ComponentGroup = "action-btn", TokenKey = "icon-only-size",   CssVariable = "--ds-action-btn-icon-only-size",   DefaultValue = "TBD", Category = "structure",  Description = "Action button icon-only square size",     IsActive = true, SortOrder = 1115, CreatedAt = SeedDate, UpdatedAt = SeedDate }
            );
        });

        return modelBuilder;
    }
}
