/*
===============================================================================
 File        : StaffDepartment.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 2.0.0
 Created     : 2026-03-15
 Updated     : 2026-04-27
-------------------------------------------------------------------------------
 Purpose     : Authoritative EF Core entity for the staffdepartments table.
               Replaces the legacy Department entity and provides the correct
               organisational grouping for staff records across the system.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-15  Initial creation.
   - 2.0.0  2026-04-27  Promoted to primary department model. Legacy Department
                        entity deprecated and removed from DbContext to prevent
                        EF Core from inferring unintended relationships (e.g.
                        StaffAssignment → Staff → Department shadow FKs).
-------------------------------------------------------------------------------
 Notes       :
   - This entity is now the single source of truth for staff department data.
   - No navigation properties; FK integrity is enforced at the database layer.
   - All required string properties use = default! to satisfy the nullable compiler.
===============================================================================
*/

namespace AbsenceApp.Data.Models
{
    public class StaffDepartment
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? HeadUserId { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
