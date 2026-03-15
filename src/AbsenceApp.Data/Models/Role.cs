/*
===============================================================================
 File        : Role.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-15
 Updated     : 2026-03-15
-------------------------------------------------------------------------------
 Purpose     : Legacy stub entity retained for backward compatibility only.
               Active role logic uses RoleType. Legacy configurations rely on this class.
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

// Legacy stub retained so RoleConfiguration.cs still compiles.
// Active role-type data is now in RoleType.cs (TABLE7).
public class Role
{
    public int RoleId { get; set; }
    public string RoleName { get; set; } = default!;
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
