/*
===============================================================================
 File        : MenuDtos.cs
 Namespace   : AbsenceApp.Api.Services.Navigation
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-04-06
 Updated     : 2026-04-06
-------------------------------------------------------------------------------
 Purpose     : Render-ready DTOs serialised by GET /api/menu. The client
               deserialises these and passes them directly to SidebarV2 for
               rendering. No filtering logic is applied on the client.

               Hierarchy:
                 MenuResponseDto
                   └─ MenuCategoryDto[]
                        └─ MenuGroupDto[]
                             └─ MenuItemDto[]
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-06  Initial implementation (Phase 1 — API Menu Boundary).
-------------------------------------------------------------------------------
 Notes       :
   - Property names use camelCase serialisation (System.Text.Json default).
   - All collections are non-null empty lists when no children exist.
===============================================================================
*/

namespace AbsenceApp.Api.Services.Navigation;

// ===========================================================================
// MenuResponseDto
// ===========================================================================

/// <summary>Top-level response envelope for GET /api/menu.</summary>
public sealed class MenuResponseDto
{
    public List<MenuCategoryDto> Categories { get; set; } = [];
}

// ===========================================================================
// MenuCategoryDto
// ===========================================================================

/// <summary>A named section header containing one or more menu groups.</summary>
public sealed class MenuCategoryDto
{
    // ---------------------------------------------------------------------------
    // A null or empty Category means no section header is rendered (root groups).
    // ---------------------------------------------------------------------------
    public string? Category { get; set; }

    public List<MenuGroupDto> Groups { get; set; } = [];
}

// ===========================================================================
// MenuGroupDto
// ===========================================================================

/// <summary>An accordion group or flat navigation link group.</summary>
public sealed class MenuGroupDto
{
    public string  Group   { get; set; } = string.Empty;
    public string  Icon    { get; set; } = string.Empty;
    public bool    Flat    { get; set; }
    public List<MenuItemDto> Items { get; set; } = [];
}

// ===========================================================================
// MenuItemDto
// ===========================================================================

/// <summary>A single navigation link rendered inside a group.</summary>
public sealed class MenuItemDto
{
    public string  Title       { get; set; } = string.Empty;
    public string  Href        { get; set; } = string.Empty;
    public string  Icon        { get; set; } = string.Empty;
    public string  Status      { get; set; } = string.Empty;
    public string? Description { get; set; }
}
