/*
===============================================================================
 File        : IAttendanceService.cs
 Namespace   : AbsenceApp.Core.Interfaces
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-14
 Updated     : 2026-03-14
-------------------------------------------------------------------------------
 Purpose     : Defines the async read contract for attendance operations.
               Implemented by AttendanceService in AbsenceApp.Data.Services.
               Returns AttendanceDto to keep the Core layer free of EF entities.
-------------------------------------------------------------------------------
 Description :
   Attendance records are write-heavy (created by a clock-in/out system)
   so only the read methods needed by the UI are declared on this interface.
   Write operations are handled directly by the repository in the Data layer.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-14  Initial interface defined.
-------------------------------------------------------------------------------
 Notes       :
   - GetAllAsync materialises all records; use with caution on large datasets.
   - GetByUserAsync pushes the WHERE clause to SQL via IQueryable in the
     implementing service.
===============================================================================
*/

using AbsenceApp.Core.DTOs;

namespace AbsenceApp.Core.Interfaces;

public interface IAttendanceService
{
    // =========================================================================
    // Interface methods — async read operations for Attendance data
    // =========================================================================

    /// <summary>Returns all attendance records.</summary>
    Task<IEnumerable<AttendanceDto>> GetAllAsync();

    /// <summary>Returns a single attendance record by its primary key.
    /// Returns null if the record does not exist.</summary>
    Task<AttendanceDto?> GetByIdAsync(int id);

    /// <summary>Returns all attendance records for the specified user.</summary>
    Task<IEnumerable<AttendanceDto>> GetByUserAsync(int userId);
}
