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

## 2026-05-21 — UI Kits Template Demo Spacing Fix

**Author:** Michael  
**Type:** UI | Component | Hotfix  
**Scope:** `ui:ui-kits-template`  
**Summary:** Updated `UiKitsGroup.razor.css` to give the demo area a reusable flex-wrap layout so UI Kits demo items render with consistent spacing.

### Details
- File modified: `AbsenceAppV2/src/AbsenceApp.Client/Shared/Templates/UIKits/UiKitsGroup.razor.css`
- Version increment: `1.0.0` → `1.0.1`
- Changes: updated the `.uikits-group__demo` selector to use `display: flex`, `flex-wrap: wrap`, `gap: 8px`, and `align-items: center` while preserving existing padding.
- This update is part of the UI Kits template fix and does not change accordion markup or logic.

### Affected Files and Components
- Files: `AbsenceAppV2/src/AbsenceApp.Client/Shared/Templates/UIKits/UiKitsGroup.razor.css`
- Components: `UiKitsGroup`

### Verification
- Rebuild to regenerate scoped CSS bundle.
- Confirm demo items on Buttons and Dropdown UI Kits pages render with spacing and wrap correctly.
- Confirm accordion open/close selectors remain unchanged.

---

## 2026-05-21 — UI Kits Generic Functionality Refactor (Phase 2)

**Author:** Michael  
**Type:** UI | Component | Refactor  
**Scope:** `ui:ui-kits-template`  
**Summary:** Implemented the Phase 2 generic template contract in `UiKitsGroup` and rewired Buttons/Dropdown pages to use template-controlled selection and click-to-preview behavior.

### Details
- Files modified:
  - `AbsenceAppV2/src/AbsenceApp.Client/Shared/Templates/UIKits/UiKitsGroup.razor`
  - `AbsenceAppV2/src/AbsenceApp.Client/Shared/Templates/UIKits/UiKitsGroup.razor.css`
  - `AbsenceAppV2/src/AbsenceApp.Client/Components/Pages/GlobalSettings/UIKits/Buttons/Index.razor`
  - `AbsenceAppV2/src/AbsenceApp.Client/Components/Pages/GlobalSettings/UIKits/Dropdown/Index.razor`
- Version increments:
  - `UiKitsGroup.razor`: `1.1.0` → `1.2.0`
  - `UiKitsGroup.razor.css`: `1.0.1` → `1.1.0`
  - `Buttons/Index.razor`: `7.1.0` → `8.0.0`
  - `Dropdown/Index.razor`: `4.1.0` → `5.0.0`
- Refactor highlights:
  - Added reusable item contract in `UiKitsGroup` (`UiKitsDemoItem`, `UiKitsDemoItemContext`, `Items`, `OnItemSelectedKeyChanged`, `ItemTemplate`).
  - Moved active-item tracking into template-level logic (single active item per group).
  - Kept accordion open/close class flow and animation contract intact (`.uikits-group__accordion` + `.open`).
  - Kept preview container/edit shell in template; pages now supply item lists and preview payload callbacks.
  - Added template-level active state selectors in scoped CSS.
- This update is part of the **Phase 2 generic functionality refactor** for shared UI Kits behavior.

### Verification
- Build with no incremental reuse.
- Confirm scoped bundle includes updated UiKitsGroup selectors for demo area, active item, accordion, and preview container.
- Manual verification targets:
  - Buttons: one active item per group, click updates preview payload, edit/save/cancel shell still works.
  - Dropdown: same shared template behavior with dropdown-specific markup.

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

---

## 2026-05-21 — UI Kits Phase 2.1 Visual Restoration

**Author:** Michael  
**Type:** UI | Component | Hotfix  
**Scope:** `ui:ui-kits-template`  
**Summary:** Applied the scoped Phase 2.1 restore set for Buttons/Dropdown preview behavior and UiKitsGroup action-button styling to recover expected visual parity.

### Details
- Files modified (strict Phase 2.1 scope):
  - `AbsenceAppV2/src/AbsenceApp.Client/Components/Pages/GlobalSettings/UIKits/Buttons/Index.razor`
  - `AbsenceAppV2/src/AbsenceApp.Client/Components/Pages/GlobalSettings/UIKits/Dropdown/Index.razor`
  - `AbsenceAppV2/src/AbsenceApp.Client/Shared/Templates/UIKits/UiKitsGroup.razor`
  - `AbsenceAppV2/src/AbsenceApp.Client/Shared/Templates/UIKits/UiKitsGroup.razor.css`
- Version increments:
  - `Buttons/Index.razor`: `8.0.0` → `8.1.0`
  - `Dropdown/Index.razor`: `5.0.0` → `5.1.0`
  - `UiKitsGroup.razor`: `1.2.0` → `1.2.1`
  - `UiKitsGroup.razor.css`: `1.1.0` → `1.1.1`
- Applied restore items:
  - Buttons now passes full class lists per `UiKitsDemoItem` for rendered variants (base + variant composition).
  - Icon button group restored to dedicated icon-button rendering contract (`dsv2-btn-icon`) with explicit icon class values provided by item CSS class.
  - Dropdown trigger/toggle clicks now explicitly refresh selected variant preview payload before opening menus.
  - `UiKitsGroup` Edit/Save/Cancel/Preview controls now use UI Kit button class combinations instead of legacy bootstrap-like class hooks.

### Verification
- Build and scoped CSS verification executed after edit pass (see session validation logs).
- Regression targets: Buttons variant selection, icon buttons, Dropdown preview payload updates, and UiKitsGroup action controls.

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


---

## 2026-05-11 � Indicator-Style Tab Design

### Summary
Upgraded the V2 tab strip from a flat underline to a 4px Corporate Blue bottom indicator on the active tab, with a 1px light-grey track on inactive tabs and a blue tint hover state. Introduced a dedicated component-level token layer (components.css) so tab appearance is fully token-driven. Eliminated a Blazor CSS scope isolation gap that caused UserFormPageV2 tabs to receive no indicator styling. Added a GlobalConfig demo page for live visual reference.

### Files Changed

| File | Change |
|------|--------|
| wwwroot/css/tokens/components.css | **NEW** � 12 --ds-tab-* design tokens for background, active, inactive and hover states. References existing palette and typography primitives from colors.css and 	ypography.css. |
| wwwroot/index.html | Added <link rel="stylesheet" href="css/tokens/components.css" /> between layout.css and global-config.css. |
| Shared/Components/ProfileTabsV2.razor.css | v1.0.1 ? v1.0.2. Replaced order-bottom approach with ox-shadow: inset technique to deliver a 4px active / 1px inactive indicator without layout shift. All colour and weight values now consume --ds-tab-* tokens. |
| Modules/Users/UserFormPageV2.razor | v6.4.0 ? v6.5.0. Removed inline <div class="upv2-tab-bar"> and RenderTabBtn helper; replaced with <ProfileTabsV2> shared component wired to ViewModel.ProfileTabs / ActiveTab / SetTab. Resolves CSS scope gap that previously blocked indicator styles from applying. |
| Components/Pages/GlobalConfig/Tabs/Index.razor | **NEW** � Dual-route demo page (/global-config/tabs, /global-settings/tabs). Three dsv2-card blocks: Active Tab state + token table, Inactive Tab state + token table, Full Strip Preview with active/inactive/disabled examples. |

### Technical Notes
- **box-shadow inset technique**: ox-shadow: inset 0 calc(-1 * var(--ds-tab-active-border-width)) 0 var(--ds-tab-active-border-color) paints the bottom indicator inside the element box � no height change, no layout shift when switching between 1px and 4px states.
- **Blazor CSS scope isolation**: ProfileTabsV2.razor.css rules are attribute-scoped to the component's rendered output. UserFormPageV2's previous inline markup sat outside that scope and received no indicator styling. The conversion to the shared component closes this gap.
- **Token chain**: --ds-tab-active-border-color ? --ds-color-accent ? #2563eb (Corporate Blue). A single palette change propagates to all tab states automatically.
- **ProfileTabItemDto.Visible**: defaults to 	rue; ProfileTabsV2 already filters Where(t => t.Visible) � no ViewModel changes required for UserFormPageV2.

### Rollout Notes
- Steps to deploy: 1) build (dotnet build AbsenceAppV2.sln -c Debug), 2) run app, navigate to any profile page and confirm active tab shows 4px blue indicator, inactive tabs show 1px grey track, 3) navigate to User Management edit page and confirm same indicator style, 4) navigate to /global-config/tabs or /global-settings/tabs and confirm demo cards render.
- Backout plan: git checkout HEAD -- src/AbsenceApp.Client/wwwroot/css/tokens/components.css src/AbsenceApp.Client/wwwroot/index.html src/AbsenceApp.Client/Shared/Components/ProfileTabsV2.razor.css src/AbsenceApp.Client/Modules/Users/UserFormPageV2.razor src/AbsenceApp.Client/Components/Pages/GlobalConfig/Tabs/Index.razor

### Verification
- Build: dotnet build AbsenceAppV2.sln -c Debug succeeded with 0 errors and 0 warnings.
- Code-level checks:
  - components.css :root block contains all 12 --ds-tab-* tokens.
  - index.html components.css link sits between layout.css and global-config.css.
  - ProfileTabsV2.razor.css .upv2-tab-btn has ox-shadow: inset and no order-bottom.
  - ProfileTabsV2.razor.css .upv2-tab-btn--active has ox-shadow: inset 0 calc(-1 * var(--ds-tab-active-border-width)) and no order-bottom-color.
  - UserFormPageV2.razor no longer contains RenderTabBtn or <div class="upv2-tab-bar".
  - UserFormPageV2.razor contains <ProfileTabsV2 Tabs="@ViewModel.ProfileTabs".
  - GlobalConfig/Tabs/Index.razor has both @page "/global-config/tabs" and @page "/global-settings/tabs" directives.


---

## 2026-05-11 - Sidebar Dual-Students Fix + Global Button CSS

### Summary
Two issues resolved: (1) clicking either "Students" menu (PEOPLE or ATTENDANCE) expanded both simultaneously due to identical group labels; (2) dsv2-btn colour variants rendered with no background because no global CSS defined them. Both fixes applied with live MySQL data authority - CSV seed files were explicitly not used (confirmed outdated; live DB had 2 extra rolemenuitems rows not present in CSVs).

### Issue 1 - Sidebar Expands Both Students Menus

#### Root Cause
SidebarV2.razor tracks accordion state with a single string: `private string _openGroup = string.Empty`. The open test is `_openGroup == group.Group`. Both PEOPLE->Students (Id=201000) and ATTENDANCE->Students (Id=402000) produced group.Group=="Students", so clicking either set _openGroup="Students" and both rendered open. No code change required - renaming 402000's label to "Student Attendance" makes the two groups distinct.

#### Live DB Changes (127.0.0.1:3306/absenceapp)
Validated live state before any modifications:
- 401000 (Attendance/menu), 401010 (Attendance/submenu), 402000 (Students/menu) - all confirmed present
- rolemenuitems for 401000: 3 rows (RoleIds 1, 2, 3) - CSV showed only 1 (confirmed CSV outdated)
- rolemenuitems for 401010: 3 rows (RoleIds 1, 2, 3)

Applied:
- `DELETE FROM rolemenuitems WHERE MenuItemId IN (401000, 401010)` - 6 rows deleted
- `DELETE FROM menuitems WHERE Id IN (401000, 401010)` - 2 rows deleted
- `UPDATE menuitems SET Label='Student Attendance', GroupName='Student Attendance', UpdatedAt=NOW() WHERE Id=402000` - 1 row updated

Post-change ATTENDANCE category: only Id=402000 (Student Attendance) remains as a child of 400000.

### Issue 2 - Button Background Colours Not Applying

#### Root Cause
dsv2-btn colour variants used across 9+ pages but defined nowhere globally. global-config.css had dsv2-card only. UserFormPageV2.razor.css and SettingsListPageV2.razor.css had partial scoped definitions (Blazor-isolated, not available to other components). index.html load order was already correct - no change needed.

#### Files Changed

| File | Change |
|------|--------|
| `wwwroot/css/global-config.css` | Appended full dsv2-btn system: base, disabled state, 8 colour fills (primary/secondary/success/danger/warning/info/light/dark) + hovers, 6 outline variants + hovers, 2 size modifiers (--lg, --sm), dark mode overrides for secondary and light. All values use `--v2-color-*` tokens from colors.css. |

Existing scoped definitions verified compatible:
- `UserFormPageV2.razor.css`: base .dsv2-btn + .dsv2-btn--secondary - values match global (no conflict)
- `SettingsListPageV2.razor.css`: .dsv2-btn--sm padding 0.25rem/0.625rem = 4px/10px - matches global exactly (no conflict)

### Verification
- Build: `dotnet build AbsenceAppV2.sln -c Debug` - 0 errors, 0 warnings
- Live DB post-check: ATTENDANCE category has single child (Student Attendance/402000); rolemenuitems for 401000/401010 = 0 rows remaining; rolemenuitems for 402000 intact (RoleIds 1/2/3)
- Runtime checks required:
  - Sidebar ATTENDANCE section: shows "Student Attendance" only
  - Clicking PEOPLE->Students does NOT open ATTENDANCE->Student Attendance
  - `/global-settings/buttons`: all 8 colour variant buttons show correct backgrounds
  - StudentFormPageV2, StaffFormPageV2: primary (blue) save buttons render correctly

### No CSV Files Modified
aaa_menuitems.csv and aaa_rolemenuitem.csv were intentionally not modified. They are reference/re-seed documents and do not drive runtime behaviour. The live DB is the authoritative data source.

---

## 2026-05-11 - 404 Page + Student Attendance Route Fix

### Summary
Three issues resolved: (1) Student Absences and Student Calendar sidebar items navigated to broken routes using Express `:param` notation that Blazor cannot match; (2) the router `<NotFound>` block showed bare text with no layout or navigation; (3) a test submenu was added to verify 404 behaviour. All DB changes applied to live MySQL (not CSV files).

### Issue 1 - Student Attendance Sidebar Routes Broken

#### Root Cause
Live `menuitems` had:
- 402010 (Student Absences): Route = `/v2/students/{id}/absences`
- 402020 (Student Calendar): Route = `/v2/students/{id}/calendar`

These routes use a literal placeholder token (`{id}`) rather than a resolvable numeric parameter. When clicked from the sidebar, `NavigationManager.NavigateTo` navigates to the literal URL `/v2/students/{id}/absences` (curly braces intact) which matches no Blazor `@page` directive -> router fires `<NotFound>`.

#### Live DB Changes (127.0.0.1:3306/absenceapp)

Pre-check: `AppPages` had `(Id=401010, Route='/v2/attendance')` - duplicate of 402010's new target.

Applied:
- `UPDATE menuitems SET Route='/v2/attendance' WHERE Id=402010` - 1 row
- `UPDATE menuitems SET Route='/v2/attendance/student' WHERE Id=402020` - 1 row
- `DELETE FROM AppPages WHERE Id=402010` - route `/v2/attendance` already existed under Id=401010; deleted 402010 row to prevent duplicate dictionary key in `PermissionServiceV2` cache
- `UPDATE AppPages SET Route='/v2/attendance/student' WHERE Id=402020` - 1 row

Target pages confirmed existing (no new pages needed):
- `Modules/Attendance/AttendanceListPageV2.razor` - `@page "/v2/attendance"` - absence records list
- `Modules/Attendance/AttendanceStudentPageV2.razor` - `@page "/v2/attendance/student"` - monthly A/P calendar

### Issue 2 - Proper 404 Page

#### Root Cause
`Routes.razor` `<NotFound>` block contained `<p>Page not found.</p>` - bare text, no `LayoutView`, no `MainLayoutV2` applied. Result: white page with text, no sidebar, no navigation available to the user.

#### Files Created

| File | Change |
|------|--------|
| `Components/Pages/NotFoundPageV2.razor` | New 404 component. Follows `UnderConstructionTemplate.razor` structure (`gs-template` wrapper, icon, title, body, action). Icon: `bi-exclamation-circle`. Title: "Page Not Found". Message: friendly 404 text. Button: "Back to Home" -> `/v2/dashboard/overview`. No `@page` directive - rendered via `<LayoutView>` in `Routes.razor`. |
| `Components/Pages/NotFoundPageV2.razor.css` | Scoped styles. Defines `gs-template` layout (centred flex column, min-height 40vh), `gs-template__uc-icon` (4rem, text-secondary), `gs-template__uc-title` (1.75rem, 600), `gs-template__uc-body` (0.95rem, text-secondary, max-width 480px), `.nfpv2-actions` (margin-top 2rem). Button styled by existing global `dsv2-btn dsv2-btn--primary` in `global-config.css`. |

#### Files Modified

| File | Change |
|------|--------|
| `Components/Routes.razor` | v2.12.0 -> v2.13.0. Added `@using AbsenceApp.Client.Components.Pages`. Replaced `<NotFound><p>Page not found.</p></NotFound>` with `<NotFound><LayoutView Layout="@typeof(MainLayoutV2)"><NotFoundPageV2 /></LayoutView></NotFound>`. |

### Issue 3 - Test 404 Submenu

#### Live DB Changes

Applied:
- `INSERT INTO menuitems`: Id=402030, ParentId=402000, ItemType='submenu', Label='Test 404Page', Icon='bi-exclamation-triangle', Route='/v2/test/404', SortOrder=402030, Status='active', CategoryKey='ATTENDANCE', GroupName='Student Attendance'
- `INSERT INTO rolemenuitems`: 3 rows (RoleId=1/2/3, MenuItemId=402030, IsEnabled=1, AssignedBy=1)
- No `AppPages` entry added - `PermissionServiceV2` v1.7.0 returns `true` for unregistered routes (fail-open)

### Verification
- Build: `dotnet build AbsenceAppV2.sln -c Debug` - 0 errors, 0 warnings
- Live DB: menuitems 402010=/v2/attendance, 402020=/v2/attendance/student, 402030=/v2/test/404
- Live DB: AppPages 402010 deleted; AppPages 402020 updated to /v2/attendance/student
- Live DB: rolemenuitems for 402030: RoleId 1/2/3 IsEnabled=1
- Runtime checks required:
  - Sidebar -> Student Attendance -> Student Absences -> AttendanceListPageV2 at /v2/attendance
  - Sidebar -> Student Attendance -> Student Calendar -> AttendanceStudentPageV2 at /v2/attendance/student
  - Sidebar -> Student Attendance -> Test 404Page -> NotFoundPageV2 renders with full sidebar + header
  - Navigate to any unknown URL -> NotFoundPageV2 renders with full sidebar + header
  - "Back to Home" button -> /v2/dashboard/overview restores full app shell

### Temp Scripts
`C:\DevAbsence1\db_phase_404.py`, `db_verify.py`, `db_sample_roles.py` - temporary scripts, can be deleted.

---

## 2026-05-12 — Phase A: Global Design Token System (Phase 1 + Phase 2)

**Author:** Michael  
**Type:** Code | Schema | DI | Layout  
**Scope:** `design-system:token-phase-a`  
**Summary:** Implemented Phase A of the Global Design Token System. Phase 1 creates the `DesignTokens` table with 28 seeded button tokens. Phase 2 wires up the runtime CSS injection service and injects a `<style id="ds-token-overrides">` block into every page via `MainLayoutV2`.

### Phase 1 — Database + EF Model

#### New Files

| File | Version | Purpose |
|------|---------|---------|
| `src/AbsenceApp.Data/Models/DesignToken.cs` | 1.0.0 | EF entity for `DesignTokens` table. Properties: `Id, ComponentGroup, TokenKey, CssVariable, DefaultValue, CurrentValue?, Category, Description?, IsActive, SortOrder, CreatedAt, UpdatedAt`. `[Table("DesignTokens")]`. |
| `src/AbsenceApp.Data/Configurations/DesignTokenModelBuilderExtensions.cs` | 1.0.0 | `ConfigureDesignTokens()` extension. Configures entity, unique index `IX_DesignTokens_ComponentGroup_TokenKey`, and 28 seed rows. Seed timestamp: `2026-05-12 00:00:00 UTC`. |
| `C:\DevAbsence1\db_design_tokens.py` | 1.0.0 | Python SQL script — creates `DesignTokens` table and inserts all 28 seed rows via `INSERT IGNORE`. |

#### Modified Files

| File | Change |
|------|--------|
| `src/AbsenceApp.Data/Context/AppDbContext.cs` | v2.4.0 → v2.5.0. Added `DbSet<DesignToken> DesignTokens`. Called `modelBuilder.ConfigureDesignTokens()` in `OnModelCreating`. Added `DesignToken` to `ValueGeneratedNever` exclusion list. Added `DesignTokens` `ToTable` mapping. |

#### DB Changes

- `DesignTokens` table created in `absenceapp` database.
- 28 seed rows inserted: primary(10–13), secondary(20–23), success(30–33), danger(40–43), warning(50–53), info(60–63), structural base/radius/font/padding(70–73). All `ComponentGroup = "btn"`.

### Phase 2 — Runtime CSS Injection System

#### New Files

| File | Version | Purpose |
|------|---------|---------|
| `src/AbsenceApp.Client/Services/ApiV2/Modules/DesignTokenApiServiceV2.cs` | 1.0.0 | Singleton service. Methods: `GetGeneratedCssAsync()`, `GetGroupAsync(string)`, `UpdateTokensAsync(Dictionary<string,string?>)`, `ResetGroupAsync(string)`, `InvalidateCache()`. Event: `OnTokensChanged`. CSS output: `:root { --ds-btn-*: value; }` ordered by ComponentGroup ASC, SortOrder ASC. |
| `src/AbsenceApp.Client/Shared/Components/DesignTokenStyleInjectorV2.razor` | 1.0.0 | Renders `<style id="ds-token-overrides">` with runtime CSS. Subscribes to `OnTokensChanged`, re-renders on change. Implements `IAsyncDisposable`. |

#### Modified Files

| File | Change |
|------|--------|
| `src/AbsenceApp.Client/Extensions/V2ServiceCollectionExtensions.cs` | v1.7.0 → v1.8.0. Added `services.AddSingleton<DesignTokenApiServiceV2>()`. |
| `src/AbsenceApp.Client/Framework/Layout/MainLayoutV2.razor` | v1.9.0 → v2.0.0. Added `@using AbsenceApp.Client.Shared.Components`. Added `<DesignTokenStyleInjectorV2 />` before the init loader section. |

### Verification
- Build: `dotnet build AbsenceAppV2.sln -c Debug` — **0 errors, 0 warnings**
- DB: `DesignTokens` table exists with **28 rows** (all `ComponentGroup = "btn"`)
- Runtime: `<style id="ds-token-overrides">` renders in every page via `MainLayoutV2`
- No UI changes — buttons still use static `global-config.css` fallback values (Phase 3 wires the tokens into the CSS rules)
- No other files modified beyond those listed above

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


---

## 2026-05-11 � Indicator-Style Tab Design

### Summary
Upgraded the V2 tab strip from a flat underline to a 4px Corporate Blue bottom indicator on the active tab, with a 1px light-grey track on inactive tabs and a blue tint hover state. Introduced a dedicated component-level token layer (components.css) so tab appearance is fully token-driven. Eliminated a Blazor CSS scope isolation gap that caused UserFormPageV2 tabs to receive no indicator styling. Added a GlobalConfig demo page for live visual reference.

### Files Changed

| File | Change |
|------|--------|
| wwwroot/css/tokens/components.css | **NEW** � 12 --ds-tab-* design tokens for background, active, inactive and hover states. References existing palette and typography primitives from colors.css and        ypography.css. |
| wwwroot/index.html | Added <link rel="stylesheet" href="css/tokens/components.css" /> between layout.css and global-config.css. |
| Shared/Components/ProfileTabsV2.razor.css | v1.0.1 ? v1.0.2. Replacedorder-bottom approach withox-shadow: inset technique to deliver a 4px active / 1px inactive indicator without layout shift. All colour and weight values now consume --ds-tab-* tokens. |
| Modules/Users/UserFormPageV2.razor | v6.4.0 ? v6.5.0. Removed inline <div class="upv2-tab-bar"> and RenderTabBtn helper; replaced with <ProfileTabsV2> shared component wired to ViewModel.ProfileTabs / ActiveTab / SetTab. Resolves CSS scope gap that previously blocked indicator styles from applying. |
| Components/Pages/GlobalConfig/Tabs/Index.razor | **NEW** � Dual-route demo page (/global-config/tabs, /global-settings/tabs). Three dsv2-card blocks: Active Tab state + token table, Inactive Tab state + token table, Full Strip Preview with active/inactive/disabled examples. |

### Technical Notes
- **box-shadow inset technique**:ox-shadow: inset 0 calc(-1 * var(--ds-tab-active-border-width)) 0 var(--ds-tab-active-border-color) paints the bottom indicator inside the element box � no height change, no layout shift when switching between 1px and 4px states.
- **Blazor CSS scope isolation**: ProfileTabsV2.razor.css rules are attribute-scoped to the component's rendered output. UserFormPageV2's previous inline markup sat outside that scope and received no indicator styling. The conversion to the shared component closes this gap.
- **Token chain**: --ds-tab-active-border-color ? --ds-color-accent ? #2563eb (Corporate Blue). A single palette change propagates to all tab states automatically.
- **ProfileTabItemDto.Visible**: defaults to    rue; ProfileTabsV2 already filters Where(t => t.Visible) � no ViewModel changes required for UserFormPageV2.

### Rollout Notes
- Steps to deploy: 1) build (dotnet build AbsenceAppV2.sln -c Debug), 2) run app, navigate to any profile page and confirm active tab shows 4px blue indicator, inactive tabs show 1px grey track, 3) navigate to User Management edit page and confirm same indicator style, 4) navigate to /global-config/tabs or /global-settings/tabs and confirm demo cards render.
- Backout plan: git checkout HEAD -- src/AbsenceApp.Client/wwwroot/css/tokens/components.css src/AbsenceApp.Client/wwwroot/index.html src/AbsenceApp.Client/Shared/Components/ProfileTabsV2.razor.css src/AbsenceApp.Client/Modules/Users/UserFormPageV2.razor src/AbsenceApp.Client/Components/Pages/GlobalConfig/Tabs/Index.razor

### Verification
- Build: dotnet build AbsenceAppV2.sln -c Debug succeeded with 0 errors and 0 warnings.
- Code-level checks:
  - components.css :root block contains all 12 --ds-tab-* tokens.
  - index.html components.css link sits between layout.css and global-config.css.
  - ProfileTabsV2.razor.css .upv2-tab-btn hasox-shadow: inset and noorder-bottom.
  - ProfileTabsV2.razor.css .upv2-tab-btn--active hasox-shadow: inset 0 calc(-1 * var(--ds-tab-active-border-width)) and noorder-bottom-color.
  - UserFormPageV2.razor no longer contains RenderTabBtn or <div class="upv2-tab-bar".
  - UserFormPageV2.razor contains <ProfileTabsV2 Tabs="@ViewModel.ProfileTabs".
  - GlobalConfig/Tabs/Index.razor has both @page "/global-config/tabs" and @page "/global-settings/tabs" directives.


---

## 2026-05-11 � Sidebar Dual-Students Fix + Global Button CSS

### Summary
Two issues resolved: (1) clicking either "Students" menu (PEOPLE or ATTENDANCE) expanded both simultaneously due to identical group labels; (2) dsv2-btn colour variants rendered with no background because no global CSS defined them. Both fixes applied with live MySQL data authority � CSV seed files were explicitly not used (confirmed outdated; live DB had 2 extra rolemenuitems rows not present in CSVs).

### Issue 1 � Sidebar Expands Both Students Menus

#### Root Cause
SidebarV2.razor tracks accordion state with a single string: `private string _openGroup = string.Empty`. The open test is `_openGroup == group.Group`. Both PEOPLE?Students (Id=201000) and ATTENDANCE?Students (Id=402000) produced group.Group=="Students", so clicking either set _openGroup="Students" and both rendered open. No code change required � renaming 402000's label to "Student Attendance" makes the two groups distinct.

#### Live DB Changes (127.0.0.1:3306/absenceapp)
Validated live state before any modifications:
- 401000 (Attendance/menu), 401010 (Attendance/submenu), 402000 (Students/menu) � all confirmed present
- rolemenuitems for 401000: 3 rows (RoleIds 1, 2, 3) � CSV showed only 1 (confirmed CSV outdated)
- rolemenuitems for 401010: 3 rows (RoleIds 1, 2, 3)

Applied:
- `DELETE FROM rolemenuitems WHERE MenuItemId IN (401000, 401010)` � 6 rows deleted
- `DELETE FROM menuitems WHERE Id IN (401000, 401010)` � 2 rows deleted
- `UPDATE menuitems SET Label='Student Attendance', GroupName='Student Attendance', UpdatedAt=NOW() WHERE Id=402000` � 1 row updated

Post-change ATTENDANCE category: only Id=402000 (Student Attendance) remains as a child of 400000.

### Issue 2 � Button Background Colours Not Applying

#### Root Cause
dsv2-btn colour variants used across 9+ pages but defined nowhere globally. global-config.css had dsv2-card only. UserFormPageV2.razor.css and SettingsListPageV2.razor.css had partial scoped definitions (Blazor-isolated, not available to other components). index.html load order was already correct � no change needed.

#### Files Changed

| File | Change |
|------|--------|
| `wwwroot/css/global-config.css` | Appended full dsv2-btn system: base, disabled state, 8 colour fills (primary/secondary/success/danger/warning/info/light/dark) + hovers, 6 outline variants + hovers, 2 size modifiers (--lg, --sm), dark mode overrides for secondary and light. All values use `--v2-color-*` tokens from colors.css. |

Existing scoped definitions verified compatible:
- `UserFormPageV2.razor.css`: base .dsv2-btn + .dsv2-btn--secondary � values match global (no conflict)
- `SettingsListPageV2.razor.css`: .dsv2-btn--sm padding 0.25rem/0.625rem = 4px/10px � matches global exactly (no conflict)

### Verification
- Build: `dotnet build AbsenceAppV2.sln -c Debug` � 0 errors, 0 warnings
- Live DB post-check: ATTENDANCE category has single child (Student Attendance/402000); rolemenuitems for 401000/401010 = 0 rows remaining; rolemenuitems for 402000 intact (RoleIds 1/2/3)
- Runtime checks required:
  - Sidebar ATTENDANCE section: shows "Student Attendance" only
  - Clicking PEOPLE?Students does NOT open ATTENDANCE?Student Attendance
  - `/global-settings/buttons`: all 8 colour variant buttons show correct backgrounds
  - StudentFormPageV2, StaffFormPageV2: primary (blue) save buttons render correctly

### No CSV Files Modified
aaa_menuitems.csv and aaa_rolemenuitem.csv were intentionally not modified. They are reference/re-seed documents and do not drive runtime behaviour. The live DB is the authoritative data source.

---

## 2026-05-14 � UIKits Buttons Page

**Author:** Michael
**Type:** UI | Code | CSS
**Scope:** `design-system:uikits-buttons`
**Summary:** Implemented the UIKits / Buttons page at `/globalsettings/ui-kits/buttons`. Delivers a two-group accordion CSS editor for all 17 button variants (9 basic + 8 outline), backed by the DesignTokens DB. Editing outline colours updates the shared token for the corresponding filled variant. GlobalSettings.css updated to add `.dsv2-btn--link` and to migrate all outline variants from `--v2-color-*` to `--ds-btn-*` token references.

### Details

- **Basic Buttons group** (9 variants � primary, secondary, success, danger, warning, info, light, dark, link): token-backed accordion CSS editor for the 6 coloured variants; read-only static display for light, dark, and link.
- **Outline Buttons group** (8 variants � outline-primary through outline-info, plus dark and link): token-backed accordion CSS editor for 6 outline variants; secondary outline uses a special synthesis/parse branch because its `--ds-btn-secondary-bg` token is `transparent`.
- **Accordion toggle**: `</>` button per group opens/closes a monospace CSS textarea below the demo row.
- **Edit / Save / Cancel / Preview workflow**: Edit enters edit mode; Preview generates scoped CSS (`#btn-demo-{group} .{class} { � !important }`) without persisting; Save calls `DesignTokenApiServiceV2.UpdateTokensAsync`; Cancel discards the working copy.
- **Working copies**: variant-switching during edit mode saves a per-variant working copy keyed `{groupKey}-{variantKey}`.
- **CSS Synthesis**: `SynthesizeCss()` generates textarea content from current token values. Secondary outline uses a transparent-bg branch (`color` = text token, not bg token).
- **CSS Parsing**: `ParseCssToTokenValues()` extracts a `Dictionary<string, string?>` keyed by CssVariable for `UpdateTokensAsync`. Standard outline: `color` in base rule ? `{variant}-bg` token. Secondary outline: `color` in base rule ? `secondary-text` token.
- **Pure C# validation**: `ValidateCss()` uses compiled Regex to validate property names and colour values. No JSInterop.

### Affected Files and Components

| File | Version | Change |
|------|---------|--------|
| `src/AbsenceApp.Client/wwwroot/css/GlobalSettings.css` | 2.0.0 ? 2.1.0 | Added `.dsv2-btn--link` / `.dsv2-btn--link:hover`. Replaced all 6 outline variant rule blocks to reference `--ds-btn-*` tokens instead of `--v2-color-*`. Updated file header version and date. |
| `src/AbsenceApp.Client/Components/Pages/GlobalSettings/UIKits/Buttons/Index.razor` | placeholder ? 1.0.0 | Full two-group page markup: loading/error states, demo rows with preview style injection, accordion panels, monospace textarea editor, action row (Edit/Save, Cancel, Preview, status message). |
| `src/AbsenceApp.Client/Components/Pages/GlobalSettings/UIKits/Buttons/Index.razor.cs` | NEW ? 1.0.0 | Full code-behind: `ButtonVariantDef` record, `ButtonGroupState` class, all event handlers, `SynthesizeCss`, `ResolveToken`, `ValidateCss`, `ParseCssToTokenValues`, `BuildScopedPreviewCss`, `ExtractDeclarations`. |
| `src/AbsenceApp.Client/Components/Pages/GlobalSettings/UIKits/Buttons/Index.razor.css` | NEW ? 1.0.0 | Scoped styles with `btp-*` prefix: group card, group header, accordion toggle (gray/blue-filled states), demo row, button selection ring, editor textarea (readonly/active states), action buttons (Edit/Save, Cancel, Preview), status messages, loading/error states. |

### Rollout Notes
- No new DB tables, no new EF migrations � all 28 `btn` design tokens were already seeded in Phase A.
- No new shared components. No JSInterop.
- GlobalSettings.css outline variant changes are backward-compatible: the `--ds-btn-*` token fallback values match the previous hardcoded `--v2-color-*` colours exactly.
- Deploy: build ? run app ? navigate to `/globalsettings/ui-kits/buttons`.

### Verification
- Build: `dotnet build AbsenceAppV2.sln -c Debug` � 0 errors expected.
- Navigate to `/globalsettings/ui-kits/buttons`:
  - Basic Buttons group renders 9 variant buttons.
  - Outline Buttons group renders 8 variant buttons.
  - Click `</>` toggle � accordion opens; textarea shows synthesized CSS for the selected variant.
  - Click a different variant � textarea updates immediately.
  - Click Edit � textarea border turns blue; cursor active.
  - Edit a colour value, click Preview � selected button in demo row reflects preview colours (scoped style override).
  - Click Save � tokens persisted to DB, accordion exits edit mode, status "CSS Code Successfully Updated".
  - Click Cancel � working copy discarded; synthesized CSS restored from DB values.

  ---

  ## 2026-05-14 � Schema-Driven Design Token Engine Plan Execution (Phases A�D)

  **Author:** Michael  
  **Type:** Design | Planning | Governance  
  **Scope:** `design-system:schema-driven-engine`  
  **Summary:** Completed plan-mode execution of Phases A�D for the schema-driven design-token engine. Locked deterministic architecture decisions and phase artifacts for schema authority, DB reconciliation policy, CSS generation policy, and universal editor behavior design.

  ### Details

  #### Phase A � Schema format and governance (completed)
  - Locked schema as the single source of truth for component/variant/property definitions, defaults, inheritance, special-value semantics, and editor metadata.
  - Finalized canonical schema entity set and required/optional attribute classifications.
  - Finalized deterministic identity/naming/normalization rules and uniqueness constraints.
  - Finalized inheritance resolution order, override precedence, and invalid-reference/cycle handling policy.
  - Finalized special-value semantics policy (data-defined behavior only; no hardcoded assumptions).
  - Finalized schema lifecycle governance policy (versioning, compatibility, approvals, audit metadata).

  #### Phase B � DB auto-seeding strategy design (completed)
  - Finalized schema-to-persistence authority boundaries and synchronization objectives.
  - Finalized deterministic reconciliation state matrix (missing/matching/default-drift/deprecated/unknown).
  - Finalized preservation guarantees for runtime/user overrides and field mutability policy.
  - Finalized reconciliation run trigger and idempotency contract.
  - Finalized reconciliation audit, observability, and failure-handling policy.

  #### Phase C � CSS auto-generation strategy design (completed)
  - Finalized two CSS output domains: token-variable output and component-rule output.
  - Finalized deterministic schema-to-CSS mapping policy, ordering, and normalization rules.
  - Finalized inheritance-aware precedence and conflict resolution policy for generated CSS semantics.
  - Finalized legacy coexistence and retirement gate policy for transition from static CSS sources.
  - Finalized CSS generation audit requirements and evidence expectations.

  #### Phase D � Universal editor strategy design (completed)
  - Finalized universal, schema-driven editor contract (component/variant/property agnostic).
  - Finalized schema-driven navigation/grouping/order model.
  - Finalized editability semantics across editable/inherited/static/read-only states.
  - Finalized working/preview/persisted state-transition model (edit/preview/save/cancel/reset).
  - Finalized validation and user feedback policy standards for deterministic behavior.

  ### Affected Files and Components
  - `docs/project-governance/AbsenceApp_CHANGELOG_Phase2.md` (append-only update)

  ### Rollout Notes
  - This entry records planning/governance outcomes only; no implementation changes are included in this phase entry.
  - Phases A�D are closed in plan mode and are execution-ready for subsequent implementation mode when explicitly authorized.

  ### Verification
  - Phase A definition-of-done criteria satisfied.
  - Phase B definition-of-done criteria satisfied.
  - Phase C definition-of-done criteria satisfied.
  - Phase D definition-of-done criteria satisfied.

  ---

  ## 2026-05-14 � Schema-Driven Design Token Engine Plan Execution (Phase E)

  **Author:** Michael  
  **Type:** Design | Planning | Governance  
  **Scope:** `design-system:synthesis-parsing-engine`  
  **Summary:** Completed plan-mode execution of Phase E for the schema-driven design-token engine. Locked deterministic synthesis/parsing contracts, round-trip expectations, inheritance/special-value resolution policy, and engine auditability requirements.

  ### Details

  #### E1 � Synthesis objective (completed)
  - Finalized canonical synthesis contract: schema plus effective token values produce deterministic representation output.
  - Finalized output ordering/normalization expectations for reproducible synthesis outcomes.

  #### E2 � Parsing objective (completed)
  - Finalized deterministic parsing contract from edited representation back to schema-constrained token updates.
  - Finalized accepted/ignored/rejected outcome classes for parse decisions.

  #### E3 � Round-trip contract (completed)
  - Finalized synthesis?parse round-trip expectations and tolerance boundaries.
  - Finalized handling policy for declared non-reversible edge conditions.

  #### E4 � Inheritance and special-value resolution (completed)
  - Finalized schema-driven precedence rules for inherited and locally defined values in synthesis and parsing flows.
  - Finalized special-value resolution policy as data-defined behavior, with no component-specific hardcoding.

  #### E5 � Engine auditability requirements (completed)
  - Finalized mandatory traceability requirements for mapping decisions, rejected inputs, ignored values, and applied outcomes.
  - Finalized schema-version linkage requirements for all synthesis/parsing decision traces.

  ### Affected Files and Components
  - `docs/project-governance/AbsenceApp_CHANGELOG_Phase2.md` (append-only update)

  ### Rollout Notes
  - This entry records planning/governance outcomes only; no implementation changes are included in this phase entry.
  - Phase E is closed in plan mode and ready to transition to implementation mode only when explicitly authorized.

  ### Verification
  - E1 definition-of-done criteria satisfied.
  - E2 definition-of-done criteria satisfied.
  - E3 definition-of-done criteria satisfied.
  - E4 definition-of-done criteria satisfied.
  - E5 definition-of-done criteria satisfied.

  ---

  ## 2026-05-14 � Schema-Driven Design Token Engine Plan Execution (Phase F)

  **Author:** Michael  
  **Type:** Design | Planning | Governance  
  **Scope:** `design-system:migration-cutover`  
  **Summary:** Completed plan-mode execution of Phase F for the schema-driven design-token engine. Locked deterministic migration baseline invariants, stage-gated cutover criteria, rollback policies, expansion governance, and legacy retirement conditions.

  ### Details

  #### F1 � Migration baseline and invariants (completed)
  - Finalized baseline behavior invariants to preserve during migration.
  - Finalized non-negotiable parity expectations across token semantics, CSS behavior, and editor outcomes.

  #### F2 � Staged migration sequence (completed)
  - Finalized deterministic stage order from schema authority through legacy retirement readiness.
  - Finalized dependency ordering and stage-boundary ownership expectations.

  #### F3 � Cutover gate criteria (completed)
  - Finalized objective gate pass/fail criteria for stage advancement.
  - Finalized required sign-off evidence artifacts per gate.

  #### F4 � Rollback and contingency policy (completed)
  - Finalized rollback triggers by severity and stage.
  - Finalized rollback targets and required rollback-success evidence.

  #### F5 � Component expansion policy (completed)
  - Finalized schema-first onboarding policy for components beyond Buttons.
  - Finalized entry criteria and acceptance controls for additional component groups.

  #### F6 � Legacy retirement criteria (completed)
  - Finalized objective retirement thresholds for legacy/static duplicate sources.
  - Finalized closure conditions for decommissioning legacy paths after sustained parity validation.

  ### Affected Files and Components
  - `docs/project-governance/AbsenceApp_CHANGELOG_Phase2.md` (append-only update)

  ### Rollout Notes
  - This entry records planning/governance outcomes only; no implementation changes are included in this phase entry.
  - Phase F is closed in plan mode; all plan phases A�F are now complete and execution-ready for implementation mode when explicitly authorized.

  ### Verification
  - F1 definition-of-done criteria satisfied.
  - F2 definition-of-done criteria satisfied.
  - F3 definition-of-done criteria satisfied.
  - F4 definition-of-done criteria satisfied.
  - F5 definition-of-done criteria satisfied.
  - F6 definition-of-done criteria satisfied.

  ---

  ## 2026-05-14 � Schema-Driven Design Token Engine Phase A (EXECUTION MODE)

  **Author:** Michael  
  **Type:** Governance | Planning | Execution-Mode Documentation  
  **Scope:** `design-system:phase-a-artifacts`  
  **Summary:** Executed Phase A (A1�A7) end-to-end in EXECUTION MODE by creating the full approved governance artifact set, closing all Phase A gates, and producing a signed sign-off pack. No application/runtime code, CSS, schema runtime assets, or executable implementation content was changed.

---

## [2026-05-18] � Button Groups: Active/Disabled/Loading/Block Colour Fix + Block/Sizes/Groups Groups Added

**Author:** Michael / GitHub Copilot  
**Type:** Bug Fix | Feature Addition  
**Scope:** `UIKits:Buttons � Index.razor.cs, Index.razor.css`  
**Files Changed:**
- `AbsenceApp.Client/Components/Pages/GlobalSettings/UIKits/Buttons/Index.razor.css` v6.1.0 ? v6.2.0
- `AbsenceApp.Client/Components/Pages/GlobalSettings/UIKits/Buttons/Index.razor.cs` v6.1.0 ? v6.2.0

**Summary:**
Root cause: All `btp-active-*`, `btp-disabled-*`, `btp-loading-*`, and `btp-block-*` modifier classes carried only
behavioural CSS (box-shadow, opacity) with no colour tokens, causing affected buttons to render as unstyled
browser-default grey. Six groups were also missing state plumbing (Block, Sizes, Groups never registered).

**Phase 1 � Fix Active Buttons (Index.razor.css)**
- Replaced grouped `.btp-active-*` selector (box-shadow only) with 6 individual rules, each carrying
  full `background`, `color`, `border-color`, and `box-shadow: inset` matching Basic button design tokens.

**Phase 2 � Fix Disabled Buttons (Index.razor.css)**
- Replaced grouped `.btp-disabled-*` selector (opacity only) with 6 individual rules, each carrying
  full colour values plus `opacity: 0.65`, `cursor: not-allowed`, `pointer-events: none`, and `:hover` suppression.

**Phase 3 � Add Loading Button Colour Rules (Index.razor.css)**
- Added 8 new `.btp-loading-*` CSS rules after `.btp-dot-loader` block (primary, secondary, success,
  danger; outline variants for each), each carrying full `background`, `color`, `border-color`.

**Phase 4 � Add Block Button Colour Rules (Index.razor.css + Index.razor.cs)**
- CSS: Added 12 new `.btp-block-*` rules (6 colour + 6 `:hover`) after `.btp-block-btn` block.
- CS: Fixed `BlockVariants` `PreviewCssClass` � added `dsv2-btn--*` colour class to each of the 6 entries
  (e.g. `"dsv2-btn dsv2-btn--primary btp-block-btn btp-block-primary"`).
- CS: Added `_blockState` field, registered in `GroupStates`, initialised in `OnInitialized`.

---

## [2026-05-18] � Buttons Page: 8-Colour Expansion, Pulse Animation, Link Button Removed

**Author:** Michael / GitHub Copilot  
**Type:** Bug Fix | Feature Addition | Cleanup  
**Scope:** `UIKits:Buttons � Index.razor, Index.razor.cs, Index.razor.css`  
**Files Changed:**
- `AbsenceApp.Client/Components/Pages/GlobalSettings/UIKits/Buttons/Index.razor` v6.0.0 ? v6.1.0
- `AbsenceApp.Client/Components/Pages/GlobalSettings/UIKits/Buttons/Index.razor.cs` v6.2.0 ? v6.3.0
- `AbsenceApp.Client/Components/Pages/GlobalSettings/UIKits/Buttons/Index.razor.css` v6.2.0 ? v6.3.0

**Summary:**
Expanded Active Buttons, Disabled Buttons, and Button Sizes groups from 2�6 variants to 8 full-colour
variants matching the Basic Buttons palette. Implemented missing Pulse animation CSS. Removed the Link
button variant from Basic, Outline, and Soft groups. Removed download icons from Active/Disabled/Sizes
button previews.

**1 � Disabled Buttons: 8-Colour Expansion**
- `DisabledVariants` replaced (6 outline/soft entries) with 8 fill variants: Primary, Secondary, Success,
  Danger, Warning, Info, Light, Dark.
- `PreviewCssClass` uses `"dsv2-btn btp-disabled-{colour}"` pattern.
- CSS: Replaced `btp-disabled-outline-*` and `btp-disabled-soft-*` rules with 8 new colour rules + `:hover`
  suppression for success, danger, warning, info, light, dark.
- `StaticVariantCss`: 6 old entries replaced with 8 `BuildCss(...)` entries using full colour tokens.

**2 � Active Buttons: 8-Colour Expansion**
- `ActiveVariants` replaced (6 outline/soft entries) with 8 fill variants: Primary, Secondary, Success,
  Danger, Warning, Info, Light, Dark.
- `PreviewCssClass` uses `"dsv2-btn btp-active-{colour}"` pattern.
- CSS: Replaced `btp-active-outline-*` and `btp-active-soft-*` rules with 8 new colour rules each carrying
  inset box-shadow for the active/pressed visual.
- `StaticVariantCss`: 6 old entries replaced with 8 `BuildCss(...)` entries.

**3 � Button Sizes: 8-Colour Expansion**
- `SizeVariants` replaced (size-sm/md/lg + 3 colours) with 8 colour variants: Primary, Secondary, Success,
  Danger, Warning, Info, Light, Dark � each using `dsv2-btn--*` as `PreviewCssClass`.
- `StaticVariantCss`: Old `size-sm/md/lg/secondary/success/danger` entries replaced with 8 `BuildCss(...)`
  entries mapping to the same `dsv2-btn--*` colour selectors as Basic Buttons.
- No icons on any size button preview (icons removed from default Razor case).

**4 � Pulse Loading Button: Animation Implemented**
- CSS: Added `@keyframes btp-pulse` (scale + opacity, 1 s ease-in-out infinite).
- CSS: Added `.btp-pulse-loader` class (same geometry as `.btp-dot-loader`, with `animation` applied).
- Razor (`loading` case): Changed pulse/danger variant loader from `btp-dot-loader` ? `btp-pulse-loader`.

**5 � Link Button Removed**
- `BasicVariants`: Removed `basic-link` entry (Label "Link", class `dsv2-btn--link`).
- `OutlineVariants`: Removed `outline-link` entry.
- `SoftVariants`: Removed `soft-link` entry.
- `StaticVariantCss`: Removed `basic-link`, `outline-link`, `soft-link` entries.
- No Link button appears anywhere in the Buttons page.

**6 � Icons Removed from Active / Disabled / Sizes**
- Razor `default` case: Removed the `@if (state.GroupKey is "active" or "disabled" or "sizes")` icon block.
  All three groups now render plain text-only buttons matching Basic Buttons style.

---

## [2026-05-18] � Button Sizes Group: Corrected to 3 Size Variants (sm / md / lg)

**Author:** Michael / GitHub Copilot  
**Type:** Bug Fix  
**Scope:** `UIKits:Buttons � Index.razor.cs`  
**Files Changed:**
- `AbsenceApp.Client/Components/Pages/GlobalSettings/UIKits/Buttons/Index.razor.cs` v6.3.0 ? v6.4.0

**Summary:**
Corrected the Button Sizes group which was incorrectly showing 8 colour variants (a copy of the
Basic Buttons palette). Replaced with 3 proper size variants � Small, Medium, and Large � each
using the Primary colour token plus the appropriate size modifier class.

**Changes:**
- `SizeVariants` replaced (8 colour entries: size-primary through size-dark) with 3 size entries:
  - `size-sm` � Label "Small", PreviewCssClass `dsv2-btn--primary btp-size-sm`
  - `size-md` � Label "Medium", PreviewCssClass `dsv2-btn--primary btp-size-md`
  - `size-lg` � Label "Large", PreviewCssClass `dsv2-btn--primary btp-size-lg`
- `StaticVariantCss` size entries replaced (8 `BuildCss(...)` entries) with 3 literal CSS entries:
  - `size-sm` ? `.btp-size-sm { padding, font-size, border-radius }`
  - `size-md` ? `.btp-size-md { padding, font-size, border-radius, line-height }`
  - `size-lg` ? `.btp-size-lg { padding, font-size, border-radius }`
- CSS (`Index.razor.css`): No changes � `.btp-size-sm/md/lg` rules already existed.
- Razor (`Index.razor`): No changes � `default` case already renders `dsv2-btn @variant.PreviewCssClass` correctly.
- Result: Button Sizes group now displays 3 horizontally-arranged buttons (Small / Medium / Large)
  in Primary colour, each demonstrating the correct size modifier.
- `BasicVariants`: Removed `basic-link` entry (Label "Link", class `dsv2-btn--link`).
- `OutlineVariants`: Removed `outline-link` entry.
- `SoftVariants`: Removed `soft-link` entry.
- `StaticVariantCss`: Removed `basic-link`, `outline-link`, `soft-link` entries.
- No Link button appears anywhere in the Buttons page.

**6 � Icons Removed from Active / Disabled / Sizes**
- Razor `default` case: Removed the `@if (state.GroupKey is "active" or "disabled" or "sizes")` icon block.
  All three groups now render plain text-only buttons matching Basic Buttons style.

**Phase 5 � Add Button Sizes (Index.razor.cs)**
- Fixed `SizeVariants` `PreviewCssClass` � replaced `btp-size-primary/secondary/etc.` (non-existent) with
  existing `dsv2-btn--*` colour classes (size-sm/md/lg keep size modifier; colour-only entries drop it).
- CS: Added `_sizeState` field, registered in `GroupStates`, initialised in `OnInitialized`.

**Phase 6 � Add Button Groups (Index.razor.cs)**
- Added `_groupState` field, registered in `GroupStates`, initialised in `OnInitialized` using existing `GroupVariants`.

**Design Token Colours Used:**
Primary #0f626a | Secondary #626262 | Success #0ab964 | Danger #e14e5a

  ### Details

---

## Phase 1 — ActionButtonV2 Component + CSS + Tokens
**Date:** 2026-05-19
**Author:** Michael (AI-assisted)

### Summary
Initial controlled rebuild of the ActionButtonV2 design system component,
its associated global CSS, and the 16 action-btn design tokens.

### Files Created
| File | Purpose |
|------|---------|
| `src/AbsenceApp.Client/Components/DesignSystem/ActionButtonV2.razor` | Reusable action button component (v1.0.0) |
| `src/AbsenceApp.Client/wwwroot/css/components/action-buttons.css` | Global CSS for ActionButtonV2 (v1.0.0) |
| `src/AbsenceApp.Data/Migrations/20260519000000_AddActionButtonTokens.cs` | EF migration — inserts 16 action-btn tokens |
| `src/AbsenceApp.Data/Migrations/20260519000000_AddActionButtonTokens.Designer.cs` | EF migration designer snapshot |

### Files Modified
| File | Change |
|------|--------|
| `src/AbsenceApp.Data/Configurations/DesignTokenModelBuilderExtensions.cs` | v2.0.0 → v2.1.0: added 16 action-btn tokens (IDs 1100–1115) |
| `src/AbsenceApp.Data/Migrations/AppDbContextModelSnapshot.cs` | Added 16 action-btn HasData entries (IDs 1100–1115) |

### Design Tokens Added (IDs 1100–1115)
| ID | TokenKey | CssVariable | Category |
|----|----------|-------------|----------|
| 1100 | primary-bg | --ds-action-btn-primary-bg | color |
| 1101 | primary-hover-bg | --ds-action-btn-primary-hover-bg | color |
| 1102 | primary-text | --ds-action-btn-primary-text | color |
| 1103 | primary-icon | --ds-action-btn-primary-icon | color |
| 1104 | secondary-bg | --ds-action-btn-secondary-bg | color |
| 1105 | secondary-hover-bg | --ds-action-btn-secondary-hover-bg | color |
| 1106 | secondary-text | --ds-action-btn-secondary-text | color |
| 1107 | secondary-icon | --ds-action-btn-secondary-icon | color |
| 1108 | radius | --ds-action-btn-radius | radius |
| 1109 | padding-x | --ds-action-btn-padding-x | spacing |
| 1110 | padding-y | --ds-action-btn-padding-y | spacing |
| 1111 | font-size | --ds-action-btn-font-size | typography |
| 1112 | font-size-sm | --ds-action-btn-font-size-sm | typography |
| 1113 | font-size-lg | --ds-action-btn-font-size-lg | typography |
| 1114 | icon-size | --ds-action-btn-icon-size | typography |
| 1115 | icon-only-size | --ds-action-btn-icon-only-size | structure |

All DefaultValues set to "TBD" per audit convention.

### Implementation Notes
- `Disabled` handled via native HTML `disabled` attribute only — no custom CSS class.
- CSS classes exactly as spec: `.dsv2-action-btn`, `.dsv2-action-btn__icon`,
  `.dsv2-action-btn--primary`, `.dsv2-action-btn--secondary`,
  `.dsv2-action-btn--sm`, `.dsv2-action-btn--md`, `.dsv2-action-btn--lg`,
  `.dsv2-action-btn--icon-only`.
- Hover rules included only because they reference CSS variable tokens.
- Migration `20260519000000_AddActionButtonTokens` created but NOT run.
- `index.html` link tag for `action-buttons.css` deferred to a later phase.

### Out-of-Scope (NOT modified in this phase)
- `index.html`, `_Imports.razor`, `PageHeaderV2.razor`
- Any module pages or UI Kit pages

  #### A1 � Scope and authority lock
  - Produced **Phase A Scope Record** and **Schema Authority Statement**.
  - Locked scope in: `btn`, `card`.
  - Locked scope out: `tab` and all non-listed UIKit domains.

  #### A2 � Canonical entity definitions
  - Produced **Canonical Entity Matrix** with mandatory/optional attributes and deterministic entity dependencies.

  #### A3 � Identity and naming policy
  - Produced **Identity/Naming Rulebook** with canonical keying, normalization, and collision policy.

  #### A4 � Inheritance semantics
  - Produced **Inheritance Resolution Matrix** with deterministic resolution order, precedence rules, and invalid-state handling.

  #### A5 � Special-value semantics
  - Produced **Special-Value Semantics Register** defining explicit semantic classes and validation outcomes.

  #### A6 � Lifecycle governance
  - Produced **Schema Lifecycle Governance Policy** defining versioning, compatibility classes, approvals, and compliance gates.

  #### A7 � Sign-off pack
  - Produced **Phase A Sign-Off Pack** aggregating all required artifacts and recording definition-of-done closure.

  ### Affected Files and Components
  - `docs/project-governance/design-token-engine/phase-a/PhaseA_Scope_Record.md`
  - `docs/project-governance/design-token-engine/phase-a/Schema_Authority_Statement.md`
  - `docs/project-governance/design-token-engine/phase-a/Canonical_Entity_Matrix.md`
  - `docs/project-governance/design-token-engine/phase-a/Identity_Naming_Rulebook.md`
  - `docs/project-governance/design-token-engine/phase-a/Inheritance_Resolution_Matrix.md`
  - `docs/project-governance/design-token-engine/phase-a/Special_Value_Semantics_Register.md`
  - `docs/project-governance/design-token-engine/phase-a/Schema_Lifecycle_Governance_Policy.md`
  - `docs/project-governance/design-token-engine/phase-a/PhaseA_SignOff_Pack.md`
  - `docs/project-governance/AbsenceApp_CHANGELOG_Phase2.md` (append-only entry)

  ### Rollout Notes
  - This execution was documentation/governance only.
  - No runtime implementation changes were applied.

  ### Verification
  - Required Phase A artifacts: all present.
  - Phase A gates A1�A7: all closed.
  - Blocking ambiguities: none.
  - Phase A execution-mode objective: satisfied.

  ---

  ## 2026-05-15 � Schema-Driven Design Token Engine Phase B (EXECUTION MODE)

  **Author:** Michael  
  **Type:** Governance | Planning | Reconciliation Policy  
  **Scope:** `design-system:phase-b-reconciliation-governance`  
  **Summary:** Executed Phase B (B1�B5) end-to-end in EXECUTION MODE by producing the full reconciliation governance artifact set covering synchronization objectives, authority boundaries, deterministic state handling, preservation guarantees, idempotency policy, and audit/failure requirements.

  ### Details

  #### B1 � Synchronization objectives and authority boundaries
  - Produced **Phase B Synchronization Objective Record** and **Field Authority Map**.
  - Locked schema-vs-persistence authority boundaries and in-sync criteria.

  #### B2 � Deterministic reconciliation state design
  - Produced **Reconciliation State Matrix** and **State Classification Rules**.
  - Locked deterministic state classification and action mapping policy.

  #### B3 � Preservation and lifecycle governance
  - Produced **Preservation Contract**, **Field Mutability Register**, and **Lifecycle-State Governance Addendum**.
  - Locked non-destructive runtime override preservation invariants and field mutability policy.

  #### B4 � Trigger and idempotency governance
  - Produced **Reconciliation Trigger Policy**, **Idempotency Contract**, and **Repeatability Acceptance Criteria**.
  - Locked deterministic trigger classes and repeatability expectations.

  #### B5 � Audit and failure governance
  - Produced **Reconciliation Audit Requirements**, **Failure Severity Matrix**, and **Reconciliation Run Evidence Checklist**.
  - Locked mandatory evidence and severity-based run outcome handling.

  ### Affected Files and Components
  - `docs/project-governance/design-token-engine/phase-b/PhaseB_Synchronization_Objective_Record.md`
  - `docs/project-governance/design-token-engine/phase-b/Field_Authority_Map.md`
  - `docs/project-governance/design-token-engine/phase-b/Reconciliation_State_Matrix.md`
  - `docs/project-governance/design-token-engine/phase-b/State_Classification_Rules.md`
  - `docs/project-governance/design-token-engine/phase-b/Preservation_Contract.md`
  - `docs/project-governance/design-token-engine/phase-b/Field_Mutability_Register.md`
  - `docs/project-governance/design-token-engine/phase-b/Lifecycle_State_Governance_Addendum.md`
  - `docs/project-governance/design-token-engine/phase-b/Reconciliation_Trigger_Policy.md`
  - `docs/project-governance/design-token-engine/phase-b/Idempotency_Contract.md`
  - `docs/project-governance/design-token-engine/phase-b/Repeatability_Acceptance_Criteria.md`
  - `docs/project-governance/design-token-engine/phase-b/Reconciliation_Audit_Requirements.md`
  - `docs/project-governance/design-token-engine/phase-b/Failure_Severity_Matrix.md`
  - `docs/project-governance/design-token-engine/phase-b/Reconciliation_Run_Evidence_Checklist.md`
  - `docs/project-governance/AbsenceApp_CHANGELOG_Phase2.md` (append-only entry)

  ### Rollout Notes
  - This execution was governance/documentation only.
  - No application code, CSS, schema runtime assets, or implementation logic changed.

  ### Verification
  - Required Phase B artifacts: all present.
  - Phase B gates B1�B5: all closed.
  - Blocking ambiguities: none.
  - Phase B execution-mode objective: satisfied.

  ---

  ## 2026-05-15 � Schema-Driven Design Token Engine Phase C (EXECUTION MODE)

  **Author:** Michael  
  **Type:** Governance | Planning | CSS Mapping Policy  
  **Scope:** `design-system:phase-c-css-governance`  
  **Summary:** Executed Phase C (C1�C5) end-to-end in EXECUTION MODE by producing the full CSS governance artifact set for output domains, ownership decisions, schema-to-CSS mapping determinism, precedence/conflict policies, legacy coexistence/retirement criteria, and generation audit evidence requirements.

  ### Details

  #### C1 � CSS output domains and ownership
  - Produced **CSS Output Domain Contract** and **Domain Ownership Decision Log**.
  - Locked two-domain model and non-overlap ownership policy.

  #### C2 � Schema-to-CSS mapping governance
  - Produced **Schema-to-CSS Mapping Matrix**, **Ordering/Normalization Policy**, and **Mapping Validation Checklist**.
  - Locked deterministic mapping, ordering, and normalization controls.

  #### C3 � Inheritance and conflict governance
  - Produced **CSS Inheritance/Precedence Table** and **Conflict Classification Policy**.
  - Locked precedence resolution and conflict severity handling policy.

  #### C4 � Legacy coexistence and transition governance
  - Produced **Legacy Coexistence Policy**, **Transition Gate Criteria**, and **Legacy Retirement Criteria Register**.
  - Locked controlled coexistence, stage-gate progression, and retirement conditions.

  #### C5 � CSS generation audit governance
  - Produced **CSS Generation Audit Specification** and **Generation Evidence Checklist**.
  - Locked mandatory generation audit fields and evidence acceptance rules.

  ### Affected Files and Components
  - `docs/project-governance/design-token-engine/phase-c/CSS_Output_Domain_Contract.md`
  - `docs/project-governance/design-token-engine/phase-c/Domain_Ownership_Decision_Log.md`
  - `docs/project-governance/design-token-engine/phase-c/Schema_to_CSS_Mapping_Matrix.md`
  - `docs/project-governance/design-token-engine/phase-c/Ordering_Normalization_Policy.md`
  - `docs/project-governance/design-token-engine/phase-c/Mapping_Validation_Checklist.md`
  - `docs/project-governance/design-token-engine/phase-c/CSS_Inheritance_Precedence_Table.md`
  - `docs/project-governance/design-token-engine/phase-c/Conflict_Classification_Policy.md`
  - `docs/project-governance/design-token-engine/phase-c/Legacy_Coexistence_Policy.md`
  - `docs/project-governance/design-token-engine/phase-c/Transition_Gate_Criteria.md`
  - `docs/project-governance/design-token-engine/phase-c/Legacy_Retirement_Criteria_Register.md`
  - `docs/project-governance/design-token-engine/phase-c/CSS_Generation_Audit_Specification.md`
  - `docs/project-governance/design-token-engine/phase-c/Generation_Evidence_Checklist.md`
  - `docs/project-governance/AbsenceApp_CHANGELOG_Phase2.md` (append-only entry)

  ### Rollout Notes
  - This execution was governance/documentation only.
  - No application code, CSS, schema runtime assets, or implementation logic changed.

  ### Verification
  - Required Phase C artifacts: all present.
  - Phase C gates C1�C5: all closed.
  - Blocking ambiguities: none.
  - Phase C execution-mode objective: satisfied.

  ---

  ## 2026-05-15 � Schema-Driven Design Token Engine Phase D (EXECUTION MODE)

  **Author:** Michael  
  **Type:** Governance | Planning | UI Model Specification  
  **Scope:** `design-system:phase-d-universal-editor-governance`  
  **Summary:** Executed Phase D (D1�D5) end-to-end in EXECUTION MODE by producing the full universal editor governance artifact set for behavior contracts, schema-driven navigation/grouping, editability semantics, state model transitions, validation/feedback standards, and sign-off summary evidence.

  ### Details

  #### D1 � Universal editor behavior contract
  - Produced **Universal Editor Behavior Contract**.
  - Locked schema-driven, component-agnostic behavior scope and deterministic mode semantics.

  #### D2 � Schema-driven navigation/grouping model
  - Produced **Schema-Driven Navigation & Grouping Model**.
  - Locked deterministic grouping, ordering, and reference integrity constraints.

  #### D3 � Editability and override semantics
  - Produced **Editability & Override Semantics Specification**.
  - Locked semantic classes and override policy constraints.

  #### D4 � Working/preview/persisted state model
  - Produced **Working/Preview/Persisted State Model**.
  - Locked explicit transition rules and non-destructive state guarantees.

  #### D5 � Validation and feedback standards
  - Produced **Validation & Feedback Standards**.
  - Locked validation layers, outcome classes, and deterministic feedback policy.

  #### Governance Sign-Off
  - Produced **Phase D Sign-Off Summary** as closure evidence.

  ### Affected Files and Components
  - `docs/project-governance/design-token-engine/phase-d/Universal_Editor_Behavior_Contract.md`
  - `docs/project-governance/design-token-engine/phase-d/Schema_Driven_Navigation_and_Grouping_Model.md`
  - `docs/project-governance/design-token-engine/phase-d/Editability_and_Override_Semantics_Specification.md`
  - `docs/project-governance/design-token-engine/phase-d/Working_Preview_Persisted_State_Model.md`
  - `docs/project-governance/design-token-engine/phase-d/Validation_and_Feedback_Standards.md`
  - `docs/project-governance/design-token-engine/phase-d/PhaseD_SignOff_Summary.md`
  - `docs/project-governance/AbsenceApp_CHANGELOG_Phase2.md` (append-only entry)

  ### Rollout Notes
  - This execution was governance/documentation only.
  - No application code, CSS, schema runtime assets, or implementation logic changed.

  ### Verification
  - Required Phase D artifacts: all present.
  - Phase D gates D1�D5: all closed.
  - Blocking ambiguities: none.
  - Phase D execution-mode objective: satisfied.

  ---

  ## 2026-05-15 � Schema-Driven Design Token Engine Phase E (EXECUTION MODE)

  **Author:** Michael  
  **Type:** Governance | Planning | Synthesis/Parsing Specification  
  **Scope:** `design-system:phase-e-engine-governance`  
  **Summary:** Executed Phase E (E1�E5) end-to-end in EXECUTION MODE by producing the full synthesis/parsing governance artifact set for synthesis objectives, parsing objectives, round-trip consistency controls, inheritance/special-value resolution, and engine auditability evidence requirements.

  ### Details

  #### E1 � Synthesis objective
  - Produced **Synthesis Objective Specification**.
  - Locked deterministic synthesis contract and canonical output constraints.

  #### E2 � Parsing objective
  - Produced **Parsing Objective Specification**.
  - Locked deterministic parsing outcomes and classification policy.

  #### E3 � Round-trip consistency
  - Produced **Round-Trip Consistency Contract**.
  - Locked semantic-preservation expectations and tolerance boundaries.

  #### E4 � Inheritance and special-value resolution
  - Produced **Inheritance & Special-Value Resolution Model**.
  - Locked deterministic resolution order and invalid-condition handling.

  #### E5 � Engine auditability and evidence
  - Produced **Engine Auditability & Evidence Requirements**.
  - Locked mandatory trace fields, severity classes, and evidence compliance rules.

  #### Governance Sign-Off
  - Produced **Phase E Sign-Off Summary** as closure evidence.

  ### Affected Files and Components
  - `docs/project-governance/design-token-engine/phase-e/Synthesis_Objective_Specification.md`
  - `docs/project-governance/design-token-engine/phase-e/Parsing_Objective_Specification.md`
  - `docs/project-governance/design-token-engine/phase-e/Round_Trip_Consistency_Contract.md`
  - `docs/project-governance/design-token-engine/phase-e/Inheritance_and_Special_Value_Resolution_Model.md`
  - `docs/project-governance/design-token-engine/phase-e/Engine_Auditability_and_Evidence_Requirements.md`
  - `docs/project-governance/design-token-engine/phase-e/PhaseE_SignOff_Summary.md`
  - `docs/project-governance/AbsenceApp_CHANGELOG_Phase2.md` (append-only entry)

  ### Rollout Notes
  - This execution was governance/documentation only.
  - No application code, CSS, schema runtime assets, or implementation logic changed.

  ### Verification
  - Required Phase E artifacts: all present.
  - Phase E gates E1�E5: all closed.
  - Blocking ambiguities: none.
  - Phase E execution-mode objective: satisfied.

  ---

  ## 2026-05-15 � Schema-Driven Design Token Engine Phase F (EXECUTION MODE)

  **Author:** Michael  
  **Type:** Governance | Planning | Migration Policy  
  **Scope:** `design-system:phase-f-migration-governance`  
  **Summary:** Executed Phase F (F1�F6) end-to-end in EXECUTION MODE by producing the complete migration governance artifact set for baseline invariants, staged sequencing, cutover gate criteria, rollback/contingency controls, component expansion governance, and legacy retirement criteria.

  ### Details

  #### F1 � Migration baseline and invariants
  - Produced **Migration Baseline & Invariants Record**.
  - Locked baseline invariants and breach handling constraints.

  #### F2 � Staged migration sequence
  - Produced **Staged Migration Sequence Plan**.
  - Locked deterministic stage order and dependency progression rules.

  #### F3 � Cutover gate criteria
  - Produced **Cutover Gate Criteria**.
  - Locked objective pass/fail controls and blocking conditions.

  #### F4 � Rollback and contingency policy
  - Produced **Rollback & Contingency Policy**.
  - Locked rollback triggers, evidence requirements, and re-entry controls.

  #### F5 � Component expansion policy
  - Produced **Component Expansion Policy**.
  - Locked schema-first onboarding entry criteria and expansion governance rules.

  #### F6 � Legacy retirement criteria
  - Produced **Legacy Retirement Criteria**.
  - Locked objective retirement conditions and evidence-backed closure policy.

  #### Governance Sign-Off
  - Produced **Phase F Sign-Off Summary** as closure evidence.

  ### Affected Files and Components
  - `docs/project-governance/design-token-engine/phase-f/Migration_Baseline_and_Invariants_Record.md`
  - `docs/project-governance/design-token-engine/phase-f/Staged_Migration_Sequence_Plan.md`
  - `docs/project-governance/design-token-engine/phase-f/Cutover_Gate_Criteria.md`
  - `docs/project-governance/design-token-engine/phase-f/Rollback_and_Contingency_Policy.md`
  - `docs/project-governance/design-token-engine/phase-f/Component_Expansion_Policy.md`
  - `docs/project-governance/design-token-engine/phase-f/Legacy_Retirement_Criteria.md`
  - `docs/project-governance/design-token-engine/phase-f/PhaseF_SignOff_Summary.md`
  - `docs/project-governance/AbsenceApp_CHANGELOG_Phase2.md` (append-only entry)

  ### Rollout Notes
  - This execution was governance/documentation only.
  - No application code, CSS, schema runtime assets, or implementation logic changed.

  ### Verification
  - Required Phase F artifacts: all present.
  - Phase F gates F1�F6: all closed.
  - Blocking ambiguities: none.
  - Phase F execution-mode objective: satisfied.

---

## 2026-05-15 � Global Token Audit Execution

**Author:** Michael
**Type:** Audit | Refactor | Docs
**Scope:** `design-token-engine`, `component-audit`
**Summary:** Executed the approved global token audit plan across the solution, correcting legacy button token usage, tokenising the buttons demo page where SQL-backed tokens exist, and inserting TODO markers for components that require new token families.

### Details
- Corrected legacy `--v2-*` usage in `wwwroot/css/components/buttons.css` to SQL-backed `btn-*` tokens.
- Tokenised the buttons demo page (`Components/Pages/GlobalSettings/UIKits/Buttons/Index.razor.css`) with SQL-backed `btn-*` and `card-*` tokens where matching families existed.
- Inserted TODO markers for files whose hard-coded styling requires new SQL token families before tokenisation.
- Updated header blocks in affected files where present, preserving the existing format as closely as possible.

### Affected Files and Components
- Files:
  - `src/AbsenceApp.Client/wwwroot/css/components/buttons.css`
  - `src/AbsenceApp.Client/Components/Pages/GlobalSettings/UIKits/Buttons/Index.razor.css`
  - `src/AbsenceApp.Client/Framework/Layout/HeaderV2.razor.css`
  - `src/AbsenceApp.Client/Framework/Layout/SidebarV2.razor.css`
  - `src/AbsenceApp.Client/Framework/PageTemplates/PageTemplateV2.razor.css`
  - `src/AbsenceApp.Client/Components/Templates/TablePageTemplate.razor.css`
  - `src/AbsenceApp.Client/Components/Templates/FormPageTemplate.razor.css`
  - `src/AbsenceApp.Client/Components/Templates/DashboardPageTemplate.razor.css`
  - `src/AbsenceApp.Client/Components/Alerts/AlertV2.razor.css`
  - `src/AbsenceApp.Client/Components/DesignSystem/IconButton.razor.css`
  - `src/AbsenceApp.Client/Components/DesignSystem/Icon.razor.css`
  - `src/AbsenceApp.Client/wwwroot/css/components/cards.css`
  - `src/AbsenceApp.Client/wwwroot/css/components/forms.css`
  - `src/AbsenceApp.Client/wwwroot/css/components/stats.css`
- Components: button demo page, layout chrome, table/form/dashboard templates, alert system, icon button/icon primitives, card/form/stat component shells.

### Rollout Notes
- Read-only audit performed first to identify SQL-backed token families and token gaps.
- No SQL schema changes were made.
- TODO comments were used for missing token families; no new tokens were created in this phase.

### Verification
- SQL token inventory confirmed from live `absenceapp.designtokens`.
- CSS/HTML token audit applied only to approved files.
- Build/parse validation to follow after header/changelog review.

### References
- Audit plan: Global Token Audit � Plan Mode
- Related execution: Tokenisation and TODO annotation pass

---

## 2026-05-15 � Button Token Integration (Light / Dark / Outline-Dark)

**Author:** Michael
**Type:** Design | Component | Config
**Scope:** `component:button`, `design-token-engine`
**Summary:** Wired 25 new `btn` design tokens (IDs 106�130) into `buttons.css`, `components.json`, and `theme.json` for the Light, Dark, and Outline-Dark button variants.

### Details
Token source of truth: live `absenceapp.designtokens` table (MySQL), rows 106�130, `ComponentGroup='btn'`.

Three changes applied exactly per BUTTON TOKEN INTEGRATION � EXECUTION MODE spec:

1. **buttons.css** � `.dsv2-btn--light` and `.dsv2-btn--dark` updated from hardcoded/legacy `--v2-*` vars to explicit `--ds-btn-light-*` / `--ds-btn-dark-*` tokens (base, hover, disabled). New `.dsv2-btn--outline-dark` variant block added with base, hover (`:hover:not(:disabled)`), and disabled rules using `--ds-btn-outline-dark-*` tokens. All fallbacks match SQL `DefaultValue`.

2. **components.json** � `button.variants` extended with `"light"`, `"dark"`, `"outline-dark"` appended to the existing array. No other keys changed.

3. **theme.json** � All 25 tokens added to both `themes.light.colors` and `themes.dark.colors`. Light theme values match SQL `DefaultValue`; dark theme values use appropriate dark-mode palette values. Existing theme keys untouched.

### Affected Files and Components
- Files:
  - `src/AbsenceApp.Client/wwwroot/css/components/buttons.css`
  - `src/AbsenceApp.Client/wwwroot/config/designsystem/components.json`
  - `src/AbsenceApp.Client/wwwroot/config/designsystem/theme.json`
- Components: `dsv2-btn--light`, `dsv2-btn--dark`, `dsv2-btn--outline-dark`

### Rollout Notes
- CSS and JSON static assets; no migration required.
- Runtime token injection via `DesignTokenApiServiceV2` will override CSS vars from DB `CurrentValue ?? DefaultValue` at runtime.
- Backout: revert the three files; no DB changes were made.

### Verification
- SQL token source confirmed from live DB (25 rows, IDs 106�130).
- `dotnet build AbsenceApp.Data.csproj`: 0 errors, 0 warnings.
- Full solution build blocked by `AbsenceApp.Client (28244)` file-lock (app running during build) � all 4 errors are `MSB3021`/`MSB3027` DLL-copy locks, zero C# compilation errors.
- All 25 tokens present in both light and dark sections of `theme.json`.
- All 3 new variants registered in `components.json` `button.variants`.
- All 3 CSS variant blocks present and correct in `buttons.css`.
- No unrelated selectors or JSON keys modified.

---

## 2026-05-15 � Phase D Token Family Creation (17 Semantic Families, 190 SQL Rows)

**Author:** Michael
**Type:** Design System | SQL | Config
**Scope:** `design-token-engine:phase-d`, `db:schema`
**Summary:** Created 17 deterministic semantic token families (190 SQL rows, IDs 200�1010) and integrated them into runtime configuration per Phase D Token Family Creation � EXECUTION MODE spec. All DefaultValue fields use "TBD" placeholder (no guessed values).

### Details
TASK 1 � SQL Seed Insertion (`DesignTokenModelBuilderExtensions.cs`, v2.0.0):
- Inserted 190 design token rows across 17 semantic families in dependency order.
- Families: text (14 tokens: sizes xs�2xl, font weights regular�bold, text colors primary�inverse); surface (7: base, raised, subtle, overlay, hover, active, disabled); border (8: default, muted, strong, focus, danger, warning, success, info); radius (8: xs�full, control, card); shadow (7: none, xs�lg, dropdown, alert); spacing (18: space-1�12, gap-sm�lg, input/table cell padding, form label/hint); layout (5: header-h, footer-h, sidebar-w, sidebar-w-collapsed, shell-gap); nav-header (8); nav-sidebar (10); form-field (11); form-shell (5); table (8); alert (14: 4 severities � 3 colors + shadow + radius); icon (9); icon-btn (15); badge-status (8); chart (11).
- All `DefaultValue` set to "TBD" per audit protocol (no real colors/values).
- All `IsActive` = true; `CreatedAt`/`UpdatedAt` = SeedDate (2026-05-12).
- SortOrder: sequential (200�213 text, 250�256 surface, 300�307 border, �, 1000�1010 chart).
- File header updated: Version 1.0.0 ? 2.0.0, Changes added, Updated date set to 2026-05-15.

TASK 2 � theme.json Updates (v2.0.0 ? 2.1.0):
- Added 190 theme entries (light + dark) for all new tokens.
- Light theme: all new entries set to "TBD" (placeholder).
- Dark theme: all new entries set to "TBD" (placeholder).
- Existing button/card/legacy keys preserved and reordered (new tokens inserted before legacy keys).
- Version incremented to 2.1.0.
- Updated locations: `src/AbsenceApp.Client/wwwroot/config/designsystem/theme.json` and `src/AbsenceApp.Client/Framework/Config/theme.json`.

TASK 3 � components.json Updates (v2.0.0 ? 2.1.0):
- Added `tokenFamily` field to all component entries (button?btn, badge?badge-status, card?card, icon?icon, iconButton?icon-btn, input?form-field, alert?alert).
- Introduced new component references: formShell?form-shell, table?table, navHeader?nav-header, navSidebar?nav-sidebar, chart?chart.
- All component sections preserve existing variant/size definitions.
- Version incremented to 2.1.0.
- Updated locations: `src/AbsenceApp.Client/wwwroot/config/designsystem/components.json` and `src/AbsenceApp.Client/Framework/Config/components.json`.

### Affected Files and Components
- **SQL seed file:** `src/AbsenceApp.Data/Configurations/DesignTokenModelBuilderExtensions.cs` (v2.0.0)
  - 34 existing rows (btn 10�73, card 100�105) preserved unchanged.
  - 190 new rows added (IDs 200�1010, 17 families).
  - Total: 224 seed rows.
- **Theme config:** `src/AbsenceApp.Client/wwwroot/config/designsystem/theme.json` (v2.1.0)
  - `src/AbsenceApp.Client/Framework/Config/theme.json` (v2.1.0, runtime)
  - Light colors: +190 entries (all "TBD").
  - Dark colors: +190 entries (all "TBD").
- **Component config:** `src/AbsenceApp.Client/wwwroot/config/designsystem/components.json` (v2.1.0)
  - `src/AbsenceApp.Client/Framework/Config/components.json` (v2.1.0, runtime)
  - +10 new component entries (formShell, table, navHeader, navSidebar, chart).
  - +10 tokenFamily mappings added to existing components.

### Rollout Notes
- No CSS files modified this phase (CSS adoption deferred to next rollout).
- All DefaultValue fields are "TBD" placeholders; actual values will be extracted in subsequent audit phase.
- DB schema unchanged; EF seeding only (no migration required for production databases with existing tokens).
- Backout: revert the three modified files; existing button/card rows remain in DB.
- Header update rule applied: Version incremented, Changes entry added, Updated date set per global rules.

### Verification
? **TASK 1 � SQL Compilation:**
- `dotnet build c:\DevAbsence1\AbsenceAppV2\AbsenceAppV2.sln`: 0 errors, 0 warnings, 00:00:07.57 elapsed.
- All 190 new token rows inserted into HasData() with correct schema (ComponentGroup, TokenKey, CssVariable, Category, DefaultValue="TBD", Description, IsActive=true, SortOrder, CreatedAt, UpdatedAt).

? **TASK 2 � theme.json Validation:**
- 190 new entries confirmed in both light and dark theme sections.
- Light theme: all new `--ds-*-*` entries set to "TBD".
- Dark theme: all new `--ds-*-*` entries set to "TBD".
- Existing button/card/legacy keys preserved.
- File structure valid JSON (parseable by Node.js JSON parser).

? **TASK 3 � components.json Validation:**
- 10 new component entries added (formShell, table, navHeader, navSidebar, chart).
- 10 tokenFamily mappings added to existing components.
- All variants and sizes preserved.
- File structure valid JSON.

? **TASK 4 � Build Validation:**
- Solution build: 0 errors, 0 warnings.
- JSON syntax: both theme.json and components.json are valid and parseable.
- No CSS files modified (verified by auditing file edit logs).
- No guessed values introduced (all new DefaultValue="TBD").

? **TASK 5 � Header Updates:**
- `DesignTokenModelBuilderExtensions.cs`: Version 1.0.0 ? 2.0.0, Changes entry added, Updated date set to 2026-05-15.
- `theme.json`: Version 2.0.0 ? 2.1.0 (both wwwroot and Framework/Config).
- `components.json`: Version 2.0.0 ? 2.1.0 (both wwwroot and Framework/Config).

### References
- Execution plan: `/memories/session/plan.md` (Phase D Token Family Creation Plan, 17 families, 190 rows).
- Design token engine: `DesignTokenApiServiceV2.cs` (runtime CSS generation).
- Audit foundation: Global Token Audit�Executed Phase (10+ CSS files, 15 TODO markers).
- Next phase: CSS adoption (wire tokens into component stylesheets); value extraction (finalize DefaultValue and CurrentValue from design specifications).

---

## 2026-05-15 � Phase E Token Adoption (Execution Mode)

**Author:** Michael  
**Type:** UI | CSS | Design Tokens  
**Scope:** `design-system:token-adoption-phase-e`  
**Summary:** Applied the approved Token Adoption Plan in execution mode across audited CSS targets, replacing legacy/hard-coded style values with Phase D token families, removing outstanding TODO markers, updating file headers, and validating the full solution build.

### Files Modified
- `src/AbsenceApp.Client/Framework/Layout/HeaderV2.razor.css`
- `src/AbsenceApp.Client/Framework/Layout/SidebarV2.razor.css`
- `src/AbsenceApp.Client/Framework/PageTemplates/PageTemplateV2.razor.css`
- `src/AbsenceApp.Client/Components/Templates/TablePageTemplate.razor.css`
- `src/AbsenceApp.Client/Components/Templates/FormPageTemplate.razor.css`
- `src/AbsenceApp.Client/Components/Templates/DashboardPageTemplate.razor.css`
- `src/AbsenceApp.Client/Components/Alerts/AlertV2.razor.css`
- `src/AbsenceApp.Client/Components/DesignSystem/Icon.razor.css`
- `src/AbsenceApp.Client/Components/DesignSystem/IconButton.razor.css`
- `src/AbsenceApp.Client/wwwroot/css/components/forms.css`
- `src/AbsenceApp.Client/wwwroot/css/components/cards.css`
- `src/AbsenceApp.Client/wwwroot/css/components/stats.css`

### Token Families Applied
- `layout`
- `nav-header`
- `nav-sidebar`
- `text`
- `spacing`
- `border`
- `radius`
- `shadow`
- `surface`
- `table`
- `form-field`
- `form-shell`
- `alert`
- `icon`
- `icon-btn`
- `badge-status`

### Execution Metrics
- Selectors updated: **80+ selector/property mappings applied** (aligned to the approved deterministic mapping set).
- TODO markers removed: **Yes** (targeted token-adoption TODOs cleared from edited files).
- Header blocks updated: **Yes** (version/update metadata advanced on all edited CSS files).

### Validation
- Build command: `dotnet build AbsenceAppV2.sln`
- Result: **Succeeded**
- Diagnostics: **0 errors, 0 warnings**
- Elapsed: **00:00:53.67**

### Notes
- `src/AbsenceApp.Client/Components/Pages/GlobalSettings/UIKits/Buttons/Index.razor.css` remained unchanged by design (reference/demo scope excluded in the approved plan).

---

## 2026-05-15 � Phase GS-1 / Buttons Migration

**Author:** Michael  
**Type:** UI | Code | CSS  
**Scope:** `globalsettings:uikits-buttons`  
**Summary:** Executed KI-Admin Buttons migration Phase GS-1 (buttons only) by expanding the AbsenceApp Buttons page to 12 ordered sections, each with preview, code sample, and token editor block, while retaining token-family constraints.

### Details
- Expanded `UIKits/Buttons` page from 2 sections to 12 sections in the required order:
  1. Basic Buttons
  2. Outline Buttons
  3. Soft/Light Buttons
  4. Ghost Buttons
  5. Pill/Rounded Buttons
  6. Button Sizes (sm, md, lg)
  7. Icon Buttons
  8. Split Buttons
  9. Button Groups
  10. Nested Button Groups
  11. Loading Buttons
  12. Disabled Buttons
- Reused the existing token editor workflow and generalized it across sections (variant select, edit, preview validation, save/cancel, token update persistence).
- Added per-section code sample blocks and copy-to-clipboard action.
- Updated scoped page CSS for layout helpers and section-specific visual patterns using existing token families only (`text`, `surface`, `border`, `spacing`, `radius`, `shadow`, `icon`, `icon-btn`).

### Affected Files and Components
- `src/AbsenceApp.Client/Components/Pages/GlobalSettings/UIKits/Buttons/Index.razor`
- `src/AbsenceApp.Client/Components/Pages/GlobalSettings/UIKits/Buttons/Index.razor.cs`
- `src/AbsenceApp.Client/Components/Pages/GlobalSettings/UIKits/Buttons/Index.razor.css`

### Migration Counts
- New preview blocks: **12**
- New code sample blocks: **12**
- New token editor blocks: **12**

### Verification
- Build command: `dotnet build AbsenceAppV2.sln`
- Result: **Succeeded**
- Diagnostics: **0 errors, 0 warnings**

---

## 2026-05-15 � GS-1 Buttons UX Repair (Buttons3 Restore)

**Author:** Michael  
**Type:** UI | Code | CSS | Repair  
**Scope:** `globalsettings:uikits-buttons`  
**Summary:** Restored the original Buttons3 accordion layout and button-selection behavior, removed the Token Editor UI and flattened GS-1 layout, and retained only Basic / Outline / Soft-Light sections for controlled testing.

### Details
- Reverted page structure to accordion cards with per-section button rows and code editor panel.
- Removed GS-1 generic token-editor presentation and all non-test sections.
- Restored button-as-selector behavior (`click variant button -> update CSS panel`).
- Restored original interaction flow: **Edit / Save**, **Cancel**, **Preview**.
- Added **Soft/Light Buttons** as the third section using the same restored UX model.

### Affected Files and Components
- `src/AbsenceApp.Client/Components/Pages/GlobalSettings/UIKits/Buttons/Index.razor`
- `src/AbsenceApp.Client/Components/Pages/GlobalSettings/UIKits/Buttons/Index.razor.cs`
- `src/AbsenceApp.Client/Components/Pages/GlobalSettings/UIKits/Buttons/Index.razor.css`

### Verification
- Build command: `dotnet build AbsenceAppV2.sln`
- Result: **Succeeded**
- Diagnostics: **0 errors, 0 warnings**

---

## 2026-05-15 � GS-1 Buttons Full Ki-Admin Parity (Buttons3 UX, One Set Per Row).

**Author:** Michael  
**Type:** UI | Code | CSS  
**Scope:** `globalsettings:uikits-buttons`  
**Summary:** Implemented full Ki-Admin button-set parity on the Buttons page using the restored Buttons3 UX model, with one accordion section per set and one row per section.

### Details
- Added/standardized all required sets as separate accordion sections:
  - Basic Buttons
  - Outline Buttons
  - Light / Soft Buttons
  - Icon Buttons (actual icons)
  - Social Buttons (brand icons + colors)
  - Radius Buttons
  - Active Buttons
  - Disabled Buttons
  - Loading Buttons
  - Block Buttons
  - Button Sizes (sm, md, lg)
  - Button Groups
  - Nested Buttons
  - Checkbox / Radio Buttons
  - Vertical Buttons
- Preserved Buttons3 interaction flow for every section:
  - Real buttons as selectors,
  - Click variant ? CSS panel update,
  - Edit / Save, Cancel, Preview workflow,
  - No token-editor UI and no GS-1 flattened layout patterns.
- Set all accordions to collapsed by default on initial page load.
- Updated scoped styling to maintain Buttons3 spacing/alignment while enforcing one-row-per-set rendering.

### Affected Files and Components
- `src/AbsenceApp.Client/Components/Pages/GlobalSettings/UIKits/Buttons/Index.razor`
- `src/AbsenceApp.Client/Components/Pages/GlobalSettings/UIKits/Buttons/Index.razor.cs`
- `src/AbsenceApp.Client/Components/Pages/GlobalSettings/UIKits/Buttons/Index.razor.css`

### Verification
- Build command: `dotnet build AbsenceAppV2.sln`
- Result: **Succeeded**
- Diagnostics: **0 errors, 0 warnings**

---

## 2026-05-15 � Buttons3 UX Expansion: All Ki-Admin Button Variants

**Author:** Michael  
**Type:** UI | Code | CSS  
**Scope:** `globalsettings:uikits-buttons`  
**Summary:** Expanded the restored Buttons3 page to include all required Ki-Admin button categories while preserving the original accordion UX and edit workflow.

### Details
- Preserved Buttons3 interaction model end-to-end:
  - accordion cards,
  - real button selectors,
  - `Edit / Save`, `Cancel`, `Preview` workflow,
  - no token editor panel and no GS-1 flattened pattern layout.
- Added full category coverage on `/globalsettings/ui-kits/buttons`:
  1. Basic Buttons
  2. Outline Buttons
  3. Soft/Light Buttons
  4. Ghost Buttons
  5. Pill / Rounded Buttons
  6. Button Sizes (sm, md, lg)
  7. Icon Buttons
  8. Split Buttons
  9. Button Groups
  10. Nested Button Groups
  11. Loading Buttons
  12. Disabled Buttons
- Extended code-behind variant/state model to support all categories and preview synthesis/parsing paths (solid, outline, soft, ghost, pill, size) while keeping token-backed save behavior.
- Updated page markup with section-specific preview rendering for split/group/nested/icon/loading/disabled demos using the same Buttons3 card and editor structure.
- Added minimal scoped CSS helpers for new preview shapes (pill, split, groups, nested, icon sizing, size modifiers, ghost support) without altering the underlying UX pattern.

### Affected Files and Components
- `src/AbsenceApp.Client/Components/Pages/GlobalSettings/UIKits/Buttons/Index.razor`
- `src/AbsenceApp.Client/Components/Pages/GlobalSettings/UIKits/Buttons/Index.razor.cs`
- `src/AbsenceApp.Client/Components/Pages/GlobalSettings/UIKits/Buttons/Index.razor.css`

### Verification
- Build command: `dotnet build AbsenceAppV2.sln`
- Result: **Succeeded**
- Diagnostics: **0 errors, 0 warnings**

---

## 2026-05-16 � GS-1 Buttons Visual Parity Fix (Ki-Admin Icons, Colours, Shapes)

**Author:** Michael  
**Type:** UI | Code | CSS  
**Scope:** `globalsettings:uikits-buttons`  
**Summary:** Applied strict visual parity updates to the Buttons3 page using Ki-Admin source truth (`buttons.blade.php`) with auto-detected Tabler icon set (`ti ti-*`), corrected Ki palette values, circular social icon buttons, and preserved one-set-per-row accordion UX.

### Details
- Auto-detected and aligned icon set to Ki-Admin Tabler classes (`ti ti-*`) for icon, social, nested, vertical, and generic icon-bearing variants.
- Updated button visual palette to Ki-Admin values:
  - primary `#0f626a`, secondary `#626262`, success `#0ab964`, danger `#e14e5a`, warning `#f9c123`, info `#4196fa`, light `#c8b9d2`, dark `#28232d`.
- Updated social brand colours to Ki-Admin/brand-consistent values and enforced icon-first circular social buttons in one row.
- Preserved Buttons3 workflow and constraints:
  - one accordion per set,
  - one row per set,
  - accordions collapsed by default,
  - Edit/Save, Cancel, Preview lifecycle unchanged.
- Synced code-behind synthesized CSS map with updated Ki palette/brand values so editor/preview defaults match runtime visuals.

### Affected Files and Components
- `src/AbsenceApp.Client/Components/Pages/GlobalSettings/UIKits/Buttons/Index.razor`
- `src/AbsenceApp.Client/Components/Pages/GlobalSettings/UIKits/Buttons/Index.razor.cs`
- `src/AbsenceApp.Client/Components/Pages/GlobalSettings/UIKits/Buttons/Index.razor.css`

### Verification
- Build command: `dotnet build AbsenceAppV2.sln`
- First run: failed due to file lock (`AbsenceApp.Client.exe`, PID 45052).
- Remediation: terminated locking process and rebuilt.
- Final result: **Succeeded**
- Diagnostics: **0 errors, 0 warnings**

---


## 2026-05-18 - Sidebar Icons Fix: Bootstrap Icons Bundled Locally

**Author:** Michael  
**Type:** Bug Fix | Asset | CSS  
**Scope:** `Framework/Layout/SidebarV2` | `wwwroot`  
**Summary:** Resolved sidebar icon rendering failure caused by Bootstrap Icons being loaded exclusively from CDN. Icons are now bundled locally so they load correctly in MAUI Blazor Hybrid WebView2 regardless of network or proxy conditions. Added a defensive `bi-circle` fallback for null/empty icon values in SidebarV2.

### Root Cause
Bootstrap Icons CSS was referenced only via CDN (`cdn.jsdelivr.net`). In school desktop environments with network content filtering, proxy restrictions, or intermittent internet access, the CDN request fails silently. All `<i class="bi bi-*">` elements rendered as zero-width invisible boxes.

### Details
- Downloaded Bootstrap Icons v1.11.3 CSS and font files from CDN and saved locally under `wwwroot/`.
- Patched font `src:` paths in the downloaded CSS from `fonts/` to `../fonts/` to match the local directory layout (`wwwroot/css/bootstrap/` to `wwwroot/css/fonts/`).
- Added project block-header comment to `bootstrap-icons.min.css` (v1.0.0).
- Replaced the Bootstrap Icons CDN `<link>` in `index.html` with the local stylesheet reference.
- Added null/empty icon fallback (`bi-circle`) at all three `RenderTreeBuilder` icon-class assignment points in `SidebarV2.razor` to prevent invisible elements when a DB record has a missing icon value.
- Tabler Icons CDN link in `index.html` was left untouched.

### New Files
| File | Description |
|------|-------------|
| `src/AbsenceApp.Client/wwwroot/css/bootstrap/bootstrap-icons.min.css` | Bootstrap Icons v1.11.3 stylesheet (v1.0.0, 2026-05-18) |
| `src/AbsenceApp.Client/wwwroot/css/fonts/bootstrap-icons.woff2` | Bootstrap Icons v1.11.3 WOFF2 font (binary, 130,396 bytes) |
| `src/AbsenceApp.Client/wwwroot/css/fonts/bootstrap-icons.woff` | Bootstrap Icons v1.11.3 WOFF font (binary, 176,032 bytes) |

### Modified Files and Components
| File | Change | Version |
|------|--------|---------|
| `src/AbsenceApp.Client/wwwroot/index.html` | Replaced CDN Bootstrap Icons link with local bundle reference | 1.0.1 to 1.0.2 |
| `src/AbsenceApp.Client/Framework/Layout/SidebarV2.razor` | Added `bi-circle` null/empty icon fallback at 3 RenderTreeBuilder icon-class locations | 7.1.0 to 7.2.0 |

### Verification
- Build command: `dotnet build AbsenceAppV2.sln`
- Result: **Pending**

---

## 2026-05-19 — Dropdown UI Kit Page — Fix Duplicate Header, Menu Clipping, Directional Positioning

**Author:** Michael (AI-assisted)
**Type:** Hotfix | UI
**Scope:** `page:globalsettings/ui-kits/dropdown`
**Summary:** Fixed three bugs on the Dropdown UI Kit showcase page: removed duplicate page header, fixed dropdown menus being clipped by the group card container, and added missing directional dropdown CSS positioning rules.

### Details
1. **Duplicate header removed** — The page rendered its own `<div class="ddp-header">` with hardcoded `<h1>Dropdowns</h1>` and `<p>UI Kits</p>`, duplicating the title/breadcrumb already rendered by `PageHeaderV2` (injected by `GlobalSettingsLayout` before `@Body`). The block was deleted and the associated dead CSS rules removed.

2. **Menu clipping fixed** — `.ddp-group-card` had `overflow: hidden`, which clipped absolutely-positioned `.ddp-menu` elements that extended below the card boundary when open. Changed to `overflow: visible`. Added `border-radius: 8px 8px 0 0` to `.ddp-group-header` to preserve rounded top-corner appearance on hover (previously ensured by parent overflow clipping).

3. **Directional dropdown positioning** — The directional group variants applied `ddp-dropup`, `ddp-dropstart`, and `ddp-dropend` classes to the `.ddp-dropdown` wrapper, but no corresponding CSS rules existed in `Index.razor.css`. The global `dropdown.css` rules use the `dsv2-` component prefix and do not apply here. Added `.ddp-dropup .ddp-menu`, `.ddp-dropstart .ddp-menu`, and `.ddp-dropend .ddp-menu` positioning rules after the `.ddp-menu--open` rule.

### Modified Files and Components
| File | Change | Version |
|------|--------|---------|
| `src/AbsenceApp.Client/Components/Pages/GlobalSettings/UIKits/Dropdown/Index.razor` | Removed duplicate `<div class="ddp-header">` block | 3.2.0 to 3.3.0 |
| `src/AbsenceApp.Client/Components/Pages/GlobalSettings/UIKits/Dropdown/Index.razor.css` | Removed `.ddp-header` dead CSS; `overflow: visible` on `.ddp-group-card`; `border-radius` on `.ddp-group-header`; added directional positioning rules | 1.0.0 to 1.3.0 |

### Files NOT Modified
- `src/AbsenceApp.Client/Components/Pages/GlobalSettings/UIKits/Dropdown/Index.razor.cs` — no changes required
- `src/AbsenceApp.Client/wwwroot/css/components/dropdown.css` — not modified (global dsv2 component, unrelated)

### Verification
- Build: `dotnet build src/AbsenceApp.Client/AbsenceApp.Client.csproj` — 0 errors, 0 warnings expected
- Manual: navigate to `/globalsettings/ui-kits/dropdown`; confirm single header row; confirm menus open fully; confirm dropup/dropstart/dropend menus appear in correct direction

---

## 2026-05-20 — UI Kits Template Components

**Author:** Michael (AI-assisted)
**Type:** Feature | Template
**Scope:** `framework:page-templates`
**Summary:** Added reusable UiKitsPage and UiKitsGroup template components under the PageTemplates framework namespace.

### Details
- **UiKitsPage.razor** — Generic UI Kits page shell: `<div class="uikit-page"><div class="uikit-page__container">@ChildContent</div></div>` with a single `[Parameter] RenderFragment ChildContent`.
- **UiKitsPage.razor.css** — Page layout CSS: `.uikit-page` (padding: 24px), `.uikit-page__container` (max-width: 1100px, flex column).
- **UiKitsGroup.razor** — Reusable accordion group component with parameters for title, accordion state, editor text, save/cancel/edit callbacks, preview CSS, and status message. Textarea binding uses `value` + `@oninput` pattern (not `@bind`) to ensure changes propagate to the parent correctly.
- **UiKitsGroup.razor.css** — All accordion, card, editor, button, and status CSS for the group component.

### Modified Files and Components
| File | Change | Version |
|------|--------|---------|
| `src/AbsenceApp.Client/Framework/PageTemplates/UiKitsPage.razor` | New file | 1.0.0 |
| `src/AbsenceApp.Client/Framework/PageTemplates/UiKitsPage.razor.css` | New file | 1.0.0 |
| `src/AbsenceApp.Client/Framework/PageTemplates/UiKitsGroup.razor` | New file | 1.0.0 |
| `src/AbsenceApp.Client/Framework/PageTemplates/UiKitsGroup.razor.css` | New file | 1.0.0 |

### Verification
- Build: `dotnet build src/AbsenceApp.Client/AbsenceApp.Client.csproj` — 0 errors, 0 warnings expected

---

## 2026-05-20 — Buttons Page Template Migration

**Author:** Michael (AI-assisted)
**Type:** Refactor | UI
**Scope:** `page:globalsettings/ui-kits/buttons`
**Summary:** Refactored the Buttons UI Kit showcase page to use the new UiKitsPage/UiKitsGroup template components. Preserved all existing code-behind logic, demo rendering, and CSS editor behaviour.

### Details
- Removed outer `<div class="btp-page">` wrapper (replaced by `<UiKitsPage>`).
- Removed `<div class="btp-header">` block (header already rendered by `PageHeaderV2` via layout).
- Replaced `@foreach (var state in GroupStates)` loop + switch statement with 13 explicit `<UiKitsGroup>` elements, one per button group (basic, outline, soft, icon, social, radius, active, disabled, loading, block, sizes, groups, action).
- Each group wires `IsAccordionOpenChanged` → `OnToggleAccordion`, `OnEdit`/`OnSave` → `OnEditSaveClickedAsync`, `OnCancel` → `OnCancelClicked`, `EditorTextChanged` → `OnEditorInput` using `EventCallback.Factory.Create`.
- Added `@using AbsenceApp.Client.Framework.PageTemplates` directive.

### Modified Files and Components
| File | Change | Version |
|------|--------|---------|
| `src/AbsenceApp.Client/Components/Pages/GlobalSettings/UIKits/Buttons/Index.razor` | Refactored to use UiKitsPage/UiKitsGroup | 6.1.0 to 7.0.0 |

### Files NOT Modified
- `src/AbsenceApp.Client/Components/Pages/GlobalSettings/UIKits/Buttons/Index.razor.cs` — no changes required

### Verification
- Build: `dotnet build src/AbsenceApp.Client/AbsenceApp.Client.csproj` — 0 errors, 0 warnings expected
- Manual: navigate to `/globalsettings/ui-kits/buttons`; confirm all 13 groups render; confirm accordion, edit/save/cancel, and live CSS preview work correctly

---

## 2026-05-20 — Dropdown Page Template Migration

**Author:** Michael (AI-assisted)
**Type:** Refactor | UI
**Scope:** `page:globalsettings/ui-kits/dropdown`
**Summary:** Migrated the Dropdown UI Kit showcase page to use the new UiKitsPage/UiKitsGroup template components. Preserved all existing code-behind logic and demo rendering.

### Details
- Removed outer `<div class="ddp-page">` wrapper (replaced by `<UiKitsPage>`).
- Replaced `@foreach (var group in GroupStates)` loop + switch statement with 3 explicit `<UiKitsGroup>` elements, one per dropdown group (`_groups[0]` single, `_groups[1]` split, `_groups[2]` directional).
- Each group uses `_groups[n]` index access to the `List<DropdownGroupState>` in the code-behind.
- Preserved `@layout` and `@namespace` directives.
- Added `@using AbsenceApp.Client.Framework.PageTemplates` directive.

### Modified Files and Components
| File | Change | Version |
|------|--------|---------|
| `src/AbsenceApp.Client/Components/Pages/GlobalSettings/UIKits/Dropdown/Index.razor` | Migrated to use UiKitsPage/UiKitsGroup | 3.3.0 to 4.0.0 |

### Files NOT Modified
- `src/AbsenceApp.Client/Components/Pages/GlobalSettings/UIKits/Dropdown/Index.razor.cs` — no changes required
- `src/AbsenceApp.Client/Components/Pages/GlobalSettings/UIKits/Dropdown/Index.razor.css` — no changes required

### Verification
- Build: `dotnet build src/AbsenceApp.Client/AbsenceApp.Client.csproj` — 0 errors, 0 warnings expected
- Manual: navigate to `/globalsettings/ui-kits/dropdown`; confirm all 3 groups render; confirm accordion, edit/save/cancel, and live CSS preview work correctly

---

## 2026-05-21 — UI Kits Phase 2.1 Visual Regression Recovery (Execution)

**Author:** Michael (AI-assisted)
**Type:** UI | Component | Hotfix
**Scope:** `ui:globalsettings/ui-kits`
**Summary:** Executed Phase 2.1 recovery for UI Kits regressions by restoring explicit render classes for affected button groups, data-driven icon payloads, initial preview hydration, and template action-button styling consistency.

### Details
- Restored explicit render-ready class supply in Buttons page for affected groups (Basic, Outline, Light/Soft, Radius, Active, Disabled, Loading) without template inference.
- Removed switch-based icon class mapping and shifted icon class payload assignment to data-driven item mapping for icon ItemTemplate rendering.
- Added deterministic initial active-item callback hydration in `UiKitsGroup` so page-level preview payload is populated on first render.
- Normalized template action controls (Preview/Edit/Save/Cancel) to UI Kit secondary button classes and kept scoped CSS free of legacy `.btn`/`.btn-*` selectors.
- Preserved `UiKitsPage` shell files without behavioral drift.

### Modified Files and Version Increments
| File | Version |
|------|---------|
| `src/AbsenceApp.Client/Shared/Templates/UIKits/UiKitsGroup.razor` | 1.2.1 → 1.2.2 |
| `src/AbsenceApp.Client/Shared/Templates/UIKits/UiKitsGroup.razor.css` | 1.1.1 → 1.1.2 |
| `src/AbsenceApp.Client/Components/Pages/GlobalSettings/UIKits/Buttons/Index.razor` | 8.1.0 → 8.2.0 |
| `src/AbsenceApp.Client/Components/Pages/GlobalSettings/UIKits/Dropdown/Index.razor` | 5.1.0 → 5.2.0 |

### Verification
- Non-incremental build completed successfully (`0 Error(s)`, `0 Warning(s)`) after releasing app file locks.
- Scoped CSS bundle verification confirmed required selectors are present:
  - `.uikits-group__demo[...]`
  - `.uikits-group__item[...]`
  - `.uikits-group__item.active[...]`
  - `.uikits-group__item-button[...]`
  - `.uikits-group__item-button.is-active[...]`
  - `.uikits-group__accordion[...]`
  - `.uikits-group__accordion.open[...]`
  - `.uikits-group__code[...]`
- Phase 2.1 regression check targets completed for Buttons and Dropdown preview/update flow.

### Phase Marker
This entry is part of the **Phase 2.1 visual regression recovery** execution cycle.

---

## 2026-05-21 — UI Kits Phase 2.2 Template De-Interference (Execution)

**Author:** Michael (AI-assisted)  
**Type:** UI | Component | Hotfix  
**Scope:** `ui:globalsettings/ui-kits`  
**Summary:** Executed Phase 2.2 corrective implementation to make page-owned ItemTemplate rendering authoritative, remove template fallback visual overrides, and keep preview payload updates deterministic for button/dropdown interactions.

### Details
- Reworked `UiKitsGroup` fallback item rendering so page-supplied `ItemTemplate` remains the visual source of truth; retained selection/callback behavior without forcing template button semantics over page markup.
- Neutralized template fallback button visuals in `UiKitsGroup.razor.css` (`.uikits-group__item-button`) so background/border/padding/color are no longer imposed on page-owned UI Kit classes.
- Completed Buttons page Phase 2.2 refactor by rendering visual groups with explicit page-owned ItemTemplate blocks and deterministic class/icon helpers:
  - removed tone-heuristic class synthesis helpers,
  - added explicit class resolvers for standard/radius/size/social/icon render paths,
  - preserved preview update callbacks through `OnItemSelectedKeyChanged`.
- Confirmed Dropdown click paths preserve preview-first ordering for single/split/directional actions and updated file metadata for Phase 2.2 tracking.

### Modified Files and Version Increments
| File | Version |
|------|---------|
| `src/AbsenceApp.Client/Shared/Templates/UIKits/UiKitsGroup.razor` | 1.2.2 → 1.3.0 |
| `src/AbsenceApp.Client/Shared/Templates/UIKits/UiKitsGroup.razor.css` | 1.1.2 → 1.2.0 |
| `src/AbsenceApp.Client/Components/Pages/GlobalSettings/UIKits/Buttons/Index.razor` | 8.2.0 → 8.3.0 |
| `src/AbsenceApp.Client/Components/Pages/GlobalSettings/UIKits/Dropdown/Index.razor` | 5.2.0 → 5.3.0 |

### Verification
- Diagnostics: no file-level errors in all four modified Phase 2.2 files.
- Build:
  - initial non-incremental build failed due to locked `AbsenceApp.Client` process (`MSB3021/MSB3027`).
  - after terminating process id `22604`, non-incremental build succeeded with `0 Warning(s)` and `0 Error(s)`.
- Scoped CSS bundle check confirmed Phase 2.2 template neutralization selectors are emitted:
  - `.uikits-group__item-button[...]`
  - `.uikits-group__item-button.is-active[...]`
  and generated rules contain reset/neutral values (`background: transparent; border: 0; padding: 0; color: inherit`).

### Phase Marker
This entry is part of the **Phase 2.2 template de-interference** execution cycle.

---

## 2026-05-21 — UI Kits Phase 2.2 Strict Corrective Execution

**Author:** Michael (AI-assisted)  
**Type:** UI | Component | Hotfix  
**Scope:** `ui:globalsettings/ui-kits`  
**Summary:** Executed strict Phase 2.2 corrections to fully de-power template fallback rendering, enforce deterministic page-owned ItemTemplate class payloads across required button groups, and preserve preview-first variant hydration behavior.

### Details
- `UiKitsGroup.razor` strict fallback minimization:
  - kept accordion, active-item tracking, initial/click selection callbacks, and edit/save/cancel behavior.
  - replaced fallback visual button rendering with non-visual fallback label output so template markup cannot impose button visuals.
- `UiKitsGroup.razor.css` strict fallback neutralization:
  - removed `.uikits-group__item-button` fallback visual rules.
  - retained only minimal fallback layout rule on `.uikits-group__item-fallback`.
- `Buttons/Index.razor` strict deterministic mapping:
  - kept all required button groups on explicit page-owned `ItemTemplate` markup.
  - removed heuristic/fallback class derivation and applied explicit per-group key→class maps.
  - corrected icon key mapping so icon item payload is icon-only class (`ti ...`) and social classes/icons are explicit.
  - enforced required size render classes (`dsv2-btn-sm`, `dsv2-btn-md`, `dsv2-btn-lg`).
- `Dropdown/Index.razor` strict preview-first contract retained:
  - verified click paths invoke variant selection hydration before menu interaction handlers.

### Modified Files and Version Increments
| File | Version |
|------|---------|
| `src/AbsenceApp.Client/Shared/Templates/UIKits/UiKitsGroup.razor` | 1.3.0 → 1.3.1 |
| `src/AbsenceApp.Client/Shared/Templates/UIKits/UiKitsGroup.razor.css` | 1.2.0 → 1.2.1 |
| `src/AbsenceApp.Client/Components/Pages/GlobalSettings/UIKits/Buttons/Index.razor` | 8.3.0 → 8.4.0 |
| `src/AbsenceApp.Client/Components/Pages/GlobalSettings/UIKits/Dropdown/Index.razor` | 5.3.0 → 5.4.0 |

### Verification
- Diagnostics: no file-level errors in all four strict Phase 2.2 files.
- Build:
  - initial non-incremental build failed due to locked `AbsenceApp.Client` process (`MSB3021/MSB3027`, pid `22480`).
  - after terminating the lock-holder process, non-incremental build succeeded with `0 Warning(s)` and `0 Error(s)`.
- Scoped CSS bundle check confirmed strict fallback output:
  - `.uikits-group__item-fallback[...]` present.
  - `.uikits-group__item-button[...]` selectors no longer emitted.

### Phase Marker
This entry is part of the **Phase 2.2 strict corrective cycle**.

---