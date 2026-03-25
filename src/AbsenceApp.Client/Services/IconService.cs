/*
===============================================================================
 File        : IconService.cs
 Namespace   : AbsenceApp.Client.Services
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-21
 Updated     : 2026-03-21
-------------------------------------------------------------------------------
 Purpose     : V2 icon lookup service backed by Bootstrap Icons. Maps semantic
               icon names (e.g. 'user', 'class') to Bootstrap Icons class
               strings. Provides a centralised, type-safe way for components
               to resolve icon names without hard-coding 'bi bi-...' strings.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-21  Initial implementation.
-------------------------------------------------------------------------------
 Notes       :
   - Phase 2 service. Register as Singleton in DI.
===============================================================================
*/

using System.Text.Json.Nodes;

namespace AbsenceApp.Client.Services;

/// <summary>
/// Resolves Bootstrap Icons class strings from the V2 icon registry (icons.json).
/// Uses DesignSystemConfigService to load and cache the icon config.
/// </summary>
public class IconService
{
    private readonly DesignSystemConfigService _config;
    private JsonObject? _icons;

    // Well-known fallback icon if a key is not found.
    private const string FallbackIcon = "bi-question-circle";

    public IconService(DesignSystemConfigService config)
    {
        _config = config;
    }

    /// <summary>
    /// Resolves a dot-path icon key to a Bootstrap Icons class string.
    /// E.g. "actions.add" → "bi bi-plus-circle"
    ///      "status.success" → "bi bi-check-circle-fill"
    /// Returns a fallback icon class if the key is not found.
    /// </summary>
    public async Task<string> ResolveAsync(string dotPath)
    {
        var iconsConfig = await GetIconsConfigAsync();
        var iconsNode = iconsConfig["icons"] as JsonObject;

        if (iconsNode is null)
            return BuildClass(FallbackIcon);

        // Navigate dot path inside the "icons" object
        // e.g. "actions.add" → iconsNode["actions"]["add"]
        var segments = dotPath.Split('.', 2);
        if (segments.Length == 2
            && iconsNode[segments[0]] is JsonObject group
            && group[segments[1]] is JsonNode value)
        {
            return BuildClass(value.GetValue<string>());
        }

        // Try direct key lookup within any group
        foreach (var kvp in iconsNode)
        {
            if (kvp.Value is JsonObject groupObj && groupObj[dotPath] is JsonNode match)
                return BuildClass(match.GetValue<string>());
        }

        return BuildClass(FallbackIcon);
    }

    /// <summary>
    /// Synchronous resolution from a pre-loaded icon registry snapshot.
    /// Returns the raw Bootstrap icon class name (bi-*) only.
    /// Falls back to FallbackIcon if not found.
    /// </summary>
    public string Resolve(string dotPath, JsonObject iconsConfig)
    {
        var iconsNode = iconsConfig["icons"] as JsonObject;
        if (iconsNode is null) return FallbackIcon;

        var segments = dotPath.Split('.', 2);
        if (segments.Length == 2
            && iconsNode[segments[0]] is JsonObject group
            && group[segments[1]] is JsonNode value)
        {
            return value.GetValue<string>();
        }

        return FallbackIcon;
    }

    /// <summary>
    /// Returns all icon keys in a given group (e.g. "actions") as a dictionary of name → class.
    /// </summary>
    public async Task<Dictionary<string, string>> GetGroupAsync(string groupName)
    {
        var iconsConfig = await GetIconsConfigAsync();
        var iconsNode = iconsConfig["icons"] as JsonObject;
        var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        if (iconsNode?[groupName] is JsonObject group)
        {
            foreach (var item in group)
            {
                if (item.Value is not null)
                    result[item.Key] = item.Value.GetValue<string>();
            }
        }

        return result;
    }

    // -------------------------------------------------------------------------

    // Private

    // -------------------------------------------------------------------------
    private async Task<JsonObject> GetIconsConfigAsync()
        => _icons ??= await _config.GetIconsAsync();

    private static string BuildClass(string iconName)
        => $"bi {iconName}";
}
