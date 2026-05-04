/*
===============================================================================
 File        : ClassMember.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 1.1.0
 Created     : 2026-03-15
 Updated     : 2026-05-04
-------------------------------------------------------------------------------
 Purpose     : Legacy stub entity retained for backward compatibility only.
               Class membership is now derived from Student.ClassId.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-15  Initial creation.
   - 1.1.0  2026-05-04  Phase 4.13: Fixed property UserId → StaffId to match
                         live DB column name (classmembers.StaffId confirmed
                         via INFORMATION_SCHEMA). Changed User navigation
                         property to Staff.
-------------------------------------------------------------------------------
 Notes       :
   - All properties mirror the original schema; do not add new properties.
   - Remove this stub once all legacy references have been migrated.
===============================================================================
*/
namespace AbsenceApp.Data.Models;

// Legacy stub retained so ClassMembersConfiguration.cs still compiles.
public class ClassMembers
{
    public int ClassId { get; set; }
    public int StaffId { get; set; }
    public string RoleInClass { get; set; } = default!;
    public TeachingGroup Class { get; set; } = default!;
    public Staff Staff { get; set; } = default!;
}
