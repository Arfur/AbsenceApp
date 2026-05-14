/*
===============================================================================
 File        : NavigationServiceV2.cs
 Namespace   : AbsenceApp.Client.Services
 Author      : Michael
 Version     : 4.4.0
   Created     : 2026-03-21
   Updated     : 2026-04-12
-------------------------------------------------------------------------------
 Purpose     : V2 navigation service. Retrieves the sidebar menu structure
               from the database via NavigationApiServiceV2.
               The DB is authoritative for menu content and role-based
               visibility. This service performs no filtering of any kind.

               The main menu and the Global Settings menu are both served by
               the same DB call — the TVF decides what each role sees.
               The client caches per-session with ResetAsync() clearing the
               cache on logout.

               This service operates in parallel to the V1 navigation system
               and does not modify or replace it.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-21  Initial implementation.
   - 2.0.0  2026-03-23  Upgraded to parse menu.json v4.0.0 category structure.
   - 2.1.0  2026-03-26  Added GetGlobalSettingsMenuGroupsAsync().
   - 2.2.0  2026-03-26  Phase 3 layout repair; no functional changes.
   - 2.3.0  2026-04-05  Inert feature-flag seam for DB-driven menus.
   - 3.0.0  2026-04-06  Option A refactor: replaced all JSON-file loading
                         with NavigationApiServiceV2 (GET /api/menu).
                         Removed DesignSystemConfigService dependency.
                         Removed JSON/System.Text.Json.Nodes references.
                         Added Reset() for logout cache clearing.
                         GetGlobalSettingsMenuGroupsAsync() now extracts
                         globalsettings groups from the same API response.
   - 4.0.0  2026-04-06  Schema alignment: replaced all i.Href references with
                         i.Route to match MenuItemModel.Route (was Href).
                         GetGlobalSettingsMenuGroupsAsync() updated to also
                         detect leaf groups (no Items) whose own Route starts
                         with /globalsettings. FindItemByHrefAsync updated
                         to match on i.Route.
   - 4.1.0  2026-04-07  Debug instrumentation: added AppLog.Write calls to
                         GetMenuCategoriesAsync (cache-hit/miss/lock/DB paths),
                         GetGlobalSettingsMenuGroupsAsync, GetMenuGroupsAsync,
                         FindItemByHrefAsync, GetAllItemsAsync, and Reset().
                         Reset() logged BEFORE and AFTER _lock.Wait() so
                         the synchronous block duration was visible.
   - 4.2.0  2026-04-07  ***CRITICAL FIX*** Converted Reset() to ResetAsync()
                         and replaced synchronous _lock.Wait() with
                         await _lock.WaitAsync() to eliminate UI freeze
                         during login when BreadcrumbV2 holds the async lock.
   - 4.3.0  2026-04-10  Added GetGlobalSettingsCategoriesAsync() returning a
                         filtered List<MenuCategoryModel> for globalsettings
                         mode. Preserves category structure (labels + groups)
                         so SidebarV2 can render section headers in the
                         globalsettings sidebar.
   - 4.4.0  2026-04-12  Reworked GetGlobalSettingsCategoriesAsync() to call
                         NavigationApiServiceV2.GetGlobalSettingsCategoriesAsync()
                         which reads dbo.MenuItemsGlobalSettings directly.
                         Added separate _globalSettingsCache + _gsLock for the
                         global settings hierarchy (same double-check pattern
                         as the main menu cache). Updated
                         GetGlobalSettingsMenuGroupsAsync() to flatten groups
                         from GetGlobalSettingsCategoriesAsync() instead of
                         filtering the main menu. Updated ResetAsync() to also
                         clear the global settings cache.
-------------------------------------------------------------------------------
 Notes       :
   - No role checks, entitlement checks, or menu filtering on the client.
   - SidebarV2 renders exactly what this service returns.
===============================================================================
*/

using AbsenceApp.Client.Models.V2;
using AbsenceApp.Client.Services.ApiV2.Modules;

namespace AbsenceApp.Client.Services;

public sealed class NavigationServiceV2
{
    // ---------------------------------------------------------------------------
    // Dependencies
    // ---------------------------------------------------------------------------

    private readonly NavigationApiServiceV2 _api;

    // ---------------------------------------------------------------------------
    // Cache — cleared on ResetAsync() (called at logout)
    // ---------------------------------------------------------------------------

    private List<MenuCategoryModel>? _cache;
    private readonly SemaphoreSlim _lock = new(1, 1);

    // Global settings has its own independent cache and lock so that
    // the main-menu and globalsettings read paths never contend.
    private List<MenuCategoryModel>? _globalSettingsCache;
    private readonly SemaphoreSlim _gsLock = new(1, 1);

    public NavigationServiceV2(NavigationApiServiceV2 api)
    {
        _api = api;
    }

    // ===========================================================================
    // GetMenuCategoriesAsync
    // Returns all categories from the API (main menu).
    // ===========================================================================

    public async Task<List<MenuCategoryModel>> GetMenuCategoriesAsync()
    {
        if (_cache is not null)
        {
            AppLog.Write("NavigationServiceV2.cs", "GetMenuCategoriesAsync",
                $"CACHE HIT — returning {_cache.Count} cached categories");
            return _cache;
        }

        AppLog.Write("NavigationServiceV2.cs", "GetMenuCategoriesAsync",
            "CACHE MISS — acquiring async lock");
        await _lock.WaitAsync();
        AppLog.Write("NavigationServiceV2.cs", "GetMenuCategoriesAsync",
            "Lock acquired");
        try
        {
            if (_cache is not null)
            {
                AppLog.Write("NavigationServiceV2.cs", "GetMenuCategoriesAsync",
                    $"Double-check: cache was populated while we waited — returning {_cache.Count} cached categories");
                return _cache;
            }
            AppLog.Write("NavigationServiceV2.cs", "GetMenuCategoriesAsync",
                "Requesting from NavigationApiServiceV2");
            _cache = await _api.GetMenuCategoriesAsync();
            AppLog.Write("NavigationServiceV2.cs", "GetMenuCategoriesAsync",
                $"NavigationApiServiceV2 returned — caching {_cache?.Count ?? 0} categories");
        }
        finally
        {
            _lock.Release();
            AppLog.Write("NavigationServiceV2.cs", "GetMenuCategoriesAsync",
                "Lock released");
        }

        return _cache!;
    }

    // ===========================================================================
    // GetGlobalSettingsMenuGroupsAsync
    // Returns the flat list of all groups from the Global Settings menu.
    // Delegates to GetGlobalSettingsCategoriesAsync() so both methods share
    // the same cached, correctly-sourced data.
    // ===========================================================================

    public async Task<List<MenuGroupModel>> GetGlobalSettingsMenuGroupsAsync()
    {
        AppLog.Write("NavigationServiceV2.cs", "GetGlobalSettingsMenuGroupsAsync",
            "ENTER");
        var categories = await GetGlobalSettingsCategoriesAsync();
        var groups = categories
            .SelectMany(c => c.Groups)
            .ToList();
        AppLog.Write("NavigationServiceV2.cs", "GetGlobalSettingsMenuGroupsAsync",
            $"Returning {groups.Count} globalsettings groups");
        return groups;
    }

    // ===========================================================================
    // GetGlobalSettingsCategoriesAsync
    // Returns the full category → group → submenu hierarchy for the Global
    // Settings sidebar by reading dbo.MenuItemsGlobalSettings via the API service.
    // Cached separately from the main menu; cleared by ResetAsync().
    // ===========================================================================

    public async Task<List<MenuCategoryModel>> GetGlobalSettingsCategoriesAsync()
    {
        if (_globalSettingsCache is not null)
        {
            AppLog.Write("NavigationServiceV2.cs", "GetGlobalSettingsCategoriesAsync",
                $"CACHE HIT — returning {_globalSettingsCache.Count} cached categories");
            return _globalSettingsCache;
        }

        AppLog.Write("NavigationServiceV2.cs", "GetGlobalSettingsCategoriesAsync",
            "CACHE MISS — acquiring async lock");
        await _gsLock.WaitAsync();
        AppLog.Write("NavigationServiceV2.cs", "GetGlobalSettingsCategoriesAsync",
            "Lock acquired");
        try
        {
            if (_globalSettingsCache is not null)
            {
                AppLog.Write("NavigationServiceV2.cs", "GetGlobalSettingsCategoriesAsync",
                    $"Double-check: cache was populated while we waited — returning {_globalSettingsCache.Count} cached categories");
                return _globalSettingsCache;
            }
            AppLog.Write("NavigationServiceV2.cs", "GetGlobalSettingsCategoriesAsync",
                "Requesting from NavigationApiServiceV2.GetGlobalSettingsCategoriesAsync");
            _globalSettingsCache = await _api.GetGlobalSettingsCategoriesAsync();
            AppLog.Write("NavigationServiceV2.cs", "GetGlobalSettingsCategoriesAsync",
                $"NavigationApiServiceV2 returned — caching {_globalSettingsCache?.Count ?? 0} categories");
        }
        finally
        {
            _gsLock.Release();
            AppLog.Write("NavigationServiceV2.cs", "GetGlobalSettingsCategoriesAsync",
                "Lock released");
        }

        return _globalSettingsCache!;
    }

    // ===========================================================================
    // GetMenuGroupsAsync
    // ===========================================================================

    public async Task<List<MenuGroupModel>> GetMenuGroupsAsync()
    {
        AppLog.Write("NavigationServiceV2.cs", "GetMenuGroupsAsync",
            "ENTER");
        var categories = await GetMenuCategoriesAsync();
        var groups = categories.SelectMany(c => c.Groups).ToList();
        AppLog.Write("NavigationServiceV2.cs", "GetMenuGroupsAsync",
            $"Returning {groups.Count} groups");
        return groups;
    }

    // ===========================================================================
    // FindItemByHrefAsync
    // ===========================================================================

    public async Task<MenuItemModel?> FindItemByHrefAsync(string href)
    {
        AppLog.Write("NavigationServiceV2.cs", "FindItemByHrefAsync",
            $"ENTER href='{href}'");
        var categories = await GetMenuCategoriesAsync();
        var item = categories
            .SelectMany(c => c.Groups)
            .SelectMany(g => g.Items)
            .FirstOrDefault(i =>
                string.Equals(i.Route, href, StringComparison.OrdinalIgnoreCase));
        AppLog.Write("NavigationServiceV2.cs", "FindItemByHrefAsync",
            $"Result: {(item is null ? "null (not found)" : $"found '{item.Title}' at '{item.Route}'")} for href='{href}'");
        return item;
    }

    // ===========================================================================
    // GetAllItemsAsync
    // ===========================================================================

    public async Task<List<MenuItemModel>> GetAllItemsAsync()
    {
        AppLog.Write("NavigationServiceV2.cs", "GetAllItemsAsync",
            "ENTER");
        var categories = await GetMenuCategoriesAsync();
        var items = categories
            .SelectMany(c => c.Groups)
            .SelectMany(g => g.Items)
            .ToList();
        AppLog.Write("NavigationServiceV2.cs", "GetAllItemsAsync",
            $"Returning {items.Count} items");
        return items;
    }

    // ===========================================================================
    // ResetAsync  (FIXED)
    // Clears the cache so the next call re-fetches from the API.
    // Called from Login.razor on logout or re-login.
    // ===========================================================================

    public async Task ResetAsync()
    {
        AppLog.Write("NavigationServiceV2.cs", "ResetAsync",
            $"ENTER ResetAsync() — _cache is {(_cache is null ? "null" : $"populated ({_cache.Count} categories)")} — acquiring async lock");

        await _lock.WaitAsync();
        AppLog.Write("NavigationServiceV2.cs", "ResetAsync",
            "Lock acquired — clearing main menu cache");

        try
        {
            _cache = null;
        }
        finally
        {
            _lock.Release();
            AppLog.Write("NavigationServiceV2.cs", "ResetAsync",
                "Main menu cache cleared — lock released");
        }

        await _gsLock.WaitAsync();
        AppLog.Write("NavigationServiceV2.cs", "ResetAsync",
            "gsLock acquired — clearing global settings cache");

        try
        {
            _globalSettingsCache = null;
        }
        finally
        {
            _gsLock.Release();
            AppLog.Write("NavigationServiceV2.cs", "ResetAsync",
                "Global settings cache cleared — gsLock released");
        }
    }
}
