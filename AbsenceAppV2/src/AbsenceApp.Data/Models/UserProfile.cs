/*
===============================================================================
 File        : UserProfile.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 1.5.0
 Created     : 2026-03-15
 Updated     : 2026-04-25
-------------------------------------------------------------------------------
 Purpose     : EF Core entity for the userprofiles table.
               Holds extended personal and organisational profile data for a
               user, including job title, department, school, and localisation
               preferences.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-15  Initial creation.
   - 1.1.0  2026-04-22  Added explicit [Table("userprofiles")] mapping to
                        prevent EF from merging UserProfile into Users table.
                        Replaced DateOnly with DateTime to ensure correct
                        MySQL DATE column mapping and avoid shadow property
                        generation.
   - 1.2.0  2026-04-24  Session 7 fix-phase Task B: re-validated [Table(
                        "userprofiles")] attribute present on class. Confirmed
                        AppDbContext.UserProfiles => Set<UserProfile>() maps to
                        the correct table. DB rename (UserProfiles → userprofiles)
                        handled by scripts/E33_E15PermissionTables.sql Step 0.
                        No C# code change required; header version incremented
                        as evidence of the mandatory validation pass.
   - 1.3.0  2026-04-25  Session 9: removed ToTable("UserProfiles") override
                        from AppDbContext (v2.0.2). SQL still errored — see 1.4.0.
   - 1.4.0  2026-04-25  Session 9 corrected fix: reverted [Table] attribute
                        back to "userprofiles" (no underscore). The DB table is
                        "userprofiles" — MySQL on Windows stores all names
                        lowercase, so E33 Step 0 RENAME TABLE UserProfiles TO
                        user_profiles never executed (case-sensitive check
                        against information_schema returned 0 rows). The table
                        was never renamed. [Table("user_profiles")] was wrong.
   - 1.5.0  2026-04-25  Session 9 final fix: removed DateOfBirth property.
                        DateOfBirth belongs to the Staff table, not userprofiles.
                        EF was generating SQL 'u.DateOfBirth' against userprofiles
                        which has no such column → MySqlException on every profile
                        load → GetUserProfileHeaderAsync returned null → "User not
                        found". DateOfBirth is now loaded from Staff in
                        GetUserProfileDetailAsync (UserManagementService v1.7.0).
-------------------------------------------------------------------------------
 Notes       :
   - No navigation properties; FK integrity is enforced at the database layer.
   - All required string properties use = default! to satisfy the nullable
     compiler.
===============================================================================
*/

using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AbsenceApp.Data.Models;

[Table("userprofiles")]
public class UserProfile
{
    public int Id { get; set; }
    public int UserId { get; set; }

    public string? ProfilePictureUrl { get; set; }
    public string? Bio { get; set; }

    public string Timezone { get; set; } = default!;
    public string LanguageCode { get; set; } = default!;

    /// <summary>Display name preference.</summary>
    public string DisplayName { get; set; } = default!;
    /// <summary>UI theme preference, e.g. "light" or "dark".</summary>
    public string ThemePreference { get; set; } = default!;
    /// <summary>Accent colour hex/name.</summary>
    public string? AccentColor { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}


