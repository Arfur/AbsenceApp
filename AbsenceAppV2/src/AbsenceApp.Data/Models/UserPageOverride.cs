/*
===============================================================================
 File        : UserPageOverride.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 1.2.0
 Created     : 2026-04-11
 Updated     : 2026-04-24
-------------------------------------------------------------------------------
 Purpose     : EF Core entity for the UserPageOverrides table.
               Stores explicit Grant/Deny override entries per user+page.
               Overrides are checked before role defaults and user-specific
               permission rows.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-11  Initial creation (E15 User Management).
   - 1.2.0  2026-04-24  EF mapping update: explicit table mapping to PascalCase
                        `UserPageOverrides`. Added CreatedAt audit timestamp
                        to match the database schema.
-------------------------------------------------------------------------------
 Notes       :
   - OverrideType values: 'Grant' or 'Deny'.
   - Unique constraint: (UserId, PageId).
   - PageId FK references AppPages.Id with ON DELETE CASCADE.
===============================================================================
*/

using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AbsenceApp.Data.Models;

[Table("UserPageOverrides")]
public class UserPageOverride
{
    public int    Id           { get; set; }
    public long   UserId       { get; set; }
    public int    PageId       { get; set; }
    public string OverrideType { get; set; } = default!; // 'Grant' or 'Deny'

    // Audit timestamp to match DB schema
    public DateTime CreatedAt  { get; set; }
}
