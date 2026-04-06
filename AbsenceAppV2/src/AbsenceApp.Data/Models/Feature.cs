/*
===============================================================================
 File        : Feature.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-04-05
 Updated     : 2026-04-05
-------------------------------------------------------------------------------
 Purpose     : EF Core entity representing a single entitlement feature key.
               Phase 2 — Entitlement-driven navigation and feature control.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-05  Initial implementation. Created in AbsenceApp.Data.Models
                         so AppDbContext can configure the entity without
                         creating a circular dependency on AbsenceApp.Api.
-------------------------------------------------------------------------------
 Notes       :
   - Feature keys must be stable and unique.
   - Configured via EntitlementsModelBuilderExtensions.ConfigureEntitlements().
===============================================================================
*/

namespace AbsenceApp.Data.Models;

public sealed class Feature
{
    public int FeatureId { get; set; }

    public string Key { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAtUtc { get; set; }
}
