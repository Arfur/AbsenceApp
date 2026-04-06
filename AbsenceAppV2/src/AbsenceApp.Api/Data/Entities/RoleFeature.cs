/*
===============================================================================
 File        : RoleFeature.cs
 Namespace   : AbsenceApp.Api.Data.Entities
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-04-05
 Updated     : 2026-04-05
-------------------------------------------------------------------------------
 Purpose     : Represents default entitlements assigned to a role.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-05  Initial implementation.
-------------------------------------------------------------------------------
 Notes       :
   - RoleType must align with the existing RoleType source of truth.
===============================================================================
*/

namespace AbsenceApp.Api.Data.Entities;

public sealed class RoleFeature
{
    public int RoleFeatureId { get; set; }

    public int RoleType { get; set; }

    public int FeatureId { get; set; }
    public Feature? Feature { get; set; }

    public bool IsAllowed { get; set; } = true;

    public DateTime CreatedAtUtc { get; set; }
}
