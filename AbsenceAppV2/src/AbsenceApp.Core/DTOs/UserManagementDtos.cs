/*
===============================================================================
 File        : UserManagementDtos.cs
 Namespace   : AbsenceApp.Core.DTOs
 Author      : Michael
 Version     : 1.1.0
 Created     : 2026-04-11
 Updated     : 2026-04-11
-------------------------------------------------------------------------------
 Purpose     : Data Transfer Objects for the E15 User Management module.
               Covers list display, create/update forms, page permission
               editing, and the effective-permissions resolved view.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-11  Initial creation (E15 User Management).
   - 1.1.0  2026-04-11  E16 Pages Registry: extended AppPageDto with Slug,
                         CategoryKey, MenuKey, IconKey, Description, and the
                         six SupportsXxx capability flags. These are available
                         for future PermissionMatrixV2 column filtering (E17+).
-------------------------------------------------------------------------------
 Notes       :
   - No EF / DbContext references. Pure POCO DTOs for cross-layer contracts.
   - AppPageDto is also used as the row items in the PermissionMatrixV2 UI
     component.
===============================================================================
*/
namespace AbsenceApp.Core.DTOs;

// ─────────────────────────────────────────────────────────────────────────────
// AppPageDto — descriptor for a single page in the permission matrix
// ─────────────────────────────────────────────────────────────────────────────

public sealed class AppPageDto
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
    // ── Permission capability flags (informational for PermissionMatrixV2) ───
    public bool    SupportsRead   { get; set; }
    public bool    SupportsWrite  { get; set; }
    public bool    SupportsCreate { get; set; }
    public bool    SupportsDelete { get; set; }
    public bool    SupportsImport { get; set; }
    public bool    SupportsExport { get; set; }
}

// ─────────────────────────────────────────────────────────────────────────────
// UserListItemDto — row in the Users list table
// ─────────────────────────────────────────────────────────────────────────────

public sealed class UserListItemDto
{
    public long   Id           { get; set; }
    public string FullName     { get; set; } = default!;
    public string Username     { get; set; } = default!;
    public string Email        { get; set; } = default!;
    public string? PhoneNumber { get; set; }
    public string RoleTypeName { get; set; } = default!;
    public string Status       { get; set; } = default!;
    public DateTime CreatedAt  { get; set; }
}

// ─────────────────────────────────────────────────────────────────────────────
// UserCreateDto — payload for creating a new user account
// ─────────────────────────────────────────────────────────────────────────────

public sealed class UserCreateDto
{
    public string  FirstName    { get; set; } = default!;
    public string  LastName     { get; set; } = default!;
    public string  Username     { get; set; } = default!;
    public string  Email        { get; set; } = default!;
    public string? PhoneNumber  { get; set; }
    /// <summary>Plain-text password; hashed by the service layer before storage.</summary>
    public string  Password     { get; set; } = default!;
    public long    RoleTypeId   { get; set; }
}

// ─────────────────────────────────────────────────────────────────────────────
// UserUpdateDto — payload for editing an existing user account
// ─────────────────────────────────────────────────────────────────────────────

public sealed class UserUpdateDto
{
    public long    Id           { get; set; }
    public string  FirstName    { get; set; } = default!;
    public string  LastName     { get; set; } = default!;
    public string  Username     { get; set; } = default!;
    public string  Email        { get; set; } = default!;
    public string? PhoneNumber  { get; set; }
    /// <summary>If null or empty, the password is NOT changed.</summary>
    public string? NewPassword  { get; set; }
    public long    RoleTypeId   { get; set; }
    public string  Status       { get; set; } = default!;
}

// ─────────────────────────────────────────────────────────────────────────────
// PagePermissionDto — one row in the permission matrix for a user × page
// ─────────────────────────────────────────────────────────────────────────────

public sealed class PagePermissionDto
{
    public int    PageId    { get; set; }
    public string PageName  { get; set; } = default!;
    public string PageRoute { get; set; } = default!;

    /// <summary>True when a user-specific row exists for this page.</summary>
    public bool HasOverride  { get; set; }

    public bool CanRead   { get; set; }
    public bool CanWrite  { get; set; }
    public bool CanCreate { get; set; }
    public bool CanDelete { get; set; }
    public bool CanImport { get; set; }
    public bool CanExport { get; set; }
}

// ─────────────────────────────────────────────────────────────────────────────
// EffectivePermissionDto — resolved CRUD flags for one user × page combination
// (output of PermissionServiceV2; not stored in DB)
// ─────────────────────────────────────────────────────────────────────────────

public sealed class EffectivePermissionDto
{
    public string PageRoute { get; set; } = default!;
    public bool   CanRead   { get; set; }
    public bool   CanWrite  { get; set; }
    public bool   CanCreate { get; set; }
    public bool   CanDelete { get; set; }
    public bool   CanImport { get; set; }
    public bool   CanExport { get; set; }
}

// ─────────────────────────────────────────────────────────────────────────────
// RoleTypeSelectDto — lightweight item for the Role dropdown in the user form
// ─────────────────────────────────────────────────────────────────────────────

public sealed class RoleTypeSelectDto
{
    public long   Id          { get; set; }
    public string Name        { get; set; } = default!;
    public string DisplayName { get; set; } = default!;
}
