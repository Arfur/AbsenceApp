================================================================================
 File        : UserManagement_CHANGELOG.md
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-04-21
 Updated     : 2026-04-21
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
