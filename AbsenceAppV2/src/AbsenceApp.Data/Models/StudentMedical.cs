/*
===============================================================================
 File        : StudentMedical.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-15
 Updated     : 2026-03-15
-------------------------------------------------------------------------------
 Purpose     : EF Core entity for the student_medical table.
               Documents medical conditions and severity levels for a student.
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

public class StudentMedical
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public string MedicalCondition { get; set; } = default!;
    public bool IsAllergic { get; set; }
    public string? AllergyDetails { get; set; }
    public string? Medication { get; set; }
    public string? EmergencyActionPlan { get; set; }
    public int RecordedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
