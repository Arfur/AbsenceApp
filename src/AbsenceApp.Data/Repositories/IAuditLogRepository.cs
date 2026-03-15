/*
===============================================================================
 File        : IAuditLogRepository.cs
 Namespace   : AbsenceApp.Data.Repositories
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : Defines the async CRUD contract for AuditLog entity persistence.
               Implemented by AuditLogRepository using AppDbContext.
-------------------------------------------------------------------------------
 Description :
   Declared in AbsenceApp.Data.Repositories because AuditLog is an
   AbsenceApp.Data.Models entity and must remain in the Data layer.
   The Query() accessor is used by AuditLogService.GetByUserAsync to push
   WHERE filtering to SQL rather than loading all rows into memory.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial interface defined.
-------------------------------------------------------------------------------
 Notes       :
   - Even though audit logs are append-only at the service level, the
     repository interface exposes full CRUD so that the Data layer
     retains flexibility for administrative tooling.
===============================================================================
*/

using AbsenceApp.Data.Models;

namespace AbsenceApp.Data.Repositories;

public interface IAuditLogRepository
{
    // =========================================================================
    // Interface methods — async CRUD contract for AuditLog entities
    // =========================================================================

    IQueryable<AuditLog> Query();
    Task<AuditLog?> FindByIdAsync(int id);
    Task<IEnumerable<AuditLog>> ListAsync();
    Task AddAsync(AuditLog entity);
    Task UpdateAsync(AuditLog entity);
    Task DeleteAsync(int id);
}
