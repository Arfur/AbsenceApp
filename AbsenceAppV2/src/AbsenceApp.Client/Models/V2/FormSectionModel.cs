/*
===============================================================================
 File        : FormSectionModel.cs
 Namespace   : AbsenceApp.Client.Models.V2
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-21
 Updated     : 2026-03-21
-------------------------------------------------------------------------------
 Purpose     : Configuration model for a single bordered group of fields within FormPageV2. Carries section title, icon, and description text.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-21  Initial implementation.
-------------------------------------------------------------------------------
 Notes       :
   - Phase 5 model. No DI registration required.
===============================================================================
*/

namespace AbsenceApp.Client.Models.V2;

/// <summary>
/// Defines display configuration for a single form section group
/// rendered by <see cref="AbsenceApp.Client.Components.PageTemplatesV2.FormSectionV2"/>.
/// </summary>
public sealed class FormSectionModel
{
    /// <summary>Heading text displayed in the section header bar.</summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>Optional Bootstrap icon class shown beside the title (e.g. "bi-pencil").</summary>
    public string? Icon { get; set; }

    /// <summary>Optional description or hint text rendered below the title.</summary>
    public string? Description { get; set; }

    /// <summary>Number of columns in the form field grid (1, 2, or 3). Default: 2.</summary>
    public int Columns { get; set; } = 2;

    /// <summary>Optional CSS modifier class applied to the section wrapper.</summary>
    public string? CssClass { get; set; }
}
