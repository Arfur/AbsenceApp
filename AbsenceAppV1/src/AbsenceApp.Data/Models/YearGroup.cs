/*
===============================================================================
 File        : YearGroup.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-15
 Updated     : 2026-03-15
-------------------------------------------------------------------------------
 Purpose     : EF Core entity for the year_groups table.
               Represents a curriculum year group within a phase and school.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-15  Initial creation.
-------------------------------------------------------------------------------
 Notes       :
   - No navigation properties; FK integrity is enforced at the database layer.
   - All required string properties use = default! to satisfy the nullable compiler.
===============================================================================
*/
namespace AbsenceApp.Data.Models;

public class YearGroup
{
    public long Id { get; set; }
    public string Name { get; set; } = default!;
    public string Code { get; set; } = default!;
    public int NumericValue { get; set; }
    public long PhaseId { get; set; }
    public long SchoolId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
