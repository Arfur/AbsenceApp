/*
===============================================================================
 File        : MenuGroupModel.cs
 Namespace   : AbsenceApp.Client.Models.V2
 Author      : Michael
 Version     : 1.1.0
 Created     : 2026-03-21
 Updated     : 2026-03-23
-------------------------------------------------------------------------------
 Purpose     : Data model representing a navigation menu group. Contains a group label and a list of MenuItemModel entries for V2 sidebar navigation.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-21  Initial implementation.
   - 1.1.0  2026-03-23  Added Flat (bool) to support direct-link groups that
                         render without an accordion. Added RequiredRole
                         (string?) for JSON-driven role-based visibility.
-------------------------------------------------------------------------------
 Notes       :
   - Phase 3 model. No DI registration required.
===============================================================================
*/

namespace AbsenceApp.Client.Models.V2;

/// <summary>Represents a top-level navigation group (accordion section in SidebarV2).</summary>
public sealed class MenuGroupModel
{
    /// <summary>Display label for the group heading.</summary>
    public string Group { get; set; } = string.Empty;

    /// <summary>Bootstrap icon class for the group (e.g. "bi-grid").</summary>
    public string Icon { get; set; } = string.Empty;

    /// <summary>Navigation items within this group.</summary>
    public List<MenuItemModel> Items { get; set; } = [];

    /// <summary>
    /// When true, the group renders as a direct navigation link rather than
    /// an expandable accordion button. The link target is Items[0].Href.
    /// </summary>
    public bool Flat { get; set; } = false;

    /// <summary>
    /// Required role to see this group. Null means visible to all authenticated users.
    /// </summary>
    public string? RequiredRole { get; set; }
}
