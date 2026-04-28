/*  
===============================================================================  
 File        : PermissionServiceV2.cs  
 Namespace   : AbsenceApp.Client.Services  
 Author      : Michael  
 Version     : 1.4.1  
 Created     : 2026-04-11  
 Updated     : 2026-04-24  
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
                         userrole → roles → roletypes chain. Eliminates the  
                         "Unknown column 'u.RoleTypeId'" startup crash.  
   - 1.3.0  2026-04-24  Task C fix: removed fail-open behaviour.  
                         (1) Added _loadFailed flag — set on DB exception, cleared  
                             by ResetAsync() so next login retries the load.  
                         (2) LoadAsync catch no longer sets _cache=[]; sets  
                             _loadFailed=true and leaves _cache=null instead.  
                         (3) GetAsync and CanViewAsync check _loadFailed and  
                             return Deny / false immediately.  
                         (4) CanViewAsync final fall-through changed from true  
                             to false — unknown routes now deny by default.  
   - 1.4.0  2026-04-24  Phase 0 (E15 migration): introduced TEMPORARY fail-open  
                         behaviour while E15 tables are being implemented, plus  
                         additional AppLog.Write diagnostics in GetAsync and  
                         CanViewAsync when _loadFailed=true. This version is  
                         explicitly for development/testing and will be reverted  
                         to fail-closed once the E15 schema is complete.  
   - 1.4.1  2026-04-24  Added post-load verification log in LoadAsync to confirm  
                         AppPages, RoleDefaultPagePermissions, UserPagePermissions,  
                         and UserPageOverrides row counts after cache build.  
-------------------------------------------------------------------------------  
 Notes       :  
   - Registered as Singleton in V2ServiceCollectionExtensions.cs.  
   - IServiceScopeFactory is used to resolve the Scoped AppDbContext from  
     a Singleton service — same pattern as NavigationApiServiceV2.  
   - In 1.4.0, _loadFailed=true triggers TEMPORARY fail-open behaviour so the  
     menu sidebar remains visible during E15 rollout. This is a development  
     mode only and must be reverted to fail-closed in a later version.  
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
    // =========================================================================
    // Dependencies
    // =========================================================================

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly AppStateService      _appState;

    public PermissionServiceV2(IServiceScopeFactory scopeFactory, AppStateService appState)
    {
        _scopeFactory = scopeFactory;
        _appState     = appState;
    }

    // =========================================================================
    // Cache — cleared on Reset()
    // =========================================================================

    private readonly SemaphoreSlim _lock = new(1, 1);

    // key = pageRoute (lower-case)
    private Dictionary<string, EffectivePermissionDto>? _cache;

    /// <summary>
    /// Set to true when LoadAsync encounters a DB error. Remains set until
    /// ResetAsync() is called (e.g. on logout) so the next authenticated request
    /// retries the load. While true, all permission checks return deny in
    /// production, but 1.4.0 temporarily fails open for development.
    /// </summary>
    private bool _loadFailed;

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
                    $"_loadFailed=true → TEMP FAIL-OPEN → returning ALLOW for route='{pageRoute}'");

                return new EffectivePermissionDto
                {
                    PageRoute = pageRoute,
                    CanRead   = true,
                    CanWrite  = true,
                    CanCreate = true,
                    CanDelete = true,
                    CanImport = true,
                    CanExport = true
                };
            }

            if (_cache!.TryGetValue(key, out var hit))
            {
                AppLog.Write("PermissionServiceV2.cs", "GetAsync",
                    $"Cache HIT for route='{pageRoute}' (key='{key}') → CanRead={hit.CanRead}");
                return hit;
            }

            AppLog.Write("PermissionServiceV2.cs", "GetAsync",
                $"Cache MISS for route='{pageRoute}' (key='{key}') → returning Deny()");
        }
        finally
        {
            _lock.Release();
        }

        return Deny(pageRoute);
    }

    public async Task<bool> CanViewAsync(string route, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(route))
        {
            AppLog.Write("PermissionServiceV2.cs", "CanViewAsync",
                "route is null/empty/whitespace → returning true");
            return true;
        }

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
                    $"_loadFailed=true → TEMP FAIL-OPEN → returning true for route='{route}'");
                return true;
            }

            if (_cache!.TryGetValue(key, out var perm))
            {
                AppLog.Write("PermissionServiceV2.cs", "CanViewAsync",
                    $"Cache HIT for route='{route}' (key='{key}') → CanRead={perm.CanRead}");
                return perm.CanRead;
            }
        }
        finally
        {
            _lock.Release();
        }

        AppLog.Write("PermissionServiceV2.cs", "CanViewAsync",
            $"Route '{route}' (key='{key}') not found in cache → returning false (deny by default)");
        return false;
    }

    public async Task ResetAsync(CancellationToken ct = default)
    {
        await _lock.WaitAsync(ct);
        try
        {
            _cache      = null;
            _loadFailed = false;
            AppLog.Write("PermissionServiceV2.cs", "ResetAsync", "Permission cache cleared; _loadFailed reset to false.");
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
        AppLog.Write("PermissionServiceV2.cs", "LoadAsync", $"Loading permissions for userId={userId}");

        try
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var roleTypeNames = await db.Database
                .SqlQueryRaw<string>(
                    "SELECT rt.Name " +
                    "FROM userrole ur " +
                    "INNER JOIN roles r  ON r.Id  = ur.RoleId " +
                    "INNER JOIN roletypes rt ON rt.Id = r.RoleTypeId " +
                    "WHERE ur.UserId = @UserId LIMIT 1",
                    new MySqlParameter("@UserId", userId))
                .ToListAsync(ct);
            var roleTypeName = roleTypeNames.FirstOrDefault() ?? string.Empty;

            AppLog.Write("PermissionServiceV2.cs", "LoadAsync", $"roleTypeName='{roleTypeName}'");

            var pages = await db.AppPages
                .AsNoTracking()
                .Where(p => p.IsActive)
                .ToListAsync(ct);

            AppLog.Write("PermissionServiceV2.cs", "LoadAsync",
                $"Loaded {pages.Count} active AppPage record(s) from app_pages.");

            var userPerms = await db.UserPagePermissions
                .AsNoTracking()
                .Where(p => p.UserId == userId)
                .ToListAsync(ct);

            AppLog.Write("PermissionServiceV2.cs", "LoadAsync",
                $"Loaded {userPerms.Count} UserPagePermission record(s) for userId={userId}.");

            var userPermMap = userPerms.ToDictionary(p => p.PageId);

            var roleDefaults = await db.RoleDefaultPagePermissions
                .AsNoTracking()
                .Where(r => r.RoleTypeName == roleTypeName)
                .ToListAsync(ct);

            AppLog.Write("PermissionServiceV2.cs", "LoadAsync",
                $"Loaded {roleDefaults.Count} RoleDefaultPagePermission record(s) for roleTypeName='{roleTypeName}'.");

            var roleDefaultMap = roleDefaults.ToDictionary(r => r.PageId);

            var cache = new Dictionary<string, EffectivePermissionDto>(StringComparer.OrdinalIgnoreCase);

            foreach (var page in pages)
            {
                EffectivePermissionDto dto;

                if (userPermMap.TryGetValue(page.Id, out var up))
                {
                    dto = new EffectivePermissionDto
                    {
                        PageRoute = page.Route,
                        CanRead   = up.CanRead,
                        CanWrite  = up.CanWrite,
                        CanCreate = up.CanCreate,
                        CanDelete = up.CanDelete,
                        CanImport = up.CanImport,
                        CanExport = up.CanExport,
                    };

                    AppLog.Write("PermissionServiceV2.cs", "LoadAsync",
                        $"Effective permissions (USER OVERRIDE) for pageId={page.Id}, route='{page.Route}' → " +
                        $"R={dto.CanRead}, W={dto.CanWrite}, C={dto.CanCreate}, D={dto.CanDelete}, " +
                        $"I={dto.CanImport}, E={dto.CanExport}");
                }
                else if (roleDefaultMap.TryGetValue(page.Id, out var rd))
                {
                    dto = new EffectivePermissionDto
                    {
                        PageRoute = page.Route,
                        CanRead   = rd.CanRead,
                        CanWrite  = rd.CanWrite,
                        CanCreate = rd.CanCreate,
                        CanDelete = rd.CanDelete,
                        CanImport = rd.CanImport,
                        CanExport = rd.CanExport,
                    };

                    AppLog.Write("PermissionServiceV2.cs", "LoadAsync",
                        $"Effective permissions (ROLE DEFAULT) for pageId={page.Id}, route='{page.Route}' → " +
                        $"R={dto.CanRead}, W={dto.CanWrite}, C={dto.CanCreate}, D={dto.CanDelete}, " +
                        $"I={dto.CanImport}, E={dto.CanExport}");
                }
                else
                {
                    dto = Deny(page.Route);

                    AppLog.Write("PermissionServiceV2.cs", "LoadAsync",
                        $"Effective permissions (DENY ALL) for pageId={page.Id}, route='{page.Route}' → " +
                        "no user override and no role default found.");
                }

                cache[page.Route.ToLowerInvariant()] = dto;
            }

            _cache      = cache;
            _loadFailed = false;

            AppLog.Write("PermissionServiceV2.cs", "LoadAsync",
                $"Cache loaded — {cache.Count} page(s) resolved for userId={userId}; _loadFailed=false.");

            // -----------------------------------------------------------------
            // NEW IN 1.4.1 — Post-load verification log
            // -----------------------------------------------------------------
            AppLog.Write("PermissionServiceV2.cs", "LoadAsync",
                $"Post-load verification → AppPages={pages.Count}, RoleDefaults={roleDefaults.Count}, " +
                $"UserPerms={userPerms.Count}, Overrides={(await db.UserPageOverrides.CountAsync(ct))}");
        }
        catch (Exception ex)
        {
            AppLog.Write("PermissionServiceV2.cs", "LoadAsync",
                $"LOAD FAILED — {ex.GetType().Name}: {ex.Message}. " +
                "In 1.4.0, _loadFailed=true will trigger TEMP FAIL-OPEN behaviour " +
                "so the menu sidebar remains visible during E15 rollout.");
            _loadFailed = true;
            _cache      = null;
        }
    }

    // =========================================================================
    // Helper
    // =========================================================================

    private static EffectivePermissionDto Deny(string route) =>
        new() { PageRoute = route };
}
