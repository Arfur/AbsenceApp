================================================================================
 File        : AbsenceApp_DECISIONS.md
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-04-16
 Updated     : 2026-04-16
--------------------------------------------------------------------------------
 Purpose     :
   Authoritative log of every architectural, schema, naming, UI, and process 
   decision made for AbsenceApp V2.
--------------------------------------------------------------------------------
 Changes     :
   - 1.0.0 (2026-04-16) Initial creation from all docs in C:\DevAbsence1\docs\
--------------------------------------------------------------------------------
 Notes       :
   - This file is the single source of truth for all decisions.
   - No decision may be contradicted unless explicitly superseded.
================================================================================


## How to Read This File

Each decision entry follows this format:

| Field | Description |
|-------|-------------|
| **ID** | Unique decision identifier (e.g., `DEC-001`) |
| **Date** | Date decision was made or first documented |
| **Category** | Area: Platform / Architecture / Schema / Naming / UI-UX / Permissions / Coding / Testing / Process / Defect |
| **Description** | What was decided |
| **Rationale** | Why this decision was made |
| **Impacted Docs** | Source documents or files this decision traces to |

---

## Decision Log

---

### DEC-001 — Platform: Windows-only desktop app

| Field | Value |
|-------|-------|
| **Date** | Pre-2026-03-13 |
| **Category** | Platform |
| **Description** | AbsenceApp V2 will be a Windows-only local-network desktop application using .NET 8 MAUI Blazor Hybrid. No web, no mobile. |
| **Rationale** | The school uses Windows workstations exclusively. The application requires local server access. Windows desktop delivery avoids the complexity of web hosting, certificates, and browser compatibility. |
| **Impacted Docs** | PRD Questions Initially What you wanted from APP.txt, Architecture.md, baseline-architecture-phase1.txt |

---

### DEC-002 — UI framework: Blazor (Razor components) with CSS + Bootstrap 5

| Field | Value |
|-------|-------|
| **Date** | Pre-2026-03-13 |
| **Category** | Platform |
| **Description** | The UI is built using Blazor Razor components within MAUI. All styling is via CSS and Bootstrap 5. No XAML styles. No JavaScript frameworks. |
| **Rationale** | Blazor enables component reuse and familiar HTML/CSS development. Bootstrap 5 provides a well-understood responsive grid and component library. XAML styles were explicitly ruled out to keep the entire styling surface in CSS. |
| **Impacted Docs** | baseline-architecture-phase1.txt, Architecture.md |

---

### DEC-003 — Icon set: Fluent UI System Icons exclusively

| Field | Value |
|-------|-------|
| **Date** | Pre-2026-04-16 |
| **Category** | UI-UX |
| **Description** | The only permitted icon library is Fluent UI System Icons. No other icon libraries may be introduced. |
| **Rationale** | Fluent UI System Icons provides consistent, modern, Microsoft-aligned iconography suitable for a Windows school application. Using a single icon set ensures visual consistency. |
| **Impacted Docs** | PRD App Questions.txt (Q decision on icon set) |

---

### DEC-004 — Breadcrumb replaces page title entirely

| Field | Value |
|-------|-------|
| **Date** | Pre-2026-04-16 |
| **Category** | UI-UX |
| **Description** | Breadcrumbs are always shown and completely replace the page title. There is no separate page title element; the breadcrumb serves as the page title. |
| **Rationale** | Avoids duplication of navigation context. The breadcrumb provides richer positional information (full path) than a standalone title. |
| **Impacted Docs** | PRD App Questions.txt |

---

### DEC-005 — Page layout is hybrid (fluid for tables/dashboards, fixed-width for forms)

| Field | Value |
|-------|-------|
| **Date** | Pre-2026-04-16 |
| **Category** | UI-UX |
| **Description** | Table and dashboard pages use a fluid/responsive layout. Form pages use a fixed-width layout. |
| **Rationale** | Tables and dashboards benefit from full width to show more data columns. Forms are more readable and less error-prone when constrained to a fixed width. |
| **Impacted Docs** | PRD App Questions.txt |

---

### DEC-006 — Form spacing, columns, and button alignment

| Field | Value |
|-------|-------|
| **Date** | Pre-2026-04-16 |
| **Category** | UI-UX |
| **Description** | Form spacing: 16px. Form columns: fully responsive grid of 1–3 columns. Form sections: headers only (no divider lines). Save/Cancel buttons: right-aligned. |
| **Rationale** | 16px spacing is a standard design token value. Responsive grid ensures forms are usable on smaller windows. Right-aligned action buttons follow standard Windows/web form conventions. |
| **Impacted Docs** | PRD App Questions.txt |

---

### DEC-007 — Table layout: sticky header, 40px rows, zebra striping, hover highlight

| Field | Value |
|-------|-------|
| **Date** | Pre-2026-04-16 |
| **Category** | UI-UX |
| **Description** | All data tables must have: sticky column headers, 40px row height, alternate row colour (zebra striping), and row highlight on hover. |
| **Rationale** | Sticky headers ensure column labels are always visible when scrolling. 40px row height is legible without being too tall. Zebra striping and hover highlights improve readability and findability. |
| **Impacted Docs** | PRD App Questions.txt |

---

### DEC-008 — Table row selection: click-to-select only (no bulk selection)

| Field | Value |
|-------|-------|
| **Date** | Pre-2026-04-16 |
| **Category** | UI-UX |
| **Description** | Clicking a row selects it. No checkbox column or bulk selection mechanism. |
| **Rationale** | The application's workflow is single-record focused. Bulk operations are not a current requirement. Removing bulk selection simplifies the table component and reduces cognitive load. |
| **Impacted Docs** | PRD App Questions.txt |

---

### DEC-009 — Table actions: inline icon-only buttons

| Field | Value |
|-------|-------|
| **Date** | Pre-2026-04-16 |
| **Category** | UI-UX |
| **Description** | Row actions (edit, delete, view, etc.) are inline icon-only buttons within the table row. No text labels on action buttons. |
| **Rationale** | Conserves horizontal space in data-dense tables. Icons from Fluent UI System Icons are sufficiently self-explanatory for standard table actions. |
| **Impacted Docs** | PRD App Questions.txt |

---

### DEC-010 — Table column alignment defaults

| Field | Value |
|-------|-------|
| **Date** | Pre-2026-04-16 |
| **Category** | UI-UX |
| **Description** | Text columns: left-aligned. Number columns: right-aligned. Status columns: centred. All configurable per column. |
| **Rationale** | Standard data presentation conventions. Right-aligned numbers make decimal/integer comparison easier. |
| **Impacted Docs** | PRD App Questions.txt |

---

### DEC-011 — Table empty state: "No records found"

| Field | Value |
|-------|-------|
| **Date** | Pre-2026-04-16 |
| **Category** | UI-UX |
| **Description** | When a table has no data to display, it shows the message "No records found" — not blank space, not a spinner, not an error. |
| **Rationale** | An explicit empty state message prevents users from thinking the page has failed to load. |
| **Impacted Docs** | PRD App Questions.txt |

---

### DEC-012 — Table loading state: skeleton rows

| Field | Value |
|-------|-------|
| **Date** | Pre-2026-04-16 |
| **Category** | UI-UX |
| **Description** | During data loading, tables show skeleton placeholder rows (not a spinner or blank area). |
| **Rationale** | Skeleton rows maintain visual layout during loading, reducing perceived wait time and preventing layout shifts. |
| **Impacted Docs** | PRD App Questions.txt |

---

### DEC-013 — Table filter row under column headers

| Field | Value |
|-------|-------|
| **Date** | Pre-2026-04-16 |
| **Category** | UI-UX |
| **Description** | Tables include a filter input row directly below the column header row. |
| **Rationale** | Inline filters are immediately accessible and contextually positioned next to the column they filter. |
| **Impacted Docs** | PRD App Questions.txt |

---

### DEC-014 — Table search restricted to "Searchable" columns only

| Field | Value |
|-------|-------|
| **Date** | Pre-2026-04-16 |
| **Category** | UI-UX |
| **Description** | Global search in a table only searches columns explicitly marked as `Searchable` in the table schema definition. |
| **Rationale** | Prevents irrelevant partial matches from cluttering search results. Allows the table system to be configured per-use-case. |
| **Impacted Docs** | PRD App Questions.txt |

---

### DEC-015 — Global UI settings: hybrid (JSON defaults + optional DB overrides)

| Field | Value |
|-------|-------|
| **Date** | Pre-2026-04-16 |
| **Category** | UI-UX |
| **Description** | UI configuration defaults come from JSON config files. Optional per-instance overrides can be stored in the database. |
| **Rationale** | JSON defaults are safe and predictable. DB overrides allow customisation per school without requiring code changes. |
| **Impacted Docs** | PRD App Questions.txt |

---

### DEC-016 — Design system V2: overlay/additive, non-breaking, phased

| Field | Value |
|-------|-------|
| **Date** | Pre-2026-04-16 |
| **Category** | Process |
| **Description** | V2 design system runs alongside V1 as an overlay. V1 components, CSS, and pages are frozen. V2 introduces new paths and replaces V1 components incrementally. |
| **Rationale** | A full rewrite would introduce too much risk of regression. The overlay approach allows progressive improvement while keeping the app functional throughout. |
| **Impacted Docs** | baseline-architecture-phase1.txt, gap-analysis-phase1.txt |

---

### DEC-017 — Phase gate model: explicit user confirmation required per phase

| Field | Value |
|-------|-------|
| **Date** | Pre-2026-04-16 |
| **Category** | Process |
| **Description** | Each V2 design system phase requires an explicit user confirmation ("go ahead Phase N") before beginning. No phase may auto-start. |
| **Rationale** | Prevents unintended scope creep or premature implementation. Ensures the human owner controls the pace of change. |
| **Impacted Docs** | baseline-architecture-phase1.txt |

---

### DEC-018 — Clean architecture: Core has no upward references

| Field | Value |
|-------|-------|
| **Date** | Pre-2026-03-13 |
| **Category** | Architecture |
| **Description** | The `AbsenceApp.Core` project must never reference `AbsenceApp.Data`, `AbsenceApp.Client`, or `AbsenceApp.Api`. All dependency flows toward Core. |
| **Rationale** | Classic clean architecture rule. Keeps domain models and contracts pure and independently testable. |
| **Impacted Docs** | Architecture.md, CodingStandards.md |

---

### DEC-019 — Services return DTOs, not entities

| Field | Value |
|-------|-------|
| **Date** | Pre-2026-03-13 |
| **Category** | Architecture |
| **Description** | All service methods return DTOs defined in `AbsenceApp.Core`. EF entity objects must not cross layer boundaries. |
| **Rationale** | Prevents EF tracking state from leaking into the UI or API layer. DTOs form stable, versioned contracts. |
| **Impacted Docs** | Architecture.md, CodingStandards.md, DTOsAndMappers.md |

---

### DEC-020 — Mappers are static classes, never injected

| Field | Value |
|-------|-------|
| **Date** | Pre-2026-03-13 |
| **Category** | Architecture |
| **Description** | Mapper classes (e.g., `UserMapper`, `ClassMapper`) are static. They are never registered in DI and never accept dependencies. |
| **Rationale** | Mappers are pure functions — input in, output out. Static classes make this explicit and avoid lifecycle management complexity. |
| **Impacted Docs** | Architecture.md, CodingStandards.md, DTOsAndMappers.md |

---

### DEC-021 — Repository pattern with generic IRepository<T>

| Field | Value |
|-------|-------|
| **Date** | Pre-2026-03-13 |
| **Category** | Architecture |
| **Description** | All data access uses the repository pattern. A generic `IRepository<T>` defines the base contract: AddAsync, FindByIdAsync, ListAllAsync, Query, UpdateAsync, DeleteAsync. Per-entity repositories extend this where needed. |
| **Rationale** | Decouples data access from service logic. Enables mocking in tests. |
| **Impacted Docs** | Architecture.md, Repositories.md |

---

### DEC-022 — Validation in services, not repositories

| Field | Value |
|-------|-------|
| **Date** | Pre-2026-03-13 |
| **Category** | Architecture |
| **Description** | All business rule validation (blank/null checks, domain constraints) lives in service methods, not in repositories. `ArgumentException` is the exception type for missing required input. |
| **Rationale** | Repositories are data access wrappers — not business logic owners. Centralising validation in services keeps repositories simple and reusable. |
| **Impacted Docs** | CodingStandards.md, Services.md |

---

### DEC-023 — AuditLog is append-only

| Field | Value |
|-------|-------|
| **Date** | Pre-2026-03-13 |
| **Category** | Architecture |
| **Description** | Audit log records are never updated or deleted via the service layer. `AuditLogService.LogAsync` only inserts new rows. |
| **Rationale** | Audit logs are compliance records. Allowing modification would undermine their integrity and legal/traceability purpose. |
| **Impacted Docs** | DataModel.md, Services.md |

---

### DEC-024 — All FK constraints use DeleteBehavior.Restrict (no cascade deletes)

| Field | Value |
|-------|-------|
| **Date** | Pre-2026-03-13 |
| **Category** | Schema |
| **Description** | All EF Core foreign key configurations specify `DeleteBehavior.Restrict`. No cascade deletes are permitted. |
| **Rationale** | Prevents accidental data loss. In a school records system, orphaned relationships are better surfaced as errors than silently deleted. |
| **Impacted Docs** | DataModel.md |

---

### DEC-025 — DI registration centralised in DataServiceRegistration.AddDataLayer()

| Field | Value |
|-------|-------|
| **Date** | Pre-2026-03-13 |
| **Category** | Architecture |
| **Description** | All DI service registrations for the Data layer are in the single `DataServiceRegistration.AddDataLayer()` extension method. No DI registrations are scattered across individual files. |
| **Rationale** | Single point of truth for the service registration graph. Easier to review, debug, and test startup behaviour. |
| **Impacted Docs** | Architecture.md |

---

### DEC-026 — Mandatory file header comment on every .cs file

| Field | Value |
|-------|-------|
| **Date** | Pre-2026-03-13 |
| **Category** | Coding |
| **Description** | Every `.cs` file must start with a standardised block-header comment containing: File, Project, Author, Created, Updated, Purpose, Changes. Enforced by Roslyn analyzer AA0001 and a Git pre-commit hook in `.githooks/`. |
| **Rationale** | Ensures traceability and context for every file. Particularly important when using AI-assisted development where file context can be lost between sessions. |
| **Impacted Docs** | CodingStandards.md |

---

### DEC-027 — Naming: PascalCase classes, _camelCase private fields, Async suffix

| Field | Value |
|-------|-------|
| **Date** | Pre-2026-03-13 |
| **Category** | Coding |
| **Description** | Classes and methods: PascalCase. Interfaces: `I` + PascalCase. Private fields: `_camelCase`. Async methods: must have `Async` suffix. |
| **Rationale** | Standard C# conventions. Consistency across a codebase assists readability and AI-assisted code generation quality. |
| **Impacted Docs** | CodingStandards.md |

---

### DEC-028 — Section comments required in every class

| Field | Value |
|-------|-------|
| **Date** | Pre-2026-03-13 |
| **Category** | Coding |
| **Description** | Classes must contain delimiter section comments: `// Constructor`, `// Public methods`, `// Private helpers`. |
| **Rationale** | Provides consistent internal navigation in files. Particularly useful in large service and repository classes. |
| **Impacted Docs** | CodingStandards.md |

---

### DEC-029 — Git pre-commit hook enforces header comment

| Field | Value |
|-------|-------|
| **Date** | Pre-2026-03-13 |
| **Category** | Process |
| **Description** | A Git pre-commit hook in `.githooks/` runs the Roslyn analyzer check before allowing a commit. `git config core.hooksPath .githooks` must be run after every clone. |
| **Rationale** | Prevents header-less files from being committed. The hook provides a fail-fast enforcement mechanism without needing to run a full build. |
| **Impacted Docs** | CodingStandards.md, DeveloperSetup.md |

---

### DEC-030 — EfHost is the startup project for all migrations

| Field | Value |
|-------|-------|
| **Date** | Pre-2026-03-13 |
| **Category** | Architecture |
| **Description** | EF Core migrations use `--project src/AbsenceApp.Data --startup-project src/AbsenceApp.EfHost`. EfHost is the designated startup host for migration and seeder operations. |
| **Rationale** | Separates the migration/seeder host from the production API host. Keeps migration tooling clean and isolated from runtime API configuration. |
| **Impacted Docs** | DeveloperSetup.md, MigrationHistory.md |

---

### DEC-031 — Menu system: DB is single source of truth (no hardcoded menus in code)

| Field | Value |
|-------|-------|
| **Date** | Pre-2026-04-16 |
| **Category** | Permissions |
| **Description** | All sidebar menu items, groups, and categories are defined in the `ui_MenuItems` database table. No menu items may be hardcoded in Blazor components or C# code. |
| **Rationale** | Allows menu items to be updated without a code deployment. Enables runtime control of navigation by role without recompiling. |
| **Impacted Docs** | PRD Menu Control System.txt |

---

### DEC-032 — Menu access: default deny

| Field | Value |
|-------|-------|
| **Date** | Pre-2026-04-16 |
| **Category** | Permissions |
| **Description** | Unless a role is explicitly granted access to a menu item via `perm_RoleFeatureMap`, access is denied. There is no default-allow behaviour. |
| **Rationale** | Security-first approach. Prevents accidental exposure of menu items when new items are added without explicit role assignment. |
| **Impacted Docs** | PRD Menu Control System.txt |

---

### DEC-033 — Menu visibility gated by fn_GetVisibleMenuItems TVF

| Field | Value |
|-------|-------|
| **Date** | Pre-2026-04-16 |
| **Category** | Permissions |
| **Description** | Menu visibility is determined by calling `fn_GetVisibleMenuItems(@RoleType)` — a table-valued function. The resolved C# caller is `MenuResolver.cs`. |
| **Rationale** | Encapsulates the permission logic in the database layer. Makes the permission rules auditable and testable independently of the application. |
| **Impacted Docs** | PRD Menu Control System.txt, MenuResolver.cs |

---

### DEC-034 — Feature access gated by fn_IsFeatureAllowed scalar function

| Field | Value |
|-------|-------|
| **Date** | Pre-2026-04-16 |
| **Category** | Permissions |
| **Description** | Feature-level permission checks call `fn_IsFeatureAllowed(@RoleType, @FeatureId)`. The resolved C# caller is `FeaturePermissionResolver.cs`. |
| **Rationale** | Separates menu visibility (navigation boundary) from feature execution (action boundary). Prevents users who can see a page from performing actions they are not authorised for. |
| **Impacted Docs** | PRD Menu Control System.txt, FeaturePermissionResolver.cs |

---

### DEC-035 — GlobalConfig table: EnableGlobalSettings and EnableRoleBasedNavigation

| Field | Value |
|-------|-------|
| **Date** | Pre-2026-04-16 |
| **Category** | Permissions |
| **Description** | The `GlobalConfig` table stores two master switches: `EnableGlobalSettings` (controls visibility of the Global Settings module) and `EnableRoleBasedNavigation` (enforces or bypasses all role-based menu/feature filtering). |
| **Rationale** | Development and debugging require the ability to bypass permission enforcement. The `EnableRoleBasedNavigation = false` flag provides a safe mechanism for this. Must always be `true` in production. |
| **Impacted Docs** | PRD Menu Control System.txt, Renamed Tables.txt |

---

### DEC-036 — Parent category pruning (remove empty categories from sidebar)

| Field | Value |
|-------|-------|
| **Date** | Pre-2026-04-16 |
| **Category** | Permissions |
| **Description** | If all child items within a category or group are hidden (denied) for a role, the parent category/group header is also removed from the sidebar. |
| **Rationale** | Prevents confusing empty category headers from appearing in the navigation. This is a presentation concern only — it does not affect the underlying permission data. |
| **Impacted Docs** | PRD Menu Control System.txt |

---

### DEC-037 — Staff domain is separate from Users domain

| Field | Value |
|-------|-------|
| **Date** | Pre-2026-04-16 |
| **Category** | Schema |
| **Description** | `Staff` (HR identity) and `Users` (login accounts) are in separate tables with an optional foreign key relationship. A staff member may or may not have a user account. A user account may or may not be linked to a staff record. |
| **Rationale** | Staff records hold HR data regardless of whether the person has system access. User accounts control login access independently. Separating them avoids a tight dependency between HR data and authentication. |
| **Impacted Docs** | PRD Data Structure of Table in Text Format.txt, Renamed Tables.txt |

---

### DEC-038 — Table naming: table renames documented in Renamed Tables.txt

| Field | Value |
|-------|-------|
| **Date** | Pre-2026-04-16 |
| **Category** | Naming |
| **Description** | Multiple tables were renamed from their original V1 names for clarity and domain grouping. The canonical rename mapping is defined in `Renamed Tables.txt`. UI tables are prefixed `ui_`; permission tables are prefixed `perm_`; audit and domain tables retain their descriptive names. |
| **Rationale** | Original names were ambiguous or inconsistent. New names communicate domain ownership and purpose at a glance. |
| **Impacted Docs** | Renamed Tables.txt |

---

### DEC-039 — Classes have year group assignments (many-to-many via ClassYearGroupAssignments)

| Field | Value |
|-------|-------|
| **Date** | Pre-2026-04-16 |
| **Category** | Schema |
| **Description** | Classes and year groups are linked via a junction table `ClassYearGroupAssignments`. A class may belong to more than one year group. |
| **Rationale** | Mixed-year classes exist in some school settings. A junction table handles this without data duplication. |
| **Impacted Docs** | PRD Data Structure of Table in Text Format.txt, Renamed Tables.txt |

---

### DEC-040 — Multi-school support via Schools lookup table

| Field | Value |
|-------|-------|
| **Date** | Pre-2026-04-16 |
| **Category** | Schema |
| **Description** | The `StaffSchools` table provides a school-site lookup used as a FK in the `Staff` table. Supports multi-campus or federation deployments. |
| **Rationale** | Some schools operate across multiple sites or buildings. Capturing the school FK on staff records enables reporting by site. |
| **Impacted Docs** | PRD Data Structure of Table in Text Format.txt, Renamed Tables.txt |

---

### DEC-041 — SQL Server → MariaDB migration

| Field | Value |
|-------|-------|
| **Date** | 2026-04-16 |
| **Category** | Platform |
| **Description** | The database backend was migrated from SQL Server Express (LocalDB) to MariaDB 10.4.32 running via XAMPP. Connection strings, EF provider registration, and migration files were updated. |
| **Rationale** | SQL Server Express has storage and connection limits that may hinder school use. MariaDB via XAMPP is freely available, has no artificial limits for this scale, and XAMPP is already commonly available in school IT environments. phpMyAdmin provides a UI for database administration. |
| **Impacted Docs** | DataServiceRegistration.cs, AppDbContextFactory.cs, EfHost/Program.cs, appsettings.json (Api and Client), MigrationHistory.md |

---

### DEC-042 — EF Core and Pomelo versions pinned to 8.0.2

| Field | Value |
|-------|-------|
| **Date** | 2026-04-16 |
| **Category** | Platform |
| **Description** | EF Core was bumped from 8.0.0 to 8.0.2. Pomelo.EntityFrameworkCore.MySql is pinned to 8.0.2. Both must match the same minor version. |
| **Rationale** | Pomelo 8.0.2 requires EF Core 8.0.2. Version mismatch causes build-time or runtime failures. Pinning prevents accidental version drift. |
| **Impacted Docs** | AbsenceApp.Data.csproj, AbsenceApp.EfHost.csproj |

---

### DEC-043 — MariaDbServerVersion(10, 4, 32) used in all EF provider registrations

| Field | Value |
|-------|-------|
| **Date** | 2026-04-16 |
| **Category** | Platform |
| **Description** | EF Core MySQL provider is registered with `new MariaDbServerVersion(10, 4, 32)` — not auto-detect, not a MySQL version. |
| **Rationale** | Auto-detection requires a live database connection at startup, which can fail in some deployment contexts. Explicit version specification is predictable. MariaDB 10.4.32 is the confirmed target version (XAMPP). |
| **Impacted Docs** | DataServiceRegistration.cs, AppDbContextFactory.cs |

---

### DEC-044 — InitialMySQL migration Up() is deliberately empty

| Field | Value |
|-------|-------|
| **Date** | 2026-04-16 |
| **Category** | Schema |
| **Description** | The `20260416103036_InitialMySQL.cs` migration has an empty `Up()` method. The schema was already established in the database. The migration was registered manually in `__EFMigrationsHistory`. |
| **Rationale** | EF Core requires an applied migration baseline to track future migrations. Emptying `Up()` records the baseline without re-running schema creation (which would fail or cause conflicts against an existing schema). |
| **Impacted Docs** | Migrations/20260416103036_InitialMySQL.cs, MigrationHistory.md |

---

### DEC-045 — MenuResolver and FeaturePermissionResolver updated to MySqlConnector

| Field | Value |
|-------|-------|
| **Date** | 2026-04-16 |
| **Category** | Platform |
| **Description** | `MenuResolver.cs` and `FeaturePermissionResolver.cs` were updated to use `MySqlConnector.MySqlConnection` and `MySqlConnector.MySqlParameter`. The `dbo.` schema prefix was removed from all function calls. |
| **Rationale** | SQL Server uses `System.Data.SqlClient` and schema-qualified function names (e.g., `dbo.fn_GetVisibleMenuItems`). MySQL/MariaDB uses MySqlConnector and no schema prefix. |
| **Impacted Docs** | MenuResolver.cs, FeaturePermissionResolver.cs |

---

### DEC-046 — 13-phase SQL build plan for the database schema

| Field | Value |
|-------|-------|
| **Date** | Pre-2026-04-16 |
| **Category** | Process |
| **Description** | The database schema was built using a 13-phase plan: (1) schemas, (2) reference tables, (3) domain tables, (4) junction tables, (5) audit tables, (6) UI/config tables, (7) FK constraints, (8) seed data, (9) indexes, (10) stored procedures, (11) views, (12) functions, (13) transform/staging scripts. |
| **Rationale** | A phased approach with per-phase verification prevents errors from compounding. Each phase is verified before proceeding. |
| **Impacted Docs** | PRD Build SQL Database Structure Instructions to AI.txt, docs/migration/phases/ (01Phase1 through 13Phase13) |

---

### DEC-047 — No data invention or inference permitted

| Field | Value |
|-------|-------|
| **Date** | Pre-2026-04-16 |
| **Category** | Process |
| **Description** | All data (seed values, table definitions, mappings) must be explicitly defined in canonical source files. No values may be invented, inferred, or assumed. AI tools operating on this codebase must follow this rule. |
| **Rationale** | School records data has legal and compliance implications. Invented or inferred values could cause incorrect records, reporting errors, or audit failures. |
| **Impacted Docs** | A01_TableDefinitions.txt, A02_IndexDefinitions.txt, A03_SeedData.txt, A04_StagingTransformMappings.txt |

---

### DEC-048 — Audit tables are standalone or loosely linked

| Field | Value |
|-------|-------|
| **Date** | Pre-2026-04-16 |
| **Category** | Schema |
| **Description** | `LoginAudit` and `SystemEventsLog` have no foreign key relationships to domain tables. Domain audit tables (e.g., `StaffAbsenceAudit`) link to their parent domain table by ID. |
| **Rationale** | `LoginAudit` must record failed logins where no valid user exists — a FK to `Users` would prevent this. `SystemEventsLog` records system-level events that may occur independently of any user session. |
| **Impacted Docs** | Renamed Tables.txt |

---

### DEC-049 — GlobalConfig table to be reassessed in refactor phase

| Field | Value |
|-------|-------|
| **Date** | Pre-2026-04-16 |
| **Category** | Schema |
| **Description** | The `GlobalConfig` table is retained for now but is flagged for reassessment during the refactor/cleanup phase. It may be removed or repurposed once the architecture stabilises. |
| **Rationale** | Both settings in the table (`EnableGlobalSettings`, `EnableRoleBasedNavigation`) duplicate logic already enforced by roles and permissions. The table may be a development/debugging legacy rather than a required production component. |
| **Impacted Docs** | Renamed Tables.txt |

---

### DEC-050 — Test naming conventions: xUnit, AAA, MethodName_Scenario_ExpectedResult

| Field | Value |
|-------|-------|
| **Date** | Pre-2026-03-13 |
| **Category** | Testing |
| **Description** | All tests use xUnit `[Fact]` or `[Theory]`. Tests follow Arrange/Act/Assert. Test class names: `<Target>Tests`. Test method names: `MethodName_Scenario_ExpectedResult`. |
| **Rationale** | Consistent test naming makes test reports self-documenting. The naming format encodes intent (what is being tested, under what conditions, and what the expected outcome is). |
| **Impacted Docs** | CodingStandards.md |

---

### DEC-051 — V1 pages are frozen at 43 (no modifications permitted)

| Field | Value |
|-------|-------|
| **Date** | Pre-2026-04-16 |
| **Category** | Process |
| **Description** | The 43 existing V1 pages in `AbsenceApp.Client` are frozen. No modifications to V1 pages are permitted during the V2 design system overlay phases. |
| **Rationale** | V2 is additive. Modifying V1 pages while building V2 overlays introduces regression risk and contaminates the V1 baseline. |
| **Impacted Docs** | baseline-architecture-phase1.txt, structure-phase1.txt |

---

### DEC-052 — Theme persistence on cold start is a known deferred defect

| Field | Value |
|-------|-------|
| **Date** | 2026-04-16 |
| **Category** | Defect |
| **Description** | The application resets to Light mode on every cold start (full exit and relaunch). Theme persists correctly during warm sessions (minimise/restore). Fix deferred: persist theme to local storage or configuration file. |
| **Rationale** | Theme state is held in memory and not yet persisted to a store. Resolution requires either `localStorage` (Blazor JSInterop) or a config file. |
| **Impacted Docs** | Testing 40 questions.txt (Q26B) |

---

### DEC-053 — Global error boundary missing — critical defect

| Field | Value |
|-------|-------|
| **Date** | 2026-04-16 |
| **Category** | Defect |
| **Description** | No global error boundary is implemented. Navigation to invalid routes or exception states causes a blank screen or app instability, requiring tray-close to recover. This is the highest priority defect. |
| **Rationale** | Blazor requires an `<ErrorBoundary>` component (or global `OnError` handling) to intercept unhandled exceptions. Without it, the shell becomes unresponsive after any unhandled error. |
| **Impacted Docs** | Testing 40 questions.txt (Q37C) |

---

## Open Decisions

| ID | Topic | Status |
|----|-------|--------|
| OD-01 | MySQL equivalents for `fn_GetVisibleMenuItems` and `fn_IsFeatureAllowed` | Deferred — SQL Server stored procedures exist; MySQL-compatible versions not yet created |
| OD-02 | GlobalConfig table — retain or remove | Deferred to refactor/cleanup phase |
| OD-03 | Theme persistence implementation (localStorage vs config file) | Deferred to V2 Phase 2 or later |
| OD-04 | Global error boundary implementation | Deferred to V2 Phase 1 or early Phase 3 (layout) |
| OD-05 | Global keyboard shortcuts (Ctrl+K, Ctrl+L, etc.) | Future feature — not currently planned in any phase |
| OD-06 | Future features: medical logs, dietary needs, safeguarding, behaviour tracking | Deferred to a V3 or future track |
| OD-07 | Auto-detect Windows login implementation | Not yet implemented — mechanism TBD |
| OD-08 | Persistent absence alert threshold configuration | Not yet defined — requires product decision |
| OD-09 | Email reminder mechanism (SMTP, Exchange, etc.) | Not yet specified — requires infrastructure decision |

### DEC-054 — Introduction of RoleMenuItems table for explicit menu visibility mapping

| Field | Value |
|-------|-------|
| **Date** | 2026-04-16 |
| **Category** | Permissions |
| **Description** | A new table `RoleMenuItems` was introduced during the MySQL migration to explicitly map roles to menu items. Menu visibility is now determined by direct Role → MenuItem mappings instead of being inferred from feature permissions. |
| **Rationale** | Separating menu visibility from feature permissions provides clearer RBAC boundaries, reduces ambiguity, and aligns with modern permission models. It also simplifies MySQL function design and improves maintainability. |
| **Impacted Docs** | AbsenceApp_PRD.md (Menu & Permission System), AbsenceApp_PROGRESS.md (DB migration), MenuResolver.cs |

## DECISION-2026-04-16 — Establish Baseline Permission Data
### Context
The MySQL environment did not contain the baseline permission data required for
feature toggles and role-based access control.

### Decision
Seed the `feature` and `roletypes` tables with the baseline values used by the
application. These values must exist in all environments.

### Rationale
- Required for consistent permission evaluation
- Aligns MySQL with SQL Server behaviour
- Prevents runtime errors caused by missing feature codes or role types

### Consequences
- MySQL now has a stable permission baseline
- Future changes to features or role types must be documented in CHANGELOG

## DECISION-2026-04-16 — Integrate Migration Work into V2 Phase 1
### Context
The SQL Server → MySQL migration was originally tracked as a separate 8‑phase plan and represented as Track 2 in the TRACKER. This created confusion when combined with the 10‑phase V2 design system.

### Decision
Migration work will be integrated into V2 Phase 1 as sub‑phases:
- Phase 1.1 — Migration Work (Historical)
- Phase 1.2 — V2 Foundations & Documentation

### Rationale
This keeps the V2 phase model clean, avoids mixing two unrelated phase systems, and accurately reflects that migration occurred during Phase 1.

### Consequences
Track 2 remains for ongoing database tasks, but historical migration phases are now part of Phase 1.
