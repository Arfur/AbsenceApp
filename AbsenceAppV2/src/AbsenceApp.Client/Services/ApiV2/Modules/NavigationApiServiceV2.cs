/*
===============================================================================
 File        : NavigationApiServiceV2.cs
 Namespace   : AbsenceApp.Client.Services.ApiV2.Modules
 Author      : Michael
 Version     : 4.0.0
 Created     : 2026-04-06
 Updated     : 2026-04-06
-------------------------------------------------------------------------------
 Purpose     : Client-side navigation service that executes
               dbo.fn_GetVisibleMenuItems(@RoleType) via EF Core SqlQueryRaw
               and assembles the result rows into a MenuCategoryModel hierarchy
               for use by NavigationServiceV2 and SidebarV2.

               In MAUI Blazor Hybrid the C# HttpClient cannot reach
               http://localhost/ (that scheme exists only inside the WebView2
               browser context). This service therefore calls the database
               directly using IServiceScopeFactory + AppDbContext instead of
               making an HTTP call to AbsenceApp.Api.

               The menu tree is assembled from TVF rows using the authoritative
               MenuItems schema: Id, ParentId, ItemType, Label, Icon, Route,
               SortOrder, IsHidden. Hierarchy is derived solely from ParentId.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-06  Initial implementation (Phase 2 — Client Menu
                         Rendering). Used HttpClient to call GET /api/menu.
   - 2.0.0  2026-04-06  Breaking fix: replaced HttpClient with direct DB call
                         via IServiceScopeFactory + AppDbContext. HttpClient
                         cannot reach http://localhost/ in MAUI C# context.
                         Now calls dbo.fn_GetVisibleMenuItems(@RoleType)
                         using the current user's RoleTypeId from Users table.
   - 3.0.0  2026-04-06  Schema alignment: SQL updated to select only the
                         canonical MenuItems columns (Id, ParentId, ItemType,
                         Label, Icon, Route, SortOrder, IsHidden). BuildCategories
                         rewritten to derive hierarchy from ParentId instead of
                         the old denormalised Category/GroupName columns.
                         MenuItemRow updated to match DB schema exactly.
   - 4.0.0  2026-04-06  Logging migration: replaced all Debug.WriteLine and
                         Console.WriteLine calls with AppLog.Write for
                         file-based diagnostic output. Removed System.Diagnostics.
-------------------------------------------------------------------------------
 Notes       :
   - Registered as Singleton in V2ServiceCollectionExtensions.
   - IServiceScopeFactory used to resolve Scoped AppDbContext from Singleton.
   - SqlParameter prevents SQL injection on all parameterised queries.
===============================================================================
*/

using AbsenceApp.Client.Models.V2;
using AbsenceApp.Client.Services;
using AbsenceApp.Data.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AbsenceApp.Client.Services.ApiV2.Modules;

// ===========================================================================
// NavigationApiServiceV2
// ===========================================================================

public sealed class NavigationApiServiceV2
{
    // ---------------------------------------------------------------------------
    // Dependencies
    // ---------------------------------------------------------------------------

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly AppStateService      _appState;

    public NavigationApiServiceV2(
        IServiceScopeFactory scopeFactory,
        AppStateService      appState)
    {
        _scopeFactory = scopeFactory;
        _appState     = appState;
    }

    // ===========================================================================
    // GetMenuCategoriesAsync
    // Looks up the current user's RoleTypeId, executes the TVF, and builds
    // the client-side menu tree.
    // ===========================================================================

    public async Task<List<MenuCategoryModel>> GetMenuCategoriesAsync(
        CancellationToken ct = default)
    {
        AppLog.Write("NavigationApiServiceV2.cs", "GetMenuCategoriesAsync",
            "ENTER GetMenuCategoriesAsync");

        try
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // -----------------------------------------------------------------------
            // Resolve current user's RoleTypeId
            // -----------------------------------------------------------------------
            var userId     = _appState.CurrentUserId;
            AppLog.Write("NavigationApiServiceV2.cs", "GetMenuCategoriesAsync",
                $"CurrentUserId={userId}");

            var roleTypeId = await db.Users
                .AsNoTracking()
                .Where(u => u.Id == userId)
                .Select(u => u.RoleTypeId)
                .FirstOrDefaultAsync(ct);
            AppLog.Write("NavigationApiServiceV2.cs", "GetMenuCategoriesAsync",
                $"RoleTypeId={roleTypeId}");

            // -----------------------------------------------------------------------
            // Execute TVF — parameterised to prevent SQL injection
            // -----------------------------------------------------------------------
            const string sql = """
                SELECT Id, ParentId, ItemType, Label, Icon, Route, SortOrder, IsHidden
                FROM   dbo.fn_GetVisibleMenuItems(@RoleType)
                ORDER  BY SortOrder;
                """;

            var rows = await db.Database
                .SqlQueryRaw<MenuItemRow>(sql,
                    new SqlParameter("@RoleType", (int)roleTypeId))
                .ToListAsync(ct);
            AppLog.Write("NavigationApiServiceV2.cs", "GetMenuCategoriesAsync",
                $"Rows returned = {rows.Count}");

            // -----------------------------------------------------------------------
            // Assemble tree: category → group → items (preserves TVF row order)
            // -----------------------------------------------------------------------
            var categories = BuildCategories(rows);
            AppLog.Write("NavigationApiServiceV2.cs", "GetMenuCategoriesAsync",
                $"Categories built = {categories.Count}");
            return categories;
        }
        catch (Exception ex)
        {
            AppLog.Write("NavigationApiServiceV2.cs", "GetMenuCategoriesAsync",
                $"ERROR {ex.GetType().Name}: {ex.Message}");
            throw;
        }
    }

    // ===========================================================================
    // BuildCategories
    // Assembles the TVF rows into the client-side hierarchy using ParentId.
    // Rows are already sorted by SortOrder from the SQL ORDER BY clause.
    // Prunes groups with no items and no direct route, then prunes empty categories.
    // ===========================================================================

    private static List<MenuCategoryModel> BuildCategories(List<MenuItemRow> rows)
    {
        // ---------------------------------------------------------------------------
        // Index by Id for O(1) parent lookups
        // ---------------------------------------------------------------------------
        var categoryById = new Dictionary<int, MenuCategoryModel>();
        var groupById    = new Dictionary<int, MenuGroupModel>();
        var categories   = new List<MenuCategoryModel>();

        foreach (var row in rows)
        {
            switch (row.ItemType)
            {
                case "category":
                    var cat = new MenuCategoryModel { Category = row.Label };
                    categoryById[row.Id] = cat;
                    categories.Add(cat);
                    break;

                case "menu":
                    var grp = new MenuGroupModel
                    {
                        Group = row.Label ?? string.Empty,
                        Icon  = row.Icon  ?? string.Empty,
                        Route = row.Route ?? string.Empty,
                    };
                    groupById[row.Id] = grp;
                    if (row.ParentId.HasValue &&
                        categoryById.TryGetValue(row.ParentId.Value, out var parentCat))
                    {
                        parentCat.Groups.Add(grp);
                    }
                    else
                    {
                        // Orphan group: attach to an implicit unnamed category
                        var root = new MenuCategoryModel();
                        root.Groups.Add(grp);
                        categories.Add(root);
                    }
                    break;

                case "submenu":
                    if (row.ParentId.HasValue &&
                        groupById.TryGetValue(row.ParentId.Value, out var parentGrp))
                    {
                        parentGrp.Items.Add(new MenuItemModel
                        {
                            Title = row.Label ?? string.Empty,
                            Icon  = row.Icon  ?? string.Empty,
                            Route = row.Route ?? string.Empty,
                        });
                    }
                    break;
            }
        }

        // ---------------------------------------------------------------------------
        // Prune: remove groups with no items and no direct route, then empty categories
        // ---------------------------------------------------------------------------
        return categories
            .Select(c =>
            {
                c.Groups = c.Groups
                    .Where(g => g.Items.Count > 0 || !string.IsNullOrEmpty(g.Route))
                    .ToList();
                return c;
            })
            .Where(c => c.Groups.Count > 0)
            .ToList();
    }

    // ---------------------------------------------------------------------------
    // Private projection — column names MUST match the TVF output exactly
    // ---------------------------------------------------------------------------

    private sealed class MenuItemRow
    {
        public int     Id        { get; set; }
        public int?    ParentId  { get; set; }
        public string? ItemType  { get; set; }
        public string? Label     { get; set; }
        public string? Icon      { get; set; }
        public string? Route     { get; set; }
        public int     SortOrder { get; set; }
        public bool    IsHidden  { get; set; }
    }
}

