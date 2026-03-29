/*
===============================================================================
 File        : TableConfigService.cs
 Namespace   : AbsenceApp.Client.Services.TableV2
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-21
 Updated     : 2026-03-21
-------------------------------------------------------------------------------
 Purpose     : V2 table configuration service. Manages per-schema column definitions for TableV2. Loads default column schemas, persists user column customisations (visibility, order), and returns the resolved column list for a given schema key.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-21  Initial implementation.
-------------------------------------------------------------------------------
 Notes       :
   - Phase 4 service. Register as Singleton in DI.
===============================================================================
*/

using AbsenceApp.Client.Models.TableV2;
using AbsenceApp.Client.Services;
using System.Text.Json;

namespace AbsenceApp.Client.FrameworkV2.Services;

/// <summary>
/// Provides per-table column definitions loaded from table-schema.json.
/// Register as Singleton in MauiProgram.cs when instructed.
/// </summary>
public sealed class TableConfigService
{
    private readonly DesignSystemConfigService _config;

    private readonly Dictionary<string, List<TableColumnModel>> _cache = new();
    private readonly SemaphoreSlim _lock = new(1, 1);

    private static readonly JsonSerializerOptions _jsonOpts = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    // Column default values if JSON omits the property
    private const bool DefaultSortable = true;
    private const bool DefaultFilterable = true;
    private const bool DefaultHideable = true;
    private const bool DefaultReorderable = true;
    private const bool DefaultVisible = true;

    public TableConfigService(DesignSystemConfigService config)
    {
        _config = config;
    }

    /// <summary>
    /// Returns the column list for a given schema key (e.g. "absences").
    /// Returns a default single-column list if the key is not found.
    /// </summary>
    public async Task<List<TableColumnModel>> GetColumnsAsync(string schemaKey)
    {
        if (_cache.TryGetValue(schemaKey, out var cached)) return cached;

        await _lock.WaitAsync();
        try
        {
            if (_cache.TryGetValue(schemaKey, out cached)) return cached;

            var schema = await _config.GetTableSchemaAsync();

            if (!schema.TryGetPropertyValue("tables", out var tablesNode) || tablesNode is null)
                return CacheEmpty(schemaKey);

            if (!tablesNode.AsObject().TryGetPropertyValue(schemaKey, out var tableNode) || tableNode is null)
                return CacheEmpty(schemaKey);

            if (!tableNode.AsObject().TryGetPropertyValue("columns", out var colsNode) || colsNode is null)
                return CacheEmpty(schemaKey);

            var columns = JsonSerializer.Deserialize<List<TableColumnModel>>(
                colsNode.ToJsonString(), _jsonOpts) ?? [];

            // Ensure Order field is set if absent from JSON
            for (int i = 0; i < columns.Count; i++)
                if (columns[i].Order == 0 && i > 0)
                    columns[i].Order = i;

            _cache[schemaKey] = columns;
            return columns;
        }
        catch
        {
            return CacheEmpty(schemaKey);
        }
        finally
        {
            _lock.Release();
        }
    }

    /// <summary>
    /// Returns default page-size for a given table schema (falls back to 25).
    /// </summary>
    public async Task<int> GetDefaultPageSizeAsync(string schemaKey)
    {
        try
        {
            var schema = await _config.GetTableSchemaAsync();
            var defaultsNode = DesignSystemConfigService.GetValue(schema, "defaults.pageSize");
            return defaultsNode?.GetValue<int>() ?? 25;
        }
        catch
        {
            return 25;
        }
    }

    /// <summary>
    /// Returns available page-size options for a given table schema.
    /// </summary>
    public async Task<int[]> GetPageSizeOptionsAsync(string schemaKey)
    {
        try
        {
            var schema = await _config.GetTableSchemaAsync();
            var node = DesignSystemConfigService.GetValue(schema, "defaults.pageSizeOptions");
            if (node is null) return [10, 25, 50, 100];
            return JsonSerializer.Deserialize<int[]>(node.ToJsonString()) ?? [10, 25, 50, 100];
        }
        catch
        {
            return [10, 25, 50, 100];
        }
    }

    private List<TableColumnModel> CacheEmpty(string key)
    {
        _cache[key] = [];
        return _cache[key];
    }
}
