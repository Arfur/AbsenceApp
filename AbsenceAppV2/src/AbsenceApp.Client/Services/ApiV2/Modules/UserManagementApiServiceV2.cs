/*
===============================================================================
 File        : UserManagementApiServiceV2.cs
 Namespace   : AbsenceApp.Client.Services.ApiV2.Modules
 Author      : Michael
 Version     : 1.4.0
 Created     : 2026-04-11
 Updated     : 2026-05-04
-------------------------------------------------------------------------------
 Purpose     : Client-side User Management API service. Provides full user
               CRUD, role/page reference data, per-user permission matrix
               read/write, staff-linked helpers, list-page data methods
               for Roles, Permissions, and Page Access pages, and the full
               User Profile page data methods (v1.2.0).
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-11  Initial creation (E15 User Management).
   - 1.1.0  2026-04-21  Added GetAllRoleTypesAsync(), GetFeaturesAsync(), and
                         GetPageAccessAsync().
   - 1.2.0  2026-04-21  Added full User Profile page wrapper methods:
                         GetUserProfileHeaderAsync, GetUserProfileDetailAsync,
                         GetStaffContactAsync, GetStaffClassAssignmentsAsync,
                         GetStaffDevicesAsync, GetStaffExternalAccountsAsync,
                         GetStaffAbsencesAsync, GetUserLoginAuditAsync,
                         SaveUserProfileAsync, ChangePasswordAsync,
                         UpdateProfilePhotoAsync.
    - 1.3.0  2026-04-25  Updated GetUserProfileDetailAsync to return unified
                                 UserProfileFullDetailDto (one-call full profile load).
    - 1.4.0  2026-05-04  Added GetUsersForSelectAsync() wrapper — returns all users
                         with accounts as IReadOnlyList<UserSelectDto>, used for the
                         Edit Mode user-navigation dropdown (Amendment C).
===============================================================================
*/

using AbsenceApp.Client.Services;
using AbsenceApp.Client.Shared;
using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AbsenceApp.Client.Services.ApiV2.Modules;

public sealed class UserManagementApiServiceV2
{
    // =========================================================================
    // Dependencies
    // =========================================================================

    private readonly IServiceScopeFactory _scopeFactory;

    public UserManagementApiServiceV2(IServiceScopeFactory scopeFactory)
        => _scopeFactory = scopeFactory;

    // =========================================================================
    // User CRUD
    // =========================================================================

    public async Task<IReadOnlyList<UserListItemDto>> GetUsersAsync(CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var svc = scope.ServiceProvider.GetRequiredService<IUserManagementService>();
            return await svc.GetUsersAsync(ct);
        }
        catch (Exception ex)
        {
            AppLog.Write("UserManagementApiServiceV2.cs", "GetUsersAsync", $"ERROR {ex.Message}");
            return [];
        }
    }

    public async Task<UserUpdateDto?> GetUserForEditAsync(int userId, CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var svc = scope.ServiceProvider.GetRequiredService<IUserManagementService>();
            return await svc.GetUserForEditAsync(userId, ct);
        }
        catch (Exception ex)
        {
            AppLog.Write("UserManagementApiServiceV2.cs", "GetUserForEditAsync", $"ERROR {ex.Message}");
            return null;
        }
    }

    public async Task<(bool Success, string? Error, long UserId)> CreateUserAsync(
        UserCreateDto dto, CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var svc = scope.ServiceProvider.GetRequiredService<IUserManagementService>();
            var id  = await svc.CreateUserAsync(dto, ct);
            return (true, null, id);
        }
        catch (Exception ex)
        {
            AppLog.Write("UserManagementApiServiceV2.cs", "CreateUserAsync", $"ERROR {ex.Message}");
            return (false, ex.Message, 0);
        }
    }

    public async Task<(bool Success, string? Error)> UpdateUserAsync(
        UserUpdateDto dto, CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var svc = scope.ServiceProvider.GetRequiredService<IUserManagementService>();
            await svc.UpdateUserAsync(dto, ct);
            return (true, null);
        }
        catch (Exception ex)
        {
            AppLog.Write("UserManagementApiServiceV2.cs", "UpdateUserAsync", $"ERROR {ex.Message}");
            return (false, ex.Message);
        }
    }

    public async Task<(bool Success, string? Error)> DeleteUserAsync(
        int userId, CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var svc = scope.ServiceProvider.GetRequiredService<IUserManagementService>();
            await svc.DeleteUserAsync(userId, ct);
            return (true, null);
        }
        catch (Exception ex)
        {
            AppLog.Write("UserManagementApiServiceV2.cs", "DeleteUserAsync", $"ERROR {ex.Message}");
            return (false, ex.Message);
        }
    }

    // =========================================================================
    // Reference data
    // =========================================================================

    public async Task<IReadOnlyList<RoleTypeSelectDto>> GetRoleTypesAsync(CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var svc = scope.ServiceProvider.GetRequiredService<IUserManagementService>();
            return await svc.GetRoleTypesAsync(ct);
        }
        catch (Exception ex)
        {
            AppLog.Write("UserManagementApiServiceV2.cs", "GetRoleTypesAsync", $"ERROR {ex.Message}");
            return [];
        }
    }

    public async Task<IReadOnlyList<AppPageDto>> GetPagesAsync(CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var svc = scope.ServiceProvider.GetRequiredService<IUserManagementService>();
            return await svc.GetPagesAsync(ct);
        }
        catch (Exception ex)
        {
            AppLog.Write("UserManagementApiServiceV2.cs", "GetPagesAsync", $"ERROR {ex.Message}");
            return [];
        }
    }

    // =========================================================================
    // Permission matrix
    // =========================================================================

    public async Task<IReadOnlyList<PagePermissionDto>> GetUserPermissionsAsync(
        long userId, CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var svc = scope.ServiceProvider.GetRequiredService<IUserManagementService>();
            return await svc.GetUserPermissionsAsync(userId, ct);
        }
        catch (Exception ex)
        {
            AppLog.Write("UserManagementApiServiceV2.cs", "GetUserPermissionsAsync", $"ERROR {ex.Message}");
            return [];
        }
    }

    public async Task<(bool Success, string? Error)> SaveUserPermissionsAsync(
        long userId, IEnumerable<PagePermissionDto> permissions, CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var svc = scope.ServiceProvider.GetRequiredService<IUserManagementService>();
            await svc.SaveUserPermissionsAsync(userId, permissions, ct);
            return (true, null);
        }
        catch (Exception ex)
        {
            AppLog.Write("UserManagementApiServiceV2.cs", "SaveUserPermissionsAsync", $"ERROR {ex.Message}");
            return (false, ex.Message);
        }
    }

    public async Task<IReadOnlyList<PagePermissionDto>> GetRoleDefaultsAsync(
        string roleTypeName, CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var svc = scope.ServiceProvider.GetRequiredService<IUserManagementService>();
            return await svc.GetRoleDefaultsAsync(roleTypeName, ct);
        }
        catch (Exception ex)
        {
            AppLog.Write("UserManagementApiServiceV2.cs", "GetRoleDefaultsAsync", $"ERROR {ex.Message}");
            return [];
        }
    }

    // =========================================================================
    // Staff-linked helpers
    // =========================================================================

    public async Task<StaffSelectDto?> GetStaffForUserCreateAsync(
        int staffId, CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var svc = scope.ServiceProvider.GetRequiredService<IUserManagementService>();
            return await svc.GetStaffForUserCreateAsync(staffId, ct);
        }
        catch (Exception ex)
        {
            AppLog.Write("UserManagementApiServiceV2.cs", "GetStaffForUserCreateAsync", $"ERROR {ex.Message}");
            return null;
        }
    }

    public async Task<bool> StaffHasUserAsync(int staffId, CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var svc = scope.ServiceProvider.GetRequiredService<IUserManagementService>();
            return await svc.StaffHasUserAsync(staffId, ct);
        }
        catch (Exception ex)
        {
            AppLog.Write("UserManagementApiServiceV2.cs", "StaffHasUserAsync", $"ERROR {ex.Message}");
            return false;
        }
    }

    public async Task<IReadOnlyList<StaffSelectDto>> GetStaffWithoutUsersAsync(CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var svc = scope.ServiceProvider.GetRequiredService<IUserManagementService>();
            return await svc.GetStaffWithoutUsersAsync(ct);
        }
        catch (Exception ex)
        {
            AppLog.Write("UserManagementApiServiceV2.cs", "GetStaffWithoutUsersAsync", $"ERROR {ex.Message}");
            return [];
        }
    }

    public async Task<IReadOnlyList<UserSelectDto>> GetUsersForSelectAsync(CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var svc = scope.ServiceProvider.GetRequiredService<IUserManagementService>();
            return await svc.GetUsersForSelectAsync(ct);
        }
        catch (Exception ex)
        {
            AppLog.Write("UserManagementApiServiceV2.cs", "GetUsersForSelectAsync", $"ERROR {ex.Message}");
            return [];
        }
    }

    // =========================================================================
    // Roles / Permissions / Page Access list pages
    // =========================================================================

    public async Task<IReadOnlyList<RoleListItemDto>> GetAllRoleTypesAsync(CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var svc = scope.ServiceProvider.GetRequiredService<IUserManagementService>();
            return await svc.GetAllRoleTypesAsync(ct);
        }
        catch (Exception ex)
        {
            AppLog.Write("UserManagementApiServiceV2.cs", "GetAllRoleTypesAsync", $"ERROR {ex.Message}");
            return [];
        }
    }

    public async Task<IReadOnlyList<FeatureListItemDto>> GetFeaturesAsync(CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var svc = scope.ServiceProvider.GetRequiredService<IUserManagementService>();
            return await svc.GetFeaturesAsync(ct);
        }
        catch (Exception ex)
        {
            AppLog.Write("UserManagementApiServiceV2.cs", "GetFeaturesAsync", $"ERROR {ex.Message}");
            return [];
        }
    }

    public async Task<IReadOnlyList<PageAccessRowDto>> GetPageAccessAsync(CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var svc = scope.ServiceProvider.GetRequiredService<IUserManagementService>();
            return await svc.GetPageAccessAsync(ct);
        }
        catch (Exception ex)
        {
            AppLog.Write("UserManagementApiServiceV2.cs", "GetPageAccessAsync", $"ERROR {ex.Message}");
            return [];
        }
    }

    // =========================================================================
    // User Profile page wrappers (v1.2.0)
    // =========================================================================

    public async Task<UserProfileHeaderDto?> GetUserProfileHeaderAsync(
        int userId, CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var svc = scope.ServiceProvider.GetRequiredService<IUserManagementService>();
            return await svc.GetUserProfileHeaderAsync(userId, ct);
        }
        catch (Exception ex)
        {
            AppLog.Write("UserManagementApiServiceV2.cs", "GetUserProfileHeaderAsync", $"ERROR {ex.Message}");
            return null;
        }
    }

    public async Task<UserProfileFullDetailDto> GetUserProfileDetailAsync(
        int userId, CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var svc = scope.ServiceProvider.GetRequiredService<IUserManagementService>();
            return await svc.GetUserProfileDetailAsync(userId, ct);
        }
        catch (Exception ex)
        {
            AppLog.Write("UserManagementApiServiceV2.cs", "GetUserProfileDetailAsync", $"ERROR {ex.Message}");
            return new UserProfileFullDetailDto
            {
                UserExists = false,
                UserId     = userId,
            };
        }
    }

    public async Task<StaffContactDto?> GetStaffContactAsync(
        int staffId, CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var svc = scope.ServiceProvider.GetRequiredService<IUserManagementService>();
            return await svc.GetStaffContactAsync(staffId, ct);
        }
        catch (Exception ex)
        {
            AppLog.Write("UserManagementApiServiceV2.cs", "GetStaffContactAsync", $"ERROR {ex.Message}");
            return null;
        }
    }

    public async Task<IReadOnlyList<StaffClassRowDto>> GetStaffClassAssignmentsAsync(
        int staffId, CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var svc = scope.ServiceProvider.GetRequiredService<IUserManagementService>();
            return await svc.GetStaffClassAssignmentsAsync(staffId, ct);
        }
        catch (Exception ex)
        {
            AppLog.Write("UserManagementApiServiceV2.cs", "GetStaffClassAssignmentsAsync", $"ERROR {ex.Message}");
            return [];
        }
    }

    public async Task<IReadOnlyList<StaffDeviceRowDto>> GetStaffDevicesAsync(
        int staffId, CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var svc = scope.ServiceProvider.GetRequiredService<IUserManagementService>();
            return await svc.GetStaffDevicesAsync(staffId, ct);
        }
        catch (Exception ex)
        {
            AppLog.Write("UserManagementApiServiceV2.cs", "GetStaffDevicesAsync", $"ERROR {ex.Message}");
            return [];
        }
    }

    public async Task<IReadOnlyList<StaffExternalRowDto>> GetStaffExternalAccountsAsync(
        int staffId, CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var svc = scope.ServiceProvider.GetRequiredService<IUserManagementService>();
            return await svc.GetStaffExternalAccountsAsync(staffId, ct);
        }
        catch (Exception ex)
        {
            AppLog.Write("UserManagementApiServiceV2.cs", "GetStaffExternalAccountsAsync", $"ERROR {ex.Message}");
            return [];
        }
    }

    public async Task<IReadOnlyList<StaffAbsenceRowDto>> GetStaffAbsencesAsync(
        int staffId, CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var svc = scope.ServiceProvider.GetRequiredService<IUserManagementService>();
            return await svc.GetStaffAbsencesAsync(staffId, ct);
        }
        catch (Exception ex)
        {
            AppLog.Write("UserManagementApiServiceV2.cs", "GetStaffAbsencesAsync", $"ERROR {ex.Message}");
            return [];
        }
    }

    public async Task<IReadOnlyList<LoginAuditRowDto>> GetUserLoginAuditAsync(
        int userId, CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var svc = scope.ServiceProvider.GetRequiredService<IUserManagementService>();
            return await svc.GetUserLoginAuditAsync(userId, ct);
        }
        catch (Exception ex)
        {
            AppLog.Write("UserManagementApiServiceV2.cs", "GetUserLoginAuditAsync", $"ERROR {ex.Message}");
            return [];
        }
    }

    public async Task<(bool Success, string? Error)> SaveUserProfileAsync(
        UserProfileSaveDto dto, CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var svc = scope.ServiceProvider.GetRequiredService<IUserManagementService>();
            await svc.SaveUserProfileAsync(dto, ct);
            return (true, null);
        }
        catch (Exception ex)
        {
            AppLog.Write("UserManagementApiServiceV2.cs", "SaveUserProfileAsync", $"ERROR {ex.Message}");
            return (false, ex.Message);
        }
    }

    public async Task<(bool Success, string? Error)> ChangePasswordAsync(
        ChangePasswordDto dto, CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var svc = scope.ServiceProvider.GetRequiredService<IUserManagementService>();
            await svc.ChangePasswordAsync(dto, ct);
            return (true, null);
        }
        catch (Exception ex)
        {
            AppLog.Write("UserManagementApiServiceV2.cs", "ChangePasswordAsync", $"ERROR {ex.Message}");
            return (false, ex.Message);
        }
    }

    public async Task<(bool Success, string? Error)> UpdateProfilePhotoAsync(
        int userId, string photoUrl, CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var svc = scope.ServiceProvider.GetRequiredService<IUserManagementService>();
            await svc.UpdateProfilePhotoAsync(userId, photoUrl, ct);
            return (true, null);
        }
        catch (Exception ex)
        {
            AppLog.Write("UserManagementApiServiceV2.cs", "UpdateProfilePhotoAsync", $"ERROR {ex.Message}");
            return (false, ex.Message);
        }
    }
}
