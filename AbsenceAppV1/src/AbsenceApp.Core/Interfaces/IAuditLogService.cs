/*
===============================================================================
 File        : IAuditLogService.cs
 Namespace   : AbsenceApp.Core.Interfaces
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : Defines the async contract for audit trail operations.
               Implemented by AuditLogService in AbsenceApp.Data.Services.
-------------------------------------------------------------------------------
 Description :
   Audit logs are intentionally append-only; no UpdateAsync or DeleteAsync
   is declared on this interface.
   All methods return DTOs (AuditLogDto) rather than EF Core entities.
   Registered against this interface in DataServiceRegistration for DI.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial interface defined.
-------------------------------------------------------------------------------
 Notes       :
   - LogAsync is the sole write method; it timestamps and persists the entry.
   - GetByUserAsync filters by UserId; the implementation pushes the WHERE
     clause to SQL via IQueryable to avoid full-table loads.
===============================================================================
*/

using AbsenceApp.Core.DTOs;

namespace AbsenceApp.Core.Interfaces;

public interface IAuditLogService
{
    // =========================================================================
    // Interface methods — read operations
    // =========================================================================

    /// <summary>Returns all audit log entries.</summary>
    Task<IEnumerable<AuditLogDto>> GetAllAsync();

    /// <summary>Returns a single entry by its primary key.</summary>
    Task<AuditLogDto?> GetByIdAsync(int id);

    /// <summary>Returns all entries recorded for a specific user.</summary>
    Task<IEnumerable<AuditLogDto>> GetByUserAsync(int userId);

    // =========================================================================
    // Interface methods — append-only write operation
    // =========================================================================

    /// <summary>
    /// Creates a new audit log entry.
    /// Business rule: audit logs are append-only; no update or delete is exposed.
    /// </summary>
    Task<AuditLogDto> LogAsync(int userId, string action);
}
