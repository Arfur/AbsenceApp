/*
===============================================================================
 File        : RoleRepository.cs
 Namespace   : AbsenceApp.Data.Repositories
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-15
 Updated     : 2026-03-15
-------------------------------------------------------------------------------
 Purpose     : Legacy stub implementation of IRoleRepository.
               Operates on the retained Role stub entity via Set<T>().
               Active role logic uses RoleType (TABLE6).
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-15  Initial creation.
-------------------------------------------------------------------------------
 Notes       :
   - Uses _context.Set<Role>() for the same reason as AttendanceRepository.
   - Remove this stub once all role UI targets RoleType.
===============================================================================
*/

using AbsenceApp.Data.Context;
using AbsenceApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AbsenceApp.Data.Repositories;

/// <summary>
/// Stub: legacy Role entity replaced by RoleType (TABLE7).
/// Full implementation pending RoleType repository pass.
/// </summary>
public class RoleRepository : IRoleRepository
{
    // =========================================================================
    // Dependencies -- constructor-injected DbContext
    // =========================================================================

    private readonly AppDbContext _context;

    public RoleRepository(AppDbContext context) => _context = context;

    // =========================================================================
    // IRoleRepository implementation
    // =========================================================================

    public IQueryable<Role> Query() => _context.Set<Role>().AsQueryable();

    public async Task<Role?> FindByIdAsync(int id) =>
        await _context.Set<Role>().FindAsync(id);

    public async Task<IEnumerable<Role>> ListAsync() =>
        await _context.Set<Role>().ToListAsync();

    public async Task AddAsync(Role entity)
    {
        await _context.Set<Role>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Role entity)
    {
        _context.Set<Role>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Set<Role>().FindAsync(id);
        if (entity is not null)
        {
            _context.Set<Role>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
