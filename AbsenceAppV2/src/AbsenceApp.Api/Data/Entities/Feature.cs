/*
===============================================================================
 File        : Feature.cs
 Namespace   : AbsenceApp.Api.Data.Entities
 Author      : Michael
 Version     : 2.0.0
 Created     : 2026-04-05
 Updated     : 2026-04-19
-------------------------------------------------------------------------------
 Purpose     : Represents a single entitlement feature key.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-05  Initial implementation.
   - 2.0.0  2026-04-19  Schema alignment: renamed FeatureId→Id, Key→Code,
                         IsActive→IsEnabled, CreatedAtUtc→CreatedAt.
                         Added DisplayName and UpdatedAt to match CSV schema.
-------------------------------------------------------------------------------
 Notes       :
   - Feature codes must be stable and unique.
===============================================================================
*/

namespace AbsenceApp.Api.Data.Entities;

public sealed class Feature
{
    public int Id { get; set; }

    public string Code { get; set; } = string.Empty;

    public string? DisplayName { get; set; }

    public string? Description { get; set; }

    public bool IsEnabled { get; set; } = true;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
