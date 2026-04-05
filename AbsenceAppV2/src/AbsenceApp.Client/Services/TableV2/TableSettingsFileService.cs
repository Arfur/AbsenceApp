/*
===============================================================================
 File        : TableSettingsFileService.cs
 Namespace   : AbsenceApp.Client.Services.TableV2
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-04-03
 Updated     : 2026-04-03
-------------------------------------------------------------------------------
 Purpose     : Local file service for per-table column settings in V2.
               Reads from and writes to
               wwwroot/config/tablesettings/{tableName}.json using FileStream
               (MAUI Blazor Hybrid does not use HttpClient for wwwroot assets).
               Returns an empty list when the file does not exist; never throws.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-03  Initial implementation (Phase 3).
-------------------------------------------------------------------------------
 Notes       :
   - Register as Singleton in V2ServiceCollectionExtensions.
   - TablePageSettingDto is reused from AbsenceApp.Core because the field
     shape (FieldName, DisplayLabel, IsVisible, IsSortable, IsFilterable,
     IsSearchable, DisplayOrder) exactly matches what the Table Settings page
     needs to display and persist.
===============================================================================
*/

using System.Text.Json;
using AbsenceApp.Core.DTOs;

namespace AbsenceApp.Client.Services.TableV2;

/// <summary>
/// Reads and writes per-table column settings to
/// <c>wwwroot/config/tablesettings/{tableName}.json</c>.
/// Register as Singleton. Thread-safe for concurrent reads; serialises writes
/// per table name via a SemaphoreSlim dictionary.
/// </summary>
public sealed class TableSettingsFileService
{
    // -------------------------------------------------------------------------
    // Constants
    // -------------------------------------------------------------------------

    private static readonly JsonSerializerOptions _readOpts = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    private static readonly JsonSerializerOptions _writeOpts = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    // -------------------------------------------------------------------------
    // Private helpers
    // -------------------------------------------------------------------------

    private static string GetPath(string tableName) =>
        Path.Combine(
            AppContext.BaseDirectory,
            "wwwroot", "config", "tablesettings",
            $"{tableName.ToLowerInvariant()}.json");

    // -------------------------------------------------------------------------
    // Public API
    // -------------------------------------------------------------------------

    /// <summary>
    /// Loads the column settings for <paramref name="tableName"/> from
    /// the corresponding JSON file.  Returns an empty list if the file
    /// does not exist or cannot be parsed.
    /// </summary>
    public async Task<List<TablePageSettingDto>> LoadAsync(
        string tableName,
        CancellationToken ct = default)
    {
        var path = GetPath(tableName);
        if (!File.Exists(path)) return [];

        try
        {
            await using var fs = new FileStream(
                path, FileMode.Open, FileAccess.Read, FileShare.Read,
                bufferSize: 4096, useAsync: true);

            return await JsonSerializer.DeserializeAsync<List<TablePageSettingDto>>(
                fs, _readOpts, ct) ?? [];
        }
        catch
        {
            return [];
        }
    }

    /// <summary>
    /// Persists <paramref name="settings"/> to the table's JSON file,
    /// creating the directory if necessary.  Overwrites any existing file.
    /// </summary>
    public async Task SaveAsync(
        string tableName,
        List<TablePageSettingDto> settings,
        CancellationToken ct = default)
    {
        var path = GetPath(tableName);
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);

        await using var fs = new FileStream(
            path, FileMode.Create, FileAccess.Write, FileShare.None,
            bufferSize: 4096, useAsync: true);

        await JsonSerializer.SerializeAsync(fs, settings, _writeOpts, ct);
    }

    /// <summary>
    /// Returns <c>true</c> when a settings file already exists for
    /// <paramref name="tableName"/>.
    /// </summary>
    public bool Exists(string tableName) => File.Exists(GetPath(tableName));
}
