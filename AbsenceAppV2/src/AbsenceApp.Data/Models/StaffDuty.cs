/*
===============================================================================
 File        : StaffAssignment.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 2.0.0
 Created     : 2026-03-15
 Updated     : 2026-04-27
-------------------------------------------------------------------------------
 Purpose     : EF Core entity for the staff_assignments table.
               Represents a staff member’s location assignment for a defined
               period. This model now accurately reflects the database schema.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-15  Initial creation.
   - 2.0.0  2026-04-27  Corrected model to match actual MySQL table:
                        • Removed invalid fields (JobTitleId, JobGroupId,
                          DepartmentId, ClassId, ResponsibilityId, DaysOfWeek)
                        • Added LocationId (int)
                        • Updated property types to match schema
                        • Updated header metadata
-------------------------------------------------------------------------------
 Notes       :
   - No navigation properties; FK integrity is enforced at the database layer.
   - All required string properties use nullable reference types where appropriate.
===============================================================================
*/

namespace AbsenceApp.Data.Models;

public class StaffDuty
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
