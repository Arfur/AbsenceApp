/*
===============================================================================
 File        : TablePageSettingDto.cs
 Namespace   : AbsenceApp.Core.DTOs
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-17
 Updated     : 2026-03-17
-------------------------------------------------------------------------------
 Purpose     : Data Transfer Object representing one row of field-level display
               configuration for a table page.  Used by ITableSettingsService
               and TableSettingsViewModel.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-17  Initial implementation.
===============================================================================
*/

namespace AbsenceApp.Core.DTOs;

public class TablePageSettingDto
{
    public int    Id           { get; set; }
    public string PageName     { get; set; } = string.Empty;
    public string FieldName    { get; set; } = string.Empty;
    public string DisplayLabel { get; set; } = string.Empty;
    public bool   IsVisible    { get; set; } = true;
    public bool   IsSortable   { get; set; } = true;
    public bool   IsFilterable { get; set; } = false;
    public bool   IsSearchable { get; set; } = true;
    public int    DisplayOrder { get; set; } = 0;
}
