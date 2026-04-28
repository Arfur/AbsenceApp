/*
===============================================================================
 File        : UserPagePermission.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 1.2.0
 Created     : 2026-04-11
 Updated     : 2026-04-24
-------------------------------------------------------------------------------
 Purpose     : EF Core entity for the UserPagePermissions table.
               Stores per-user explicit CRUD flags for AppPages. When present,
               this row overrides role defaults for the given (UserId, PageId).
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-11  Initial creation (E15 User Management).
   - 1.2.0  2026-04-24  EF mapping update: explicit table mapping to PascalCase
                        `UserPagePermissions`. Added CreatedAt audit timestamp
                        to match the database schema.
-------------------------------------------------------------------------------
 Notes       :
   - One row per (UserId, PageId). Unique constraint enforced in DB.
   - PageId FK references AppPages.Id with ON DELETE CASCADE.
   - ValueGeneratedOnAdd is set in configuration; exempt from the
     ValueGeneratedNever loop via AppDbContext exclusion.
===============================================================================
*/

using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AbsenceApp.Data.Models;

[Table("UserPagePermissions")]
public class UserPagePermission
{
    public int    Id        { get; set; }
    public long   UserId    { get; set; }
    public int    PageId    { get; set; }

    public bool   CanRead   { get; set; }
    public bool   CanWrite  { get; set; }
    public bool   CanCreate { get; set; }
    public bool   CanDelete { get; set; }
    public bool   CanImport { get; set; }
    public bool   CanExport { get; set; }

    // Audit timestamp to match DB schema
    public DateTime CreatedAt { get; set; }
}
