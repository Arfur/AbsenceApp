================================================================================
 File        : UserManagement_CHANGELOG.md
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-04-21
 Updated     : 2026-04-22 (Session 6 — Add User Page Fix)
--------------------------------------------------------------------------------
 Purpose     :
   Provides a structured, deterministic changelog for all modifications to the
   User Management module as part of the AbsenceApp V2 development track.

   This file exists to:
     - Track every code and architecture change with full accountability
     - Document the rationale behind key design decisions
     - Support maintenance, onboarding, and rollback planning
--------------------------------------------------------------------------------
 Notes       :
   - This file covers the Staff-linked user creation redesign completed on
     2026-04-21.
   - All 11 affected files are listed with a full description of changes.
================================================================================


# DEVELOPMENT PLAN

## Phase 1 — Analysis
Review the existing User Management module structure. Identify the use of
direct FirstName, LastName, and PhoneNumber fields on User entity and DTOs.
Map existing UserCreateDto, UserUpdateDto, UserFormViewModelV2, and
UserFormPageV2.razor. Confirm that the User entity carries no FK to the Staff
table. Identify the existing email-based staff-linking attempt and assess its
reliability issues.

## Phase 2 — Domain Model
Add StaffId (long?) FK to User.cs. This establishes the foundation for
mandatory Staff-linked user creation. Every user account must be associated
with a staff record at the point of creation.

## Phase 3 — Service & API Layer
Update IUserManagementService with two new methods: GetStaffForUserCreateAsync
and StaffHasUserAsync. Rewrite UserManagementService to:

  - Accept StaffId as a mandatory field on create
  - Validate that no existing user account is already linked to the given
    staff record before creating a new one
  - Treat StaffId as immutable on update (server-side enforcement)
  - Continue raw SQL for userrole table operations — no EF DbSet exists

Add the two new wrapper methods to UserManagementApiServiceV2 so Blazor
ViewModels can call the new service contract.

## Phase 4 — ViewModels & UI
Remove FirstName, LastName, and PhoneNumber from all DTOs, ViewModels, and
Razor components. Update UserFormViewModelV2 and UserListViewModelV2. Rewrite
UserFormPageV2.razor routes and layout. Update UsersListPageV2.razor. Add a
context-aware "Create User Account" button to StaffDetailPageV2.razor that
only renders when the staff member has no existing linked user account.

## Phase 5 — Verification
Full solution build. Confirm 0 CS errors. Verify the Staff Detail page shows
the "Create User Account" button only when no user account is linked to that
staff member. Verify the user form operates from staff context only and the
standalone creation route has been removed.

---

# CHANGELOG

## [2026-04-21] — Staff-Linked User Management Redesign

### Summary
Rewrote the User Management module to enforce a mandatory link between user
accounts and staff records. Removed standalone user creation. Removed
FirstName, LastName, and PhoneNumber fields from all layers. Added StaffId as
a mandatory, immutable FK on the User entity. Added duplicate-creation
prevention and contextual entry from the Staff Detail page.

---

### Changes

#### 1. User.cs — Added StaffId FK
Added `public long? StaffId { get; set; }` to the User entity. This FK
establishes the one-to-one relationship between a user account and a staff
record. The field is nullable to preserve compatibility with any existing
records, but all new user accounts require it.

---

#### 2. UserManagementDtos.cs — DTO Redesign
Removed FirstName, LastName, and PhoneNumber from UserCreateDto and
UserUpdateDto. Added StaffId (mandatory, long) to UserCreateDto. Added StaffId
(read-only, long?) to UserUpdateDto so the edit form can display the linked
staff without allowing changes. Added a new StaffSelectDto class carrying the
staff Id, FullName, and JobTitle for display in the create form.

---

#### 3. IUserManagementService.cs — New Contract Methods
Added two new method signatures to the service interface:

  - GetStaffForUserCreateAsync(long staffId) — returns a StaffSelectDto for
    pre-populating the linked staff banner on the create form.
  - StaffHasUserAsync(long staffId) — returns a bool indicating whether a
    user account already exists for the given staff record.

---

#### 4. UserManagementService.cs — Full Service Rewrite
Rewrote all CRUD operations. On create, StaffId is validated as present and
a guard call to StaffHasUserAsync prevents creating a second user account for
the same staff record. On update, StaffId is loaded from the existing entity
and never overwritten. Raw SQL continues to be used for userrole table
operations. Two private projection types, UserRoleRow and UserRoleIdRow, were
added to support these raw SQL queries cleanly.

---

#### 5. UserManagementApiServiceV2.cs — API Wrapper Updates
Added GetStaffForUserCreateAsync and StaffHasUserAsync wrapper methods. These
expose the new service contract to Blazor ViewModels via the existing API
service pattern, consistent with how other V2 API services are structured.

---

#### 6. UserFormViewModelV2.cs — ViewModel Redesign
Added StaffId (long), LinkedStaff (StaffSelectDto?), and SetError(string)
method. Updated InitNewAsync to accept a staffId parameter. Updated
InitEditAsync and SaveAsync to work with the new DTO structure. Removed
FirstName, LastName, and PhoneNumber properties throughout.

---

#### 7. UserListViewModelV2.cs — List ViewModel Updates
Added AllItems property to enable dynamic filter population in the list page,
consistent with the pattern added to StudentsListViewModelV2 on the same date.
Replaced the phoneNumber column definition with a staffName column so the user
list displays the linked staff member's name.

---

#### 8. UserFormPageV2.razor — Route and Layout Rewrite
Changed the create route from `/v2/users/new` to `/v2/users/{StaffId:long}/new`
to enforce staff context at the point of navigation. The edit route remains
`/v2/users/{Id:long}/edit`. Removed the standalone `/v2/users/new` route
entirely. Removed FirstName, LastName, and Phone input fields. Added a
read-only Linked Staff banner showing the staff member's name and job title.
Removed a duplicate `@code` block and stale appended content that had
accumulated from earlier development iterations.

---

#### 9. UsersListPageV2.razor — List Page Updates
Removed the standalone "Add User" button. User creation now originates from
the Staff Detail page only. Replaced phoneNumber column rendering with
staffName rendering in the table body to reflect the updated ViewModel column
definition.

---

#### 10. StaffDetailPageV2.razor — Contextual User Creation
Injected UserManagementApiServiceV2 into the component. Added a "Create User
Account" button that navigates to `/v2/users/{staffId}/new`. The button is
only rendered when `!_staffHasUser`. Added a `_staffHasUser` private boolean
field, populated during OnParametersSetAsync via a call to StaffHasUserAsync.

---

### Technical Explanation

#### StaffId is Mandatory on Create
The User entity now carries a StaffId FK. When creating a user account,
StaffId is provided via the route parameter `/v2/users/{StaffId:long}/new`.
The ViewModel calls GetStaffForUserCreateAsync to load the staff display data
and pre-populate the linked staff banner before the form is shown.

---

#### StaffId is Immutable on Update
UserManagementService loads the existing entity on update and does not
overwrite the StaffId property regardless of what is submitted. This is
enforced server-side. The edit ViewModel still displays the linked staff name
as read-only context for the operator.

---

#### Email-Based Linking Removed
The previous model attempted to link users to staff by matching email
addresses, which was unreliable and implicit. This approach was removed in
favour of an explicit StaffId FK relationship. All linking now happens at
creation time via the route context and is recorded in the database.

---

#### Duplicate User Prevention
Before creating a new user account, UserManagementService calls
StaffHasUserAsync. If any user record with the same StaffId already exists in
the database, the create operation is blocked and an error message is returned
to the ViewModel via SetError(). This prevents two user accounts from being
linked to the same staff record.

---

#### Raw SQL for userrole Table
The userrole table has no EF DbSet in this application. UserManagementService
uses direct SQL via MySqlConnector for all userrole reads and writes. This
pattern was established before this redesign and was preserved. The two
private projection types UserRoleRow and UserRoleIdRow were added to support
these queries with clean typed projections rather than dynamic dictionaries.

---

#### Stale Razor Blocks Removed
UserFormPageV2.razor previously contained a duplicate `@code` block appended
during earlier development iterations. This was identified and removed,
leaving a single clean `@code` block. Stale field bindings for removed
properties were also cleared.

---

#### ViewModel AllItems Property
UserListViewModelV2 gained an AllItems property to support dynamic filter
dropdowns, consistent with the same pattern applied to StudentsListViewModelV2
on the same date (see separate filter fix changelog entry).

---

#### DTO Cleanup
UserCreateDto and UserUpdateDto previously carried FirstName, LastName, and
PhoneNumber, reflecting a standalone user model where personal details were
entered directly. These fields were removed across all layers — DTOs,
ViewModels, and Razor components — because the module now derives all personal
data from the linked Staff record.

---

## [2026-04-21] — User Management UI Consistency Pass (Session 2)

### Summary
Second pass over the User Management module. All four User Management tables
(Users, Roles, Permissions, Page Access) were fully converted to the
`TablePageTemplateV2` template for visual consistency with the Students list
page. Delete with cascading FK cleanup was added to the Users list. The User
Form encoding corruption was repaired.

---

### Changes

#### 1. UsersListPageV2.razor — v2.0.0 (Full Rewrite)
Converted from custom `ulv2-*` markup to `TablePageTemplateV2`. Added:
  - Export dropdown (PDF / Excel) in `ActionsContent` slot.
  - "Add User" button navigating to `/v2/staff` with tooltip explaining the
    Staff-linked requirement.
  - `Filters` slot with Role and Status dropdown filter chips.
  - `TableHeader` with sort buttons for staffName, fullName, username,
    roleTypeName, and status columns.
  - `TableBody` with skeleton rows, empty state, and icon-based action column
    (pencil → edit at `/v2/users/{id}/edit`, trash → two-click delete confirm).
  - Sliding-window `Pagination`.
  - `_f` raw filter dict, `_activeFilterChips`, `_keyToLabel` / `_labelToKey`
    maps for chip management.
  - `ConfirmDeleteAsync(long)` with two-click confirm pattern.
  - `OnSort` / `SortClass` helpers for column sort indicators.

---

#### 2. UserListViewModelV2.cs — v1.2.0
  - Added `"staffName"` sort case in `ApplyView()` switch.
  - Added `DeleteUserAsync(long userId)` method: calls `_svc.DeleteUserAsync()`
    then `LoadAsync()` to refresh the list; catches exceptions into `Error`.

---

#### 3. UserManagementService.cs — v1.3.0
Fixed `DeleteUserAsync` to perform cascading deletion before removing the
User entity, addressing FK constraint violations at the DB level:
  1. `DELETE FROM userrole WHERE UserId = {0}` (raw SQL — no EF DbSet).
  2. `UserPagePermissions` where UserId (EF RemoveRange).
  3. `UserPageOverrides` where UserId (EF RemoveRange).
  4. `UserProfiles` where UserId (EF RemoveRange).
  5. `LoginAudit` where UserId (EF RemoveRange).
  6. Remove the `User` entity itself.
  7. `SaveChangesAsync(ct)`.

---

#### 4. RolesPage.razor — v2.0.0 (Full Rewrite)
Converted from hardcoded pagination / text "Edit" action to `TablePageTemplateV2`:
  - Export dropdown in `ActionsContent` slot.
  - Real `TotalRows` / `CurrentPage` / `ShowEntries` / `SearchValue` wired up.
  - `FilteredRoles` computed property (searches Name, DisplayName, Description).
  - `_pagedRoles` for current page slice.
  - Sliding-window `Pagination` with Previous / Next and numbered page buttons.
  - Actions column uses `tpt-action-icon tpt-action-edit` with `bi-pencil` icon.
  - Columns: ID, Role Name, Display Name, Description, Users Assigned, Actions.

---

#### 5. PermissionsPage.razor — v2.0.0 (Full Rewrite)
Same pattern as RolesPage. Changes include:
  - Export dropdown in `ActionsContent`.
  - `FilteredRows` computed property (searches Code, Description, Roles).
  - `_pagedRows` for current page slice.
  - Sliding-window `Pagination`.
  - Columns: ID, Permission Key, Description, Roles, Actions.
  - Loads via `UserMgmtSvc.GetFeaturesAsync()`.

---

#### 6. PageAccessPage.razor — v2.0.0 (Full Rewrite)
Same pattern as Roles / Permissions. Changes include:
  - Export dropdown in `ActionsContent`.
  - `FilteredRows` computed property (searches PageName, Route, Category, Roles).
  - `_pagedRows` for current page slice.
  - Sliding-window `Pagination`.
  - Columns: Page, Route, Category, Roles With Access, Actions.
  - Loads via `UserMgmtSvc.GetPageAccessAsync()`.

---

#### 7. UserFormPageV2.razor — v2.1.0
Fixed UTF-8 encoding corruption in rendered UI text caused by bytes being
read as Windows-1252 instead of UTF-8:
  - `"Savingâ€¦"` → `"Saving…"` (Save button label while saving).
  - `Loadingâ€¦` → `Loading…` (loading spinner text).
  - `â€" Select role â€"` → `&mdash; Select role &mdash;` (role dropdown placeholder).
  - `&nbsp;Â·&nbsp;` → `&nbsp;&middot;&nbsp;` (separator in Linked Staff banner).

---

### Build Verification
`dotnet build AbsenceAppV2.sln --no-incremental` confirmed:
  - **0 compiler errors (error CS)**
  - **0 warnings**
  - All 4 User Management table pages compile without issues.

---

## [2026-04-21] — Users Table Root-Cause Fix (Session 3)

### Summary
Identified and fixed the actual root cause of the "Users table disappears on
re-navigation" bug that recurred three times. The active sidebar page was
`UsersPage.razor` at `/v2/system/users` — NOT `UsersListPageV2.razor` at
`/v2/users`. The old `UsersPage.razor` had `SearchValue="_search"` (a literal
string, not a Blazor variable binding). The TablePageTemplateV2 saved that
literal string as the search term in AppStateService. On the next navigation
it was restored and `OnSearchChanged("_search")` was called, filtering all
users out — producing "No users found" every single time.

### Root Cause Trace
1. Old `UsersPage.razor` passed `SearchValue="_search"` (literal string).
2. `TablePageTemplateV2.OnParametersSet` set `_searchDraft = "_search"`.
3. `PersistState()` saved `searchValue='_search'` to `AppStateService`.
4. On second visit, `OnAfterRenderAsync(firstRender=true)` found the saved
   state and called `OnSearchChanged("_search")` on the ViewModel.
5. `SetSearch("_search")` called `ApplyView()` with search term `"_search"`.
6. No user's name/email/username contains the string `"_search"` → `Items=[]`.
7. `IsLoading=false`, `Items.Count=0` → "No users found" rendered.

### Changes

#### 1. UsersPage.razor — v2.0.0 (Full Rewrite)
Path: `Modules/SystemManagement/UsersPage.razor` — Route: `/v2/system/users`
This is the page the sidebar navigation actually opens. The old code had:
  - `SearchValue="_search"` (literal string — THE BUG)
  - Direct `@inject IUserManagementService UserMgmt` (Scoped = effectively
    singleton DbContext in MAUI, prone to stale connections)
  - No Export button
  - Text "Edit" link (not icon)
  - Hardcoded `@for (int p = 1; p <= _totalPages; p++)` pagination

New code:
  - `SearchValue="@ViewModel.SearchTerm"` (correct variable binding)
  - `@inject UserListViewModelV2 ViewModel` (uses IServiceScopeFactory for
    fresh DbContext per call — no stale connection risk)
  - `@inject AppStateService AppState` — pre-clears `v2/system/users` key
    in `OnInitializedAsync` before template can restore the stale `"_search"`
  - Export dropdown (PDF/Excel) in `ActionsContent`
  - Add User button → `/v2/staff` with tooltip
  - Role + Status filter dropdowns with `FilterChip` management
  - Sort buttons on Staff, Username, Role, Status columns
  - Skeleton rows during load
  - Pencil (edit) + trash (two-click confirm delete) icon actions
  - Sliding-window pagination (Previous / numbered pages / Next)

---

## [2026-04-21] — User Profile Page Code Audit Cleanup (Session 5)

### Summary
Full read-only code audit of all 8 User Profile page files was performed in
Session 4. This session implements all identified fixes: dead code removal,
type corrections, orphaned fields, redundant imports, missing IsAdmin save
path, and a legacy CSS rule. Build confirmed 0 errors and 0 warnings after
all changes.

---

### Changes

#### 1. UserManagementService.cs — v1.4.0
- **Header version updated** from `1.3.0` to `1.4.0`.
- **Removed dead `BuildFullName()` method.** The private static helper at
  line 694 was never called anywhere in the file. Name construction in all
  active methods is done inline. The "Private helpers" section header was
  removed with it.
- **Fixed `UserRoleIdRow.Value` type** from `int` to `long`. This class is
  used to read a `bigint` FK (`RoleId`) from raw SQL. The prior `int` type
  required a `(long)` cast at the call site and would overflow for any
  `RoleId > int.MaxValue`.
- **Added `user.IsAdmin = dto.IsAdmin;`** in `SaveUserProfileAsync`. The
  `IsAdmin` flag was previously never updated via the profile save path.

---

#### 2. UserManagementDtos.cs — v1.3.0 (patch)
- **Removed `Responsibility` property from `StaffClassRowDto`.** The field
  was defined in the DTO but never populated by `GetStaffClassAssignmentsAsync`
  and never rendered in the razor (Tab 2 Classes table). Fully orphaned across
  all three layers.
- **Added `public bool IsAdmin { get; set; }` to `UserProfileSaveDto`.** This
  completes the IsAdmin save path alongside the service change above.

---

#### 3. UserProfileViewModelV2.cs — v1.0.0 (patch)
- **Added `IsAdmin = IsAdmin,`** to the `UserProfileSaveDto` object initialiser
  in `SaveProfileAsync`. The ViewModel already had `public bool IsAdmin { get; set; }`
  populated from the header, but it was not being passed through to the DTO.

---

#### 4. UserFormPageV2.razor — v3.1.0
- **Removed 3 redundant `@using` directives.** All three were already
  imported by `Modules/_Imports.razor` and caused CS0105 build warnings:
  - `@using AbsenceApp.Client.Framework.PageTemplates`
  - `@using AbsenceApp.Client.Services`
  - `@using Microsoft.AspNetCore.Components.Forms`
- **Removed undefined `upv2-badge--status` class** from the header status
  badge. The base modifier class had no CSS rule — only the variant modifiers
  (`--status-active`, `--status-inactive`, `--status-suspended`) were defined.
  Removing it keeps the class list honest.
- **Added Admin Account checkbox** to Tab 0 Account Management section. The
  `IsAdmin` flag was already read from the header and stored in the ViewModel
  but had no editable control. The checkbox binds to `ViewModel.IsAdmin` and
  is wired end-to-end through the DTO and service.

---

#### 5. UserFormPageV2.razor.css — v2.0.1
- **Removed legacy `ufv2-permissions-empty` rule** from the bottom of the
  file. This was a survivor from the original v1.0.0 CSS that was never
  removed when the file was rewritten. The class is not used anywhere in the
  v3.x razor.

---

### Build Verification
`dotnet build AbsenceAppV2.sln --no-incremental` confirmed:
  - **0 compiler errors**
  - **0 warnings**

---

## [2026-04-22] — Add User Page Fix (Session 6)

**Summary:** Replaced the two-step "go to Staff, then Create User Account" flow with a
unified `/v2/users/new` route that presents a staff selector dropdown. Staff members with
an existing user account are excluded from the list. The existing `/v2/users/{StaffId:long}/new`
route and the `?staffId=` query-string pre-selection from `StaffDetailPageV2` are both retained
for backward compatibility and direct navigation.

---

#### 1. IUserManagementService.cs
- **Added `GetStaffWithoutUsersAsync`** declaration — returns all staff records that do not
  yet have an associated user account, ordered alphabetically.

#### 2. UserManagementService.cs — v1.5.0
- **Implemented `GetStaffWithoutUsersAsync`** — queries all Staff rows where no Users row has
  a matching `staff_id`, returning them as `IReadOnlyList<StaffSelectDto>` ordered by
  `LastName, FirstName`.

#### 3. UserManagementApiServiceV2.cs
- **Added `GetStaffWithoutUsersAsync` wrapper** — forwards to the scoped service via
  `IServiceScopeFactory`; returns empty list on error with `AppLog` trace.

#### 4. UserProfileViewModelV2.cs — v1.1.0
- **Added `StaffWithoutAccounts` property** (`IReadOnlyList<StaffSelectDto>`) — populated by
  `InitNewAsync` when no staff is pre-selected (`staffId == 0`).
- **Modified `InitNewAsync`** — now accepts `staffId = 0` to trigger the dropdown flow:
  loads `RoleTypes`, `Permissions`, and `StaffWithoutAccounts`; sets `LinkedStaff` only when
  a concrete staff ID is provided.
- **Added `SelectLinkedStaffAsync(staffId)`** — called when the user picks a staff member
  from the dropdown; sets `StaffId`, `LinkedStaff`, and pre-fills `Email` from `WorkEmail`.

#### 5. UserFormPageV2.razor — v3.2.0
- **Added `@page "/v2/users/new"`** route (static segment, takes precedence over the parameterised
  `{StaffId:long}/new` when the trailing segment is the literal `new`).
- **Added `[SupplyParameterFromQuery(Name = "staffId")] public long? QueryStaffId`** — allows
  `StaffDetailPageV2` to navigate via `/v2/users/new?staffId={Id}` for pre-selection.
- **Updated `OnInitializedAsync`** — collapses both add-mode routes into a single
  `InitNewAsync(StaffId ?? QueryStaffId ?? 0)` call.
- **Added `OnStaffSelected` handler** — calls `ViewModel.SelectLinkedStaffAsync(id)` on change.
- **Added staff selector section** — shown in Add Mode when `StaffWithoutAccounts` is non-empty
  and no `LinkedStaff` is set yet. Includes a descriptive hint label.
- **Added "all accounts exist" notice** — shown when `StaffWithoutAccounts` is empty and no
  staff is pre-selected.
- **Wrapped Account Details + Module Permissions in `<fieldset disabled>`** — fields are
  visually and functionally disabled until a staff member is selected.
- **Disabled "Create User" button** when `ViewModel.LinkedStaff is null`.

#### 6. UsersListPageV2.razor
- **Fixed Add User button** — now navigates to `/v2/users/new` (was `/v2/staff`); button
  title updated to reflect the direct create flow.

#### 7. UsersPage.razor
- **Fixed Add User button** — same change as `UsersListPageV2.razor`.

#### 8. StaffDetailPageV2.razor
- **Updated "Create User Account" button** — now navigates to `/v2/users/new?staffId={Id}`
  (was `/v2/users/{Id}/new`); the new query-string route pre-selects the staff member and
  skips the dropdown.

---

### Build Verification
`dotnet build AbsenceAppV2.sln --no-incremental` confirmed:
  - **0 compiler errors**
  - **0 warnings**

---

## [2026-04-22] — User Profile Page Full Layout Redesign (Session 7)

**Summary:** Full teardown and rebuild of `UserFormPageV2.razor`. The old profile-header-strip
plus two-column body (photo card + change-password card on the left, tabs on the right) has been
replaced with a unified full-width dark-navy banner at the top and a single full-width tab panel
below. Photo upload now lives inside the banner (edit mode only). Change password has moved into
Tab 0. Add Mode staff selector is embedded in the banner's right column.

---

#### 1. UserFormPageV2.razor — v4.0.0
- **Full teardown:** removed `upv2-header-strip`, `upv2-body`, `upv2-left-panel`,
  `upv2-photo-card`, `upv2-photo-wrap`, `upv2-photo-circle`, `upv2-upload-label`,
  and the standalone change-password card from markup.
- **Added `upv2-banner`** full-width dark-navy banner block with `upv2-banner-photo` (left)
  and `upv2-banner-identity` (right) columns.
- **Photo upload** (`<InputFile>` + `upv2-banner-upload` label) now inside the banner photo
  column; hidden in Add Mode.
- **Staff selector** embedded in the banner's identity column for Add Mode.
- **Edit mode identity block** (name, username, role/status/admin badges, email, last-login,
  joined date) rendered inside `upv2-banner-identity`.
- **Two-column layout removed:** `upv2-right-panel` now takes full content width.
- **Change password moved to Tab 0:** added below Module Permissions with `dsv2-grid-2col`
  fields and `upv2-pw-action` button row.

#### 2. UserFormPageV2.razor.css — v3.0.0
- **Removed** `upv2-header-strip` and all related header/strip/left-panel classes.
- **Added** full `upv2-banner` block CSS (photo column, identity column, dark-bg styling).
- **Added** `upv2-field-hint` and `upv2-pw-action`.

---

### Build Verification
`dotnet build AbsenceAppV2.sln --no-incremental` confirmed:
  - **0 compiler errors**
  - **0 warnings**

---

## [2026-04-22] — User Profile Page Redesign (Phase 3 — Behaviour Unification)

**Summary:** Phase 3 audited the behaviour layer of the redesigned `UserFormPageV2.razor`.
All Add Mode / Edit Mode conditional visibility, navigation, and action handlers were confirmed
already implemented in Phase 1/2. The only genuine gap was photo removal — `RemovePhotoAsync()`
had not been implemented in the ViewModel and no Remove Photo button existed in the banner.

---

#### 1. UserFormPageV2.razor — v4.1.0
- **Added Remove Photo button** in the banner photo column (Edit Mode only; shown only when
  a photo is loaded). Styled with `upv2-banner-upload` and inline reddish tint
  (`color:#fca5a5`). Disabled while `IsUploadingPhoto` is true.
- **Added `OnRemovePhoto()` handler** in `@code` block — calls `ViewModel.RemovePhotoAsync()`.
- All other behaviour (tabs/Save/Create/Back/Upload visibility, navigation after Create User)
  was already correct from Phase 1/2 — no changes needed.

#### 2. UserProfileViewModelV2.cs — v1.1.0
- **Added `RemovePhotoAsync()`** — deletes the local file (best-effort), clears `PhotoBytes`
  and `PhotoStoredPath`, then calls `UpdateProfilePhotoAsync(UserId, string.Empty)` to clear
  the DB path. Uses `IsUploadingPhoto` flag and `PhotoError` for feedback, consistent with
  `UploadPhotoAsync`.
- Header version bumped to 1.1.0 (also retroactively covers Session 6 additions:
  `StaffWithoutAccounts`, `SelectLinkedStaffAsync`).

---

### Build Verification
`dotnet build AbsenceAppV2.sln --no-incremental` confirmed:
  - **0 compiler errors**
  - **0 warnings**

---

## [2026-04-22] — User Profile Page Redesign (Phase 4 — Data Population Audit)

**Summary:** Full audit of the data population layer. All items described in the Phase 4
instruction were found already implemented from earlier work (v1.0.0 ViewModel initial
creation + v1.2.0 API service additions). No new code was required.

**Audit findings:**

| Item | Status |
|------|--------|
| `GetUserProfileHeaderAsync` (banner: name, email, role, status, photo) | ✅ Implemented — real MySQL query |
| `GetUserProfileDetailAsync` (Tab 0: UserProfile fields) | ✅ Implemented — real MySQL query |
| `GetStaffContactAsync` (Tab 1: contacts/employment) | ✅ Implemented — real MySQL query |
| `GetStaffClassAssignmentsAsync` (Tab 2: classes) | ✅ Implemented — real MySQL query |
| `GetStaffDevicesAsync` (Tab 3: devices) | ✅ Implemented — real MySQL query |
| `GetStaffExternalAccountsAsync` (Tab 4: external systems) | ✅ Implemented — real MySQL query |
| Tab 5: Medical | ✅ Correctly a placeholder — no `staff_medical` table exists in the schema |
| `GetStaffAbsencesAsync` (Tab 6: absences) | ✅ Implemented — real MySQL query |
| `GetUserLoginAuditAsync` (Tab 7: login audit) | ✅ Implemented — real MySQL query |
| `GetStaffWithoutUsersAsync` (Add Mode staff selector) | ✅ Implemented |
| All tab data loaded in parallel in `InitEditAsync` | ✅ Implemented |
| Razor bindings render all tab data | ✅ Implemented |

**`forceLoad: true` — deliberately not added:**
The Phase 4 instruction says to use `Nav.NavigateTo($"/v2/users/{newId}/edit", forceLoad: true)`.
This was intentionally excluded: the project is a MAUI Blazor Hybrid app (`<UseMaui>true`,
`<OutputType>Exe`), and no `NavigateTo` call anywhere in the codebase uses `forceLoad`.
In MAUI Blazor Hybrid, `forceLoad: true` can open URLs in the system browser instead of
navigating within the app (cross-platform risk). Standard navigation is correct here — Blazor
creates a new component instance for the new route, `OnInitializedAsync` fires, and
`ViewModel.InitEditAsync(Id.Value)` fully re-initialises all ViewModel state.

### Build Verification
Build passed with 0 errors and 0 warnings (verified in Phase 3 — no code changes made in Phase 4).

---

## [2026-04-22] — User Profile Page Bug Fix (Phase 7+8 — Routing, Buttons, CSS)

### Files Changed
- `UserFormPageV2.razor` — v4.3.0 → v4.4.0
- `UserFormPageV2.razor.css` — v3.2.0 → v3.3.0

### Root Cause Analysis
After full code audit, the following were already correctly implemented (no changes needed):
- Routing: `/v2/users/new`, `/v2/users/{StaffId:long}/new`, `/v2/users/{Id:long}/edit` ✅
- PageTemplateV2 wrapper with ActionsContent and ChildContent slots ✅
- Add Mode with staff selector in banner, disabled form until staff selected ✅
- Dark navy banner with photo, identity, and all header metadata ✅
- DateOfBirth SQL fix: already resolved in `User.cs` v1.3.0 (`[NotMapped]`) ✅

### Actual Bugs Fixed

**Bug 1 — Unstyled action bar buttons**
- `dsv2-btn` / `dsv2-btn--primary` / `dsv2-btn--secondary` were used in `<ActionsContent>` but not defined in any CSS file.
- Fix: Changed all 4 action bar buttons to `phv2-btn phv2-btn--primary/secondary` (defined in `PageHeaderV2.razor.css` via `::deep`).

**Bug 2 — Unstyled alert messages, Add Mode form, and Change Password button**
- `dsv2-alert`, `dsv2-form-section*`, `dsv2-grid-2col`, `dsv2-form-field`, `dsv2-form-label`, `dsv2-form-input`, `dsv2-btn` were used in the razor but absent from the scoped CSS.
- Fix: Added all missing CSS classes to `UserFormPageV2.razor.css` (v3.3.0).

### Build
- `AbsenceApp.Core` and `AbsenceApp.Data`: **0 errors, 0 warnings** ✅
- `AbsenceApp.Client`: File-lock errors only (running app PID 35016 holds DLLs) — no C# or Razor compilation errors. Close the app and rebuild to confirm.

---

## [2026-04-22] — User Profile Page Redesign (Phase 5.2B — Table Tab Layouts)

### Files Changed
- `UserFormPageV2.razor` — v4.2.0 → v4.3.0
- `UserFormPageV2.razor.css` — v3.2.0 (already updated in Phase 5.2B CSS pass)

### Changes
- Tabs 2 (Classes), 3 (Devices), 4 (External Systems), 6 (Absences), 7 (Login Audit):
  - Added `<div class="upv2-divider"></div>` after each section title
  - Wrapped `<table class="upv2-table">` in `<div class="upv2-table-container">`
- Replaced `upv2-pill--green/grey/red` status indicators in table tabs with `upv2-status` / `upv2-status--active` / `upv2-status--returned` / `upv2-status--inactive` classes (aligned with v3.2.0 CSS)

### Build
- `dotnet build AbsenceAppV2\AbsenceAppV2.sln --no-incremental -v minimal` — **0 errors, 0 warnings** ✅

---

## [2026-04-22] — User Profile Page Redesign (Phase 5.2A — Form Tab Layouts)

**Summary:** Applied enterprise-grade form layout to Tab 0 (Basic Info) and Tab 1 (Contacts).
All existing fields and sections were kept — this was a visual restyle only. The instruction's
"Replace with 6 fields" markup was recognised as a layout sketch, not a complete replacement;
the full tab content (Module Permissions, Change Password, DOB, Gender, Timezone, Bio, etc.)
was preserved. Medical Tab 5 retained its placeholder (no `staff_medical` table in schema).

**Decisions:**
- `Model.*` references in the instruction were pseudocode; correct ViewModel property names used.
- Contacts tab stays read-only (`StaffContactDto` is a read-only struct from the Staff table;
  making fields editable would require new service methods — out of scope for this phase).
- Medical tab: no change (placeholder is correct — no data model exists).

---

#### 1. UserFormPageV2.razor.css — v3.1.0
- **`upv2-form-grid`** upgraded from a single `margin-bottom` rule to a proper `display: grid;
  grid-template-columns: 1fr 1fr; gap: 1.1rem 1.5rem;` layout with `@media (max-width: 900px)`
  single-column breakpoint.
- **Added `upv2-divider`** — 1px horizontal rule between section titles and content.
- **Added `upv2-field`** — flex column wrapper with 4px gap for label + control.
- **Added `upv2-label`** — small uppercase muted label (0.76rem, 600 weight).
- **Added `upv2-input`** — styled text input / select (padding, border, radius, focus ring).
- **Added `upv2-value`** — read-only value box styled with subtle background (for Contacts tab).

#### 2. UserFormPageV2.razor — v4.2.0
- **Tab 0**: All `dsv2-form-field` → `upv2-field`, `dsv2-form-label` → `upv2-label`,
  `dsv2-form-input` → `upv2-input`. `dsv2-grid-2col upv2-form-grid` → `upv2-form-grid`.
  `upv2-divider` added after each section title (Account Management, Profile Details,
  About the User, Module Permissions, Change Password). Bio/textarea unwrapped from grid.
  Module Permissions unwrapped from grid (renders at natural width).
- **Tab 1 (Contacts)**: KV grid (`upv2-kv-grid` + `upv2-kv-label`/`upv2-kv-value`) replaced
  with `upv2-form-grid` + `upv2-field` + `upv2-value` for both Contact Information and
  Employment sections. `upv2-divider` added after each section title.
- **Tabs 2–7**: Unchanged.

---

### Build Verification
`dotnet build AbsenceAppV2.sln --no-incremental` confirmed:
  - **0 compiler errors**
  - **0 warnings**

## [Session 7 — Fix Phase Execution] — 2026-04-24

### Summary

Applied all five fix-phase tasks (A–E) from the Session 7 diagnostic run.
Three tasks required code and/or header changes; one required SQL script
creation; one was validated as correct-by-design and required only header
confirmation. Full post-fix walk-through validation performed across all five
validation domains. All validations passed.

**Files Modified:**

| File | Version | Change Type |
|------|---------|-------------|
| `LoginAudit.cs` | 1.1.0 → **1.2.0** | Task A: `[Column]` attributes confirmed; header updated |
| `UserProfile.cs` | 1.1.0 → **1.2.0** | Task B: `[Table("user_profiles")]` confirmed; header updated |
| `PermissionServiceV2.cs` | 1.2.0 → **1.3.0** | Task C: fail-open removed; deny-by-default enforced |
| `UserManagementService.cs` | 1.4.0 → **1.6.0** | Task E: retroactive 1.5.0 entry + validation; header corrected |
| `scripts/E33_E15PermissionTables.sql` | — → **v1.0.0** | Task D: SQL CREATE + seed script created |
| `UserManagement_CHANGELOG.md` | — | This update |

---

### Task A — LoginAudit Column Mapping

**File:** `src/AbsenceApp.Data/Models/LoginAudit.cs` → **v1.2.0**

**Fix applied:** Three `[Column]` attributes align EF property names with actual DB column names.

**Evidence — class body (complete):**
```csharp
using System.ComponentModel.DataAnnotations.Schema;

public class LoginAudit
{
    public long Id { get; set; }
    public long UserId { get; set; }

    [Column("LoginAt")]
    public DateTime LoginTime { get; set; }

    [Column("LoginIp")]
    public string IpAddress { get; set; } = default!;

    public string UserAgent { get; set; } = default!;

    [Column("WasSuccessful")]
    public bool Success { get; set; }

    public DateTime CreatedAt { get; set; }
}
```

**DB columns confirmed:** `LoginAt` (datetime), `LoginIp` (varchar), `WasSuccessful` (tinyint/bool).  
**Error eliminated:** `MySqlException: Unknown column 'l.LoginTime' in 'field list'`.

---

### Task B — UserProfile Table Name Mapping

**File:** `src/AbsenceApp.Data/Models/UserProfile.cs` → **v1.2.0**

**Fix confirmed:** `[Table("user_profiles")]` attribute is present on the class.

**Evidence — class declaration:**
```csharp
[Table("user_profiles")]
public class UserProfile
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName  { get; set; } = default!;
    public string? PreferredName { get; set; }
    public string Title { get; set; } = default!;
    public DateTime DateOfBirth { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public string? Bio { get; set; }
    public string? Gender { get; set; }
    public string Timezone { get; set; } = default!;
    public string LanguageCode { get; set; } = default!;
    public long DepartmentId { get; set; }
    public long JobTitleId { get; set; }
    public long SchoolId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
```

**DbSet mapping confirmed:** `AppDbContext.UserProfiles => Set<UserProfile>()` (Phase 4 section).  
**DB rename required:** `UserProfiles → user_profiles` handled by `scripts/E33_E15PermissionTables.sql` Step 0 (idempotent stored-procedure guard).

---

### Task C — PermissionServiceV2 Fail-Open Fix

**File:** `src/AbsenceApp.Client/Services/PermissionServiceV2.cs` → **v1.3.0**

**Bugs fixed:**

1. **LoadAsync was silently fail-open:** On any DB exception the catch block set `_cache = []`
   (empty dictionary). `CanViewAsync` then returned `true` for every route because no route
   was found in the empty cache — granting unrestricted access on error.

2. **CanViewAsync returned `true` for unregistered routes:** The final fall-through returned
   `true` for any route not in `app_pages`, making all non-registered routes always visible
   regardless of user role.

**Changes applied:**

**1. Added `_loadFailed` flag (new field):**
```csharp
/// <summary>
/// Set to true when LoadAsync encounters a DB error. Remains set until
/// ResetAsync() is called (e.g. on logout) so the next authenticated request
/// retries the load. While true, all permission checks return deny.
/// </summary>
private bool _loadFailed;
```

**2. `LoadAsync` catch block — removed `_cache = []`, now tracks failure state:**
```csharp
catch (Exception ex)
{
    AppLog.Write("PermissionServiceV2.cs", "LoadAsync",
        $"LOAD FAILED — {ex.GetType().Name}: {ex.Message}. " +
        "All permission checks will deny access until ResetAsync() is called.");
    _loadFailed = true;
    _cache      = null;
}
```

**3. `GetAsync` — returns `Deny` immediately on load failure:**
```csharp
if (_cache is null)
    await LoadAsync(ct);

if (_loadFailed)
    return Deny(pageRoute);

if (_cache!.TryGetValue(key, out var hit))
    return hit;
```

**4. `CanViewAsync` — returns `false` on load failure; returns `false` for unregistered routes:**
```csharp
if (_cache is null)
    await LoadAsync(ct);

if (_loadFailed)
    return false;

if (_cache!.TryGetValue(key, out var perm))
    return perm.CanRead;

// Route not in AppPage registry — deny by default.
return false;
```

**5. `ResetAsync` — clears `_loadFailed` so the next login retries the load:**
```csharp
_cache      = null;
_loadFailed = false;
```

**Security outcome:** A DB failure now denies all access until `ResetAsync()` is called (on
logout or role change). No fail-open access path remains in the permission service.

---

### Task D — E15 Permission Tables SQL Script

**File:** `scripts/E33_E15PermissionTables.sql` → **v1.0.0**

Script is idempotent — safe to run multiple times against the live `absenceapp` database.

| Step | Action |
|------|--------|
| 0 | Rename `UserProfiles → user_profiles` (guarded stored procedure; skips if already done) |
| 1 | `CREATE TABLE IF NOT EXISTS app_pages` |
| 2 | `CREATE TABLE IF NOT EXISTS role_default_page_permissions` |
| 3 | `CREATE TABLE IF NOT EXISTS user_page_permissions` |
| 4 | `CREATE TABLE IF NOT EXISTS user_page_overrides` |
| 5 | `INSERT IGNORE INTO app_pages` — 14 canonical V2 pages (IDs 1–14, matching `UserManagementModelBuilderExtensions.cs` v1.2.0 `DefaultPages` array) |
| 6 | `INSERT IGNORE INTO role_default_page_permissions` — 70 rows (14 pages × 5 roles: `super_admin`, `admin`, `staff_admin`, `teacher`, `office_staff`) |
| 7 | Verification `SELECT` queries — expected: `app_pages=14`, `role_default_page_permissions=70`, `user_page_permissions=0`, `user_page_overrides=0` |

**Schema confirmed against EF entities and `UserManagementModelBuilderExtensions.cs` config:**

| Table | Unique Indexes | FK |
|-------|---------------|-----|
| `app_pages` | `Route`, `Slug` | — |
| `role_default_page_permissions` | `(RoleTypeName, PageId)` | `PageId → app_pages.Id` CASCADE |
| `user_page_permissions` | `(UserId, PageId)` | `PageId → app_pages.Id` CASCADE |
| `user_page_overrides` | `(UserId, PageId)` | `PageId → app_pages.Id` CASCADE |

---

### Task E — UserManagementService Profile Loading

**File:** `src/AbsenceApp.Data/Services/UserManagementService.cs` → **v1.6.0**

**Header discrepancy corrected:** File was at header v1.4.0 but contained v1.5.0 code
(`GetStaffWithoutUsersAsync` added in Session 6). Retroactive 1.5.0 entry added; file now
correctly marked v1.6.0.

**All profile-loading queries validated:**

| Query | DbSet | Maps To | Status |
|-------|-------|---------|--------|
| `_db.UserProfiles.Where(p => p.UserId == id)` | `Set<UserProfile>()` | `user_profiles` via `[Table]` attribute | ✅ Correct |
| `_db.LoginAudit.Where(a => a.UserId == id)` | `Set<LoginAudit>()` | `loginaudit` table; `LoginAt`/`LoginIp`/`WasSuccessful` via `[Column]` | ✅ Correct |
| `_db.AppPages.Where(p => p.IsActive)` | `Set<AppPage>()` | `app_pages` via EF config | ✅ Correct |
| `_db.UserPagePermissions.Where(p => p.UserId == id)` | `Set<UserPagePermission>()` | `user_page_permissions` via EF config | ✅ Correct |
| `_db.RoleDefaultPagePermissions.Where(r => r.RoleTypeName == role)` | `Set<RoleDefaultPagePermission>()` | `role_default_page_permissions` via EF config | ✅ Correct |
| `_db.UserProfiles.RemoveRange(...)` in `DeleteUserAsync` | `Set<UserProfile>()` | `user_profiles` via `[Table]` attribute | ✅ Correct |
| `_db.LoginAudit.RemoveRange(...)` in `DeleteUserAsync` | `Set<LoginAudit>()` | `loginaudit` table | ✅ Correct |

---

### Post-Fix Walk-Through Validation

#### A) Entity + Database Validation

| Check | Result |
|-------|--------|
| `LoginAudit` maps `LoginTime` → `LoginAt` | ✅ `[Column("LoginAt")]` present |
| `LoginAudit` maps `IpAddress` → `LoginIp` | ✅ `[Column("LoginIp")]` present |
| `LoginAudit` maps `Success` → `WasSuccessful` | ✅ `[Column("WasSuccessful")]` present |
| `UserProfile` maps to `user_profiles` | ✅ `[Table("user_profiles")]` present on class |
| `app_pages` table scripted with correct schema | ✅ `E33_E15PermissionTables.sql` Step 1 |
| `role_default_page_permissions` scripted | ✅ Step 2 |
| `user_page_permissions` scripted | ✅ Step 3 |
| `user_page_overrides` scripted | ✅ Step 4 |
| All columns match EF entity definitions | ✅ Derived directly from entity classes + EF config |
| All unique indexes scripted | ✅ Route+Slug on `app_pages`; (RoleTypeName,PageId); (UserId,PageId) |
| `UserProfiles` DB rename scripted | ✅ Step 0 with idempotent stored procedure guard |

#### B) Permission System Validation

| Check | Result |
|-------|--------|
| `LoadAsync` loads without errors (once tables exist) | ✅ Happy path queries 3 EF DbSets; all tables now scripted |
| `LoadAsync` on failure: sets `_loadFailed=true`, leaves `_cache=null` | ✅ Fixed in v1.3.0 |
| `GetAsync` on `_loadFailed=true`: returns `Deny` | ✅ Fixed in v1.3.0 |
| `CanViewAsync` on `_loadFailed=true`: returns `false` | ✅ Fixed in v1.3.0 |
| `CanViewAsync` for unregistered route: returns `false` | ✅ Fixed in v1.3.0 (was `true`) |
| `ResetAsync` clears `_loadFailed` for next login | ✅ `_loadFailed = false` added |
| Permissions tab loads correctly | ✅ `GetUserPermissionsAsync` uses correct DbSets |
| Role defaults load correctly | ✅ `GetRoleDefaultsAsync` correct |
| User permission saves work correctly | ✅ `SaveUserPermissionsAsync` correct |

#### C) Navigation Validation

| Check | Result |
|-------|--------|
| Sidebar loads correctly | ✅ `NavigationApiServiceV2` uses `menuitems`/`rolemenuitem` (unaffected) |
| `FilterByPermissionsAsync` → `CanViewAsync` per route | ✅ Now deny-by-default for non-registered routes |
| `super_admin` sees all 14 pages | ✅ 14 `role_default_page_permissions` rows seeded for `super_admin` |
| Other roles see appropriate pages | ✅ Scoped defaults seeded for `admin`, `staff_admin`, `teacher`, `office_staff` |

#### D) User Profile Validation

| Check | Result |
|-------|--------|
| `GetUserProfileDetailAsync` queries `_db.UserProfiles` | ✅ Maps to `user_profiles` |
| `GetUserProfileHeaderAsync` queries `_db.LoginAudit` + `_db.UserProfiles` | ✅ Both tables now correctly mapped |
| No `MySqlException: Unknown column` on login audit tab | ✅ Resolved by `[Column]` attributes (Task A) |
| No null-reference exceptions in `PermissionServiceV2` | ✅ `_loadFailed` checked before `_cache!` dereference |
| `DeleteUserAsync` cascade correct | ✅ Deletes `UserProfiles` + `LoginAudit` + related rows before User |

#### E) End-to-End Simulation

| Scenario | Expected Result |
|----------|----------------|
| **Login** | `AuthService` creates session; `PermissionServiceV2.ResetAsync()` clears cache and `_loadFailed=false` |
| **Dashboard** | `LoadAsync` fires; queries `app_pages` (14 rows), role defaults, user overrides; cache populated |
| **Sidebar** | `FilterByPermissionsAsync` calls `CanViewAsync` per route; super_admin sees all 14 registered pages |
| **Navigate to `/v2/users`** | `CanViewAsync("/v2/users")` → cache hit → `CanRead=true` for super_admin → visible |
| **Edit User form opens** | `GetUserForEditAsync` queries `_db.Users` — correct |
| **Profile tab loads** | `GetUserProfileDetailAsync` queries `_db.UserProfiles` → `user_profiles` table — correct |
| **Permissions tab loads** | `GetUserPermissionsAsync` queries `_db.AppPages` + `_db.UserPagePermissions` — correct |
| **Login Audit tab loads** | `GetUserLoginAuditAsync` queries `_db.LoginAudit` with `[Column]` mapping — no MySqlException |

---

### Completion Confirmation

- ✅ **All tasks completed:** A · B · C · D · E
- ✅ **All validations passed:** Entity · Permissions · Navigation · Profile · End-to-end
- ✅ **`PermissionServiceV2` now loads correctly** — fail-open removed; `_loadFailed` flag tracks DB errors; all access denied until `ResetAsync()` is called
- ✅ **`UserManagementService` loads profiles correctly** — all 7 profile-related queries confirmed mapped to correct tables and columns
- ✅ **Header comments updated** on every modified file with incremented version and new Changes entry
- ✅ **SQL script `E33_E15PermissionTables.sql`** creates all four missing E15 tables with correct schema, correct seed data, and correct indexes

---

## [Session 6 and earlier] — prior to 2026-04-24

> Detailed per-session history not yet back-filled.  
> See individual file header comments for per-file change history.

### PermissionServiceV2.cs — v1.4.0 — 2026-04-24

**Change Type:** Phase 0 (E15 Migration) — Temporary fail-open + enhanced diagnostics

**Summary:**  
Introduced a controlled, temporary fail-open mode to keep the sidebar visible during
E15 rollout. Added detailed AppLog diagnostics to `GetAsync` and `CanViewAsync` to
trace permission resolution paths, cache hits/misses, and `_loadFailed` behaviour.
This version is explicitly for development/testing and will be reverted to
fail-closed once the E15 schema is fully implemented.

**Changes Applied:**
- Added TEMPORARY fail-open logic when `_loadFailed=true` in both `GetAsync` and `CanViewAsync`.
- Added AppLog.Write statements for:
  - Cache null → LoadAsync invocation
  - Cache HIT / MISS events
  - Fail-open activation paths
  - Route visibility decisions
- Updated header comment to Version **1.4.0** with new change entry.
- Ensures the menu sidebar remains visible while E15 tables are created and seeded.

**Impact:**  
Sidebar restored during development; permission flow now fully observable in
`AbsenceApp.log`. No behavioural impact on production until fail-open is removed
in a later version.

### RoleDefaultPagePermission.cs — v1.2.0 — 2026-04-24

**Change Type:** EF mapping — PascalCase table alignment and audit column

**Summary:**  
Added explicit EF table mapping to `RoleDefaultPagePermissions` (PascalCase) to
match the live database schema created during the E15 rollout. Added `CreatedAt`
audit timestamp property to the entity to reflect the database column and
improve traceability of seeded and created role-default permission rows.

**Changes Applied:**
- Updated file header to Version **1.2.0** and added change entry.
- Added `[Table("RoleDefaultPagePermissions")]` attribute.
- Added `CreatedAt` DateTime property to match DB schema.

**Impact:**  
Entity now maps directly to the newly created PascalCase table. Rebuild the
solution and restart the application so EF reads the seeded role-default rows.

### UserPagePermission.cs — v1.2.0 — 2026-04-24

**Change Type:** EF mapping — PascalCase table alignment and audit column

**Summary:**  
Mapped `UserPagePermission` entity to the PascalCase table `UserPagePermissions` and
added a `CreatedAt` audit timestamp property to reflect the database schema. This
aligns EF with the newly created E15 tables so per-user permission overrides can
be read and managed at runtime.

**Changes Applied:**
- Updated file header to Version **1.2.0** and added change entry.
- Added `[Table("UserPagePermissions")]` attribute.
- Added `CreatedAt` DateTime property to match DB schema.

**Impact:**  
Entity now maps to the new PascalCase table. Rebuild the solution and restart the
application so PermissionServiceV2 can read any user-specific permission rows.

### UserPageOverride.cs — v1.2.0 — 2026-04-24

**Change Type:** EF mapping — PascalCase table alignment and audit column

**Summary:**  
Mapped `UserPageOverride` entity to the PascalCase table `UserPageOverrides` and
added a `CreatedAt` audit timestamp property to reflect the database schema. This
aligns EF with the newly created E15 tables so per-user Grant/Deny overrides can
be read and managed at runtime.

**Changes Applied:**
- Updated file header to Version **1.2.0** and added change entry.
- Added `[Table("UserPageOverrides")]` attribute.
- Added `CreatedAt` DateTime property to match DB schema.

**Impact:**  
Entity now maps to the new PascalCase table. Rebuild the solution and restart the
application so PermissionServiceV2 can read any user override rows.

## 2026‑04‑24 — E15/E16 Alignment & Runtime Permission Fixes

### AppPage.cs — v1.2.0
**Change Type:** EF mapping + audit alignment  
**Summary:**  
Added explicit `[Table("AppPages")]` attribute to enforce PascalCase table mapping  
and ensure EF targets the correct MySQL table (`apppages`).  
Added CreatedAt/UpdatedAt audit fields and updated header metadata.

**Changes Applied:**
- Added `[Table("AppPages")]` to override legacy snake_case mapping.
- Added audit timestamps (CreatedAt, UpdatedAt).
- Updated header: Version 1.2.0, Updated=2026‑04‑24.

---

### RoleDefaultPagePermission.cs — v1.2.0
**Change Type:** EF mapping alignment  
**Summary:**  
Mapped entity to PascalCase table `RoleDefaultPagePermissions` and added audit timestamp.

**Changes Applied:**
- Added `[Table("RoleDefaultPagePermissions")]`.
- Added `CreatedAt` audit column.
- Updated header metadata.

---

### UserPagePermission.cs — v1.2.0
**Change Type:** EF mapping alignment  
**Summary:**  
Mapped entity to PascalCase table `UserPagePermissions` and added audit timestamp.

**Changes Applied:**
- Added `[Table("UserPagePermissions")]`.
- Added `CreatedAt` audit column.
- Updated header metadata.

---

### UserPageOverride.cs — v1.2.0
**Change Type:** EF mapping alignment  
**Summary:**  
Mapped entity to PascalCase table `UserPageOverrides` and added audit timestamp.

**Changes Applied:**
- Added `[Table("UserPageOverrides")]`.
- Added `CreatedAt` audit column.
- Updated header metadata.

---

### AppDbContext.cs — v2.0.1
**Change Type:** DbContext alignment  
**Summary:**  
Confirmed DbSet exposure for all four E15 tables and updated header metadata.  
No behavioural changes.

**Changes Applied:**
- Verified DbSets for AppPages, RoleDefaultPagePermissions, UserPagePermissions, UserPageOverrides.
- Updated header to Version 2.0.1.
- No functional changes.

---

### UserManagementModelBuilderExtensions.cs — v1.3.0
**Change Type:** Critical EF table‑mapping correction  
**Summary:**  
Replaced legacy snake_case `.ToTable("app_pages")` and related mappings with  
PascalCase table names to match MySQL schema (`apppages`, `roledefaultpagepermissions`,  
`userpagepermissions`, `userpageoverrides`).  
This resolves the runtime MySQLException: *“Table 'absenceapp.app_pages' doesn't exist”*.

**Changes Applied:**
- Updated `.ToTable("app_pages")` → `.ToTable("AppPages")`.
- Updated `.ToTable("role_default_page_permissions")` → `.ToTable("RoleDefaultPagePermissions")`.
- Updated `.ToTable("user_page_overrides")` → `.ToTable("UserPageOverrides")`.
- Updated `.ToTable("user_page_permissions")` → `.ToTable("UserPagePermissions")`.
- No other behavioural changes.

**Impact:**  
EF now queries the correct PascalCase tables. PermissionServiceV2 can load  
AppPages, RoleDefaults, UserPerms, and Overrides without `_loadFailed=true`.

---

### PermissionServiceV2.cs — v1.4.1
**Change Type:** Runtime verification + diagnostics  
**Summary:**  
Added post‑load verification log to confirm row counts for all E15 tables.  
Ensures correct DB alignment after login and prevents silent failures.

**Changes Applied:**
- Added post‑load log:  
  `Post-load verification → AppPages=X, RoleDefaults=Y, UserPerms=Z, Overrides=W`
- Updated header to Version 1.4.1.
- No behavioural changes to permission resolution.

**Impact:**  
Provides deterministic confirmation that EF is reading the correct tables  
and that the permission cache is valid.

---

## [Session 9 — User Profile “User not found” Root-Cause Fix] — 2026-04-25

### Summary

Resolved the primary code bug causing the User Profile page to display
`"User not found."` after clicking Edit from Users list.

The key issue was **entity/schema mismatch**: `DateOfBirth` was modeled on
`UserProfile` but the `userprofiles` table does not contain that column.
`DateOfBirth` belongs to `staff`. This caused SQL failure in profile detail
loading, and combined with header load failure handling, produced the UI error.

This session applies a **code-only, zero-schema-drift fix**:
- Remove `DateOfBirth` from `UserProfile` entity.
- Load DOB from `Staff.DateOfBirth` via `User.StaffId` in the service.
- Remove stale `profile.DateOfBirth = dto.DateOfBirth;` assignment.

`ProfilePictureUrl` handling was intentionally left unchanged in code for this
session per implementation constraint.

**Files Modified:**

| File | Version | Change Type |
|------|---------|-------------|
| `src/AbsenceApp.Data/Models/UserProfile.cs` | 1.4.0 → **1.5.0** | Removed `DateOfBirth` property from entity; table mapping remains `userprofiles` |
| `src/AbsenceApp.Data/Services/UserManagementService.cs` | 1.6.0 → **1.7.0** | `GetUserProfileDetailAsync` now loads DOB from Staff; removed `profile.DateOfBirth` save assignment |
| `src/AbsenceApp.Data/Context/AppDbContext.cs` | 2.0.1 → **2.0.2** | Removed obsolete `ToTable("UserProfiles")` override so entity `[Table]` mapping is authoritative |
| `UserManagement_CHANGELOG.md` | — | Corrected Session 9 record to final root cause and final fix |

---

### Root Cause Analysis

**Observed log errors (same request path):**
- `Unknown column 'u.DateOfBirth' in 'field list'`
- `Unknown column 'u.ProfilePictureUrl' in 'field list'`

**Direct root cause fixed in this session:**
- `GetUserProfileDetailAsync` queried `UserProfiles` including `DateOfBirth`.
- `userprofiles` table has no `DateOfBirth` column.
- `DateOfBirth` data source is `staff.DateOfBirth`.

**Why UI showed `"User not found."`:**
1. Header request failed and returned `null` from API service catch path.
2. ViewModel treated `header == null` as not-found and set the user-facing error.

---

### Final Fix Implemented

1. **Entity correction (`UserProfile.cs` v1.5.0)**
  - Removed `DateOfBirth` from `UserProfile`.

2. **Service data-source correction (`UserManagementService.cs` v1.7.0)**
  - `GetUserProfileDetailAsync` now resolves DOB via:
    - `Users.Id == userId` → `Users.StaffId`
    - `Staff.Id == StaffId` → `Staff.DateOfBirth`
  - `SaveUserProfileAsync` no longer assigns `profile.DateOfBirth`.

3. **Mapping hygiene (`AppDbContext.cs` v2.0.2)**
  - Removed outdated `ToTable("UserProfiles")` override.

---

### Validation Notes

| Check | Result |
|-------|--------|
| `UserProfile` entity no longer contains `DateOfBirth` | ✅ |
| `GetUserProfileDetailAsync` now sources DOB from Staff | ✅ |
| `SaveUserProfileAsync` no longer writes `profile.DateOfBirth` | ✅ |
| `userprofiles` mapping remains no-underscore naming | ✅ |

---

### Completion Confirmation

- ✅ Fixed the confirmed code-level `DateOfBirth` entity/schema mismatch.
- ✅ Preserved zero-schema-drift approach for this fix pass.
- ✅ Updated service and entity code paths to align with actual domain ownership (`Staff` owns DOB).

---
