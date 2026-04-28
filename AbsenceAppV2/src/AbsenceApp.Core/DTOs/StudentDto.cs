namespace AbsenceApp.Core.DTOs;

public class StudentDto
{
    public int Id { get; set; }
    public string AdmissionNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleNames { get; set; }
    public string LastName { get; set; } = string.Empty;
    public string? LegalFirstName { get; set; }
    public string? LegalLastName { get; set; }
    public string? PreferredName { get; set; }
    public string? Gender { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public int YearGroupId { get; set; }
    public int? ClassId { get; set; }
    public int? HouseId { get; set; }
    public string? Username { get; set; }
    public string? Upn { get; set; }
    public int? SchoolId { get; set; }
    public DateOnly AdmissionDate { get; set; }
    public string? Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public string FullName => $"{FirstName} {LastName}".Trim();
}
