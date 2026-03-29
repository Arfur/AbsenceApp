/*
===============================================================================
 File        : DesignSystemConfigService.cs
 Namespace   : AbsenceApp.Client.Services
 Author      : Michael
 Version     : 2.0.0
 Created     : 2026-03-21
 Updated     : 2026-03-29
-------------------------------------------------------------------------------
 Purpose     : V2 design system configuration service. Loads V2 design-system
               JSON config files on demand and caches them for the lifetime of
               the application. Reads files directly from disk using FileStream
               so it works correctly in MAUI Blazor Hybrid where native C# code
               cannot reach the WebView2 virtual http://localhost/ scheme.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-21  Initial implementation.
   - 2.0.0  2026-03-29  BUG FIX: Replaced HttpClient.GetFromJsonAsync with
                         FileStream reading from AppContext.BaseDirectory/wwwroot.
                         In MAUI Blazor Hybrid the C# HttpClient cannot reach
                         http://localhost/ — that scheme exists only inside the
                         WebView2 browser context, not in native C# code.
                         HttpClient dependency removed from constructor.
-------------------------------------------------------------------------------
 Notes       :
   - Phase 2 service. Register as Singleton in DI.
===============================================================================
*/

using System.Text.Json;
using System.Text.Json.Nodes;

namespace AbsenceApp.Client.Services;

/// <summary>
/// Singleton service that loads V2 design system JSON config files on demand
/// and caches them for the lifetime of the application.
/// </summary>
public class DesignSystemConfigService
{
    private static readonly string _wwwrootBase =
        Path.Combine(AppContext.BaseDirectory, "wwwroot");

    private JsonObject? _theme;
    private JsonObject? _menu;
    private JsonObject? _tableSchema;
    private JsonObject? _components;
    private JsonObject? _icons;

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true
    };

    public DesignSystemConfigService()
    {
    }

    // -------------------------------------------------------------------------

    // Public accessors

    // -------------------------------------------------------------------------
    public async Task<JsonObject> GetThemeAsync()
        => _theme ??= await LoadAsync("config/designsystem/theme.json");

    public async Task<JsonObject> GetMenuAsync()
        => _menu ??= await LoadAsync("config/designsystem/menu.json");

    public async Task<JsonObject> GetTableSchemaAsync()
        => _tableSchema ??= await LoadAsync("config/designsystem/table-schema.json");

    public async Task<JsonObject> GetComponentsAsync()
        => _components ??= await LoadAsync("config/designsystem/components.json");

    public async Task<JsonObject> GetIconsAsync()
        => _icons ??= await LoadAsync("config/designsystem/icons.json");

    private JsonObject? _globalSettingsMenu;

    public async Task<JsonObject> GetGlobalSettingsMenuAsync()
        => _globalSettingsMenu ??= await LoadAsync("config/designsystem/menu.globalsettings.json");

    /// <summary>
    /// Reads a nested value from a loaded config object given a dot-separated path.
    /// Returns null if any segment is not found.
    /// Example: GetValue(config, "defaults.pageSize") → 25
    /// </summary>
    public static JsonNode? GetValue(JsonObject config, string dotPath)
    {
        JsonNode? current = config;
        foreach (var segment in dotPath.Split('.'))
        {
            if (current is not JsonObject obj || !obj.TryGetPropertyValue(segment, out current))
                return null;
        }
        return current;
    }

    // -------------------------------------------------------------------------

    // Private

    // -------------------------------------------------------------------------
    private async Task<JsonObject> LoadAsync(string relativePath)
    {
        var fullPath = Path.Combine(_wwwrootBase, relativePath);
        await using var stream = new FileStream(
            fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
        var node = await JsonSerializer.DeserializeAsync<JsonNode>(stream, _jsonOptions)
                   ?? throw new InvalidOperationException(
                       $"DesignSystemConfigService: failed to load '{relativePath}'.");

        if (node is not JsonObject obj)
            throw new InvalidOperationException(
                $"DesignSystemConfigService: '{relativePath}' is not a JSON object.");

        return obj;
    }
}
