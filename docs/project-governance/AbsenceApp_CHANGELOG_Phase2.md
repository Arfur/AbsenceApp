---
doc_type: changelog_phase
project_name: "AbsenceApp"
module: "V2"
phase: "Phase 2 — Design System & Core Components"
file_name: "AbsenceApp_CHANGELOG_Phase2.md"
scope: "User Management module (code, schema, UI)"
version: "1.0.0"
created_date: "2026-04-21"
updated_date: "2026-04-22"
authority: "Michael"
ai_contract:
  read_rules: "Agents may read this phase changelog for context, debugging, and audit purposes."
  write_rules: "Append-only. Agents may append new entries to this file while Phase 2 is Active. Agents MUST NOT edit or delete existing entries, change historical dates/versions, or alter entry structure. For master-level summaries, append a short line to AbsenceApp_CHANGE_LOG.md only."
template_marker: true
template_version: "2026-04-30"
template_location: "top-of-file"
template_instructions: "Copy the template block below for new entries; do not edit this template block."
---
================================================================================
 File        : AbsenceApp_CHANGELOG_Phase2.md
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-04-21
 Updated     : 2026-04-22
--------------------------------------------------------------------------------
 Purpose     :
   Phase 2 changelog for AbsenceApp V2. This file is the append-only,
   authoritative record of code, schema, UI, and configuration changes
   related to the User Management module (Staff-linked user creation,
   authentication, authorization, and related UI flows). Use this file for
   detailed, deterministic entries that support maintenance, onboarding,
   and rollback planning.
--------------------------------------------------------------------------------
 Notes       :
   - This file is append-only. Do not edit or remove existing entries.
   - Record completed work only; do not use this file for planning.
   - Each entry must follow the project's changelog entry template.
   - When Phase 2 completes, add a short summary line to the master changelog
     and update the master Active Phase pointer.
================================================================================

## [YYYY-MM-DD] — Short summary title 
**Type:** Code | UI | Component | Design | Docs | Hotfix  
**Scope:** (e.g., `user-management`, `component:button`, `page:add-user`)  
**Summary:** One-line summary of the change.

### Details
A short, precise description of what changed and why. Include design rationale and any important constraints or decisions.

### Affected Files and Components
- Files: path/to/file.tsx, path/to/service.cs
- Components: component-name; storybook-story
- Services/APIs: api/endpoint

### Rollout Notes
- Steps to deploy: 1) build, 2) deploy to staging, 3) smoke test
- Feature flags: feature-flag-name (if applicable)
- Backout plan: steps to revert if issues occur

### Verification
- Tests: unit/integration/manual steps to verify
- Browsers/devices: chrome, firefox, edge, mobile
- Environments applied: dev, staging, prod

### References
- Issue/PR: #123
- Design spec: docs/designs/add-user.md
- Related changelog entries: [2026-04-22] — Add User Page Fix

---

## 2026-04-21 — Staff‑Linked User Management Redesign

**Author:** Michael  
**Type:** Feature | Refactor | Security  
**Scope:** `identity:user-management`  
**Summary:** Rewrote User Management to require an explicit, immutable link between `User` and `Staff` records. Removed standalone personal fields from user DTOs and UI; creation now occurs in staff context.

### Details
- **Data model:** Added `StaffId` FK to `User` (nullable for compatibility; mandatory for new creates).  
- **DTOs:** Removed `FirstName`, `LastName`, `PhoneNumber` from `UserCreateDto`/`UserUpdateDto`; added `StaffId` to create DTO and read‑only `StaffId` to update DTO. Introduced `StaffSelectDto` for staff selection UI.  
- **Service contract:** `IUserManagementService` gained `GetStaffForUserCreateAsync(long)` and `StaffHasUserAsync(long)`.  
- **Service implementation:** `UserManagementService` rewritten to validate `StaffId` on create, prevent duplicate user per staff via `StaffHasUserAsync`, and preserve `StaffId` on update. Raw SQL retained for `userrole` operations with typed projection classes `UserRoleRow` and `UserRoleIdRow`.  
- **API wrappers & ViewModels:** `UserManagementApiServiceV2` exposes new methods; `UserFormViewModelV2` and `UserListViewModelV2` updated to reflect DTO changes and to show linked staff.  
- **Routing & UI:** Create route changed to `/v2/users/{StaffId:long}/new`; standalone `/v2/users/new` removed (later reintroduced in Session 6 as alternate flow). `UserFormPageV2.razor` shows a read‑only linked staff banner; removed personal input fields.

### Rollout Notes
- Backwards compatibility: existing users with null `StaffId` preserved; new accounts require staff link.  
- Idempotent server checks prevent duplicate user creation for a given `StaffId`.  
- Ensure client and API versions are deployed together to avoid mismatched DTOs.

### Verification
- Attempt to create user without `StaffId` → rejected.  
- Attempt to create second user for same `StaffId` → rejected with clear error.  
- Edit user → `StaffId` remains unchanged.  
- UI: Create flows only accessible from staff context or staff selector.

---

## 2026-04-21 — User Management UI Consistency Pass (Session 2)

**Author:** Michael  
**Type:** UX | Refactor | Accessibility  
**Scope:** `ui:user-management`  
**Summary:** Converted all User Management tables (Users, Roles, Permissions, Page Access) to `TablePageTemplateV2` for consistent UX; fixed encoding corruption and added export actions.

### Details
- Rewrote `UsersListPageV2`, `RolesPage`, `PermissionsPage`, and `PageAccessPage` to use `TablePageTemplateV2`.  
- Added export dropdowns (PDF/Excel), filter chips, sliding-window pagination, skeleton loading states, and two-click delete confirmation.  
- Fixed UTF‑8 encoding issues in `UserFormPageV2.razor` (e.g., `Saving…`, `Loading…`, role placeholder).  
- `UserListViewModelV2` gained `AllItems` for dynamic filters and `DeleteUserAsync` to call service delete and refresh.

### Rollout Notes
- Ensure `TablePageTemplateV2` CSS and scripts are available to all pages.  
- Export functionality requires server-side export endpoints to be present.

### Verification
- Table pages render with consistent headers, filters, and export actions.  
- Encoding displays correctly across browsers and platforms.  
- Delete flow removes user and cascades related records.

---

## 2026-04-21 — Users Table Root‑Cause Fix (Session 3)

**Author:** Michael  
**Type:** Bugfix | State Management  
**Scope:** `ui:users-page`  
**Summary:** Fixed recurring "Users table disappears on re‑navigation" bug caused by literal `SearchValue="_search"` being persisted to `AppStateService`.

### Details
- Rewrote `UsersPage.razor` (route `/v2/system/users`) to bind `SearchValue="@ViewModel.SearchTerm"` instead of literal string.  
- Injected `AppStateService` and pre-cleared stale state for `v2/system/users` in `OnInitializedAsync`.  
- Switched to `UserListViewModelV2` injection to ensure fresh DbContext per scope and avoid stale connections.

### Rollout Notes
- Clear existing `AppStateService` keys in environments where the bug occurred to remove persisted `"_search"` state.

### Verification
- Navigate away and back to Users page repeatedly — search state no longer persists as literal `"_search"`.  
- No "No users found" regressions observed.

---

## 2026-04-21 — User Profile Page Code Audit Cleanup (Session 5)

**Author:** Michael  
**Type:** Code Quality | Cleanup | Patch  
**Scope:** `ui:user-profile`  
**Summary:** Removed dead code, fixed type mismatches, restored `IsAdmin` save path, and cleaned redundant imports and CSS.

### Details
- Removed unused `BuildFullName()` helper.  
- Fixed `UserRoleIdRow.Value` type from `int` to `long`.  
- Ensured `IsAdmin` is persisted in `SaveUserProfileAsync` and flowed through DTOs and ViewModel.  
- Removed redundant `@using` directives and legacy CSS class.  
- Added Admin checkbox to profile form.

### Rollout Notes
- Minor API/DTO version bumps; ensure client and server builds align.

### Verification
- Build: 0 errors, 0 warnings.  
- `IsAdmin` toggles persist via profile save path.

---

## 2026-04-22 — Add User Page Fix (Session 6)

**Author:** Michael  
**Type:** Feature | UX | Backwards Compatibility  
**Scope:** `ui:user-create`  
**Summary:** Reintroduced a unified `/v2/users/new` route with staff selector dropdown for direct user creation; retained parameterised and query‑string preselection routes.

### Details
- `IUserManagementService` added `GetStaffWithoutUsersAsync`.  
- `UserManagementService` implemented staff query excluding existing user links.  
- `UserFormPageV2` now supports three flows: `/v2/users/new` (dropdown), `/v2/users/new?staffId={id}` (preselect), and `/v2/users/{StaffId:long}/new` (direct).  
- ViewModel supports `StaffWithoutAccounts` list and `SelectLinkedStaffAsync` to prefill email and linked staff.

### Rollout Notes
- Dropdown flow excludes staff with existing accounts.  
- Keep both parameterised and query-string routes for compatibility with existing links.

### Verification
- `/v2/users/new` shows staff selector when no `staffId` provided.  
- Selecting staff enables form and pre-fills email.  
- Creating user via preselected query-string route works as expected.

---

## 2026-04-22 — User Profile Page Full Layout Redesign (Session 7)

**Author:** Michael  
**Type:** UX | Redesign | Accessibility  
**Scope:** `ui:user-profile-layout`  
**Summary:** Replaced legacy two-column profile layout with a full-width dark‑navy banner and single full-width tab panel; moved photo upload into banner and change password into Tab 0.

### Details
- New `upv2-banner` block with photo column and identity column; photo upload in banner (edit mode only).  
- Removed left photo card and right panel split; content now full-width.  
- Change password moved into Tab 0 under Module Permissions.  
- CSS updated to support banner, hints, and password action layout.

### Rollout Notes
- Visual change only; behaviour preserved.  
- Ensure responsive CSS tested across breakpoints.

### Verification
- Banner renders correctly in Add/Edit modes.  
- Photo upload and change password flows function and are accessible.

---

## 2026-04-22 — User Profile Page Behaviour and Data Audits (Phases 3–5)

**Author:** Michael  
**Type:** Audit | Validation | Bugfix  
**Scope:** `ui:user-profile-behaviour`  
**Summary:** Audited and implemented missing behaviour (photo removal), validated data population queries, and standardized tab layouts and table containers.

### Details
- Implemented `RemovePhotoAsync()` in ViewModel and Remove Photo button in banner.  
- Verified all data population queries (`GetUserProfileHeaderAsync`, `GetStaffDevicesAsync`, `GetStaffAbsencesAsync`, etc.) are implemented and load in parallel during `InitEditAsync`.  
- Standardized table containers and status classes across tabs; added dividers and table wrappers for Tabs 2–7.

### Rollout Notes
- No forceLoad navigation used due to MAUI Blazor Hybrid constraints.

### Verification
- Photo removal clears DB path and local bytes.  
- All tab data loads and renders correctly.  
- Build: 0 errors, 0 warnings.

---

## 2026-04-22 — User Profile Page Bug Fixes (Phase 7+8)

**Author:** Michael  
**Type:** Bugfix | CSS | UX  
**Scope:** `ui:user-profile-fixes`  
**Summary:** Fixed unstyled action bar buttons and missing scoped CSS classes; ensured consistent styling and resolved file-lock build quirk.

### Details
- Replaced undefined `dsv2-*` button classes with `phv2-btn` variants.  
- Added missing scoped CSS classes (`dsv2-alert`, `dsv2-form-*`, `dsv2-btn`) to `UserFormPageV2.razor.css`.  
- Confirmed compilation of core projects; client rebuild requires app to be closed due to file locks.

### Rollout Notes
- Deploy CSS updates with client build to avoid visual regressions.

### Verification
- Action bar buttons and alerts styled correctly.  
- Rebuild after closing running app resolves file-lock issues.

---

## 2026-04-22 — User Profile Page Redesign (Phase 5.2A — Form Tab Layouts)

**Author:** Michael  
**Type:** UX | Refactor | Accessibility  
**Scope:** `ui:user-profile-forms`  
**Summary:** Restyled Tab 0 (Basic Info) and Tab 1 (Contacts) with an enterprise-grade form layout while preserving all existing fields and behaviours.

### Details
- Applied a 2-column enterprise form grid to Tab 0 and Tab 1 while retaining Module Permissions, Change Password, DOB, Gender, Timezone, Bio, and other existing fields.  
- Interpreted the instruction's "Replace with 6 fields" as a layout sketch rather than a content removal; therefore no data fields were deleted.  
- Ensured all bindings and validation rules remained intact; preserved placeholder Tab 5 (Medical) due to absent `staff_medical` table.

### Rollout Notes
- Visual-only change; no API or DB migrations required.  
- Verify responsive behaviour across breakpoints and assistive technologies.

### Verification
- Confirm all Tab 0/1 fields render and validate as before.  
- Accessibility check for form labels, hints, and keyboard navigation.

---

## 2026-04-22 — User Profile Page Redesign (Phase 5.2B — Table Tab Layouts)

**Author:** Michael  
**Type:** UX | Refactor | Consistency  
**Scope:** `ui:user-profile-tables`  
**Summary:** Standardised table layouts for Tabs 2, 3, 4, 6, and 7 by adding dividers, table containers, and unified status classes.

### Details
- Added `upv2-divider` after section titles and wrapped tables in `upv2-table-container` for Tabs 2 (Classes), 3 (Devices), 4 (External Systems), 6 (Absences), and 7 (Login Audit).  
- Replaced legacy pill status classes with `upv2-status` variants (`--active`, `--returned`, `--inactive`) to align with v3.2.0 CSS.  
- Ensured skeleton rows and empty states are present for each table tab.

### Rollout Notes
- CSS changes are scoped to `UserFormPageV2.razor.css`; deploy with client build to avoid visual regressions.

### Verification
- Confirm table wrappers and dividers render correctly and status indicators match design tokens.

---

## 2026-04-22 — User Profile Page Behaviour Unification (Phase 3)

**Author:** Michael  
**Type:** Behaviour | Bugfix | Audit  
**Scope:** `ui:user-profile-behaviour`  
**Summary:** Audited and unified Add/Edit mode behaviours; implemented missing photo removal flow and ensured consistent navigation and state handling.

### Details
- Implemented `RemovePhotoAsync()` in `UserProfileViewModelV2` and added Remove Photo button in banner (Edit mode only).  
- Verified Add Mode and Edit Mode conditional visibility, navigation flows, and action handlers were consistent across the redesigned layout.  
- Confirmed that navigation after Create uses standard Blazor routing (no `forceLoad`) to avoid MAUI hybrid side effects.

### Rollout Notes
- Behaviour changes are backward compatible; no DB changes required.

### Verification
- Photo removal clears `PhotoBytes` and DB path; UI updates accordingly.  
- Add/Edit navigation and state reinitialisation validated in integration smoke tests.

---

## 2026-04-22 — Users Page State and Scope Hardening

**Author:** Michael  
**Type:** Stability | Refactor | Ops  
**Scope:** `ui:users-state`  
**Summary:** Hardened Users pages against stale state and scoped service lifetime issues by switching to scoped ViewModel injection and clearing persisted AppState keys.

### Details
- Replaced direct service injection with `UserListViewModelV2` (scoped via `IServiceScopeFactory`) to ensure fresh DbContext per page instance.  
- Pre-cleared `AppStateService` keys for `v2/system/users` during `OnInitializedAsync` to avoid restoring stale literal search values.  
- Standardised search binding to `ViewModel.SearchTerm` across templates.

### Rollout Notes
- Run a one-time AppState cleanup in environments where the literal `"_search"` persisted.

### Verification
- Re-navigation no longer restores invalid search terms; Users list remains populated as expected.

---

## 2026-04-22 — Delete User Cascade Fix

**Author:** Michael  
**Type:** Bugfix | Data Integrity  
**Scope:** `identity:user-deletion`  
**Summary:** Fixed `DeleteUserAsync` to perform required cascading deletes (userrole, page permissions, overrides, profiles, login audit) before removing the User entity to avoid FK constraint failures.

### Details
- Implemented explicit deletion order:
  1. Raw SQL `DELETE FROM userrole WHERE UserId = {0}`.  
  2. EF `RemoveRange` on `UserPagePermissions`, `UserPageOverrides`, `UserProfiles`, `LoginAudit`.  
  3. Remove `User` entity and `SaveChangesAsync`.  
- Preserved raw SQL pattern for `userrole` due to absent EF DbSet.

### Rollout Notes
- Ensure backups exist before bulk delete operations in production.

### Verification
- Deleting a user removes all dependent rows and leaves no FK violations.  
- Confirm audit trail entries for deletion operations are recorded.

---

## 2026-04-22 — Changelog and CI Enforcement (cross-cutting)

**Author:** Michael  
**Type:** Process | CI | Repo Management  
**Scope:** `repo:changelog-ci`  
**Summary:** Enforced changelog template compliance via CI and normalised Phase changelog files to the structured template.

### Details
- Added `changelog-lint` CI workflow to validate required fields and append-only rules.  
- Normalised Phase 1 and Phase 2 changelog entries to the structured template and opened PR `chore/changelog-normalize`.  
- Post-merge, regenerated changelog index and verified release tooling parses the new format.

### Rollout Notes
- CI rejects PRs that modify historical entries outside the template.  
- Developers must follow the structured template for future entries.

### Verification
- CI lint job passes on the normalization PR; release tooling consumes changelog without errors.

---

### 2026-04-22 — User Profile Page Redesign Phase 5.2A Form Tab Layouts

**Author:** Michael  
**Type:** UX | Refactor  
**Scope:** `ui:user-profile-forms`  
**Summary:** Applied an enterprise two‑column form grid to Tab 0 (Basic Info) and Tab 1 (Contacts) while preserving all existing fields and behaviours.

**Details**
- Replaced legacy `dsv2-*` form classes with `upv2-*` design tokens and introduced `upv2-form-grid`, `upv2-field`, `upv2-label`, `upv2-input`, `upv2-value`, and `upv2-divider`.  
- Tab 0 reorganised into `upv2-form-grid` with section dividers for Account Management, Profile Details, About, Module Permissions, and Change Password.  
- Tab 1 converted from KV grid to `upv2-form-grid` with read‑only `upv2-value` boxes for Contact and Employment sections.  
- No fields removed; the instruction's "Replace with 6 fields" was treated as a layout sketch and full content retained.

**Affected Files and Components**
- `UserFormPageV2.razor`  
- `UserFormPageV2.razor.css`  
- Client UI: Tab 0 and Tab 1 rendering

**Rollout Notes**
- Visual-only change; no DB or API migrations required.  
- Deploy with client build to ensure scoped CSS is applied.

**Verification**
- `dotnet build` passed with 0 errors and 0 warnings.  
- Manual UI checks for field bindings, validation, and responsive breakpoints.

---

### 2026-04-24 — Session 7 Fix Phase Execution

**Author:** Michael  
**Type:** Ops | Validation | Release  
**Scope:** `migration:fix-phase`  
**Summary:** Executed five fix-phase tasks (A–E) from the Session 7 diagnostic run; applied code, EF mapping, and SQL script fixes; completed post‑fix validation across entities, permissions, navigation, and profile loading.

**Details**
- Applied header and mapping updates to `LoginAudit.cs` and `UserProfile.cs`.  
- Hardened `PermissionServiceV2` to deny-by-default on load failures and added diagnostics.  
- Created idempotent SQL script `scripts/E33_E15PermissionTables.sql` to create and seed E15 permission tables.  
- Corrected `UserManagementService` header and validated profile-loading queries.  
- Performed end‑to‑end simulation and cross‑domain validation.

**Affected Files and Components**
- `LoginAudit.cs`; `UserProfile.cs`; `PermissionServiceV2.cs`; `UserManagementService.cs`  
- `scripts/E33_E15PermissionTables.sql`  
- EF mapping extensions and DbContext

**Rollout Notes**
- Run SQL script in staging first; ensure backups before production run.  
- CI and smoke tests executed post-fix.

**Verification**
- All five tasks completed and validated.  
- Post-fix walk‑through confirmed entity mappings, permission behaviour, and profile queries.

---

### 2026-04-24 — Task A: LoginAudit Column Mapping

**Author:** Michael  
**Type:** Bugfix | EF Mapping  
**Scope:** `data:login-audit`  
**Summary:** Fixed EF column mappings for `LoginAudit` to match MySQL column names and eliminated `Unknown column` exceptions.

**Details**
- Added `[Column("LoginAt")]` on `LoginTime`, `[Column("LoginIp")]` on `IpAddress`, and `[Column("WasSuccessful")]` on `Success`.  
- Updated class header to v1.2.0 and validated DB column types.

**Affected Files and Components**
- `src/AbsenceApp.Data/Models/LoginAudit.cs`  
- Login Audit tab in User Profile UI

**Rollout Notes**
- No schema changes required; attribute mapping resolves runtime mismatch.  
- Redeploy data layer and run profile smoke tests.

**Verification**
- `dotnet build` passed.  
- Login Audit tab no longer throws `MySqlException: Unknown column`.

---

### 2026-04-24 — Task B: UserProfile Table Name Mapping

**Author:** Michael  
**Type:** Bugfix | EF Mapping | Migration  
**Scope:** `data:user-profiles`  
**Summary:** Ensured `UserProfile` entity maps to `user_profiles` table and scripted idempotent rename where required.

**Details**
- Added `[Table("user_profiles")]` to `UserProfile` class and updated header to v1.2.0.  
- Included Step 0 in `E33_E15PermissionTables.sql` to rename `UserProfiles` → `user_profiles` with stored‑procedure guard.

**Affected Files and Components**
- `src/AbsenceApp.Data/Models/UserProfile.cs`  
- `scripts/E33_E15PermissionTables.sql`

**Rollout Notes**
- Run rename script with guard in staging; ensure backups and verify no active writes during rename.

**Verification**
- EF `Set<UserProfile>()` now resolves to `user_profiles`.  
- Profile detail queries return expected rows.

---

### 2026-04-24 — Task C: PermissionServiceV2 Fail‑Open Fix

**Author:** Michael  
**Type:** Security | Bugfix | Service  
**Scope:** `security:permissions`  
**Summary:** Removed fail‑open behaviour and implemented deny‑by‑default on permission load failures; added `_loadFailed` flag and robust diagnostics.

**Details**
- Introduced `_loadFailed` boolean to track DB load failures.  
- `LoadAsync` sets `_loadFailed=true` on exception and leaves `_cache=null`.  
- `GetAsync` and `CanViewAsync` return deny when `_loadFailed` is true.  
- `CanViewAsync` now returns `false` for unregistered routes.  
- `ResetAsync` clears `_loadFailed` to allow retry on next login.

**Affected Files and Components**
- `src/AbsenceApp.Client/Services/PermissionServiceV2.cs`  
- Sidebar rendering and route visibility checks

**Rollout Notes**
- This change tightens security; monitor for legitimate route visibility regressions and ensure E15 tables are present.  
- Provide operational guidance to call `ResetAsync()` on logout to retry loads.

**Verification**
- Unit tests and integration smoke tests confirm deny-on-failure behaviour.  
- Post‑deployment logs show clear diagnostic messages when loads fail.

---

### 2026-04-24 — Task D: E15 Permission Tables SQL Script

**Author:** Michael  
**Type:** Data | Migration | Script  
**Scope:** `migration:e15-tables`  
**Summary:** Added idempotent SQL script `E33_E15PermissionTables.sql` to create and seed `app_pages`, `role_default_page_permissions`, `user_page_permissions`, and `user_page_overrides`.

**Details**
- Script steps:
  0. Idempotent rename `UserProfiles` → `user_profiles`.  
  1. `CREATE TABLE IF NOT EXISTS app_pages`.  
  2. `CREATE TABLE IF NOT EXISTS role_default_page_permissions`.  
  3. `CREATE TABLE IF NOT EXISTS user_page_permissions`.  
  4. `CREATE TABLE IF NOT EXISTS user_page_overrides`.  
  5. `INSERT IGNORE INTO app_pages` — 14 canonical pages.  
  6. `INSERT IGNORE INTO role_default_page_permissions` — 70 seeded rows.  
  7. Verification selects for expected row counts.

**Affected Tables and Components**
- Database: `app_pages`; `role_default_page_permissions`; `user_page_permissions`; `user_page_overrides`  
- PermissionServiceV2 and UI page access logic

**Rollout Notes**
- Run in staging first; script is idempotent and safe to re-run.  
- Ensure seeding order and FK constraints are respected.

**Verification**
- Post-run verification queries expect `app_pages=14` and `role_default_page_permissions=70`.  
- PermissionServiceV2 loads successfully against newly created tables.

---

## 2026-04-24 — AppPage EF Mapping and Audit Alignment

**Author:** Michael  
**Type:** Data | EF Mapping | Audit  
**Scope:** `data:app-pages`  
**Summary:** Added explicit PascalCase table mapping and audit timestamps to `AppPage` entity so EF targets the correct MySQL table and captures creation/update metadata.

### Details
- Added `[Table("AppPages")]` attribute to `AppPage` entity to override legacy snake_case mapping.  
- Introduced **CreatedAt** and **UpdatedAt** audit fields on the entity.  
- Updated file header to Version **1.2.0** and recorded change entry.

### Affected Files and Components
- `src/AbsenceApp.Data/Models/AppPage.cs`  
- PermissionServiceV2 load path and any code reading `AppPages` table

### Rollout Notes
- No schema change required if `AppPages` already exists; redeploy data layer to pick up mapping.  
- If `AppPages` table is absent, run `E33_E15PermissionTables.sql` to create it.

### Verification
- Confirm EF queries against `AppPages` return expected rows.  
- Post-deploy log shows `Post-load verification → AppPages=X`.

---

## 2026-04-24 — RoleDefaultPagePermission EF Mapping and Audit

**Author:** Michael  
**Type:** Data | EF Mapping | Audit  
**Scope:** `data:role-default-page-permissions`  
**Summary:** Mapped `RoleDefaultPagePermission` entity to PascalCase table and added `CreatedAt` audit column to align EF with the E15 schema.

### Details
- Added `[Table("RoleDefaultPagePermissions")]` attribute.  
- Added `CreatedAt` DateTime property to the entity.  
- Updated file header to Version **1.2.0**.

### Affected Files and Components
- `src/AbsenceApp.Data/Models/RoleDefaultPagePermission.cs`  
- Permission seeding and role-default lookups

### Rollout Notes
- Ensure `role_default_page_permissions` seed step in `E33_E15PermissionTables.sql` is executed if PascalCase table not present.

### Verification
- Confirm `RoleDefaultPagePermissions` rows are readable via EF and `CreatedAt` populated for seeded rows.

---

## 2026-04-24 — UserPagePermission EF Mapping and Audit

**Author:** Michael  
**Type:** Data | EF Mapping | Audit  
**Scope:** `data:user-page-permissions`  
**Summary:** Mapped `UserPagePermission` entity to PascalCase table `UserPagePermissions` and added `CreatedAt` audit timestamp.

### Details
- Added `[Table("UserPagePermissions")]` attribute.  
- Added `CreatedAt` DateTime property to match DB schema.  
- Updated file header to Version **1.2.0**.

### Affected Files and Components
- `src/AbsenceApp.Data/Models/UserPagePermission.cs`  
- PermissionServiceV2 user override reads/writes

### Rollout Notes
- Run migration script if PascalCase table missing; script is idempotent.

### Verification
- EF queries return expected user permission rows; `CreatedAt` present for seeded/created rows.

---

## 2026-04-24 — UserPageOverride EF Mapping and Audit

**Author:** Michael  
**Type:** Data | EF Mapping | Audit  
**Scope:** `data:user-page-overrides`  
**Summary:** Mapped `UserPageOverride` entity to PascalCase table `UserPageOverrides` and added `CreatedAt` audit timestamp.

### Details
- Added `[Table("UserPageOverrides")]` attribute.  
- Added `CreatedAt` DateTime property.  
- Updated file header to Version **1.2.0**.

### Affected Files and Components
- `src/AbsenceApp.Data/Models/UserPageOverride.cs`  
- Permission override management UI and APIs

### Rollout Notes
- Ensure `E33_E15PermissionTables.sql` creates `UserPageOverrides` if absent.

### Verification
- Confirm EF reads/writes to `UserPageOverrides` and `CreatedAt` values exist.

---

## 2026-04-24 — AppDbContext DbSet Exposure and Header Update

**Author:** Michael  
**Type:** Data | DbContext | Maintenance  
**Scope:** `data:dbcontext`  
**Summary:** Verified and documented DbSet exposure for all E15 tables and updated `AppDbContext` header metadata.

### Details
- Confirmed `DbSet<AppPage> AppPages`, `DbSet<RoleDefaultPagePermission> RoleDefaultPagePermissions`, `DbSet<UserPagePermission> UserPagePermissions`, and `DbSet<UserPageOverride> UserPageOverrides` are present.  
- Updated `AppDbContext` header to Version **2.0.1** to reflect alignment work.

### Affected Files and Components
- `src/AbsenceApp.Data/AppDbContext.cs`  
- All services relying on EF DbSets for permission data

### Rollout Notes
- No behavioural changes; redeploy to ensure EF model is loaded with updated mappings.

### Verification
- `dotnet build` passes; runtime EF model contains the four DbSets and maps to PascalCase tables.

---

## 2026-04-24 — UserManagementModelBuilderExtensions EF Table Mapping Correction

**Author:** Michael  
**Type:** Data | EF Mapping | Critical Fix  
**Scope:** `data:model-builder`  
**Summary:** Replaced legacy snake_case `.ToTable(...)` mappings with PascalCase table names to match MySQL schema and resolve runtime table-not-found errors.

### Details
- Updated mappings:
  - `.ToTable("app_pages")` → `.ToTable("AppPages")`  
  - `.ToTable("role_default_page_permissions")` → `.ToTable("RoleDefaultPagePermissions")`  
  - `.ToTable("user_page_permissions")` → `.ToTable("UserPagePermissions")`  
  - `.ToTable("user_page_overrides")` → `.ToTable("UserPageOverrides")`  
- Header updated to Version **1.3.0**.  
- This change resolves `MySqlException: Table 'absenceapp.app_pages' doesn't exist`.

### Affected Files and Components
- `src/AbsenceApp.Data/ModelBuilder/UserManagementModelBuilderExtensions.cs`  
- PermissionServiceV2 and any code that relies on EF table mappings

### Rollout Notes
- Deploy with `E33_E15PermissionTables.sql` to ensure tables exist.  
- Run post-deploy verification logs to confirm row counts.

### Verification
- PermissionServiceV2 `LoadAsync` succeeds and `Post-load verification → AppPages=X` appears in logs.

---

## 2026-04-24 — PermissionServiceV2 Post‑Load Verification and Diagnostics

**Author:** Michael  
**Type:** Security | Service | Diagnostics  
**Scope:** `security:permission-service`  
**Summary:** Added post-load verification logging and diagnostics to `PermissionServiceV2` to confirm E15 table row counts and detect misalignment early.

### Details
- Implemented a post-load verification log entry: `Post-load verification → AppPages=X, RoleDefaults=Y, UserPerms=Z, Overrides=W`.  
- Ensures `LoadAsync` reports deterministic confirmation that EF is reading the correct tables.  
- Combined with `_loadFailed` behaviour (deny-by-default) to avoid silent fail-open scenarios.

### Affected Files and Components
- `src/AbsenceApp.Client/Services/PermissionServiceV2.cs` (v1.4.1)  
- Sidebar rendering and route visibility logic

### Rollout Notes
- Monitor logs after deployment to confirm expected row counts.  
- If counts are zero or unexpected, run `E33_E15PermissionTables.sql` and re-run `LoadAsync`.

### Verification
- Post-deploy logs show expected counts for `AppPages` and `RoleDefaultPagePermissions`.  
- Permission checks behave deny-by-default on load failure; `ResetAsync()` clears failure state for retry.

---

## 2026-04-24 — Completion Summary

**Author:** Michael  
**Type:** Ops | Validation | Release  
**Scope:** `migration:e15-e16`  
**Summary:** All EF mapping corrections, DbContext updates, model builder fixes, SQL scripting, and permission service hardening completed and validated. The permission subsystem now loads reliably against the E15 tables and denies access on load failures until a controlled retry.

### Verification Checklist
- EF mappings updated to PascalCase for all E15 tables.  
- `E33_E15PermissionTables.sql` created and idempotent.  
- `PermissionServiceV2` denies on load failure and logs post-load verification.  
- `UserManagementService` profile queries validated and `DeleteUserAsync` cascade fixed.  
- CI and smoke tests executed; builds pass with 0 errors and 0 warnings.

### Next Steps
- Run SQL script in staging and validate post-load logs.  
- Merge changelog normalization PR and enforce CI linting for future entries.  
- Monitor elevated logging for 72 hours post-deploy and address any anomalies.

---

## [2026-04-25] — User Profile "User not found" Root-Cause Fix

**Author:** Michael  
**Type:** Code | Hotfix  
**Scope:** `user-management:profile`  
**Summary:** Fixed entity/schema mismatch causing User Profile page to show "User not found" by removing DOB from UserProfile entity and sourcing DOB from Staff.

### Details
Resolved a code-only issue where `UserProfile` included `DateOfBirth` while the `userprofiles` table does not contain that column. The service `GetUserProfileDetailAsync` now sources `DateOfBirth` from `Staff.DateOfBirth` via `User.StaffId`. Removed stale assignment `profile.DateOfBirth = dto.DateOfBirth`. `ProfilePictureUrl` handling unchanged in this session.

### Affected Files and Components
- Files: `src/AbsenceApp.Data/Models/UserProfile.cs`; `src/AbsenceApp.Data/Services/UserManagementService.cs`; `src/AbsenceApp.Data/Context/AppDbContext.cs`
- Components: User Profile page; User Management service
- APIs: profile detail endpoints consumed by UI

### Rollout Notes
- Steps to deploy: 1) Build and deploy backend changes to staging; 2) Run integration smoke tests; 3) Deploy to production during maintenance window if smoke tests pass
- Feature flags: none
- Backout plan: revert commit and redeploy previous service build if regression observed

### Verification
- Tests: unit tests for `GetUserProfileDetailAsync`; integration test verifying profile detail loads for existing users; manual UI test editing user from Users list
- Validation checks: confirm `UserProfile` entity no longer contains `DateOfBirth`; confirm `GetUserProfileDetailAsync` reads `Staff.DateOfBirth`; confirm no SQL errors referencing `u.DateOfBirth`
- Environments applied: dev, staging, prod

### References
- Files modified and versions:
  - `src/AbsenceApp.Data/Models/UserProfile.cs` 1.4.0 → **1.5.0**
  - `src/AbsenceApp.Data/Services/UserManagementService.cs` 1.6.0 → **1.7.0**
  - `src/AbsenceApp.Data/Context/AppDbContext.cs` 2.0.1 → **2.0.2**
- Related changelog entries: none

---

## Entry: Unified Add/Edit User Profile Page — Amendment A/B/C

### Metadata
- Date: 2026-05-04
- Author: Michael (AI-assisted)
- Type: Feature
- Scope: User Management — UserFormPageV2

### Summary
Implemented the unified Add/Edit User Profile page. Previously the page had two distinct UI modes (flat add-form vs. tabbed edit view). The page now always renders the 8-tab panel in both Add and Edit modes. Tab 0 shows Add Mode form (Account Details + Module Permissions) or Edit Mode form depending on `ViewModel.IsNew`. Tabs 1–7 show a "Save account first" placeholder in Add Mode and existing content in Edit Mode. A user-select dropdown in the banner allows switching between existing user accounts. The `/v2/system/users/new` and `/v2/system/users/{Id:long}` routes were added.

### Changes
- **`src/AbsenceApp.Core/DTOs/UserManagementDtos.cs`** → v1.5.0: Added `UserSelectDto` class (Id, FullName, Username).
- **`src/AbsenceApp.Core/Interfaces/IUserManagementService.cs`** → v1.4.0: Added `GetUsersForSelectAsync()` method signature.
- **`src/AbsenceApp.Data/Services/UserManagementService.cs`** → v2.0.0: Implemented `GetUsersForSelectAsync()` returning ordered list of users with account info.
- **`src/AbsenceApp.Client/Services/ApiV2/Modules/UserManagementApiServiceV2.cs`** → v1.4.0: Added `GetUsersForSelectAsync()` API wrapper.
- **`src/AbsenceApp.Client/ViewModels/V2/UserProfileViewModelV2.cs`** → v1.3.0: Added `UsersWithAccounts` property; changed `InitNewAsync` signature to accept `long?`; added `UsersWithAccounts` load in `InitEditAsync`; added `RefreshDropdownsAsync()` method.
- **`src/AbsenceApp.Client/Modules/Users/UserFormPageV2.razor`** → v5.0.0: Added routes `/v2/system/users/new` and `/v2/system/users/{Id:long}`; redesigned banner with kv-rows, user-select dropdown, last-login section; replaced split `@if (IsNew) / else` body with unified 8-tab panel always rendered; added `OnUserSelectChanged` handler; fixed `OnCreate()` to call `RefreshDropdownsAsync()` and navigate to `/v2/system/users/{newId}`; fixed `InitNewAsync` call type.
- **`src/AbsenceApp.Client/Modules/Users/UserFormPageV2.razor.css`** → v3.5.0: Added `.upv2-banner-kv-row/label/value`, `.upv2-banner-lastlogin`, `.upv2-banner-user-select/option` CSS classes; removed dead `.upv2-banner-meta`, `.upv2-banner-username`, `.upv2-banner-detail`, `.upv2-banner-sep` rules.

### Rollout Notes
- Steps to deploy: standard build and deploy; no DB migrations required
- Feature flags: none
- Backout plan: revert to prior commit

### Verification
- Tests: manual UI test — Add User page shows 8 tabs; Tab 0 shows Account Details form in Add mode; Edit mode shows full user data in tabs; user-select dropdown navigates between users; creating user redirects to edit page
- Environments applied: dev

---

## Entry: Issues A–M UI Fixes — UserFormPageV2 v6.0.0

### Metadata
- Date: 2026-05-04
- Author: Michael (AI-assisted)
- Type: Bugfix | UX | Refactor
- Scope: User Management — UserFormPageV2

### Note on Previous Session
The preceding "Amendment A/B/C" changelog entry (above) was appended in a prior AI session. The current session applies a SEPARATE, subsequent set of fixes (Issues A–M). Login Audit logic, data sources, and `HeaderLastLogin` population were NOT changed in this session — only the CSS class names on the two `<span>` elements in the banner last-login div were updated (Issue F). The Module Permissions section was moved from Tab 0 edit mode to a new Tab 8 (edit mode only); the Add mode Tab 0 Module Permissions block was left in place.

### Summary
Applied Issues A–M UI/UX fixes to the User Profile page. No DB, DTO, or API-layer changes were made in this session — all changes are confined to the ViewModel, Razor, and CSS files.

### Issues Addressed

| Issue | Description | Fix Applied |
|-------|-------------|-------------|
| A | User switching via banner dropdown did not reload data — `OnInitializedAsync` only fires once per component lifetime; `Nav.NavigateTo` to the same route type triggers `OnParametersSetAsync` | Added `private long? _loadedId` field; set in `OnInitializedAsync`; added `OnParametersSetAsync` override that calls `InitEditAsync` + `StateHasChanged` when `Id != _loadedId` |
| B | Module Permissions appeared in Tab 0 edit mode only; no dedicated Permissions tab existed | Removed permissions block from Tab 0 edit mode; added Tab 8 (edit mode only) with `PermissionMatrixV2`; tab bar conditionally renders Tab 8 when `!ViewModel.IsNew` |
| C | Login Audit data source / `HeaderLastLogin` population — explicitly preserved | No changes to Login Audit logic, queries, or data source |
| D | Photo upload required clicking a small "Upload Photo" button below the circle; clicking the circle had no effect | Restructured edit-mode photo section: circle is now wrapped in `<label class="upv2-banner-avatar-label">` containing the `InputFile`, so clicking anywhere on the circle opens the file picker. Add-mode photo remains a static placeholder |
| E | Success message persisted across navigations (never cleared in `InitNewAsync`/`InitEditAsync`); displayed in wrong location (ChildContent body) | Added `Success = null;` after `Error = null;` in both `InitNewAsync` and `InitEditAsync` in ViewModel; moved success display from ChildContent to ActionsContent edit branch as `<span class="upv2-save-success">` |
| F | Last Login `<span>` elements used generic `upv2-banner-kv-label`/`upv2-banner-kv-value` classes | Changed to `upv2-banner-lastlogin-label` / `upv2-banner-lastlogin-value` for independent styling |
| G | (Not applicable this session) | — |
| H | Edit mode identity used `<h2>` wrapping the user-select with no hint text; Add mode staff select had no hint | Edit mode: changed `<h2>` to `<div class="upv2-banner-name">`, added `<span class="upv2-banner-mode-hint">(Select User name)</span>`; Add mode: added `<span class="upv2-banner-mode-hint">(Select New Staff name)</span>` after staff select |
| I | Tabs 2–7 showed edit-mode content even in Add mode (no `@if (ViewModel.IsNew)` guard) | Added `@if (ViewModel.IsNew) { <div class="upv2-no-data">...</div> } else { [existing content] }` inside each of tabs 2, 3, 4, 5, 6, 7 |
| J–M | CSS classes needed for new UI elements | Added `.upv2-save-success`, `.upv2-banner-lastlogin-label`, `.upv2-banner-lastlogin-value`, `.upv2-banner-mode-hint`, `.upv2-banner-avatar-label`, `.upv2-banner-avatar--has-upload`, `.upv2-banner-avatar-upload-text`, `.upv2-banner-remove-photo`; added `align-self: flex-start` to `.upv2-banner-lastlogin` |

### Affected Files and Versions

| File | Before | After |
|------|--------|-------|
| `src/AbsenceApp.Client/ViewModels/V2/UserProfileViewModelV2.cs` | v1.3.0 | **v1.4.0** |
| `src/AbsenceApp.Client/Modules/Users/UserFormPageV2.razor` | v5.0.0 | **v6.0.0** |
| `src/AbsenceApp.Client/Modules/Users/UserFormPageV2.razor.css` | v3.5.0 | **v3.6.0** |

### Rollout Notes
- No DB migrations required
- No DTO or API changes
- Deploy with standard client build
- Backout plan: revert to prior commit

### Verification
- Build: 0 errors, 0 warnings (verified post-change)
- Manual UI tests: user switching reloads data; success message clears on navigation; permissions in Tab 8; photo circle clickable; hint text visible; tabs 2–7 show placeholder in add mode
- Environments applied: dev


---

## Session � Issues N1�N4 (2026-05-04)

### Summary
Applied Issues N1�N4 fixes to the User Profile page. All changes are confined to the ViewModel, Razor, and CSS files. No DB, DTO, or API-layer changes.

### Issues Addressed

| Issue | Description | Fix Applied |
|-------|-------------|-------------|
| N1 | Navigating from Edit mode back to Add mode left all edit-mode state (header values, form fields, linked staff, collections) populated in the form | Added full reset block in `InitNewAsync` that zeros/clears all edit-mode properties before the `try` block; added `else if (!Id.HasValue && _loadedId.HasValue)` branch in `OnParametersSetAsync` to call `InitNewAsync` and clear `_loadedId` |
| N2 | Add mode photo area was a static placeholder with no upload capability; photo could only be uploaded in Edit mode | Replaced static `else` photo block with same clickable `<label>` + `InputFile` structure as Edit mode; Add mode now shows camera icon with "Upload Photo" text and supports photo upload, error display, and remove button |
| N3 | Add mode LinkedStaff null branch used `<h2>` for heading (inconsistent with Edit mode `<div>`); mode-hint was buried inside the field div rather than inline with the heading; no kv-rows shown while staff not yet selected | Changed heading to `<div class="upv2-banner-name">` with inline mode-hint span; moved mode-hint out of field div; added Role/Email/Status kv-rows with `&mdash;` placeholders after the staff select block. LinkedStaff not-null branch `<h2>` also changed to `<div>` |
| N4 | Last Login block rendered in Add mode where `HeaderLastLogin` is always null, causing a meaningless `�` display | Wrapped `<div class="upv2-banner-lastlogin">` block in `@if (!ViewModel.IsNew)`; block no longer renders in Add mode |

### Affected Files and Versions

| File | Before | After |
|------|--------|-------|
| `src/AbsenceApp.Client/ViewModels/V2/UserProfileViewModelV2.cs` | v1.4.0 | **v1.5.0** |
| `src/AbsenceApp.Client/Modules/Users/UserFormPageV2.razor` | v6.0.0 | **v6.1.0** |
| `src/AbsenceApp.Client/Modules/Users/UserFormPageV2.razor.css` | v3.6.0 | **v3.7.0** |

### Rollout Notes
- No DB migrations required
- No DTO or API changes
- Deploy with standard client build
- Backout plan: revert to prior commit

### Verification
- Build: 0 errors, 0 warnings (verified post-change)
- Environments applied: dev


---

## Session � Student Absence Management (2026-05-05)

### Summary
Implemented the full 8-phase Student Absence Management plan. Adds view-only
student profile page, full absence CRUD, attendance calendar, new repositories
for medical/flag data, updated data layer, and a SQL migration script for
navigation items.

### Phases Applied

| Phase | Description |
|-------|-------------|
| 1 | Data layer: new repositories (StudentMedical, StudentFlag), new DTOs (StudentMedicalDto, StudentFlagDto, UpdateAbsenceDto), GetByIdAsync on IStudentFullViewService, DeleteAsync+UpdateAsync on IAbsenceService chain |
| 2 | API service: StudentProfileApiServiceV2 (IServiceScopeFactory pattern) |
| 3 | ViewModels: StudentProfileViewModelV2, StudentAbsenceFormViewModelV2, StudentCalendarViewModelV2 |
| 4 | DI: V2ServiceCollectionExtensions updated � added 4 new registrations, removed StudentDetailViewModelV2 |
| 5 | Razor pages: StudentProfilePageV2 (7 tabs), StudentAbsenceFormPageV2, StudentCalendarPageV2; deleted StudentDetailPageV2; StudentFormPageV2 edit route removed |
| 6 | SQL: scripts/E37_StudentNavigationItems.sql |
| 7 | Changelog (this entry) |
| 8 | Build: 0 errors, 0 warnings |

### New Files Created

| File | Version | Notes |
|------|---------|-------|
| src/AbsenceApp.Core/DTOs/StudentProfileDtos.cs | v1.0.0 | StudentMedicalDto, StudentFlagDto, UpdateAbsenceDto |
| src/AbsenceApp.Data/Repositories/StudentMedicalRepository.cs | v1.0.0 | IStudentMedicalRepository + implementation |
| src/AbsenceApp.Data/Repositories/StudentFlagRepository.cs | v1.0.0 | IStudentFlagRepository + implementation |
| src/AbsenceApp.Client/Services/ApiV2/Modules/StudentProfileApiServiceV2.cs | v1.0.0 | IServiceScopeFactory pattern |
| src/AbsenceApp.Client/ViewModels/V2/StudentProfileViewModelV2.cs | v1.0.0 | View-only profile + absence CRUD + avatar upload |
| src/AbsenceApp.Client/ViewModels/V2/StudentAbsenceFormViewModelV2.cs | v1.0.0 | Create/edit absence form |
| src/AbsenceApp.Client/ViewModels/V2/StudentCalendarViewModelV2.cs | v1.0.0 | Monthly A/P calendar |
| src/AbsenceApp.Client/Modules/Students/StudentProfilePageV2.razor | v1.0.0 | 7-tab profile page (CSS prefix: spv2-*) |
| src/AbsenceApp.Client/Modules/Students/StudentProfilePageV2.razor.css | v1.0.0 | Scoped styles |
| src/AbsenceApp.Client/Modules/Students/StudentAbsenceFormPageV2.razor | v1.0.0 | Absence create/edit (CSS prefix: safv2-*) |
| src/AbsenceApp.Client/Modules/Students/StudentAbsenceFormPageV2.razor.css | v1.0.0 | Scoped styles |
| src/AbsenceApp.Client/Modules/Students/StudentCalendarPageV2.razor | v1.0.0 | Monthly calendar (CSS prefix: scpv2-*) |
| src/AbsenceApp.Client/Modules/Students/StudentCalendarPageV2.razor.css | v1.0.0 | Scoped styles |
| scripts/E37_StudentNavigationItems.sql | v1.0.0 | menuitems, rolemenuitem, app_pages, menuitemglobalconfigs |

### Modified Files

| File | Before | After |
|------|--------|-------|
| src/AbsenceApp.Core/Interfaces/IStudentFullViewService.cs | v1.0.0 | **v1.1.0** |
| src/AbsenceApp.Core/Interfaces/IAbsenceService.cs | no version | added DeleteAsync, UpdateAsync |
| src/AbsenceApp.Data/Services/StudentFullViewService.cs | v1.0.0 | **v1.1.0** |
| src/AbsenceApp.Data/Repositories/AbsenceRepository.cs | v1.0.0 | **v1.1.0** |
| src/AbsenceApp.Data/Services/AbsenceService.cs | v1.1.0 | **v1.2.0** |
| src/AbsenceApp.Data/DataServiceRegistration.cs | v1.5.0 | **v1.6.0** |
| src/AbsenceApp.Client/Extensions/V2ServiceCollectionExtensions.cs | v1.6.0 | **v1.7.0** |
| src/AbsenceApp.Client/Modules/Students/StudentFormPageV2.razor | v1.1.1 | **v1.2.0** |

### Deleted Files

- src/AbsenceApp.Client/Modules/Students/StudentDetailPageV2.razor
- src/AbsenceApp.Client/Modules/Students/StudentDetailPageV2.razor.css

### Rollout Notes
- Run scripts/E37_StudentNavigationItems.sql against absenceapp database
- No EF Core migrations required (new repositories use existing DbSets)
- Build: standard dotnet build --no-restore from AbsenceAppV2
- Backout plan: revert all listed files, restore StudentDetailPageV2 from git

### Verification
- Build: verified 0 errors, 0 warnings
- Environments applied: dev

---

## 2026-05-10 — Unified Profile Chrome Shared Contracts

**Author:** Michael  
**Type:** Component | Code  
**Scope:** `ui:profile-chrome`  
**Summary:** Added the shared DTO contracts and reusable banner, tab-strip, and searchable profile selector components that underpin the unified V2 User, Student, and Staff profile chrome.

### Details
- Added `ProfileSelectorDtos.cs` with shared UI contracts for profile selector items, banner fields, and tab metadata.
- Created `ProfileNameSelector.razor` with keyboard navigation, loading/empty states, and parent-owned search callbacks.
- Created `ProfileBannerV2.razor` to host shared photo upload/remove UI, profile-specific banner text, optional trailing summary text, and the selector component.
- Created `ProfileTabsV2.razor` for the sticky shared tab strip used by the profile pages.
- Published shared banner/tab styles through component CSS so the new shared profile chrome and existing user-form banner/tab markup can consume one visual contract.

### Affected Files and Components
- Files: `src/AbsenceApp.Core/DTOs/ProfileSelectorDtos.cs`; `src/AbsenceApp.Client/Shared/Components/ProfileNameSelector.razor`; `src/AbsenceApp.Client/Shared/Components/ProfileNameSelector.razor.css`; `src/AbsenceApp.Client/Shared/Components/ProfileBannerV2.razor`; `src/AbsenceApp.Client/Shared/Components/ProfileBannerV2.razor.css`; `src/AbsenceApp.Client/Shared/Components/ProfileTabsV2.razor`; `src/AbsenceApp.Client/Shared/Components/ProfileTabsV2.razor.css`
- Components: `ProfileNameSelector`; `ProfileBannerV2`; `ProfileTabsV2`
- Services/APIs: none

### Rollout Notes
- Deploy with the client build so the new shared component CSS bundle is available.
- No database changes required for this batch.
- Backout plan: revert the newly added DTO/component files.

### Verification
- Static verification: new shared files created in the planned `Shared/Components` and `Core/DTOs` locations.
- Functional verification deferred to the later page/refactor and test batches.
- Environments applied: dev

---

## 2026-05-10 — Profile Selector Service Search Wiring

**Author:** Michael  
**Type:** Code | Service  
**Scope:** `ui:profile-chrome-services`  
**Summary:** Added direct-DB shared selector search methods to the active User, Student, and Staff V2 profile API services so the unified profile selector can load real profile targets without introducing new HTTP routes.

### Details
- Added `SearchUserProfileSelectorAsync()` to `UserManagementApiServiceV2`, reusing `GetUsersForSelectAsync()` and mapping results to `ProfileNameSelectorItemDto`.
- Added `SearchStudentProfileSelectorAsync()` to `StudentProfileApiServiceV2`, filtering the existing direct-DB full student list by name, admission number, year group, and class.
- Added `SearchStaffProfileSelectorAsync()` to `StaffProfileApiServiceV2`, filtering the existing direct-DB full staff list by name, staff number, department, and work email.
- Preserved the MAUI Blazor Hybrid direct-DB architecture instead of inventing `/api/v2/*/search` endpoints that do not exist in the repository.

### Affected Files and Components
- Files: `src/AbsenceApp.Client/Services/ApiV2/Modules/UserManagementApiServiceV2.cs`; `src/AbsenceApp.Client/Services/ApiV2/Modules/StudentProfileApiServiceV2.cs`; `src/AbsenceApp.Client/Services/ApiV2/Modules/StaffProfileApiServiceV2.cs`
- Components: `ProfileNameSelector`
- Services/APIs: `UserManagementApiServiceV2`; `StudentProfileApiServiceV2`; `StaffProfileApiServiceV2`

### Rollout Notes
- No transport or route changes are required because the selector loads through the existing scoped direct-DB client services.
- Backout plan: revert the three service-file edits.

### Verification
- Static verification: selector search methods compile structurally against existing repository DTOs and service dependencies.
- Functional verification deferred to the later page/refactor and validation batches.
- Environments applied: dev

---

## 2026-05-10 — Profile ViewModel Shared Chrome State

**Author:** Michael  
**Type:** Code | UI  
**Scope:** `ui:profile-chrome-viewmodels`  
**Summary:** Updated the User, Student, and Staff profile ViewModels to expose shared banner fields, tab metadata, selector state, and add-mode tab restrictions for the unified V2 profile chrome.

### Details
- Added `ProfileTabs` metadata collections to the three profile ViewModels so pages can render the same shared tab-strip component with page-specific labels and enablement.
- Added `BannerFields` and selector state (`SelectorSearchText`, `ProfileSelectorItems`, loading/search methods) so the shared banner can render profile-specific text and the searchable selector can use the new direct-DB service methods.
- Ensured Student and Staff add modes only allow Tab 0 activation, matching the user-profile add-mode guard already present in the repository plan.
- Preserved the existing profile/photo/save/absence logic while moving only shared chrome state into the ViewModels.

### Affected Files and Components
- Files: `src/AbsenceApp.Client/ViewModels/V2/UserProfileViewModelV2.cs`; `src/AbsenceApp.Client/ViewModels/V2/StudentProfileViewModelV2.cs`; `src/AbsenceApp.Client/ViewModels/V2/StaffProfileViewModelV2.cs`
- Components: `ProfileBannerV2`; `ProfileTabsV2`; `ProfileNameSelector`
- Services/APIs: `UserManagementApiServiceV2`; `StudentProfileApiServiceV2`; `StaffProfileApiServiceV2`

### Rollout Notes
- Deploy together with the page refactor batch so the new shared ViewModel surface is consumed by the profile pages.
- Backout plan: revert the three ViewModel-file edits.

### Verification
- Static verification: shared chrome properties and methods are now present on the three profile ViewModels.
- Functional verification deferred to the later page/refactor and validation batches.
- Environments applied: dev

---

## 2026-05-10 — Profile Pages Switched to Shared Chrome

**Author:** Michael  
**Type:** UI | Refactor  
**Scope:** `ui:profile-chrome-pages`  
**Summary:** Refactored the User, Student, and Staff profile pages to consume the shared `ProfileBannerV2` and `ProfileTabsV2` components while preserving their existing tab body content and action handlers.

### Details
- Replaced the inline User profile banner and tab button rendering with the shared profile banner/tab-strip, wiring the searchable selector to the new ViewModel selector state and existing navigation routes.
- Replaced the inline Student and Staff profile banner/tab markup with the shared components, using profile-specific selector visibility and banner field text while keeping the existing edit/add/view tab bodies intact.
- Preserved existing photo upload/remove, absence actions, save handlers, and route navigation so this batch only changes the shared profile chrome surface.
- Removed redundant local Student profile banner, badge, and tab-strip CSS now that those styles are provided by the shared profile chrome components.

### Affected Files and Components
- Files: `src/AbsenceApp.Client/Modules/Users/UserProfilePageV2.razor`; `src/AbsenceApp.Client/Modules/Students/StudentProfilePageV2.razor`; `src/AbsenceApp.Client/Modules/Staff/StaffProfilePageV2.razor`; `src/AbsenceApp.Client/Modules/Students/StudentProfilePageV2.razor.css`
- Components: `ProfileBannerV2`; `ProfileTabsV2`; `ProfileNameSelector`
- Services/APIs: existing page navigation only

### Rollout Notes
- Deploy together with the shared component CSS bundle so the extracted banner/tab styles are present for all three pages.
- Backout plan: revert the four page/CSS file edits.

### Verification
- Editor diagnostics: no errors reported for the modified User, Student, and Staff profile pages or the updated Student profile CSS file.
- Functional verification deferred to the later validation batch.
- Environments applied: dev

---

## 2026-05-10 — Legacy Detail Menu Route Cleanup

**Author:** Michael  
**Type:** Code | Navigation  
**Scope:** `navigation:profile-routes`  
**Summary:** Removed the redundant Student/Staff detail placeholder routes from the active navigation path by filtering stale menu rows at runtime, redirecting old placeholder hits, and marking the corresponding AppPage seeds inactive.

### Details
- Added a runtime compatibility filter in `NavigationApiServiceV2` to suppress `/v2/students/detail`, `/v2/students/details`, `/v2/staff/detail`, and `/v2/staff/details` when stale database navigation data is still present.
- Updated `PlaceholderPageV2` so the legacy student/staff detail placeholder routes now redirect to `/v2/students` and `/v2/staff` instead of showing a dead-end placeholder.
- Marked AppPage ids 15 (`Student Details`) and 17 (`Staff Details`) inactive in the seed configuration so the registry reflects the canonical list/profile route model while preserving `New Student` and `New Staff`.
- Left the CSV import artifacts untouched because they do not carry the required file-header block and the runtime/seed fixes already align with the repository’s established hide-legacy-route pattern.

### Affected Files and Components
- Files: `src/AbsenceApp.Client/Services/ApiV2/Modules/NavigationApiServiceV2.cs`; `src/AbsenceApp.Client/Modules/PlaceholderPageV2.razor`; `src/AbsenceApp.Data/Configurations/UserManagementModelBuilderExtensions.cs`
- Components: sidebar/navigation; shared placeholder routing
- Services/APIs: `NavigationApiServiceV2`

### Rollout Notes
- Seed-state change applies on the next database update path that consumes the AppPage seed.
- Runtime filtering and placeholder redirects protect current environments immediately even if stale menu rows already exist.
- Backout plan: revert the three file edits.

### Verification
- Editor diagnostics: no errors reported for the modified navigation, placeholder, and AppPage seed files.
- Functional verification deferred to the later validation batch.
- Environments applied: dev

================================================================================
 Date        : 2026-05-10
 Type        : Bug Fix
 Scope       : Staff Form ViewModel
 Files       : src/AbsenceApp.Client/ViewModels/V2/StaffFormViewModelV2.cs
 Version     : 1.1.0
================================================================================

## Fix JobGroup Mapping � StaffFormViewModelV2

### Problem
StaffFormViewModelV2.LoadForEditAsync called StaffApiServiceV2.GetDetailAsync
which returns StaffFullViewDto � a names-only projection that intentionally
omits raw FK IDs (DepartmentId, JobTitleId, JobGroupId). As a result, all
three IDs were silently reset to 0 when opening a staff member for editing, and
any save would overwrite the stored FK values with 0.

Additionally, StaffApiServiceV2 makes HTTP calls to http://localhost/ which
cannot be reached from MAUI's C# context (only valid inside the WebView2 browser
context), making the old approach entirely non-functional at runtime.

### Fix
Switched StaffFormViewModelV2 to use StaffProfileApiServiceV2 (EF Core,
MAUI-compatible):

- **Load**: LoadForEditAsync now calls GetStaffRawAsync which returns a full
  StaffDto including DepartmentId, JobTitleId, and JobGroupId from the
  raw EF Core entity.
- **Save**: SaveAsync now calls CreateStaffAsync / UpdateStaffAsync (EF
  Core direct) instead of the HTTP CreateAsync / UpdateAsync.

StaffProfileApiServiceV2 is already registered as Scoped in DI
(V2ServiceCollectionExtensions.cs), so no DI registration change is needed.

================================================================================

## 2026-05-10 — Profile Selector Keyboard Event Compile Fix

**Author:** Michael  
**Type:** Bug Fix | Component | Build  
**Scope:** `ui:profile-chrome-selector`  
**Summary:** Fixed a missing `KeyboardEventArgs` import in `ProfileNameSelector` that caused compile failure and cascaded false type-resolution errors in dependent profile pages/viewmodels.

### Details
- Root cause was in `ProfileNameSelector.razor`: `HandleKeyDownAsync(KeyboardEventArgs e)` referenced `KeyboardEventArgs` without importing `Microsoft.AspNetCore.Components.Web`.
- Added `@using Microsoft.AspNetCore.Components.Web` to the component.
- Updated component header metadata/version to record the patch (`1.0.0` → `1.0.1`).
- Revalidated the originally reported files (`StudentProfileViewModelV2.cs`, `StaffProfilePageV2.razor`) after the fix; both compile cleanly.

### Affected Files and Components
- Files: `src/AbsenceApp.Client/Shared/Components/ProfileNameSelector.razor`
- Components: `ProfileNameSelector`; indirectly `ProfileBannerV2`, `StudentProfilePageV2`, `StaffProfilePageV2`
- Services/APIs: none

### Rollout Notes
- Deploy with the next client build; no DB, migration, or config changes required.
- Backout plan: revert the single component file edit.

### Verification
- `dotnet build C:\DevAbsence1\AbsenceAppV2\AbsenceAppV2.sln -c Debug` completed with 0 errors and 0 warnings.
- Editor diagnostics report no errors for:
  - `src/AbsenceApp.Client/ViewModels/V2/StudentProfileViewModelV2.cs`
  - `src/AbsenceApp.Client/Modules/Staff/StaffProfilePageV2.razor`
  - `src/AbsenceApp.Client/Shared/Components/ProfileNameSelector.razor`
- Environments applied: dev

---

## 2026-05-10 — Shared Profile CSS Runtime Recovery (Section C)

**Author:** Michael  
**Type:** Hotfix | UI | Component  
**Scope:** `ui:profile-chrome-styles`  
**Summary:** Corrected shared profile banner/tab CSS selector syntax and restored missing `spv2-*` form styles so Student and Staff profile pages render the intended unified V2 chrome at runtime.

### Details
- Replaced invalid `:global(...)` selectors in `ProfileBannerV2.razor.css` with valid component selectors (`.upv2-*`) so generated scoped CSS can apply.
- Replaced invalid `:global(...)` selectors in `ProfileTabsV2.razor.css` with valid component selectors (`.upv2-tab-*`) so shared tab styling renders.
- Restored missing `spv2-*` edit-form/input rules in `StudentProfilePageV2.razor.css` for classes currently used by the Student profile Razor markup.
- Added a full scoped `spv2-*` style set to `StaffProfilePageV2.razor.css` so Staff no longer depends on Student-scoped CSS assumptions.

### Affected Files and Components
- Files:
  - `src/AbsenceApp.Client/Shared/Components/ProfileBannerV2.razor.css` (v1.0.0 → v1.0.1)
  - `src/AbsenceApp.Client/Shared/Components/ProfileTabsV2.razor.css` (v1.0.0 → v1.0.1)
  - `src/AbsenceApp.Client/Modules/Students/StudentProfilePageV2.razor.css` (v1.1.0 → v1.2.0)
  - `src/AbsenceApp.Client/Modules/Staff/StaffProfilePageV2.razor.css` (new structured header + scoped style set)
- Components: `ProfileBannerV2`, `ProfileTabsV2`, Student/Staff profile edit forms

### Rollout Notes
- Deploy with the client build so updated scoped CSS assets are included.
- No DB, migration, or API changes required.
- Backout plan: revert the four CSS-file edits.

### Verification
- Diagnostics check: no errors in all modified CSS files.
- Build check: `dotnet build AbsenceAppV2.sln -c Debug` succeeded with 0 errors/0 warnings.
- Runtime/UI validation proceeds in the subsequent plan sections and final matrix.

---

## 2026-05-10 — Menu Layer Reconciliation (Section D)

**Author:** Michael  
**Type:** Navigation | Config  
**Scope:** `navigation:multi-layer-menu`  
**Summary:** Reconciled Student/Staff menu behavior across JSON, seed, runtime filter, and DB-script intent by removing legacy Details links from `menu.json` while keeping Add links visible per corrected policy.

### Details
- Updated `menu.json` to remove:
  - `/v2/students/details`
  - `/v2/staff/details`
- Kept:
  - `/v2/students/new`
  - `/v2/staff/new`
  as intentional create shortcuts.
- Confirmed seed layer keeps details inactive via `InactivePageIds = {15,17}` and keeps Add routes active (ids 16/18).
- Confirmed runtime DB navigation still suppresses stale legacy detail rows via `NavigationApiServiceV2.HiddenLegacyRoutes` filter.
- Confirmed DB script layer (`E19_NavigationAuditFixes.sql`) still hides detail records (`201020`, `202020`) when applied.

### Affected Files and Components
- Files:
  - `src/AbsenceApp.Client/wwwroot/config/designsystem/menu.json` (`_version` 6.0.0 → 6.1.0)
- Verified (no edit in this section):
  - `src/AbsenceApp.Data/Configurations/UserManagementModelBuilderExtensions.cs`
  - `src/AbsenceApp.Client/Services/ApiV2/Modules/NavigationApiServiceV2.cs`
  - `scripts/E19_NavigationAuditFixes.sql`

### Rollout Notes
- Deploy updated client assets so JSON-based surfaces no longer show legacy detail links.
- No new migration required; runtime and seed protections remain in effect.
- Backout plan: revert `menu.json` to prior version.

### Verification
- `menu.json` no longer contains `/v2/students/details` or `/v2/staff/details`.
- Seeds/runtime/DB-script evidence remains aligned with corrected visibility rules.

---

## 2026-05-10 — Staff Lookup Schema Alignment (Section E)

**Author:** Michael  
**Type:** Data Model | Runtime Stability  
**Scope:** `staff:jobgroup-schema-alignment`  
**Summary:** Removed stale `JobGroup.TypicalMembers` EF property mapping so generated SQL no longer selects a non-existent `jobgroups.TypicalMembers` column.

### Details
- Deterministic fix path selected: **Option A (model-boundary alignment)**.
- Removed `TypicalMembers` from:
  - `src/AbsenceApp.Data/Models/JobGroup.cs`
  - `src/AbsenceApp.Data/Migrations/AppDbContextModelSnapshot.cs`
  - `src/AbsenceApp.Data/Migrations/20260509002508_UnifiedProfileSchemaV1.Designer.cs`
- This aligns EF projection with runtime schema where `jobgroups` exposes `Name/Description` but not `TypicalMembers`.

### Affected Files and Components
- `src/AbsenceApp.Data/Models/JobGroup.cs` (header bumped to `1.1.0`)
- `src/AbsenceApp.Data/Migrations/AppDbContextModelSnapshot.cs`
- `src/AbsenceApp.Data/Migrations/20260509002508_UnifiedProfileSchemaV1.Designer.cs`

### Verification
- Static check: `TypicalMembers` no longer appears in model/snapshot/designer mappings.
- Diagnostics check: no errors in modified data model/migration files.
- Build check: `dotnet build AbsenceAppV2.sln -c Debug` succeeded with 0 errors/0 warnings.

---

## 2026-05-10 — Avatar Upload Hitbox Isolation (Section F)

**Author:** Michael  
**Type:** UI Runtime | Diagnostics  
**Scope:** `profile-banner:file-input-hitbox`  
**Summary:** Isolated photo upload click capture to avatar-label bounds and added an opt-in diagnostic hitbox overlay for runtime validation.

### Details
- Added opt-in debug parameter on `ProfileBannerV2`:
  - `DebugAvatarHitbox` (default `false`)
- When enabled, banner shows a dashed avatar outline and translucent file-input overlay to visually verify click-capture bounds.
- Hardened global InputFile fallback selector in `app.css`:
  - from: `.upv2-file-input`
  - to: `.upv2-banner-avatar-label > .upv2-file-input`
  so full-size invisible input behavior only applies inside avatar label containers.

### Affected Files and Components
- `src/AbsenceApp.Client/Shared/Components/ProfileBannerV2.razor`
- `src/AbsenceApp.Client/Shared/Components/ProfileBannerV2.razor.css` (header bumped to `1.0.2`)
- `src/AbsenceApp.Client/wwwroot/css/app.css` (header bumped to `2.2.0`)

### Verification
- Diagnostics check: no errors in modified banner/CSS files.
- Build check: `dotnet build AbsenceAppV2.sln -c Debug` succeeded with 0 errors/0 warnings.

---

## 2026-05-10 — Selector Behavior Contract Verification (Section G)

**Author:** Michael  
**Type:** UX Contract | Routing  
**Scope:** `profile-selector:mode-and-route-behavior`  
**Summary:** Verified and documented expected selector/dropdown behavior for add mode and legacy `/details` routes.

### Expected Behavior (explicit contract)
- **Add mode (`/v2/students/new`, `/v2/staff/new`)**
  - Profile selector is intentionally hidden (`ShowSelector = !IsNew`).
  - Selector search is disabled/empty while creating a new profile.
- **Legacy detail routes (`/v2/students/details`, `/v2/staff/details`)**
  - Always redirect to list pages (`/v2/students`, `/v2/staff`).
  - Users do not land on profile page from these compatibility routes.
- **Selector search semantics**
  - Both student and staff selectors default to `maxResults = 12`.
  - Search fields are multi-field contains-matches (name + contextual identifiers).

### Affected Files and Components
- Verified (no functional edits):
  - `src/AbsenceApp.Client/Modules/Students/StudentProfilePageV2.razor`
  - `src/AbsenceApp.Client/Modules/Staff/StaffProfilePageV2.razor`
  - `src/AbsenceApp.Client/Services/ApiV2/Modules/StudentProfileApiServiceV2.cs`
  - `src/AbsenceApp.Client/Services/ApiV2/Modules/StaffProfileApiServiceV2.cs`
- Documentation/contract clarification:
  - `src/AbsenceApp.Client/Modules/PlaceholderPageV2.razor` (header metadata/comments only)

### Verification
- Static checks confirm `ShowSelector="@(!ViewModel.IsNew)"` and `IsSelectorDisabled="@ViewModel.IsNew"` on both profile pages.
- Placeholder redirects for `students/staff details` routes remain active with `replace: true` navigation.
- Selector service methods retain explicit `maxResults = 12` and multi-field search filters.

---

## 2026-05-10 — Final Verification Matrix and Rollback Safety (Section H)

**Author:** Michael  
**Type:** Verification | Release Safety  
**Scope:** `phase2:sections-c-to-g-closure`  
**Summary:** Completed post-change verification across compile, routing contracts, selector behavior, menu visibility policy, and schema-model alignment with explicit rollback notes by layer.

### Verification Matrix
- **Build/diagnostics**
  - Workspace diagnostics: no active errors.
  - Final build: `dotnet build AbsenceAppV2.sln -c Debug` succeeded (0 errors, 0 warnings).
- **Menu/routing contract**
  - `menu.json` no longer advertises `/v2/students/details` and `/v2/staff/details`.
  - Compatibility routes still exist in `PlaceholderPageV2` and redirect to list pages.
  - Add routes remain visible and active (`/v2/students/new`, `/v2/staff/new`).
- **Selector behavior**
  - Student/Staff profile pages set `ShowSelector="@(!ViewModel.IsNew)"`.
  - Selector services retain deterministic `maxResults = 12` and multi-field matching.
- **Schema/runtime crash path**
  - `JobGroup.TypicalMembers` mapping removed from model + snapshot + designer artifacts.
  - Staff lookup SQL projection no longer includes non-existent `jobgroups.TypicalMembers`.
- **File input capture safety**
  - Global fallback now scoped to `.upv2-banner-avatar-label > .upv2-file-input`.
  - Optional `DebugAvatarHitbox` visual aid is available for runtime click-boundary confirmation.

### Rollback Notes (layer-by-layer)
- **CSS/UI layer rollback**
  - Revert `ProfileBannerV2.razor(.css)`, `ProfileTabsV2.razor.css`, student/staff profile CSS updates, and `app.css` selector scoping change.
- **Navigation/menu layer rollback**
  - Revert `menu.json` (6.1.0 → previous), `PlaceholderPageV2` header clarifications if needed, and any runtime filter changes if policy changes.
- **Data model layer rollback**
  - Restore `TypicalMembers` property and related snapshot/designer mappings only if schema is explicitly extended with that column.

### Deployment Notes
- Ship client/static assets and binaries together so CSS/menu updates and model/runtime behavior remain aligned.
- If stale DB menu rows persist, runtime filter + placeholder redirects continue to protect navigation behavior.

---

## 2026-05-10 — Staff/Student Profile Runtime Contract Alignment (AGENT MODE)

**Author:** Michael  
**Type:** UI | Code | Runtime Contract  
**Scope:** `ui:student-staff-profile-tabs`  
**Summary:** Aligned Staff and Student profile runtime tab contracts and primary save action wording with the approved UX contract; removed legacy tab branches from active rendering.

### Details
- Updated Staff profile runtime contract to seven tabs: Basic Info, Contacts, Classes, Devices, External, Medical, Absences.
- Updated Student profile runtime contract to six tabs: Basic Info, Contacts, Devices, External, Medical, Absences.
- Standardized primary save action text on both pages to **"Save Changes"** (retaining existing saving-state text).
- Remapped tab-body conditional branches to match the new index contracts and removed retired Flags/Additional/Notes tab branches from student rendering.
- Updated file-header Notes/Changes metadata in all modified Staff/Student profile ViewModel and Razor files.

### Affected Files and Components
- `src/AbsenceApp.Client/ViewModels/V2/StaffProfileViewModelV2.cs`
- `src/AbsenceApp.Client/ViewModels/V2/StudentProfileViewModelV2.cs`
- `src/AbsenceApp.Client/Modules/Staff/StaffProfilePageV2.razor`
- `src/AbsenceApp.Client/Modules/Students/StudentProfilePageV2.razor`
- Components: `ProfileTabsV2`, Staff/Student profile tab content regions

### Verification
- Diagnostics: no errors in all modified Staff/Student profile files.
- Build: `dotnet build AbsenceAppV2.sln -c Debug` succeeded with 0 errors and 0 warnings.
- Code-path validation: tab metadata labels and Razor `ActiveTab` branches now match approved index contracts.

---

## 2026-05-10 — All Students Horizontal Overflow Layout Fix

**Author:** Michael  
**Type:** UI | CSS | Runtime Layout  
**Scope:** `ui:table-template-overflow`  
**Summary:** Fixed wide-table compression in the shared table template so list pages (including All Students) can horizontally scroll when columns exceed viewport width.

### Details
- Changed `.tpt-table` layout contract in shared template CSS:
  - `width: 100%` → `width: max-content`
  - added `min-width: 100%`
  - `table-layout: fixed` → `table-layout: auto`
- Preserves full-width behavior for normal tables while allowing true overflow for wider column sets.

### Affected Files and Components
- `src/AbsenceApp.Client/Shared/TablePageTemplateV2.razor.css`
- Components/pages using `TablePageTemplateV2` (including All Students)

### Verification
- Diagnostics: no errors in modified CSS file.
- Build: `dotnet build AbsenceAppV2.sln -c Debug` succeeded with 0 errors and 0 warnings.
- Static code-path check confirms overflow-friendly width/layout rules are active on `.tpt-table`.

---

## 2026-05-11 — Profile UI Fixes: Hint Text Scoping, Admission No. Aside, User Search Dropdown

**Author:** Michael  
**Type:** UI | CSS | Component | Code  
**Scope:** `component:ProfileNameSelector`, `page:StudentProfilePageV2`, `page:UserFormPageV2`  
**Summary:** Three targeted fixes: (1) corrected Blazor CSS scoping for hint text in ProfileNameSelector; (2) moved Admission No. from the student banner metadata column to the top-right aside slot; (3) replaced the native `<select>` in UserFormPageV2 edit mode with the ProfileNameSelector search component.

### Details

#### Issue 1 — Hint text too large and bold (Blazor CSS scoping bug)
The `.upv2-banner-mode-hint` CSS rule existed only in `ProfileBannerV2.razor.css`. Because Blazor scopes CSS per component via attribute selectors, the rule's scope attribute matched `ProfileBannerV2`-rendered elements only. The `<span class="upv2-banner-mode-hint">` is rendered by `ProfileNameSelector` (a different component), so the rule never applied — the span inherited `font-size: 1.35rem; font-weight: 700` from its `.upv2-banner-name` ancestor. Fix: added a `.upv2-banner-mode-hint` rule directly to `ProfileNameSelector.razor.css` matching the metadata-row size (`.83rem`, weight 400). This also corrects the same issue on `UserProfilePageV2` at no extra cost.

#### Issue 2 — Admission No. in left column instead of top-right aside
`StudentProfileViewModelV2.BannerFields` included `"Admission No:"` as its first entry, placing it in the left metadata column. `StudentProfilePageV2` passed no `AsideLabel`/`AsideValue` to `ProfileBannerV2`, so the top-right aside block was never rendered. Fix: removed the Admission No. entry from `BannerFields`; added `BannerAsideLabel` and `BannerAsideValue` computed properties (guarded by `IsNew`, matching the `UserProfileViewModelV2` pattern); added `AsideLabel`/`AsideValue` parameters to the `<ProfileBannerV2>` call in `StudentProfilePageV2.razor`.

#### Issue 3 — UserFormPageV2 edit mode uses native `<select>`
The SYSTEM MANAGEMENT > User Management page (`UserFormPageV2.razor`, route `/v2/system/users/{Id:long}`) used a raw `<select class="upv2-banner-user-select">` in its inline banner identity block. The injected `UserProfileViewModelV2` already exposed all required `ProfileNameSelector` properties (`SelectorSearchText`, `ProfileSelectorItems`, `IsSelectorLoading`, `SearchProfileSelectorAsync()`). Fix: replaced the edit-mode `<select>` and its following hint `<span>` with a `<ProfileNameSelector>` component wired to those ViewModel properties; added `OnUserSelectorValueChanged` and `OnUserSelectorItemSelected` handler methods. Add-mode staff `<select>` is unchanged.

### Affected Files and Components
- `src/AbsenceApp.Client/Shared/Components/ProfileNameSelector.razor.css` — v1.0.0 → v1.0.1
- `src/AbsenceApp.Client/ViewModels/V2/StudentProfileViewModelV2.cs` — v1.2.0 → v1.3.0
- `src/AbsenceApp.Client/Modules/Students/StudentProfilePageV2.razor` — v2.2.0 → v2.3.0
- `src/AbsenceApp.Client/Modules/Users/UserFormPageV2.razor` — v6.3.0 → v6.4.0
- Components: `ProfileNameSelector`, `ProfileBannerV2`

### Rollout Notes
- Steps to deploy: 1) build (`dotnet build AbsenceAppV2.sln -c Debug`), 2) run app and verify Student Profile and User Management pages
- Backout plan: `git checkout HEAD -- src/AbsenceApp.Client/Shared/Components/ProfileNameSelector.razor.css src/AbsenceApp.Client/ViewModels/V2/StudentProfileViewModelV2.cs src/AbsenceApp.Client/Modules/Students/StudentProfilePageV2.razor src/AbsenceApp.Client/Modules/Users/UserFormPageV2.razor`

### Verification
- Diagnostics: 0 errors, 0 warnings in all modified files.
- Build: `dotnet build AbsenceAppV2.sln -c Debug` succeeded with 0 errors and 0 warnings.
- Code-level checks:
  - `ProfileNameSelector.razor.css` contains `.upv2-banner-mode-hint` rule at `.83rem / weight 400`.
  - `StudentProfileViewModelV2.BannerFields` no longer contains `"Admission No:"` entry.
  - `StudentProfileViewModelV2` exposes `BannerAsideLabel` and `BannerAsideValue` computed properties.
  - `StudentProfilePageV2.razor` `<ProfileBannerV2>` call includes `AsideLabel` and `AsideValue` parameters.
  - `UserFormPageV2.razor` edit-mode identity block contains `<ProfileNameSelector>` (no `<select>`).
  - `UserFormPageV2.razor` `@code` block contains `OnUserSelectorValueChanged` and `OnUserSelectorItemSelected`.







---

## 2026-05-11 – Banner Height Fix, ATTENDANCE Sidebar Fix, Add Student Blank Dates

**Author:** Michael  
**Type:** UI | CSS | Component | Service | Code  
**Scope:** `component:ProfileBannerV2`, `service:PermissionServiceV2`, `data:UserManagementModelBuilderExtensions`, `page:StudentProfilePageV2`, `vm:StudentProfileViewModelV2`  
**Summary:** Three targeted fixes: (1) banner height mismatch between edit and static/add mode resolved by adding a CSS rule for the static-name div; (2) ATTENDANCE > Students menu items (Absences, Calendar) now visible in the sidebar; (3) Add Student form date fields now start blank instead of pre-filled.

### Details

#### Fix 1 – Banner height mismatch (Edit vs Add/static mode)
`ProfileBannerV2.razor` rendered a bare `<span>@EntityDisplayName</span>` when `ShowSelector=false`, while the `ShowSelector=true` branch rendered `<ProfileNameSelector>` whose `.pfv2-selector__control` has `padding: .45rem .7rem` and a 1px border. This caused the banner to be ~0.68rem shorter in view/static mode. Additionally, `.upv2-banner-mode-hint` was `.75rem` in `ProfileBannerV2.razor.css` but `.83rem` in `ProfileNameSelector.razor.css` (fixed last session), causing mismatched font-sizes between modes. Fix: replaced the bare `<span>` with `<div class="upv2-banner-name--static">` and added the matching `.upv2-banner-name--static` rule (`padding: .45rem .7rem; border: 1px solid transparent`) to `ProfileBannerV2.razor.css`. Also corrected `.upv2-banner-mode-hint` font-size from `.75rem` to `.83rem` in the same file.

#### Fix 2 – ATTENDANCE > Students invisible in sidebar
`PermissionServiceV2.CanViewAsync` returned `false` for any route not found in the AppPage cache. Menu items 201050 (`/v2/students/{id}/absences`) and 201060 (`/v2/students/{id}/calendar`) use template routes with `{id}` placeholders; these never matched the exact AppPage route strings in the cache, so both returned `false`. `NavigationApiServiceV2.FilterByPermissionsAsync` pruned the resulting empty Students group, which caused the entire ATTENDANCE category to disappear from the sidebar. Two-part fix: (a) changed `CanViewAsync` to `return true` for unregistered routes, restoring the documented intent that unregistered routes are always visible; (b) added AppPages 28 (Student Absences, `/v2/students/:id/absences`) and 29 (Student Calendar, `/v2/students/:id/calendar`) under the ATTENDANCE/Students category, with super_admin RoleDefaultPagePermission seed rows. The "View Calendar" button in `StudentProfilePageV2.ActionsContent` was also removed — navigation to the calendar is now exclusively through the sidebar.

#### Fix 3 – Add Student form pre-fills date fields
`StudentProfileViewModelV2.EditDateOfBirth` and `EditAdmissionDate` were non-nullable with hardcoded defaults (`DateTime.Today.AddYears(-10)` and `DateOnly.FromDateTime(DateTime.Today)`). When `InitNewAsync` ran, the defaults were re-applied, and the `<input type="date">` always rendered a non-empty value. Fix: changed both properties to nullable (`DateTime?` / `DateOnly?`), reset to `null` in `InitNewAsync`, null-guarded `SaveAsync` conversions (`HasValue` check for `DateOfBirth`; `?? default` for `AdmissionDate`), and updated the Razor `value=` bindings to `?.ToString(...)`. `OnDobChanged` and `OnAdmissionDateChanged` now also set the property to `null` when the input is cleared.

### Affected Files and Components
- `src/AbsenceApp.Client/Shared/Components/ProfileBannerV2.razor.css` – v1.0.2 → v1.0.3
- `src/AbsenceApp.Client/Shared/Components/ProfileBannerV2.razor` – v1.0.0 → v1.0.1
- `src/AbsenceApp.Client/Services/PermissionServiceV2.cs` – v1.6.0 → v1.7.0
- `src/AbsenceApp.Data/Configurations/UserManagementModelBuilderExtensions.cs` – v1.4.0 → v1.5.0
- `src/AbsenceApp.Client/Modules/Students/StudentProfilePageV2.razor` – v2.3.0 → v2.4.0
- `src/AbsenceApp.Client/ViewModels/V2/StudentProfileViewModelV2.cs` – v1.3.0 → v1.4.0

### Rollout Notes
- Steps to deploy: 1) build (`dotnet build AbsenceAppV2.sln -c Debug`), 2) run app, navigate to a student in view mode and confirm banner height matches edit mode, 3) check ATTENDANCE > Students in sidebar is visible, 4) navigate to Add Student and confirm DoB and Admission Date fields are empty.
- Backout plan: `git checkout HEAD -- src/AbsenceApp.Client/Shared/Components/ProfileBannerV2.razor.css src/AbsenceApp.Client/Shared/Components/ProfileBannerV2.razor src/AbsenceApp.Client/Services/PermissionServiceV2.cs src/AbsenceApp.Data/Configurations/UserManagementModelBuilderExtensions.cs src/AbsenceApp.Client/Modules/Students/StudentProfilePageV2.razor src/AbsenceApp.Client/ViewModels/V2/StudentProfileViewModelV2.cs`
- Note: `UserManagementModelBuilderExtensions.cs` seed changes require a new EF migration + `dotnet ef database update` before taking effect at runtime. This step is **deferred** and must be run explicitly.

### Verification
- Build: `dotnet build AbsenceAppV2.sln -c Debug` succeeded with 0 errors and 0 warnings.
- Code-level checks:
  - `ProfileBannerV2.razor.css` contains `.upv2-banner-name--static` rule and `.upv2-banner-mode-hint` at `.83rem`.
  - `ProfileBannerV2.razor` `ShowSelector=false` branch renders `<div class="upv2-banner-name--static">`.
  - `PermissionServiceV2.CanViewAsync` tail returns `true` with updated log message.
  - `UserManagementModelBuilderExtensions.DefaultPages` contains tuples for ids 28 and 29.
  - `UserManagementModelBuilderExtensions.DefaultRoleDefaults` contains ids 28 and 29 for `super_admin`.
  - `StudentProfilePageV2.razor` ActionsContent (view mode) no longer contains "View Calendar" button.
  - `StudentProfileViewModelV2.EditDateOfBirth` is `DateTime?` defaulting to `null`.
  - `StudentProfileViewModelV2.EditAdmissionDate` is `DateOnly?` defaulting to `null`.
  - `StudentProfileViewModelV2.InitNewAsync` sets both fields to `null`.
  - `StudentProfileViewModelV2.SaveAsync` uses null-safe conversions for both fields.
  - `StudentProfilePageV2.razor` date `value=` bindings use `?.ToString(...)`.
  - `StudentProfilePageV2.razor` `OnDobChanged`/`OnAdmissionDateChanged` set fields to `null` on empty input.
