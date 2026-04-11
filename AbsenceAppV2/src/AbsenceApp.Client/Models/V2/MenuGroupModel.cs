/*
===============================================================================
 File        : MenuGroupModel.cs
 Namespace   : AbsenceApp.Client.Models.V2
 Author      : Michael
 Version     : 2.1.0
 Created     : 2026-03-21
 Updated     : 2026-04-08
-------------------------------------------------------------------------------
 Purpose     : Data model representing a navigation menu group ('menu' row from
               dbo.fn_GetVisibleMenuItems). Contains the group label, icon,
               optional direct route, IsFlat flag, optional description, and
               child submenu items.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-21  Initial implementation.
   - 1.1.0  2026-03-23  Added Flat (bool) to support direct-link groups that
                         render without an accordion. Added RequiredRole
                         (string?) for JSON-driven role-based visibility.
   - 2.0.0  2026-04-06  Schema alignment: removed Flat (structure derived at
                         render time from Items.Count == 0) and RequiredRole
                         (not in DB schema; DB is authoritative). Added Route
                         to hold the MenuItems.Route value for 'menu' nodes
                         that have no submenu children.
   - 2.1.0  2026-04-08  Schema expansion: added IsFlat (bool — DB-authoritative
                         direct-link flag) and Description (string? — group
                         tooltip/description). IsFlat is now the primary signal
                         for flat rendering; Items.Count == 0 remains as
                         fallback. Description maps to MenuItems.Description.
-------------------------------------------------------------------------------
 Notes       :
   - Phase 3 model. No DI registration required.
===============================================================================
*/

namespace AbsenceApp.Client.Models.V2;

public sealed class MenuGroupModel
{
    public string  Group       { get; set; } = string.Empty;
    public string  Icon        { get; set; } = string.Empty;
    public string  Route       { get; set; } = string.Empty;
    public bool    IsFlat      { get; set; }
    public string? Description { get; set; }

    public List<MenuItemModel> Items { get; set; } = [];
}
