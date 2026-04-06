/*
===============================================================================
 File        : RoleFeature.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-04-05
 Updated     : 2026-04-05
-------------------------------------------------------------------------------
 Purpose     : EF Core entity representing default entitlements assigned to a
               role type. Phase 2 — Entitlement-driven navigation and feature
               control.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-05  Initial implementation. Created in AbsenceApp.Data.Models
                         so AppDbContext can configure the entity without
                         creating a circular dependency on AbsenceApp.Api.
-------------------------------------------------------------------------------
 Notes       :
   - RoleType must align with the existing RoleType source of truth.
   - Configured via EntitlementsModelBuilderExtensions.ConfigureEntitlements().
===============================================================================
*/

namespace AbsenceApp.Data.Models;

public sealed class RoleFeature
{
    public int RoleFeatureId { get; set; }

    public int RoleType { get; set; }

    public int FeatureId { get; set; }
    public Feature? Feature { get; set; }

    public bool IsAllowed { get; set; } = true;

    public DateTime CreatedAtUtc { get; set; }
}
