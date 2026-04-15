/*
===============================================================================
 File        : RoleDefaultPagePermission.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-04-11
 Updated     : 2026-04-11
-------------------------------------------------------------------------------
 Purpose     : EF Core entity for the role_default_page_permissions table.
               Stores the default CRUD-action permission set for a given
               role × page combination. Used by PermissionServiceV2 as the
               role-level fallback when no user-specific override exists.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-11  Initial creation (E15 User Management).
-------------------------------------------------------------------------------
 Notes       :
   - RoleTypeName is a denormalised string matching RoleType.Name (not a FK)
     to avoid cascade issues and allow fast lookup by name.
   - Unique constraint: (RoleTypeName, PageId) — one row per role × page.
   - ValueGeneratedOnAdd is set in configuration; exempt from the
     ValueGeneratedNever loop via AppDbContext exclusion.
===============================================================================
*/
namespace AbsenceApp.Data.Models;

public class RoleDefaultPagePermission
{
    public int    Id           { get; set; }
    public string RoleTypeName { get; set; } = default!;
    public int    PageId       { get; set; }
    public bool   CanRead      { get; set; }
    public bool   CanWrite     { get; set; }
    public bool   CanCreate    { get; set; }
    public bool   CanDelete    { get; set; }
    public bool   CanImport    { get; set; }
    public bool   CanExport    { get; set; }
}
