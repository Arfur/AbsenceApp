/*
===============================================================================
 File        : SettingsItemModel.cs
 Namespace   : AbsenceApp.Client.Models.V2
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-21
 Updated     : 2026-03-21
-------------------------------------------------------------------------------
 Purpose     : Configuration model for a single settings row rendered inside a SettingsGroupV2. Carries icon, label, description, and hint text.
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
/// Represents a single settings item/row rendered by
/// <see cref="AbsenceApp.Client.Components.PageTemplatesV2.SettingsItemV2"/>.
/// </summary>
public sealed class SettingsItemModel
{
    /// <summary>Unique key identifying this settings item (e.g. "theme.darkMode").</summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>Primary label displayed on the left side of the row.</summary>
    public string Label { get; set; } = string.Empty;

    /// <summary>Optional supporting description shown below the label.</summary>
    public string? Description { get; set; }

    /// <summary>Optional hint text shown on the right of the control when no control slot is supplied.</summary>
    public string? HintText { get; set; }

    /// <summary>Optional Bootstrap icon class shown beside the label.</summary>
    public string? Icon { get; set; }

    /// <summary>Whether this item is currently disabled/read-only. Default: false.</summary>
    public bool Disabled { get; set; } = false;

    /// <summary>Optional badge text label (e.g. "Beta", "New").</summary>
    public string? BadgeText { get; set; }
}
