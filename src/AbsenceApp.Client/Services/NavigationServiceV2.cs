/*
===============================================================================
 File        : NavigationServiceV2.cs
 Namespace   : AbsenceApp.Client.Services
 Author      : Michael
 Version     : 2.0.0
 Created     : 2026-03-21
 Updated     : 2026-03-23
-------------------------------------------------------------------------------
 Purpose     : V2 navigation service. Loads the sidebar menu structure from menu.json, resolves active routes, builds breadcrumb trails, and exposes NavGroups to SidebarV2. Parallel to V1 navigation; does NOT modify or replace it.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-21  Initial implementation.
   - 2.0.0  2026-03-23  Upgraded to parse menu.json v4.0.0 category structure.
                         Primary method is now GetMenuCategoriesAsync() which
                         returns List<MenuCategoryModel>. GetMenuGroupsAsync()
                         retained as a flat shim for backward compatibility.
                         FindItemByHrefAsync and GetAllItemsAsync updated to
                         traverse category → group → item hierarchy.
-------------------------------------------------------------------------------
 Notes       :
   None.
===============================================================================
*/

using AbsenceApp.Client.Models.V2;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace AbsenceApp.Client.Services;

/// <summary>
/// Provides V2 navigation menu data loaded from the design-system menu.json config.
/// Singleton; results are cached in memory after the first call.
/// </summary>
public sealed class NavigationServiceV2
{
    private readonly DesignSystemConfigService _config;

    private List<MenuCategoryModel>? _cache;
    private readonly SemaphoreSlim _lock = new(1, 1);

    private static readonly JsonSerializerOptions _jsonOpts = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    public NavigationServiceV2(DesignSystemConfigService config)
    {
        _config = config;
    }

    /// <summary>
    /// Returns all menu categories from menu.json. Cached after first successful load.
    /// Returns an empty list if loading fails (graceful degradation).
    /// </summary>
    public async Task<List<MenuCategoryModel>> GetMenuCategoriesAsync()
    {
        if (_cache is not null) return _cache;

        await _lock.WaitAsync();
        try
        {
            if (_cache is not null) return _cache;

            var menuObj = await _config.GetMenuAsync();

            if (!menuObj.TryGetPropertyValue("categories", out var categoriesNode) || categoriesNode is null)
            {
                _cache = [];
                return _cache;
            }

            _cache = JsonSerializer.Deserialize<List<MenuCategoryModel>>(
                categoriesNode.ToJsonString(), _jsonOpts) ?? [];
        }
        catch
        {
            _cache = [];
        }
        finally
        {
            _lock.Release();
        }

        return _cache;
    }

    /// <summary>
    /// Returns a flat list of all groups across all categories.
    /// </summary>
    public async Task<List<MenuGroupModel>> GetMenuGroupsAsync()
    {
        var categories = await GetMenuCategoriesAsync();
        return categories.SelectMany(c => c.Groups).ToList();
    }

    /// <summary>
    /// Returns the menu item matching the given href, or null if not found.
    /// </summary>
    public async Task<MenuItemModel?> FindItemByHrefAsync(string href)
    {
        var categories = await GetMenuCategoriesAsync();
        return categories
            .SelectMany(c => c.Groups)
            .SelectMany(g => g.Items)
            .FirstOrDefault(i => string.Equals(i.Href, href, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Returns a flat list of all menu items across all categories and groups.
    /// Useful for search and breadcrumb resolution.
    /// </summary>
    public async Task<List<MenuItemModel>> GetAllItemsAsync()
    {
        var categories = await GetMenuCategoriesAsync();
        return categories
            .SelectMany(c => c.Groups)
            .SelectMany(g => g.Items)
            .ToList();
    }
}
