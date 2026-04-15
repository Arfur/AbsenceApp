/*
===============================================================================
 File        : AppPage.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 1.1.0
 Created     : 2026-04-11
 Updated     : 2026-04-11
-------------------------------------------------------------------------------
 Purpose     : EF Core entity for the app_pages table.
               Represents a named, addressable page in the application that
               can be targeted by the permission system (E15) and managed
               through the Pages Registry admin UI (E16).
               Each row corresponds to a navigable route and is referenced by
               RoleDefaultPagePermission and UserPagePermission FK columns.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-11  Initial creation (E15 User Management).
   - 1.1.0  2026-04-11  E16 Pages Registry: added Slug, CategoryKey, MenuKey,
                         IconKey, Description, the six Supports* capability
                         flags, and CreatedAt / UpdatedAt audit timestamps.
-------------------------------------------------------------------------------
 Notes       :
   - Seeded on first migration via UserManagementModelBuilderExtensions.
   - Route must match the Blazor @page route exactly (e.g. "/v2/users").
   - ValueGeneratedOnAdd is set in configuration; exempt from the
     ValueGeneratedNever loop via AppDbContext exclusion.
   - Slug and Route both have unique indexes. Slug is the human-readable
     URL fragment (e.g. "students"); Route is the full Blazor @page path.
   - CategoryKey and MenuKey drive future sidebar generation (E17+).
   - SupportsXxx flags indicate which permission actions are meaningful for
     this page; used by PermissionMatrixV2 for column filtering (E17+).
===============================================================================
*/
namespace AbsenceApp.Data.Models;

public class AppPage
{
    public int     Id           { get; set; }
    public string  Name         { get; set; } = default!;
    /// <summary>Short URL-safe slug, e.g. "students". Unique.</summary>
    public string  Slug         { get; set; } = string.Empty;
    public string  Route        { get; set; } = default!;
    /// <summary>Top-level grouping key, e.g. "ADMIN", "PEOPLE", "ATTENDANCE".</summary>
    public string  CategoryKey  { get; set; } = string.Empty;
    /// <summary>Sidebar menu group label, e.g. "Staff", "Attendance".</summary>
    public string  MenuKey      { get; set; } = string.Empty;
    /// <summary>Bootstrap Icons class (without bi-), e.g. "bi-speedometer2".</summary>
    public string? IconKey      { get; set; }
    public string? Description  { get; set; }
    public bool    IsActive     { get; set; } = true;
    public int     SortOrder    { get; set; }
    // ── Permission capability flags ──────────────────────────────────────────
    public bool    SupportsRead   { get; set; }
    public bool    SupportsWrite  { get; set; }
    public bool    SupportsCreate { get; set; }
    public bool    SupportsDelete { get; set; }
    public bool    SupportsImport { get; set; }
    public bool    SupportsExport { get; set; }
    // ── Audit timestamps ─────────────────────────────────────────────────────
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
