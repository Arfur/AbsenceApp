/*
===============================================================================
 File        : DetailSectionModel.cs
 Namespace   : AbsenceApp.Client.Models.V2
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-21
 Updated     : 2026-03-21
-------------------------------------------------------------------------------
 Purpose     : Configuration model for a single collapsible section within DetailPageV2. Carries section title, icon, field list, and collapsed-by-default flag.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-21  Initial implementation.
-------------------------------------------------------------------------------
 Notes       :
   - Phase 5 model. No DI registration required.
===============================================================================
*/

namespace AbsenceApp.Client.FrameworkV2.Models;

/// <summary>
/// Defines display configuration for a single collapsible detail section
/// rendered by <see cref="AbsenceApp.Client.Components.PageTemplatesV2.DetailSectionV2"/>.
/// </summary>
public sealed class DetailSectionModel
{
    /// <summary>Heading text displayed in the section header bar.</summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>Optional Bootstrap icon class shown beside the title (e.g. "bi-person").</summary>
    public string? Icon { get; set; }

    /// <summary>Number of columns in the field grid (1, 2, or 3). Default: 2.</summary>
    public int Columns { get; set; } = 2;

    /// <summary>Whether the section renders as collapsed on first load. Default: false.</summary>
    public bool CollapsedByDefault { get; set; } = false;

    /// <summary>Optional CSS modifier class applied to the section wrapper.</summary>
    public string? CssClass { get; set; }
}
