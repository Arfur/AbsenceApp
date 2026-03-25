/*
===============================================================================
 File        : AuditLogDto.cs
 Namespace   : AbsenceApp.Core.DTOs
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : Data transfer object for passing audit trail data across layer
               boundaries without exposing the EF Core AuditLog entity.
-------------------------------------------------------------------------------
 Description :
   Carries the audit entry identifier, user reference, action description,
   and timestamp for a single audit log record.
   Used as the return type from AuditLogService and IAuditLogRepository calls.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial creation.
-------------------------------------------------------------------------------
 Notes       :
   - Action defaults to string.Empty to satisfy nullable analysis.
   - Timestamps are set to DateTime.UtcNow by AuditLogService.LogAsync.
===============================================================================
*/

namespace AbsenceApp.Core.DTOs;

public class AuditLogDto
{
    // =========================================================================
    // Properties — data fields transferred across layer boundaries
    // =========================================================================

    public int AuditId { get; set; }
    public int UserId { get; set; }
    public string Action { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}
