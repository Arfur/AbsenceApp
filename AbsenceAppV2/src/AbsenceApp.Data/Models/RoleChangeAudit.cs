/*
===============================================================================
 File        : RoleChangeAudit.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-15
 Updated     : 2026-03-15
-------------------------------------------------------------------------------
 Purpose     : EF Core entity for the role_change_audit table.
               Tracks every change to a user's role type with before/after values.
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

public class RoleChangeAudit
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int? OldRoleId { get; set; }
    public int? NewRoleId { get; set; }
    public int ChangedBy { get; set; }
    public string? ChangeReason { get; set; }
    public DateTime ChangedAt { get; set; }
}
