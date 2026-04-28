/*
===============================================================================
 File        : RoleDefaultPagePermission.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 1.2.0
 Created     : 2026-04-11
 Updated     : 2026-04-24
-------------------------------------------------------------------------------
 Purpose     : EF Core entity for the RoleDefaultPagePermissions table.
               Stores the default CRUD-action permission set for a given
               role × page combination. Used by PermissionServiceV2 as the
               role-level fallback when no user-specific override exists.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-11  Initial creation (E15 User Management).
   - 1.1.0  2026-04-11  Minor clarifications to notes and constraints.
   - 1.2.0  2026-04-24  EF mapping update: explicit table mapping to PascalCase
                        `RoleDefaultPagePermissions` to align with the live DB
                        schema (no underscores). Added CreatedAt audit column
                        to match DB schema and improve traceability.
-------------------------------------------------------------------------------
 Notes       :
   - RoleTypeName is a denormalised string matching RoleType.Name (not a FK)
     to avoid cascade issues and allow fast lookup by name.
   - Unique constraint: (RoleTypeName, PageId) — one row per role × page.
   - ValueGeneratedOnAdd is set in configuration; exempt from the
     ValueGeneratedNever loop via AppDbContext exclusion.
   - PageId is a FK to AppPages.Id with ON DELETE CASCADE.
===============================================================================
*/

using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AbsenceApp.Data.Models;

[Table("RoleDefaultPagePermissions")]
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

    // Audit timestamp added to align with DB schema and for traceability.
    public DateTime CreatedAt  { get; set; }
}
