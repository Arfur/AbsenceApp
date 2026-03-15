/*
===============================================================================
 File        : UserRole.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-15
 Updated     : 2026-03-15
-------------------------------------------------------------------------------
 Purpose     : Legacy stub entity retained for backward compatibility only.
               Active role assignment is handled via User.RoleTypeId and RoleType.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-15  Initial creation.
-------------------------------------------------------------------------------
 Notes       :
   - All properties mirror the original schema; do not add new properties.
   - Remove this stub once all legacy references have been migrated.
===============================================================================
*/
namespace AbsenceApp.Data.Models;

// Legacy stub retained so UserRoleConfiguration.cs still compiles.
public class UserRole
{
    public int UserId { get; set; }
    public int RoleId { get; set; }
    public User User { get; set; } = default!;
    public Role Role { get; set; } = default!;
}
