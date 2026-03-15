/*
===============================================================================
 File        : StaffExternalAccountAudit.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-15
 Updated     : 2026-03-15
-------------------------------------------------------------------------------
 Purpose     : EF Core entity for the staff_external_account_audit table.
               Captures every create / update / delete action on a staff external account.
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

public class StaffExternalAccountAudit
{
    public long Id { get; set; }
    public long StaffExternalAccountId { get; set; }
    public long StaffId { get; set; }
    public string Action { get; set; } = default!;
    public long ChangedBy { get; set; }
    public DateTime ChangeTime { get; set; }
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
}
