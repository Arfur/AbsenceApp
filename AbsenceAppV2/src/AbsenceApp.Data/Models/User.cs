/*
===============================================================================
 File        : User.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 1.3.0
 Created     : 2026-03-15
 Updated     : 2026-04-19
-------------------------------------------------------------------------------
 Purpose     : EF Core entity for the users table (TABLE1).
               Central authentication and account record linking every person in the system
               to a role type and optional profile.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-15  Initial creation.
   - 1.1.0  2026-04-11  E15 User Management: added FirstName and LastName
                         columns. Name is retained for backward compatibility
                         with existing queries (AuthService, etc.).
   - 1.2.0  2026-04-19  RoleId resolution fix: marked RoleTypeId as [NotMapped]
                         so EF Core no longer includes it in any SQL SELECT,
                         INSERT or UPDATE. Role resolution now goes via the
                         userrole table (users → userrole → roles → roletypes).
                         Fixes MySqlException "Unknown column 'u.RoleTypeId'".
   - 1.3.0  2026-04-19  Full CSV schema sweep: marked 14 non-existent columns
                         as [NotMapped] (Name, FirstName, LastName, ProfilePhotoPath,
                         PhoneNumber, DepartmentId, Designation, Bio, DateOfBirth,
                         Gender, Address, City, Country, PostalCode).
                         Fixes MySqlException "Unknown column 'u.Address'" and
                         prevents future errors from all other invalid fields.
-------------------------------------------------------------------------------
   - No navigation properties; FK integrity is enforced at the database layer.
   - All required string properties use = default! to satisfy the nullable compiler.
   - Passwords are stored as PBKDF2 hashes (format: "<iterations>:<salt>:<hash>")
     for new users created via UserManagementService.
===============================================================================
*/
using System.ComponentModel.DataAnnotations.Schema;
namespace AbsenceApp.Data.Models;

/// <summary>
/// EF Core entity for SQL table dbo.users (TABLE1).
/// </summary>
public class User
{
    public long   Id        { get; set; }
    /// <summary>Not in users table — retained for ViewModel compatibility only.</summary>
    [NotMapped]
    public string Name      { get; set; } = default!;
    /// <summary>Not in users table — retained for ViewModel compatibility only.</summary>
    [NotMapped]
    public string FirstName { get; set; } = string.Empty;
    /// <summary>Not in users table — retained for ViewModel compatibility only.</summary>
    [NotMapped]
    public string LastName  { get; set; } = string.Empty;
    public string Username  { get; set; } = default!;
    public string Email { get; set; } = default!;
    public DateTime? EmailVerifiedAt { get; set; }
    public string Password { get; set; } = default!;
    /// <summary>
    /// Not mapped to DB — role assignment is via the userrole table.
    /// Retained as a C# property for ViewModel/DTO compatibility only.
    /// </summary>
    [NotMapped]
    public long RoleTypeId { get; set; }
    public string Status { get; set; } = default!;
    public bool IsAdmin { get; set; }
    /// <summary>Not in users table — retained for ViewModel compatibility only.</summary>
    [NotMapped]
    public string? ProfilePhotoPath { get; set; }
    /// <summary>Not in users table — retained for ViewModel compatibility only.</summary>
    [NotMapped]
    public string? PhoneNumber { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public string? LastLoginIp { get; set; }
    public string? RememberToken { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    /// <summary>Not in users table — retained for ViewModel compatibility only.</summary>
    [NotMapped]
    public long? DepartmentId { get; set; }
    /// <summary>Not in users table — retained for ViewModel compatibility only.</summary>
    [NotMapped]
    public string? Designation { get; set; }
    public int LoginCount { get; set; }
    public bool IsTwoFactorEnabled { get; set; }
    public string? TwoFactorSecret { get; set; }
    public string? BackupCodes { get; set; }
    public string? Timezone { get; set; }
    public string? LanguageCode { get; set; }
    /// <summary>Not in users table — retained for ViewModel compatibility only.</summary>
    [NotMapped]
    public string? Bio { get; set; }
    /// <summary>Not in users table — retained for ViewModel compatibility only.</summary>
    [NotMapped]
    public DateOnly? DateOfBirth { get; set; }
    /// <summary>Not in users table — retained for ViewModel compatibility only.</summary>
    [NotMapped]
    public string? Gender { get; set; }
    /// <summary>Not in users table — retained for ViewModel compatibility only.</summary>
    [NotMapped]
    public string? Address { get; set; }
    /// <summary>Not in users table — retained for ViewModel compatibility only.</summary>
    [NotMapped]
    public string? City { get; set; }
    /// <summary>Not in users table — retained for ViewModel compatibility only.</summary>
    [NotMapped]
    public string? Country { get; set; }
    /// <summary>Not in users table — retained for ViewModel compatibility only.</summary>
    [NotMapped]
    public string? PostalCode { get; set; }
}
