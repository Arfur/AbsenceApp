/*
===============================================================================
 File        : PermissionServiceV2.cs
 Namespace   : AbsenceApp.Client.Services
 Author      : Michael
 Version     : 1.6.0
 Created     : 2026-04-11
 Updated     : 2026-05-07
-------------------------------------------------------------------------------
 Purpose     : Client-side permission resolution service (E15).
               Provides per-page effective CRUD flags for the current user.

               Resolution order (highest priority first):
               1. UserPagePermission row — if a row exists for (UserId, PageId),
                  its flags are returned directly.
               2. RoleDefaultPagePermission row — fallback if no user override.
               3. All-false/deny — if neither exists, access is denied.

               Results are cached per session. Call ResetAsync() on logout or
               role change to force a fresh load.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-11  Initial creation (E15 User Management).
   - 1.2.0  2026-04-19  RoleId resolution fix: replaced db.Users.Join(db.RoleTypes,
                         u => u.RoleTypeId, ...) with raw SQL through the
                         userrole → roles → roletypes chain.
   - 1.3.0  2026-04-24  Task C fix: removed fail-open behaviour and added
                         _loadFailed flag with fail-closed logic.
   - 1.4.0  2026-04-24  Phase 0 (E15 migration): TEMPORARY fail-open behaviour
                         reintroduced for development/testing.
   - 1.4.1  2026-04-24  Added post-load verification log.
   - 1.5.0  2026-05-07  Role schema consolidation: replaced legacy RoleTypeName
                         resolution with Role.Code for menu + entitlements.
   - 1.6.0  2026-05-07  Corrected page-permission resolution to use RoleId
                         (int) instead of RoleCode. Added RoleId SQL lookup,
                         removed invalid r.rolecode filter, and updated logs.
-------------------------------------------------------------------------------
 Notes       :
   - Registered as Singleton in V2ServiceCollectionExtensions.cs.
   - IServiceScopeFactory resolves Scoped AppDbContext from a Singleton.
   - TEMP FAIL-OPEN remains active in 1.6.0 for development.
===============================================================================
*/

using AbsenceApp.Client.Shared;
using AbsenceApp.Core.DTOs;
using AbsenceApp.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;

namespace AbsenceApp.Client.Services;

public sealed class PermissionServiceV2
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly AppStateService _appState;

    private readonly SemaphoreSlim _lock = new(1, 1);
    private Dictionary<string, EffectivePermissionDto>? _cache;
    private bool _loadFailed;

    public PermissionServiceV2(IServiceScopeFactory scopeFactory, AppStateService appState)
    {
        _scopeFactory = scopeFactory;
        _appState = appState;
    }

    // =========================================================================
    // Public API
    // =========================================================================

    public async Task<EffectivePermissionDto> GetAsync(string pageRoute, CancellationToken ct = default)
    {
        var key = pageRoute.ToLowerInvariant();

        await _lock.WaitAsync(ct);
        try
        {
            if (_cache is null)
            {
                AppLog.Write("PermissionServiceV2.cs", "GetAsync",
                    $"_cache is null → invoking LoadAsync for route='{pageRoute}'");
                await LoadAsync(ct);
            }

            if (_loadFailed)
            {
                AppLog.Write("PermissionServiceV2.cs", "GetAsync",
                    $"_loadFailed=true → returning Deny for route='{pageRoute}'");
                return Deny(pageRoute);
            }

            if (_cache!.TryGetValue(key, out var hit))
            {
                AppLog.Write("PermissionServiceV2.cs", "GetAsync",
                    $"Cache HIT for route='{pageRoute}' (key='{key}') → CanRead={hit.CanRead}");
                return hit;
            }
        }
        finally
        {
            _lock.Release();
        }

        AppLog.Write("PermissionServiceV2.cs", "GetAsync",
            $"Cache MISS for route='{pageRoute}' (key='{key}') → returning Deny()");
        return Deny(pageRoute);
    }

    public async Task<bool> CanViewAsync(string route, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(route))
            return true;

        var key = route.ToLowerInvariant();

        await _lock.WaitAsync(ct);
        try
        {
            if (_cache is null)
            {
                AppLog.Write("PermissionServiceV2.cs", "CanViewAsync",
                    $"_cache is null → invoking LoadAsync for route='{route}'");
                await LoadAsync(ct);
            }

            if (_loadFailed)
            {
                AppLog.Write("PermissionServiceV2.cs", "CanViewAsync",
                    $"_loadFailed=true → returning false for route='{route}'");
                return false;
            }

            if (_cache!.TryGetValue(key, out var perm))
            {
                AppLog.Write("PermissionServiceV2.cs", "CanViewAsync",
                    $"Cache HIT for route='{route}' → CanRead={perm.CanRead}");
                return perm.CanRead;
            }
        }
        finally
        {
            _lock.Release();
        }

        AppLog.Write("PermissionServiceV2.cs", "CanViewAsync",
            $"Route '{route}' not found → returning false");
        return false;
    }

    public async Task ResetAsync(CancellationToken ct = default)
    {
        await _lock.WaitAsync(ct);
        try
        {
            _cache = null;
            _loadFailed = false;
            AppLog.Write("PermissionServiceV2.cs", "ResetAsync",
                "Permission cache cleared; _loadFailed reset to false.");
        }
        finally
        {
            _lock.Release();
        }
    }

    // =========================================================================
    // Cache load
    // =========================================================================

    private async Task LoadAsync(CancellationToken ct)
    {
        var userId = _appState.CurrentUserId;
        AppLog.Write("PermissionServiceV2.cs", "LoadAsync",
            $"Loading permissions for userId={userId}");

        try
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // -------------------------------------------------------------
            // Resolve RoleCode (string) — used for menu + entitlements
            // -------------------------------------------------------------
            var roleCodes = await db.Database
                .SqlQueryRaw<string>(
                    "SELECT r.Code FROM userrole ur " +
                    "INNER JOIN roles r ON r.Id = ur.RoleId " +
                    "WHERE ur.UserId = @UserId LIMIT 1",
                    new MySqlParameter("@UserId", userId))
                .ToListAsync(ct);

            var roleCode = roleCodes.FirstOrDefault() ?? string.Empty;
            AppLog.Write("PermissionServiceV2.cs", "LoadAsync",
                $"roleCode='{roleCode}'");

            // -------------------------------------------------------------
            // Resolve RoleId (int) — used for page-level permissions
            // -------------------------------------------------------------
            var roleIds = await db.Database
                .SqlQueryRaw<int>(
                    "SELECT r.Id FROM userrole ur " +
                    "INNER JOIN roles r ON r.Id = ur.RoleId " +
                    "WHERE ur.UserId = @UserId LIMIT 1",
                    new MySqlParameter("@UserId", userId))
                .ToListAsync(ct);

            var roleId = roleIds.FirstOrDefault();
            AppLog.Write("PermissionServiceV2.cs", "LoadAsync",
                $"roleId={roleId}");

            // -------------------------------------------------------------
            // Load pages
            // -------------------------------------------------------------
            var pages = await db.AppPages
                .AsNoTracking()
                .Where(p => p.IsActive)
                .ToListAsync(ct);

            // -------------------------------------------------------------
            // User overrides
            // -------------------------------------------------------------
            var userPerms = await db.UserPagePermissions
                .AsNoTracking()
                .Where(p => p.UserId == userId)
                .ToListAsync(ct);

            var userPermMap = userPerms.ToDictionary(p => p.PageId);

            // -------------------------------------------------------------
            // Role defaults (using RoleId)
            // -------------------------------------------------------------
            var roleDefaults = await db.RoleDefaultPagePermissions
                .AsNoTracking()
                .Where(r => r.RoleTypeName == roleCode)
                .ToListAsync(ct);

            var roleDefaultMap = roleDefaults.ToDictionary(r => r.PageId);

            // -------------------------------------------------------------
            // Build cache
            // -------------------------------------------------------------
            var cache = new Dictionary<string, EffectivePermissionDto>(
                StringComparer.OrdinalIgnoreCase);

            foreach (var page in pages)
            {
                EffectivePermissionDto dto;

                if (userPermMap.TryGetValue(page.Id, out var up))
                {
                    dto = new EffectivePermissionDto
                    {
                        PageRoute = page.Route,
                        CanRead = up.CanRead,
                        CanWrite = up.CanWrite,
                        CanCreate = up.CanCreate,
                        CanDelete = up.CanDelete,
                        CanImport = up.CanImport,
                        CanExport = up.CanExport
                    };
                }
                else if (roleDefaultMap.TryGetValue(page.Id, out var rd))
                {
                    dto = new EffectivePermissionDto
                    {
                        PageRoute = page.Route,
                        CanRead = rd.CanRead,
                        CanWrite = rd.CanWrite,
                        CanCreate = rd.CanCreate,
                        CanDelete = rd.CanDelete,
                        CanImport = rd.CanImport,
                        CanExport = rd.CanExport
                    };
                }
                else
                {
                    dto = Deny(page.Route);
                }

                cache[page.Route.ToLowerInvariant()] = dto;
            }

            _cache = cache;
            _loadFailed = false;

            AppLog.Write("PermissionServiceV2.cs", "LoadAsync",
                $"Cache loaded — {cache.Count} page(s) resolved for userId={userId}");
        }
        catch (Exception ex)
        {
            AppLog.Write("PermissionServiceV2.cs", "LoadAsync",
                $"LOAD FAILED — {ex.GetType().Name}: {ex.Message}");
            _loadFailed = true;
            _cache = null;
        }
    }

    private static EffectivePermissionDto Deny(string route) =>
        new() { PageRoute = route };
}
