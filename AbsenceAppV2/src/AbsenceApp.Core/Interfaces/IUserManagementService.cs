/*
===============================================================================
 File        : IUserManagementService.cs
 Namespace   : AbsenceApp.Core.Interfaces
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-04-11
 Updated     : 2026-04-11
-------------------------------------------------------------------------------
 Purpose     : Service contract for the E15 User Management module.
               Covers user CRUD, role listing, page listing, and per-user
               permission matrix read/write operations.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-11  Initial creation (E15 User Management).
-------------------------------------------------------------------------------
 Notes       :
   - Implemented by UserManagementService in AbsenceApp.Data.Services.
   - Register as Scoped (EF Core DbContext dependency).
===============================================================================
*/
using AbsenceApp.Core.DTOs;

namespace AbsenceApp.Core.Interfaces;

public interface IUserManagementService
{
    // ── User CRUD ─────────────────────────────────────────────────────────────

    Task<IReadOnlyList<UserListItemDto>> GetUsersAsync(CancellationToken ct = default);
    Task<UserUpdateDto?>                 GetUserForEditAsync(long userId, CancellationToken ct = default);
    Task<long>                           CreateUserAsync(UserCreateDto dto, CancellationToken ct = default);
    Task                                 UpdateUserAsync(UserUpdateDto dto, CancellationToken ct = default);
    Task                                 DeleteUserAsync(long userId, CancellationToken ct = default);

    // ── Reference data ───────────────────────────────────────────────────────

    Task<IReadOnlyList<RoleTypeSelectDto>> GetRoleTypesAsync(CancellationToken ct = default);
    Task<IReadOnlyList<AppPageDto>>        GetPagesAsync(CancellationToken ct = default);

    // ── Permission matrix ─────────────────────────────────────────────────────

    /// <summary>
    /// Returns one PagePermissionDto per active page for the given user,
    /// reflecting any existing UserPagePermission row (HasOverride=true) or
    /// zeroed-out defaults (HasOverride=false).
    /// </summary>
    Task<IReadOnlyList<PagePermissionDto>> GetUserPermissionsAsync(long userId, CancellationToken ct = default);

    /// <summary>
    /// Upserts the full permission matrix for the given user. Rows where all
    /// six flags are false and HasOverride=false are deleted instead of saved.
    /// </summary>
    Task SaveUserPermissionsAsync(long userId, IEnumerable<PagePermissionDto> permissions, CancellationToken ct = default);

    // ── Role-level defaults ───────────────────────────────────────────────────

    Task<IReadOnlyList<PagePermissionDto>> GetRoleDefaultsAsync(string roleTypeName, CancellationToken ct = default);
}
