namespace AbsenceApp.Core.DTOs;

public class StudentDto
{
    public long Id { get; set; }
    public string AdmissionNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleNames { get; set; }
    public string LastName { get; set; } = string.Empty;
    public string LegalFirstName { get; set; } = string.Empty;
    public string LegalLastName { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public long YearGroupId { get; set; }
    public long ClassId { get; set; }
    public long? HouseId { get; set; }
    public string? Username { get; set; }
    public string? Upn { get; set; }
    public long SchoolId { get; set; }
    public DateOnly AdmissionDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public string FullName => $"{FirstName} {LastName}".Trim();
}
