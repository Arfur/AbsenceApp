/*
===============================================================================
 File        : SettingsGroupModel.cs
 Namespace   : AbsenceApp.Client.Models.V2
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-21
 Updated     : 2026-03-21
-------------------------------------------------------------------------------
 Purpose     : Configuration model for a settings group rendered within SettingsPageV2. Carries group title, icon, and a list of SettingsItemModel entries.
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
/// Represents a named group of settings items displayed by
/// <see cref="AbsenceApp.Client.Components.PageTemplatesV2.SettingsGroupV2"/>.
/// </summary>
public sealed class SettingsGroupModel
{
    /// <summary>Group heading text displayed at the top of the bordered group card.</summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>Optional sub-description rendered beneath the group title.</summary>
    public string? Description { get; set; }

    /// <summary>Optional Bootstrap icon class shown beside the group title.</summary>
    public string? Icon { get; set; }

    /// <summary>The settings items belonging to this group.</summary>
    public List<SettingsItemModel> Items { get; set; } = [];

    /// <summary>Optional CSS modifier class applied to the group wrapper.</summary>
    public string? CssClass { get; set; }
}
