/*
===============================================================================
 File        : ClassFullViewService.cs
 Namespace   : AbsenceApp.Data.Services
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-17
 Updated     : 2026-03-17
-------------------------------------------------------------------------------
 Purpose     : Fetches all class records and projects them to ClassFullViewDto
               via ClassFullViewMapper.  Class has no FK IDs to resolve;
               the service simply strips system fields (Id, CreatedAt, UpdatedAt).
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-17  Initial creation.
-------------------------------------------------------------------------------
 Notes       :
   - Takes AppDbContext directly for consistency with the other FullView services.
   - No lookup tables required — Class.Code and Class.Description are plain strings.
===============================================================================
*/

using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;
using AbsenceApp.Data.Context;
using AbsenceApp.Data.Mappers;
using Microsoft.EntityFrameworkCore;

namespace AbsenceApp.Data.Services;

// =========================================================================
// ClassFullViewService — projects classes to full-view DTOs (no FK joins)
// =========================================================================

public class ClassFullViewService : IClassFullViewService
{
    private readonly AppDbContext _db;

    public ClassFullViewService(AppDbContext db) => _db = db;

    // =========================================================================
    // GetAllAsync — load all classes and project to ClassFullViewDto
    // =========================================================================

    public async Task<IReadOnlyList<ClassFullViewDto>> GetAllAsync()
    {
        var classes = await _db.TeachingGroups
            .AsNoTracking()
            .ToListAsync();

        return classes
            .Select(ClassFullViewMapper.ToDto)
            .ToList()
            .AsReadOnly();
    }
}
