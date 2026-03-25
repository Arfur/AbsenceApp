/*
===============================================================================
 File        : StaffAssignment.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-15
 Updated     : 2026-03-15
-------------------------------------------------------------------------------
 Purpose     : EF Core entity for the staff_assignments table.
               Links a staff member to a job title, group, department, and optionally
               a class and responsibility for a defined period.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-15  Initial creation.
-------------------------------------------------------------------------------
 Notes       :
   - No navigation properties; FK integrity is enforced at the database layer.
   - All required string properties use = default! to satisfy the nullable compiler.
===============================================================================
*/
namespace AbsenceApp.Data.Models;

public class StaffAssignment
{
    public long Id { get; set; }
    public long StaffId { get; set; }
    public long JobTitleId { get; set; }
    public long JobGroupId { get; set; }
    public long DepartmentId { get; set; }
    public long? ClassId { get; set; }
    public long? ResponsibilityId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string? DaysOfWeek { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
