/*
===============================================================================
 File        : DesignSystemConfigService.cs
 Namespace   : AbsenceApp.Client.Services
 Author      : Michael
 Version     : 3.3.1
 Created     : 2026-03-21
 Updated     : 2026-06-26
-------------------------------------------------------------------------------
 Purpose     : V2 design system configuration service. Loads V2 design-system
               JSON config files on demand and caches them for the lifetime of
               the application. Phase 2 adds full dynamic regeneration.

               Phase 4.1 update:
                 - ComponentGroup removed from domain model.
                 - Component (domain) now maps to ComponentGroup (DB).
                 - SQL updated to SELECT ComponentGroup AS Component.
                 - Grouping updated to use Component.
                 - DTO updated to expose Component instead of ComponentGroup.

-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-21  Initial implementation.
   - 2.0.0  2026-03-29  Replaced HttpClient with FileStream loader.
   - 3.0.0  2026-06-02  Added full dynamic regeneration.
   - 3.0.1  2026-06-02  Added full debug tracing + MSIX-safe output path +
                         exception surfacing + async fixes.
   - 3.0.2  2026-06-02  FIX: Prevented crash in cssVariables sorting by ensuring
                         values are sorted as strings, not JsonNode.
   - 3.1.0  2026-06-03  Phase 4.1 — Replaced ComponentGroup with Component,
                        updated SQL, grouping, DTO, and dynamic JSON builder.
   - 3.2.0  2026-06-03  Added DefaultValue + CurrentValue support and resolved
                        token output in dynamic JSON builder.
   - 3.3.0  2026-06-04  FIX: Components loader now prioritises dynamic
                        components.json from LocalState before falling back
                        to static wwwroot version.
    - 3.3.1  2026-06-26  Added Step 3 dynamic button resolver methods:
                        GetButtonTokens(), GetButtonPreviewCss(), ResolveButtonCss().
                        No UI changes; read-only resolver stage.
-------------------------------------------------------------------------------
 Notes       :
   - Phase 2 service. Register as Singleton in DI.
===============================================================================
*/

using System.Diagnostics;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using Dapper;
using Microsoft.Maui.Storage;

namespace AbsenceApp.Client.Services;
[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Interoperability",
    "CA1416:Validate platform compatibility",
    Justification = "This MAUI build targets Windows MSIX only.")]

public class DesignSystemConfigService
{
    private readonly IDbConnection _db;

    private static readonly string _wwwrootBase =
        Path.Combine(AppContext.BaseDirectory, "wwwroot");

    private static readonly string _msixOutputBase =
        FileSystem.AppDataDirectory;

    private JsonObject? _theme;
    private JsonObject? _menu;
    private JsonObject? _tableSchema;
    private JsonObject? _components;
    private JsonObject? _icons;
    private JsonObject? _globalSettingsMenu;

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true
    };

    public DesignSystemConfigService(IDbConnection db)
    {
        _db = db;
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

    // -------------------------------------------------------------------------
    // FIX APPLIED HERE — Gemini’s solution
    // -------------------------------------------------------------------------
    public async Task<JsonObject> GetComponentsAsync()
    {
        if (_components != null)
            return _components;

        // 1. Try dynamic regenerated components.json from LocalState
        var localPath = Path.Combine(FileSystem.AppDataDirectory, "components.json");

        if (File.Exists(localPath))
        {
            try
            {
                Console.WriteLine($"DESIGN SYSTEM: Loading dynamic components.json → {localPath}");
                Debug.WriteLine($"DESIGN SYSTEM: Loading dynamic components.json → {localPath}");

                await using var stream = new FileStream(localPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                var node = await JsonSerializer.DeserializeAsync<JsonNode>(stream, _jsonOptions);

                if (node is JsonObject obj)
                {
                    _components = obj;
                    return _components;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DESIGN SYSTEM: WARNING — Failed to load dynamic components.json. Falling back. {ex.Message}");
                Debug.WriteLine($"DESIGN SYSTEM: WARNING — Failed to load dynamic components.json. Falling back. {ex.Message}");
            }
        }

        // 2. Fallback to static wwwroot version
        _components = await LoadAsync("config/designsystem/components.json");
        return _components;
    }

    public async Task<JsonObject> GetIconsAsync()
        => _icons ??= await LoadAsync("config/designsystem/icons.json");

    public async Task<JsonObject> GetGlobalSettingsMenuAsync()
        => _globalSettingsMenu ??= await LoadAsync("config/designsystem/menu.globalsettings.json");

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
// Private loader
// -------------------------------------------------------------------------

private async Task<JsonObject> LoadAsync(string relativePath)
{
    var fullPath = Path.Combine(_wwwrootBase, relativePath);

    Console.WriteLine($"DESIGN SYSTEM: Loading JSON → {fullPath}");
    Debug.WriteLine($"DESIGN SYSTEM: Loading JSON → {fullPath}");

    await using var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);

    var node = await JsonSerializer.DeserializeAsync<JsonNode>(stream, _jsonOptions)
               ?? throw new InvalidOperationException(
                   $"DesignSystemConfigService: failed to load '{relativePath}'.");

    if (node is not JsonObject obj)
        throw new InvalidOperationException(
            $"DesignSystemConfigService: '{relativePath}' is not a JSON object.");

    return obj;
}


// -------------------------------------------------------------------------
// Step 3 — Dynamic Button Resolver (Read‑Only Stage)
// -------------------------------------------------------------------------

public IReadOnlyList<DesignTokenRow> GetButtonTokens(string family, string variant)
{
    var prefix = $"btn.{family}.{variant}.";

    if (_components == null)
        return Array.Empty<DesignTokenRow>();

    var result = new List<DesignTokenRow>();

    foreach (var comp in _components)
    {
        if (comp.Value is not JsonObject obj)
            continue;

        if (!obj.TryGetPropertyValue("resolved", out var resolvedNode))
            continue;

        if (resolvedNode is not JsonObject resolved)
            continue;

        foreach (var kvp in resolved)
        {
            if (kvp.Key.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            {
                result.Add(new DesignTokenRow
                {
                    CssVariable = kvp.Key,
                    CurrentValue = kvp.Value?.ToString() ?? ""
                });
            }
        }
    }

    return result;
}

public string GetButtonPreviewCss(string family, string variant)
{
    var tokens = GetButtonTokens(family, variant);

    if (tokens.Count == 0)
        return "/* No tokens found for this variant */";

    var sb = new System.Text.StringBuilder();

    foreach (var token in tokens)
    {
        var shortName = token.CssVariable.Replace($"btn.{family}.{variant}.", "");
        sb.AppendLine($"{shortName}: var(--{token.CssVariable});");
    }

    return sb.ToString();
}

public string ResolveButtonCss(string family, string variant)
{
    return $"dsv2-btn dsv2-btn--{variant}";
}

// -------------------------------------------------------------------------
// Phase 2: Full dynamic regeneration
// -------------------------------------------------------------------------

    public async Task RegenerateDesignSystemAsync()
    {
        try
        {
            Console.WriteLine("DESIGN SYSTEM: Regeneration started");
            Debug.WriteLine("DESIGN SYSTEM: Regeneration started");

            var sql = @"
                SELECT 
                    ComponentGroup AS Component,
                    Family,
                    Variant,
                    GroupName,
                    TokenKey,
                    CssVariable,
                    DefaultValue,
                    CurrentValue
                FROM designtokens
                WHERE IsActive = 1
                ORDER BY ComponentGroup, Family, Variant, SortOrder;
            ";

            var rows = (await _db.QueryAsync<DesignTokenRow>(sql)).ToList();

            Console.WriteLine($"DESIGN SYSTEM: Loaded {rows.Count} token rows");
            Debug.WriteLine($"DESIGN SYSTEM: Loaded {rows.Count} token rows");

            var dynamicJson = BuildDynamicComponentsJson(rows);

            var staticPath = Path.Combine(_wwwrootBase, "config/designsystem/static-components.json");

            Console.WriteLine($"DESIGN SYSTEM: Loading static JSON → {staticPath}");
            Debug.WriteLine($"DESIGN SYSTEM: Loading static JSON → {staticPath}");

            var staticJson = JsonSerializer.Deserialize<JsonObject>(File.ReadAllText(staticPath), _jsonOptions)
                            ?? throw new InvalidOperationException("Failed to load static-components.json");

            foreach (var kvp in staticJson)
            {
                dynamicJson[kvp.Key] = kvp.Value!.DeepClone();
            }

            var outputPath = Path.Combine(_msixOutputBase, "components.json");

            Console.WriteLine($"DESIGN SYSTEM: Writing components.json → {outputPath}");
            Debug.WriteLine($"DESIGN SYSTEM: Writing components.json → {outputPath}");

            var outputOptions = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(outputPath, dynamicJson.ToJsonString(outputOptions));

            Console.WriteLine("DESIGN SYSTEM: Regeneration completed successfully");
            Debug.WriteLine("DESIGN SYSTEM: Regeneration completed successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine("DESIGN SYSTEM: ERROR during regeneration");
            Console.WriteLine(ex.ToString());

            Debug.WriteLine("DESIGN SYSTEM: ERROR during regeneration");
            Debug.WriteLine(ex.ToString());

            throw;
        }
    }

    // -------------------------------------------------------------------------
    // Dynamic JSON builder
    // -------------------------------------------------------------------------

    private JsonObject BuildDynamicComponentsJson(List<DesignTokenRow> rows)
    {
        Console.WriteLine("DESIGN SYSTEM: Building dynamic components.json structure");
        Debug.WriteLine("DESIGN SYSTEM: Building dynamic components.json structure");

        var root = new JsonObject
        {
            ["_comment"] = "AbsenceApp V2 Design System — components.json — Phase 2",
            ["_description"] = "Global default configuration for V2 design system components.",
            ["version"] = "2.1.0",
            ["_created"] = "2026-05-22T00:00:00Z",
            ["_updated"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
        };

        var groups = rows.GroupBy(r => r.Component);

        foreach (var group in groups)
        {
            Console.WriteLine($"DESIGN SYSTEM: Processing group '{group.Key}'");
            Debug.WriteLine($"DESIGN SYSTEM: Processing group '{group.Key}'");

            var comp = new JsonObject
            {
                ["tokenFamily"] = group.Key,
                ["defaultVariant"] = "primary",
                ["defaultSize"] = "md"
            };

            comp["preview"] = new JsonObject
            {
                ["cssVariables"] = new JsonArray(
                    group.Select(r => r.CssVariable)
                         .Where(v => !string.IsNullOrWhiteSpace(v))
                         .Distinct()
                         .OrderBy(v => v)
                         .Select(v => (JsonNode?)v)
                         .ToArray()
                )
            };

            comp["variants"] = new JsonArray(
                group.Select(r => r.Variant)
                     .Distinct()
                     .OrderBy(v => v)
                     .Select(v => (JsonNode?)v)
                     .ToArray()
            );

            comp["variantAliases"] = new JsonObject(
                group.GroupBy(r => r.GroupName)
                     .ToDictionary(
                         g => g.Key,
                         g => (JsonNode?)g.First().Variant
                     )
            );

            comp["tokenMappings"] = new JsonObject(
                group.ToDictionary(
                    r => r.TokenKey,
                    r => (JsonNode?)r.CssVariable
                )
            );

            comp["editor"] = new JsonObject
            {
                ["groups"] = new JsonObject(
                    group.GroupBy(r => r.GroupName)
                         .ToDictionary(
                             g => g.Key,
                             g => (JsonNode?)new JsonArray(
                                 g.Select(r => (JsonNode?)r.TokenKey).ToArray()
                             )
                         )
                )
            };

            comp["resolved"] = new JsonObject(
                group.ToDictionary(
                    r => r.CssVariable,
                    r => (JsonNode?)(r.CurrentValue ?? r.DefaultValue)
                )
            );

            root[group.Key] = comp;
        }

        return root;
    }

    // -------------------------------------------------------------------------
    // DTO
    // -------------------------------------------------------------------------

    public class DesignTokenRow
    {
        public string Component { get; set; } = "";
        public string Family { get; set; } = "";
        public string Variant { get; set; } = "";
        public string GroupName { get; set; } = "";
        public string TokenKey { get; set; } = "";
        public string CssVariable { get; set; } = "";
        public string DefaultValue { get; set; } = "";
        public string? CurrentValue { get; set; }
    }
}
