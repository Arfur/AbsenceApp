/*
===============================================================================
 File        : AttendanceRepository.cs
 Namespace   : AbsenceApp.Data.Repositories
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-15
 Updated     : 2026-03-15
-------------------------------------------------------------------------------
 Purpose     : Legacy stub implementation of IAttendanceRepository.
               Operates on the retained Attendance stub entity via Set<T>().
               Full re-implementation targeting AttendanceRegister / AttendanceMark
               is pending.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-15  Initial creation.
-------------------------------------------------------------------------------
 Notes       :
   - Uses _context.Set<Attendance>() because the DbSet property for the
     legacy Attendance type was not added to AppDbContext.
   - Remove this stub once attendance UI targets the new register entities.
===============================================================================
*/

using AbsenceApp.Data.Context;
using AbsenceApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AbsenceApp.Data.Repositories;

/// <summary>
/// Legacy stub: IAttendanceRepository operates on the retained Attendance stub entity.
/// Full re-implementation targeting AttendanceRegisters/AttendanceMarks is pending.
/// </summary>
public class AttendanceRepository : IAttendanceRepository
{
    // =========================================================================
    // Dependencies -- constructor-injected DbContext
    // =========================================================================

    private readonly AppDbContext _context;

    public AttendanceRepository(AppDbContext context) => _context = context;

    // =========================================================================
    // IAttendanceRepository implementation
    // =========================================================================

    public IQueryable<Attendance> Query() => _context.Set<Attendance>().AsQueryable();

    public async Task<Attendance?> FindByIdAsync(int id) =>
        await _context.Set<Attendance>().FindAsync(id);

    public async Task<IEnumerable<Attendance>> ListAsync() =>
        await _context.Set<Attendance>().ToListAsync();

    public async Task AddAsync(Attendance entity)
    {
        await _context.Set<Attendance>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Attendance entity)
    {
        _context.Set<Attendance>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Set<Attendance>().FindAsync(id);
        if (entity is not null)
        {
            _context.Set<Attendance>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
