/*
===============================================================================
 File        : ClassMember.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-15
 Updated     : 2026-03-15
-------------------------------------------------------------------------------
 Purpose     : Legacy stub entity retained for backward compatibility only.
               Class membership is now derived from Student.ClassId.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-15  Initial creation.
-------------------------------------------------------------------------------
 Notes       :
   - All properties mirror the original schema; do not add new properties.
   - Remove this stub once all legacy references have been migrated.
===============================================================================
*/
namespace AbsenceApp.Data.Models;

// Legacy stub retained so ClassMemberConfiguration.cs still compiles.
public class ClassMember
{
    public int ClassId { get; set; }
    public int UserId { get; set; }
    public string RoleInClass { get; set; } = default!;
    public Class Class { get; set; } = default!;
    public User User { get; set; } = default!;
}
