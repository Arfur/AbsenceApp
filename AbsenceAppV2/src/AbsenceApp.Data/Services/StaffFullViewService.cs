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
               using scalar IDs already present on Staff plus a Staff self-join
               for reporting manager display names, then projecting via
               StaffFullViewMapper.
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
                jobTitleName:   s.JobTitleId > 0 ? $"Job Title #{s.JobTitleId}" : string.Empty,
                jobGroupName:   s.JobGroupId > 0 ? $"Job Group #{s.JobGroupId}" : string.Empty,
                departmentName: s.DepartmentId > 0 ? $"Department #{s.DepartmentId}" : string.Empty,
                managerName:    s.ReportingManagerId.HasValue
                                    ? staffNames.GetValueOrDefault(s.ReportingManagerId.Value)
                                    : null))
            .ToList()
            .AsReadOnly();
    }
}
