/*
===============================================================================
 File        : UserFeatureOverride.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 2.0.0
 Created     : 2026-04-05
 Updated     : 2026-04-19
-------------------------------------------------------------------------------
 Purpose     : EF Core entity representing per-user entitlement overrides.
               User overrides are authoritative over role defaults.
               Phase 2 — Entitlement-driven navigation and feature control.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-05  Initial implementation. Created in AbsenceApp.Data.Models
                         so AppDbContext can configure the entity without
                         creating a circular dependency on AbsenceApp.Api.
   - 2.0.0  2026-04-19  Schema alignment: renamed UserFeatureOverrideId→Id,
                         FeatureId (int FK)→FeatureCode (string), IsAllowed→IsEnabled,
                         UpdatedAtUtc→OverriddenAt, UpdatedByUserId→OverriddenBy.
                         Removed Reason (not in CSV schema) and Feature navigation
                         property (join is by code, not FK).
                         Table renamed user_feature_overrides→userfeatureoverride.
-------------------------------------------------------------------------------
 Notes       :
   - Configured via EntitlementsModelBuilderExtensions.ConfigureEntitlements().
===============================================================================
*/

namespace AbsenceApp.Data.Models;

public sealed class UserFeatureOverride
{
    public int Id { get; set; }

    public Guid UserId { get; set; }

    public string FeatureCode { get; set; } = string.Empty;

    public bool IsEnabled { get; set; }

    public DateTime OverriddenAt { get; set; }

    public long? OverriddenBy { get; set; }
}
