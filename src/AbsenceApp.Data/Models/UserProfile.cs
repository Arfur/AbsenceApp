/*
===============================================================================
 File        : UserProfile.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-15
 Updated     : 2026-03-15
-------------------------------------------------------------------------------
 Purpose     : EF Core entity for the user_profiles table.
               Holds extended personal and organisational profile data for a user, including
               job title, department, school, and localisation preferences.
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

public class UserProfile
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string? PreferredName { get; set; }
    public string Title { get; set; } = default!;
    public DateOnly DateOfBirth { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public string? Bio { get; set; }
    public string? Gender { get; set; }
    public string Timezone { get; set; } = default!;
    public string LanguageCode { get; set; } = default!;
    public long DepartmentId { get; set; }
    public long JobTitleId { get; set; }
    public long SchoolId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
