/*
===============================================================================
 File        : LoginAudit.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-15
 Updated     : 2026-03-15
-------------------------------------------------------------------------------
 Purpose     : EF Core entity for the login_audit table.
               Records every authentication attempt with IP address and outcome.
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

public class LoginAudit
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public DateTime LoginTime { get; set; }
    public string IpAddress { get; set; } = default!;
    public string UserAgent { get; set; } = default!;
    public bool Success { get; set; }
    public DateTime CreatedAt { get; set; }
}
