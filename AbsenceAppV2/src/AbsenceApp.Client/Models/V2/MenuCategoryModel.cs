/*
===============================================================================
 File        : MenuCategoryModel.cs
 Namespace   : AbsenceApp.Client.Models.V2
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-23
 Updated     : 2026-03-23
-------------------------------------------------------------------------------
 Purpose     : Data model representing a top-level navigation category (section
               header) in the V2 sidebar. Each category groups one or more
               MenuGroupModel entries and supports optional role-based
               visibility. A null Category value means no section header is
               rendered (root-level groups).
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-23  Initial implementation.
-------------------------------------------------------------------------------
 Notes       :
   - Part of the V2 JSON-driven navigation system (menu.json v4.0.0).
   - No DI registration required.
===============================================================================
*/

namespace AbsenceApp.Client.Models.V2;

/// <summary>
/// Represents a navigation category (section header) that contains one or
/// more menu groups. Loaded from menu.json; all navigation logic lives in JSON.
/// </summary>
public sealed class MenuCategoryModel
{
    /// <summary>
    /// Section header label (e.g. "PEOPLE", "ACADEMICS"). Null means no header
    /// is rendered — the groups appear at the root level of the sidebar.
    /// </summary>
    public string? Category { get; set; }

    /// <summary>
    /// Required role to see this entire category and all its groups.
    /// Null means visible to all authenticated users.
    /// </summary>
    public string? RequiredRole { get; set; }

    /// <summary>Navigation groups (menus) within this category.</summary>
    public List<MenuGroupModel> Groups { get; set; } = [];
}
