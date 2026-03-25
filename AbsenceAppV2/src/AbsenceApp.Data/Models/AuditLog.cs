/*
===============================================================================
 File        : AuditLog.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-15
 Updated     : 2026-03-15
-------------------------------------------------------------------------------
 Purpose     : Legacy stub entity retained for backward compatibility only.
               Active audit trail is distributed across the five dedicated audit tables.
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

// Legacy stub retained so AuditLogConfiguration.cs still compiles.
// Active audit trail is now split across the dedicated audit entities.
public class AuditLog
{
    public int AuditId { get; set; }
    public int UserId { get; set; }
    public string Action { get; set; } = default!;
    public DateTime Timestamp { get; set; }
    public User User { get; set; } = default!;
}
