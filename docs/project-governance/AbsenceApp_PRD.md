================================================================================
 File        : AbsenceApp_PRD.md
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-04-16
 Updated     : 2026-04-16
--------------------------------------------------------------------------------
 Purpose     :
   Master product requirements, architecture, schema, coding standards, and 
   design reference for AbsenceApp V2.
--------------------------------------------------------------------------------
 Changes     :
   - 1.0.0 (2026-04-16) Initial creation from all docs in C:\DevAbsence1\docs\
--------------------------------------------------------------------------------
 Notes       :
   - This file defines the authoritative requirements for AbsenceApp V2.
   - It must be loaded before any project work begins.
================================================================================

## 1. Executive Summary

AbsenceApp V2 is a **Windows-only, local-network, school-based absence management system** built as a .NET 8 MAUI Blazor Hybrid desktop application. It is the primary tool for office staff to record and track student absences, manage staff absence data, and maintain a centralised and auditable record for a single or multi-school site.

AbsenceApp V2 is **not the school's main MIS system**. It is a supplemental records tool designed to complement MIS data, not replace it. Its primary function is to record parent/carer phone-ins and students sent home sick, cross-referenced against MIS-imported student data.

The V2 designation refers to a major design-system overhaul that runs alongside the existing V1 application in a **non-breaking overlay pattern**, progressively replacing V1 components in ten defined phases.

---

## 2. Problem Statement

### Problems AbsenceApp V2 Solves

- No dedicated tool exists for recording parent/carer phone-ins about student absences.
- No local, auditable record of students sent home mid-day.
- No structured tracking of staff absence from a local, school-controlled system.
- School relies on manual processes, spreadsheets, or MIS workarounds for day-to-day absence management.
- No automated alerting for persistent absence patterns.
- No unified access control aligned to school staff roles and responsibilities.

### Explicit Non-Problems (Out of Scope)

- Replacing the school's primary MIS system (e.g., SIMS, Arbor, etc.).
- Full HR management.
- Online or cloud deployment.
- Mobile device support.
- Payroll integration.

---

## 3. Goals and Non-Goals

### Functional Goals

- Record parent/carer phone-in absence notifications for students.
- Record students sent home sick during the school day.
- Import student data from MIS via CSV (UPN, Name, Class, Year Group, DOB, SEN flags, Medical flags).
- Track staff absences (sickness, leave, etc.).
- Provide role-based access for office staff, head teacher, SLT, SEND coordinator, and admin.
- Auto-detect Windows login for seamless authentication.
- Export records to CSV and PDF (available to all users).
- Trigger persistent absence alerts and email reminders to teachers.
- Maintain a full, append-only audit trail of all changes.
- Enforce field-level access restrictions by role.

### Non-Goals

- Full autonomous absence management or MIS replacement.
- Mobile or web delivery.
- Cloud storage or remote access.
- Automated two-way MIS integration.
- Medical logs, dietary needs, safeguarding, or behaviour tracking (deferred to future phases).

---

## 4. Functional Requirements

### 4.1 Core Application Requirements

| ID   | Requirement |
|------|-------------|
| F-01 | The application must launch with a splash screen, transition smoothly to a login screen, and then to the main shell. |
| F-02 | The application shell must include a sidebar, header, breadcrumb, and main content area. |
| F-03 | The sidebar must be collapsible and expandable with smooth animation. |
| F-04 | The breadcrumb must always reflect the current page route and replace the page title entirely. |
| F-05 | The theme must support Light and Dark modes with a toggle in the header. |
| F-06 | Theme state must persist across minimise/restore. Theme reset on cold restart is a known defect to be resolved. |
| F-07 | The application must handle navigation errors via a global error boundary. This is an open critical defect in V1. |
| F-08 | Window size and position must be persisted between application launches. |
| F-09 | The application must support minimise-to-tray behaviour. |
| F-10 | The application must support multi-monitor usage without DPI artefacts. |

### 4.2 User Management Requirements

| ID   | Requirement |
|------|-------------|
| F-11 | Three access levels: Super Admin (full access), Office Staff (full access except access rights management), Others (reporting only). |
| F-12 | Windows login must be auto-detected to pre-fill or auto-authenticate the user. |
| F-13 | Role assignment must be managed by Admin/Super Admin only. |
| F-14 | Users may optionally be linked to a staff record, but this relationship is not required. |
| F-15 | Account verification events must be logged. |

### 4.3 Staff Management Requirements

| ID   | Requirement |
|------|-------------|
| F-16 | Staff are tracked separately from user accounts. A staff member may or may not have a system login. |
| F-17 | Each staff member belongs to a department, job title, job group, and school site. |
| F-18 | Staff can hold responsibilities (e.g., Head of Year). |
| F-19 | Staff can be assigned to classes or duties via StaffAssignments. |
| F-20 | Staff devices must be tracked (laptops, tablets, etc.) with an audit trail. |
| F-21 | Staff external accounts (Google, Microsoft, etc.) must be tracked with an audit trail. |
| F-22 | Staff absences must be recorded with an audit trail. |

### 4.4 Student Management Requirements

| ID   | Requirement |
|------|-------------|
| F-23 | Students must be importable via CSV from MIS with the following fields: UPN, Name, Class, Year Group, DOB, SEN flag, Medical flag, Reason, Duration, Doctor's note, Caller name. |
| F-24 | Student contacts (parent/guardian/emergency) must be stored and linked to the student record. |
| F-25 | Student flags must support structured markers: behaviour, medical, pastoral, SEN, attendance concern. |
| F-26 | Student medical information must be stored separately from core identity. |
| F-27 | Students are linked to classes via the ClassMember table (many-to-many). |
| F-28 | Classes are linked to year groups via ClassYearGroupAssignments (many-to-many). |

### 4.5 Absence Tracking Requirements

| ID   | Requirement |
|------|-------------|
| F-29 | Student absences must be recorded with: student, absence type, date, duration, reason, caller name, doctor's note flag. |
| F-30 | Staff absences must be recorded with: staff member, absence type, dates, reason. |
| F-31 | Absence types must be seeded from a canonical reference table. |
| F-32 | Persistent absence must trigger an alert (configurable threshold). |
| F-33 | Email reminders must be sent to class teachers for student absences. |
| F-34 | All absence changes must generate an entry in the relevant audit table. |

### 4.6 Data Import and Export Requirements

| ID   | Requirement |
|------|-------------|
| F-35 | CSV import from MIS must map to the canonical table structure; failed rows must be identifiable. |
| F-36 | CSV export of absence records must be available to all user roles. |
| F-37 | PDF export of absence records must be available to all user roles. |

### 4.7 Navigation and UI Requirements

| ID   | Requirement |
|------|-------------|
| F-38 | Breadcrumbs are always shown and completely replace the page title. |
| F-39 | Page layout is hybrid: fluid/responsive for tables and dashboards; fixed-width for forms. |
| F-40 | Form spacing is 16px; forms use a fully responsive grid of 1–3 columns; save/cancel buttons are right-aligned. |
| F-41 | Tables must have: sticky header, 40px row height, zebra striping, hover highlight. |
| F-42 | Row selection is click-to-select (no bulk selection). |
| F-43 | Table actions are inline icon-only buttons. |
| F-44 | Column alignment defaults: text = left, numbers = right, status = centred (configurable). |
| F-45 | Empty state message in tables: "No records found". |
| F-46 | Table loading state: skeleton rows. |
| F-47 | Tables have a filter row under the column headers. |
| F-48 | Search is restricted to columns explicitly marked as "Searchable". |
| F-49 | Icon set is Fluent UI System Icons exclusively. No alternatives permitted. |

### 4.8 Security Requirements

| ID   | Requirement |
|------|-------------|
| F-50 | The database must be encrypted at rest. |
| F-51 | Windows login must be the only authentication mechanism. |
| F-52 | A full, append-only audit trail must exist for all user-driven changes. |
| F-53 | Restricted fields must not be visible to users without the correct role. |
| F-54 | Role-based menu filtering must enforce visibility at the navigation layer (default deny). |
| F-55 | Role-based feature gating must enforce permissions at the feature/action layer. |
| F-56 | Login attempts, successes, failures, and lockouts must be recorded in LoginAudit. |

### 4.9 Notifications Requirements

| ID   | Requirement |
|------|-------------|
| F-57 | Persistent absence notifications must display a notification icon in the application header. |
| F-58 | Email reminders must be sent to teachers for student absences. |

### 4.10 System Management Requirements

| ID   | Requirement |
|------|-------------|
| F-59 | Global settings must be configurable by Super Admin via the GlobalConfig table. |
| F-60 | EnableGlobalSettings and EnableRoleBasedNavigation flags control master system behaviour. |
| F-61 | Menu items are managed via the ui_MenuItems table (DB-first, single source of truth). |
| F-62 | Role-to-feature mappings are managed via perm_RoleFeatureMap. |

---

## 5. Non-Functional Requirements

| ID   | Requirement |
|------|-------------|
| NF-01 | Platform: Windows only. No web, no mobile. |
| NF-02 | Concurrent users: up to 5+ on a local network. |
| NF-03 | Database host: SQL Server Express (original) / MariaDB 10.4.32 via XAMPP (current). |
| NF-04 | Database hosted on a local school server, not cloud. |
| NF-05 | All application transitions (splash → login → shell) must be smooth with no white flash, jitter, or partial renders. |
| NF-06 | The sidebar must be interactive immediately after login. |
| NF-07 | Window state (size, position) must persist across restarts. |
| NF-08 | Scrollbars must be styled consistently with the application theme. |
| NF-09 | All code must comply with the coding standards documented in Section 10. |
| NF-10 | The codebase must have no build errors or warnings at any commit. |
| NF-11 | All new tests must follow the xUnit Arrange/Act/Assert pattern with named test identifiers. |

---

## 6. Architecture Overview

### 6.1 Technology Stack

| Layer              | Technology |
|--------------------|-----------|
| UI Platform        | .NET 8 MAUI Blazor Hybrid (Windows desktop) |
| UI Framework       | Blazor (Razor components) |
| Styling            | CSS + Bootstrap 5 (no XAML styles) |
| Icons              | Fluent UI System Icons |
| API                | ASP.NET Core Minimal API |
| ORM                | Entity Framework Core 8.0.2 |
| Database Provider  | Pomelo.EntityFrameworkCore.MySql 8.0.2 |
| Database           | MariaDB 10.4.32 via XAMPP |
| Testing (Server)   | xUnit |
| Testing (Client)   | xUnit + bUnit |
| Analyzer           | Custom Roslyn analyzer (AA0001 — file header) |
| Source Control     | Git with pre-commit hooks |

### 6.2 Solution Projects

| Project                 | SDK                        | Purpose |
|-------------------------|----------------------------|---------|
| `AbsenceApp.Core`       | `Microsoft.NET.Sdk`        | Domain models, DTOs, interfaces, ViewModels. No references to Data, Client, or Api. |
| `AbsenceApp.Data`       | `Microsoft.NET.Sdk`        | EF Core DbContext, entities, configurations, repositories, services, mappers, seeder. |
| `AbsenceApp.Client`     | `Microsoft.NET.Sdk.Razor`  | .NET MAUI Blazor Hybrid UI — all Razor components, pages, and layout. |
| `AbsenceApp.EfHost`     | `Microsoft.NET.Sdk`        | Startup project for EF Core migrations and seeder execution. |
| `AbsenceApp.Api`        | `Microsoft.NET.Sdk.Web`    | ASP.NET Core Minimal API — REST endpoints with Swagger/OpenAPI. |
| `AbsenceApp.Analyzers`  | `Microsoft.NET.Sdk`        | Roslyn diagnostic analyzer enforcing file header comment (AA0001). |
| `AbsenceApp.Updater`    | `Microsoft.NET.Sdk`        | Auto-update stub (not yet implemented). |
| `AbsenceApp.Tests`      | `Microsoft.NET.Sdk`        | xUnit server-side unit tests. |
| `AbsenceApp.Client.Tests`| `Microsoft.NET.Sdk`       | xUnit + bUnit client-side component tests. |

### 6.3 Layer Dependencies

```
AbsenceApp.Client  ──────────────────────────────┐
AbsenceApp.Api     ──────────────────────────────┤
                                                  ↓
AbsenceApp.Data  ─────────────────────────────► AbsenceApp.Core
                                                  ↑
AbsenceApp.EfHost  ──────────────────────────────┘

AbsenceApp.Analyzers  (standalone — no project references)
AbsenceApp.Updater    (standalone — no project references)
AbsenceApp.Tests → AbsenceApp.Data, AbsenceApp.Core
AbsenceApp.Client.Tests → AbsenceApp.Client, AbsenceApp.Core
```

**Core constraint:** `AbsenceApp.Core` must never reference Data, Client, or Api.

### 6.4 Design Principles

| Principle | Rule |
|-----------|------|
| Core-First | Core has no upward references. All other projects reference Core. |
| DTO at Boundary | Services return DTOs, not entities. Entities must not cross layer boundaries. |
| Static Mappers | Mapper classes are static and never injected. |
| Repository Pattern | All data access is via repositories, injected through interfaces. |
| Centralised DI | All DI registration in `DataServiceRegistration.AddDataLayer()`. |
| Validation in Services | Validation logic lives in services, not repositories. `ArgumentException` used for blank required fields. |
| Append-Only Audit | Audit log records are never updated or deleted via the service layer. |

---

## 7. Domain Model

The database is logically divided into six domains. In the original SQL Server design these were SQL schemas; in the MySQL migration they are all tables in the single `absenceapp` database.

### 7.1 Staff Domain (`org` schema)

| Table | Purpose |
|-------|---------|
| `Staff` | Core HR identity record for each staff member. Links to department, job title, job group, and school. |
| `StaffDepartments` | Lookup — staff department names. |
| `StaffJobTitles` | Lookup — job title names. |
| `StaffJobGroups` | Lookup — job group names. |
| `StaffSchools` | Lookup — school sites or campuses. |
| `StaffAbsences` | Records staff absence events (sickness, leave, etc.). |
| `StaffAssignments` | Links staff to classes, duties, or responsibilities. |
| `StaffDevices` | Records devices assigned to staff. |
| `StaffExternalAccounts` | External linked accounts (Google, Microsoft, etc.). |
| `Responsibilities` | Lookup — staff responsibilities (e.g., Head of Year). |

### 7.2 Students Domain (`core` schema)

| Table | Purpose |
|-------|---------|
| `Students` | Core identity record for each student. |
| `StudentContacts` | Parent/guardian/emergency contact details linked to a student. |
| `StudentFlags` | Structured flags: behaviour, medical, pastoral, SEN, attendance concern. |
| `StudentMedical` | Medical information for a student. |
| `StudentAbsences` | Records student absence events. |
| `StudentAbsenceAudit` | Audit trail for changes to student absences. |

### 7.3 Users and Permissions Domain (`auth` schema)

| Table | Purpose |
|-------|---------|
| `Users` | Login accounts for authorised system users (separate from Staff). |
| `UserProfiles` | Extended profile details linked to Users. |
| `Roles` | Defines system roles (SuperAdmin, Admin, Teacher, etc.). |
| `RoleTypes` | Lookup — role category names (Admin, StaffAdmin, Teacher). |
| `UserRole` | Maps Users to Roles (many-to-many). |
| `perm_FeatureFlags` | Defines system and UI feature flags (previously `Feature`). |
| `perm_RoleFeatureMap` | Maps Roles to feature flags (previously `RoleFeature`). |
| `perm_UserFeatureOverride` | User-level overrides for feature behaviour (previously `UserFeatureOverride`). |

### 7.4 Academic Organisation Domain (`core` schema)

| Table | Purpose |
|-------|---------|
| `Classes` | Defines teaching groups and classes. |
| `ClassMember` | Links students to classes (many-to-many). |
| `ClassYearGroupAssignments` | Links classes to year groups (many-to-many). |
| `YearGroups` | Lookup — year group names (Y1, Y2, KS1, etc.). |
| `Phases` | Lookup — school phases (EYFS, KS1, KS2, etc.). |
| `Houses` | Lookup — school house names. |

### 7.5 Operational Domain (`ops` schema)

| Table | Purpose |
|-------|---------|
| `StaffAbsences` | Staff absence records (cross-referenced with Staff). |
| `StudentAbsences` | Student absence records. |
| `AbsenceTypes` | Lookup — absence type names and codes. |
| `DeviceTypes` | Lookup — device type names. |
| `ExternalSystems` | Lookup — external system names (Google, Microsoft, etc.). |

### 7.6 Audit Domain (`audit` schema)

| Table | Purpose |
|-------|---------|
| `AuditLog` | General system audit log (user actions, system events). |
| `LoginAudit` | Tracks all login attempts, successes, failures, lockouts. Standalone — no FK to Users. |
| `StaffAbsenceAudit` | Audit trail for changes to staff absence records. |
| `StaffAssignmentAudit` | Audit trail for changes to staff assignments. |
| `StaffDeviceAudit` | Audit trail for changes to staff device records. |
| `StaffExternalAccountAudit` | Audit trail for changes to staff external account records. |
| `AccountVerificationEvents` | Logs account verification events. |
| `RoleChangeAudit` | Logs role assignment and removal events. |
| `SystemEventsLog` | System-level events: errors, warnings, operational logs. Standalone — no FK to domain tables. |

### 7.7 UI and Configuration Domain

| Table | Purpose |
|-------|---------|
| `ui_MenuItems` | Sidebar menu and submenu items (previously `MenuItems`). |
| `ui_MenuItemsConfig` | Global configuration for menu behaviour (previously `MenuItemsGlobalConfig`). |
| `GlobalConfig` | Master system switches: `EnableGlobalSettings`, `EnableRoleBasedNavigation`. |

---

## 8. Database Schema

### 8.1 Overview

| Item | Value |
|------|-------|
| Database engine | MariaDB 10.4.32 |
| Database name | `absenceapp` |
| Host | `127.0.0.1:3306` |
| User | `root` |
| Character set | `utf8mb4` |
| EF provider | Pomelo.EntityFrameworkCore.MySql 8.0.2 |
| Total tables | ~59 |

### 8.2 Connection String (Development)

```
Server=127.0.0.1;Port=3306;Database=absenceapp;User=root;Password=Calm1309!;CharSet=utf8mb4
```

Used in:
- `AbsenceApp.Api/appsettings.json` — key `AbsenceAppDatabase`
- `AbsenceApp.Client/appsettings.json` — key `AbsenceAppDatabase`
- `AbsenceApp.EfHost/Program.cs` — hardcoded for migration runner

### 8.3 EF Core Migration

| Item | Value |
|------|-------|
| Migration file | `20260416103036_InitialMySQL.cs` |
| Up() | Empty (schema already exists in the database) |
| Down() | Contains full reverse scaffold |
| `__EFMigrationsHistory` | Row `('20260416103036_InitialMySQL', '8.0.2')` inserted manually |

### 8.4 SQL Schema Build Plan

The database was originally designed for SQL Server Express using a 13-phase build plan:

| Phase | Description |
|-------|-------------|
| 1  | Create schemas (core, org, ops, auth, assets, audit) |
| 2  | Create reference/lookup tables |
| 3  | Create domain tables |
| 4  | Create junction tables |
| 5  | Create audit tables |
| 6  | Create UI and config tables |
| 7  | Add foreign key constraints |
| 8  | Seed reference data |
| 9  | Create indexes |
| 10 | Create stored procedures (optional) |
| 11 | Create views (optional) |
| 12 | Create functions (fn_GetVisibleMenuItems, fn_IsFeatureAllowed) |
| 13 | Transform/migration scripts for staging data |

---

## 9. Menu and Permission System

### 9.1 Design Principles

| Principle | Description |
|-----------|-------------|
| Single Source of Truth | The database (`ui_MenuItems` table) is the only authoritative source for menu items. No hardcoded menu definitions in code. |
| Explicit Precedence | Permission rules have a defined evaluation order. |
| Default Deny | Unless a role is explicitly granted access, access is denied. |
| Separation of Concerns | Menu visibility is separate from feature permission checks. |
| No Assumptions | The system never infers permissions; all access must be explicitly defined. |

### 9.2 Enforcement Architecture

```
GlobalConfig flags
    ↓
fn_GetVisibleMenuItems(@RoleType)   — menu boundary
    ↓
Parent category pruning (remove empty groups — presentation only, not permissions)
    ↓
fn_IsFeatureAllowed(@RoleType, @FeatureId)  — feature boundary
    ↓
UI renders only permitted items
```

### 9.3 GlobalConfig Flags

| Flag | Purpose |
|------|---------|
| `EnableGlobalSettings` | Master switch to show/hide the Global Settings module. |
| `EnableRoleBasedNavigation` | Master switch to enforce or bypass role-based menu filtering. When `false`, all users see all menus (development mode only). |

### 9.4 Key Functions (originally SQL Server — MySQL equivalents needed)

| Function | Signature | Purpose |
|----------|-----------|---------|
| `fn_GetVisibleMenuItems` | `fn_GetVisibleMenuItems(@RoleType)` | Returns the set of sidebar menu items visible to a given role type. |
| `fn_IsFeatureAllowed` | `fn_IsFeatureAllowed(@RoleType, @FeatureId)` | Returns 1/0 for whether a feature is accessible to a given role. |

### 9.5 RoleMenuItems — Explicit Menu Visibility Mapping (New in MySQL Migration)

AbsenceApp V2 now uses the `RoleMenuItems` table to explicitly control menu visibility.  
This table defines a direct mapping between Roles and Menu Items:

- A role sees a menu item only if a corresponding entry exists in `RoleMenuItems`.
- Menu visibility is no longer inferred from feature permissions.
- Feature permissions (`perm_RoleFeatureMap`) continue to control action-level access.

This separation provides a clearer RBAC model:

- **RoleMenuItems** → Navigation boundary (what the user can see)
- **perm_RoleFeatureMap** → Action boundary (what the user can do)

The MySQL migration introduced this table to improve maintainability, reduce ambiguity, and simplify permission logic.


## 10. Design System V2 Plan

V2 is an **overlay design system** built alongside V1. V1 components, CSS, and pages are frozen and must not be modified. V2 introduces new component paths, JSON-driven configuration, and a phased component replacement strategy.

### 10.1 V2 Phase Overview

| Phase | Deliverable | Gate |
|-------|-------------|------|
| 1 | Documentation + gap analysis + baseline | ✅ COMPLETE |
| 2 | CSS tokens, JSON config, core components | User confirms "go ahead Phase 2" |
| 3 | Layout V2 (SidebarV2, HeaderV2, BreadcrumbV2, ScrollSpyV2) | User confirms "go ahead Phase 3" |
| 4 | Table System V2 | User confirms "go ahead Phase 4" |
| 5 | Page Templates V2 | User confirms "go ahead Phase 5" |
| 6 | Data & Services V2 | User confirms "go ahead Phase 6" |
| 7 | Feature Modules V2 | User confirms "go ahead Phase 7" |
| 8 | Theming & Branding V2 | User confirms "go ahead Phase 8" |
| 9 | Reusability Framework | User confirms "go ahead Phase 9" |
| 10 | Integration & QA | User confirms "go ahead Phase 10" |

### 10.2 V2 New Paths (by Phase)

| Phase | New Path(s) |
|-------|-------------|
| 2 | `wwwroot/css/tokens/`, `wwwroot/config/designsystem/` (theme.json, menu.json, table-schema.json, components.json, icons.json, branding.json), `Components/DesignSystem/`, `Services/DesignSystemConfigService.cs`, `Services/IconService.cs` |
| 3 | `Components/LayoutV2/` (SidebarV2, HeaderV2, BreadcrumbV2, ScrollSpyV2), `Components/PageTemplateV2.razor` |
| 4 | `Components/TableV2/` (full table system) |
| 5 | `Components/PageTemplatesV2/` |
| 6 | `Services/ApiV2/` |
| 7 | `ModulesV2/` |
| 8 | `Services/Theming/`, `Models/Theming/` |

### 10.3 Frozen V1 Elements (Must Not Be Modified)

- All 43 existing V1 pages in `AbsenceApp.Client`
- All existing V1 layout components
- All existing V1 CSS
- All Core DTOs, Interfaces, and ViewModels
- All Data layer classes
- The entire Api project
- EfHost project
- Analyzers project
- Updater project
- Tests project

### 10.4 Safe-to-Extend V1 Elements

- `AppStateService` (additive properties only)
- `NavigationMetadataService` (additive entries only)
- `IAuthService` (new methods only)
- `Core.DTOs` (new DTOs only)
- `Core.Interfaces` (new interfaces only)

---

## 11. Coding Standards

### 11.1 File Header (Mandatory on Every .cs File)

```csharp
// ============================================================
// File: <FileName>.cs
// Project: AbsenceApp.<ProjectName>
// Author: <Author>
// Created: <YYYY-MM-DD>
// Updated: <YYYY-MM-DD>
// Purpose: <One-sentence description>
// Changes: <version> — <description>
// ============================================================
```

Enforced by: Roslyn analyzer `AA0001` + Git pre-commit hook (`.githooks/`).

### 11.2 Naming Conventions

| Item | Convention | Example |
|------|------------|---------|
| Classes | PascalCase | `UserRepository` |
| Interfaces | `I` + PascalCase | `IUserRepository` |
| Methods | PascalCase | `GetByIdAsync` |
| Private fields | `_camelCase` | `_dbContext` |
| Async methods | Suffix `Async` | `FindByIdAsync` |
| Test classes | `<Target>Tests` | `UserServiceTests` |
| Test methods | `MethodName_Scenario_ExpectedResult` | `GetById_UserNotFound_ThrowsException` |

### 11.3 Architecture Rules

| Rule | Description |
|------|-------------|
| AR-01 | `Core` project has zero upward references. |
| AR-02 | Services return DTOs, never entities. |
| AR-03 | Repositories are injected via interfaces (never concrete types). |
| AR-04 | Mappers are static classes — never registered in DI. |
| AR-05 | Validation (blank/null checks, business rules) lives in services, not repositories. |
| AR-06 | `ArgumentException` is used for missing required input — never silent failures. |
| AR-07 | AuditLog entries are append-only via `AuditLogService` — no UPDATE or DELETE. |
| AR-08 | Section comments required in code: `// Constructor`, `// Public methods`, `// Private helpers`. |
| AR-09 | EF repositories registered as `Scoped`; `AbsenceRepository` and `StudentRepository` registered as `Singleton` in `MauiProgram.cs`. |
| AR-10 | All DI registered via `DataServiceRegistration.AddDataLayer()`. |

### 11.4 V2-Specific Architecture Rules

| Rule | Description |
|------|-------------|
| V2-01 | V2 components live in new paths only — never overwrite V1 files. |
| V2-02 | V2 components must not break V1 components at any point. |
| V2-03 | App shell must remain functional at 100% of the time, even when V2 components are partial or missing. |
| V2-04 | V2 JSON config is loaded via `DesignSystemConfigService` — never accessed directly. |
| V2-05 | V2 CSS tokens must be in separate token files, never inline in app.css. |
| V2-06 | No V2 phase may begin without explicit user gate confirmation. |
| V2-07 | Each V2 phase must pass its own verification before moving on. |
| V2-08 | V2 components must produce identical rendered output to their V1 equivalents before the V1 version is retired. |
| V2-09 | V2 components must undergo the full 40-item test checklist before final integration. |
| V2-10 | The V2 system runs with CSS + Bootstrap 5 only — no XAML styles, no JS frameworks. |

### 11.5 Test Standards

| Rule | Description |
|------|-------------|
| T-01 | All tests must use xUnit `[Fact]` or `[Theory]` attributes. |
| T-02 | All tests must follow Arrange / Act / Assert structure. |
| T-03 | Test class names: `<Target>Tests` (e.g., `UserServiceTests`). |
| T-04 | Test method names: `MethodName_Scenario_ExpectedResult`. |
| T-05 | Client component tests use bUnit. |

---

## 12. Developer Setup

### 12.1 Prerequisites

| Requirement | Version |
|-------------|---------|
| .NET SDK | 8.x |
| Visual Studio | 2022 v17.8+ (with MAUI workload) |
| dotnet-ef global tool | Latest |
| Git | Latest |
| XAMPP | 8.x (MariaDB 10.4.32) |
| phpMyAdmin | Included with XAMPP |

### 12.2 Setup Commands

```bash
# After clone
git config core.hooksPath .githooks

# Run migrations
dotnet ef migrations add <MigrationName> \
  --project src/AbsenceApp.Data \
  --startup-project src/AbsenceApp.EfHost

dotnet ef database update \
  --project src/AbsenceApp.Data \
  --startup-project src/AbsenceApp.EfHost
```

### 12.3 Database Credentials (Development)

| Item | Value |
|------|-------|
| Host | `127.0.0.1` |
| Port | `3306` |
| Database | `absenceapp` |
| User | `root` |
| Password | `Calm1309!` |
| phpMyAdmin | `http://localhost/phpmyadmin` |

---

## 13. Known Defects and Open Issues

| ID | Severity | Description | Source |
|----|----------|-------------|--------|
| D-01 | Critical | Global error boundary not implemented. Navigation to invalid routes causes blank screen or crash requiring tray-close. | Testing Q37 |
| D-02 | Medium | Theme does not persist on cold start. Resets to Light mode every time the app is fully closed and reopened. | Testing Q26 |
| D-03 | Low | No global keyboard shortcuts (Ctrl+K, Ctrl+L, etc.) implemented. Not a defect — missing feature. | Testing Q28 |
| D-04 | Medium | MySQL equivalents for `fn_GetVisibleMenuItems` and `fn_IsFeatureAllowed` were written for SQL Server — need MySQL-compatible replacements for `MenuResolver.cs` and `FeaturePermissionResolver.cs`. | Migration work |
| D-05 | Low | Page title consistency cannot be fully verified — most content pages not yet implemented. | Testing Q35 |
| D-06 | Low | Loading state behaviour cannot be fully verified — most pages have no real data sources yet. | Testing Q36 |

---

## 14. Constraints and Assumptions

| ID | Constraint |
|----|------------|
| C-01 | Windows-only platform. No web, no mobile deployment is in scope. |
| C-02 | Local network deployment only. No cloud hosting. |
| C-03 | The school's MIS is the authoritative source for student identity data. AbsenceApp imports from it; it never pushes back. |
| C-04 | The database is on the school's local server, not the user's workstation. |
| C-05 | Up to 5+ concurrent users over the local network. |
| C-06 | All AI-assisted code generation must produce no build errors or warnings. |
| C-07 | No data may be invented, inferred, or assumed — only explicitly defined data from source docs is used. |
| C-08 | No phase may begin without an explicit user gate confirmation. |
| C-09 | Pomelo version must remain at 8.0.2 to match Pomelo/EF Core 8 compatibility matrix. |
| C-10 | MariaDB 10.4.32 is the target database. Server version `new MariaDbServerVersion(10, 4, 32)` must be used in all EF Core provider registrations. |
