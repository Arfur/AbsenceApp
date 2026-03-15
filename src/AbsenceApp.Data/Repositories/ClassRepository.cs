/*
===============================================================================
 File        : ClassRepository.cs
 Namespace   : AbsenceApp.Data.Repositories
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : EF Core implementation of IClassRepository.
               Persists and retrieves Class entities via AppDbContext.
-------------------------------------------------------------------------------
 Description :
   Each mutating operation calls SaveChangesAsync immediately for a
   self-contained unit of work. Query() exposes IQueryable<Class> for
   caller-composed SQL predicates (e.g. filtering by ClassName).
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial implementation.
-------------------------------------------------------------------------------
 Notes       :
   - DeleteAsync is a no-op when the entity is not found.
===============================================================================
*/

using AbsenceApp.Data.Context;
using AbsenceApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AbsenceApp.Data.Repositories;

public class ClassRepository : IClassRepository
{
    // =========================================================================
    // Dependencies — constructor-injected DbContext
    // =========================================================================

    private readonly AppDbContext _context;

    public ClassRepository(AppDbContext context) => _context = context;

    // =========================================================================
    // IQueryable accessor — enables composable, SQL-pushed filtering
    // =========================================================================

    public IQueryable<Class> Query() => _context.Classes.AsQueryable();

    // =========================================================================
    // Query operations — async read methods
    // =========================================================================

    public async Task<Class?> FindByIdAsync(int id) =>
        await _context.Classes.FindAsync(id);

    public async Task<IEnumerable<Class>> ListAsync() =>
        await _context.Classes.ToListAsync();

    // =========================================================================
    // Mutation operations — async write methods
    // =========================================================================

    public async Task AddAsync(Class entity)
    {
        await _context.Classes.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Class entity)
    {
        _context.Classes.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Classes.FindAsync(id);
        if (entity is not null)
        {
            _context.Classes.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
