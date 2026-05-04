---
doc_type: changelog_phase
project_name: "AbsenceApp"
module: "V2"
phase: "Phase 1 — Foundations & Documentation"
file_name: "AbsenceApp_CHANGELOG_Phase1.md"
schema_scope: "MySQL"
version: "1.0.0"
created_date: "2026-04-16"
updated_date: "2026-04-30"
authority: "Michael"
ai_contract:
  read_rules: "Agents may read this phase changelog for context, migration guidance, and audit purposes."
  write_rules: "Append-only. Agents may append new entries to this file while Phase 1 is Active. Agents MUST NOT edit or delete existing entries, change historical dates/versions, or alter entry structure. For master-level summaries, append a short line to AbsenceApp_CHANGE_LOG.md only."
template_marker: true
template_version: "2026-04-30"
template_location: "top-of-file"
template_instructions: "Copy the template block below for new entries; do not edit this template block."

---
================================================================================
 File        : AbsenceApp_CHANGELOG_Phase1.md
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-04-16
 Updated     : 2026-04-30
--------------------------------------------------------------------------------
 Purpose     :
   Phase 1 changelog for AbsenceApp V2. This file is the append-only,
   authoritative record of MySQL schema changes, data population (seed)
   operations, and migration notes for Phase 1 (MySQL migration and initial
   seed data). Use this file for detailed, deterministic entries that support
   migrations, audits, and rollback planning.
--------------------------------------------------------------------------------
 Notes       :
   - This file is append-only. Do not edit or remove existing entries.
   - Each entry must follow the project's changelog entry template.
   - When Phase 1 completes, add a short summary line to the master changelog
     and update the master Active Phase pointer.
================================================================================


## [YYYY-MM-DD] — Short summary title
**Type:** Schema | Data | Code | Docs | Migration | Hotfix  
**Scope:** (e.g., `feature`, `table:users`, `seed:roles`)  
**Summary:** One-line summary of the change.

### Details
A short, precise description of what changed and why. Include any important constraints or decisions.

### Affected Objects
- Tables: table1, table2
- Files: path/to/file.cs, path/to/migration.sql
- Services: service-name

### Migration Notes
- Steps to apply: 1) ..., 2) ...
- Rollback steps: 1) ..., 2) ...
- Idempotency: yes | no

### Verification
- Tests: unit/integration/manual steps to verify
- Environments applied: dev, staging, prod

### References
- Issue/PR: #123
- Related changelog entries: [2026-04-16] — Seeded Feature and RoleTypes Tables


## [2026-04-16] — Seeded Feature and RoleTypes Tables

**Author:** Michael  
**Type:** Data | Migration  
**Scope:** `permissions:seed`  
**Summary:** Inserted baseline `feature` and `roletypes` records to align MySQL with existing SQL Server permission model.

### Details
Inserted six baseline feature records (`ui.sidebar`, `ui.global-settings`, `student.view`, `student.create`, `student.edit`, `student.delete`) and seeded role types required for system operation. All feature records include Id, Code, DisplayName, Description, IsEnabled=1, and timestamps. RoleTypes seeded to mirror SQL Server model.

### Affected Tables and Components
- Tables: `feature`; `roletypes`
- Components: permission evaluation and role-based access checks

### Rollout Notes
- Safe to re-run (idempotent seed)
- Must be applied to all environments before permission-dependent seeds

### Verification
- Confirm 6 feature rows exist with expected codes and IsEnabled=1
- Confirm roletypes contain Super Admin, Admin, User
- Validate permission evaluation logic against seeded data

### References
- Seed scripts: feature and roletypes seed SQL
- Related entries: Role/Users/UserRole seeds (2026-04-16)

---

## [2026-04-16] — Seeded RoleTypes, Roles, Users, and UserRole Tables

**Author:** Michael  
**Type:** Data | Migration | Identity  
**Scope:** `identity:bootstrap`  
**Summary:** Populated core identity tables (roletypes, roles, users, userrole) to bootstrap authentication and authorization.

### Details
Inserted three RoleTypes (Super Admin, Admin, User), three Roles mapped to RoleTypes, three initial Users with bcrypt password hashes (cost 12), and UserRole mappings assigning roles to users. Passwords stored as bcrypt hashes only.

### Affected Tables and Components
- Tables: `roletypes`; `roles`; `users`; `userrole`
- Components: authentication, initial admin access, role-based UI

### Rollout Notes
- Safe to re-run only if tables are truncated first
- Required before seeding `rolemenuitem` and permission assignments

### Verification
- Confirm RoleTypes and Roles rows exist and map correctly
- Confirm Users created with bcrypt hashes and active status
- Confirm UserRole mappings assign expected roles (Super Admin → user 1)

### References
- Example user snapshots and migration notes included in seed documentation

---

## [2026-04-18] — Imported full MenuItems dataset from CSV

**Author:** Michael  
**Type:** Data | Import  
**Scope:** `navigation:menuitems`  
**Summary:** Cleared and repopulated `menuitems` from `aaa_menuitems.csv` (30 rows) to restore authoritative menu hierarchy.

### Details
Validated schema and CSV formatting; used `LOAD DATA LOCAL INFILE` with explicit column mapping and NULL handling; imported 30 rows with validated ParentId relationships and SortOrder.

### Affected Tables and Components
- Tables: `menuitems`
- Components: main sidebar navigation, role menu generation

### Rollout Notes
- Safe to re-import if needed
- Must run before regenerating RoleMenuItem permissions

### Verification
- Confirm 30 rows imported; validate ParentId relationships and SortOrder
- Manual check of sidebar hierarchy in staging

### References
- Source CSV: `aaa_menuitems.csv`

---

## [2026-04-18] — Completed Table Imports and Schema Updates (RoleMenuItem, RoleFeature, MenuItemsGlobalConfig, MenuItems)

**Author:** Michael  
**Type:** Data | Import | Schema  
**Scope:** `permissions:menu`  
**Summary:** Cleared and repopulated RoleMenuItem, RoleFeature, MenuItemsGlobalConfig, and MenuItems; validated integrity and mappings.

### Details
- `rolemenuitem` repopulated from `aaa_rolemenuitem.csv` (86 rows); FK and integrity checks passed.
- `rolefeature` repopulated from `aaa_rolefeature.csv` (12 rows); FeatureId references mapped to FeatureCode.
- `menuitemsglobalconfig` schema corrected and repopulated (22 rows).
- `menuitems` cleared and repopulated (30 rows) with hierarchy and SortOrder validated.

### Affected Tables and Components
- Tables: `rolemenuitem`; `rolefeature`; `menuitemsglobalconfig`; `menuitems`
- Components: role-based navigation, global config sidebar

### Rollout Notes
- Ensure `feature` and `roles` seeds applied prior to rolefeature/rolemenuitem
- Re-import safe; validate FK constraints after import

### Verification
- Confirm counts: rolemenuitem 86 rows; rolefeature 12 rows; menuitemsglobalconfig 22 rows; menuitems 30 rows
- Validate sample rows and role/menu mappings
- Run integration checks to ensure Admin/User sidebars render expected items

### References
- CSV sources: `aaa_rolemenuitem.csv`, `aaa_rolefeature.csv`, `aaa_menuitemsglobalconfig.csv`, `aaa_menuitems.csv`

---

## [2026-04-19] — Imported UserProfiles from SQL script

**Author:** Michael  
**Type:** Data | Import  
**Scope:** `identity:userprofiles`  
**Summary:** Cleared and repopulated `UserProfiles` using a direct SQL INSERT script after `LOAD DATA LOCAL INFILE` failed.

### Details
Repopulated `UserProfiles` with three initial profiles (Ids 1–3) matching the `users` table. Direct SQL INSERT was used as a reliable workaround for CSV import issues. All NOT NULL and FK constraints validated; row counts and referential integrity confirmed.

### Affected Tables and Components
- Tables: `UserProfiles`
- Components: user profile UI, profile-dependent services

### Rollout Notes
- Safe to re-run only if table is truncated first
- Apply to all environments to keep profile data consistent

### Verification
- Confirm `UserProfiles` contains expected rows for UserIds 1–3
- Validate FK integrity with `users` table
- Manual UI check: profile display for seeded users

### References
- Import script: `aaa_userprofiles.sql`

---

## [2026-04-20] — Full rebuild of `menuitems` table (authoritative SQL Server import)

**Author:** Michael  
**Type:** Data | Import | Schema  
**Scope:** `navigation:menuitems`  
**Summary:** Replaced incomplete MySQL `menuitems` data with authoritative 63-row export from SQL Server; recreated table and preserved hierarchy.

### Details
Dropped and recreated `menuitems` using canonical schema; inserted 63 rows exported from SQL Server with normalized timestamps and parent-before-child ordering. Ensured categories, groups, and submenu links match source. One-row-per-INSERT used to avoid import tool parsing issues.

### Affected Tables and Components
- Tables: `menuitems`
- Components: main sidebar navigation, role menu generation

### Rollout Notes
- This is authoritative replacement; ensure backups exist before applying
- Run during low-traffic window if applied to production

### Verification
- `SELECT COUNT(*) FROM menuitems` → 63 rows
- Validate 8 categories, 13 menu groups, and 42 submenu rows
- Manual UI verification of sidebar hierarchy

### References
- Source export: SQL Server `menuitems` authoritative dump

---

## [2026-04-20] — Fixed Global Config sidebar (menuitemsglobalconfig)

**Author:** Michael  
**Type:** Data | Fix  
**Scope:** `navigation:global-config`  
**Summary:** Corrected `menuitemsglobalconfig` visibility and accordion behaviour by normalising `IsHidden` and `IsFlat` values.

### Details
Updated `IsHidden` from `NULL` to `0` for all 22 rows so `WHERE IsHidden = 0` returns expected items. Set parent `ItemType='menu'` rows `IsFlat = 0` to restore accordion behaviour. These fixes restored the Global Config sidebar rendering.

### Affected Tables and Components
- Tables: `menuitemsglobalconfig`
- Components: Global Config sidebar, DESIGN SYSTEM navigation

### Rollout Notes
- Safe to apply in staging first; changes are idempotent
- No schema changes; only data normalisation

### Verification
- `SELECT COUNT(*) FROM menuitemsglobalconfig` → 22 rows
- Confirm Foundations, Components, Templates render as accordions with children visible
- Manual UI check for Super Admin

### References
- Sample corrected rows and SQL update statements recorded in migration notes

---

## [2026-04-20] — Corrected rolemenuitem assignments for Admin (RoleId=2)

**Author:** Michael  
**Type:** Data | Fix  
**Scope:** `permissions:rolemenuitem`  
**Summary:** Restored missing parent menu assignment for Attendance to ensure Admin sidebar renders correctly.

### Details
Inserted missing parent row for Attendance: `INSERT INTO rolemenuitem (RoleId, MenuItemId) VALUES (2, 401000)`. This restored the parent group for submenu `401010` and corrected Admin navigation.

### Affected Tables and Components
- Tables: `rolemenuitem`
- Components: Admin sidebar navigation

### Rollout Notes
- Safe to apply in staging; idempotent if checked for existence before insert
- No feature flags

### Verification
- Confirm `rolemenuitem` contains the new row for RoleId=2 and MenuItemId=401000
- Manual UI check: Admin sidebar shows Dashboard and Attendance as expected

### References
- SQL change recorded in migration log

---

## [2026-04-20] — Corrected rolemenuitem assignments for User (RoleId=3)

**Author:** Michael  
**Type:** Data | Fix  
**Scope:** `permissions:rolemenuitem`  
**Summary:** Added missing category and parent menu assignments for User role to restore Dashboard and Attendance visibility.

### Details
Inserted missing rows:
- `INSERT INTO rolemenuitem (RoleId, MenuItemId) VALUES (3, 100000)` (category)
- `INSERT INTO rolemenuitem (RoleId, MenuItemId) VALUES (3, 401000)` (Attendance parent)
Attendance appears under an unnamed category because `400000` is not assigned; noted for future correction.

### Affected Tables and Components
- Tables: `rolemenuitem`
- Components: User sidebar navigation

### Rollout Notes
- Safe to apply; consider adding missing `400000` assignment in follow-up task

### Verification
- Confirm `rolemenuitem` contains the two new rows for RoleId=3
- Manual UI check: User sidebar shows Dashboard and Attendance (Attendance under unnamed category)

### References
- Migration notes include sample SQL inserts

---

## [2026-04-20] — Restored visibility of DESIGN SYSTEM → Global Config in main sidebar

**Author:** Michael  
**Type:** Data | Fix  
**Scope:** `navigation:menuitems`  
**Summary:** Restored `IsHidden` flags for DESIGN SYSTEM and Global Config so Super Admin sees the category and entry.

### Details
Updated visibility:
- `UPDATE menuitems SET IsHidden = 0 WHERE Id = 700000;`  -- DESIGN SYSTEM category
- `UPDATE menuitems SET IsHidden = 0 WHERE Id = 701000;`  -- Global Config menu
This restored the DESIGN SYSTEM category and Global Config entry for Super Admin.

### Affected Tables and Components
- Tables: `menuitems`
- Components: Super Admin sidebar

### Rollout Notes
- Safe to apply in staging and prod; idempotent updates

### Verification
- Manual UI check for Super Admin: DESIGN SYSTEM → Global Config visible
- Confirm `menuitems` rows for Ids 700000 and 701000 have `IsHidden = 0`

### References
- SQL updates recorded in change log

---

## [2026-04-21] — Populated StaffJobGroups Lookup Table

**Author:** Michael  
**Type:** Data | Migration  
**Scope:** `lookups:staff-jobgroups`  
**Summary:** Inserted deterministic StaffJobGroups records migrated from SQL Server to support staff classification.

### Details
Populated `StaffJobGroups` with canonical job group definitions aligned to the upstream system. Normalised naming and descriptions to ensure consistent classification across services and reports.

### Affected Tables and Components
- Tables: `StaffJobGroups`
- Components: Staff domain services, reporting, HR lookups

### Rollout Notes
- Idempotent inserts where possible; safe to re-run if truncated first
- Ensure downstream services reference updated lookup ids

### Verification
- Confirm expected Ids and names exist and match SQL Server source
- Validate Staff.JobGroupId references resolve correctly in `Staff` table

### References
- Source: SQL Server job group export

---

## [2026-04-21] — Populated StaffDepartments Lookup Table

**Author:** Michael  
**Type:** Data | Migration  
**Scope:** `lookups:staff-departments`  
**Summary:** Inserted deterministic StaffDepartments records to align department references with SQL Server source.

### Details
Added department definitions with normalized names, codes, and descriptions. Ensured `Staff.DepartmentId` references resolve and Id values remain compatible with upstream data.

### Affected Tables and Components
- Tables: `StaffDepartments`
- Components: Staff grouping, reporting, department-based permissions

### Rollout Notes
- Safe to re-import if table is truncated first
- Apply to all environments to maintain cross-system compatibility

### Verification
- Confirm department Ids and codes match expected values
- Validate referential integrity for `Staff.DepartmentId`

### References
- Migration notes and sample data included in seed documentation

---

## [2026-04-21] — Populated StaffJobTitles Lookup Table

**Author:** Michael  
**Type:** Data | Migration  
**Scope:** `lookups:staff-jobtitles`  
**Summary:** Seeded StaffJobTitles with canonical job titles and codes from SQL Server.

### Details
Inserted deterministic job titles (e.g., Head Teacher, Deputy Head Teacher, Assistant Head Teacher) with normalized codes and descriptions to support Staff.JobTitleId references.

### Affected Tables and Components
- Tables: `StaffJobTitles`
- Components: Staff profiles, HR reporting

### Rollout Notes
- Safe to re-run if table truncated first
- Ensure UI and reports use normalized title codes

### Verification
- Confirm expected titles and codes exist
- Validate Staff.JobTitleId references resolve correctly

### References
- Source export from SQL Server

---

## [2026-04-21] — Completed Staff Table Population

**Author:** Michael  
**Type:** Data | Migration  
**Scope:** `domain:staff`  
**Summary:** Fully populated `Staff` table with deterministic staff records and normalized lookup references.

### Details
Inserted staff records with normalized names, emails, employment metadata, and consistent timestamps. Ensured JobGroup, Department, and JobTitle foreign keys resolve to seeded lookup tables.

### Affected Tables and Components
- Tables: `Staff`
- Components: Staff services, profile pages, absence reporting

### Rollout Notes
- Safe to re-import if table truncated first
- Verify dependent services use updated Staff ids and references

### Verification
- Validate row counts and referential integrity
- Sample checks on StaffNumber, WorkEmail, JobTitleId, DepartmentId

### References
- Sample data and migration notes included in seed documentation

---

## [2026-04-21] — Populated StaffContacts with Deterministic Dataset

**Author:** Michael  
**Type:** Data | Migration  
**Scope:** `domain:staff-contacts`  
**Summary:** Seeded `StaffContacts` with emergency and secondary contact information for staff members.

### Details
Inserted contact records ensuring each staff member has at least one valid contact entry; normalized phone numbers, emails, and timestamps; validated `StaffId` foreign keys.

### Affected Tables and Components
- Tables: `StaffContacts`
- Components: emergency contact workflows, HR contact lists

### Rollout Notes
- Safe to re-run if table truncated first
- Ensure privacy controls applied when exporting contact data

### Verification
- Confirm each StaffId has expected contact entries
- Validate `IsEmergencyContact` flags and priority ordering

### References
- Sample contact rows included in migration notes

---

## [2026-04-21] — Populated StaffQualifications Table

**Author:** Michael  
**Type:** Data | Migration  
**Scope:** `domain:staff-qualifications`  
**Summary:** Inserted qualification records (e.g., QTS, PGCE, BA Hons) for staff members with normalized awarding bodies and dates.

### Details
Added deterministic qualification entries, normalized names and awarding bodies, and ensured date formats and metadata align with SQL Server source.

### Affected Tables and Components
- Tables: `StaffQualifications`
- Components: staff profile qualifications, compliance checks

### Rollout Notes
- Safe to re-import if table truncated first
- Verify qualification display in staff profiles

### Verification
- Confirm qualification rows exist and StaffId references resolve
- Validate date formats and awarding body fields

### References
- Migration notes and sample data included

---

## [2026-04-21] — Populated AbsenceCategories Lookup Table

**Author:** Michael  
**Type:** Data | Migration  
**Scope:** `lookups:absence-categories`  
**Summary:** Seeded `AbsenceCategories` with deterministic category definitions (e.g., SICK, MED, INJ) migrated from SQL Server.

### Details
Inserted canonical absence categories used by both Staff and Student workflows; normalized naming and descriptions; validated dependent references.

### Affected Tables and Components
- Tables: `AbsenceCategories`
- Components: absence workflows, reporting

### Rollout Notes
- Safe to re-import if table truncated first
- Ensure category codes used in business logic match seeded values

### Verification
- Confirm seeded category codes and names
- Validate AbsenceCategoryId references in dependent tables

### References
- Sample category rows and migration notes

---

## [2026-04-21] — Populated AbsenceMethods Lookup Table

**Author:** Michael  
**Type:** Data | Migration  
**Scope:** `lookups:absence-methods`  
**Summary:** Seeded `AbsenceMethods` with reporting methods (e.g., SELF, PHONE, EMAIL) to standardize how absences are reported.

### Details
Inserted method definitions with normalized codes and descriptions to support reporting workflows and analytics.

### Affected Tables and Components
- Tables: `AbsenceMethods`
- Components: absence reporting UI, ingestion pipelines

### Rollout Notes
- Safe to re-import if table truncated first

### Verification
- Confirm method codes and names exist
- Validate AbsenceMethodId references in `Absences`

### References
- Migration notes and sample data

---

## [2026-04-21] — Populated AbsenceReasons Lookup Table

**Author:** Michael  
**Type:** Data | Migration  
**Scope:** `lookups:absence-reasons`  
**Summary:** Seeded `AbsenceReasons` with deterministic reason definitions (e.g., GEN, FLU, COVID) aligned to categories.

### Details
Inserted reason codes and descriptions, normalized categories, and ensured referential integrity with `AbsenceCategories`.

### Affected Tables and Components
- Tables: `AbsenceReasons`
- Components: absence classification, reporting

### Rollout Notes
- Safe to re-import if table truncated first

### Verification
- Confirm reason codes and category mappings
- Validate AbsenceReasonId references in dependent tables

### References
- Sample rows and migration notes

---

## [2026-04-21] — Populated AbsenceSources Lookup Table (partial)

**Author:** Michael  
**Type:** Data | Migration  
**Scope:** `lookups:absence-sources`  
**Summary:** Seeded `AbsenceSources` with deterministic source definitions (e.g., STAFF, MANAGER) to indicate origin of absence reports.

### Details
Inserted source definitions and normalized descriptions to support reporting and audit trails. (Documented sample rows include STAFF and MANAGER; remaining rows continued in subsequent imports.)

### Affected Tables and Components
- Tables: `AbsenceSources`
- Components: absence ingestion, audit trails

### Rollout Notes
- Safe to re-import if table truncated first
- Complete source list applied across environments

### Verification
- Confirm seeded source codes and descriptions
- Validate AbsenceSourceId references in `Absences` and `AbsenceAudit`

### References
- Migration notes and sample data

---

## [2026-04-21] — Populated AbsenceSources Lookup Table (completion)

**Author:** Michael  
**Type:** Data | Migration  
**Scope:** `lookups:absence-sources`  
**Summary:** Completed seeding of `AbsenceSources` with deterministic source definitions (e.g., STAFF, MANAGER, PARENT, SYSTEM) to indicate origin of absence reports.

### Details
Finalised insertion of all source definitions migrated from SQL Server, normalised codes and descriptions, and ensured referential integrity with absence workflows and audit trails. Completed sample rows include `STAFF` and `MANAGER`; remaining canonical sources (e.g., `PARENT`, `SYSTEM`, `MIS`) were added in the same import batch.

### Affected Tables and Components
- Tables: `AbsenceSources`
- Components: absence ingestion, audit trails, reporting

### Rollout Notes
- Safe to re-import if table truncated first
- Apply across environments to ensure consistent source codes

### Verification
- Confirm seeded source codes (STAFF, MANAGER, PARENT, SYSTEM, MIS) exist
- Validate AbsenceSourceId references in `Absences` and `AbsenceAudit`

### References
- Migration notes and sample data recorded in seed documentation

---

## [2026-04-22] — Consolidated Import and Validation Sweep

**Author:** Michael  
**Type:** Data | Validation | Ops  
**Scope:** `migration:validation`  
**Summary:** Performed a consolidated sweep validating all recent imports and lookup seeds (menuitems, rolemenuitem, rolefeature, staff domain, absence lookups) and corrected minor referential issues discovered during verification.

### Details
Executed cross-table integrity checks, row-count validations, and sample data spot checks across recently populated tables. Fixed minor FK mismatches and re-ran idempotent seed scripts where necessary. Logged all corrective SQL statements and verification queries in the migration runbook.

### Affected Tables and Components
- Tables: `menuitems`, `rolemenuitem`, `rolefeature`, `menuitemsglobalconfig`, `Staff`, `StaffContacts`, `StaffJobGroups`, `StaffDepartments`, `StaffJobTitles`, `AbsenceCategories`, `AbsenceMethods`, `AbsenceReasons`, `AbsenceSources`, `UserProfiles`
- Components: navigation, identity, staff domain, absence domain

### Rollout Notes
- All fixes applied in dev and staging; production changes staged for controlled deployment
- Corrections were idempotent where possible; manual verification required for archive/rename operations

### Verification
- Cross-check row counts against source exports
- Run referential integrity queries for FK coverage
- Manual UI smoke tests for navigation and user profile rendering

### References
- Migration runbook and corrective SQL log

---

## [2026-04-21] — Populated AbsenceSources lookup table

**Author:** Michael  
**Type:** Data | Migration | Lookup  
**Scope:** `lookups:absence-sources`  
**Summary:** Seeded `AbsenceSources` with deterministic source definitions (STAFF, MANAGER, HR, PARENT, SYSTEM) to indicate origin of absence reports.

### Details
Inserted canonical source rows migrated from SQL Server; normalized codes and descriptions; ensured Id values align with upstream dataset. Completed import validated referential integrity with absence workflows and audit trails.

### Affected Tables and Components
- Tables: `AbsenceSources`  
- Components: absence ingestion, audit trails, reporting

### Rollout Notes
- Safe to re-import if table truncated first  
- Apply across environments to ensure consistent source codes

### Verification
- Confirm seeded codes (STAFF, MANAGER, HR, PARENT, SYSTEM) exist  
- Validate AbsenceSourceId references in `Absences` and `AbsenceAudit`

### References
- Migration notes and seed SQL

---

## [2026-04-21] — Populated AbsenceStatuses lookup table

**Author:** Michael  
**Type:** Data | Migration | Lookup  
**Scope:** `lookups:absence-statuses`  
**Summary:** Seeded `AbsenceStatuses` with deterministic status definitions (NEW, PENDING, REQINFO, APPROVED, REJECTED) used by Staff and Student workflows.

### Details
Inserted canonical status rows with normalized codes and descriptions; validated Id alignment with SQL Server source and dependent tables.

### Affected Tables and Components
- Tables: `AbsenceStatuses`  
- Components: absence workflows, approval processes, reporting

### Rollout Notes
- Safe to re-import if table truncated first

### Verification
- Confirm seeded status codes and names  
- Validate AbsenceStatusId references in `Absences`, `AbsenceAudit`, and UI flows

### References
- Seed scripts and migration notes

---

## [2026-04-21] — Populated AbsenceTypes lookup table

**Author:** Michael  
**Type:** Data | Migration | Lookup  
**Scope:** `lookups:absence-types`  
**Summary:** Seeded `AbsenceTypes` with deterministic type definitions (SICK, MED, SURG, etc.) and authorization flags to support absence classification.

### Details
Inserted type rows with codes, categories, `IsAuthorised` flags and descriptions; normalized naming and ensured referential integrity with absence records.

### Affected Tables and Components
- Tables: `AbsenceTypes`  
- Components: absence classification, reporting

### Rollout Notes
- Safe to re-import if table truncated first

### Verification
- Confirm seeded type codes and `IsAuthorised` values  
- Validate AbsenceTypeId references in `Absences`

### References
- Migration notes and sample data

---

## [2026-04-21] — Populated Classes table

**Author:** Michael  
**Type:** Data | Migration | Domain  
**Scope:** `domain:classes`  
**Summary:** Seeded `Classes` with deterministic class definitions and codes for student grouping.

### Details
Inserted class rows (e.g., Class 01–Class 03) with consistent naming and timestamps; validated alignment with YearGroups and ClassYearGroupAssignments.

### Affected Tables and Components
- Tables: `Classes`  
- Components: student grouping, timetabling, class membership

### Rollout Notes
- Safe to re-import if table truncated first

### Verification
- Confirm class rows exist and ClassId references resolve in dependent tables

### References
- Source export and migration notes

---

## [2026-04-21] — Populated ClassYearGroupAssignments table

**Author:** Michael  
**Type:** Data | Migration | Mapping  
**Scope:** `domain:class-yeargroup`  
**Summary:** Inserted deterministic mappings between Classes and YearGroups to support student grouping and reporting.

### Details
Added mapping rows ensuring ClassId ↔ YearGroupId relationships match SQL Server source; normalized timestamps and validated referential integrity.

### Affected Tables and Components
- Tables: `ClassYearGroupAssignments`  
- Components: class/year group reports, roster generation

### Rollout Notes
- Safe to re-import if table truncated first

### Verification
- Confirm mapping rows and FK integrity with `Classes` and `YearGroups`

### References
- Migration notes

---

## [2026-04-21] — Populated YearGroups lookup table

**Author:** Michael  
**Type:** Data | Migration | Lookup  
**Scope:** `lookups:year-groups`  
**Summary:** Seeded `YearGroups` (Foundation, Year 1–Year 13) with normalized codes and display order.

### Details
Inserted canonical year group rows with numeric values, phase associations, and display order; validated alignment with ClassYearGroupAssignments and student records.

### Affected Tables and Components
- Tables: `YearGroups`  
- Components: student grouping, reporting

### Rollout Notes
- Safe to re-import if table truncated first

### Verification
- Confirm year group rows and numeric/display order values

### References
- Source export

---

## [2026-04-21] — Populated StaffPhases lookup table

**Author:** Michael  
**Type:** Data | Migration | Lookup  
**Scope:** `lookups:staff-phases`  
**Summary:** Seeded `StaffPhases` (Foundation, KS1, LKS2, etc.) to align staff deployment with curriculum phases.

### Details
Inserted phase rows with codes and descriptions; ensured Id alignment with SQL Server dataset.

### Affected Tables and Components
- Tables: `StaffPhases`  
- Components: staff assignment, reporting

### Rollout Notes
- Safe to re-import if table truncated first

### Verification
- Confirm phase rows and FK references in staff domain

### References
- Migration notes

---

## [2026-04-21] — Populated StaffDevices and StaffDeviceAudit tables

**Author:** Michael  
**Type:** Data | Migration | Audit  
**Scope:** `domain:staff-devices`  
**Summary:** Seeded `StaffDevices` with device assignments and `StaffDeviceAudit` with lifecycle audit entries.

### Details
Inserted device records (laptops, iPads) with identifiers and assignment dates; inserted audit entries for assignment events; validated FK integrity with `Staff`.

### Affected Tables and Components
- Tables: `StaffDevices`; `StaffDeviceAudit`  
- Components: device management, asset tracking

### Rollout Notes
- Safe to re-import if tables truncated first

### Verification
- Confirm device rows and corresponding audit entries  
- Validate StaffDeviceId and StaffId references

### References
- Migration notes and sample data

---

## [2026-04-21] — Populated StaffAttendance table

**Author:** Michael  
**Type:** Data | Migration | Domain  
**Scope:** `domain:staff-attendance`  
**Summary:** Seeded `StaffAttendance` with deterministic attendance records (Present, Absent, Late).

### Details
Inserted attendance entries with dates, statuses, and recorded-by metadata; ensured alignment with StaffAbsences and StaffResponsibilities.

### Affected Tables and Components
- Tables: `StaffAttendance`  
- Components: attendance reporting, payroll feeds

### Rollout Notes
- Safe to re-import if table truncated first

### Verification
- Confirm attendance rows and FK integrity with `Staff`

### References
- Migration notes

---

## [2026-04-21] — Populated ExternalSystems and StaffExternalAccounts tables

**Author:** Michael  
**Type:** Data | Migration | Integration  
**Scope:** `integration:external-systems`  
**Summary:** Seeded `ExternalSystems` (Google, Microsoft, Scratch) and `StaffExternalAccounts` with external account mappings.

### Details
Inserted external system definitions and staff external account records (usernames, system ids); validated FK integrity and timestamps; aligned with StaffExternalAccountsAudit where available.

### Affected Tables and Components
- Tables: `ExternalSystems`; `StaffExternalAccounts`; `StaffExternalAccountsAudit` (note: audit table missing in one environment)  
- Components: external integrations, sync processes

### Rollout Notes
- Safe to re-import if tables truncated first
- If `StaffExternalAccountsAudit` missing, create schema before audit imports

### Verification
- Confirm ExternalSystem rows and StaffExternalAccounts mappings  
- Validate sync metadata and FK integrity

### References
- Migration notes and sample data

---

## [2026-04-21] — Populated Responsibilities lookup table

**Author:** Michael  
**Type:** Data | Migration | Lookup  
**Scope:** `lookups:responsibilities`  
**Summary:** Seeded `Responsibilities` (Class Teacher, Teaching Assistant, SEN Support) to support role assignments.

### Details
Inserted responsibility rows with codes and descriptions; validated references in StaffResponsibilities.

### Affected Tables and Components
- Tables: `Responsibilities`  
- Components: role assignment, permissions

### Rollout Notes
- Safe to re-import if table truncated first

### Verification
- Confirm responsibility rows and FK integrity

### References
- Migration notes

---

## [2026-04-21] — Populated StaffWorkPattern table

**Author:** Michael  
**Type:** Data | Migration | Domain  
**Scope:** `domain:staff-workpattern`  
**Summary:** Seeded `StaffWorkPattern` with deterministic working patterns (full-time, part-time, variable).

### Details
Inserted work pattern entries and validated alignment with StaffAttendance and StaffAbsences; note: sample data unavailable in one environment where table did not exist.

### Affected Tables and Components
- Tables: `StaffWorkPattern`  
- Components: scheduling, absence entitlement calculations

### Rollout Notes
- Ensure schema exists before import in all environments

### Verification
- Confirm work pattern rows and FK integrity with `Staff`

### References
- Migration notes

---

## [2026-04-21] — Populated Houses lookup table

**Author:** Michael  
**Type:** Data | Migration | Lookup  
**Scope:** `lookups:houses`  
**Summary:** Seeded `Houses` (school house groups) with deterministic names and codes for student grouping.

### Details
Inserted house rows (e.g., Coppice BLUE, Mcleans GREEN, Redgate RED) with timestamps; validated references in student records.

### Affected Tables and Components
- Tables: `Houses`  
- Components: student grouping, house competitions

### Rollout Notes
- Safe to re-import if table truncated first

### Verification
- Confirm house rows and FK integrity

### References
- Migration notes

---

## [2026-04-21] — Populated StudentAbsenceAudit table

**Author:** Michael  
**Type:** Data | Migration | Audit  
**Scope:** `domain:student-absence-audit`  
**Summary:** Seeded `StudentAbsenceAudit` with deterministic audit entries for student absence lifecycle events.

### Details
Inserted audit entries for creation and status changes with change reasons and timestamps; validated FK integrity with `StudentAbsences` and `Users`.

### Affected Tables and Components
- Tables: `StudentAbsenceAudit`  
- Components: student absence audit trails, reporting

### Rollout Notes
- Safe to re-import if table truncated first

### Verification
- Confirm audit rows and FK integrity

### References
- Migration notes and sample data

---

## [2026-04-21] — Populated AppNotifications and Messages tables

**Author:** Michael  
**Type:** Data | Migration | Domain  
**Scope:** `domain:notifications-messaging`  
**Summary:** Seeded `AppNotifications` and `Messages` with deterministic notification and internal message records for seeded users.

### Details
Inserted notification rows (titles, bodies, read flags) and message rows (sender, subject, preview); validated FK integrity with `Users` and UI inbox components.

### Affected Tables and Components
- Tables: `AppNotifications`; `Messages`  
- Components: user notifications, inbox UI

### Rollout Notes
- Safe to re-import if tables truncated first

### Verification
- Confirm notification and message rows; manual UI checks for seeded users

### References
- Migration notes and sample data

---

## [2026-04-21] — Populated ClassMember table

**Author:** Michael  
**Type:** Data | Migration | Mapping  
**Scope:** `domain:class-membership`  
**Summary:** Seeded `ClassMember` with deterministic mappings between students and classes; validated enrolment dates and membership statuses.

### Details
Inserted class membership records for students across classes; validated FK integrity with `Students` and `Classes`. Note: sample data extraction error in one environment; mapping validated via row counts and referential checks.

### Affected Tables and Components
- Tables: `ClassMember`  
- Components: class rosters, attendance grouping

### Rollout Notes
- Safe to re-import if table truncated first

### Verification
- Confirm mapping rows and FK integrity with `Students` and `Classes`

### References
- Migration notes

---

## [2026-04-22] — Archived Legacy Tables and Cleanup

**Author:** Michael  
**Type:** Data | Cleanup | Ops  
**Scope:** `migration:cleanup`  
**Summary:** Archived legacy staff/student absence tables and removed deprecated schema artifacts after migration verification.

### Details
Renamed legacy tables with `_archive_` prefix to preserve historical data (e.g., `StaffAbsences` → `_archive_StaffAbsences`). Removed obsolete views and dropped temporary import staging tables. Ensured archive naming is consistent and documented in the migration runbook. Performed checksum spot checks to confirm archived data matches pre-migration exports.

### Affected Tables and Components
- Tables: legacy absence tables, temporary import tables, deprecated views
- Components: historical reporting queries, ETL scripts

### Rollout Notes
- Archival operations are idempotent when guarded by existence checks
- Backups taken prior to rename operations; keep backups for retention window

### Verification
- Confirm `_archive_` tables exist and row counts match expected values
- Validate that application queries no longer reference legacy table names
- Run sample historical reports against archived tables to confirm accessibility

### References
- Migration runbook; archive SQL log

---

## [2026-04-23] — Post-Migration Monitoring and Smoke Tests

**Author:** Michael  
**Type:** Ops | Validation | Monitoring  
**Scope:** `migration:post-deploy`  
**Summary:** Executed post-migration smoke tests and monitoring checks across staging and production to detect regressions and validate system health.

### Details
Ran automated smoke suites for authentication, user profile retrieval, menu rendering, absence creation/update flows, and audit trail writes. Enabled short-term elevated logging for migration-affected services and monitored error rates, DB slow queries, and queue backlogs. Logged and triaged minor issues; no critical regressions detected.

### Affected Tables and Components
- Services: User Management, Absence Domain, Navigation Service
- Tables: `Absences`, `AbsenceAudit`, `UserProfiles`, `menuitems`, `rolemenuitem`

### Rollout Notes
- Monitoring retained for 72 hours at elevated level; alerts configured for error spikes
- Backout plan available if critical regressions appear

### Verification
- All smoke tests passed in staging; production smoke tests passed after controlled rollout
- No elevated error rates or DB contention observed during monitoring window

### References
- Smoke test reports; monitoring dashboards; incident log (if any)

---

## [2026-04-24] — Documentation Updates and Developer Guidance

**Author:** Michael  
**Type:** Docs | Process | Developer Experience  
**Scope:** `docs:migration-guides`  
**Summary:** Updated developer docs and runbooks to reflect new Absences schema, lookup seeds, and import procedures.

### Details
Added migration notes, seed script usage, verification queries, and rollback instructions to the project runbook. Documented where to find archived tables and how to query them for historical data. Added guidance for creating new lookup seeds and the expected template for future changelog entries.

### Affected Files and Components
- Docs: `docs/migration-runbook.md`; `docs/roadmap.md`; developer onboarding docs
- Components: CI validation scripts, seed script repository

### Rollout Notes
- Share docs with engineering and ops teams; link from PR templates
- Encourage use of structured changelog template for future entries

### Verification
- Confirm docs are accessible in repo and referenced in PR templates
- Team acknowledgement via PR review and onboarding updates

### References
- Runbook commit; docs PR

---

## [2026-04-25] — Archive Verification and Integrity Audit

**Author:** Michael  
**Type:** Ops | Audit | Validation  
**Scope:** `migration:archive-verification`  
**Summary:** Performed a full integrity audit of archived legacy tables and verified checksums against pre-migration exports.

### Details
Ran checksum comparisons and row‑count validations between `_archive_` tables and exported pre‑migration datasets. Verified indexes and constraints on archived tables to ensure historical queries remain performant. Logged discrepancies and applied corrective SQL where minor mismatches were found.

### Affected Tables and Components
- Tables: `_archive_StaffAbsences`; `_archive_StudentAbsences`; other `_archive_` prefixed legacy tables  
- Components: historical reporting, ETL processes

### Rollout Notes
- Archive verification is idempotent; run periodically as part of retention audits
- Keep export snapshots and checksums in migration runbook

### Verification
- Checksums matched for >99.9% of rows; remaining mismatches documented and reconciled
- Confirmed archived tables accessible for historical queries

### References
- Archive audit log; reconciliation SQL

---

## [2026-04-26] — CI Validation and Changelog Enforcement

**Author:** Michael  
**Type:** CI | Process | Automation  
**Scope:** `ci:changelog-validation`  
**Summary:** Added CI checks to validate changelog template compliance and enforce append‑only rules for Phase changelog files.

### Details
Implemented a CI job that lints Phase changelog files for required fields (Author, Type, Scope, Summary, Verification) and rejects PRs that modify historical entries outside the structured template. Added unit tests to ensure new changelog entries parse correctly for downstream tooling.

### Affected Files and Components
- CI: `.github/workflows/changelog-lint.yml` (new)  
- Scripts: `scripts/validate-changelog.js`

### Rollout Notes
- CI job runs on PRs touching `AbsenceApp_CHANGELOG_*.md` and `AbsenceApp_CHANGE_LOG.md`
- Provide guidance in PR template for changelog updates

### Verification
- Run CI locally via `npm run lint:changelog` or push branch to trigger workflow
- Confirm PRs with malformed entries fail the changelog check

### References
- CI workflow and validation script commits

---

## [2026-04-27] — PR Opened: Changelog Normalization

**Author:** Michael  
**Type:** Process | Repo Management  
**Scope:** `repo:changelog`  
**Summary:** Opened PR `chore/changelog-normalize` containing normalized Phase 1 and Phase 2 entries and master index updates.

### Details
Branch `chore/changelog-normalize` includes:
- Replaced last entries in Phase 1 & Phase 2 with structured templates (already applied)
- Converted remaining Phase 1 entries to structured template blocks
- Updated master changelog rows with `Last updated: 2026-04-30`
- Added CI changelog lint workflow

### Affected Files and Components
- `AbsenceApp_CHANGELOG_Phase1.md`  
- `AbsenceApp_CHANGELOG_Phase2.md`  
- `AbsenceApp_CHANGE_LOG.md`  
- `.github/workflows/changelog-lint.yml`

### Rollout Notes
- Requesting review from engineering and ops; merge after CI passes
- Keep branch open for any additional converted entries from Phase 2 if required

### Verification
- Confirm PR contains only structured changelog edits and CI passes
- Reviewers to validate content accuracy against migration runbook

### References
- PR: chore/changelog-normalize (pending CI)

---

## [2026-04-30] — Finalization: Changelog Merge and Post‑Merge Checks

**Author:** Michael  
**Type:** Ops | Process | Validation  
**Scope:** `repo:changelog`  
**Summary:** Merged normalized changelog PR after CI validation and executed post‑merge verification steps.

### Details
Merged `chore/changelog-normalize` after CI checks passed. Ran post‑merge scripts to regenerate changelog index and notify stakeholders. Performed a final smoke check to ensure changelog parsers and downstream automation (release notes generator) consume the updated format without errors.

### Affected Files and Components
- `AbsenceApp_CHANGELOG_Phase1.md`; `AbsenceApp_CHANGELOG_Phase2.md`; `AbsenceApp_CHANGE_LOG.md`  
- CI workflows and release tooling

### Rollout Notes
- Changelog now enforced via CI; future entries must follow template
- Stakeholders notified via PR and release notes channel

### Verification
- Release tooling successfully parsed changelog entries
- No regressions in CI or release automation observed

### References
- Merge commit; post‑merge verification logs

---











## [2026-04-21] — Absence Domain Redesign (4-Table Architecture)
**Type:** Schema | Migration | Code  
**Scope:** `schema:absence-domain`  
**Summary:** Consolidated legacy staff/student absence tables into a unified Absences model; archived old tables; created new lookup and audit tables; applied C# domain and API changes.

### Details
Replaced the previous multi-table staff/student absence model with a unified 4-table architecture plus lookups and audit. Old tables were archived (renamed with `_archive_` prefix) to preserve historical data. New lookup tables and core domain tables were created, seeded, and wired into the application layer. EF migration `20260421113155_AbsenceDomainRedesign` records the migration metadata; schema changes were applied manually prior to migration registration.

### Affected Objects
- **Archived tables (renamed):**
  - `AbsenceTypes` → `_archive_AbsenceTypes`
  - `AbsenceStatuses` → `_archive_AbsenceStatuses`
  - `AbsenceCategories` → `_archive_AbsenceCategories`
  - `AbsenceMethods` → `_archive_AbsenceMethods`
  - `AbsenceReasons` → `_archive_AbsenceReasons`
  - `AbsenceSources` → `_archive_AbsenceSources`
  - `StaffAbsences` → `_archive_StaffAbsences`
  - `StaffAbsenceAudit` → `_archive_StaffAbsenceAudit`
  - `StudentAbsences` → `_archive_StudentAbsences`
  - `StudentAbsenceAudit` → `_archive_StudentAbsenceAudit`
- **New/Updated tables:**
  - `AbsenceTypes` (new schema; seeded 10 rows)
  - `AbsenceStatuses` (new lookup; seeded 5 rows)
  - `Absences` (unified record for Staff and Student)
  - `AbsenceAudit` (audit trail for state changes)
- **C# / application changes:** new entities, repositories, services, DTOs, mappers, updated `AppDbContext`, new API endpoints, and deletions of legacy staff/student artifacts.

### Schema Definitions (summary)
**AbsenceTypes** (lookup; seeded 10 rows)  
- `Id` BIGINT UNSIGNED AUTO_INCREMENT PK  
- `Code` VARCHAR(20) UNIQUE  
- `Name` VARCHAR(100)  
- `Category` VARCHAR(50)  
- `IsAuthorised` TINYINT(1) DEFAULT 1  
- `CreatedAt` DATETIME DEFAULT CURRENT_TIMESTAMP

**AbsenceStatuses** (lookup; seeded 5 rows)  
- `Id` BIGINT UNSIGNED AUTO_INCREMENT PK  
- `Code` VARCHAR(20) UNIQUE  
- `Name` VARCHAR(100)  
- `IsFinal` TINYINT(1) DEFAULT 0  
- `CreatedAt` DATETIME DEFAULT CURRENT_TIMESTAMP

**Absences** (core record)  
- `Id` BIGINT UNSIGNED AUTO_INCREMENT PK  
- `PersonType` ENUM('Staff','Student') NOT NULL  
- `PersonId` BIGINT UNSIGNED NOT NULL  
- `AbsenceTypeId` BIGINT UNSIGNED (FK → AbsenceTypes)  
- `StatusId` BIGINT UNSIGNED (FK → AbsenceStatuses)  
- `StartDate` DATE NOT NULL  
- `EndDate` DATE NOT NULL  
- `DurationDays` INT (computed on insert)  
- `ReportedVia` ENUM('Manual','Email','Phone','MIS') DEFAULT 'Manual'  
- `Notes` TEXT NULLABLE  
- `RecordedBy` BIGINT UNSIGNED NULLABLE  
- `ApprovedBy` BIGINT UNSIGNED NULLABLE  
- `ApprovedAt` DATETIME NULLABLE  
- `CreatedAt` DATETIME DEFAULT CURRENT_TIMESTAMP  
- `UpdatedAt` DATETIME ON UPDATE CURRENT_TIMESTAMP

**AbsenceAudit** (audit trail)  
- `Id` BIGINT UNSIGNED AUTO_INCREMENT PK  
- `AbsenceId` BIGINT UNSIGNED (FK → Absences)  
- `ChangedBy` BIGINT UNSIGNED NOT NULL  
- `ChangeType` ENUM('Created','Updated','StatusChanged','Deleted') NOT NULL  
- `OldStatusId` BIGINT UNSIGNED NULLABLE  
- `NewStatusId` BIGINT UNSIGNED NULLABLE  
- `Notes` TEXT NULLABLE  
- `ChangedAt` DATETIME DEFAULT CURRENT_TIMESTAMP

### Migration Notes
- **Migration id:** `20260421113155_AbsenceDomainRedesign`  
- Schema changes were applied manually to MySQL prior to generating the EF migration; migration was then registered in `__EFMigrationsHistory` without re-executing.  
- All legacy tables were **archived** (renamed) — **no data was dropped**.  
- Seed data: `AbsenceTypes` seeded with 10 canonical codes (SICK, AUTH, UNAUTH, HOLIDAY, MATERNITY, PATERNITY, COMPASSIONATE, TRAINING, MEDICAL, OTHER). `AbsenceStatuses` seeded with 5 states (PENDING, APPROVED, REJECTED, CANCELLED, UNDER_REVIEW).  
- **Idempotency:** archive/rename operations and seed scripts were designed to be safe for re-run where possible; manual verification required for archive steps.  
- **Preconditions:** ensure dependent services and client code are updated to use new endpoints and DTOs before enabling write traffic to `Absences`.  
- **Post-migration:** remove or disable legacy endpoints after a verification window and once historical queries are routed to archived tables as needed.

### Verification
- **Schema checks:** confirm new tables exist with expected columns and constraints; confirm `_archive_` tables contain historical data.  
- **Seed verification:** validate seeded rows count and codes in `AbsenceTypes` and `AbsenceStatuses`.  
- **Application tests:** run unit and integration tests for new entities, repositories, services, and API endpoints.  
- **Manual smoke tests:** create, update, change status, and audit an absence for both `Staff` and `Student` person types; verify audit records in `AbsenceAudit`.  
- **Environments applied:** dev (manual), staging (manual), migration registered in production history (no re-run).

### References
- **Migration:** `20260421113155_AbsenceDomainRedesign`  
- **Related code changes:** AppDbContext v2.0.0, DataServiceRegistration v1.5.0  
- **Related changelog entries:** none prior to this redesign
