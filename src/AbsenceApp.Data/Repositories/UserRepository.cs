/*
===============================================================================
 File        : UserRepository.cs
 Namespace   : AbsenceApp.Data.Repositories
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : EF Core implementation of IUserRepository.
               Persists and retrieves User entities via AppDbContext.
-------------------------------------------------------------------------------
 Description :
   Each mutating operation (AddAsync, UpdateAsync, DeleteAsync) calls
   SaveChangesAsync immediately so that callers receive a self-contained
   unit of work. If a larger transaction scope is needed, a Unit-of-Work
   wrapper can be layered above this class.
   Query() exposes an IQueryable<User> to allow callers to push additional
   predicates to SQL before materialisation.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial implementation.
-------------------------------------------------------------------------------
 Notes       :
   - DeleteAsync is a no-op (no exception) when the entity does not exist,
     matching defensive repository convention.
===============================================================================
*/

using AbsenceApp.Data.Context;
using AbsenceApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AbsenceApp.Data.Repositories;

public class UserRepository : IUserRepository
{
    // =========================================================================
    // Dependencies — constructor-injected DbContext
    // =========================================================================

    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context) => _context = context;

    // =========================================================================
    // IQueryable accessor — enables composable, SQL-pushed filtering
    // =========================================================================

    public IQueryable<User> Query() => _context.Users.AsQueryable();

    // =========================================================================
    // Query operations — async read methods
    // =========================================================================

    public async Task<User?> FindByIdAsync(int id) =>
        await _context.Users.FindAsync(id);

    public async Task<IEnumerable<User>> ListAsync() =>
        await _context.Users.ToListAsync();

    // =========================================================================
    // Mutation operations — async write methods
    // =========================================================================

    public async Task AddAsync(User entity)
    {
        await _context.Users.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User entity)
    {
        _context.Users.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Users.FindAsync(id);
        if (entity is not null)
        {
            _context.Users.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
