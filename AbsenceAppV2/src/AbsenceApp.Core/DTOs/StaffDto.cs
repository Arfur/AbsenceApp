namespace AbsenceApp.Core.DTOs;

public class StaffDto
{
    public int Id { get; set; }
    public string StaffNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? PreferredName { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string WorkEmail { get; set; } = string.Empty;
    public string? AltEmail { get; set; }
    public string? PhoneHome { get; set; }
    public string? PhoneMobile { get; set; }
    public string? PhoneEmergency { get; set; }
    public string EmploymentType { get; set; } = string.Empty;
    public string ContractType { get; set; } = string.Empty;
    public DateOnly HireDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string WorkLocation { get; set; } = string.Empty;
    public int JobTitleId { get; set; }
    public int JobGroupId { get; set; }
    public int DepartmentId { get; set; }
    public int? ReportingManagerId { get; set; }
    public string? ProfilePhotoUrl { get; set; }
    public string AccountStatus { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public string FullName => $"{FirstName} {LastName}".Trim();
    public bool IsActive => AccountStatus == "Active";
}
