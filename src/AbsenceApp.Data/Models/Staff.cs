/*
===============================================================================
 File        : Staff.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-15
 Updated     : 2026-03-15
-------------------------------------------------------------------------------
 Purpose     : EF Core entity for the staff table.
               Stores all employment, contact, and organisational details for each member
               of staff.
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

public class Staff
{
    public long Id { get; set; }
    public string StaffNumber { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string? PreferredName { get; set; }
    public string Title { get; set; } = default!;
    public DateOnly DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string WorkEmail { get; set; } = default!;
    public string? AltEmail { get; set; }
    public string? PhoneHome { get; set; }
    public string? PhoneMobile { get; set; }
    public string? PhoneEmergency { get; set; }
    public string EmploymentType { get; set; } = default!;
    public string ContractType { get; set; } = default!;
    public DateOnly HireDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string WorkLocation { get; set; } = default!;
    public long? ReportingManagerId { get; set; }
    public long JobTitleId { get; set; }
    public long JobGroupId { get; set; }
    public long DepartmentId { get; set; }
    public string? ProfilePhotoUrl { get; set; }
    public string AccountStatus { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
