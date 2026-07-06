/*
===============================================================================
 File        : DesignToken.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 1.1.1
 Created     : 2026-05-12
 Updated     : 2026-06-03
-------------------------------------------------------------------------------
 Purpose     : EF Core entity representing a single design token stored in the
               DesignTokens table. Each row maps a logical token key
               (e.g. "primary-bg") within a component group (e.g. "btn") to
               a CSS custom property (e.g. "--ds-btn-primary-bg") and its
               editable current value.

               DefaultValue holds the canonical fallback; CurrentValue holds
               the admin-overridden value (NULL means "use the default").
               DesignTokenApiServiceV2 generates a :root { ... } block at
               runtime using CurrentValue ?? DefaultValue for every active row.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-05-12  Initial creation (Phase A — Design Token System).
   - 1.1.0  2026-06-03  Phase 4.1 — Added Family, Variant, GroupName fields.
                        Promoted ComponentGroup → Component (domain-level),
                        retained DB column name via [Column("ComponentGroup")].
   - 1.1.1  2026-06-24  Added Enabled property to support token visibility
                        filtering in V2 (IsActive && Enabled).
-------------------------------------------------------------------------------
 Notes       :
   - Table name "DesignTokens" is PascalCase to distinguish it from the
     lowercase snake-case legacy tables.
   - Unique index: (Component, TokenKey) — enforced at DB and EF level.
   - SortOrder governs the order of CSS variable emission within a group.
===============================================================================
*/

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AbsenceApp.Data.Models;

/// <summary>
/// Represents a single design token entry in the DesignTokens table.
/// </summary>
[Table("DesignTokens")]
public class DesignToken
{
    // -------------------------------------------------------------------------
    // Identity
    // -------------------------------------------------------------------------

    /// <summary>Auto-increment primary key.</summary>
    [Key]
    public long Id { get; set; }

    // -------------------------------------------------------------------------
    // Token addressing
    // -------------------------------------------------------------------------

    /// <summary>
    /// Logical grouping key, e.g. "btn", "card", "badge".
    /// Combined with TokenKey forms a unique token address.
    /// Domain name: Component
    /// DB column: ComponentGroup
    /// </summary>
    [Required]
    [MaxLength(100)]
    [Column("ComponentGroup")] // keep DB compatibility
    public string Component { get; set; } = string.Empty;

    /// <summary>
    /// Unique key within the component group, e.g. "primary-bg".
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string TokenKey { get; set; } = string.Empty;

    // -------------------------------------------------------------------------
    // CSS mapping
    // -------------------------------------------------------------------------

    /// <summary>
    /// The CSS custom property name, e.g. "--ds-btn-primary-bg".
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string CssVariable { get; set; } = string.Empty;

    /// <summary>
    /// The canonical fallback value (never null). Used when CurrentValue is null.
    /// </summary>
    [Required]
    [MaxLength(500)]
    public string DefaultValue { get; set; } = string.Empty;

    /// <summary>
    /// Admin-overridden value. NULL means the token is using its DefaultValue.
    /// </summary>
    [MaxLength(500)]
    public string? CurrentValue { get; set; }

    // -------------------------------------------------------------------------
    // Metadata
    // -------------------------------------------------------------------------

    /// <summary>
    /// Semantic category for grouping in the editor UI, e.g. "color", "structure".
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Human-readable description of what this token controls.
    /// </summary>
    [MaxLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// When false the token is excluded from CSS generation and the editor.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// When false the token is hidden from CSS generation and the editor.
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Emission order within Component (ascending).
    /// </summary>
    public int SortOrder { get; set; }

    // -------------------------------------------------------------------------
    // Phase 4 — Family / Variant / GroupName
    // -------------------------------------------------------------------------

    /// <summary>
    /// High-level family grouping, e.g. "basic", "outline", "soft".
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Family { get; set; } = string.Empty;

    /// <summary>
    /// Variant within the family, e.g. "primary", "secondary", "danger".
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Variant { get; set; } = string.Empty;

    /// <summary>
    /// Deterministic combined key, e.g. "basic-primary".
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string GroupName { get; set; } = string.Empty;

    // -------------------------------------------------------------------------
    // Audit
    // -------------------------------------------------------------------------

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
