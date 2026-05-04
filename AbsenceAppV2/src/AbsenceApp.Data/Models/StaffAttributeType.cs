/*
===============================================================================
 File        : StaffAttributeType.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-05-04
 Updated     : 2026-05-04
-------------------------------------------------------------------------------
 Purpose     : EF Core entity for the staffattributetypes lookup table.
               Defines the types of attributes that can be assigned to staff.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-05-04  Initial creation. Stub model required because
                         AppDbContext.cs references this type and the
                         staffattributetypes table is confirmed in the live DB.
                         Properties derived from INFORMATION_SCHEMA query
                         and AppDbContext column-type configuration.
-------------------------------------------------------------------------------
 Notes       :
   - No navigation properties; FK integrity is enforced at the database layer.
   - Id maps to int unsigned in the DB (configured via HasColumnType in
     AppDbContext). Stored as int in C# for consistency with other PK columns.
===============================================================================
*/
namespace AbsenceApp.Data.Models;

public class StaffAttributeType
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
}
