/*
===============================================================================
 File        : MenuItemModel.cs
 Namespace   : AbsenceApp.Client.Models.V2
 Author      : Michael
 Version     : 2.0.0
 Created     : 2026-03-21
 Updated     : 2026-04-06
-------------------------------------------------------------------------------
 Purpose     : Data model representing a single navigation submenu item within
               a MenuGroupModel. Maps directly to a 'submenu' row returned by
               dbo.fn_GetVisibleMenuItems. Contains only the fields present in
               the MenuItems table schema.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-21  Initial implementation.
   - 1.1.0  2026-03-23  Added RequiredRole (string?) for JSON-driven
                         role-based visibility at item level.
   - 1.2.0  2026-03-29  BUG FIX: Added [JsonPropertyName("label")] to
                         Title so JSON deserialization maps the "label"
                         field in menu.json to Title. Without this
                         attribute, deserialization left Title empty.
   - 2.0.0  2026-04-06  Schema alignment: replaced Href with Route to match
                         the MenuItems.Route column. Removed Status,
                         Description, and RequiredRole (not in DB schema).
                         Removed [JsonPropertyName] and JSON serialization
                         import (model is no longer JSON-deserialized).
-------------------------------------------------------------------------------
 Notes       :
   - Phase 3 model. No DI registration required.
===============================================================================
*/

namespace AbsenceApp.Client.Models.V2;

public sealed class MenuItemModel
{
    public string Title { get; set; } = string.Empty;
    public string Icon  { get; set; } = string.Empty;
    public string Route { get; set; } = string.Empty;
}
