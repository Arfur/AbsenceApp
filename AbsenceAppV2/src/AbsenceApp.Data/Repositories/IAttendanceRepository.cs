/*
===============================================================================
 File        : IAttendanceRepository.cs
 Namespace   : AbsenceApp.Data.Repositories
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : Defines the async CRUD contract for Attendance entity persistence.
               Implemented by AttendanceRepository using AppDbContext.
-------------------------------------------------------------------------------
 Description :
   Declared in AbsenceApp.Data.Repositories because Attendance is an
   AbsenceApp.Data.Models entity and must remain in the Data layer.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial interface defined.
-------------------------------------------------------------------------------
 Notes       :
   - Query() returns IQueryable<Attendance> for composable, SQL-pushed filtering.
   - FindByIdAsync returns null if the entity does not exist.
===============================================================================
*/

using AbsenceApp.Data.Models;

namespace AbsenceApp.Data.Repositories;

public interface IAttendanceRepository
{
    // =========================================================================
    // Interface methods — async CRUD contract for Attendance entities
    // =========================================================================

    IQueryable<Attendance> Query();
    Task<Attendance?> FindByIdAsync(int id);
    Task<IEnumerable<Attendance>> ListAsync();
    Task AddAsync(Attendance entity);
    Task UpdateAsync(Attendance entity);
    Task DeleteAsync(int id);
}
