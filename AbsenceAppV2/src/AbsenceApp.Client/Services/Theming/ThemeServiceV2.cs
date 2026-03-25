/*
===============================================================================
 File        : ThemeServiceV2.cs
 Namespace   : AbsenceApp.Client.Services.Theming
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-21
 Updated     : 2026-03-21
-------------------------------------------------------------------------------
 Purpose     : V2 theme management service. Loads ThemeConfigModel from
               theme.json via DesignSystemConfigService, tracks the user's
               ThemeMode preference (Light / Dark / System), persists it via
               Microsoft.Maui.Storage.Preferences, and notifies subscribers
               via OnChange so components can call StateHasChanged.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-21  Initial implementation (Phase 8).
-------------------------------------------------------------------------------
 Notes       :
   - Phase 8 service. Register as Singleton in DI (Phase 10 step).
   - Does NOT modify AppStateStoreV2 directly. Components that need to keep
     the V1 dark-mode toggle in sync should subscribe to OnChange and call
     Store.ToggleDarkMode() / SetDarkMode() as needed.
   - System mode falls back to Light if the platform preference is unavailable.
===============================================================================
*/

using AbsenceApp.Client.Models.Theming;
using System.Text.Json.Nodes;

namespace AbsenceApp.Client.Services.Theming;

/// <summary>
/// Singleton service that owns the active <see cref="ThemeMode"/> preference
/// and exposes the resolved <see cref="ThemeConfigModel"/>. Persists the user's
/// choice across application launches via MAUI Preferences.
/// </summary>
public sealed class ThemeServiceV2
{
    // -------------------------------------------------------------------------
    // Dependencies and constants
    // -------------------------------------------------------------------------
    private readonly DesignSystemConfigService _config;

    private const string PreferenceKey = "v2.theme.mode";

    // -------------------------------------------------------------------------
    // State
    // -------------------------------------------------------------------------
    private ThemeConfigModel? _model;
    private bool              _loaded;

    // -------------------------------------------------------------------------
    // Change notification
    // -------------------------------------------------------------------------
    /// <summary>
    /// Raised whenever the active theme mode changes. V2 components subscribe
    /// so they can call StateHasChanged and re-render with the new theme class.
    /// </summary>
    public event Action? OnChange;

    // -------------------------------------------------------------------------
    // Constructor
    // -------------------------------------------------------------------------
    public ThemeServiceV2(DesignSystemConfigService config)
    {
        _config = config;
    }

    // -------------------------------------------------------------------------
    // Public API
    // -------------------------------------------------------------------------
    /// <summary>
    /// Returns the fully populated <see cref="ThemeConfigModel"/> for the
    /// current session, loading from theme.json on the first call.
    /// Subsequent calls return the cached instance.
    /// </summary>
    public async Task<ThemeConfigModel> GetConfigAsync()
    {
        if (!_loaded)
            await LoadAsync();
        return _model!;
    }

    /// <summary>
    /// Sets the active theme mode, persists the choice, and notifies subscribers.
    /// Safe to call from a component event handler without awaiting LoadAsync first.
    /// </summary>
    public async Task SetModeAsync(ThemeMode mode)
    {
        if (!_loaded) await LoadAsync();

        _model!.ActiveMode    = mode;
        _model.EffectiveMode  = ResolveEffective(mode);

        Preferences.Default.Set(PreferenceKey, (int)mode);
        Notify();
    }

    /// <summary>
    /// Cycles through Light → Dark → System → Light.
    /// </summary>
    public async Task CycleModeAsync()
    {
        if (!_loaded) await LoadAsync();
        var next = _model!.ActiveMode switch
        {
            ThemeMode.Light  => ThemeMode.Dark,
            ThemeMode.Dark   => ThemeMode.System,
            _                => ThemeMode.Light
        };
        await SetModeAsync(next);
    }

    /// <summary>
    /// Returns <c>true</c> when the resolved effective mode is Dark.
    /// Safe to call without awaiting — returns false until the first load.
    /// </summary>
    public bool IsDarkMode => _model?.EffectiveMode == ThemeMode.Dark;

    /// <summary>
    /// Returns the CSS class to apply to the body element for the current
    /// effective mode (empty string for Light, "dark" for Dark).
    /// Safe to call without awaiting — returns empty string until first load.
    /// </summary>
    public string ActiveBodyClass =>
        IsDarkMode ? (_model?.DarkCssClass ?? "dark") : string.Empty;

    // -------------------------------------------------------------------------
    // Private helpers
    // -------------------------------------------------------------------------
    private async Task LoadAsync()
    {
        JsonObject json = await _config.GetThemeAsync();

        // Read the persisted user preference (default: Light).
        var savedMode = (ThemeMode)Preferences.Default.Get(PreferenceKey, (int)ThemeMode.Light);

        // Extract values from theme.json with safe fallbacks.
        string darkCssClass  = json["themes"]?["dark"]?["cssClass"]?.GetValue<string>() ?? "dark";
        string fontFamily    = json["font"]?["family"]?.GetValue<string>()               ?? "Inter, Segoe UI, Arial, sans-serif";
        string fontSizeBase  = json["font"]?["sizeBase"]?.GetValue<string>()             ?? "14px";
        string headerHeight  = json["layout"]?["headerHeight"]?.GetValue<string>()       ?? "52px";
        string sidebarWidth  = json["layout"]?["sidebarWidth"]?.GetValue<string>()       ?? "230px";
        string sidebarColl   = json["layout"]?["sidebarCollapsed"]?.GetValue<string>()   ?? "52px";
        string version       = json["version"]?.GetValue<string>()                       ?? "2.0.0";

        _model = new ThemeConfigModel
        {
            ActiveMode            = savedMode,
            EffectiveMode         = ResolveEffective(savedMode),
            Version               = version,
            DarkCssClass          = darkCssClass,
            FontFamily            = fontFamily,
            FontSizeBase          = fontSizeBase,
            HeaderHeight          = headerHeight,
            SidebarWidth          = sidebarWidth,
            SidebarCollapsedWidth = sidebarColl
        };

        _loaded = true;
    }

    /// <summary>
    /// Resolves <see cref="ThemeMode.System"/> to the platform preference.
    /// Falls back to Light if the system preference cannot be determined.
    /// </summary>
    private static ThemeMode ResolveEffective(ThemeMode mode)
    {
        if (mode != ThemeMode.System) return mode;

        try
        {
            // AppInfo.RequestedTheme is available in MAUI on all target platforms.
            var osTheme = AppInfo.Current.RequestedTheme;
            return osTheme == AppTheme.Dark ? ThemeMode.Dark : ThemeMode.Light;
        }
        catch
        {
            return ThemeMode.Light;
        }
    }

    private void Notify() => OnChange?.Invoke();
}
