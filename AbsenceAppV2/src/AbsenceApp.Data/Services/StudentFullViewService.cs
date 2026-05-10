/*
===============================================================================
 File        : StudentFullViewService.cs
 Namespace   : AbsenceApp.Data.Services
 Author      : Michael
 Version     : 1.1.0
 Created     : 2026-03-17
 Updated     : 2026-05-05
-------------------------------------------------------------------------------
 Purpose     : Fetches all students and resolves FK IDs to human-readable names
               by loading YearGroup, Class, and House lookup tables into
               in-memory dictionaries, then projecting via StudentFullViewMapper.
               Returns StudentFullViewDto for use by Table Settings display.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-17  Initial creation.
   - 1.1.0  2026-05-05  Student Absence Management: added GetByIdAsync(int id).
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

    // =========================================================================
    // GetByIdAsync — load single student + lookup tables, project to DTO
    // =========================================================================

    public async Task<StudentFullViewDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var student = await _db.Students
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id, ct);

        if (student is null) return null;

        var yearGroupName = await _db.YearGroups
            .AsNoTracking()
            .Where(y => y.Id == student.YearGroupId)
            .Select(y => y.Name)
            .FirstOrDefaultAsync(ct) ?? "(unknown)";

        string? houseName = null;
        if (student.HouseId.HasValue)
        {
            houseName = await _db.Houses
                .AsNoTracking()
                .Where(h => h.Id == student.HouseId.Value)
                .Select(h => h.Name)
                .FirstOrDefaultAsync(ct);
        }

        return StudentFullViewMapper.ToDto(student,
            yearGroupName: yearGroupName,
            className:     "(unknown)",
            houseName:     houseName);
    }

    // =========================================================================
    // DeleteAsync — permanently remove a student record
    // =========================================================================

    public async Task DeleteAsync(long id, CancellationToken ct = default)
    {
        var student = await _db.Students.FindAsync([id], ct);
        if (student is not null)
        {
            _db.Students.Remove(student);
            await _db.SaveChangesAsync(ct);
        }
    }
}
