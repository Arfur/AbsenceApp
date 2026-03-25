/*
===============================================================================
 File        : UserFullViewService.cs
 Namespace   : AbsenceApp.Data.Services
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-17
 Updated     : 2026-03-17
-------------------------------------------------------------------------------
 Purpose     : Fetches all users and resolves FK IDs to human-readable names
               by loading RoleType and Department lookup tables into in-memory
               dictionaries, then projecting via UserFullViewMapper.
               Returns UserFullViewDto for use by Table Settings display.
               All sensitive credential fields are excluded at the mapper layer.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-17  Initial creation.
-------------------------------------------------------------------------------
 Notes       :
   - Takes AppDbContext directly (not IUserRepository) for cross-table joins.
   - All lookups are loaded with AsNoTracking() for read-only performance.
   - RoleTypeName resolved from RoleType.DisplayName (not RoleType.Name).
   - DepartmentName is nullable — users without a department get null.
===============================================================================
*/

using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;
using AbsenceApp.Data.Context;
using AbsenceApp.Data.Mappers;
using Microsoft.EntityFrameworkCore;

namespace AbsenceApp.Data.Services;

// =========================================================================
// UserFullViewService — resolves FK IDs and returns flattened projections
// =========================================================================

public class UserFullViewService : IUserFullViewService
{
    private readonly AppDbContext _db;

    public UserFullViewService(AppDbContext db) => _db = db;

    // =========================================================================
    // GetAllAsync — load users + lookup tables, project to full-view DTOs
    // =========================================================================

    public async Task<IReadOnlyList<UserFullViewDto>> GetAllAsync()
    {
        var roleTypes = await _db.RoleTypes
            .AsNoTracking()
            .ToDictionaryAsync(r => r.Id, r => r.DisplayName);

        var departments = await _db.Departments
            .AsNoTracking()
            .ToDictionaryAsync(d => d.Id, d => d.Name);

        var users = await _db.Users
            .AsNoTracking()
            .ToListAsync();

        return users
            .Select(u => UserFullViewMapper.ToDto(
                u,
                roleTypeName:   roleTypes.GetValueOrDefault(u.RoleTypeId, "(unknown)"),
                departmentName: u.DepartmentId.HasValue
                                    ? departments.GetValueOrDefault(u.DepartmentId.Value)
                                    : null))
            .ToList()
            .AsReadOnly();
    }
}
