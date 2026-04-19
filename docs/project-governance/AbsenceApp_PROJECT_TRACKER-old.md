================================================================================
 File        : AbsenceApp_PROJECT_TRACKER.md
 Author      : Michael
================================================================================
 Created     : 2026-04-16
 Updated     : 2026-04-17
--------------------------------------------------------------------------------
 Purpose     :
   Simple task tracker for AbsenceApp V2. One row per task.
   To start a task: set Status to In Progress, fill in the Started date.
--------------------------------------------------------------------------------
 Status values : Not Started | In Progress | Done
--------------------------------------------------------------------------------
 Changes     :
   - 1.0.0 (2026-04-16) Initial creation.
   - 1.1.0 (2026-04-17) Rewritten as pure task tracker. Removed analysis
                         sections. Added MySQL table population tasks.
                         Corrected column structure (Status / Started / Completed).
----------+---------------------------------------------------------------+--------------+------------------+------------------+
================================================================================
----------+---------------------------------------------------------------+--------------+------------------+------------------+

## DB Track — SQL Server to MySQL: Code Migration

| Task ID | Description | Status | Started | Completed |
|---------|-------------|--------|---------|-----------|
| MIG-01 | SQL Server Express schema designed (13-phase plan) | Done | — | Pre-2026-04-16 |
| MIG-02 | SQL Server schema built (all 13 phases) | Done | — | Pre-2026-04-16 |
| MIG-03 | EF Core initial migration on SQL Server (InitialCreate) | Done | — | 2026-03-13 |
| MIG-04 | Decision to migrate from SQL Server to MariaDB 10.4.32 | Done | — | 2026-04-16 |
| MIG-05 | Pomelo.EntityFrameworkCore.MySql 8.0.2 installed | Done | — | 2026-04-16 |
| MIG-06 | EF Core bumped from 8.0.0 to 8.0.2 | Done | — | 2026-04-16 |
| MIG-07 | DataServiceRegistration.cs — UseMySql with MariaDbServerVersion(10,4,32) | Done | — | 2026-04-16 |
| MIG-08 | AppDbContextFactory.cs — updated to UseMySql | Done | — | 2026-04-16 |
| MIG-09 | EfHost/Program.cs — updated to MySQL connection string | Done | — | 2026-04-16 |
| MIG-10 | Api/appsettings.json — updated to MariaDB connection string | Done | — | 2026-04-16 |
| MIG-11 | Client/appsettings.json — updated to MariaDB connection string | Done | — | 2026-04-16 |
| MIG-12 | SQL Server migrations deleted | Done | — | 2026-04-16 |
| MIG-13 | InitialMySQL migration generated (20260416103036_InitialMySQL.cs) | Done | — | 2026-04-16 |
| MIG-14 | InitialMySQL migration Up() emptied (schema already exists in DB) | Done | — | 2026-04-16 |
| MIG-15 | __EFMigrationsHistory table created in MySQL | Done | — | 2026-04-16 |
----------+---------------------------------------------------------------+--------------+------------------+------------------+
| MIG-16 | InitialMySQL row inserted into __EFMigrationsHistory | Done | — | 2026-04-16 |
| MIG-17 | MenuResolver.cs updated to MySqlConnector | Done | — | 2026-04-16 |
| MIG-18 | FeaturePermissionResolver.cs updated to MySqlConnector | Done | — | 2026-04-16 |
----------+---------------------------------------------------------------+--------------+------------------+------------------+
| MIG-19 | Full solution build — 0 errors, 0 warnings verified | Done | — | 2026-04-16 |
----------+---------------------------------------------------------------+--------------+------------------+------------------+
| MIG-20 | rolemenuitem table created during MySQL migration | Done | — | 2026-04-16 |
| MIG-21 | Create MySQL equivalents of fn_GetVisibleMenuItems and fn_IsFeatureAllowed | Not Started | — | — |

---

## DB Track — MySQL Table Population: Group 1 — Absence Lookup Tables

----------+---------------------------------------------------------------+--------------+------------------+------------------+
Tables: `absencecategories` `absencemethods` `absencereasons` `absencesources` `absencestatuses` `absencetypes`

| Task ID | Description | Table | Status | Started | Completed |
|---------|-------------|-------|--------|---------|-----------|
| POP-01 | Seed absence categories (e.g. Authorised, Unauthorised) | absencecategories | Not Started | — | — |
| POP-02 | Seed absence methods (e.g. Phone, In Person, Email) | absencemethods | Not Started | — | — |
| POP-03 | Seed absence reasons (e.g. Illness, Holiday, Medical) | absencereasons | Not Started | — | — |
| POP-04 | Seed absence sources (e.g. Parent, School, Self) | absencesources | Not Started | — | — |
| POP-05 | Seed absence statuses (e.g. Open, Closed, Pending) | absencestatuses | Not Started | — | — |
| POP-06 | Seed absence types (e.g. Sick, Annual Leave, TOIL) | absencetypes | Not Started | — | — |

---

## DB Track — MySQL Table Population: Group 2 — Organisation Lookups

Tables: `devicetypes` `externalsystems` `houses` `phases` `responsibilities` `staffdepartments` `staffjobgroups` `staffjobtitles` `staffschools` `yeargroups`

| Task ID | Description | Table | Status | Started | Completed |
|---------|-------------|-------|--------|---------|-----------|
| POP-07 | Seed device types (e.g. Laptop, Tablet, Desktop) | devicetypes | Not Started | — | — |
| POP-08 | Seed external systems (e.g. Google, Microsoft) | externalsystems | Not Started | — | — |
| POP-09 | Seed school houses | houses | Not Started | — | — |
| POP-10 | Seed school phases (EYFS, KS1, KS2, KS3, KS4) | phases | Not Started | — | — |
| POP-11 | Seed staff responsibilities (e.g. Head of Year, SENCO) | responsibilities | Not Started | — | — |
| POP-12 | Seed staff departments | staffdepartments | Not Started | — | — |
| POP-13 | Seed staff job groups | staffjobgroups | Not Started | — | — |
| POP-14 | Seed staff job titles | staffjobtitles | Not Started | — | — |
| POP-15 | Seed school sites / campuses | staffschools | Not Started | — | — |
| POP-16 | Seed year groups (EYFS, Y1–Y13) | yeargroups | Not Started | — | — |

---

## DB Track — MySQL Table Population: Group 3 — Permissions & Menu

Tables: `feature` `roletypes` `roles` `rolemenuitem` `rolefeature` `menuitems` `menuitemsglobalconfig` `globalconfig` `userfeatureoverride`

| Task ID | Description | Table | Status | Started | Completed |
|---------|-------------|-------|--------|---------|-----------|
| POP-17 | Seed feature flags (6 records) | feature | Done | 2026-04-16 | 2026-04-16 |
| POP-18 | Seed role types (Admin, StaffAdmin, Teacher, etc.) | roletypes | Done | 2026-04-16 | 2026-04-16 |
| POP-19 | Seed roles (SuperAdmin, Admin, Office, Teacher, etc.) | roles | Not Started | — | — |
| POP-20 | Seed role–menu-item mappings (what each role can see) | rolemenuitem | Done | 2026-04-16 | 2026-04-16 |
| POP-21 | Seed role–feature mappings (what each role can do) | rolefeature | Not Started | — | — |
| POP-22 | Seed sidebar menu items and structure | menuitems | Not Started | — | — |
| POP-23 | Seed menu global config (EnableRoleBasedNavigation, etc.) | menuitemsglobalconfig | Not Started | — | — |
| POP-24 | Seed global config switches (EnableGlobalSettings, etc.) | globalconfig | Not Started | — | — |
| POP-25 | Seed user feature overrides (leave empty for baseline) | userfeatureoverride | Not Started | — | — |

---

## DB Track — MySQL Table Population: Group 4 — Users & Auth

Tables: `users` `userprofiles` `userrole`

| Task ID | Description | Table | Status | Started | Completed |
|---------|-------------|-------|--------|---------|-----------|
| POP-26 | Create admin user account | users | Not Started | — | — |
| POP-27 | Create test office staff user account | users | Not Started | — | — |
| POP-28 | Create test teacher user account | users | Not Started | — | — |
| POP-29 | Create user profiles for all test users | userprofiles | Not Started | — | — |
| POP-30 | Assign roles to test users via userrole | userrole | Not Started | — | — |

---

## DB Track — MySQL Table Population: Group 5 — Staff Domain

Tables: `staff` `staffabsences` `staffassignments` `staffdevices` `staffexternalaccounts` `stafflocations` `staffresponsibilities`

| Task ID | Description | Table | Status | Started | Completed |
|---------|-------------|-------|--------|---------|-----------|
| POP-31 | Create at least 3 test staff records | staff | Not Started | — | — |
| POP-32 | Create test staff absence records | staffabsences | Not Started | — | — |
| POP-33 | Create test staff assignment records | staffassignments | Not Started | — | — |
| POP-34 | Create test staff device records | staffdevices | Not Started | — | — |
| POP-35 | Create test staff external account records | staffexternalaccounts | Not Started | — | — |
| POP-36 | Create test staff location records | stafflocations | Not Started | — | — |
| POP-37 | Assign responsibilities to test staff | staffresponsibilities | Not Started | — | — |

---

## DB Track — MySQL Table Population: Group 6 — Academic & Students

Tables: `classes` `classyeargroupassignments` `classmember` `students` `studentcontacts` `studentflags` `studentmedical` `studentabsences`

| Task ID | Description | Table | Status | Started | Completed |
|---------|-------------|-------|--------|---------|-----------|
| POP-38 | Create at least 3 test classes | classes | Not Started | — | — |
| POP-39 | Assign year groups to test classes | classyeargroupassignments | Not Started | — | — |
[================================================================================]
| POP-41 | Assign students to classes | classmember | Not Started | — | — |
| POP-42 | Create student contact records (parent/guardian) | studentcontacts | Not Started | — | — |
| POP-43 | Create student flag records (SEN, medical, pastoral) | studentflags | Not Started | — | — |
| POP-45 | Create test student absence records | studentabsences | Not Started | — | — |

---

## DB Track — MySQL Table Population: Group 7 — Attendance

Tables: `attendanceregisters` `attendancemarks` `attendance`
--------+-----------------------------------------------+----------+---------+-----------+

| Task ID | Description | Table | Status | Started | Completed |
|---------|-------------|-------|--------|---------|-----------|
| POP-46 | Create test attendance registers | attendanceregisters | Not Started | — | — |
| POP-47 | Create test attendance marks | attendancemarks | Not Started | — | — |
| POP-48 | Create test attendance records | attendance | Not Started | — | — |

---

## DB Track — MySQL Table Population: Group 8 — Notifications & Messages

Tables: `appnotifications` `messages`

| Task ID | Description | Table | Status | Started | Completed |
|---------|-------------|-------|--------|---------|-----------|
| POP-49 | Create test app notification records | appnotifications | Not Started | — | — |
| POP-50 | Create test message records | messages | Not Started | — | — |

> **Note — Audit tables:** `auditlog` `loginaudit` `staffabsenceaudit` `staffdeviceaudit`
> `staffexternalaccountaudit` `staffschoolaudit` `studentabsenceaudit` `rolechangeaudit`
> `accountverificationevents` `systemevents` — These are populated automatically by the
> application as events occur. No manual seeding required.

---
--------+-----------------------------------------------+----------+---------+-----------+

## DB Track — MySQL Population: End-to-End Validation

| Task ID | Description | Status | Started | Completed |
--------+-----------------------------------------------+----------+---------+-----------+
|---------|-------------|--------|---------|-----------|
| VAL-01 | App launches and connects to MySQL without error | Not Started | — | — |
| VAL-02 | Login screen authenticates a test user | Not Started | — | — |
| VAL-03 | Sidebar loads correct menu items for Admin role | Not Started | — | — |
| VAL-04 | Sidebar loads correct menu items for Teacher role | Not Started | — | — |
| VAL-05 | Role-based menu filtering works (MIG-21 prerequisite) | Not Started | — | — |
| VAL-06 | Feature permission checks work (MIG-21 prerequisite) | Not Started | — | — |
| VAL-07 | Students list page shows test student records | Not Started | — | — |
| VAL-08 | Staff list page shows test staff records | Not Started | — | — |
| VAL-09 | Absence recording creates a row in studentabsences | Not Started | — | — |
| VAL-10 | Audit log row created for a tracked user action | Not Started | — | — |
| VAL-11 | End-to-end test with real data — signed off | Not Started | — | — |

---

[================================================================================]

| Task ID | Description | Status | Started | Completed |
|---------|-------------|--------|---------|-----------|
| PH1-01 | Generate Architecture.md | Done | 2026-04-16 | 2026-04-16 |
| PH1-02 | Generate CodingStandards.md | Done | 2026-04-16 | 2026-04-16 |
| PH1-03 | Generate DataModel.md | Done | 2026-04-16 | 2026-04-16 |
| PH1-04 | Generate DTOsAndMappers.md | Done | 2026-04-16 | 2026-04-16 |
| PH1-05 | Generate Repositories.md | Done | 2026-04-16 | 2026-04-16 |
| PH1-06 | Generate Services.md | Done | 2026-04-16 | 2026-04-16 |
| PH1-07 | Generate DeveloperSetup.md | Done | 2026-04-16 | 2026-04-16 |
| PH1-08 | Generate MigrationHistory.md | Done | 2026-04-16 | 2026-04-16 |
| PH1-09 | Generate gap-analysis-phase1.txt | Done | 2026-04-16 | 2026-04-16 |
| PH1-10 | Generate structure-phase1.txt (V1 folder snapshot) | Done | 2026-04-16 | 2026-04-16 |
| PH1-11 | Generate baseline-architecture-phase1.txt | Done | 2026-04-16 | 2026-04-16 |
| PH1-12 | Generate AbsenceApp_PRD.md | Done | 2026-04-16 | 2026-04-16 |
----------+---------------------------------------------------------------+--------------+------------------+------------------+
| PH1-13 | Generate AbsenceApp_DECISIONS.md | Done | 2026-04-16 | 2026-04-16 |
----------+---------------------------------------------------------------+--------------+------------------+------------------+
| PH1-14 | Generate AbsenceApp_PROGRESS.md | Done | 2026-04-16 | 2026-04-16 |
| PH1-15 | Generate AbsenceApp_PROJECT_TRACKER.md | Done | 2026-04-16 | 2026-04-16 |
| PH1-16 | Verify solution builds with 0 errors | Done | 2026-04-16 | 2026-04-16 |
| PH1-17 | Verify Roslyn analyzer AA0001 active | Done | 2026-04-16 | 2026-04-16 |
| PH1-18 | Verify Git pre-commit hook wired (.githooks/) | Done | 2026-04-16 | 2026-04-16 |
| PH1-19 | Verify EF Core connected to MariaDB | Done | 2026-04-16 | 2026-04-16 |
| PH1-20 | Verify __EFMigrationsHistory registered | Done | 2026-04-16 | 2026-04-16 |
| PH1-21 | Verify 43 V1 pages present and functional | Done | 2026-04-16 | 2026-04-16 |
| PH1-22 | Verify sidebar, header, breadcrumb working (V1) | Done | 2026-04-16 | 2026-04-16 |
| PH1-23 | Verify Light/Dark theme toggle working | Done | 2026-04-16 | 2026-04-16 |
| PH1-24 | Verify window state persistence working | Done | 2026-04-16 | 2026-04-16 |
| PH1-25 | Complete 40-item V1 shell audit checklist | Done | 2026-04-16 | 2026-04-16 |
| PH1-26 | Freeze V1 pages, CSS, and layout (Overlay Rules active) | Done | 2026-04-16 | 2026-04-16 |
--------+-----------------------------------------------+----------+---------+-----------+

---

## Phase 2 — CSS Tokens + JSON Config + Core Components
--------+-----------------------------------------------+----------+---------+-----------+

----------+---------------------------------------------------------------+--------------+------------------+------------------+
> Gate: user says "go ahead Phase 2" before starting.

| Task ID | Description | Status | Started | Completed |
----------+---------------------------------------------------------------+--------------+------------------+------------------+
|---------|-------------|--------|---------|-----------|
----------+---------------------------------------------------------------+--------------+------------------+------------------+
| PH2-01 | Create CSS token files (wwwroot/css/tokens/) | Not Started | — | — |
| PH2-02 | Create theme.json (wwwroot/config/designsystem/) | Not Started | — | — |
| PH2-03 | Create menu.json | Not Started | — | — |
| PH2-04 | Create table-schema.json | Not Started | — | — |
| PH2-05 | Create components.json | Not Started | — | — |
| PH2-06 | Create icons.json | Not Started | — | — |
| PH2-07 | Create branding.json | Not Started | — | — |
----------+---------------------------------------------------------------+--------------+------------------+------------------+
| PH2-08 | Build Icon component (Components/DesignSystem/Icon.razor) | Not Started | — | — |
| PH2-09 | Build IconButton component | Not Started | — | — |
| PH2-10 | Build SectionHeader component | Not Started | — | — |
----------+---------------------------------------------------------------+--------------+------------------+------------------+
| PH2-11 | Build Card component | Not Started | — | — |
----------+---------------------------------------------------------------+--------------+------------------+------------------+
| PH2-12 | Implement DesignSystemConfigService | Not Started | — | — |
| PH2-13 | Implement IconService | Not Started | — | — |
| PH2-14 | Verify all 43 V1 pages unaffected by Phase 2 changes | Not Started | — | — |
| PH2-15 | Phase 2 verification pass | Not Started | — | — |

----------+---------------------------------------------------------------+--------------+------------------+------------------+
--------+-----------------------------------------------+----------+---------+-----------+
---

----------+---------------------------------------------------------------+--------------+------------------+------------------+
## Phase 3 — Layout V2
----------+---------------------------------------------------------------+--------------+------------------+------------------+

--------+-----------------------------------------------+----------+---------+-----------+
> Gate: user says "go ahead Phase 3" before starting.

| Task ID | Description | Status | Started | Completed |
----------+---------------------------------------------------------------+--------------+------------------+------------------+
|---------|-------------|--------|---------|-----------|
| PH3-01 | Implement GlobalErrorBoundaryV2 | Not Started | — | — |
| PH3-02 | Implement ScrollSpy service | Not Started | — | — |
----------+---------------------------------------------------------------+--------------+------------------+------------------+
--------+-----------------------------------------------+----------+---------+-----------+
----------+---------------------------------------------------------------+--------------+------------------+------------------+
| PH3-03 | Implement LayoutV2 container (Components/LayoutV2/) | Not Started | — | — |
| PH3-04 | Implement HeaderV2 | Not Started | — | — |
| PH3-05 | Implement SidebarV2 | Not Started | — | — |
----------+---------------------------------------------------------------+--------------+------------------+------------------+
| PH3-06 | Implement BreadcrumbV2 | Not Started | — | — |
--------+-----------------------------------------------+----------+---------+-----------+
| PH3-07 | Implement theme persistence (localStorage or config file) | Not Started | — | — |
----------+---------------------------------------------------------------+--------------+------------------+------------------+
| PH3-08 | Re-run all 40 V1 shell tests after LayoutV2 integration | Not Started | — | — |
----------+---------------------------------------------------------------+--------------+------------------+------------------+
| PH3-09 | Phase 3 verification pass | Not Started | — | — |

---

## Phase 4 — Table System V2

> Gate: user says "go ahead Phase 4" before starting.
----------+---------------------------------------------------------------+--------------+------------------+------------------+

| Task ID | Description | Status | Started | Completed |
|---------|-------------|--------|---------|-----------|
----------+---------------------------------------------------------------+--------------+------------------+------------------+
| PH4-01 | Build TableV2 component (Components/TableV2/) | Not Started | — | — |
----------+---------------------------------------------------------------+--------------+------------------+------------------+
| PH4-02 | Build column schema loader | Not Started | — | — |
| PH4-03 | Build pagination module | Not Started | — | — |
| PH4-04 | Build sorting module | Not Started | — | — |
| PH4-05 | Build filtering module (filter row under header) | Not Started | — | — |
| PH4-06 | Build row selection module (click-to-select) | Not Started | — | — |
| PH4-07 | Build row actions module (inline icon-only buttons) | Not Started | — | — |
| PH4-08 | Build empty state module ("No records found") | Not Started | — | — |
----------+---------------------------------------------------------------+--------------+------------------+------------------+
| PH4-09 | Build skeleton row loading state | Not Started | — | — |
| PH4-10 | Verify table on a sample dataset | Not Started | — | — |
| PH4-11 | Phase 4 verification pass | Not Started | — | — |
----------+---------------------------------------------------------------+--------------+------------------+------------------+

----------+---------------------------------------------------------------+--------------+------------------+------------------+
---

## Phase 5 — Page Templates V2

> Gate: user says "go ahead Phase 5" before starting.

| Task ID | Description | Status | Started | Completed |
----------+---------------------------------------------------------------+--------------+------------------+------------------+
|---------|-------------|--------|---------|-----------|
| PH5-01 | Build ListPage template (Components/PageTemplatesV2/) | Not Started | — | — |
| PH5-02 | Build DetailPage template | Not Started | — | — |
----------+---------------------------------------------------------------+--------------+------------------+------------------+
| PH5-03 | Build EditPage template | Not Started | — | — |
----------+---------------------------------------------------------------+--------------+------------------+------------------+
| PH5-04 | Build CreatePage template | Not Started | — | — |
| PH5-05 | Build DashboardPage template | Not Started | — | — |
----------+---------------------------------------------------------------+--------------+------------------+------------------+
--------+-----------------------------------------------+----------+---------+-----------+
| PH5-06 | Integrate TableV2 into list and dashboard templates | Not Started | — | — |
| PH5-07 | Phase 5 verification pass | Not Started | — | — |
----------+---------------------------------------------------------------+--------------+------------------+------------------+

----------+---------------------------------------------------------------+--------------+------------------+------------------+
---
--------+-----------------------------------------------+----------+---------+-----------+

## Phase 6 — Data & Services V2

----------+---------------------------------------------------------------+--------------+------------------+------------------+
> Gate: user says "go ahead Phase 6" before starting.

| Task ID | Description | Status | Started | Completed |
| PH6-04 | Implement error handling layer | Not Started | — | — |
| PH6-05 | Implement logging layer | Not Started | — | — |
| PH6-06 | Phase 6 verification pass | Not Started | — | — |

---

--------+-----------------------------------------------+----------+---------+-----------+
## Phase 7 — Feature Modules V2

> Gate: user says "go ahead Phase 7" before starting.

--------+-----------------------------------------------+----------+---------+-----------+
| Task ID | Description | Status | Started | Completed |
|---------|-------------|--------|---------|-----------|
| PH7-01 | Build Student Management V2 (ModulesV2/) | Not Started | — | — |
| PH7-02 | Build Staff Management V2 | Not Started | — | — |
| PH7-03 | Build Student Absence Recording V2 | Not Started | — | — |
| PH7-04 | Build Staff Absence Recording V2 | Not Started | — | — |
| PH7-05 | Build persistent absence alert system | Not Started | — | — |
| PH7-06 | Build email reminder system (teacher notifications) | Not Started | — | — |
| PH7-07 | Build CSV import from MIS | Not Started | — | — |
| PH7-08 | Build CSV export | Not Started | — | — |
| PH7-09 | Build PDF export | Not Started | — | — |
| PH7-10 | Phase 7 verification pass | Not Started | — | — |

---

## Phase 8 — Theming & Branding V2

> Gate: user says "go ahead Phase 8" before starting.

| Task ID | Description | Status | Started | Completed |
|---------|-------------|--------|---------|-----------|
| PH8-01 | Implement theme switching logic (Services/Theming/) | Not Started | — | — |
| PH8-02 | Implement branding loader | Not Started | — | — |
--------+-----------------------------------------------+----------+---------+-----------+
| PH8-03 | Implement custom colour palette support | Not Started | — | — |
| PH8-04 | Implement high-contrast mode | Not Started | — | — |
| PH8-05 | Implement dark mode refinements | Not Started | — | — |
| PH8-06 | Phase 8 verification pass | Not Started | — | — |
--------+-----------------------------------------------+----------+---------+-----------+

---

## Phase 9 — Reusability Framework
--------+-----------------------------------------------+----------+---------+-----------+

> Gate: user says "go ahead Phase 9" before starting.

| Task ID | Description | Status | Started | Completed |
--------+-----------------------------------------------+----------+---------+-----------+
|---------|-------------|--------|---------|-----------|
| PH9-01 | Build component registry | Not Started | — | — |
| PH9-02 | Build reusable form controls | Not Started | — | — |
| PH9-03 | Build reusable layout primitives | Not Started | — | — |
| PH9-04 | Build reusable data services | Not Started | — | — |
| PH9-05 | Build reusable validation rules | Not Started | — | — |
| PH9-06 | Phase 9 verification pass | Not Started | — | — |
--------+-----------------------------------------------+----------+---------+-----------+

---

| Task ID | Description | Status | Started | Completed |
|---------|-------------|--------|---------|-----------|
| PH10-01 | Full regression test across all pages | Not Started | — | — |
| PH10-02 | Full accessibility audit | Not Started | — | — |
| PH10-03 | Full performance audit | Not Started | — | — |
| PH10-04 | Full database validation | Not Started | — | — |
| PH10-05 | Full migration validation (V1 → V2 component swap) | Not Started | — | — |
| PH10-06 | Finalise all documentation | Not Started | — | — |
| PH10-07 | Prepare release package | Not Started | — | — |
| PH10-08 | Phase 10 verification pass — final sign off | Not Started | — | — |

---

_Track 1 — V2 Design System Phases (for reference only)

| Phase | Name | Status | Date Completed | Gate Condition |
|-------|------|--------|----------------|----------------|
| 1 | Foundations & Documentation | ✅ Complete | 2026-04-16 | — |
| 2 | CSS Tokens + JSON Config + Core Components | ⬜ Not Started | — | User: "go ahead Phase 2" |
| 3 | Layout V2 (Sidebar, Header, Breadcrumb, ScrollSpy) | ⬜ Not Started | — | User: "go ahead Phase 3" |
| 4 | Table System V2 | ⬜ Not Started | — | User: "go ahead Phase 4" |
| 5 | Page Templates V2 | ⬜ Not Started | — | User: "go ahead Phase 5" |
| 6 | Data & Services V2 | ⬜ Not Started | — | User: "go ahead Phase 6" |
| 7 | Feature Modules V2 | ⬜ Not Started | — | User: "go ahead Phase 7" |
| 8 | Theming & Branding V2 | ⬜ Not Started | — | User: "go ahead Phase 8" |
| 9 | Reusability Framework | ⬜ Not Started | — | User: "go ahead Phase 9" |
| 10 | Integration & QA | ⬜ Not Started | — | User: "go ahead Phase 10" |

### Track 2 — Database Migration

| Step | Description | Status | Date |
|------|-------------|--------|------|
| DB-01 | SQL Server Express schema designed (13-phase plan) | ✅ Complete | Pre-2026-04-16 |
| DB-02 | SQL Server schema built (Phases 1–13) | ✅ Complete | Pre-2026-04-16 |
| DB-03 | EF Core initial migration on SQL Server (`InitialCreate`) | ✅ Complete | 2026-03-13 |
| DB-04 | Decision made to migrate to MariaDB 10.4.32 | ✅ Complete | 2026-04-16 |
| DB-05 | Pomelo.EntityFrameworkCore.MySql 8.0.2 installed | ✅ Complete | 2026-04-16 |
| DB-06 | EF Core bumped from 8.0.0 → 8.0.2 | ✅ Complete | 2026-04-16 |
| DB-07 | `DataServiceRegistration.cs` updated to UseMySql with MariaDbServerVersion | ✅ Complete | 2026-04-16 |
| DB-08 | `AppDbContextFactory.cs` updated to UseMySql | ✅ Complete | 2026-04-16 |
| DB-09 | `EfHost/Program.cs` updated to MySQL connection string | ✅ Complete | 2026-04-16 |
| DB-10 | `Api/appsettings.json` updated to MariaDB connection string | ✅ Complete | 2026-04-16 |
| DB-11 | `Client/appsettings.json` updated to MariaDB connection string | ✅ Complete | 2026-04-16 |
| DB-12 | SQL Server migrations deleted | ✅ Complete | 2026-04-16 |
| DB-13 | `InitialMySQL` migration generated (`20260416103036_InitialMySQL.cs`) | ✅ Complete | 2026-04-16 |
| DB-14 | `InitialMySQL` migration `Up()` emptied (schema already exists) | ✅ Complete | 2026-04-16 |
| DB-15 | `__EFMigrationsHistory` table created in MySQL | ✅ Complete | 2026-04-16 |
| DB-16 | `InitialMySQL` row inserted into `__EFMigrationsHistory` | ✅ Complete | 2026-04-16 |
| DB-17 | `MenuResolver.cs` updated to MySqlConnector | ✅ Complete | 2026-04-16 |
| DB-18 | `FeaturePermissionResolver.cs` updated to MySqlConnector | ✅ Complete | 2026-04-16 |
| DB-19 | Full solution build — 0 errors, 0 warnings | ✅ Complete | 2026-04-16 |
| DB-20 | MySQL equivalents of fn_GetVisibleMenuItems and fn_IsFeatureAllowed | ⬜ Not Done | — |
| DB-21 | RoleMenuItems table created during MySQL migration | ✅ Complete | 2026-04-16 |
| DB-22 | Began inserting real data into MySQL for end-to-end application validation | ✅ In Progress | 2026-04-16 |
| DB-23 | Seeded baseline data into `feature` and `roletypes` tables (6 feature records + system role types) | ✅ Complete | 2026-04-16 |



### Track 3 — Application Feature Completeness

| Domain | Feature | Status | Notes |
|--------|---------|--------|-------|
| Shell | Splash screen | ✅ Working | Smooth, no white flash |
| Shell | Login screen | ✅ Working | No flicker |
| Shell | Sidebar | ✅ Working | Collapse/expand smooth |
| Shell | Sidebar icon rendering | ✅ Working | Both themes |
| Shell | Sidebar labels | ✅ Working | Correct in both modes |
| Shell | Sidebar hover states | ✅ Working | Correct |
| Shell | Sidebar active highlight | ✅ Working | Correct |
| Shell | Sidebar/Breadcrumb sync | ✅ Working | Always in sync |
| Shell | Header | ✅ Working | Loads correctly |
| Shell | Header buttons | ✅ Working | All responsive |
| Shell | Breadcrumb | ✅ Working | Loads and updates correctly |
| Shell | Page title | ⚠️ Partial | Not fully testable — most content pages not yet implemented |
| Shell | Theme toggle (Light/Dark) | ✅ Working | Smooth, instant |
| Shell | Theme persistence (cold start) | ❌ Defect | Always resets to Light on cold start (DEC-052) |
| Shell | System theme detection | ✅ Working | Correct |
| Shell | Window state persistence | ✅ Working | Size and position retained |
| Shell | Tray icon behaviour | ✅ Working | Minimise/restore correct |
| Shell | Multi-monitor | ✅ Working | No DPI issues |
| Shell | Scroll behaviour | ✅ Working | Smooth |
| Shell | Scrollbar styling | ✅ Working | Styled correctly |
| Shell | Window resizing | ✅ Working | Clean reflow |
| Shell | Fullscreen/Maximise | ✅ Working | No DPI issues |
| Shell | Global error boundary | ❌ Defect | Blank screen on any error — critical (DEC-053) |
| Shell | Global keyboard shortcuts | ❌ Not Implemented | Missing feature (not a defect) |
| Shell | Loading state behaviour | ⚠️ Partial | Cannot test — no data-driven pages yet |
| Data | Real data population into MySQL | ⬜ In Progress | Started inserting real records to validate app behaviour against the new database backend |
| Users | User management pages | ⚠️ Partial | V1 pages exist; V2 feature modules not started |
| Staff | Staff management pages | ⬜ Not Started | — |
| Students | Student management pages | ⬜ Not Started | — |
| Absences | Student absence recording | ⬜ Not Started | — |
| Absences | Staff absence recording | ⬜ Not Started | — |
| Absences | Persistent absence alerts | ⬜ Not Started | — |
| Absences | Email reminders | ⬜ Not Started | — |
| Import | CSV import from MIS | ⬜ Not Started | — |
| Export | CSV export | ⬜ Not Started | — |
| Export | PDF export | ⬜ Not Started | — |
| Notifications | Header notification icon | ⬜ Not Started | — |
| Settings | Global settings management | ⬜ Not Started | — |
| Permissions | Role-based menu filtering | ✅ Backend coded | MenuResolver.cs wired; MySQL functions needed |
| Permissions | Feature permission checks | ✅ Backend coded | FeaturePermissionResolver.cs wired; MySQL functions needed |
| Permissions | RoleMenuItems mapping | ✅ Implemented | New explicit Role → MenuItem mapping introduced during MySQL migration |
| Auth | Windows login auto-detect | ⬜ Not Started | Mechanism not yet defined |

### Track 4 — Documentation

| Document | Status | Location |
|----------|--------|----------|
| Architecture.md | ✅ Written | docs/architecture/ |
| CodingStandards.md | ✅ Written | docs/architecture/ |
[================================================================================
| DTOsAndMappers.md | ✅ Written | docs/architecture/ |
| Repositories.md | ✅ Written | docs/architecture/ |
| Services.md | ✅ Written | docs/architecture/ |
| DeveloperSetup.md | ✅ Written | docs/architecture/ |
| MigrationHistory.md | ✅ Written | docs/migration/notes/ |
| AbsenceApp_DECISIONS.md | ✅ Written | docs/project-governance/ |
| AbsenceApp_PROGRESS.md | ✅ Written | docs/project-governance/ |


## 2. Current Phase Status

| Item | Value |
|------|-------|
| **Active Track** | Track 1 — V2 Design System |
----------+--------------------------------------------------------------------------+--------------+------------------+------------------+
| **Current Phase** | Phase 1 (Complete) — awaiting gate confirmation for Phase 2 |
----------+--------------------------------------------------------------------------+--------------+------------------+------------------+
| **Current Step** | None — Phase 1 is fully complete |
| **Last Completed Phase** | Phase 1 — Foundations & Documentation |
| **Last Completed Step** | Generation of governance documents (PRD, Decisions, Progress) |
| **Overall Status** | On track — Phase 1 complete, Phase 2 not yet confirmed |
| **Blockers** | See Section 5 |
| **Next Planned Action** | Await user gate confirmation: "go ahead Phase 2" |

---

## 3. Phase 1 — Completed Deliverables

Phase 1 (Foundations & Documentation) is fully complete. All deliverables are listed below.

### 3.1 Documentation Package

| Deliverable | File | Status |
|-------------|------|--------|
| Architecture document | `docs/architecture/Architecture.md` | ✅ Done |
| Coding standards | `docs/architecture/CodingStandards.md` | ✅ Done |
| Data model | `docs/architecture/DataModel.md` | ✅ Done |
----------+--------------------------------------------------------------------------+--------------+------------------+------------------+
| DTOs and mappers | `docs/architecture/DTOsAndMappers.md` | ✅ Done |
| Repositories | `docs/architecture/Repositories.md` | ✅ Done |
| Services | `docs/architecture/Services.md` | ✅ Done |
----------+--------------------------------------------------------------------------+--------------+------------------+------------------+
| Developer setup | `docs/architecture/DeveloperSetup.md` | ✅ Done |
----------+--------------------------------------------------------------------------+--------------+------------------+------------------+
| Migration history | `docs/migration/notes/MigrationHistory.md` | ✅ Done |
| Gap analysis | `docs/migration/notes/gap-analysis-phase1.txt` | ✅ Done |
| V1 structure snapshot | `docs/migration/notes/structure-phase1.txt` | ✅ Done |
| Baseline architecture (frozen/safe-to-extend) | `docs/legacy/baseline-architecture-phase1.txt` | ✅ Done |
| V2 governance: PRD | `docs/project-governance/AbsenceApp_PRD.md` | ✅ Done |
| V2 governance: Decisions | `docs/project-governance/AbsenceApp_DECISIONS.md` | ✅ Done |
| V2 governance: Progress | `docs/project-governance/AbsenceApp_PROGRESS.md` | ✅ Done |
----------+--------------------------------------------------------------------------+--------------+------------------+------------------+

### 3.2 Code Infrastructure Verified Working (Phase 1 Baseline)

----------+--------------------------------------------------------------------------+--------------+------------------+------------------+
| Item | Status |
----------+--------------------------------------------------------------------------+--------------+------------------+------------------+
|------|--------|
| Solution builds with 0 errors | ✅ |
| Roslyn analyzer AA0001 active | ✅ |
| Git pre-commit hook wired (`.githooks/`) | ✅ |
| EF Core connected to MariaDB | ✅ |
----------+--------------------------------------------------------------------------+--------------+------------------+------------------+
| `__EFMigrationsHistory` registered | ✅ |
| 43 V1 pages present and functional | ✅ |
| Sidebar, Header, Breadcrumb working (V1) | ✅ |
----------+--------------------------------------------------------------------------+--------------+------------------+------------------+
| Light/Dark theme toggle working | ✅ |
----------+--------------------------------------------------------------------------+--------------+------------------+------------------+
| Window state persistence working | ✅ |

---

## 4. Phase 2 — Planned Deliverables (Not Started)
----------+--------------------------------------------------------------------------+--------------+------------------+------------------+

Phase 2 has not been confirmed. The following is the planned scope when confirmed:

----------+--------------------------------------------------------------------------+--------------+------------------+------------------+
| Deliverable | Planned Path |
----------+--------------------------------------------------------------------------+--------------+------------------+------------------+
|-------------|-------------|
| CSS token files | `wwwroot/css/tokens/` |
| Design system JSON config files | `wwwroot/config/designsystem/theme.json` |
----------+--------------------------------------------------------------------------+--------------+------------------+------------------+
| | `wwwroot/config/designsystem/menu.json` |
| | `wwwroot/config/designsystem/table-schema.json` |
| | `wwwroot/config/designsystem/components.json` |
----------+--------------------------------------------------------------------------+--------------+------------------+------------------+
| | `wwwroot/config/designsystem/icons.json` |
----------+--------------------------------------------------------------------------+--------------+------------------+------------------+
| | `wwwroot/config/designsystem/branding.json` |
| Design system core components | `Components/DesignSystem/Icon.razor` |
| | `Components/DesignSystem/IconButton.razor` |
[================================================================================
| | `Components/DesignSystem/Card.razor` |
| Design services | `Services/DesignSystemConfigService.cs` |
| | `Services/IconService.cs` |
----------+--------------------------------------------------------------------------+--------------+------------------+------------------+


----------+--------------------------------------------------------------------------+--------------+------------------+------------------+
---

## 5. Blockers

| ID | Blocker | Severity | Owner |
|----|---------|----------|-------|
| BLK-01 | Global error boundary not implemented. `GlobalErrorBoundaryV2` component does not exist. App crashes on any unhandled navigation error. | Critical | Phase 3 (Layout V2) |
----------+--------------------------------------------------------------------------+--------------+------------------+------------------+
| BLK-02 | Theme persistence on cold start not implemented. Theme resets to Light mode after full exit/relaunch. | Medium | Phase 2 or Phase 3 |
----------+--------------------------------------------------------------------------+--------------+------------------+------------------+
----------+--------------------------------------------------------------------------+--------------+------------------+------------------+
| BLK-03 | MySQL equivalents of `fn_GetVisibleMenuItems` and `fn_IsFeatureAllowed` not created. Role-based menu filtering and feature controls will not execute correctly in production. | High | Database track |
| BLK-04 | Phase 2 gate not yet confirmed by user. | Process | User action required |

----------+--------------------------------------------------------------------------+--------------+------------------+------------------+
---
----------+--------------------------------------------------------------------------+--------------+------------------+------------------+

## 6. Open Defects

| ID | Description | Severity | Origin |
|----|-------------|----------|--------|
| DEF-01 | Global error boundary missing — blank screen on any unhandled error | Critical | Testing Q37 |
| DEF-02 | Theme resets to Light on cold start (warm start works correctly) | Medium | Testing Q26 |
----------+--------------------------------------------------------------------------+--------------+------------------+------------------+
| DEF-03 | Page title consistency not fully verifiable — most content pages absent | Low | Testing Q35 |
| DEF-04 | Loading state not verifiable — no data-driven pages exist yet | Low | Testing Q36 |
| DEF-05 | Breadcrumb post-error state not evaluable (depends on error boundary fix) | Low | Testing Q38 |
----------+--------------------------------------------------------------------------+--------------+------------------+------------------+
| DEF-06 | Sidebar post-error state not evaluable (depends on error boundary fix) | Low | Testing Q39 |
----------+--------------------------------------------------------------------------+--------------+------------------+------------------+
----------+--------------------------------------------------------------------------+--------------+------------------+------------------+

---
----------+--------------------------------------------------------------------------+--------------+------------------+------------------+
----------+--------------------------------------------------------------------------+--------------+------------------+------------------+
----------+--------------------------------------------------------------------------+--------------+------------------+------------------+

## 7. V2 Overlay Rules (Active Constraints)

----------+--------------------------------------------------------------------------+--------------+------------------+------------------+
These rules are active for the duration of all V2 design system phases:
----------+--------------------------------------------------------------------------+--------------+------------------+------------------+

----------+--------------------------------------------------------------------------+--------------+------------------+------------------+
| Rule | Description |
|------|-------------|
| OVERLAY-01 | Do not modify any of the 43 V1 pages. |
----------+--------------------------------------------------------------------------+--------------+------------------+------------------+
| OVERLAY-02 | Do not modify any V1 layout components. |
----------+--------------------------------------------------------------------------+--------------+------------------+------------------+
----------+--------------------------------------------------------------------------+--------------+------------------+------------------+
| OVERLAY-03 | Do not modify any V1 CSS. |
| OVERLAY-04 | All new V2 components live in new, namespaced paths only. |
| OVERLAY-05 | The app shell must be functional at 100% of the time throughout all V2 phases. |
| OVERLAY-06 | A V2 component may only replace a V1 component after the V2 component passes all 40 test checklist items. |
----------+--------------------------------------------------------------------------+--------------+------------------+------------------+
| OVERLAY-07 | No V2 phase may begin without explicit user gate confirmation. |
| OVERLAY-08 | Each V2 phase must have a documented verification pass before the next phase begins. |

----------+--------------------------------------------------------------------------+--------------+------------------+------------------+
---
----------+--------------------------------------------------------------------------+--------------+------------------+------------------+

## 8. Testing Checklist Summary (Phase 1 — V1 Shell Audit)

The 40-item shell testing checklist was completed as part of Phase 1. Results summary:

----------+--------------------------------------------------------------------------+--------------+------------------+------------------+
| Result Code | Meaning | Count |
|-------------|---------|-------|
| A — Pass | Working correctly | 29 |
----------+--------------------------------------------------------------------------+--------------+------------------+------------------+
| B — Pass with caveat | Working with a known limitation | 1 |
----------+--------------------------------------------------------------------------+--------------+------------------+------------------+
| C — Failure | Known defect | 2 |
| E — Not evaluable | Cannot test yet (missing pages/data) | 8 |

----------+--------------------------------------------------------------------------+--------------+------------------+------------------+
| Item | Topic | Result |
|------|-------|--------|
| 0 | Login screen behaviour | A — Pass |
----------+--------------------------------------------------------------------------+--------------+------------------+------------------+
| 1 | App launch + splash | A — Pass |
----------+--------------------------------------------------------------------------+--------------+------------------+------------------+
| 2 | Sidebar loads | A — Pass |
| 3 | Sidebar navigation | A — Pass |
| 4 | Breadcrumb loads | A — Pass |
| 5 | Breadcrumb updates | A — Pass |
| 6 | Page title loads | A — Pass |
| 7 | Page title updates | A — Pass |
| 8 | Header loads | A — Pass |
----------+--------------------------------------------------------------------------+--------------+------------------+------------------+
| 9 | Header buttons | A — Pass |
| 10 | Theme toggle | A — Pass |
| 11 | Sidebar collapse/expand | A — Pass |
----------+--------------------------------------------------------------------------+--------------+------------------+------------------+
| 12 | Sidebar icons | A — Pass |
----------+--------------------------------------------------------------------------+--------------+------------------+------------------+
| 13 | Sidebar labels | A — Pass |
| 14 | Sidebar hover states | A — Pass |
| 15 | Sidebar active highlight | A — Pass |
| 16 | Sidebar width | A — Pass |
| 17 | Collapsed width | A — Pass |
| 18 | Expanded width | A — Pass |
| 19 | Header height | A — Pass |
----------+--------------------------------------------------------------------------+--------------+------------------+------------------+
| 20 | Breadcrumb spacing | A — Pass |
| 21 | Page padding | A — Pass |
| 22 | Scroll behaviour | A — Pass |
----------+--------------------------------------------------------------------------+--------------+------------------+------------------+
| 23 | Scrollbar styling | A — Pass |
----------+--------------------------------------------------------------------------+--------------+------------------+------------------+
| 24 | Window resizing | A — Pass |
| 25 | Fullscreen / maximise | A — Pass |
| 26 | Theme persistence | **B — Cold start resets to Light** |
| 27 | System theme detection | A — Pass |
| 28 | Global hotkeys | **C — Not implemented** |
| 29 | Window state persistence | A — Pass |
| 30 | Tray icon behaviour | A — Pass |
----------+--------------------------------------------------------------------------+--------------+------------------+------------------+
| 31 | Multi-monitor behaviour | A — Pass |
| 32 | Startup behaviour (splash → login → shell) | A — Pass |
| 33 | Sidebar animation | A — Pass |
----------+--------------------------------------------------------------------------+--------------+------------------+------------------+
| 34 | Sidebar/Breadcrumb sync | A — Pass |
----------+--------------------------------------------------------------------------+--------------+------------------+------------------+
| 35 | Page title consistency | **E — Not evaluable (no content pages)** |
| 36 | Loading state behaviour | **E — Not evaluable (no data-driven pages)** |
----------+--------------------------------------------------------------------------+--------------+------------------+------------------+
| 37 | Error boundary behaviour | **C — Failure: blank screen / crash** |
| 38 | Breadcrumb during errors | **E — Not evaluable (depends on DEF-01)** |
| 39 | Sidebar after errors | **E — Not evaluable (depends on DEF-01)** |
----------+--------------------------------------------------------------------------+--------------+------------------+------------------+
| 40 | (Additional items — not evaluable at this stage) | E |
----------+--------------------------------------------------------------------------+--------------+------------------+------------------+

---

## 9. Next Planned Actions

----------+--------------------------------------------------------------------------+--------------+------------------+------------------+
| Priority | Action | Phase | Condition |
|----------|--------|-------|-----------|
| 1 | User confirms gate: "go ahead Phase 2" | 2 | User action |
| 2 | Create CSS token files under `wwwroot/css/tokens/` | 2 | After gate |
| 3 | Create design system JSON config files | 2 | After gate |
| 4 | Create core design system components (Icon, IconButton, SectionHeader, Card) | 2 | After gate |
| 5 | Create DesignSystemConfigService and IconService | 2 | After gate |
| 6 | Create MySQL equivalents for `fn_GetVisibleMenuItems` and `fn_IsFeatureAllowed` | DB | Any time |
| 7 | Implement global error boundary (GlobalErrorBoundaryV2) | 3 | Phase 3 gate |
| 8 | Implement theme persistence (localStorage or config file) | 2 or 3 | Phase 2/3 |

---

## 10. Solution Build State (Last Verified)

| Item | State |
|------|-------|
| Build result | ✅ 0 errors, 0 warnings |
| Date verified | 2026-04-16 |
| EF Core version | 8.0.2 |
| Pomelo version | 8.0.2 |
| Target framework | net8.0 |
| Database connection | MariaDB 10.4.32 @ 127.0.0.1:3306 |
| EF migration applied | `20260416103036_InitialMySQL` |


## 11. Phase‑Driven Master Task List (Backlog)

This section lists all actionable tasks grouped by the 10 V2 phases. 
Each task is a one‑liner and can be ticked off independently.

---

### Phase 1 — Foundations & Documentation (Complete)
- [x] Generate PRD
- [x] Generate DECISIONS file
- [x] Generate PROJECT_TRACKER file
- [x] Generate architecture documentation
- [x] Generate migration notes
- [x] Verify V1 shell behaviour (40‑item checklist)
- [x] Freeze V1 pages, CSS, and layout (Overlay Rules)

---

### Phase 2 — CSS Tokens + JSON Config + Core Components
- [ ] Create CSS token files (`wwwroot/css/tokens/`)
- [ ] Create theme.json
- [ ] Create menu.json
- [ ] Create table-schema.json
- [ ] Create components.json
- [ ] Create icons.json
- [ ] Create branding.json
- [ ] Build Icon component
- [ ] Build IconButton component
- [ ] Build SectionHeader component
- [ ] Build Card component
- [ ] Implement DesignSystemConfigService
- [ ] Implement IconService
- [ ] Verify V1 pages remain unaffected
- [ ] Phase 2 verification pass

---

### Phase 3 — Layout V2 (Sidebar, Header, Breadcrumb, ScrollSpy)
- [ ] Implement GlobalErrorBoundaryV2
- [ ] Implement ScrollSpy service
- [ ] Implement LayoutV2 container
- [ ] Implement HeaderV2
- [ ] Implement SidebarV2
- [ ] Implement BreadcrumbV2
- [ ] Implement theme persistence (if not done in Phase 2)
- [ ] Verify all 40 shell tests again
- [ ] Phase 3 verification pass

---

### Phase 4 — Table System V2
- [ ] Build TableV2 component
- [ ] Build column schema loader
- [ ] Build pagination module
- [ ] Build sorting module
- [ ] Build filtering module
- [ ] Build row selection module
- [ ] Build row actions module
- [ ] Build empty state module
- [ ] Verify table behaviour on sample dataset
- [ ] Phase 4 verification pass

---

### Phase 5 — Page Templates V2
- [ ] Build ListPage template
- [ ] Build DetailPage template
- [ ] Build EditPage template
- [ ] Build CreatePage template
- [ ] Build DashboardPage template
- [ ] Integrate TableV2 into templates
- [ ] Phase 5 verification pass

---

### Phase 6 — Data & Services V2
- [ ] Implement new DataService pattern
- [ ] Implement caching layer
- [ ] Implement validation layer
- [ ] Implement error handling layer
- [ ] Implement logging layer
- [ ] Implement API abstraction layer
- [ ] Phase 6 verification pass

---

### Phase 7 — Feature Modules V2
- [ ] Build Student Management V2
- [ ] Build Staff Management V2
- [ ] Build Absence Recording V2
- [ ] Build Persistent Absence Alerts
- [ ] Build Email Reminder System
- [ ] Build CSV Import
- [ ] Build CSV Export
- [ ] Build PDF Export
- [ ] Phase 7 verification pass

---

### Phase 8 — Theming & Branding V2
- [ ] Implement theme switching logic
- [ ] Implement branding loader
- [ ] Implement custom colour palettes
- [ ] Implement high‑contrast mode
- [ ] Implement dark mode refinements
- [ ] Phase 8 verification pass

---

### Phase 9 — Reusability Framework
- [ ] Build component registry
- [ ] Build reusable form controls
- [ ] Build reusable layout primitives
- [ ] Build reusable data services
- [ ] Build reusable validation rules
- [ ] Phase 9 verification pass

---

### Phase 10 — Integration & QA
- [ ] Full regression test
- [ ] Full accessibility audit
- [ ] Full performance audit
- [ ] Full database validation
- [ ] Full migration validation
- [ ] Finalise documentation
- [ ] Prepare release package
- [ ] Phase 10 verification pass
