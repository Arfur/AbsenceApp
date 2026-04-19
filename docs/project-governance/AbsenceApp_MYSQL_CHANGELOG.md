================================================================================
 File        : AbsenceApp_MYSQL_CHANGELOG.md
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-04-16
 Updated     : 2026-04-16
--------------------------------------------------------------------------------
 Purpose     :
   Provides a structured, deterministic changelog for all future modifications
   to the MySQL AbsenceApp schema. This ensures traceability, governance, and
   consistent documentation of schema evolution.

   This file exists to:
     - Track every schema change with full accountability
     - Prevent undocumented modifications
     - Support migrations, rollback planning, and audit requirements
--------------------------------------------------------------------------------
 Notes       :
   - This file starts empty and grows over time.
   - Each entry must follow the exact template below.
================================================================================


# CHANGELOG (MySQL Schema)

## [2026-04-16] — Seeded Feature and RoleTypes Tables

### Summary
Initial baseline permission data was inserted into the `feature` and `roletypes`
tables to align MySQL with the existing SQL Server permission model.

### Details
#### Feature Table
Inserted 6 baseline feature records:

1. ui.sidebar — Controls visibility of the main application sidebar  
2. ui.global-settings — Access to global system configuration  
3. student.view — View student records  
4. student.create — Create student records  
5. student.edit — Edit student records  
6. student.delete — Delete student records  

All records include:
- Id (1–6)
- Code
- DisplayName
- Description
- IsEnabled = 1
- CreatedAt / UpdatedAt timestamps

#### RoleTypes Table
Inserted baseline role types required for system operation (e.g., System Role,
Default Role, etc.).

### Affected Tables
- feature  
- roletypes  

### Migration Notes
- Safe to re-run (idempotent seed)  
- Required for permission evaluation  
- Must be applied to all environments  

## [2026‑04‑16] — Seeded RoleTypes, Roles, Users, and UserRole Tables  

### Summary  
Populated all core identity and access‑control tables required before assigning
menu‑based permissions. This includes role types, roles, initial system users,
and user‑to‑role mappings. These entries establish the authentication and
authorization foundation for the MySQL AbsenceApp environment.

---

### Details  

#### 1. RoleTypes Table  
Inserted the three baseline role types used throughout the system:

| Id | Name        |
|----|-------------|
| 1  | Super Admin |
| 2  | Admin       |
| 3  | User        |

These values mirror the SQL Server model and are required before inserting into
`roles`.

---

#### 2. Roles Table  
Inserted the three system roles, each mapped to the corresponding RoleType:

| Id | RoleTypeId | Name        |
|----|-------------|-------------|
| 1  | 1           | Super Admin |
| 2  | 2           | Admin       |
| 3  | 3           | User        |

These roles are referenced by `userrole` and later by `rolemenuitem`.

---

#### 3. Users Table  
Inserted the three initial users required for system bootstrap.  
Each user record includes:

- Username  
- Email  
- Bcrypt password hash (12‑round cost)  
- Status = active  
- IsAdmin flag  
- CreatedAt / UpdatedAt timestamps  

**Example snapshot of inserted data:**

| Id | Username | Email                         | PasswordHash (bcrypt)                                   | Status | IsAdmin |
|----|----------|-------------------------------|----------------------------------------------------------|--------|---------|
| 1  | mbattle  | mbattle@orchard.leics.sch.uk  | $2a$12$K26ro9YBQfLBES7wryTXheoNaDbAhxXo2LIhM.vlKm...     | active | 1       |
| 2  | ewoods   | ewoods@orchard.leics.sch.uk   | $2a$12$eeFz6N3iyvHsN4r/9oRo0eDoWwroObYySTdRkA4Whdz...     | active | 0       |
| 3  | mfarrar  | mfarrar@orchard.leics.sch.uk  | $2a$12$MPkqZhUWNzUOdo.xPyXc..exvlfTCOT2Oawt4B55p11...     | active | 0       |

**Password generation process:**  
All passwords were hashed using bcrypt with a cost factor of 12.  
The plaintext passwords were never stored — only the bcrypt hashes.

---

#### 4. UserRole Table  
Mapped each user to the correct role:

| Id | UserId | RoleId |
|----|--------|--------|
| 1  | 1      | 1      |  ← Super Admin  
| 2  | 2      | 3      |  ← Standard User  
| 3  | 3      | 3      |  ← Standard User  

Each record includes an AssignedAt timestamp and AssignedBy = 1.

---

### Affected Tables  
- roletypes  
- roles  
- users  
- userrole  

### Migration Notes  
- These tables must be populated before any permission‑based tables  
- Safe to re‑run only if tables are truncated first  
- Required before seeding `rolemenuitem`

## 2026-04-18 — Imported full MenuItems dataset from CSV

**Summary:**
Cleared the MenuItems table and repopulated it using the file `aaa_menuitems.csv`.

**Details:**
- Confirmed MenuItems schema was correct prior to import.
- CSV formatting validated: header row present, quoted descriptions, literal NULLs handled, UTF-8 encoding.
- Used LOAD DATA LOCAL INFILE for import with explicit column mapping and NULL handling.
- Import completed successfully: 30 rows loaded, 0 invalid ParentId relationships, hierarchy and SortOrder validated.

**Affected Tables:**
- MenuItems

**Sample Data:**
| Id     | Label                | ParentId | SortOrder | Status |
|--------|----------------------|----------|-----------|--------|
| 0      | NULL                 | NULL     | NULL      | NULL   |
| 100000 | Main Sidebar         | NULL     | 100000    | NULL   |
| 101000 | Dashboard            | 100000   | 101000    | NULL   |
| 101010 | Operational Overview | 101000   | 101010    | active |
| 101020 | Attendance & Trends  | 101000   | 101020    | active |

**Migration Notes:**
- Safe to re-import if needed.
- Must be done before regenerating RoleMenuItem permissions.

## 2026-04-18 — Completed Table Imports and Schema Updates

### RoleMenuItem
- Table cleared and repopulated from `aaa_rolemenuitem.csv` (86 rows imported)
- Structure, foreign keys, and data integrity validated
- Distinct RoleId: 3, Distinct MenuItemId: 46
- Sample Data:
  | Id | RoleId | MenuItemId | IsEnabled | CreatedAt           | AssignedBy |
  |----|--------|------------|-----------|---------------------|------------|
  | 1  | 1      | 100000     | 1         | 2026-04-18 00:00:00 | 1          |
  | 2  | 2      | 101000     | 1         | 2026-04-18 00:00:00 | 1          |
  | 3  | 3      | 101010     | 1         | 2026-04-18 00:00:00 | 1          |

### RoleFeature
- Table cleared and repopulated from `aaa_rolefeature.csv` (12 rows imported)
- All FeatureId references mapped to FeatureCode
- Sample Data:
  | Id | RoleId | FeatureCode        | IsEnabled | AssignedAt          | AssignedBy |
  |----|--------|--------------------|-----------|---------------------|------------|
  | 1  | 1      | ui.sidebar         | 1         | 2026-04-18 00:00:00 | 1          |
  | 2  | 1      | ui.global-settings | 1         | 2026-04-18 00:00:00 | 1          |
  | 3  | 1      | student.view       | 1         | 2026-04-18 00:00:00 | 1          |

### MenuItemsGlobalConfig
- Schema corrected (columns dropped/added as required)
- Table cleared and repopulated from `aaa_menuitemsglobalconfig.csv` (22 rows imported)
- Sample Data:
  | Id     | ItemType | Label         | ParentId | SortOrder | Category      | GroupName   | GroupIcon  | IsFlat | Status | IsHidden | CreatedAt           | UpdatedAt           |
  |--------|----------|---------------|----------|-----------|--------------|-------------|------------|--------|--------|----------|---------------------|---------------------|
  | 100000 | category | DESIGN SYSTEM |          | 100000    | DESIGN SYSTEM|             |            |        |        |          | 2026-04-06 12:15:00 | 2026-04-06 12:15:00 |
  | 101000 | menu     | Foundations   | 100000   | 101000    | DESIGN SYSTEM| Foundations | bi-sliders |        |        |          | 2026-04-06 12:15:00 | 2026-04-06 12:15:00 |

### UserFeatureOverride
- Table initialised using header-only CSV as designed (no records inserted)

### MenuItems
- Table cleared and repopulated from `aaa_menuitems.csv` (30 rows imported)
- Hierarchy and SortOrder validated
- Sample Data:
  | Id     | Label                | ParentId | SortOrder | Status |
  |--------|----------------------|----------|-----------|--------|
  | 0      | NULL                 | NULL     | NULL      | NULL   |
  | 100000 | Main Sidebar         | NULL     | 100000    | NULL   |
  | 101000 | Dashboard            | 100000   | 101000    | NULL   |
  | 101010 | Operational Overview | 101000   | 101010    | active |
  | 101020 | Attendance & Trends  | 101000   | 101020    | active |

### Notes
- All SQL commands executed directly by the agent
- All imports validated for row count and data integrity

## 2026-04-19 — Imported UserProfiles from SQL script

**Summary:**
- Cleared the UserProfiles table and repopulated it using a direct SQL INSERT script (`aaa_userprofiles.sql`) as a workaround for LOAD DATA LOCAL INFILE issues.
- All three user profiles (Id 1–3) were inserted and validated against the users table (foreign key constraint).
- Manual insert confirmed schema and data compatibility; import now complete and validated.

**Details:**
- Table was empty before import.
- Data matches the CSV source and all NOT NULL and FK constraints.
- Row count and data integrity validated post-insert.

**Affected Tables:**
- UserProfiles

**Sample Data:**
| Id | UserId | DisplayName     | Timezone      | LanguageCode | ThemePreference | AccentColor | CreatedAt           | UpdatedAt           |
|----|--------|-----------------|--------------|--------------|-----------------|-------------|---------------------|---------------------|
| 1  | 1      | Michael Battle  | Europe/London| en-GB        | system          | #0078D4     | 2026-04-19 12:00:00 | 2026-04-19 12:00:00 |
| 2  | 2      | Emma Woods      | Europe/London| en-GB        | system          | #0078D4     | 2026-04-19 12:00:00 | 2026-04-19 12:00:00 |
| 3  | 3      | Michelle Farrar | Europe/London| en-GB        | system          | #0078D4     | 2026-04-19 12:00:00 | 2026-04-19 12:00:00 |

**Migration Notes:**
- LOAD DATA LOCAL INFILE failed silently; direct SQL insert used as workaround.
- Safe to re-import if table is truncated first.
- All SQL executed directly by the agent.
- Data validated for row count and referential integrity.

