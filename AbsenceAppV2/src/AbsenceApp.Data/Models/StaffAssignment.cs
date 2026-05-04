/*
===============================================================================
 File        : StaffAssignment.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-05-04
 Updated     : 2026-05-04
-------------------------------------------------------------------------------
 Purpose     : EF Core entity for the staffassignments table.
               Links a staff member to a location for a defined period.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-05-04  Initial creation. Stub model required because
                         AppDbContext.cs references this type and the
                         staffassignments table is confirmed in the live DB.
                         Properties derived from INFORMATION_SCHEMA query
                         and AppDbContext column-type configuration.
-------------------------------------------------------------------------------
 Notes       :
   - No navigation properties; FK integrity is enforced at the database layer.
   - All required string properties use = default! to satisfy the nullable compiler.
===============================================================================
*/
namespace AbsenceApp.Data.Models;

public class StaffAssignment
{
    public int Id { get; set; }
    public int StaffId { get; set; }
    public int LocationId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
