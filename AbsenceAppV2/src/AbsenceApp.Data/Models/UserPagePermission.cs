/*
===============================================================================
 File        : UserPagePermission.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-04-11
 Updated     : 2026-04-11
-------------------------------------------------------------------------------
 Purpose     : EF Core entity for the user_page_permissions table.
               Stores an explicit per-user, per-page CRUD-action permission set.
               When a row exists for (UserId, PageId), PermissionServiceV2
               returns these values directly (highest priority — overrides role
               defaults).
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-11  Initial creation (E15 User Management).
-------------------------------------------------------------------------------
 Notes       :
   - Unique constraint: (UserId, PageId) — one row per user × page.
   - ValueGeneratedOnAdd is set in configuration; exempt from the
     ValueGeneratedNever loop via AppDbContext exclusion.
===============================================================================
*/
namespace AbsenceApp.Data.Models;

public class UserPagePermission
{
    public int  Id        { get; set; }
    public long UserId    { get; set; }
    public int  PageId    { get; set; }
    public bool CanRead   { get; set; }
    public bool CanWrite  { get; set; }
    public bool CanCreate { get; set; }
    public bool CanDelete { get; set; }
    public bool CanImport { get; set; }
    public bool CanExport { get; set; }
}
