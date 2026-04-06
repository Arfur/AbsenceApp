/*
===============================================================================
 File        : UserFeatureOverride.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-04-05
 Updated     : 2026-04-05
-------------------------------------------------------------------------------
 Purpose     : EF Core entity representing per-user entitlement overrides.
               User overrides are authoritative over role defaults.
               Phase 2 — Entitlement-driven navigation and feature control.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-05  Initial implementation. Created in AbsenceApp.Data.Models
                         so AppDbContext can configure the entity without
                         creating a circular dependency on AbsenceApp.Api.
-------------------------------------------------------------------------------
 Notes       :
   - Configured via EntitlementsModelBuilderExtensions.ConfigureEntitlements().
===============================================================================
*/

namespace AbsenceApp.Data.Models;

public sealed class UserFeatureOverride
{
    public int UserFeatureOverrideId { get; set; }

    public Guid UserId { get; set; }

    public int FeatureId { get; set; }
    public Feature? Feature { get; set; }

    public bool IsAllowed { get; set; }

    public string? Reason { get; set; }

    public Guid? UpdatedByUserId { get; set; }

    public DateTime UpdatedAtUtc { get; set; }
}
