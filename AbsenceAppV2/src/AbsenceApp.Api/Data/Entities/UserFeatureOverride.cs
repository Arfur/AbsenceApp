/*
===============================================================================
 File        : UserFeatureOverride.cs
 Namespace   : AbsenceApp.Api.Data.Entities
 Author      : Michael
 Version     : 2.0.0
 Created     : 2026-04-05
 Updated     : 2026-04-19
-------------------------------------------------------------------------------
 Purpose     : Represents per-user entitlement overrides.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-05  Initial implementation.
   - 2.0.0  2026-04-19  Schema alignment: renamed UserFeatureOverrideId→Id,
                         FeatureId (int FK)→FeatureCode (string), IsAllowed→IsEnabled,
                         UpdatedAtUtc→OverriddenAt, UpdatedByUserId→OverriddenBy.
                         Removed Reason and Feature navigation property.
-------------------------------------------------------------------------------
 Notes       :
   - User overrides are authoritative over role defaults.
===============================================================================
*/

namespace AbsenceApp.Api.Data.Entities;

public sealed class UserFeatureOverride
{
    public int Id { get; set; }

    public Guid UserId { get; set; }

    public string FeatureCode { get; set; } = string.Empty;

    public bool IsEnabled { get; set; }

    public DateTime OverriddenAt { get; set; }

    public long? OverriddenBy { get; set; }
}
