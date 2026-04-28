/*
===============================================================================
 File        : Feature.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 3.0.0
 Created     : 2026-04-05
 Updated     : 2026-04-28
-------------------------------------------------------------------------------
 Purpose     : EF Core entity representing a single entitlement feature key.
               Phase 2 — Entitlement-driven navigation and feature control.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-05  Initial implementation. Created in AbsenceApp.Data.Models
                         so AppDbContext can configure the entity without
                         creating a circular dependency on AbsenceApp.Api.

   - 2.0.0  2026-04-19  Schema alignment: renamed FeatureId→Id, Key→Code,
                         IsActive→IsEnabled, CreatedAtUtc→CreatedAt.
                         Added DisplayName and UpdatedAt to match CSV schema.
                         Table renamed features→feature.

   - 3.0.0  2026-04-28  Database rename: underlying table renamed from
                         `feature` → `features`. Added explicit EF Core table
                         mapping via [Table("features")]. No behavioural changes.
-------------------------------------------------------------------------------
 Notes       :
   - Feature codes must be stable and unique.
   - Configured via EntitlementsModelBuilderExtensions.ConfigureEntitlements().
===============================================================================
*/

namespace AbsenceApp.Data.Models;

public sealed class Feature
{
    public int Id { get; set; }

    public string Code { get; set; } = string.Empty;

    public string? DisplayName { get; set; }

    public string? Description { get; set; }

    public bool IsEnabled { get; set; } = true;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
