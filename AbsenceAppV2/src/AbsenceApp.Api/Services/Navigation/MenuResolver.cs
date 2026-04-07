/*
===============================================================================
 File        : MenuResolver.cs
 Namespace   : AbsenceApp.Api.Services.Navigation
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-04-06
 Updated     : 2026-04-06
-------------------------------------------------------------------------------
 Purpose     : Executes dbo.fn_GetVisibleMenuItems(@RoleType) via raw SQL,
               applies server-side parent pruning, and maps the flat row
               results to a fully nested MenuResponseDto.

               Pruning rules (server-side, presentation only):
                 1. Always keep submenus (items are never pruned here).
                 2. Keep groups only if they have at least one item.
                 3. Keep categories only if they have at least one group.

               No permission logic is applied here. The TVF already filters
               by RoleType — this class is responsible only for tree assembly.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-06  Initial implementation (Phase 1 — API Menu Boundary).
-------------------------------------------------------------------------------
 Notes       :
   - Raw SQL uses SqlParameter to prevent SQL injection.
   - Ordering is preserved from the TVF ORDER BY SortOrder clause.
   - Registered as Scoped in Program.cs.
===============================================================================
*/

using AbsenceApp.Data.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

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

    // ===========================================================================
    // GetMenuAsync
    // Queries the TVF, assembles the tree, and prunes empty parents.
    // ===========================================================================

    public async Task<MenuResponseDto> GetMenuAsync(int roleType, CancellationToken ct = default)
    {
        // -----------------------------------------------------------------------
        // Execute TVF — raw SQL with parameterised input
        // -----------------------------------------------------------------------
        const string sql = """
            SELECT Category, GroupName, GroupIcon, IsFlat,
                   Label, Href, ItemIcon, Status, Description, SortOrder
            FROM   dbo.fn_GetVisibleMenuItems(@RoleType)
            ORDER  BY SortOrder;
            """;

        var rows = await _db.Database
            .SqlQueryRaw<MenuItemRow>(sql,
                new SqlParameter("@RoleType", roleType))
            .ToListAsync(ct);

        // -----------------------------------------------------------------------
        // Assemble nested structure, preserving TVF row order
        // -----------------------------------------------------------------------
        //   Category (string? key) → List<MenuGroupDto>
        //   within each category, Group (string key) → List<MenuItemDto>
        // -----------------------------------------------------------------------
        var categoryMap  = new Dictionary<string?, MenuCategoryDto>(StringComparer.Ordinal);
        var groupTracked = new Dictionary<(string? cat, string? grp), MenuGroupDto>();

        foreach (var row in rows)
        {
            // -------------------------------------------------------------------
            // Resolve or create category bucket
            // -------------------------------------------------------------------
            if (!categoryMap.TryGetValue(row.Category, out var cat))
            {
                cat = new MenuCategoryDto { Category = row.Category };
                categoryMap[row.Category] = cat;
            }

            // -------------------------------------------------------------------
            // Resolve or create group bucket
            // -------------------------------------------------------------------
            var groupKey = (row.Category, row.GroupName);
            if (!groupTracked.TryGetValue(groupKey, out var grp))
            {
                grp = new MenuGroupDto
                {
                    Group = row.GroupName ?? string.Empty,
                    Icon  = row.GroupIcon ?? string.Empty,
                    Flat  = row.IsFlat
                };
                groupTracked[groupKey] = grp;
                cat.Groups.Add(grp);
            }

            // -------------------------------------------------------------------
            // Append item (submenus are always kept)
            // -------------------------------------------------------------------
            grp.Items.Add(new MenuItemDto
            {
                Title       = row.Label       ?? string.Empty,
                Href        = row.Href        ?? string.Empty,
                Icon        = row.ItemIcon    ?? string.Empty,
                Status      = row.Status      ?? "Live",
                Description = row.Description
            });
        }

        // -----------------------------------------------------------------------
        // Prune: remove groups with no items, then categories with no groups
        // -----------------------------------------------------------------------
        var prunedCategories = categoryMap.Values
            .Select(c =>
            {
                c.Groups = c.Groups.Where(g => g.Items.Count > 0).ToList();
                return c;
            })
            .Where(c => c.Groups.Count > 0)
            .ToList();

        return new MenuResponseDto { Categories = prunedCategories };
    }
}
