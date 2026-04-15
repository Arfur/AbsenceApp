/*
===============================================================================
 File        : UserPageOverride.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-04-11
 Updated     : 2026-04-11
-------------------------------------------------------------------------------
 Purpose     : EF Core entity for the user_page_overrides table.
               Records explicit Grant or Deny overrides for a given user × page
               combination. Checked by PermissionServiceV2 before falling back
               to RoleDefaultPagePermissions.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-11  Initial creation (E15 User Management).
-------------------------------------------------------------------------------
 Notes       :
   - OverrideType: "Grant" or "Deny" (enforced by EF max-length + code).
   - Unique constraint: (UserId, PageId) — one override row per user × page.
   - ValueGeneratedOnAdd is set in configuration; exempt from the
     ValueGeneratedNever loop via AppDbContext exclusion.
===============================================================================
*/
namespace AbsenceApp.Data.Models;

public class UserPageOverride
{
    public int    Id           { get; set; }
    public long   UserId       { get; set; }
    public int    PageId       { get; set; }
    /// <summary>"Grant" or "Deny"</summary>
    public string OverrideType { get; set; } = default!;
}
