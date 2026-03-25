namespace AbsenceApp.Core.DTOs;

public class StaffDto
{
    public long Id { get; set; }
    public string StaffNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? PreferredName { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string WorkEmail { get; set; } = string.Empty;
    public string? AltEmail { get; set; }
    public string? PhoneMobile { get; set; }
    public string EmploymentType { get; set; } = string.Empty;
    public string ContractType { get; set; } = string.Empty;
    public DateOnly HireDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string WorkLocation { get; set; } = string.Empty;
    public long JobTitleId { get; set; }
    public long JobGroupId { get; set; }
    public long DepartmentId { get; set; }
    public string? ProfilePhotoUrl { get; set; }
    public string AccountStatus { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public string FullName => $"{FirstName} {LastName}".Trim();
    public bool IsActive => AccountStatus == "Active";
}
