/*
===============================================================================
 File        : IDesignSystemService.cs
 Namespace   : AbsenceApp.Client.Services.DesignSystem
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-26
-------------------------------------------------------------------------------
 Purpose     : Interface for the design system service. Defines placeholder
               preview methods for all global settings categories (buttons,
               tables, forms, dashboards, typography, colours). Implementations
               return hard-coded preview values only — no JSON is loaded.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-26  Initial implementation (Phase 1 placeholder).
-------------------------------------------------------------------------------
 Notes       :
   - Phase 2 will replace the hard-coded return values with real JSON loading.
   - Do NOT load JSON or implement dynamic defaults here.
===============================================================================
*/

namespace AbsenceApp.Client.Services.DesignSystem;

/// <summary>
/// Provides hard-coded preview values for each global design-system category.
/// Phase 1 placeholder only — no JSON is loaded.
/// </summary>
public interface IDesignSystemService
{
    /// <summary>Returns a preview description for the Buttons configuration.</summary>
    string GetButtonPreview();

    /// <summary>Returns a preview description for the Tables configuration.</summary>
    string GetTablePreview();

    /// <summary>Returns a preview description for the Forms configuration.</summary>
    string GetFormPreview();

    /// <summary>Returns a preview description for the Dashboards configuration.</summary>
    string GetDashboardPreview();

    /// <summary>Returns a preview description for the Typography configuration.</summary>
    string GetTypographyPreview();

    /// <summary>Returns a preview description for the Colors configuration.</summary>
    string GetColorPreview();
}
