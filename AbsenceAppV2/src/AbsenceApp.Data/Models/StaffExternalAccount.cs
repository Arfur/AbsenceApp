/*
===============================================================================
 File        : StaffExternalAccount.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-15
 Updated     : 2026-03-15
-------------------------------------------------------------------------------
 Purpose     : EF Core entity for the staff_external_accounts table.
               Stores per-staff credentials for third-party/external systems.
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

public class StaffExternalAccount
{
    public int Id { get; set; }
    public int StaffId { get; set; }
    public int ExternalSystemId { get; set; }
    public string ExternalUsername { get; set; } = default!;
    public string? ExternalUserId { get; set; }
    public string AccountType { get; set; } = "user";
    public string Status { get; set; } = default!;
    public DateTime? LastSyncAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
