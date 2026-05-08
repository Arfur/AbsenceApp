/*
===============================================================================
 File        : UserFullViewService.cs
 Namespace   : AbsenceApp.Data.Services
 Author      : Michael
 Version     : 1.1.0
 Created     : 2026-03-17
 Updated     : 2026-04-25
-------------------------------------------------------------------------------
 Purpose     : Provides a lightweight, read-only projection of Users for the
               Users table view. Resolves RoleType names only. Department
               resolution is intentionally NOT performed here because:

               - Users table does NOT contain DepartmentId.
               - Staff.DepartmentId is authoritative but Staff is NOT joined
                 here by design.
               - Department resolution for Staff is handled in
                 UserManagementService (User Profile detail endpoint).

               This service must remain fast, isolated, and free of Staff
               domain dependencies.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-17  Initial creation.
   - 1.1.0  2026-04-25  Removed invalid Users.DepartmentId usage.
                        Ensured DepartmentName is always null.
                        Updated header to reflect correct domain boundaries.
-------------------------------------------------------------------------------
 Notes       :
   - This service is intentionally lightweight.
   - It does NOT join Staff or StaffDepartments.
   - DepartmentName is always null unless UserProfile metadata is extended.
   - All sensitive credential fields are excluded at the mapper layer.
===============================================================================
*/

using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;
using AbsenceApp.Data.Context;
using AbsenceApp.Data.Mappers;
using Microsoft.EntityFrameworkCore;

namespace AbsenceApp.Data.Services;

public class UserFullViewService : IUserFullViewService
{
    private readonly AppDbContext _db;

    public UserFullViewService(AppDbContext db) => _db = db;

    // =========================================================================
    // GetAllAsync — load users + lookup tables, project to full-view DTOs
    // =========================================================================

    public async Task<IReadOnlyList<UserFullViewDto>> GetAllAsync()
    {
        // NOTE:
        // Users table does NOT contain DepartmentId.
        // Staff.DepartmentId is authoritative but NOT used here.
        // DepartmentName is therefore always null.
        var users = await _db.Users
            .AsNoTracking()
            .ToListAsync();

        return users
          .Select(u => UserFullViewMapper.ToDto(
            u,
            roleTypeName:   "(unknown)", // RoleTypeId removed from User
            departmentName: null))   // Correct: Users do NOT have DepartmentId
          .ToList()
          .AsReadOnly();
    }
}
