/*
===============================================================================
 File        : StaffFullViewService.cs
 Namespace   : AbsenceApp.Data.Services
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-17
 Updated     : 2026-03-17
-------------------------------------------------------------------------------
 Purpose     : Fetches all staff and resolves FK IDs to human-readable names
               by loading JobTitle, JobGroup, Department, and Staff (for the
               reporting manager self-join) lookup tables into in-memory
               dictionaries, then projecting via StaffFullViewMapper.
               Returns StaffFullViewDto for use by Table Settings display.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-17  Initial creation.
-------------------------------------------------------------------------------
 Notes       :
   - Takes AppDbContext directly (not IStaffRepository) for cross-table joins.
   - All lookups are loaded with AsNoTracking() for read-only performance.
   - ReportingManagerName is resolved via a self-join on Staff: the manager's
     full name is "{FirstName} {LastName}".  Null when no manager assigned.
   - Missing lookup entries fall back to "(unknown)" for required FKs.
===============================================================================
*/

using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;
using AbsenceApp.Data.Context;
using AbsenceApp.Data.Mappers;
using Microsoft.EntityFrameworkCore;

namespace AbsenceApp.Data.Services;

// =========================================================================
// StaffFullViewService — resolves FK IDs and returns flattened projections
// =========================================================================

public class StaffFullViewService : IStaffFullViewService
{
    private readonly AppDbContext _db;

    public StaffFullViewService(AppDbContext db) => _db = db;

    // =========================================================================
    // GetAllAsync — load staff + lookup tables, project to full-view DTOs
    // =========================================================================

    public async Task<IReadOnlyList<StaffFullViewDto>> GetAllAsync()
    {
        var jobTitles = await _db.JobTitles
            .AsNoTracking()
            .ToDictionaryAsync(j => j.Id, j => j.Title);

        var jobGroups = await _db.JobGroups
            .AsNoTracking()
            .ToDictionaryAsync(g => g.Id, g => g.Name);

        var departments = await _db.Departments
            .AsNoTracking()
            .ToDictionaryAsync(d => d.Id, d => d.Name);

        // Self-join: load a name lookup for all staff to resolve ReportingManagerId.
        var staffNames = await _db.Staff
            .AsNoTracking()
            .ToDictionaryAsync(
                s => s.Id,
                s => $"{s.FirstName} {s.LastName}".Trim());

        var allStaff = await _db.Staff
            .AsNoTracking()
            .ToListAsync();

        return allStaff
            .Select(s => StaffFullViewMapper.ToDto(
                s,
                jobTitleName:   jobTitles.GetValueOrDefault(s.JobTitleId, "(unknown)"),
                jobGroupName:   jobGroups.GetValueOrDefault(s.JobGroupId, "(unknown)"),
                departmentName: departments.GetValueOrDefault(s.DepartmentId, "(unknown)"),
                managerName:    s.ReportingManagerId.HasValue
                                    ? staffNames.GetValueOrDefault(s.ReportingManagerId.Value)
                                    : null))
            .ToList()
            .AsReadOnly();
    }
}
