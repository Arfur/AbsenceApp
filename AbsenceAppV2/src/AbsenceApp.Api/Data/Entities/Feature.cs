/*
===============================================================================
 File        : Feature.cs
 Namespace   : AbsenceApp.Api.Data.Entities
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-04-05
 Updated     : 2026-04-05
-------------------------------------------------------------------------------
 Purpose     : Represents a single entitlement feature key.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-05  Initial implementation.
-------------------------------------------------------------------------------
 Notes       :
   - Feature keys must be stable and unique.
===============================================================================
*/

namespace AbsenceApp.Api.Data.Entities;

public sealed class Feature
{
    public int FeatureId { get; set; }

    public string Key { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAtUtc { get; set; }
}
