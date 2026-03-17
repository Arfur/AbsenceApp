/*
===============================================================================
 File        : ITableSettingsService.cs
 Namespace   : AbsenceApp.Core.Interfaces
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-17
 Updated     : 2026-03-17
-------------------------------------------------------------------------------
 Purpose     : Contract for reading and writing per-table field-level display
               configuration stored in the table_page_settings database table.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-17  Initial implementation.
===============================================================================
*/

using AbsenceApp.Core.DTOs;

namespace AbsenceApp.Core.Interfaces;

public interface ITableSettingsService
{
    /// <summary>
    /// Returns saved settings for <paramref name="pageName"/>.
    /// Returns hard-coded defaults when no settings have been persisted yet.
    /// </summary>
    Task<IReadOnlyList<TablePageSettingDto>> GetSettingsAsync(string pageName);

    /// <summary>
    /// Upserts all field rows for <paramref name="pageName"/>.
    /// New fields are inserted; existing fields are updated.
    /// </summary>
    Task SaveSettingsAsync(string pageName, IEnumerable<TablePageSettingDto> settings);

    /// <summary>
    /// Deletes all persisted settings for <paramref name="pageName"/>,
    /// reverting the page to hard-coded defaults.
    /// </summary>
    Task ResetToDefaultsAsync(string pageName);
}
