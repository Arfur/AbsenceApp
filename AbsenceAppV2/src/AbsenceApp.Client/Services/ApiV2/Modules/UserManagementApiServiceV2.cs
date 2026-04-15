/*
===============================================================================
 File        : UserManagementApiServiceV2.cs
 Namespace   : AbsenceApp.Client.Services.ApiV2.Modules
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-04-11
 Updated     : 2026-04-11
-------------------------------------------------------------------------------
 Purpose     : Client-side User Management API service. Provides full user
               CRUD, role/page reference data, and per-user permission matrix
               read/write by delegating to IUserManagementService via EF Core
               direct DB access (IServiceScopeFactory pattern).

               In MAUI Blazor Hybrid the C# HttpClient cannot reach
               http://localhost/ (that scheme exists only inside the WebView2
               browser context). This service therefore resolves the scoped
               IUserManagementService from a DI scope instead of making an HTTP
               call to AbsenceApp.Api.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-11  Initial creation (E15 User Management).
-------------------------------------------------------------------------------
 Notes       :
   - Registered as Scoped in V2ServiceCollectionExtensions.cs.
   - Returns false / null / empty on any error (fail-safe pattern).
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

    public async Task<UserUpdateDto?> GetUserForEditAsync(long userId, CancellationToken ct = default)
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
        long userId, CancellationToken ct = default)
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
}
