/*
===============================================================================
 File        : PagesDtos.cs
 Namespace   : AbsenceApp.Core.DTOs
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-04-11
 Updated     : 2026-04-11
-------------------------------------------------------------------------------
 Purpose     : Data Transfer Objects for the E16 Pages Registry admin module.
               Used by IPagesService / PagesService and the client-side
               PagesApiServiceV2, PagesListViewModelV2, and PageFormViewModelV2.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-11  Initial creation (E16 Pages Registry).
-------------------------------------------------------------------------------
 Notes       :
   - AppPageDto (in UserManagementDtos.cs) is the thin DTO used by the
     permission system (E15). PagesDtos here are richer and include all
     admin-editable fields.
   - PageCreateDto and PageUpdateDto have identical shapes; both are kept
     separate for clarity and future divergence (e.g. UpdateDto has Id).
===============================================================================
*/
namespace AbsenceApp.Core.DTOs;

// ─────────────────────────────────────────────────────────────────────────────
// PageListItemDto — one row in the Pages Registry list table
// ─────────────────────────────────────────────────────────────────────────────

public sealed class PageListItemDto
{
    public int      Id          { get; set; }
    public string   Name        { get; set; } = default!;
    public string   Slug        { get; set; } = default!;
    public string   Route       { get; set; } = default!;
    public string   CategoryKey { get; set; } = default!;
    public string   MenuKey     { get; set; } = default!;
    public string?  IconKey     { get; set; }
    public bool     IsActive    { get; set; }
    public int      SortOrder   { get; set; }
    public DateTime CreatedAt   { get; set; }
    public DateTime UpdatedAt   { get; set; }
}

// ─────────────────────────────────────────────────────────────────────────────
// PageEditDto — full field set returned when opening the edit form
// ─────────────────────────────────────────────────────────────────────────────

public sealed class PageEditDto
{
    public int     Id             { get; set; }
    public string  Name           { get; set; } = default!;
    public string  Slug           { get; set; } = default!;
    public string  Route          { get; set; } = default!;
    public string  CategoryKey    { get; set; } = default!;
    public string  MenuKey        { get; set; } = default!;
    public string? IconKey        { get; set; }
    public string? Description    { get; set; }
    public bool    IsActive       { get; set; }
    public int     SortOrder      { get; set; }
    public bool    SupportsRead   { get; set; }
    public bool    SupportsWrite  { get; set; }
    public bool    SupportsCreate { get; set; }
    public bool    SupportsDelete { get; set; }
    public bool    SupportsImport { get; set; }
    public bool    SupportsExport { get; set; }
}

// ─────────────────────────────────────────────────────────────────────────────
// PageCreateDto — payload for adding a new page to the registry
// ─────────────────────────────────────────────────────────────────────────────

public sealed class PageCreateDto
{
    public string  Name           { get; set; } = default!;
    public string  Slug           { get; set; } = default!;
    public string  Route          { get; set; } = default!;
    public string  CategoryKey    { get; set; } = default!;
    public string  MenuKey        { get; set; } = default!;
    public string? IconKey        { get; set; }
    public string? Description    { get; set; }
    public bool    IsActive       { get; set; } = true;
    public int     SortOrder      { get; set; }
    public bool    SupportsRead   { get; set; }
    public bool    SupportsWrite  { get; set; }
    public bool    SupportsCreate { get; set; }
    public bool    SupportsDelete { get; set; }
    public bool    SupportsImport { get; set; }
    public bool    SupportsExport { get; set; }
}

// ─────────────────────────────────────────────────────────────────────────────
// PageUpdateDto — payload for editing an existing page
// ─────────────────────────────────────────────────────────────────────────────

public sealed class PageUpdateDto
{
    public int     Id             { get; set; }
    public string  Name           { get; set; } = default!;
    public string  Slug           { get; set; } = default!;
    public string  Route          { get; set; } = default!;
    public string  CategoryKey    { get; set; } = default!;
    public string  MenuKey        { get; set; } = default!;
    public string? IconKey        { get; set; }
    public string? Description    { get; set; }
    public bool    IsActive       { get; set; }
    public int     SortOrder      { get; set; }
    public bool    SupportsRead   { get; set; }
    public bool    SupportsWrite  { get; set; }
    public bool    SupportsCreate { get; set; }
    public bool    SupportsDelete { get; set; }
    public bool    SupportsImport { get; set; }
    public bool    SupportsExport { get; set; }
}
