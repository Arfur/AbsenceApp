/*
===============================================================================
 File        : BrandingConfigModel.cs
 Namespace   : AbsenceApp.Client.Models.Theming
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-21
 Updated     : 2026-03-21
-------------------------------------------------------------------------------
 Purpose     : Strongly-typed model representing the V2 branding configuration
               loaded from wwwroot/config/designsystem/branding.json. Carries
               the application name, logo paths, primary accent colour override,
               and optional custom CSS class suffix used for white-labelling.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-21  Initial implementation (Phase 8).
-------------------------------------------------------------------------------
 Notes       :
   - Phase 8 model. No DI registration required.
   - Populated by BrandingServiceV2.LoadAsync() from branding.json.
   - All fields have safe fallback defaults so the app renders correctly even
     when branding.json is absent or partially populated.
===============================================================================
*/

namespace AbsenceApp.Client.Models.Theming;

/// <summary>
/// Runtime branding configuration loaded from branding.json.
/// Controls the application name, logo assets, and accent colour.
/// </summary>
public sealed class BrandingConfigModel
{
    // -------------------------------------------------------------------------
    // Identity
    // -------------------------------------------------------------------------
    /// <summary>Full application name displayed in the title bar and login screen.</summary>
    public string AppName { get; set; } = "AbsenceApp";

    /// <summary>Short name used in compact / sidebar-collapsed views.</summary>
    public string AppShortName { get; set; } = "AA";

    /// <summary>Organisation / school name shown in the header subtitle.</summary>
    public string OrganisationName { get; set; } = string.Empty;

    // -------------------------------------------------------------------------
    // Logo assets
    // -------------------------------------------------------------------------
    /// <summary>
    /// Relative path to the full-width logo image used in the sidebar header
    /// when expanded (e.g. "images/branding/logo.png").
    /// </summary>
    public string LogoUrl { get; set; } = "images/branding/logo.png";

    /// <summary>
    /// Relative path to the compact/icon-only logo image used when the sidebar
    /// is collapsed (e.g. "images/branding/logo-icon.png").
    /// </summary>
    public string LogoIconUrl { get; set; } = "images/branding/logo-icon.png";

    /// <summary>Alt text for logo images (used for accessibility).</summary>
    public string LogoAltText { get; set; } = "AbsenceApp";

    // -------------------------------------------------------------------------
    // Colour overrides
    // -------------------------------------------------------------------------
    /// <summary>
    /// Optional primary accent colour override (e.g. "#1d4ed8").
    /// When non-empty, ThemeServiceV2 injects this as --ds-accent on the root.
    /// Leave empty to use the default from theme.json.
    /// </summary>
    public string PrimaryAccentColor { get; set; } = string.Empty;

    /// <summary>
    /// Optional hover state of the primary accent colour (e.g. "#1e40af").
    /// Only applied when <see cref="PrimaryAccentColor"/> is also set.
    /// </summary>
    public string PrimaryAccentHoverColor { get; set; } = string.Empty;

    // -------------------------------------------------------------------------
    // White-label
    // -------------------------------------------------------------------------
    /// <summary>
    /// Optional CSS class suffix appended to the shell &lt;body&gt; element
    /// (e.g. "acme" → body class "brand-acme") for white-label CSS overrides.
    /// Empty string means no extra class is applied.
    /// </summary>
    public string BrandCssSuffix { get; set; } = string.Empty;

    /// <summary>Schema version string (e.g. "2.0.0").</summary>
    public string Version { get; set; } = "2.0.0";
}
