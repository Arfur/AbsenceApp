/*
===============================================================================
 File        : JobGroup.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 1.1.0
 Created     : 2026-03-15
 Updated     : 2026-05-10
-------------------------------------------------------------------------------
 Purpose     : EF Core entity for the job_groups lookup table.
               Broad salary-band groupings for staff roles.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-15  Initial creation.
   - 1.1.0  2026-05-10  Removed stale TypicalMembers mapping; runtime schema
                        does not expose this column and staff list/profile
                        lookups must not project it.
-------------------------------------------------------------------------------
 Notes       :
   - No navigation properties; FK integrity is enforced at the database layer.
   - All required string properties use = default! to satisfy the nullable compiler.
===============================================================================
*/
namespace AbsenceApp.Data.Models;

public class JobGroup
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
