/*
===============================================================================
 File        : ClassYearGroupAssignment.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-15
 Updated     : 2026-03-15
-------------------------------------------------------------------------------
 Purpose     : EF Core entity for the class_year_group_assignments link table.
               Associates a class with a year group and school.
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

public class ClassYearGroupAssignment
{
    public long Id { get; set; }
    public long ClassId { get; set; }
    public long YearGroupId { get; set; }
    public long SchoolId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
