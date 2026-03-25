/*
===============================================================================
 File        : AttendanceService.cs
 Namespace   : AbsenceApp.Data.Services
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-14
 Updated     : 2026-03-14
-------------------------------------------------------------------------------
 Purpose     : Implements IAttendanceService using IAttendanceRepository.
               Provides read-only operations for attendance data, converting
               Attendance EF entities to AttendanceDto via AttendanceMapper.
-------------------------------------------------------------------------------
 Description :
   Attendance records are created by a separate clock-in/out subsystem;
   this service provides the read path consumed by the UI attendance page.
   All entity-to-DTO conversion is delegated to AttendanceMapper.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-14  Initial creation.
-------------------------------------------------------------------------------
 Notes       :
   - Does not reference AppDbContext directly; all data access is via the
     injected IAttendanceRepository.
   - GetByUserAsync pushes a WHERE predicate to SQL via LINQ before
     materialising results to avoid full-table loads.
===============================================================================
*/

using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;
using AbsenceApp.Data.Mappers;
using AbsenceApp.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AbsenceApp.Data.Services;

public class AttendanceService : IAttendanceService
{
    // =========================================================================
    // Dependencies — constructor-injected repository
    // =========================================================================

    private readonly IAttendanceRepository _repository;

    public AttendanceService(IAttendanceRepository repository)
        => _repository = repository;

    // =========================================================================
    // IAttendanceService implementation — read operations
    // =========================================================================

    /// <inheritdoc/>
    public async Task<IEnumerable<AttendanceDto>> GetAllAsync()
    {
        var entities = await _repository.ListAsync();
        return entities.Select(AttendanceMapper.ToDto);
    }

    /// <inheritdoc/>
    public async Task<AttendanceDto?> GetByIdAsync(int id)
    {
        var entity = await _repository.FindByIdAsync(id);
        return entity is null ? null : AttendanceMapper.ToDto(entity);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<AttendanceDto>> GetByUserAsync(int userId)
    {
        var entities = await _repository.Query()
            .Where(a => a.UserId == userId)
            .ToListAsync();
        return entities.Select(AttendanceMapper.ToDto);
    }
}
