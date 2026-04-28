/*
===============================================================================
 File        : RoleFeature.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 2.0.0
 Created     : 2026-04-05
 Updated     : 2026-04-19
-------------------------------------------------------------------------------
 Purpose     : EF Core entity representing default entitlements assigned to a
               role type. Phase 2 — Entitlement-driven navigation and feature
               control.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-05  Initial implementation. Created in AbsenceApp.Data.Models
                         so AppDbContext can configure the entity without
                         creating a circular dependency on AbsenceApp.Api.
   - 2.0.0  2026-04-19  Schema alignment: renamed RoleFeatureId→Id, RoleType→RoleId,
                         FeatureId (int FK)→FeatureCode (string), IsAllowed→IsEnabled,
                         CreatedAtUtc→AssignedAt. Added AssignedBy. Removed Feature
                         navigation property (join is by code, not FK).
                         Table renamed role_features→rolefeature.
-------------------------------------------------------------------------------
 Notes       :
   - RoleId aligns with roles.Id in the roles table.
   - FeatureCode aligns with feature.Code (string code, no FK constraint).
   - Configured via EntitlementsModelBuilderExtensions.ConfigureEntitlements().
===============================================================================
*/

namespace AbsenceApp.Data.Models;

public sealed class RoleFeature
{
    public int Id { get; set; }

    public int RoleId { get; set; }

    public string FeatureCode { get; set; } = string.Empty;

    public bool IsEnabled { get; set; } = true;

    public DateTime AssignedAt { get; set; }

    public int AssignedBy { get; set; }
}
