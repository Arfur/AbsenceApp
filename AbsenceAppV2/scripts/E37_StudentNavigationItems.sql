-- =============================================================================
-- E37 Student Navigation Items — Database Seed Script
-- File    : E37_StudentNavigationItems.sql
-- Version : 1.1.0
-- Created : 2026-05-05
-- Updated : 2026-05-05
-- -----------------------------------------------------------------------------
-- Changes :
--   v1.1.0  2026-05-05  Fix rolemenuitem INSERT to supply explicit Id values
--                       (900001-900009); table has no default. Merge into one
--                       statement. Fix app_pages -> AppPages (correct table name).
--                       Delete Section D (menuitemglobalconfigs does not apply
--                       to PEOPLE/ATTENDANCE navigation items).
-- -----------------------------------------------------------------------------
-- Purpose : Adds navigation and permission rows for the three new Student
--           Absence Management pages:
--             • Student Profile   /v2/students/{id}
--             • Student Absences  /v2/students/{id}/absences
--             • Student Calendar  /v2/students/{id}/calendar
--
-- Sections:
--   A. menuitems        — INSERT IGNORE three submenu rows (201040–201060)
--   B. rolemenuitem     — INSERT IGNORE for RoleId 1 (super_admin),
--                         2 (admin), and 3 (user / standard)
--   C. AppPages         — INSERT IGNORE pages 15–17
--
-- Idempotent : All statements use INSERT IGNORE.
--              Safe to re-run against a database that already has these rows.
--
-- Prerequisites:
--   • menuitems row 201000 (Students group parent) must exist.
--   • roles table must contain the target RoleId values.
--   • Run against the 'absenceapp' MariaDB database.
-- =============================================================================

USE absenceapp;

-- ===========================================================================
-- SECTION A — menuitems
-- ---------------------------------------------------------------------------
-- Adds three submenu entries under the existing Students group (Id=201000).
-- Columns: Id, Label, Route, ItemType, ParentId, Category, GroupName, Icon,
--          SortOrder, IsHidden, IsFlat, Status, Description, CreatedAt, UpdatedAt
-- SortOrder values continue after 201030 (existing last Students submenu).
-- IsHidden=0 so these items appear in navigation immediately.
-- ===========================================================================

INSERT IGNORE INTO menuitems
    (Id, Label, Route, ItemType, ParentId, Category, GroupName, Icon,
     SortOrder, IsHidden, IsFlat, Status, Description, CreatedAt, UpdatedAt)
VALUES
    (201040, 'Profile',
     '/v2/students/{id}',
     'submenu', 201000, 'PEOPLE', 'Students', 'bi-person-lines-fill',
     40, 0, 0, 'active', 'View a student full profile', NOW(), NOW()),

    (201050, 'Absences',
     '/v2/students/{id}/absences',
     'submenu', 201000, 'PEOPLE', 'Students', 'bi-calendar-x',
     50, 0, 0, 'active', 'Manage student absence records', NOW(), NOW()),

    (201060, 'Calendar',
     '/v2/students/{id}/calendar',
     'submenu', 201000, 'PEOPLE', 'Students', 'bi-calendar3',
     60, 0, 0, 'active', 'View student attendance calendar', NOW(), NOW());

-- ===========================================================================
-- SECTION B — rolemenuitem
-- ---------------------------------------------------------------------------
-- Grants visibility of the three new menu items to:
--   RoleId 1 = super_admin
--   RoleId 2 = admin
--   RoleId 3 = user (standard)
--
-- NOTE: The rolemenuitem table requires (Id, RoleId, MenuItemId, IsEnabled,
--       AssignedAt, AssignedBy). Id values are chosen to be safely above the
--       existing max. Adjust if your sequence differs.
--       AssignedBy=1 (system/super-admin user) — update if needed.
--
-- RoleId mapping:
--   1 = super_admin  (full access)
--   2 = admin        (full access)
--   3 = user/standard (read-only operational access)
-- ===========================================================================

INSERT IGNORE INTO rolemenuitem (Id, RoleId, MenuItemId, IsEnabled, AssignedAt, AssignedBy)
VALUES
    (900001, 1, 201040, 1, NOW(), 1),
    (900002, 1, 201050, 1, NOW(), 1),
    (900003, 1, 201060, 1, NOW(), 1),
    (900004, 2, 201040, 1, NOW(), 1),
    (900005, 2, 201050, 1, NOW(), 1),
    (900006, 2, 201060, 1, NOW(), 1),
    (900007, 3, 201040, 1, NOW(), 1),
    (900008, 3, 201050, 1, NOW(), 1),
    (900009, 3, 201060, 1, NOW(), 1);

-- ===========================================================================
-- SECTION C — app_pages
-- ---------------------------------------------------------------------------
-- Registers the three new pages in the canonical page registry.
-- Columns: Id, Name, Slug, Route, CategoryKey, MenuKey, IconKey,
--          IsActive, SortOrder,
--          SupportsRead, SupportsWrite, SupportsCreate, SupportsDelete,
--          SupportsImport, SupportsExport,
--          CreatedAt, UpdatedAt
-- ===========================================================================

INSERT IGNORE INTO AppPages
    (Id, Name, Slug, Route, CategoryKey, MenuKey, IconKey,
     IsActive, SortOrder,
     SupportsRead, SupportsWrite, SupportsCreate, SupportsDelete, SupportsImport, SupportsExport,
     CreatedAt, UpdatedAt)
VALUES
    (15, 'Student Profile',
     'student-profile', '/v2/students/:id',
     'PEOPLE', 'Students', 'bi-person-lines-fill',
     1, 21,
     1, 0, 0, 0, 0, 0,
     '2026-05-05 00:00:00', '2026-05-05 00:00:00'),

    (16, 'Student Absences',
     'student-absences', '/v2/students/:id/absences',
     'PEOPLE', 'Students', 'bi-calendar-x',
     1, 22,
     1, 1, 1, 1, 0, 1,
     '2026-05-05 00:00:00', '2026-05-05 00:00:00'),

    (17, 'Student Calendar',
     'student-calendar', '/v2/students/:id/calendar',
     'PEOPLE', 'Students', 'bi-calendar3',
     1, 23,
     1, 0, 0, 0, 0, 0,
     '2026-05-05 00:00:00', '2026-05-05 00:00:00');

-- =============================================================================
-- END OF SCRIPT
-- =============================================================================
