/*
===============================================================================
 File        : MenuResolver.cs
 Namespace   : AbsenceApp.Api.Services.Navigation
 Author      : Michael
 Version     : 2.0.0
 Created     : 2026-04-06
 Updated     : 2026-04-19
-------------------------------------------------------------------------------
 Purpose     : Queries the menuitems table (filtered by role via rolemenuitem
               JOIN) and assembles the flat rows into a fully nested
               MenuResponseDto using a 3-pass ParentId-based tree build.

               Pruning rules (server-side, presentation only):
                 1. Always keep submenus (items are never pruned here).
                 2. Keep groups only if they have at least one item.
                 3. Keep categories only if they have at least one group.

               No permission logic is applied here. The rolemenuitem JOIN
               already filters by RoleId — this class is responsible only
               for tree assembly.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-06  Initial implementation (Phase 1 — API Menu Boundary).
   - 2.0.0  2026-04-19  MySQL migration: removed fn_GetVisibleMenuItems TVF
                         call and MySqlConnector dependency. Replaced with
                         direct menuitems JOIN rolemenuitem SQL query.
                         Rewrote GetMenuAsync tree building from denormalised
                         (TVF row) approach to 3-pass hierarchical (ParentId)
                         approach matching the client-side BuildCategories
                         pattern. Updated MenuItemRow to reflect the actual
                         menuitems table schema (Id, ParentId, ItemType, Route,
                         Icon instead of Href, ItemIcon).
-------------------------------------------------------------------------------
 Notes       :
   - MySqlParameter prevents SQL injection on all parameterised queries.
   - Ordering is preserved from ORDER BY SortOrder.
   - Registered as Scoped in Program.cs.
===============================================================================
*/

using AbsenceApp.Data.Context;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace AbsenceApp.Api.Services.Navigation;

// ===========================================================================
// IMenuResolver
// ===========================================================================

public interface IMenuResolver
{
    Task<MenuResponseDto> GetMenuAsync(int roleType, CancellationToken ct = default);
}

// ===========================================================================
// MenuResolver
// ===========================================================================

public sealed class MenuResolver : IMenuResolver
{
    // ---------------------------------------------------------------------------
    // Dependencies
    // ---------------------------------------------------------------------------

    private readonly AppDbContext _db;

    public MenuResolver(AppDbContext db)
    {
        _db = db;
    }

    public async Task<MenuResponseDto> GetMenuAsync(int roleType, CancellationToken ct = default)
    {
        // -----------------------------------------------------------------------
        // Query menuitems joined to rolemenuitem for role-filtered visibility.
        // MySqlParameter prevents SQL injection.
        // -----------------------------------------------------------------------
        const string sql = """
            SELECT m.Id, m.ParentId, m.ItemType, m.Label, m.Icon, m.Route, m.SortOrder, m.IsHidden,
                   m.Category, m.GroupName, m.GroupIcon, m.IsFlat, m.Status, m.Description
            FROM   menuitems m
            INNER JOIN rolemenuitem rm ON rm.MenuItemId = m.Id
            WHERE  rm.RoleId    = @RoleType
              AND  m.IsHidden   = 0
              AND  rm.IsEnabled = 1
            ORDER  BY m.SortOrder;
            """;

        var rows = await _db.Database
            .SqlQueryRaw<MenuItemRow>(sql,
                new MySqlParameter("@RoleType", roleType))
            .ToListAsync(ct);

        // -----------------------------------------------------------------------
        // 3-pass hierarchical tree build using ParentId.
        // Pass 1: categories   (ItemType == "category")
        // Pass 2: groups/menus (ItemType == "menu")   — attached to parent category
        // Pass 3: submenus     (ItemType == "submenu") — attached to parent group
        // -----------------------------------------------------------------------
        var categoryById = new Dictionary<int, MenuCategoryDto>();
        var groupById    = new Dictionary<int, MenuGroupDto>();
        var categories   = new List<MenuCategoryDto>();

        // Pass 1: categories
        foreach (var row in rows.Where(r => r.ItemType == "category"))
        {
            var cat = new MenuCategoryDto { Category = row.Label };
            categoryById[row.Id] = cat;
            categories.Add(cat);
        }

        // Pass 2: groups
        foreach (var row in rows.Where(r => r.ItemType == "menu"))
        {
            var grp = new MenuGroupDto
            {
                Group = row.Label     ?? string.Empty,
                Icon  = row.GroupIcon ?? row.Icon ?? string.Empty,
                Flat  = row.IsFlat    ?? false,
            };
            groupById[row.Id] = grp;

            if (row.ParentId.HasValue &&
                categoryById.TryGetValue(row.ParentId.Value, out var parentCat))
            {
                parentCat.Groups.Add(grp);
            }
            else
            {
                // Orphan group: attach to an implicit unnamed category.
                var root = new MenuCategoryDto();
                root.Groups.Add(grp);
                categories.Add(root);
            }
        }

        // Pass 3: submenus
        foreach (var row in rows.Where(r => r.ItemType == "submenu"))
        {
            if (row.ParentId.HasValue &&
                groupById.TryGetValue(row.ParentId.Value, out var parentGrp))
            {
                parentGrp.Items.Add(new MenuItemDto
                {
                    Title       = row.Label       ?? string.Empty,
                    Href        = row.Route       ?? string.Empty,
                    Icon        = row.Icon        ?? string.Empty,
                    Status      = row.Status      ?? "Live",
                    Description = row.Description,
                });
            }
        }

        // -----------------------------------------------------------------------
        // Prune: remove groups with no items, then categories with no groups
        // -----------------------------------------------------------------------
        var prunedCategories = categories
            .Select(c =>
            {
                c.Groups = c.Groups.Where(g => g.Items.Count > 0).ToList();
                return c;
            })
            .Where(c => c.Groups.Count > 0)
            .ToList();

        return new MenuResponseDto { Categories = prunedCategories };
    }

    // ---------------------------------------------------------------------------
    // Private projection — column names MUST match the menuitems table exactly
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
