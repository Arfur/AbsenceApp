/*
===============================================================================
 File        : BrandingServiceV2.cs
 Namespace   : AbsenceApp.Client.Services.Theming
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-21
 Updated     : 2026-03-21
-------------------------------------------------------------------------------
 Purpose     : V2 branding management service. Loads BrandingConfigModel from
               wwwroot/config/designsystem/branding.json via HttpClient, caches
               the result for the lifetime of the application, and raises an
               OnChange event so components can react when branding is reloaded.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-21  Initial implementation (Phase 8).
-------------------------------------------------------------------------------
 Notes       :
   - Phase 8 service. Register as Singleton in DI (Phase 10 step).
   - branding.json is optional. When the file is absent or malformed the
     service returns a BrandingConfigModel with safe default values so the
     app continues to function without any branding overrides.
   - ThemeSettingsPageV2 injects this service to display and edit branding
     settings at runtime (preview only — not persisted in Phase 8).
===============================================================================
*/

using AbsenceApp.Client.Models.Theming;
using System.Net.Http.Json;
using System.Text.Json;

namespace AbsenceApp.Client.FrameworkV2.Services;

/// <summary>
/// Singleton service that owns the <see cref="BrandingConfigModel"/> loaded
/// from branding.json. Provides a safe <see cref="GetConfigAsync"/> that
/// returns defaults when the JSON file is unavailable.
/// </summary>
public sealed class BrandingServiceV2
{
    // -------------------------------------------------------------------------
    // Dependencies
    // -------------------------------------------------------------------------
    private readonly HttpClient _http;

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        ReadCommentHandling         = JsonCommentHandling.Skip,
        AllowTrailingCommas         = true
    };

    // -------------------------------------------------------------------------
    // State
    // -------------------------------------------------------------------------
    private BrandingConfigModel? _model;
    private bool                 _loaded;

    // -------------------------------------------------------------------------
    // Change notification
    // -------------------------------------------------------------------------
    /// <summary>
    /// Raised when the branding configuration is reloaded. Components that
    /// display branding values (logo, app name, accent) should subscribe and
    /// call StateHasChanged when this fires.
    /// </summary>
    public event Action? OnChange;

    // -------------------------------------------------------------------------
    // Constructor
    // -------------------------------------------------------------------------
    public BrandingServiceV2(HttpClient http)
    {
        _http = http;
    }

    // -------------------------------------------------------------------------
    // Public API
    // -------------------------------------------------------------------------
    /// <summary>
    /// Returns the <see cref="BrandingConfigModel"/> for the current session,
    /// loading from branding.json on the first call. Returns default values
    /// if the file is absent or cannot be parsed.
    /// </summary>
    public async Task<BrandingConfigModel> GetConfigAsync()
    {
        if (!_loaded)
            await LoadAsync();
        return _model!;
    }

    /// <summary>
    /// Forces a reload of branding.json and notifies subscribers.
    /// Use this when branding is changed at runtime (e.g. admin page).
    /// </summary>
    public async Task ReloadAsync()
    {
        _loaded = false;
        await LoadAsync();
        Notify();
    }

    // -------------------------------------------------------------------------
    // Private helpers
    // -------------------------------------------------------------------------
    private async Task LoadAsync()
    {
        try
        {
            var model = await _http.GetFromJsonAsync<BrandingConfigModel>(
                "config/designsystem/branding.json", _jsonOptions);

            _model = model ?? new BrandingConfigModel();
        }
        catch (Exception)
        {
            // branding.json is optional — fall back to safe defaults.
            _model = new BrandingConfigModel();
        }

        _loaded = true;
    }

    private void Notify() => OnChange?.Invoke();
}
