/*
===============================================================================
 File        : ClassRepository.cs
 Namespace   : AbsenceApp.Data.Repositories
 Author      : Michael
 Version     : 1.1.0
 Created     : 2026-03-13
 Updated     : 2026-05-04
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
   - 1.1.0  2026-05-04  Fix Plan #2 Step 5: replaced all 7 occurrences of
                         _context.TeachingGroups with _context.Classes. The
                         DbSet<TeachingGroup> is named Classes in AppDbContext;
                         the repository was referencing a non-existent property.
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

    public IQueryable<TeachingGroup> Query() => _context.Classes.AsQueryable();

    // =========================================================================
    // Query operations — async read methods
    // =========================================================================

    public async Task<TeachingGroup?> FindByIdAsync(long id) =>
        await _context.Classes.FindAsync(id);

    public async Task<IEnumerable<TeachingGroup>> ListAsync() =>
        await _context.Classes.ToListAsync();

    // =========================================================================
    // Mutation operations — async write methods
    // =========================================================================

    public async Task AddAsync(TeachingGroup entity)
    {
        await _context.Classes.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TeachingGroup entity)
    {
        _context.Classes.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var entity = await _context.Classes.FindAsync(id);
        if (entity is not null)
        {
            _context.Classes.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
