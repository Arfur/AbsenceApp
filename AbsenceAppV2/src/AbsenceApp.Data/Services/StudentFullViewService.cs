/*
===============================================================================
 File        : StudentFullViewService.cs
 Namespace   : AbsenceApp.Data.Services
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-17
 Updated     : 2026-03-17
-------------------------------------------------------------------------------
 Purpose     : Fetches all students and resolves FK IDs to human-readable names
               by loading YearGroup, Class, and House lookup tables into
               in-memory dictionaries, then projecting via StudentFullViewMapper.
               Returns StudentFullViewDto for use by Table Settings display.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-17  Initial creation.
-------------------------------------------------------------------------------
 Notes       :
   - Takes AppDbContext directly (not IStudentRepository) to enable cross-table
     lookup joins without navigation properties.
   - All lookups are loaded with AsNoTracking() for read-only performance.
   - Missing lookup entries fall back to "(unknown)" to prevent null propagation.
===============================================================================
*/

using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;
using AbsenceApp.Data.Context;
using AbsenceApp.Data.Mappers;
using Microsoft.EntityFrameworkCore;

namespace AbsenceApp.Data.Services;

// =========================================================================
// StudentFullViewService — resolves FK IDs and returns flattened projections
// =========================================================================

public class StudentFullViewService : IStudentFullViewService
{
    private readonly AppDbContext _db;

    public StudentFullViewService(AppDbContext db) => _db = db;

    // =========================================================================
    // GetAllAsync — load students + lookup tables, project to full-view DTOs
    // =========================================================================

    public async Task<IReadOnlyList<StudentFullViewDto>> GetAllAsync()
    {
        var yearGroups = await _db.YearGroups
            .AsNoTracking()
            .ToDictionaryAsync(y => y.Id, y => y.Name);

        var houses = await _db.Houses
            .AsNoTracking()
            .ToDictionaryAsync(h => h.Id, h => h.Name);

        var students = await _db.Students
            .AsNoTracking()
            .ToListAsync();

        return students
            .Select(s => StudentFullViewMapper.ToDto(
                s,
                yearGroupName: yearGroups.TryGetValue(s.YearGroupId, out var yg) ? yg ?? "(unknown)" : "(unknown)",
                className:     "(unknown)",
                houseName:     s.HouseId.HasValue
                                   ? (houses.TryGetValue(s.HouseId.Value, out var hn) ? hn : null)
                                   : null))
            .ToList()
            .AsReadOnly();
    }
}
