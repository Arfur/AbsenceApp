/*
===============================================================================
 File        : TableColumnModel.cs
 Namespace   : AbsenceApp.Client.Models.TableV2
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-21
 Updated     : 2026-03-21
-------------------------------------------------------------------------------
 Purpose     : Data model describing a single column in TableV2. Carries column key, display label, sort/filter eligibility, width hint, visibility flag, and display order index.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-21  Initial implementation.
-------------------------------------------------------------------------------
 Notes       :
   - Phase 4 model. No DI registration required.
===============================================================================
*/

namespace AbsenceApp.Client.Models.TableV2;

/// <summary>Defines visibility, alignment, and behaviour of a single table column.</summary>
public sealed class TableColumnModel
{
    // -------------------------------------------------------------------------
    // Identity
    // -------------------------------------------------------------------------
    /// <summary>Unique column key matching JSON data field name.</summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>Display header label.</summary>
    public string Label { get; set; } = string.Empty;

    // -------------------------------------------------------------------------

    // Display

    // -------------------------------------------------------------------------
    /// <summary>Whether the column is currently shown.</summary>
    public bool Visible { get; set; } = true;

    /// <summary>Optional fixed or min-width (CSS value, e.g. "8rem").</summary>
    public string? Width { get; set; }

    /// <summary>Text alignment: "left" | "center" | "right". Default: "left".</summary>
    public string Align { get; set; } = "left";

    /// <summary>Bootstrap icon class for the column header (optional).</summary>
    public string? Icon { get; set; }

    // -------------------------------------------------------------------------

    // Behaviour

    // -------------------------------------------------------------------------
    /// <summary>Whether this column can be sorted.</summary>
    public bool Sortable { get; set; } = true;

    /// <summary>Whether this column can be filtered via the filter row.</summary>
    public bool Filterable { get; set; } = true;

    /// <summary>Whether this column can be hidden by the user in Table Settings.</summary>
    public bool Hideable { get; set; } = true;

    /// <summary>Whether this column can be reordered by the user.</summary>
    public bool Reorderable { get; set; } = true;

    /// <summary>Display order index (lower = left). User-adjustable.</summary>
    public int Order { get; set; }

    // -------------------------------------------------------------------------

    // Data type

    // -------------------------------------------------------------------------
    /// <summary>
    /// Column data type hint for formatting/filtering:
    /// "string" | "number" | "date" | "datetime" | "bool" | "badge" | "actions".
    /// Default: "string".
    /// </summary>
    public string DataType { get; set; } = "string";

    /// <summary>Optional C# format string (e.g. "dd MMM yyyy" for dates).</summary>
    public string? Format { get; set; }

    // -------------------------------------------------------------------------

    // Responsive

    // -------------------------------------------------------------------------
    /// <summary>Hide on small screens? Columns marked true collapse below 900px.</summary>
    public bool HideOnMobile { get; set; } = false;
}
