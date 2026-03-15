/*
===============================================================================
 File        : IClassRepository.cs
 Namespace   : AbsenceApp.Data.Repositories
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : Defines the async CRUD contract for Class entity persistence.
               Implemented by ClassRepository using AppDbContext.
-------------------------------------------------------------------------------
 Description :
   Declared in AbsenceApp.Data.Repositories because Class is an
   AbsenceApp.Data.Models entity and must remain in the Data layer.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial interface defined.
-------------------------------------------------------------------------------
 Notes       :
   - Query() returns IQueryable<Class> for composable, SQL-pushed filtering.
   - FindByIdAsync returns null if the entity does not exist.
===============================================================================
*/

using AbsenceApp.Data.Models;

namespace AbsenceApp.Data.Repositories;

public interface IClassRepository
{
    // =========================================================================
    // Interface methods — async CRUD contract for Class entities
    // =========================================================================

    IQueryable<Class> Query();
    Task<Class?> FindByIdAsync(int id);
    Task<IEnumerable<Class>> ListAsync();
    Task AddAsync(Class entity);
    Task UpdateAsync(Class entity);
    Task DeleteAsync(int id);
}
