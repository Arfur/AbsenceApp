/*
===============================================================================
 File        : StaffPhase.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-05-04
 Updated     : 2026-05-04
-------------------------------------------------------------------------------
 Purpose     : EF Core entity for the staffphases table.
               Represents a curriculum phase or key stage associated with staff.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-05-04  Initial creation. Stub model required because
                         AppDbContext.cs references this type and the
                         staffphases table is confirmed in the live DB.
                         Properties derived from INFORMATION_SCHEMA query
                         and AppDbContext column-type configuration.
-------------------------------------------------------------------------------
 Notes       :
   - No navigation properties; FK integrity is enforced at the database layer.
   - All required string properties use = default! to satisfy the nullable compiler.
===============================================================================
*/
namespace AbsenceApp.Data.Models;

public class StaffPhase
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Code { get; set; } = default!;
    public string? Description { get; set; }
    public int DisplayOrder { get; set; }
}
