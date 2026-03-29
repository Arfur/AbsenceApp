/*
===============================================================================
 File        : DesignSystemService.cs
 Namespace   : AbsenceApp.Client.Services.DesignSystem
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-26
-------------------------------------------------------------------------------
 Purpose     : Phase 1 placeholder implementation of IDesignSystemService.
               Returns hard-coded preview strings for each global settings
               category. No JSON is loaded, no dynamic defaults are applied.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-26  Initial implementation (Phase 1 placeholder).
-------------------------------------------------------------------------------
 Notes       :
   - Register as Singleton in V2ServiceCollectionExtensions.
   - Phase 2 will replace hard-coded values with real JSON loading from
     wwwroot/config/global/*.json.
   - Do NOT load JSON or implement dynamic defaults here.
===============================================================================
*/

namespace AbsenceApp.Client.Services.DesignSystem;

/// <summary>
/// Phase 1 placeholder implementation. Returns hard-coded preview values only.
/// </summary>
public sealed class DesignSystemService : IDesignSystemService
{
    /// <inheritdoc/>
    public string GetButtonPreview()
        => "Default button style: filled, rounded-8, primary colour #0d6b7a.";

    /// <inheritdoc/>
    public string GetTablePreview()
        => "Default table style: bordered, striped rows, page size 25.";

    /// <inheritdoc/>
    public string GetFormPreview()
        => "Default form style: stacked labels, full-width inputs, 14px text.";

    /// <inheritdoc/>
    public string GetDashboardPreview()
        => "Default dashboard style: 4-column stat card grid, accent colour #2563eb.";

    /// <inheritdoc/>
    public string GetTypographyPreview()
        => "Default typeface: Inter / Segoe UI, base size 14px, heading weight 700.";

    /// <inheritdoc/>
    public string GetColorPreview()
        => "Default palette: primary #0d6b7a, accent #2563eb, background #eaecf0.";
}
