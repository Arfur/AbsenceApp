/*
===============================================================================
 File        : PermissionServiceV2.cs
 Namespace   : AbsenceApp.Client.Services
 Author      : Michael
 Version     : 1.1.0
 Created     : 2026-04-11
 Updated     : 2026-04-17
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
   - 1.1.0  2026-04-17  E17 Navigation Unification: added CanViewAsync(route)
                         used by NavigationApiServiceV2 to filter sidebar items.
                         Returns false only when the route is a registered, active
                         AppPage AND the current user has CanRead = false.
                         Empty and unregistered routes always return true.
-------------------------------------------------------------------------------
 Notes       :
   - Registered as Singleton in V2ServiceCollectionExtensions.cs.
   - IServiceScopeFactory is used to resolve the Scoped AppDbContext from
     a Singleton service — same pattern as NavigationApiServiceV2.
   - Returns all-false EffectivePermissionDto on any error (fail-safe).
===============================================================================
*/

using AbsenceApp.Client.Shared;
using AbsenceApp.Core.DTOs;
using AbsenceApp.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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

    // =========================================================================
    // Public API
    // =========================================================================

    /// <summary>
    /// Returns effective CRUD flags for the given page route for the current
    /// authenticated user. Returns all-false on any error.
    /// </summary>
    public async Task<EffectivePermissionDto> GetAsync(string pageRoute, CancellationToken ct = default)
    {
        var key = pageRoute.ToLowerInvariant();

        await _lock.WaitAsync(ct);
        try
        {
            if (_cache is null)
                await LoadAsync(ct);

            if (_cache!.TryGetValue(key, out var hit))
                return hit;
        }
        finally
        {
            _lock.Release();
        }

        return Deny(pageRoute);
    }

    /// <summary>
    /// Returns false only when <paramref name="route"/> is a registered AppPage
    /// AND the current user has CanRead = false.  Returns true for empty/null
    /// routes and for routes not in the AppPage registry (unregistered routes
    /// are always visible — they are not permission-controlled).
    /// </summary>
    public async Task<bool> CanViewAsync(string route, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(route))
            return true;

        var key = route.ToLowerInvariant();

        await _lock.WaitAsync(ct);
        try
        {
            if (_cache is null)
                await LoadAsync(ct);

            if (_cache!.TryGetValue(key, out var perm))
                return perm.CanRead;
        }
        finally
        {
            _lock.Release();
        }

        // Route is not in the AppPage registry — always visible.
        return true;
    }

    /// <summary>
    /// Clears the in-memory cache. Call on logout or after permission edits.
    /// </summary>
    public async Task ResetAsync(CancellationToken ct = default)
    {
        await _lock.WaitAsync(ct);
        try
        {
            _cache = null;
            AppLog.Write("PermissionServiceV2.cs", "ResetAsync", "Permission cache cleared.");
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

            // Resolve the user's RoleType.Name (string slug, not Id)
            var roleTypeName = await db.Users
                .AsNoTracking()
                .Where(u => u.Id == userId)
                .Join(db.RoleTypes, u => u.RoleTypeId, r => r.Id, (u, r) => r.Name)
                .FirstOrDefaultAsync(ct) ?? string.Empty;

            AppLog.Write("PermissionServiceV2.cs", "LoadAsync", $"roleTypeName='{roleTypeName}'");

            // Load all active pages
            var pages = await db.AppPages
                .AsNoTracking()
                .Where(p => p.IsActive)
                .ToListAsync(ct);

            // Load user-specific overrides
            var userPerms = await db.UserPagePermissions
                .AsNoTracking()
                .Where(p => p.UserId == userId)
                .ToListAsync(ct);

            var userPermMap = userPerms.ToDictionary(p => p.PageId);

            // Load role defaults
            var roleDefaults = await db.RoleDefaultPagePermissions
                .AsNoTracking()
                .Where(r => r.RoleTypeName == roleTypeName)
                .ToListAsync(ct);

            var roleDefaultMap = roleDefaults.ToDictionary(r => r.PageId);

            // Resolve effective permissions
            var cache = new Dictionary<string, EffectivePermissionDto>(StringComparer.OrdinalIgnoreCase);

            foreach (var page in pages)
            {
                EffectivePermissionDto dto;

                if (userPermMap.TryGetValue(page.Id, out var up))
                {
                    // Priority 1: user-specific permission row
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
                }
                else if (roleDefaultMap.TryGetValue(page.Id, out var rd))
                {
                    // Priority 2: role default
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
                }
                else
                {
                    // Priority 3: deny all
                    dto = Deny(page.Route);
                }

                cache[page.Route.ToLowerInvariant()] = dto;
            }

            _cache = cache;
            AppLog.Write("PermissionServiceV2.cs", "LoadAsync",
                $"Cache loaded — {cache.Count} page(s) resolved for userId={userId}");
        }
        catch (Exception ex)
        {
            AppLog.Write("PermissionServiceV2.cs", "LoadAsync",
                $"ERROR {ex.GetType().Name}: {ex.Message} — using empty cache");
            _cache = [];
        }
    }

    // =========================================================================
    // Helper
    // =========================================================================

    private static EffectivePermissionDto Deny(string route) =>
        new() { PageRoute = route };
}
