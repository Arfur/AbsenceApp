/*
===============================================================================
 File        : AuditLogRepository.cs
 Namespace   : AbsenceApp.Data.Repositories
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-15
 Updated     : 2026-03-15
-------------------------------------------------------------------------------
 Purpose     : Legacy stub implementation of IAuditLogRepository.
               Operates on the retained AuditLog stub entity via Set<T>().
               Full audit trail is now distributed across five dedicated audit tables.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-15  Initial creation.
-------------------------------------------------------------------------------
 Notes       :
   - Uses _context.Set<AuditLog>() for the same reason as AttendanceRepository.
   - Remove this stub once all audit UI targets the per-entity audit tables.
===============================================================================
*/

using AbsenceApp.Data.Context;
using AbsenceApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AbsenceApp.Data.Repositories;

/// <summary>
/// Stub: generic AuditLog has been replaced by per-entity audit tables.
/// Full implementation pending audit mapping pass.
/// </summary>
public class AuditLogRepository : IAuditLogRepository
{
    // =========================================================================
    // Dependencies -- constructor-injected DbContext
    // =========================================================================

    private readonly AppDbContext _context;

    public AuditLogRepository(AppDbContext context) => _context = context;

    // =========================================================================
    // IAuditLogRepository implementation
    // =========================================================================

    public IQueryable<AuditLog> Query() => _context.Set<AuditLog>().AsQueryable();

    public async Task<AuditLog?> FindByIdAsync(int id) =>
        await _context.Set<AuditLog>().FindAsync(id);

    public async Task<IEnumerable<AuditLog>> ListAsync() =>
        await _context.Set<AuditLog>().ToListAsync();

    public async Task AddAsync(AuditLog entity)
    {
        await _context.Set<AuditLog>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(AuditLog entity)
    {
        _context.Set<AuditLog>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Set<AuditLog>().FindAsync(id);
        if (entity is not null)
        {
            _context.Set<AuditLog>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
