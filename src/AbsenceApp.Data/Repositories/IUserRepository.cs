/*
===============================================================================
 File        : IUserRepository.cs
 Namespace   : AbsenceApp.Data.Repositories
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : Defines the async CRUD contract for User entity persistence.
               Implemented by UserRepository using AppDbContext.
-------------------------------------------------------------------------------
 Description :
   Declared in AbsenceApp.Data.Repositories rather than AbsenceApp.Core.Interfaces
   because User is an AbsenceApp.Data.Models entity; placing the interface in Core
   would introduce a Core → Data → Core circular dependency.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial interface defined.
-------------------------------------------------------------------------------
 Notes       :
   - Query() returns IQueryable<User> to allow callers to compose additional
     LINQ predicates that are pushed to SQL before materialisation.
   - FindByIdAsync returns null if the entity does not exist.
===============================================================================
*/

using AbsenceApp.Data.Models;

namespace AbsenceApp.Data.Repositories;

public interface IUserRepository
{
    // =========================================================================
    // Interface methods — async CRUD contract for User entities
    // =========================================================================

    IQueryable<User> Query();
    Task<User?> FindByIdAsync(int id);
    Task<IEnumerable<User>> ListAsync();
    Task AddAsync(User entity);
    Task UpdateAsync(User entity);
    Task DeleteAsync(int id);
}
