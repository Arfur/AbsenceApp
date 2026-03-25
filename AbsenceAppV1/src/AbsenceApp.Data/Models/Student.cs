/*
===============================================================================
 File        : Student.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-15
 Updated     : 2026-03-15
-------------------------------------------------------------------------------
 Purpose     : EF Core entity for the students table (TABLE29).
               Stores all admission, personal, and organisational details for each student.
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

/// <summary>
/// EF Core entity for the SQL 'students' table (TABLE29).
/// Distinct from AbsenceApp.Core.Models.Student which is the application-layer DTO.
/// </summary>
public class Student
{
    public long Id { get; set; }
    public string AdmissionNumber { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string? MiddleNames { get; set; }
    public string LastName { get; set; } = default!;
    public string LegalFirstName { get; set; } = default!;
    public string LegalLastName { get; set; } = default!;
    public string Gender { get; set; } = default!;
    public DateOnly DateOfBirth { get; set; }
    public long YearGroupId { get; set; }
    public long ClassId { get; set; }
    public long? HouseId { get; set; }
    public string? Username { get; set; }
    public string? Upn { get; set; }
    public long SchoolId { get; set; }
    public DateOnly AdmissionDate { get; set; }
    public string Status { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
