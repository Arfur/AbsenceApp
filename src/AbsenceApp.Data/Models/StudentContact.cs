/*
===============================================================================
 File        : StudentContact.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-15
 Updated     : 2026-03-15
-------------------------------------------------------------------------------
 Purpose     : EF Core entity for the student_contacts table.
               Stores emergency contacts, parents, and guardians linked to a student.
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

public class StudentContact
{
    public long Id { get; set; }
    public long StudentId { get; set; }
    public string ContactName { get; set; } = default!;
    public string Relationship { get; set; } = default!;
    public string? PhoneMobile { get; set; }
    public string? PhoneHome { get; set; }
    public string? Email { get; set; }
    public int Priority { get; set; }
    public bool? LivesWithStudent { get; set; }
    public bool HasParentalResponsibility { get; set; }
    public bool? SafeguardingFlag { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
