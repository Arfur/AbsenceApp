/*
===============================================================================
 File        : AttendanceRegister.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-15
 Updated     : 2026-03-15
-------------------------------------------------------------------------------
 Purpose     : EF Core entity for the attendance_registers table.
               Represents a single session register (AM or PM) opened for a class
               on a given date.
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

public class AttendanceRegister
{
    public long Id { get; set; }
    public long ClassId { get; set; }
    public DateOnly Date { get; set; }
    public string Session { get; set; } = default!;
    public long OpenedBy { get; set; }
    public long? ClosedBy { get; set; }
    public string Status { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
