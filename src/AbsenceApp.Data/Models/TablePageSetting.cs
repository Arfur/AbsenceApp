/*
===============================================================================
 File        : TablePageSetting.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-17
 Updated     : 2026-03-17
-------------------------------------------------------------------------------
 Purpose     : EF Core entity that stores per-table display configuration,
               allowing administrators to control which columns are visible,
               sortable, filterable, and searchable for each table page.
-------------------------------------------------------------------------------
 Notes       :
   - Maps to the "table_page_settings" SQL table.
   - The (PageName, FieldName) pair is unique — enforced by a database index
     defined in TablePageSettingConfiguration.
   - The Id column uses SQL Server IDENTITY; excluding this entity from the
     global ValueGeneratedNever() convention in AppDbContext is required.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-17  Initial implementation.
===============================================================================
*/

namespace AbsenceApp.Data.Models;

public class TablePageSetting
{
    public int    Id           { get; set; }
    public string PageName     { get; set; } = string.Empty;   // e.g. "students"
    public string FieldName    { get; set; } = string.Empty;   // e.g. "first_name"
    public string DisplayLabel { get; set; } = string.Empty;   // e.g. "First Name"
    public bool   IsVisible    { get; set; } = true;
    public bool   IsSortable   { get; set; } = true;
    public bool   IsFilterable { get; set; } = false;
    public bool   IsSearchable { get; set; } = true;
    public int    DisplayOrder { get; set; } = 0;
}
