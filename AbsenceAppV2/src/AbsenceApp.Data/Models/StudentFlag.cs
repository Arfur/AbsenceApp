/*
===============================================================================
 File        : StudentFlag.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-15
 Updated     : 2026-03-15
-------------------------------------------------------------------------------
 Purpose     : EF Core entity for the student_flags table.
               Records time-bounded flags such as SEN, FSM, PP, or LAC designations.
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

public class StudentFlag
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public string FlagCode { get; set; } = default!;
    public bool IsActive { get; set; }
    public string? Notes { get; set; }
    public DateTime AssignedAt { get; set; }
    public int AssignedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
