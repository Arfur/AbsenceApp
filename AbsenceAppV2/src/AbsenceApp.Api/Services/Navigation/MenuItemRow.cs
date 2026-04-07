/*
===============================================================================
 File        : MenuItemRow.cs
 Namespace   : AbsenceApp.Api.Services.Navigation
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-04-06
 Updated     : 2026-04-06
-------------------------------------------------------------------------------
 Purpose     : Raw result row returned by dbo.fn_GetVisibleMenuItems(@RoleType).
               Properties must match the column names of the TVF exactly.
               Used exclusively by MenuResolver to map raw rows to the
               render-ready menu DTO tree before the API serialises the
               response.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-06  Initial implementation. Column names reflect the
                         menu structure (Category, Group, GroupIcon, Label,
                         Href, ItemIcon, Status, SortOrder, IsFlat,
                         Description). Must align with the live TVF schema.
-------------------------------------------------------------------------------
 Notes       :
   - Do NOT add navigation properties; this is a projection-only record.
   - All string columns are nullable to tolerate SQL NULLs without throwing.
===============================================================================
*/

namespace AbsenceApp.Api.Services.Navigation;

// ===========================================================================
// MenuItemRow
// ===========================================================================

/// <summary>
/// Represents one row returned by dbo.fn_GetVisibleMenuItems(@RoleType).
/// Column names must match the TVF output exactly.
/// </summary>
public sealed class MenuItemRow
{
    // ---------------------------------------------------------------------------
    // Category level
    // ---------------------------------------------------------------------------
    public string? Category    { get; set; }

    // ---------------------------------------------------------------------------
    // Group level
    // ---------------------------------------------------------------------------
    public string? GroupName   { get; set; }
    public string? GroupIcon   { get; set; }
    public bool    IsFlat      { get; set; } = false;

    // ---------------------------------------------------------------------------
    // Item level
    // ---------------------------------------------------------------------------
    public string? Label       { get; set; }
    public string? Href        { get; set; }
    public string? ItemIcon    { get; set; }
    public string? Status      { get; set; }
    public string? Description { get; set; }

    // ---------------------------------------------------------------------------
    // Ordering
    // ---------------------------------------------------------------------------
    public int     SortOrder   { get; set; }
}
