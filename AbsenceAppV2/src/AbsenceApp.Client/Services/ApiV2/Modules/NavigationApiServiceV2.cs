/*
===============================================================================
 File        : NavigationApiServiceV2.cs
 Namespace   : AbsenceApp.Client.Services.ApiV2.Modules
 Author      : Michael
 Version     : 5.6.0
 Created     : 2026-04-06
 Updated     : 2026-04-19
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
               SortOrder, IsHidden, Category, GroupName, GroupIcon, IsFlat,
               Status, Description. Hierarchy is derived from ParentId;
               denormalised fields (GroupIcon, IsFlat) are used directly on
               group nodes, and Status/Description are mapped to item nodes.
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
   - 4.1.0  2026-04-08  Schema expansion: added Category, GroupName, GroupIcon,
                         IsFlat, Status, Description to SQL SELECT and to the
                         private MenuItemRow projection. BuildCategories now maps
                         GroupIcon (with Icon fallback) and IsFlat onto
                         MenuGroupModel, and Status/Description onto MenuItemModel.
   - 5.0.0  2026-04-10  Deterministic 3-phase hierarchy build in BuildCategories
                         (category → menu → submenu) to guarantee parent-first
                         construction, prevent orphaned submenus, and avoid
                         accidental pruning of valid groups/categories. Pruning
                         logic retained but applied after hierarchy assembly.
   - 5.1.0  2026-04-10  Added single-submenu collapsing step in BuildCategories:
                         when a group has exactly one submenu item whose Title
                         matches the group's label (case-insensitive) and the
                         group has no Route of its own, the group is promoted to
                         a flat link using the submenu's Route and Icon. Prevents
                         duplicate sidebar entries for menu/submenu pairs that
                         represent the same logical destination.
   - 5.2.0  2026-04-12  Added GetGlobalSettingsCategoriesAsync() which queries
                         dbo.MenuItemsGlobalConfig directly (no role filter).
                         Projects the table rows into the existing MenuItemRow
                         shape using SQL NULLs for absent columns, then reuses
                         BuildCategories() to produce the category → group →
                         submenu hierarchy for the Global Settings sidebar.
   - 5.3.0  2026-04-10  Fixed InvalidCastException in
                         GetGlobalSettingsCategoriesAsync (E7 Issue 2).
                         MenuItemsGlobalConfig.IsHidden was INT in the DB
                         (now corrected to BIT). Added explicit
                         CAST(IsHidden AS BIT) and CAST(IsFlat AS BIT) to
                         the SQL projection to prevent future type-mismatch
                         errors. Read Description from the table directly
                         now that the column exists (was NULL placeholder).
   - 5.4.0  2026-04-17  E17 Navigation Unification: injected PermissionServiceV2.
                         After BuildCategories(), GetMenuCategoriesAsync now calls
                         private FilterByPermissionsAsync() which removes submenu
                         items and flat-group links where the current user does not
                         have CanRead permission, then re-prunes empty groups and
                         categories. Global Settings sidebar is exempt (superadmin
                         only — no per-user permission filter applied).
   - 5.6.0  2026-04-19  RoleId resolution fix: removed users.RoleTypeId lookup.
                         GetMenuCategoriesAsync now resolves the user's role by
                         joining userrole directly inside the menu SQL query
                         (INNER JOIN userrole ur ON ur.RoleId = rm.RoleId WHERE
                         ur.UserId = @UserId). Eliminates the \"Unknown column
                         'u.RoleTypeId'\" startup crash.
-------------------------------------------------------------------------------
 Notes       :
   - Registered as Singleton in V2ServiceCollectionExtensions.
   - IServiceScopeFactory used to resolve Scoped AppDbContext from Singleton.
   - MySqlParameter prevents SQL injection on all parameterised queries.
===============================================================================
*/

using AbsenceApp.Client.Models.V2;
using AbsenceApp.Client.Services;
using AbsenceApp.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;

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
    private readonly PermissionServiceV2  _permService;

    public NavigationApiServiceV2(
        IServiceScopeFactory scopeFactory,
        AppStateService      appState,
        PermissionServiceV2  permService)
    {
        _scopeFactory = scopeFactory;
        _appState     = appState;
        _permService  = permService;
    }

    // ===========================================================================
    // GetMenuCategoriesAsync
    // Resolves menu items for the current user via userrole JOIN — no
    // users.RoleTypeId lookup required.
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

            var userId = _appState.CurrentUserId;
            AppLog.Write("NavigationApiServiceV2.cs", "GetMenuCategoriesAsync",
                $"CurrentUserId={userId}");

            // -----------------------------------------------------------------------
            // Query menuitems joined to rolemenuitem AND userrole so the user's
            // RoleId is resolved inside a single SQL statement.
            // userrole.UserId  = users.Id  (bigint)
            // userrole.RoleId  = rolemenuitem.RoleId (the role-level permission FK)
            // MySqlParameter prevents SQL injection.
            // -----------------------------------------------------------------------
            const string sql = """
                SELECT m.Id, m.ParentId, m.ItemType, m.Label, m.Icon, m.Route, m.SortOrder, m.IsHidden,
                       m.Category, m.GroupName, m.GroupIcon, m.IsFlat, m.Status, m.Description
                FROM   menuitems m
                INNER JOIN rolemenuitem rm ON rm.MenuItemId = m.Id
                INNER JOIN userrole     ur ON ur.RoleId     = rm.RoleId
                WHERE  ur.UserId    = @UserId
                  AND  (m.ItemType != 'submenu' OR m.IsHidden = 0)
                  AND  rm.IsEnabled = 1
                ORDER  BY m.SortOrder;
                """;

            var rows = await db.Database
                .SqlQueryRaw<MenuItemRow>(sql,
                    new MySqlParameter("@UserId", userId))
                .ToListAsync(ct);
            AppLog.Write("NavigationApiServiceV2.cs", "GetMenuCategoriesAsync",
                $"Rows returned = {rows.Count}");

            // -----------------------------------------------------------------------
            // Assemble tree: category → group → items (preserves TVF row order
            // within each type; hierarchy is built parent-first).
            // -----------------------------------------------------------------------
            var categories = BuildCategories(rows);
            await FilterByPermissionsAsync(categories, ct);
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
    // GetGlobalSettingsCategoriesAsync
    // Queries dbo.MenuItemsGlobalConfig directly and builds the full
    // category → group → submenu hierarchy for the Global Settings sidebar.
    // No role filter is applied — this table is superadmin-only by design.
    // The table columns (Id, ParentId, ItemType, Label, Icon, IsHidden, Route,
    // SortOrder) are projected into the existing MenuItemRow shape so that
    // BuildCategories() can be reused without modification.
    // ===========================================================================

    public async Task<List<MenuCategoryModel>> GetGlobalSettingsCategoriesAsync(
        CancellationToken ct = default)
    {
        AppLog.Write("NavigationApiServiceV2.cs", "GetGlobalSettingsCategoriesAsync",
            "ENTER GetGlobalSettingsCategoriesAsync");

        try
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // -----------------------------------------------------------------------
            // Query MenuItemsGlobalConfig directly.
            // Columns absent from this table are supplied as SQL NULLs / defaults
            // so the projection matches MenuItemRow exactly and BuildCategories
            // can be reused unchanged. Icon is used as GroupIcon fallback
            // (BuildCategories does: row.GroupIcon ?? row.Icon ?? string.Empty).
            // -----------------------------------------------------------------------
            const string sql = """
                SELECT Id, ParentId, ItemType, Label, Icon, Route, SortOrder,
                       IsHidden,
                       NULL        AS Category,
                       NULL        AS GroupName,
                       NULL        AS GroupIcon,
                       IsFlat,
                       NULL        AS Status,
                       Description
                FROM   menuitemsglobalconfig
                WHERE  IsHidden = 0
                ORDER  BY SortOrder;
                """;

            var rows = await db.Database
                .SqlQueryRaw<MenuItemRow>(sql)
                .ToListAsync(ct);
            AppLog.Write("NavigationApiServiceV2.cs", "GetGlobalSettingsCategoriesAsync",
                $"Rows returned = {rows.Count}");

            var categories = BuildCategories(rows);
            AppLog.Write("NavigationApiServiceV2.cs", "GetGlobalSettingsCategoriesAsync",
                $"Categories built = {categories.Count}");
            return categories;
        }
        catch (Exception ex)
        {
            AppLog.Write("NavigationApiServiceV2.cs", "GetGlobalSettingsCategoriesAsync",
                $"ERROR {ex.GetType().Name}: {ex.Message}");
            throw;
        }
    }

    // =======================================================================
    // FilterByPermissionsAsync
    // Post-processes the built hierarchy: removes submenu items and flat-group
    // links where the current user does not have CanRead permission, then
    // re-prunes groups with no remaining visible items and empty categories.
    //
    // Only routes registered in AppPage are permission-checked. Items whose
    // route is absent from the AppPage registry are always shown (they are not
    // permission-controlled pages — e.g. utility links or redirects).
    // =======================================================================

    private async Task FilterByPermissionsAsync(
        List<MenuCategoryModel> categories,
        CancellationToken ct)
    {
        foreach (var cat in categories)
        {
            // -----------------------------------------------------------------
            // Step 1: filter submenu items within each group
            // -----------------------------------------------------------------
            foreach (var grp in cat.Groups)
            {
                var kept = new List<MenuItemModel>();
                foreach (var item in grp.Items)
                {
                    if (await _permService.CanViewAsync(item.Route, ct))
                        kept.Add(item);
                }
                grp.Items = kept;
            }

            // -----------------------------------------------------------------
            // Step 2: re-prune groups
            //   • Flat / no-item groups with a Route: check the route itself.
            //   • Accordion groups emptied by Step 1: remove.
            // -----------------------------------------------------------------
            var keptGroups = new List<MenuGroupModel>();
            foreach (var grp in cat.Groups)
            {
                if (grp.Items.Count == 0)
                {
                    if (!string.IsNullOrEmpty(grp.Route))
                    {
                        // Flat link or single-collapsed group — check its route.
                        if (await _permService.CanViewAsync(grp.Route, ct))
                            keptGroups.Add(grp);
                    }
                    // else: accordion with no items left — drop it.
                }
                else
                {
                    keptGroups.Add(grp);
                }
            }
            cat.Groups = keptGroups;
        }

        // -----------------------------------------------------------------
        // Step 3: remove categories that became empty after group pruning
        // -----------------------------------------------------------------
        categories.RemoveAll(c => c.Groups.Count == 0);
    }

    // =======================================================================
    // BuildCategories
    // Assembles the TVF rows into the client-side hierarchy using ParentId.
    // Rows are sorted by SortOrder in SQL; hierarchy is built in three passes:
    //   1) categories, 2) menus (groups), 3) submenus (items).
    // Prunes groups with no items and no direct route, then prunes empty
    // categories. No client-side role or entitlement filtering.
    // =======================================================================

    private static List<MenuCategoryModel> BuildCategories(List<MenuItemRow> rows)
    {
        // -------------------------------------------------------------------
        // Indexes for O(1) parent lookups
        // -------------------------------------------------------------------
        var categoryById = new Dictionary<int, MenuCategoryModel>();
        var groupById    = new Dictionary<int, MenuGroupModel>();
        var categories   = new List<MenuCategoryModel>();

        // -------------------------------------------------------------------
        // Phase 1: categories
        // -------------------------------------------------------------------
        foreach (var row in rows.Where(r => r.ItemType == "category"))
        {
            var cat = new MenuCategoryModel
            {
                Category = row.Label ?? string.Empty
            };

            categoryById[row.Id] = cat;
            categories.Add(cat);
        }

        // -------------------------------------------------------------------
        // Phase 2: menus (groups)
        // -------------------------------------------------------------------
        foreach (var row in rows.Where(r => r.ItemType == "menu"))
        {
            var grp = new MenuGroupModel
            {
                Group       = row.Label     ?? string.Empty,
                Icon        = row.GroupIcon ?? row.Icon ?? string.Empty,
                Route       = row.Route     ?? string.Empty,
                IsFlat      = row.IsFlat    ?? false,
                Description = row.Description,
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
        }

        // -------------------------------------------------------------------
        // Phase 3: submenus (items)
        // -------------------------------------------------------------------
        foreach (var row in rows.Where(r => r.ItemType == "submenu"))
        {
            if (row.ParentId.HasValue &&
                groupById.TryGetValue(row.ParentId.Value, out var parentGrp))
            {
                parentGrp.Items.Add(new MenuItemModel
                {
                    Title       = row.Label ?? string.Empty,
                    Icon        = row.Icon  ?? string.Empty,
                    Route       = row.Route ?? string.Empty,
                    Status      = row.Status,
                    Description = row.Description,
                });
            }
            else
            {
                // Orphan submenu: no valid parent group in this result set.
                // Intentionally ignored; no client-side repair or reassignment.
            }
        }

        // -------------------------------------------------------------------
        // Phase 4: single-submenu collapsing
        // When a group has exactly one submenu item whose Title matches the
        // group's own label (case-insensitive), and the group has no Route of
        // its own, the group and submenu represent the same logical destination.
        // Promote the submenu's Route and Icon into the group and clear Items
        // so the sidebar renders one flat link instead of an accordion with a
        // single duplicate entry.
        // -------------------------------------------------------------------
        foreach (var cat in categories)
        {
            foreach (var grp in cat.Groups)
            {
                if (grp.Items.Count == 1 &&
                    string.IsNullOrEmpty(grp.Route) &&
                    string.Equals(grp.Group.Trim(), grp.Items[0].Title.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    grp.Route  = grp.Items[0].Route;
                    grp.Icon   = grp.Items[0].Icon;
                    grp.IsFlat = true;
                    grp.Items.Clear();
                }
            }
        }

        // -------------------------------------------------------------------
        // Prune: remove groups with no items and no direct route,
        // then remove categories with no remaining groups.
        // -------------------------------------------------------------------
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
        public int     Id          { get; set; }
        public int?    ParentId    { get; set; }
        public string? ItemType    { get; set; }
        public string? Label       { get; set; }
        public string? Icon        { get; set; }
        public string? Route       { get; set; }
        public int     SortOrder   { get; set; }
        public bool    IsHidden    { get; set; }
        public string? Category    { get; set; }
        public string? GroupName   { get; set; }
        public string? GroupIcon   { get; set; }
        public bool?   IsFlat      { get; set; }
        public string? Status      { get; set; }
        public string? Description { get; set; }
    }
}
