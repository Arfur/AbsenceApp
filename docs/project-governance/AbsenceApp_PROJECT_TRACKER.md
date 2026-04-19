================================================================================
 AbsenceApp_PROJECT_TRACKER.md
 All Phases — Fully Populated — Current Status Accurate
================================================================================
 Status values : Not Started | In Progress | Done
 Updated       : 2026-04-18
================================================================================

# 1. Project Overview
+--------+----------------------------------------------------------+-------------+---------+-----------+
| Phase  | Description                                              | Status      | Started | Completed |
+--------+----------------------------------------------------------+-------------+---------+-----------+
| Ph 1   | Foundations & Documentation                              | Done        | —       | 2026-04-16|
| DB     | SQL Server to MySQL Migration                            | Done*       | —       | 2026-04-16|
| DB-POP | MySQL Table Population                                   | In Progress | —       | —         |
| DB-VAL | MySQL End-to-End Validation                              | Not Started | —       | —         |
| Ph 2   | CSS Tokens + JSON Config + Core Components               | Not Started | —       | —         |
| Ph 3   | Layout V2 (Sidebar, Header, Breadcrumb, ScrollSpy)       | Not Started | —       | —         |
| Ph 4   | Table System V2                                          | Not Started | —       | —         |
| Ph 5   | Page Templates V2                                        | Not Started | —       | —         |
| Ph 6   | Data & Services V2                                       | Not Started | —       | —         |
| Ph 7   | Feature Modules V2                                       | Not Started | —       | —         |
| Ph 8   | Theming & Branding V2                                    | Not Started | —       | —         |
| Ph 9   | Reusability Framework                                    | Not Started | —       | —         |
| Ph 10  | Integration & QA                                         | Not Started | —       | —         |
+--------+----------------------------------------------------------+-------------+---------+-----------+
  * MIG-21 (MySQL function equivalents) is outstanding in the DB track.

================================================================================

# 2. Phase 1 — Foundations & Documentation
+--------+-------------------------------------------------------------------------------+-------------+------------+------------+
| TaskID | Description                                                                   | Status      | Started    | Completed  |
+--------+-------------------------------------------------------------------------------+-------------+------------+------------+
| PH1-01 | Generate Architecture.md                                                      | Done        | 2026-04-16 | 2026-04-16 |
| PH1-02 | Generate CodingStandards.md                                                   | Done        | 2026-04-16 | 2026-04-16 |
| PH1-03 | Generate DataModel.md                                                         | Done        | 2026-04-16 | 2026-04-16 |
| PH1-04 | Generate DTOsAndMappers.md                                                    | Done        | 2026-04-16 | 2026-04-16 |
| PH1-05 | Generate Repositories.md                                                      | Done        | 2026-04-16 | 2026-04-16 |
| PH1-06 | Generate Services.md                                                          | Done        | 2026-04-16 | 2026-04-16 |
| PH1-07 | Generate DeveloperSetup.md                                                    | Done        | 2026-04-16 | 2026-04-16 |
| PH1-08 | Generate MigrationHistory.md                                                  | Done        | 2026-04-16 | 2026-04-16 |
| PH1-09 | Generate gap-analysis-phase1.txt                                              | Done        | 2026-04-16 | 2026-04-16 |
| PH1-10 | Generate structure-phase1.txt (V1 folder snapshot)                            | Done        | 2026-04-16 | 2026-04-16 |
| PH1-11 | Generate baseline-architecture-phase1.txt                                     | Done        | 2026-04-16 | 2026-04-16 |
| PH1-12 | Generate AbsenceApp_PRD.md                                                    | Done        | 2026-04-16 | 2026-04-16 |
| PH1-13 | Generate AbsenceApp_DECISIONS.md                                              | Done        | 2026-04-16 | 2026-04-16 |
| PH1-14 | Generate AbsenceApp_PROGRESS.md                                               | Done        | 2026-04-16 | 2026-04-16 |
| PH1-15 | Generate AbsenceApp_PROJECT_TRACKER.md                                        | Done        | 2026-04-16 | 2026-04-16 |
| PH1-16 | Set up initial codebase and folder structure                                  | Done        | 2026-04-16 | 2026-04-16 |
| PH1-17 | Complete gap analysis and baseline architecture                               | Done        | 2026-04-16 | 2026-04-16 |
| PH1-18 | Verify solution builds with 0 errors                                          | Done        | 2026-04-16 | 2026-04-16 |
| PH1-19 | Verify Roslyn analyzer AA0001 active                                          | Done        | 2026-04-16 | 2026-04-16 |
| PH1-20 | Verify Git pre-commit hook wired (.githooks/)                                 | Done        | 2026-04-16 | 2026-04-16 |
| PH1-21 | Verify EF Core connected to MariaDB                                           | Done        | 2026-04-16 | 2026-04-16 |
| PH1-22 | Verify __EFMigrationsHistory registered                                       | Done        | 2026-04-16 | 2026-04-16 |
| PH1-23 | Verify 43 V1 pages present and functional                                     | Done        | 2026-04-16 | 2026-04-16 |
| PH1-24 | Verify sidebar, header, breadcrumb working (V1)                               | Done        | 2026-04-16 | 2026-04-16 |
| PH1-25 | Verify Light/Dark theme toggle working                                        | Done        | 2026-04-16 | 2026-04-16 |
| PH1-26 | Verify window state persistence working                                       | Done        | 2026-04-16 | 2026-04-16 |
| DBP-01 | Import full V2 MenuItems dataset into MySQL                                   | Done        | —          | 2026-04-18 |
+--------+-------------------------------------------------------------------------------+-------------+------------+------------+

================================================================================

# 3. Phase 2 — CSS Tokens + JSON Config + Core Components
+--------+-------------------------------------------------------------------------------+-------------+---------+-----------+
| TaskID | Description                                                                   | Status      | Started | Completed |
+--------+-------------------------------------------------------------------------------+-------------+---------+-----------+
| PH2-01 | Create CSS token files (wwwroot/css/tokens/)                                  | Not Started | —       | —         |
| PH2-02 | Create theme.json (wwwroot/config/designsystem/)                              | Not Started | —       | —         |
| PH2-03 | Create menu.json                                                              | Not Started | —       | —         |
| PH2-04 | Create table-schema.json                                                      | Not Started | —       | —         |
| PH2-05 | Create components.json                                                        | Not Started | —       | —         |
| PH2-06 | Create icons.json                                                             | Not Started | —       | —         |
| PH2-07 | Create branding.json                                                          | Not Started | —       | —         |
| PH2-08 | Build Icon component (Components/DesignSystem/Icon.razor)                     | Not Started | —       | —         |
| PH2-09 | Build IconButton component                                                    | Not Started | —       | —         |
| PH2-10 | Build SectionHeader component                                                 | Not Started | —       | —         |
| PH2-11 | Build Card component                                                          | Not Started | —       | —         |
| PH2-12 | Implement DesignSystemConfigService                                           | Not Started | —       | —         |
| PH2-13 | Implement IconService                                                         | Not Started | —       | —         |
| PH2-14 | Verify all 43 V1 pages unaffected by Phase 2 changes                          | Not Started | —       | —         |
| PH2-15 | Phase 2 verification pass                                                     | Not Started | —       | —         |
+--------+-------------------------------------------------------------------------------+-------------+---------+-----------+

================================================================================

# 4. Phase 3 — Layout V2 (Sidebar, Header, Breadcrumb, ScrollSpy)
+--------+-------------------------------------------------------------------------------+-------------+---------+-----------+
| TaskID | Description                                                                   | Status      | Started | Completed |
+--------+-------------------------------------------------------------------------------+-------------+---------+-----------+
| PH3-01 | Implement GlobalErrorBoundaryV2                                               | Not Started | —       | —         |
| PH3-02 | Implement ScrollSpy service                                                   | Not Started | —       | —         |
| PH3-03 | Implement LayoutV2 container (Components/LayoutV2/)                           | Not Started | —       | —         |
| PH3-04 | Implement HeaderV2                                                            | Not Started | —       | —         |
| PH3-05 | Implement SidebarV2                                                           | Not Started | —       | —         |
| PH3-06 | Implement BreadcrumbV2                                                        | Not Started | —       | —         |
| PH3-07 | Implement theme persistence (localStorage or config file)                     | Not Started | —       | —         |
| PH3-08 | Re-run all 40 V1 shell tests after LayoutV2 integration                       | Not Started | —       | —         |
| PH3-09 | Phase 3 verification pass                                                     | Not Started | —       | —         |
+--------+-------------------------------------------------------------------------------+-------------+---------+-----------+

================================================================================

# 5. Phase 4 — Table System V2
+--------+-------------------------------------------------------------------------------+-------------+---------+-----------+
| TaskID | Description                                                                   | Status      | Started | Completed |
+--------+-------------------------------------------------------------------------------+-------------+---------+-----------+
| PH4-01 | Build TableV2 component (Components/TableV2/)                                 | Not Started | —       | —         |
| PH4-02 | Build column schema loader                                                    | Not Started | —       | —         |
| PH4-03 | Build pagination module                                                       | Not Started | —       | —         |
| PH4-04 | Build sorting module                                                          | Not Started | —       | —         |
| PH4-05 | Build filtering module (filter row under header)                              | Not Started | —       | —         |
| PH4-06 | Build row selection module (click-to-select)                                  | Not Started | —       | —         |
| PH4-07 | Build row actions module (inline icon-only buttons)                           | Not Started | —       | —         |
| PH4-08 | Build empty state module ("No records found")                                 | Not Started | —       | —         |
| PH4-09 | Build skeleton row loading state                                              | Not Started | —       | —         |
| PH4-10 | Verify table on a sample dataset                                              | Not Started | —       | —         |
| PH4-11 | Phase 4 verification pass                                                     | Not Started | —       | —         |
+--------+-------------------------------------------------------------------------------+-------------+---------+-----------+

================================================================================

# 6. Phase 5 — Page Templates V2
+--------+-------------------------------------------------------------------------------+-------------+---------+-----------+
| TaskID | Description                                                                   | Status      | Started | Completed |
+--------+-------------------------------------------------------------------------------+-------------+---------+-----------+
| PH5-01 | Build ListPage template (Components/PageTemplatesV2/)                         | Not Started | —       | —         |
| PH5-02 | Build DetailPage template                                                     | Not Started | —       | —         |
| PH5-03 | Build EditPage template                                                       | Not Started | —       | —         |
| PH5-04 | Build CreatePage template                                                     | Not Started | —       | —         |
| PH5-05 | Build DashboardPage template                                                  | Not Started | —       | —         |
| PH5-06 | Integrate TableV2 into list and dashboard templates                           | Not Started | —       | —         |
| PH5-07 | Phase 5 verification pass                                                     | Not Started | —       | —         |
+--------+-------------------------------------------------------------------------------+-------------+---------+-----------+

================================================================================

# 7. Phase 6 — Data & Services V2
+--------+-------------------------------------------------------------------------------+-------------+---------+-----------+
| TaskID | Description                                                                   | Status      | Started | Completed |
+--------+-------------------------------------------------------------------------------+-------------+---------+-----------+
| PH6-01 | Implement new DataService pattern (Services/DataV2/)                          | Not Started | —       | —         |
| PH6-02 | Implement caching layer                                                       | Not Started | —       | —         |
| PH6-03 | Implement validation layer                                                    | Not Started | —       | —         |
| PH6-04 | Implement error handling layer                                                | Not Started | —       | —         |
| PH6-05 | Implement logging layer                                                       | Not Started | —       | —         |
| PH6-06 | Implement API abstraction layer                                               | Not Started | —       | —         |
| PH6-07 | Phase 6 verification pass                                                     | Not Started | —       | —         |
+--------+-------------------------------------------------------------------------------+-------------+---------+-----------+

================================================================================

# 8. Phase 7 — Feature Modules V2
+--------+-------------------------------------------------------------------------------+-------------+---------+-----------+
| TaskID | Description                                                                   | Status      | Started | Completed |
+--------+-------------------------------------------------------------------------------+-------------+---------+-----------+
| PH7-01 | Build Student Management V2 (ModulesV2/)                                      | Not Started | —       | —         |
| PH7-02 | Build Staff Management V2                                                     | Not Started | —       | —         |
| PH7-03 | Build Student Absence Recording V2                                            | Not Started | —       | —         |
| PH7-04 | Build Staff Absence Recording V2                                              | Not Started | —       | —         |
| PH7-05 | Build persistent absence alert system                                         | Not Started | —       | —         |
| PH7-06 | Build email reminder system (teacher notifications)                           | Not Started | —       | —         |
| PH7-07 | Build CSV import from MIS                                                     | Not Started | —       | —         |
| PH7-08 | Build CSV export                                                              | Not Started | —       | —         |
| PH7-09 | Build PDF export                                                              | Not Started | —       | —         |
| PH7-10 | Phase 7 verification pass                                                     | Not Started | —       | —         |
+--------+-------------------------------------------------------------------------------+-------------+---------+-----------+

================================================================================

# 9. Phase 8 — Theming & Branding V2
+--------+-------------------------------------------------------------------------------+-------------+---------+-----------+
| TaskID | Description                                                                   | Status      | Started | Completed |
+--------+-------------------------------------------------------------------------------+-------------+---------+-----------+
| PH8-01 | Implement theme switching logic (Services/Theming/)                           | Not Started | —       | —         |
| PH8-02 | Implement branding loader                                                     | Not Started | —       | —         |
| PH8-03 | Implement custom colour palette support                                       | Not Started | —       | —         |
| PH8-04 | Implement high-contrast mode                                                  | Not Started | —       | —         |
| PH8-05 | Implement dark mode refinements                                               | Not Started | —       | —         |
| PH8-06 | Phase 8 verification pass                                                     | Not Started | —       | —         |
+--------+-------------------------------------------------------------------------------+-------------+---------+-----------+

================================================================================

# 10. Phase 9 — Reusability Framework
+--------+-------------------------------------------------------------------------------+-------------+---------+-----------+
| TaskID | Description                                                                   | Status      | Started | Completed |
+--------+-------------------------------------------------------------------------------+-------------+---------+-----------+
| PH9-01 | Build component registry                                                      | Not Started | —       | —         |
| PH9-02 | Build reusable form controls library                                          | Not Started | —       | —         |
| PH9-03 | Build layout primitives (grid, stack, spacer, divider)                        | Not Started | —       | —         |
| PH9-04 | Build reusable data service wrappers                                          | Not Started | —       | —         |
| PH9-05 | Build shared validation rules library                                         | Not Started | —       | —         |
| PH9-06 | Phase 9 verification pass                                                     | Not Started | —       | —         |
+--------+-------------------------------------------------------------------------------+-------------+---------+-----------+

================================================================================

# 11. Phase 10 — Integration & QA
+--------+-------------------------------------------------------------------------------+-------------+---------+-----------+
| TaskID | Description                                                                   | Status      | Started | Completed |
+--------+-------------------------------------------------------------------------------+-------------+---------+-----------+
| PH10-01| Full regression test (all 43 V1 pages + all V2 pages)                         | Not Started | —       | —         |
| PH10-02| Full accessibility audit (WCAG 2.1 AA)                                        | Not Started | —       | —         |
| PH10-03| Full performance audit                                                        | Not Started | —       | —         |
| PH10-04| Full database and migration validation                                        | Not Started | —       | —         |
| PH10-05| Finalise all documentation                                                    | Not Started | —       | —         |
| PH10-06| Prepare release package                                                       | Not Started | —       | —         |
| PH10-07| Final sign-off                                                                | Not Started | —       | —         |
+--------+-------------------------------------------------------------------------------+-------------+---------+-----------+

================================================================================

# 12. DB Track — SQL Server to MySQL Migration
+--------+-------------------------------------------------------------------------------+-------------+------------+------------+
| TaskID | Description                                                                   | Status      | Started    | Completed  |
+--------+-------------------------------------------------------------------------------+-------------+------------+------------+
| MIG-01 | SQL Server Express schema designed (13-phase plan)                            | Done        | —          | Pre-Apr-16 |
| MIG-02 | SQL Server schema built (all 13 phases)                                       | Done        | —          | Pre-Apr-16 |
| MIG-03 | EF Core initial migration on SQL Server (InitialCreate)                       | Done        | —          | 2026-03-13 |
| MIG-04 | Decision to migrate from SQL Server to MariaDB 10.4.32                        | Done        | —          | 2026-04-16 |
| MIG-05 | Pomelo.EntityFrameworkCore.MySql 8.0.2 installed                              | Done        | —          | 2026-04-16 |
| MIG-06 | EF Core bumped from 8.0.0 to 8.0.2                                            | Done        | —          | 2026-04-16 |
| MIG-07 | DataServiceRegistration.cs — UseMySql with MariaDbServerVersion(10,4,32)      | Done        | —          | 2026-04-16 |
| MIG-08 | AppDbContextFactory.cs — updated to UseMySql                                  | Done        | —          | 2026-04-16 |
| MIG-09 | EfHost/Program.cs — updated to MySQL connection string                        | Done        | —          | 2026-04-16 |
| MIG-10 | Api/appsettings.json — updated to MariaDB connection string                   | Done        | —          | 2026-04-16 |
| MIG-11 | Client/appsettings.json — updated to MariaDB connection string                | Done        | —          | 2026-04-16 |
| MIG-12 | SQL Server migrations deleted                                                 | Done        | —          | 2026-04-16 |
| MIG-13 | InitialMySQL migration generated (20260416103036_InitialMySQL.cs)             | Done        | —          | 2026-04-16 |
| MIG-14 | InitialMySQL migration Up() emptied (schema already exists in DB)             | Done        | —          | 2026-04-16 |
| MIG-15 | __EFMigrationsHistory table created in MySQL                                  | Done        | —          | 2026-04-16 |
| MIG-16 | InitialMySQL row inserted into __EFMigrationsHistory                          | Done        | —          | 2026-04-16 |
| MIG-17 | MenuResolver.cs updated to MySqlConnector                                     | Done        | —          | 2026-04-16 |
| MIG-18 | FeaturePermissionResolver.cs updated to MySqlConnector                        | Done        | —          | 2026-04-16 |
| MIG-19 | Full solution build — 0 errors, 0 warnings verified                           | Done        | —          | 2026-04-16 |
| MIG-20 | rolemenuitem table created during MySQL migration                             | Done        | —          | 2026-04-16 |
| MIG-21 | Create MySQL equivalents of fn_GetVisibleMenuItems and fn_IsFeatureAllowed    | Not Started | —          | —          |
+--------+-------------------------------------------------------------------------------+-------------+------------+------------+

# 13. DB Track — MySQL Table Population

## Group 1 — Absence Lookup Tables
+--------+--------------------------------------------------------+-------------------+-------------+---------+-----------+
| TaskID | Description                                            | Table             | Status      | Started | Completed |
+--------+--------------------------------------------------------+-------------------+-------------+---------+-----------+
| POP-01 | Seed absence categories (Authorised, Unauthorised)     | absencecategories | Not Started | —       | —         |
| POP-02 | Seed absence methods (Phone, In Person, Email)         | absencemethods    | Not Started | —       | —         |
| POP-03 | Seed absence reasons (Illness, Holiday, Medical)       | absencereasons    | Not Started | —       | —         |
| POP-04 | Seed absence sources (Parent, School, Self)            | absencesources    | Not Started | —       | —         |
| POP-05 | Seed absence statuses (Open, Closed, Pending)          | absencestatuses   | Not Started | —       | —         |
| POP-06 | Seed absence types (Sick, Annual Leave, TOIL)          | absencetypes      | Not Started | —       | —         |
+--------+--------------------------------------------------------+-------------------+-------------+---------+-----------+

## Group 2 — Organisation Lookups
+--------+--------------------------------------------------------+---------------------------+-------------+---------+-----------+
| TaskID | Description                                            | Table                     | Status      | Started | Completed |
+--------+--------------------------------------------------------+---------------------------+-------------+---------+-----------+
| POP-07 | Seed device types (Laptop, Tablet, Desktop)            | devicetypes               | Not Started | —       | —         |
| POP-08 | Seed external systems (Google, Microsoft)              | externalsystems           | Not Started | —       | —         |
| POP-09 | Seed school houses                                     | houses                    | Not Started | —       | —         |
| POP-10 | Seed school phases (EYFS, KS1, KS2, KS3, KS4)          | phases                    | Not Started | —       | —         |
| POP-11 | Seed staff responsibilities (Head of Year, SENCO)      | responsibilities          | Not Started | —       | —         |
| POP-12 | Seed staff departments                                 | staffdepartments          | Not Started | —       | —         |
| POP-13 | Seed staff job groups                                  | staffjobgroups            | Not Started | —       | —         |
| POP-14 | Seed staff job titles                                  | staffjobtitles            | Not Started | —       | —         |
| POP-15 | Seed school sites / campuses                           | staffschools              | Not Started | —       | —         |
| POP-16 | Seed year groups (EYFS, Y1–Y13)                        | yeargroups                | Not Started | —       | —         |
+--------+--------------------------------------------------------+---------------------------+-------------+---------+-----------+

## Group 3 — Permissions & Menu
+--------+--------------------------------------------------------+-----------------------+-------------+------------+------------+
| TaskID | Description                                            | Table                 | Status      | Started    | Completed  |
+--------+--------------------------------------------------------+-----------------------+-------------+------------+------------+
| POP-17 | Seed feature flags (6 records)                         | feature               | Done        | 2026-04-16 | 2026-04-16 |
| POP-18 | Seed role types (Admin, StaffAdmin, Teacher, etc.)     | roletypes             | Done        | 2026-04-16 | 2026-04-16 |
| POP-19 | Seed roles (SuperAdmin, Admin, Office, Teacher, etc.)  | roles                 | Done        | 2026-04-18 | 2026-04-18 |
| POP-20 | Seed role–menu-item mappings                           | rolemenuitem          | Done        | 2026-04-18 | 2026-04-18 |
| POP-21 | Seed role–feature mappings                             | rolefeature           | Done        | 2026-04-18 | 2026-04-18 |
| POP-22 | Seed sidebar menu items and structure                  | menuitems             | Done        | 2026-04-18 | 2026-04-18 |
| POP-23 | Seed menu global config (EnableRoleBasedNavigation)    | menuitemsglobalconfig | Done        | 2026-04-18 | 2026-04-18 |
| POP-24 | Seed global config switches (EnableGlobalSettings)     | globalconfig          | Not Started | —          | —          |
| POP-25 | Seed user feature overrides (empty for baseline)       | userfeatureoverride   | Done        | 2026-04-18 | 2026-04-18 |
+--------+--------------------------------------------------------+-----------------------+-------------+------------+------------+

## Group 4 — Users & Auth
+--------+--------------------------------------------------------+-----------------------+-------------+------------+------------+
| TaskID | Description                                            | Table                 | Status      | Started    | Completed  |
+--------+--------------------------------------------------------+-----------------------+-------------+------------+------------+
| POP-26 | Create admin user account                              | users                 | Not Started | —          | —          |
| POP-27 | Create test office staff user account                  | users                 | Not Started | —          | —          |
| POP-28 | Create test teacher user account                       | users                 | Not Started | —          | —          |
| POP-29 | Create user profiles for all test users                | userprofiles          | Done        | 2026-04-19 | 2026-04-19 |
| POP-30 | Assign roles to test users via userrole                | userrole              | Not Started | —          | —          |
+--------+--------------------------------------------------------+-----------------------+-------------+------------+------------+

## Group 5 — Staff Domain
+--------+--------------------------------------------------------+-----------------------+-------------+------------+------------+
| TaskID | Description                                            | Table                 | Status      | Started    | Completed  |
+--------+--------------------------------------------------------+-----------------------+-------------+------------+------------+
| POP-31 | Create at least 3 test staff records                   | staff                 | Not Started | —          | —          |
| POP-32 | Create test staff absence records                      | staffabsences         | Not Started | —          | —          |
| POP-33 | Create test staff assignment records                   | staffassignments      | Not Started | —          | —          |
| POP-34 | Create test staff device records                       | staffdevices          | Not Started | —          | —          |
| POP-35 | Create test staff external account records             | staffexternalaccounts | Not Started | —          | —          |
| POP-36 | Create test staff location records                     | stafflocations        | Not Started | —          | —          |
| POP-37 | Assign responsibilities to test staff                  | staffresponsibilities | Not Started | —          | —          |
+--------+--------------------------------------------------------+-----------------------+-------------+------------+------------+

## Group 6 — Academic & Students
+--------+-----------------------------------------------------+--------------------------+-------------+------------+------------+
| TaskID | Description                                         | Table                    | Status      | Started    | Completed  |
+--------+-----------------------------------------------------+--------------------------+-------------+------------+------------+
| POP-38 | Create at least 3 test classes                      | classes                  | Not Started | —          | —          |
| POP-39 | Assign year groups to test classes                  | classyeargroupassignments| Not Started | —          | —          |
| POP-40 | Create at least 5 test student records              | students                 | Not Started | —          | —          |
| POP-41 | Assign students to classes                          | classmember              | Not Started | —          | —          |
| POP-42 | Create student contact records (parent/guardian)    | studentcontacts          | Not Started | —          | —          |
| POP-43 | Create student flag records (SEN, medical, pastoral)| studentflags             | Not Started | —          | —          |
| POP-44 | Create student medical records                      | studentmedical           | Not Started | —          | —          |
| POP-45 | Create test student absence records                 | studentabsences          | Not Started | —          | —          |
+--------+-----------------------------------------------------+--------------------------+-------------+------------+------------+

## Group 7 — Attendance
+--------+--------------------------------------------------------+-----------------------+-------------+------------+------------+
| TaskID | Description                                            | Table                 | Status      | Started    | Completed  |
+--------+--------------------------------------------------------+-----------------------+-------------+------------+------------+
| POP-46 | Create test attendance registers                       | attendanceregisters   | Not Started | —          | —          |
| POP-47 | Create test attendance marks                           | attendancemarks       | Not Started | —          | —          |
| POP-48 | Create test attendance records                         | attendance            | Not Started | —          | —          |
+--------+--------------------------------------------------------+-----------------------+-------------+------------+------------+

## Group 8 — Notifications & Messages
+--------+--------------------------------------------------------+-----------------------+-------------+------------+------------+
| TaskID | Description                                            | Table                 | Status      | Started    | Completed  |
+--------+--------------------------------------------------------+-----------------------+-------------+------------+------------+
| POP-49 | Create test app notification records                   | appnotifications      | Not Started | —          | —          |
| POP-50 | Create test message records                            | messages              | Not Started | —          | —          |
+--------+--------------------------------------------------------+-----------------------+-------------+------------+------------+
  Note: Audit tables (auditlog, loginaudit, staffabsenceaudit, etc.) are auto-populated
  by the application and require no manual seeding.


# 14. DB Track — MySQL End-to-End Validation
+--------+-------------------------------------------------------+-------------+---------+-----------+
| TaskID | Description                                           | Status      | Started | Completed |
+--------+-------------------------------------------------------+-------------+---------+-----------+
| VAL-01 | App launches and connects to MySQL without error      | Not Started | —       | —         |
| VAL-02 | Login screen authenticates a test user                | Not Started | —       | —         |
| VAL-03 | Sidebar loads correct menu items for Admin role       | Not Started | —       | —         |
| VAL-04 | Sidebar loads correct menu items for Teacher role     | Not Started | —       | —         |
| VAL-05 | Role-based menu filtering works (MIG-21 prereq.)      | Not Started | —       | —         |
| VAL-06 | Feature permission checks work (MIG-21 prereq.)       | Not Started | —       | —         |
| VAL-07 | Students list page shows test student records         | Not Started | —       | —         |
| VAL-08 | Staff list page shows test staff records              | Not Started | —       | —         |
| VAL-09 | Absence recording creates a row in studentabsences    | Not Started | —       | —         |
| VAL-10 | Audit log row created for a tracked user action       | Not Started | —       | —         |
| VAL-11 | End-to-end test with real data — signed off           | Not Started | —       | —         |
+--------+-------------------------------------------------------+-------------+---------+-----------+

# 15. Outstanding Tasks
+--------+-------------------------------------------------------+-------------+---------+-----------+
| TaskID | Description                                           | Status      | Started | Completed |
+--------+-------------------------------------------------------+-------------+---------+-----------+
| MIG-21 | Create MySQL fn_GetVisibleMenuItems equivalent        | Not Started | —       | —         |
| MIG-21 | Create MySQL fn_IsFeatureAllowed equivalent           | Not Started | —       | —         |
| POP-19 | Seed roles table                                      | Not Started | —       | —         |
| POP-21 | Seed role–feature mappings                            | Not Started | —       | —         |
| POP-22 | Seed sidebar menu items                               | Not Started | —       | —         |
| POP-23 | Seed menu global config                               | Not Started | —       | —         |
| POP-24 | Seed global config switches                           | Not Started | —       | —         |
| POP-25 | Seed user feature overrides                           | Not Started | —       | —         |
| POP-26 | Create admin user account                             | Not Started | —       | —         |
| POP-38 | Create test classes + assign year groups              | Not Started | —       | —         |
| POP-40 | Create test student records                           | Not Started | —       | —         |
| VAL-01 | Full end-to-end validation (VAL-01 to VAL-11)         | Not Started | —       | —         |
| PH2+   | All of Phase 2 through Phase 10 not yet started       | Not Started | —       | —         |
+--------+-------------------------------------------------------+-------------+---------+-----------+