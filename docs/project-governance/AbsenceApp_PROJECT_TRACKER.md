---
doc_type: tracker
project_name: "AbsenceApp"
module: "V2"
version: "1.1.0"
created_date: "2026-04-16"
updated_date: "2026-05-02"
authority: "Michael"
ai_contract:
  read_rules: "Agents may read full tracker for context."
  write_rules: "Agents may only update Status, Started, Completed fields; never reorder rows or change TaskID/Description."
---
================================================================================
 File        : AbsenceApp_PROJECT_TRACKER.md
 Author      : Michael
================================================================================
 Created     : 2026-04-16
 Updated     : 2026-05-02
--------------------------------------------------------------------------------
 Purpose     :
   Simple task tracker for AbsenceApp V2. One row per task.
   To start a task: set Status to In Progress, fill in the Started date.
--------------------------------------------------------------------------------
 Status values : Not Started | In Progress | Done
--------------------------------------------------------------------------------
 Changes     :
   - 1.2.0 (2026-04-30) Added YAML front-matter; added AI Rules block; converted
                       tracker to pipe Markdown table; clarified agent constraints
                       and update rules (Status / Started / Completed only).
   - 1.1.0 (2026-04-17) Rewritten as pure task tracker. Removed analysis
                       sections. Added MySQL table population tasks.
                       Corrected column structure (Status / Started / Completed).
   - 1.0.0 (2026-04-16) Initial creation.
--------------------------------------------------------------------------------
 Notes       :
   - This file is the authoritative source of project execution state.
   - The YAML front-matter at the top of this file is the machine header of record.
   - Agents MUST read this file before starting any task.
   - Agents MUST NOT:
       • Start tasks marked Done
       • Modify TaskID or Description fields
       • Reorder rows
   - When work is performed:
       • Status MUST be updated accurately
       • Started date MUST be set only when work begins (ISO-8601 YYYY-MM-DD)
       • Completed date MUST be set only when work fully completes (ISO-8601)
   - Changelogs are append-only; do not edit or delete existing changelog entries.
   - When updating this file manually, keep the boxed header for human readability
     and ensure the boxed header Updated line matches the YAML updated_date.
================================================================================

## AI Rules
**Agents must follow these rules:** Do not reorder rows; do not modify **TaskID** or **Description**; only update **Status**, **Started**, **Completed**; use ISO‑8601 dates (YYYY‑MM‑DD); leave blank if unknown.

## Current Focus
- **Current Phase:** Phase 1 — AI‑readable canonical artefacts
- **Active TaskIDs:** TRK-001, TRK-002, TRK-010
- **Known blockers:** Confirm authority owners for PRD/Tracker/Changelog

# 1. Project Overview
| Phase  | Description                                         | Status      | Started    | Completed   |
|--------|-----------------------------------------------------|-------------|------------|-------------|
| Ph 1   | Foundations & Documentation                         | Done        |            | 2026-04-16  |
| DB     | SQL Server to MySQL Migration                       | Done*       |            | 2026-04-16  |
| DB-POP | MySQL Table Population                              | In Progress |            |             |
| DB-VAL | MySQL End-to-End Validation                         | Not Started |            |             |
| Ph 2   | CSS Tokens + JSON Config + Core Components          | Not Started |            |             |
| Ph 3   | Layout V2 (Sidebar, Header, Breadcrumb, ScrollSpy)  | Not Started |            |             |
| Ph 4   | Table System V2                                     | Not Started |            |             |
| Ph 5   | Page Templates V2                                   | Not Started |            |             |
| Ph 6   | Data & Services V2                                  | Not Started |            |             |
| Ph 7   | Feature Modules V2                                  | Not Started |            |             |
| Ph 8   | Theming & Branding V2                               | Not Started |            |             |
| Ph 9   | Reusability Framework                               | Not Started |            |             |
| Ph 10  | Integration & QA                                    | Not Started |            |             |

  * MIG-21 (MySQL function equivalents) is outstanding in the DB track.

# 2. Phase 1 — Foundations & Documentation
| TaskID | Description                                           | Status      | Started    | Completed  |
|--------|-------------------------------------------------------|-------------|------------|------------|
| PH1-01 | Generate Architecture.md                              | Done        | 2026-04-16 | 2026-04-16 |
| PH1-02 | Generate CodingStandards.md                           | Done        | 2026-04-16 | 2026-04-16 |
| PH1-03 | Generate DataModel.md                                 | Done        | 2026-04-16 | 2026-04-16 |
| PH1-04 | Generate DTOsAndMappers.md                            | Done        | 2026-04-16 | 2026-04-16 |
| PH1-05 | Generate Repositories.md                              | Done        | 2026-04-16 | 2026-04-16 |
| PH1-06 | Generate Services.md                                  | Done        | 2026-04-16 | 2026-04-16 |
| PH1-07 | Generate DeveloperSetup.md                            | Done        | 2026-04-16 | 2026-04-16 |
| PH1-08 | Generate MigrationHistory.md                          | Done        | 2026-04-16 | 2026-04-16 |
| PH1-09 | Generate gap-analysis-phase1.txt                      | Done        | 2026-04-16 | 2026-04-16 |
| PH1-10 | Generate structure-phase1.txt (V1 folder snapshot)    | Done        | 2026-04-16 | 2026-04-16 |
| PH1-11 | Generate baseline-architecture-phase1.txt             | Done        | 2026-04-16 | 2026-04-16 |
| PH1-12 | Generate AbsenceApp_PRD.md                            | Done        | 2026-04-16 | 2026-04-16 |
| PH1-13 | Generate AbsenceApp_DECISIONS.md                      | Done        | 2026-04-16 | 2026-04-16 |
| PH1-14 | Generate AbsenceApp_PROGRESS.md                       | Done        | 2026-04-16 | 2026-04-16 |
| PH1-15 | Generate AbsenceApp_PROJECT_TRACKER.md                | Done        | 2026-04-16 | 2026-04-16 |
| PH1-16 | Set up initial codebase and folder structure          | Done        | 2026-04-16 | 2026-04-16 |
| PH1-17 | Complete gap analysis and baseline architecture       | Done        | 2026-04-16 | 2026-04-16 |
| PH1-18 | Verify solution builds with 0 errors                  | Done        | 2026-04-16 | 2026-04-16 |
| PH1-19 | Verify Roslyn analyzer AA0001 active                  | Done        | 2026-04-16 | 2026-04-16 |
| PH1-20 | Verify Git pre-commit hook wired (.githooks/)         | Done        | 2026-04-16 | 2026-04-16 |
| PH1-21 | Verify EF Core connected to MariaDB                   | Done        | 2026-04-16 | 2026-04-16 |
| PH1-22 | Verify __EFMigrationsHistory registered               | Done        | 2026-04-16 | 2026-04-16 |
| PH1-23 | Verify 43 V1 pages present and functional             | Done        | 2026-04-16 | 2026-04-16 |
| PH1-24 | Verify sidebar, header, breadcrumb working (V1)       | Done        | 2026-04-16 | 2026-04-16 |
| PH1-25 | Verify Light/Dark theme toggle working                | Done        | 2026-04-16 | 2026-04-16 |
| PH1-26 | Verify window state persistence working               | Done        | 2026-04-16 | 2026-04-16 |
| DBP-01 | Import full V2 MenuItems dataset into MySQL           | Done        |            | 2026-04-18 || DBP-20 | EF Core Entity PK/FK Type Alignment with Live MySQL   | Done        | 2026-05-02  | 2026-05-02  |

# 3. Phase 2 — CSS Tokens + JSON Config + Core Components
| TaskID | Description                                           | Status      | Started | Completed |
|--------|-------------------------------------------------------|-------------|---------|-----------|
| PH2-01 | Create CSS token files (wwwroot/css/tokens/)          | Not Started |         |           |
| PH2-02 | Create theme.json (wwwroot/config/designsystem/)      | Not Started |         |           |
| PH2-03 | Create menu.json                                      | Not Started |         |           |
| PH2-04 | Create table-schema.json                              | Not Started |         |           |
| PH2-05 | Create components.json                                | Not Started |         |           |
| PH2-06 | Create icons.json                                     | Not Started |         |           |
| PH2-07 | Create branding.json                                  | Not Started |         |           |
| PH2-08 | Build Icon component (Components/DesignSystem/Icon.razor) | Not Started |     |           |
| PH2-09 | Build IconButton component                            | Not Started |         |           |
| PH2-10 | Build SectionHeader component                         | Not Started |         |           |
| PH2-11 | Build Card component                                  | Not Started |         |           |
| PH2-12 | Implement DesignSystemConfigService                   | Not Started |         |           |
| PH2-13 | Implement IconService                                 | Not Started |         |           |
| PH2-14 | Verify all 43 V1 pages unaffected by Phase 2 changes  | Not Started |         |           |
| PH2-15 | Phase 2 verification pass                             | Not Started |         |           |


# 4. Phase 3 — Layout V2 (Sidebar, Header, Breadcrumb, ScrollSpy)
| TaskID | Description                                               | Status      | Started | Completed |
|--------|-----------------------------------------------------------|-------------|---------|-----------|
| PH3-01 | Implement GlobalErrorBoundaryV2                           | Not Started |         |           |
| PH3-02 | Implement ScrollSpy service                               | Not Started |         |           |
| PH3-03 | Implement LayoutV2 container (Components/LayoutV2/)       | Not Started |         |           |
| PH3-04 | Implement HeaderV2                                        | Not Started |         |           |
| PH3-05 | Implement SidebarV2                                       | Not Started |         |           |
| PH3-06 | Implement BreadcrumbV2                                    | Not Started |         |           |
| PH3-07 | Implement theme persistence (localStorage or config file) | Not Started |         |           |
| PH3-08 | Re-run all 40 V1 shell tests after LayoutV2 integration   | Not Started |         |           |
| PH3-09 | Phase 3 verification pass                                 | Not Started |         |           |


# 5. Phase 4 — Table System V2
| TaskID | Description                                           | Status      | Started | Completed |
|--------|-------------------------------------------------------|-------------|---------|-----------|
| PH4-01 | Build TableV2 component (Components/TableV2/)         | Not Started |         |           |
| PH4-02 | Build column schema loader                            | Not Started |         |           |
| PH4-03 | Build pagination module                               | Not Started |         |           |
| PH4-04 | Build sorting module                                  | Not Started |         |           |
| PH4-05 | Build filtering module (filter row under header)      | Not Started |         |           |
| PH4-06 | Build row selection module (click-to-select)          | Not Started |         |           |
| PH4-07 | Build row actions module (inline icon-only buttons)   | Not Started |         |           |
| PH4-08 | Build empty state module ("No records found")         | Not Started |         |           |
| PH4-09 | Build skeleton row loading state                      | Not Started |         |           |
| PH4-10 | Verify table on a sample dataset                      | Not Started |         |           |
| PH4-11 | Phase 4 verification pass                             | Not Started |         |           |


# 6. Phase 5 — Page Templates V2
| TaskID | Description                                           | Status      | Started | Completed |
|--------|-------------------------------------------------------|-------------|---------|-----------|
| PH5-01 | Build ListPage template (Components/PageTemplatesV2/) | Not Started |         |           |
| PH5-02 | Build DetailPage template                             | Not Started |         |           |
| PH5-03 | Build EditPage template                               | Not Started |         |           |
| PH5-04 | Build CreatePage template                             | Not Started |         |           |
| PH5-05 | Build DashboardPage template                          | Not Started |         |           |
| PH5-06 | Integrate TableV2 into list and dashboard templates   | Not Started |         |           |
| PH5-07 | Phase 5 verification pass                             | Not Started |         |           |


# 7. Phase 6 — Data & Services V2
| TaskID | Description                                           | Status      | Started | Completed |
|--------|-------------------------------------------------------|-------------|---------|-----------|
| PH6-01 | Implement new DataService pattern (Services/DataV2/)  | Not Started |         |           |
| PH6-02 | Implement caching layer                               | Not Started |         |           |
| PH6-03 | Implement validation layer                            | Not Started |         |           |
| PH6-04 | Implement error handling layer                        | Not Started |         |           |
| PH6-05 | Implement logging layer                               | Not Started |         |           |
| PH6-06 | Implement API abstraction layer                       | Not Started |         |           |
| PH6-07 | Phase 6 verification pass                             | Not Started |         |           |


# 8. Phase 7 — Feature Modules V2
| TaskID | Description                                           | Status      | Started | Completed |
|--------|-------------------------------------------------------|-------------|---------|-----------|
| PH7-01 | Build Student Management V2 (ModulesV2/)              | Not Started |         |           |
| PH7-02 | Build Staff Management V2                             | Not Started |         |           |
| PH7-03 | Build Student Absence Recording V2                    | Not Started |         |           |
| PH7-04 | Build Staff Absence Recording V2                      | Not Started |         |           |
| PH7-05 | Build persistent absence alert system                 | Not Started |         |           |
| PH7-06 | Build email reminder system (teacher notifications)   | Not Started |         |           |
| PH7-07 | Build CSV import from MIS                             | Not Started |         |           |
| PH7-08 | Build CSV export                                      | Not Started |         |           |
| PH7-09 | Build PDF export                                      | Not Started |         |           |
| PH7-10 | Phase 7 verification pass                             | Not Started |         |           |


# 9. Phase 8 — Theming & Branding V2
| TaskID | Description                                           | Status      | Started | Completed |
|--------|-------------------------------------------------------|-------------|---------|-----------|
| PH8-01 | Implement theme switching logic (Services/Theming/)   | Not Started |         |           |
| PH8-02 | Implement branding loader                             | Not Started |         |           |
| PH8-03 | Implement custom colour palette support               | Not Started |         |           |
| PH8-04 | Implement high-contrast mode                          | Not Started |         |           |
| PH8-05 | Implement dark mode refinements                       | Not Started |         |           |
| PH8-06 | Phase 8 verification pass                             | Not Started |         |           |


# 10. Phase 9 — Reusability Framework
| TaskID | Description                                           | Status      | Started | Completed |
|--------|-------------------------------------------------------|-------------|---------|-----------|
| PH9-01 | Build component registry                              | Not Started |         |           |
| PH9-02 | Build reusable form controls library                  | Not Started |         |           |
| PH9-03 | Build layout primitives (grid, stack, spacer, divider)| Not Started |         |           |
| PH9-04 | Build reusable data service wrappers                  | Not Started |         |           |
| PH9-05 | Build shared validation rules library                 | Not Started |         |           |
| PH9-06 | Phase 9 verification pass                             | Not Started |         |           |


# 11. Phase 10 — Integration & QA
| TaskID  | Description                                           | Status      | Started | Completed |
|---------|-------------------------------------------------------|-------------|---------|-----------|
| PH10-01| Full regression test (all 43 V1 pages + all V2 pages) | Not Started |         |           |
| PH10-02| Full accessibility audit (WCAG 2.1 AA)                | Not Started |         |           |
| PH10-03| Full performance audit                                | Not Started |         |           |
| PH10-04| Full database and migration validation                | Not Started |         |           |
| PH10-05| Finalise all documentation                            | Not Started |         |           |
| PH10-06| Prepare release package                               | Not Started |         |           |
| PH10-07| Final sign-off                                        | Not Started |         |           |

# 12. DB Track — SQL Server to MySQL Migration
| TaskID | Description                                                                 | Status      | Started    | Completed  |
|--------|-----------------------------------------------------------------------------|-------------|------------|------------|
| MIG-01 | SQL Server Express schema designed (13-phase plan)                          | Done        |            | 2026-04-16 |
| MIG-02 | SQL Server schema built (all 13 phases)                                     | Done        |            | 2026-04-16 |
| MIG-03 | EF Core initial migration on SQL Server (InitialCreate)                     | Done        |            | 2026-03-13 |
| MIG-04 | Decision to migrate from SQL Server to MariaDB 10.4.32                      | Done        |            | 2026-04-16 |
| MIG-05 | Pomelo.EntityFrameworkCore.MySql 8.0.2 installed                            | Done        |            | 2026-04-16 |
| MIG-06 | EF Core bumped from 8.0.0 to 8.0.2                                          | Done        |            | 2026-04-16 |
| MIG-07 | DataServiceRegistration.cs — UseMySql with MariaDbServerVersion(10,4,32)    | Done        |            | 2026-04-16 |
| MIG-08 | AppDbContextFactory.cs — updated to UseMySql                                | Done        |            | 2026-04-16 |
| MIG-09 | EfHost/Program.cs — updated to MySQL connection string                      | Done        |            | 2026-04-16 |
| MIG-10 | Api/appsettings.json — updated to MariaDB connection string                 | Done        |            | 2026-04-16 |
| MIG-11 | Client/appsettings.json — updated to MariaDB connection string              | Done        |            | 2026-04-16 |
| MIG-12 | SQL Server migrations deleted                                               | Done        |            | 2026-04-16 |
| MIG-13 | InitialMySQL migration generated (20260416103036_InitialMySQL.cs)           | Done        |            | 2026-04-16 |
| MIG-14 | InitialMySQL migration Up() emptied (schema already exists in DB)           | Done        |            | 2026-04-16 |
| MIG-15 | __EFMigrationsHistory table created in MySQL                                | Done        |            | 2026-04-16 |
| MIG-16 | InitialMySQL row inserted into __EFMigrationsHistory                        | Done        |            | 2026-04-16 |
| MIG-17 | MenuResolver.cs updated to MySqlConnector                                   | Done        |            | 2026-04-16 |
| MIG-18 | FeaturePermissionResolver.cs updated to MySqlConnector                      | Done        |            | 2026-04-16 |
| MIG-19 | Full solution build — 0 errors, 0 warnings verified                         | Done        |            | 2026-04-16 |
| MIG-20 | rolemenuitem table created during MySQL migration                           | Done        |            | 2026-04-16 |
| MIG-21 | Create MySQL equivalents of fn_GetVisibleMenuItems and fn_IsFeatureAllowed  | Not Started |            |            |


# 13. DB Track — MySQL Table Population

## Group 1 — Absence Lookup Tables
| TaskID | Description                                            | Table             | Status      | Started | Completed |
|--------|--------------------------------------------------------|-------------------|-------------|---------|-----------|
| POP-01 | Seed absence categories (Authorised, Unauthorised)     | absencecategories | Not Started |         |           |
| POP-02 | Seed absence methods (Phone, In Person, Email)         | absencemethods    | Not Started |         |           |
| POP-03 | Seed absence reasons (Illness, Holiday, Medical)       | absencereasons    | Not Started |         |           |
| POP-04 | Seed absence sources (Parent, School, Self)            | absencesources    | Not Started |         |           |
| POP-05 | Seed absence statuses (Open, Closed, Pending)          | absencestatuses   | Not Started |         |           |
| POP-06 | Seed absence types (Sick, Annual Leave, TOIL)          | absencetypes      | Not Started |         |           |


## Group 2 — Organisation Lookups
| TaskID | Description                                            | Table             | Status      | Started | Completed |
|--------|--------------------------------------------------------|-------------------|-------------|---------|-----------|
| POP-07 | Seed device types (Laptop, Tablet, Desktop)            | devicetypes       | Not Started |         |           |
| POP-08 | Seed external systems (Google, Microsoft)              | externalsystems   | Not Started |         |           |
| POP-09 | Seed school houses                                     | houses            | Not Started |         |           |
| POP-10 | Seed school phases (EYFS, KS1, KS2, KS3, KS4)          | phases            | Not Started |         |           |
| POP-11 | Seed staff responsibilities (Head of Year, SENCO)      | responsibilities  | Not Started |         |           |
| POP-12 | Seed staff departments                                 | staffdepartments  | Not Started |         |           |
| POP-13 | Seed staff job groups                                  | staffjobgroups    | Not Started |         |           |
| POP-14 | Seed staff job titles                                  | staffjobtitles    | Not Started |         |           |
| POP-15 | Seed school sites / campuses                           | staffschools      | Not Started |         |           |
| POP-16 | Seed year groups (EYFS, Y1–Y13)                        | yeargroups        | Not Started |         |           |


## Group 3 — Permissions & Menu
| TaskID | Description                                            | Table             | Status      | Started | Completed |
|--------|--------------------------------------------------------|-------------------|-------------|---------|-----------|
| POP-07 | Seed device types (Laptop, Tablet, Desktop)            | devicetypes       | Not Started |         |           |
| POP-08 | Seed external systems (Google, Microsoft)              | externalsystems   | Not Started |         |           |
| POP-09 | Seed school houses                                     | houses            | Not Started |         |           |
| POP-10 | Seed school phases (EYFS, KS1, KS2, KS3, KS4)          | phases            | Not Started |         |           |
| POP-11 | Seed staff responsibilities (Head of Year, SENCO)      | responsibilities  | Not Started |         |           |
| POP-12 | Seed staff departments                                 | staffdepartments  | Not Started |         |           |
| POP-13 | Seed staff job groups                                  | staffjobgroups    | Not Started |         |           |
| POP-14 | Seed staff job titles                                  | staffjobtitles    | Not Started |         |           |
| POP-15 | Seed school sites / campuses                           | staffschools      | Not Started |         |           |
| POP-16 | Seed year groups (EYFS, Y1–Y13)                        | yeargroups        | Not Started |         |           |


## Group 4 — Users & Auth
| TaskID | Description                                            | Table             | Status      | Started | Completed |
|--------|--------------------------------------------------------|-------------------|-------------|---------|-----------|
| POP-07 | Seed device types (Laptop, Tablet, Desktop)            | devicetypes       | Not Started |         |           |
| POP-08 | Seed external systems (Google, Microsoft)              | externalsystems   | Not Started |         |           |
| POP-09 | Seed school houses                                     | houses            | Not Started |         |           |
| POP-10 | Seed school phases (EYFS, KS1, KS2, KS3, KS4)          | phases            | Not Started |         |           |
| POP-11 | Seed staff responsibilities (Head of Year, SENCO)      | responsibilities  | Not Started |         |           |
| POP-12 | Seed staff departments                                 | staffdepartments  | Not Started |         |           |
| POP-13 | Seed staff job groups                                  | staffjobgroups    | Not Started |         |           |
| POP-14 | Seed staff job titles                                  | staffjobtitles    | Not Started |         |           |
| POP-15 | Seed school sites / campuses                           | staffschools      | Not Started |         |           |
| POP-16 | Seed year groups (EYFS, Y1–Y13)                        | yeargroups        | Not Started |         |           |


## Group 5 — Staff Domain
| TaskID | Description                                            | Table                 | Status      | Started    | Completed  |
|--------|--------------------------------------------------------|-----------------------|-------------|------------|------------|
| POP-31 | Create at least 3 test staff records                   | staff                 | Done        | 2026-04-21 | 2026-04-21 |
| POP-32 | Create test staff absence records                      | staffabsences         | Done        | 2026-04-21 | 2026-04-21 |
| POP-33 | Create test staff assignment records                   | staffassignments      | Done        | 2026-04-21 | 2026-04-21 |
| POP-34 | Create test staff device records                       | staffdevices          | Done        | 2026-04-21 | 2026-04-21 |
| POP-35 | Create test staff external account records             | staffexternalaccounts | Done        | 2026-04-21 | 2026-04-21 |
| POP-36 | Create test staff location records                     | stafflocations        | Done        | 2026-04-21 | 2026-04-21 |
| POP-37 | Assign responsibilities to test staff                  | staffresponsibilities | Done        | 2026-04-21 | 2026-04-21 |


## Group 6 — Academic & Students
| TaskID | Description                                            | Table                 | Status      | Started    | Completed  |
|--------|--------------------------------------------------------|-----------------------|-------------|------------|------------|
| POP-31 | Create at least 3 test staff records                   | staff                 | Done        | 2026-04-21 | 2026-04-21 |
| POP-32 | Create test staff absence records                      | staffabsences         | Done        | 2026-04-21 | 2026-04-21 |
| POP-33 | Create test staff assignment records                   | staffassignments      | Done        | 2026-04-21 | 2026-04-21 |
| POP-34 | Create test staff device records                       | staffdevices          | Done        | 2026-04-21 | 2026-04-21 |
| POP-35 | Create test staff external account records             | staffexternalaccounts | Done        | 2026-04-21 | 2026-04-21 |
| POP-36 | Create test staff location records                     | stafflocations        | Done        | 2026-04-21 | 2026-04-21 |
| POP-37 | Assign responsibilities to test staff                  | staffresponsibilities | Done        | 2026-04-21 | 2026-04-21 |


## Group 7 — Attendance
| TaskID | Description                                            | Table                 | Status      | Started    | Completed  |
|--------|--------------------------------------------------------|-----------------------|-------------|------------|------------|
| POP-46 | Create test attendance registers                       | attendanceregisters   | Not Started |            |            |
| POP-47 | Create test attendance marks                           | attendancemarks       | Not Started |            |            |
| POP-48 | Create test attendance records                         | attendance            | Done        | 2026-04-21 | 2026-04-21 |

## Group 8 — Notifications & Messages
| TaskID | Description                                            | Table                 | Status      | Started    | Completed  |
|--------|--------------------------------------------------------|-----------------------|-------------|------------|------------|
| POP-49 | Create test app notification records                   | appnotifications      | Done        | 2026-04-21 | 2026-04-21 |
| POP-50 | Create test message records                            | messages              | Done        | 2026-04-21 | 2026-04-21 |

  Note: Audit tables (auditlog, loginaudit, staffabsenceaudit, etc.) are auto-populated
  by the application and require no manual seeding.


# 14. DB Track — MySQL End-to-End Validation
| TaskID | Description                                           | Status      | Started | Completed |
|--------|-------------------------------------------------------|-------------|---------|-----------|
| VAL-01 | App launches and connects to MySQL without error      | Not Started |         |           |
| VAL-02 | Login screen authenticates a test user                | Not Started |         |           |
| VAL-03 | Sidebar loads correct menu items for Admin role       | Not Started |         |           |
| VAL-04 | Sidebar loads correct menu items for Teacher role     | Not Started |         |           |
| VAL-05 | Role-based menu filtering works (MIG-21 prereq.)      | Not Started |         |           |
| VAL-06 | Feature permission checks work (MIG-21 prereq.)       | Not Started |         |           |
| VAL-07 | Students list page shows test student records         | Not Started |         |           |
| VAL-08 | Staff list page shows test staff records              | Not Started |         |           |
| VAL-09 | Absence recording creates a row in studentabsences    | Not Started |         |           |
| VAL-10 | Audit log row created for a tracked user action       | Not Started |         |           |
| VAL-11 | End-to-end test with real data — signed off           | Not Started |         |           |


# 15. Outstanding Tasks
| TaskID | Description                                           | Status      | Started | Completed |
|--------|-------------------------------------------------------|-------------|---------|-----------|
| MIG-21 | Create MySQL fn_GetVisibleMenuItems equivalent        | Not Started |         |           |
| MIG-21 | Create MySQL fn_IsFeatureAllowed equivalent           | Not Started |         |           |
| POP-19 | Seed roles table                                      | Not Started |         |           |
| POP-21 | Seed role–feature mappings                            | Not Started |         |           |
| POP-22 | Seed sidebar menu items                               | Not Started |         |           |
| POP-23 | Seed menu global config                               | Not Started |         |           |
| POP-24 | Seed global config switches                           | Not Started |         |           |
| POP-25 | Seed user feature overrides                           | Not Started |         |           |
| POP-26 | Create admin user account                             | Not Started |         |           |
| POP-38 | Create test classes + assign year groups              | Not Started |         |           |
| POP-40 | Create test student records                           | Not Started |         |           |
| VAL-01 | Full end-to-end validation (VAL-01 to VAL-11)         | Not Started |         |           |
| PH2+   | All of Phase 2 through Phase 10 not yet started       | Not Started |         |           |

# 16. E15 — User Management Module
| TaskID | Description                                                                   | Status      | Started    | Completed  |
|--------|-------------------------------------------------------------------------------|-------------|------------|------------|
| E15-01 | Add StaffId (long?) FK to User entity                                         | Done        | 2026-04-21 | 2026-04-21 | 
| E15-02 | Update UserManagement DTOs — remove name/phone, add StaffId                   | Done        | 2026-04-21 | 2026-04-21 |
| E15-03 | Update IUserManagementService — add Staff lookup methods                      | Done        | 2026-04-21 | 2026-04-21 | 
| E15-04 | Rewrite UserManagementService — Staff-linked CRUD enforcement                 | Done        | 2026-04-21 | 2026-04-21 |
| E15-05 | Update UserManagementApiServiceV2 — add Staff lookup wrappers                 | Done        | 2026-04-21 | 2026-04-21 | 
| E15-06 | Update UserFormViewModelV2 — StaffId, LinkedStaff, SetError                   | Done        | 2026-04-21 | 2026-04-21 |
| E15-07 | Update UserListViewModelV2 — AllItems, staffName column                       | Done        | 2026-04-21 | 2026-04-21 | 
| E15-08 | Rewrite UserFormPageV2.razor — routes, fields, remove stale blocks            | Done        | 2026-04-21 | 2026-04-21 |
| E15-09 | Update UsersListPageV2.razor — remove Add User button, phone to staffName     | Done        | 2026-04-21 | 2026-04-21 | 
| E15-10 | Update StaffDetailPageV2.razor — add Create User Account button               | Done        | 2026-04-21 | 2026-04-21 |
| E15-11 | Remove standalone and email-based user creation                               | Done        | 2026-04-21 | 2026-04-21 | 
| E15-12 | Enforce mandatory Staff-linked user creation via StaffId FK                   | Done        | 2026-04-21 | 2026-04-21 |
| E15-13 | Prevent duplicate user accounts per Staff record                              | Done        | 2026-04-21 | 2026-04-21 | 
| E15-14 | Remove FirstName, LastName, PhoneNumber from DTOs and UI                      | Done        | 2026-04-21 | 2026-04-21 |
| E15-15 | Remove stale duplicate @code block from UserFormPageV2.razor                  | Done        | 2026-04-21 | 2026-04-21 | 
| E15-16 | Fix all build errors from entity and DTO changes                              | Done        | 2026-04-21 | 2026-04-21 |
| E15-17 | Final verification build — 0 CS errors confirmed                              | Done        | 2026-04-21 | 2026-04-21 |


# 17. E15-BF — User Management Bug Fixes & Page Wiring
| TaskID | Description                                                                   | Status      | Started    | Completed  |
|--------|-------------------------------------------------------------------------------|-------------|------------|------------|
| BF-01  | Fix UserListViewModelV2 — reset filter/search/sort/page state in LoadAsync()  | Done        | 2026-04-21 | 2026-04-21 | 
| BF-02  | Fix UserManagementService.GetUsersAsync — broken raw string literal SQL       | Done        | 2026-04-21 | 2026-04-21 |
|        | (role IN clause was emitted as literal text, not a SQL predicate)             |             |            |            |
| BF-03  | Add RoleListItemDto, FeatureListItemDto, PageAccessRowDto to DTOs             | Done        | 2026-04-21 | 2026-04-21 | 
| BF-04  | Add GetAllRoleTypesAsync to IUserManagementService and UserManagementService  | Done        | 2026-04-21 | 2026-04-21 |
| BF-05  | Add GetFeaturesAsync to IUserManagementService and UserManagementService      | Done        | 2026-04-21 | 2026-04-21 | 
| BF-06  | Add GetPageAccessAsync to IUserManagementService and UserManagementService    | Done        | 2026-04-21 | 2026-04-21 |
| BF-07  | Add GetAllRoleTypesAsync wrapper to UserManagementApiServiceV2                | Done        | 2026-04-21 | 2026-04-21 | 
| BF-08  | Add GetFeaturesAsync wrapper to UserManagementApiServiceV2                    | Done        | 2026-04-21 | 2026-04-21 |
| BF-09  | Add GetPageAccessAsync wrapper to UserManagementApiServiceV2                  | Done        | 2026-04-21 | 2026-04-21 | 
| BF-10  | Wire RolesPage.razor to live DB via GetAllRoleTypesAsync                      | Done        | 2026-04-21 | 2026-04-21 |
| BF-11  | Wire PermissionsPage.razor to live DB via GetFeaturesAsync                    | Done        | 2026-04-21 | 2026-04-21 | 
| BF-12  | Wire PageAccessPage.razor to live DB via GetPageAccessAsync                   | Done        | 2026-04-21 | 2026-04-21 |
| BF-13  | Update headers on all 8 modified files (version, date, changes)               | Done        | 2026-04-21 | 2026-04-21 | 
| BF-14  | Final verification build — 0 CS errors confirmed                              | Done        | 2026-04-21 | 2026-04-21 |

