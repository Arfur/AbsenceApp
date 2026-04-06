/*
===============================================================================
 File        : UserFeatureOverride.cs
 Namespace   : AbsenceApp.Api.Data.Entities
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-04-05
 Updated     : 2026-04-05
-------------------------------------------------------------------------------
 Purpose     : Represents per-user entitlement overrides.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-05  Initial implementation.
-------------------------------------------------------------------------------
 Notes       :
   - User overrides are authoritative over role defaults.
===============================================================================
*/

namespace AbsenceApp.Api.Data.Entities;

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
