/*
===============================================================================
 File        : RoleFeature.cs
 Namespace   : AbsenceApp.Api.Data.Entities
 Author      : Michael
 Version     : 2.0.0
 Created     : 2026-04-05
 Updated     : 2026-04-19
-------------------------------------------------------------------------------
 Purpose     : Represents default entitlements assigned to a role.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-05  Initial implementation.
   - 2.0.0  2026-04-19  Schema alignment: renamed RoleFeatureId→Id, RoleType→RoleId,
                         FeatureId (int FK)→FeatureCode (string), IsAllowed→IsEnabled,
                         CreatedAtUtc→AssignedAt. Added AssignedBy. Removed Feature
                         navigation property.
===============================================================================
*/

namespace AbsenceApp.Api.Data.Entities;

public sealed class RoleFeature
{
    public int Id { get; set; }

    public int RoleId { get; set; }

    public string FeatureCode { get; set; } = string.Empty;

    public bool IsEnabled { get; set; } = true;

    public DateTime AssignedAt { get; set; }

    public long? AssignedBy { get; set; }
}
