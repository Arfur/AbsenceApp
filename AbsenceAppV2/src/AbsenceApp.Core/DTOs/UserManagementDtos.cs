/*
===============================================================================
 File        : UserManagementDtos.cs
 Namespace   : AbsenceApp.Core.DTOs
 Author      : Michael
 Version     : 1.3.0
 Created     : 2026-04-11
 Updated     : 2026-04-21
-------------------------------------------------------------------------------
 Purpose     : Data Transfer Objects for the E15 User Management module.
               Covers list display, create/update forms, page permission
               editing, effective-permissions resolved view, the three
               new list-page DTOs for Roles, Permissions, and Page Access,
               and the full User Profile page DTOs (header, detail, contacts,
               classes, devices, external systems, absences, login audit,
               change-password).
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-11  Initial creation (E15 User Management).
   - 1.1.0  2026-04-11  E16 Pages Registry: extended AppPageDto with Slug,
                         CategoryKey, MenuKey, IconKey, Description, and the
                         six SupportsXxx capability flags.
   - 1.2.0  2026-04-21  Added RoleListItemDto (Roles page), FeatureListItemDto
                         (Permissions page), and PageAccessRowDto (Page Access
                         page) to support live data on the three list pages.
   - 1.3.0  2026-04-21  Added full User Profile page DTOs: UserProfileHeaderDto,
                         UserProfileDetailDto, StaffContactDto, StaffClassRowDto,
                         StaffDeviceRowDto, StaffExternalRowDto, StaffAbsenceRowDto,
                         LoginAuditRowDto, UserProfileSaveDto, ChangePasswordDto.
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
    /// <summary>FK to staff.Id. Never null for valid users.</summary>
    public long?  StaffId      { get; set; }
    public string StaffName    { get; set; } = default!;
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
    /// <summary>Mandatory FK → staff.Id. User creation must originate from a Staff record.</summary>
    public long    StaffId      { get; set; }
    public string  Username     { get; set; } = default!;
    public string  Email        { get; set; } = default!;
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
    /// <summary>Read-only after creation — StaffId MUST NOT be changed on update.</summary>
    public long?   StaffId      { get; set; }
    public string  Username     { get; set; } = default!;
    public string  Email        { get; set; } = default!;
    /// <summary>If null or empty, the password is NOT changed.</summary>
    public string? NewPassword  { get; set; }
    public long    RoleTypeId   { get; set; }
    public string  Status       { get; set; } = default!;
}

// ─────────────────────────────────────────────────────────────────────────────
// StaffSelectDto — lightweight Staff record used when launching the Create User
// form from the Staff detail page. Carries the pre-linked Staff context.
// ─────────────────────────────────────────────────────────────────────────────

public sealed class StaffSelectDto
{
    public long   Id          { get; set; }
    public string FullName    { get; set; } = default!;
    public string StaffNumber { get; set; } = default!;
    public string WorkEmail   { get; set; } = default!;
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

// ─────────────────────────────────────────────────────────────────────────────
// RoleListItemDto — row in the Roles list page
// ─────────────────────────────────────────────────────────────────────────────

public sealed class RoleListItemDto
{
    public long   Id          { get; set; }
    public string Name        { get; set; } = default!;
    public string DisplayName { get; set; } = default!;
    public string Description { get; set; } = string.Empty;
    public int    UserCount   { get; set; }
}

// ─────────────────────────────────────────────────────────────────────────────
// FeatureListItemDto — row in the Permissions list page
// ─────────────────────────────────────────────────────────────────────────────

public sealed class FeatureListItemDto
{
    public long   Id          { get; set; }
    public string Code        { get; set; } = default!;
    public string Description { get; set; } = string.Empty;
    /// <summary>Comma-joined display names of role types that have this feature enabled.</summary>
    public string Roles       { get; set; } = string.Empty;
}

// ─────────────────────────────────────────────────────────────────────────────
// PageAccessRowDto — row in the Page Access list page
// ─────────────────────────────────────────────────────────────────────────────

public sealed class PageAccessRowDto
{
    public int    Id       { get; set; }
    public string PageName { get; set; } = default!;
    public string Route    { get; set; } = default!;
    public string Category { get; set; } = string.Empty;
    /// <summary>Comma-joined role type names that have CanRead=true for this page.</summary>
    public string Roles    { get; set; } = string.Empty;
}

// ─────────────────────────────────────────────────────────────────────────────
// User Profile page DTOs  (v1.3.0)
// ─────────────────────────────────────────────────────────────────────────────

/// <summary>
/// Drives the profile header strip (photo, name, role, status badges, last-login).
/// </summary>
public sealed class UserProfileHeaderDto
{
    public long      UserId            { get; set; }
    public long?     StaffId           { get; set; }
    public string    Username          { get; set; } = string.Empty;
    public string    FullName          { get; set; } = string.Empty;
    public string    Email             { get; set; } = string.Empty;
    public string    RoleName          { get; set; } = string.Empty;
    public string    Status            { get; set; } = string.Empty;
    public bool      IsAdmin           { get; set; }
    public string?   ProfilePictureUrl { get; set; }
    public DateTime? LastLoginAt       { get; set; }
    public DateTime  CreatedAt         { get; set; }
}

/// <summary>
/// Extended profile fields from the user_profiles table (Tab 0 form).
/// </summary>
public sealed class UserProfileDetailDto
{
    public long     ProfileId       { get; set; }
    public bool     ProfileExists   { get; set; }
    public string   FirstName       { get; set; } = string.Empty;
    public string   LastName        { get; set; } = string.Empty;
    public string?  PreferredName   { get; set; }
    public string   Title           { get; set; } = string.Empty;
    public DateOnly DateOfBirth     { get; set; }
    public string?  Bio             { get; set; }
    public string?  Gender          { get; set; }
    public string   Timezone        { get; set; } = "UTC";
    public string   LanguageCode    { get; set; } = "en";
    public long     DepartmentId    { get; set; }
    public long     JobTitleId      { get; set; }
    public string?  ProfilePictureUrl { get; set; }
}

/// <summary>
/// Staff employment and contact data (Tab 1: Contacts).
/// </summary>
public sealed class StaffContactDto
{
    public string    WorkEmail       { get; set; } = string.Empty;
    public string?   AltEmail        { get; set; }
    public string?   PhoneHome       { get; set; }
    public string?   PhoneMobile     { get; set; }
    public string?   PhoneEmergency  { get; set; }
    public string    WorkLocation    { get; set; } = string.Empty;
    public string    EmploymentType  { get; set; } = string.Empty;
    public string    ContractType    { get; set; } = string.Empty;
    public DateOnly  HireDate        { get; set; }
    public DateOnly? EndDate         { get; set; }
}

/// <summary>
/// One row for the Classes &amp; Year Groups tab (Tab 2).
/// </summary>
public sealed class StaffClassRowDto
{
    public long     AssignmentId    { get; set; }
    public string   ClassName       { get; set; } = string.Empty;
    public string   JobTitle        { get; set; } = string.Empty;
    public string   Department      { get; set; } = string.Empty;
    public string?  Responsibility  { get; set; }
    public DateOnly StartDate       { get; set; }
    public DateOnly? EndDate        { get; set; }
    public string?  DaysOfWeek      { get; set; }
}

/// <summary>
/// One row for the Devices tab (Tab 3).
/// </summary>
public sealed class StaffDeviceRowDto
{
    public long     Id              { get; set; }
    public string   DeviceType      { get; set; } = string.Empty;
    public string   SerialNumber    { get; set; } = string.Empty;
    public DateOnly AssignedDate    { get; set; }
    public DateOnly? ReturnedDate   { get; set; }
}

/// <summary>
/// One row for the External Systems tab (Tab 4).
/// </summary>
public sealed class StaffExternalRowDto
{
    public long   Id              { get; set; }
    public string SystemName      { get; set; } = string.Empty;
    public string SystemCode      { get; set; } = string.Empty;
    public string AccountUsername { get; set; } = string.Empty;
    public string AccountEmail    { get; set; } = string.Empty;
    public string Status          { get; set; } = string.Empty;
}

/// <summary>
/// One row for the Absence Records tab (Tab 6).
/// </summary>
public sealed class StaffAbsenceRowDto
{
    public long     Id            { get; set; }
    public string   AbsenceType   { get; set; } = string.Empty;
    public string   Status        { get; set; } = string.Empty;
    public DateOnly StartDate     { get; set; }
    public DateOnly EndDate       { get; set; }
    public int      DurationDays  { get; set; }
    public string   ReportedVia   { get; set; } = string.Empty;
    public string?  Notes         { get; set; }
    public DateTime? ApprovedAt   { get; set; }
}

/// <summary>
/// One row for the Login Audit tab (Tab 7).
/// </summary>
public sealed class LoginAuditRowDto
{
    public long     Id          { get; set; }
    public DateTime LoginTime   { get; set; }
    public string   IpAddress   { get; set; } = string.Empty;
    public string   UserAgent   { get; set; } = string.Empty;
    public bool     Success     { get; set; }
}

/// <summary>
/// Payload to save the full user + profile detail (Basic User Info tab save).
/// </summary>
public sealed class UserProfileSaveDto
{
    // User record
    public long    UserId        { get; set; }
    public string  Username      { get; set; } = string.Empty;
    public string  Email         { get; set; } = string.Empty;
    public long    RoleTypeId    { get; set; }
    public string  Status        { get; set; } = string.Empty;
    // UserProfile record (upserted)
    public string  FirstName     { get; set; } = string.Empty;
    public string  LastName      { get; set; } = string.Empty;
    public string? PreferredName { get; set; }
    public string  Title         { get; set; } = string.Empty;
    public DateOnly DateOfBirth  { get; set; }
    public string? Bio           { get; set; }
    public string? Gender        { get; set; }
    public string  Timezone      { get; set; } = "UTC";
    public string  LanguageCode  { get; set; } = "en";
    public long    DepartmentId  { get; set; }
    public long    JobTitleId    { get; set; }
}

/// <summary>
/// Payload to change a user's password (requires old password verification).
/// </summary>
public sealed class ChangePasswordDto
{
    public long   UserId      { get; set; }
    public string OldPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}
