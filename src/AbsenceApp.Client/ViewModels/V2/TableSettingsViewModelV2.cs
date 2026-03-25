/*
===============================================================================
 File        : TableSettingsViewModelV2.cs
 Namespace   : AbsenceApp.Client.ViewModels.V2
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-21
 Updated     : 2026-03-21
-------------------------------------------------------------------------------
 Purpose     : ViewModel for TableSettingsPageV2. Holds a working copy of
               column configuration state (visibility, order) and exposes
               methods to apply, cancel, or reset to defaults without
               immediate side effects on the original schema.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-21  Initial implementation (Phase 4).
-------------------------------------------------------------------------------
 Notes       :
   - Register as Scoped in DI (Phase 10).
===============================================================================
*/

using AbsenceApp.Client.Models.TableV2;

namespace AbsenceApp.Client.ViewModels.V2;

/// <summary>
/// Drives the TableSettingsPageV2 component. Manages the working copy of column
/// settings so changes can be confirmed or cancelled without immediate side effects.
/// </summary>
public sealed class TableSettingsViewModelV2
{
    // -------------------------------------------------------------------------
    // State
    // -------------------------------------------------------------------------

    /// <summary>The table schema key currently being configured (e.g. "absences").</summary>
    public string SchemaKey { get; private set; } = string.Empty;

    /// <summary>In-progress working copy of columns. Mutated by the UI.</summary>
    public List<TableColumnModel> WorkingColumns { get; private set; } = [];

    /// <summary>True when the working copy differs from the original.</summary>
    public bool HasChanges => WorkingColumns.Any(c =>
    {
        var orig = _original.FirstOrDefault(o => o.Key == c.Key);
        return orig is null || orig.Visible != c.Visible || orig.Order != c.Order;
    });

    private List<TableColumnModel> _original = [];

    // -------------------------------------------------------------------------
    // Lifecycle
    // -------------------------------------------------------------------------

    /// <summary>
    /// Initialises the view-model with a schema key and its column list.
    /// Makes a working copy so original state is preserved until Apply() is called.
    /// </summary>
    public void Load(string schemaKey, IEnumerable<TableColumnModel> columns)
    {
        SchemaKey = schemaKey;
        _original = columns.ToList();
        WorkingColumns = _original
            .Select(c => new TableColumnModel
            {
                Key = c.Key,
                Label = c.Label,
                Visible = c.Visible,
                Width = c.Width,
                Align = c.Align,
                Icon = c.Icon,
                Sortable = c.Sortable,
                Filterable = c.Filterable,
                Hideable = c.Hideable,
                Reorderable = c.Reorderable,
                Order = c.Order,
                DataType = c.DataType,
                Format = c.Format,
                HideOnMobile = c.HideOnMobile,
            })
            .OrderBy(c => c.Order)
            .ToList();
    }

    // -------------------------------------------------------------------------
    // Actions
    // -------------------------------------------------------------------------

    /// <summary>Toggles the Visible flag for a column by key.</summary>
    public void ToggleVisibility(string key)
    {
        var col = WorkingColumns.FirstOrDefault(c => c.Key == key);
        if (col is null || !col.Hideable) return;
        col.Visible = !col.Visible;
    }

    /// <summary>Moves a column one position to the left (lower order index).</summary>
    public void MoveLeft(string key)
    {
        var idx = WorkingColumns.FindIndex(c => c.Key == key);
        if (idx <= 0) return;
        (WorkingColumns[idx - 1], WorkingColumns[idx]) = (WorkingColumns[idx], WorkingColumns[idx - 1]);
        RecalculateOrder();
    }

    /// <summary>Moves a column one position to the right (higher order index).</summary>
    public void MoveRight(string key)
    {
        var idx = WorkingColumns.FindIndex(c => c.Key == key);
        if (idx < 0 || idx >= WorkingColumns.Count - 1) return;
        (WorkingColumns[idx + 1], WorkingColumns[idx]) = (WorkingColumns[idx], WorkingColumns[idx + 1]);
        RecalculateOrder();
    }

    /// <summary>
    /// Applies the working copy back to the original and returns the final column list.
    /// Call this when the user confirms settings.
    /// </summary>
    public List<TableColumnModel> Apply()
    {
        _original = WorkingColumns
            .Select(c => new TableColumnModel
            {
                Key = c.Key,
                Label = c.Label,
                Visible = c.Visible,
                Width = c.Width,
                Align = c.Align,
                Icon = c.Icon,
                Sortable = c.Sortable,
                Filterable = c.Filterable,
                Hideable = c.Hideable,
                Reorderable = c.Reorderable,
                Order = c.Order,
                DataType = c.DataType,
                Format = c.Format,
                HideOnMobile = c.HideOnMobile,
            })
            .ToList();

        return _original;
    }

    /// <summary>Discards all working changes and reloads the original state.</summary>
    public void Cancel() => Load(SchemaKey, _original);

    /// <summary>Resets all columns to their default visibility (all visible, original order).</summary>
    public void ResetToDefaults()
    {
        foreach (var col in WorkingColumns)
            col.Visible = true;

        WorkingColumns = WorkingColumns.OrderBy(c => c.Key).ToList();
        RecalculateOrder();
    }

    // -------------------------------------------------------------------------
    // Private helpers
    // -------------------------------------------------------------------------

    private void RecalculateOrder()
    {
        for (int i = 0; i < WorkingColumns.Count; i++)
            WorkingColumns[i].Order = i;
    }
}
