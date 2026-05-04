/*
===============================================================================
 File        : LoginAudit.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 1.3.0
 Created     : 2026-03-15
 Updated     : 2026-05-04
-------------------------------------------------------------------------------
 Purpose     : EF Core entity for the LoginAudit table.
               Records every authentication attempt with IP address and outcome.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-15  Initial creation.
   - 1.1.0  2026-04-24  Added explicit [Column] attribute mappings to align
                         EF property names with the actual DB column names.
                         DB uses LoginAt / LoginIp / WasSuccessful; the C#
                         model retains its original names so no call-site
                         changes are required. Fixes runtime MySqlException:
                         "Unknown column 'l.LoginTime' in 'field list'".
   - 1.2.0  2026-04-24  Session 7 fix-phase Task A: re-validated all three
                         [Column] attributes present and correct. No code
                         change required; header version incremented as
                         evidence of the mandatory validation pass.
   - 1.3.0  2026-05-04  Fix Plan #2 Step 1: changed UserId from int to int?
                         to allow null assignment for unknown-user login
                         audit records (AuthService assigns UserId = null
                         when the login attempt uses an unknown username).
-------------------------------------------------------------------------------
 Notes       :
   - No navigation properties; FK integrity is enforced at the database layer.
   - All required string properties use = default! to satisfy the nullable compiler.
===============================================================================
*/

using System.ComponentModel.DataAnnotations.Schema;

namespace AbsenceApp.Data.Models;

public class LoginAudit
{
    public int Id { get; set; }
    public int? UserId { get; set; }

    [Column("LoginAt")]
    public DateTime LoginTime { get; set; }

    [Column("LoginIp")]
    public string? IpAddress { get; set; }

    public string? UserAgent { get; set; }

    [Column("WasSuccessful")]
    public bool Success { get; set; }

    public string? FailureReason { get; set; }
}
