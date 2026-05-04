/*
===============================================================================
 File        : StaffAttribute.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-05-04
 Updated     : 2026-05-04
-------------------------------------------------------------------------------
 Purpose     : EF Core entity for the staffattributes table.
               Stores named attribute values assigned to staff members.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-05-04  Initial creation. Stub model required because
                         AppDbContext.cs references this type and the
                         staffattributes table is confirmed in the live DB.
                         Properties derived from INFORMATION_SCHEMA query
                         and AppDbContext column-type configuration.
-------------------------------------------------------------------------------
 Notes       :
   - No navigation properties; FK integrity is enforced at the database layer.
   - AttributeTypeId maps to int unsigned in the DB (configured via HasColumnType
     in AppDbContext). Stored as int in C# for consistency with other FK columns.
===============================================================================
*/
namespace AbsenceApp.Data.Models;

public class StaffAttribute
{
    public int Id { get; set; }
    public int AttributeTypeId { get; set; }
    public string Name { get; set; } = default!;
    public string? Code { get; set; }
    public string? Description { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
