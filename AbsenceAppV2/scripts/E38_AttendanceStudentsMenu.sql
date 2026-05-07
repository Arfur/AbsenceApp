-- =============================================================================
-- E38 Attendance → Students Menu Restructure
-- File    : E38_AttendanceStudentsMenu.sql
-- Version : 1.1.0
-- Created : 2026-05-06
-- Updated : 2026-05-06
-- -----------------------------------------------------------------------------
-- Changes :
--   v1.1.0  2026-05-06  Promoted to executable; removed example guard.
--   v1.0.0  2026-05-06  Initial creation. Creates ATTENDANCE → Students menu
--                       node (Id=402000) and moves Student Absences (201050)
--                       and Student Calendar (201060) under it. Adds
--                       rolemenuitems rows for the new menu node.
-- -----------------------------------------------------------------------------
-- Purpose : Implements Option B of the navigation restructure plan:
--
--             ATTENDANCE (400000)
--               Attendance (401000)
--                 Attendance (401010)        ← unchanged
--               Students  (402000)           ← NEW
--                 Absences  (201050)         ← moved from PEOPLE/Students
--                 Calendar  (201060)         ← moved from PEOPLE/Students
--
--           Student Profile (201040) remains under PEOPLE → Students unchanged.
--
-- Sections :
--   A. menuitems   — INSERT IGNORE new Students menu under ATTENDANCE
--   B. menuitems   — UPDATE Absences + Calendar rows to point to new parent
--   C. rolemenuitems — INSERT IGNORE role visibility rows for the new menu
--   D. Validation SELECTs — confirm tree integrity
--
-- Idempotent : Section A uses INSERT IGNORE.
--              Section B is a safe UPDATE (WHERE Id IN (...) — safe to re-run
--              because the values being SET are the target final state).
--              Section C uses INSERT IGNORE.
--              Re-running this script against an already-migrated DB is safe.
--
-- Prerequisites :
--   • menuitems row 400000 (ATTENDANCE category) must exist.
--   • menuitems rows 201050 and 201060 must exist.
--   • roles table must contain RoleId 1 (super_admin), 2 (admin), 3 (user).
--   • Run against the 'absenceapp' MariaDB database.
-- =============================================================================

USE absenceapp;

-- ===========================================================================
-- SECTION A — new Students menu under ATTENDANCE
-- ---------------------------------------------------------------------------
-- Adds one new menu node as a direct child of the ATTENDANCE category (400000).
-- Id=402000 is consistent with the SortOrder=Id convention used throughout.
-- SortOrder=402000 places it after the existing Attendance menu (401000).
-- IsHidden=0 so it is visible immediately.
-- GroupIcon is set to the mortarboard icon consistent with the PEOPLE/Students
-- group, so the sidebar renders the same icon in the ATTENDANCE context.
-- ===========================================================================

INSERT IGNORE INTO menuitems
    (Id, Label, Route, ItemType, ParentId, Category,
     GroupName, Icon, GroupIcon,
     SortOrder, IsHidden, IsFlat, Status, Description, CreatedAt, UpdatedAt)
VALUES
    (402000, 'Students', NULL, 'menu', 400000, 'ATTENDANCE',
     'Students', 'bi-mortarboard', 'bi-mortarboard',
     402000, 0, 0, NULL, 'Student absence management navigation.', NOW(), NOW());

-- ===========================================================================
-- SECTION B — move Student Absences (201050) and Student Calendar (201060)
-- ---------------------------------------------------------------------------
-- Re-parents both rows from the PEOPLE/Students menu (201000) to the new
-- ATTENDANCE/Students menu (402000). Also corrects Category and GroupName so
-- the NavigationApiServiceV2 + MenuResolver tree-build places them in the
-- ATTENDANCE sidebar section, not PEOPLE.
--
-- GroupIcon is set to bi-mortarboard to match the new parent.
-- SortOrder is left unchanged (50 and 60 respectively) — ordering within the
-- new parent remains correct.
--
-- The rolemenuitems rows for 201050 and 201060 already exist (added by E37)
-- and remain valid — their FK is the stable MenuItemId, not the location.
-- ===========================================================================

UPDATE menuitems
SET    ParentId  = 402000,
       Category  = 'ATTENDANCE',
       GroupName = 'Students',
       GroupIcon = 'bi-mortarboard',
       UpdatedAt = NOW()
WHERE  Id IN (201050, 201060);

-- ===========================================================================
-- SECTION C — rolemenuitems rows for the new Students menu (402000)
-- ---------------------------------------------------------------------------
-- The new Students menu node (402000) requires its own rolemenuitems rows so
-- NavigationApiServiceV2 includes it in the sidebar INNER JOIN result set.
-- Without these rows the group node is invisible regardless of whether its
-- child submenus have role links.
--
-- RoleId mapping (from aaa_rolemenuitem.csv and changelog):
--   1 = super_admin
--   2 = admin
--   3 = user (standard)
--
-- Id values 900010, 900011, 900012 continue the explicit sequence started
-- by E37 (900001–900009). The table has no AUTO_INCREMENT on Id.
-- AssignedBy=1 (system/super-admin user).
-- ===========================================================================

INSERT IGNORE INTO rolemenuitems (Id, RoleId, MenuItemId, IsEnabled, AssignedAt, AssignedBy)
VALUES
    (900010, 1, 402000, 1, NOW(), 1),   -- super_admin
    (900011, 2, 402000, 1, NOW(), 1),   -- admin
    (900012, 3, 402000, 1, NOW(), 1);   -- user/standard

-- ---------------------------------------------------------------------------
-- Attendance Officer placeholder
-- ---------------------------------------------------------------------------
-- If an Attendance Officer role exists in the roles/roletypes table, add a
-- row here. Verify the correct RoleId against the live database before
-- executing. Example (uncomment and set the correct Id):
--
--   INSERT IGNORE INTO rolemenuitems (Id, RoleId, MenuItemId, IsEnabled, AssignedAt, AssignedBy)
--   VALUES (900013, <AttendanceOfficerRoleId>, 402000, 1, NOW(), 1);
-- ---------------------------------------------------------------------------

-- ===========================================================================
-- SECTION D — Validation SELECTs
-- ---------------------------------------------------------------------------
-- Run these queries after executing Sections A–C to confirm the tree is
-- correct. Expected results are documented inline.
-- ===========================================================================

-- D1: Confirm the ATTENDANCE category and its direct children
-- Expected: 400000 (ATTENDANCE category), 401000 (Attendance menu), 402000 (Students menu)
SELECT Id, Label, ItemType, ParentId, Category, SortOrder
FROM   menuitems
WHERE  Id = 400000
   OR  ParentId = 400000
ORDER  BY SortOrder;

-- D2: Confirm the new Students menu and its children
-- Expected: 402000 (Students menu), 201050 (Absences), 201060 (Calendar)
SELECT Id, Label, ItemType, ParentId, Category, GroupName
FROM   menuitems
WHERE  Id = 402000
   OR  ParentId = 402000
ORDER  BY SortOrder;

-- D3: Confirm 201050 and 201060 are no longer children of PEOPLE/Students (201000)
-- Expected: 0 rows (both have been re-parented to 402000)
SELECT Id, Label, ParentId, Category
FROM   menuitems
WHERE  Id IN (201050, 201060)
  AND  ParentId = 201000;

-- D4: Confirm rolemenuitems rows exist for the new Students menu (402000)
-- Expected: at least 3 rows (RoleId 1, 2, 3)
SELECT Id, RoleId, MenuItemId, IsEnabled
FROM   rolemenuitems
WHERE  MenuItemId = 402000
ORDER  BY RoleId;

-- D5: Confirm existing rolemenuitems for 201050 and 201060 are intact
-- Expected: the 6 rows inserted by E37 (900002–900003, 900005–900006, 900008–900009)
SELECT Id, RoleId, MenuItemId, IsEnabled
FROM   rolemenuitems
WHERE  MenuItemId IN (201050, 201060)
ORDER  BY RoleId, MenuItemId;
