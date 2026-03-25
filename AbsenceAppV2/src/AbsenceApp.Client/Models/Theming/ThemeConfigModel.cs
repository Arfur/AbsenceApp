/*
===============================================================================
 File        : ThemeConfigModel.cs
 Namespace   : AbsenceApp.Client.Models.Theming
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-21
 Updated     : 2026-03-21
-------------------------------------------------------------------------------
 Purpose     : Strongly-typed model representing the V2 theme configuration
               loaded from wwwroot/config/designsystem/theme.json. Carries
               the active ThemeMode preference and the runtime colour values
               for both light and dark schemes.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-21  Initial implementation (Phase 8).
-------------------------------------------------------------------------------
 Notes       :
   - Phase 8 model. No DI registration required.
   - Populated by ThemeServiceV2.LoadAsync() from theme.json.
   - The ActiveMode property is set at runtime from Preferences; it is NOT
     persisted inside theme.json.
===============================================================================
*/

namespace AbsenceApp.Client.Models.Theming;

/// <summary>
/// Runtime state of the V2 theme system. Mirrors the structure of
/// wwwroot/config/designsystem/theme.json with an additional ActiveMode
/// property that tracks the user's current preference.
/// </summary>
public sealed class ThemeConfigModel
{
    // -------------------------------------------------------------------------
    // Runtime state
    // -------------------------------------------------------------------------
    /// <summary>The currently active theme mode selected by the user.</summary>
    public ThemeMode ActiveMode { get; set; } = ThemeMode.Light;

    /// <summary>
    /// Resolved effective mode after applying System preference.
    /// Equal to <see cref="ActiveMode"/> unless <see cref="ThemeMode.System"/>
    /// is selected, in which case it reflects the platform value (Light or Dark).
    /// </summary>
    public ThemeMode EffectiveMode { get; set; } = ThemeMode.Light;

    // -------------------------------------------------------------------------
    // Config values from theme.json
    // -------------------------------------------------------------------------
    /// <summary>Schema version string from the JSON file (e.g. "2.0.0").</summary>
    public string Version { get; set; } = "2.0.0";

    /// <summary>
    /// CSS class name applied to the &lt;body&gt; element when dark mode is active.
    /// Matches the value from theme.json themes.dark.cssClass.
    /// </summary>
    public string DarkCssClass { get; set; } = "dark";

    /// <summary>Base font family stack.</summary>
    public string FontFamily { get; set; } = "Inter, Segoe UI, Arial, sans-serif";

    /// <summary>Base font size (e.g. "14px").</summary>
    public string FontSizeBase { get; set; } = "14px";

    /// <summary>Header height token (e.g. "52px").</summary>
    public string HeaderHeight { get; set; } = "52px";

    /// <summary>Sidebar full-width token (e.g. "230px").</summary>
    public string SidebarWidth { get; set; } = "230px";

    /// <summary>Sidebar collapsed-width token (e.g. "52px").</summary>
    public string SidebarCollapsedWidth { get; set; } = "52px";
}
