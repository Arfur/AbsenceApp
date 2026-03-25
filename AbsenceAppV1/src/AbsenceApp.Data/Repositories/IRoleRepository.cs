/*
===============================================================================
 File        : IRoleRepository.cs
 Namespace   : AbsenceApp.Data.Repositories
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : Defines the async CRUD contract for Role entity persistence.
               Implemented by RoleRepository using AppDbContext.
-------------------------------------------------------------------------------
 Description :
   Declared in AbsenceApp.Data.Repositories because Role is an
   AbsenceApp.Data.Models entity and must remain in the Data layer.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial interface defined.
-------------------------------------------------------------------------------
 Notes       :
   - Query() returns IQueryable<Role> for composable, SQL-pushed filtering.
   - FindByIdAsync returns null if the entity does not exist.
===============================================================================
*/

using AbsenceApp.Data.Models;

namespace AbsenceApp.Data.Repositories;

public interface IRoleRepository
{
    // =========================================================================
    // Interface methods — async CRUD contract for Role entities
    // =========================================================================

    IQueryable<Role> Query();
    Task<Role?> FindByIdAsync(int id);
    Task<IEnumerable<Role>> ListAsync();
    Task AddAsync(Role entity);
    Task UpdateAsync(Role entity);
    Task DeleteAsync(int id);
}
