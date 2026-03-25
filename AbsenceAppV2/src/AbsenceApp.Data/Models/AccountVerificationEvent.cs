/*
===============================================================================
 File        : AccountVerificationEvent.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-15
 Updated     : 2026-03-15
-------------------------------------------------------------------------------
 Purpose     : EF Core entity for the account_verification_events audit table.
               Records each email/account verification attempt per user.
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

public class AccountVerificationEvent
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string EventType { get; set; } = default!;
    public DateTime EventTime { get; set; }
    public string IpAddress { get; set; } = default!;
    public string? Metadata { get; set; }
    public DateTime CreatedAt { get; set; }
}
