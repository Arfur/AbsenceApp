/*
===============================================================================
 File        : User.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-15
 Updated     : 2026-03-15
-------------------------------------------------------------------------------
 Purpose     : EF Core entity for the users table (TABLE1).
               Central authentication and account record linking every person in the system
               to a role type and optional profile.
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
/// EF Core entity for SQL table dbo.users (TABLE1).
/// </summary>
public class User
{
    public long Id { get; set; }
    public string Name { get; set; } = default!;
    public string Username { get; set; } = default!;
    public string Email { get; set; } = default!;
    public DateTime? EmailVerifiedAt { get; set; }
    public string Password { get; set; } = default!;
    public long RoleTypeId { get; set; }
    public string Status { get; set; } = default!;
    public bool IsAdmin { get; set; }
    public string? ProfilePhotoPath { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public string? LastLoginIp { get; set; }
    public string? RememberToken { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public long? DepartmentId { get; set; }
    public string? Designation { get; set; }
    public int LoginCount { get; set; }
    public bool IsTwoFactorEnabled { get; set; }
    public string? TwoFactorSecret { get; set; }
    public string? BackupCodes { get; set; }
    public string? Timezone { get; set; }
    public string? LanguageCode { get; set; }
    public string? Bio { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }
}
