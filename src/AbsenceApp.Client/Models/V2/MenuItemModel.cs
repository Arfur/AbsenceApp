/*
===============================================================================
 File        : MenuItemModel.cs
 Namespace   : AbsenceApp.Client.Models.V2
 Author      : Michael
 Version     : 1.1.0
 Created     : 2026-03-21
 Updated     : 2026-03-23
-------------------------------------------------------------------------------
 Purpose     : Data model representing a single navigation menu item within a MenuGroupModel. Contains label, icon, route, and optional badge count.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-21  Initial implementation.
   - 1.1.0  2026-03-23  Added RequiredRole (string?) for JSON-driven
                         role-based visibility at item level.
-------------------------------------------------------------------------------
 Notes       :
   - Phase 3 model. No DI registration required.
===============================================================================
*/

namespace AbsenceApp.Client.Models.V2;

/// <summary>Represents a single navigation link inside a menu group.</summary>
public sealed class MenuItemModel
{
    /// <summary>Display title shown in the sidebar.</summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>Blazor route href (e.g. "/absences").</summary>
    public string Href { get; set; } = string.Empty;

    /// <summary>Bootstrap icon class (e.g. "bi-calendar-check").</summary>
    public string Icon { get; set; } = string.Empty;

    /// <summary>Optional status badge text (e.g. "New", "Beta").</summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>Optional tooltip / description for the item.</summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Required role to see this item. Null means visible to all authenticated users.
    /// </summary>
    public string? RequiredRole { get; set; }
}
