/*
===============================================================================
 File        : AttendanceMark.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-15
 Updated     : 2026-03-15
-------------------------------------------------------------------------------
 Purpose     : EF Core entity for the attendance_marks table.
               Records the mark code (present, absent, late, etc.) given to each
               student within a specific attendance register session.
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

public class AttendanceMark
{
    public long Id { get; set; }
    public long AttendanceRegisterId { get; set; }
    public long StudentId { get; set; }
    public string MarkCode { get; set; } = default!;
    public bool? IsLate { get; set; }
    public int? MinutesLate { get; set; }
    public string? Notes { get; set; }
    public long RecordedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
