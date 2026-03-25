/*
===============================================================================
 File        : StudentAbsenceAudit.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-15
 Updated     : 2026-03-15
-------------------------------------------------------------------------------
 Purpose     : EF Core entity for the student_absence_audit table.
               Field-level audit log capturing before/after values for student absence edits.
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

public class StudentAbsenceAudit
{
    public long Id { get; set; }
    public long StudentAbsenceId { get; set; }
    public long StudentId { get; set; }
    public string Action { get; set; } = default!;
    public long ChangedBy { get; set; }
    public DateTime ChangedAt { get; set; }
    public string? FieldName { get; set; }
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
    public string? Notes { get; set; }
}
