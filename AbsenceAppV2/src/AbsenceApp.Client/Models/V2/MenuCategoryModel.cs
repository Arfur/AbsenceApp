/*
===============================================================================
 File        : MenuCategoryModel.cs
 Namespace   : AbsenceApp.Client.Models.V2
 Author      : Michael
 Version     : 2.0.0
 Created     : 2026-03-23
 Updated     : 2026-04-06
-------------------------------------------------------------------------------
 Purpose     : Data model representing a top-level navigation category ('category'
               row from dbo.fn_GetVisibleMenuItems). Groups one or more
               MenuGroupModel entries. A null Category value means no section
               header is rendered. Contains only the fields present in the
               MenuItems table schema.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-23  Initial implementation.
   - 2.0.0  2026-04-06  Schema alignment: removed RequiredRole (not in DB
                         schema; DB is authoritative via fn_GetVisibleMenuItems).
                         Updated purpose note to reflect DB-driven source.
-------------------------------------------------------------------------------
 Notes       :
   - No DI registration required.
===============================================================================
*/

namespace AbsenceApp.Client.Models.V2;

public sealed class MenuCategoryModel
{
    public string? Category { get; set; }

    public List<MenuGroupModel> Groups { get; set; } = [];
}
