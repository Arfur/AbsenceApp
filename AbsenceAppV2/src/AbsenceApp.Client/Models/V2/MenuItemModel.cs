/*
===============================================================================
 File        : MenuItemModel.cs
 Namespace   : AbsenceApp.Client.Models.V2
 Author      : Michael
 Version     : 2.1.0
 Created     : 2026-03-21
 Updated     : 2026-04-08
-------------------------------------------------------------------------------
 Purpose     : Data model representing a single navigation submenu item within
               a MenuGroupModel. Maps directly to a 'submenu' row returned by
               dbo.fn_GetVisibleMenuItems.
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
   - 2.1.0  2026-04-08  Schema expansion: re-added Status (string? — item
                         status badge e.g. "new", "beta") and Description
                         (string? — tooltip text). Both fields now exist in
                         the MenuItems table and are returned by
                         fn_GetVisibleMenuItems.
-------------------------------------------------------------------------------
 Notes       :
   - Phase 3 model. No DI registration required.
===============================================================================
*/

namespace AbsenceApp.Client.Models.V2;

public sealed class MenuItemModel
{
    public string  Title       { get; set; } = string.Empty;
    public string  Icon        { get; set; } = string.Empty;
    public string  Route       { get; set; } = string.Empty;
    public string? Status      { get; set; }
    public string? Description { get; set; }
}
