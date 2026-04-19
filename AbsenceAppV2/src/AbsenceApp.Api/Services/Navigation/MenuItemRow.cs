/*
===============================================================================
 File        : MenuItemRow.cs
 Namespace   : AbsenceApp.Api.Services.Navigation
 Author      : Michael
 Version     : 2.0.0
 Created     : 2026-04-06
 Updated     : 2026-04-19
-------------------------------------------------------------------------------
 Purpose     : Projection record for rows returned by the menuitems +
               rolemenuitem JOIN query in MenuResolver.
               Properties match the menuitems table columns exactly.
               Used exclusively by MenuResolver to map raw rows to the
               render-ready menu DTO tree before the API serialises the
               response.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-06  Initial implementation. Column names reflected the
                         TVF output (Category, GroupName, Href, ItemIcon).
   - 2.0.0  2026-04-19  MySQL migration: updated properties to match the actual
                         menuitems table schema (Id, ParentId, ItemType, Route,
                         Icon instead of Href, ItemIcon). Removed TVF reference.
                         Note: MenuResolver uses its own private inner class
                         MenuItemRow; this public class documents the schema.
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
/// Represents one row from the menuitems table, projected by MenuResolver.
/// Column names must match the menuitems table output exactly.
/// </summary>
public sealed class MenuItemRow
{
    // ---------------------------------------------------------------------------
    // Identity / hierarchy
    // ---------------------------------------------------------------------------
    public int     Id          { get; set; }
    public int?    ParentId    { get; set; }
    public string? ItemType    { get; set; }

    // ---------------------------------------------------------------------------
    // Display
    // ---------------------------------------------------------------------------
    public string? Label       { get; set; }
    public string? Icon        { get; set; }
    public string? Route       { get; set; }

    // ---------------------------------------------------------------------------
    // Visibility / ordering
    // ---------------------------------------------------------------------------
    public bool    IsHidden    { get; set; }
    public int     SortOrder   { get; set; }

    // ---------------------------------------------------------------------------
    // Denormalised group / category hints (may be null)
    // ---------------------------------------------------------------------------
    public string? Category    { get; set; }
    public string? GroupName   { get; set; }
    public string? GroupIcon   { get; set; }
    public bool?   IsFlat      { get; set; }

    // ---------------------------------------------------------------------------
    // Metadata
    // ---------------------------------------------------------------------------
    public string? Status      { get; set; }
    public string? Description { get; set; }
}
