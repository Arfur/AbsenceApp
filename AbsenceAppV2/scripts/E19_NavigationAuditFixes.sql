-- =============================================================================
-- E19 Navigation Audit Fixes — Database-Only Remediation Script
-- File    : E19_NavigationAuditFixes.sql
-- Version : 1.0.0
-- Created : 2026-04-19
-- Updated : 2026-04-19
-- -----------------------------------------------------------------------------
-- Purpose : Applies the SQL-only fixes from the E18 audit remediation plan.
--           These target dbo.MenuItems and dbo.RoleFeature, which are NOT
--           EF-managed entities. Run this script AFTER applying EF migrations:
--             dotnet ef database update
--
-- Execution order:
--   1. FIX-07  Update MenuItems Settings group label
--   2. FIX-08  Hide DESIGN SYSTEM entries
--   3. FIX-10  Hide stale /details placeholder routes
--   4. FIX-12  Hide empty Parents menu group
--   5. FIX-04  Insert new User Management entries in MenuItems
--   6. FIX-03  Insert RoleFeature permissions for RoleType 1 (User) and 2 (Admin)
--              (includes new entries from FIX-04)
--
-- NOTE: All statements are wrapped in a transaction. Review the output before
--       committing. Rollback on any error.
-- =============================================================================

SET NOCOUNT ON;
BEGIN TRANSACTION;

-- ===========================================================================
-- FIX-07 — Rename "Table Settings" → "Settings" in MenuItems
-- Affected rows: 601000 (group), 601010 (submenu), and the denormalized
-- GroupName column on the submenu row.
-- ===========================================================================
PRINT 'FIX-07: Renaming Table Settings → Settings...';

UPDATE dbo.MenuItems
SET    Label     = 'Settings',
       UpdatedAt = GETDATE()
WHERE  Id = 601000;  -- Table Settings group

UPDATE dbo.MenuItems
SET    Label     = 'Settings',
       GroupName = 'Settings',
       UpdatedAt = GETDATE()
WHERE  Id = 601010;  -- Table Settings submenu

-- ===========================================================================
-- FIX-08 — Hide DESIGN SYSTEM legacy entries
-- Keeps rows in place (IsHidden=1) so historical data is preserved.
-- These entries duplicate /global-settings/colours which is already in
-- dbo.MenuItemsGlobalConfig (id=101010).
-- ===========================================================================
PRINT 'FIX-08: Hiding DESIGN SYSTEM entries (700000, 701000, 701010)...';

UPDATE dbo.MenuItems
SET    IsHidden  = 1,
       UpdatedAt = GETDATE()
WHERE  Id IN (700000, 701000, 701010);

-- ===========================================================================
-- FIX-10 — Hide stale /details placeholder routes
-- These three submenu rows point to placeholder pages that do not yet have
-- real detail views. They are hidden until real detail pages are built.
-- ===========================================================================
PRINT 'FIX-10: Hiding stale /details placeholder routes...';

UPDATE dbo.MenuItems
SET    IsHidden  = 1,
       UpdatedAt = GETDATE()
WHERE  Id IN (
    201020,  -- Student Details  /v2/students/details
    202020,  -- Staff Details    /v2/staff/details
    301020   -- Class Details    /v2/classes/details
);

-- ===========================================================================
-- FIX-12 — Hide empty Parents menu group
-- The Parents group (203000) has no submenu children and will always be
-- pruned by BuildCategories. Setting IsHidden=1 prevents it from passing
-- through fn_GetVisibleMenuItems via the "OR ItemType='menu'" rule.
-- ===========================================================================
PRINT 'FIX-12: Hiding empty Parents menu group (203000)...';

UPDATE dbo.MenuItems
SET    IsHidden  = 1,
       UpdatedAt = GETDATE()
WHERE  Id = 203000;

-- ===========================================================================
-- FIX-04 — New MenuItems rows for /v2/users and /v2/pages
-- Placement: CONFIGURATION category (500000) → new group "User Management"
--            Group Id: 502000 (IsHidden=1 follows existing group pattern)
--            Submenu Ids: 502010 (Users), 502020 (Pages)
-- NOTE: SortOrder intentionally equals Id (consistent with existing rows).
-- ===========================================================================
PRINT 'FIX-04: Inserting User Management MenuItems...';

-- New group: User Management
INSERT INTO dbo.MenuItems (Id, Label, ItemType, ParentId, Category, GroupName, SortOrder, IsHidden, IsFlat, CreatedAt, UpdatedAt)
VALUES (502000, 'User Management', 'menu', 500000, 'CONFIGURATION', 'User Management', 502000, 1, 0, GETDATE(), GETDATE());

-- New submenu: Users
INSERT INTO dbo.MenuItems (Id, Label, Route, ItemType, ParentId, Category, GroupName, SortOrder, IsHidden, IsFlat, CreatedAt, UpdatedAt)
VALUES (502010, 'Users', '/v2/users', 'submenu', 502000, 'CONFIGURATION', 'User Management', 502010, 0, 0, GETDATE(), GETDATE());

-- New submenu: Pages
INSERT INTO dbo.MenuItems (Id, Label, Route, ItemType, ParentId, Category, GroupName, SortOrder, IsHidden, IsFlat, CreatedAt, UpdatedAt)
VALUES (502020, 'Pages', '/v2/pages', 'submenu', 502000, 'CONFIGURATION', 'User Management', 502020, 0, 0, GETDATE(), GETDATE());

-- ===========================================================================
-- FIX-03  RoleFeature permissions for RoleType 2 (Admin)
--
-- Policy: Admin sees all existing 36 MenuItems plus the 3 new items from
--         FIX-04 (502000, 502010, 502020). DESIGN SYSTEM entries (700000,
--         701000, 701010) are included here but will not be visible because
--         they are set to IsHidden=1 by FIX-08.
-- ===========================================================================
PRINT 'FIX-03: Inserting RoleFeature rows for RoleType=2 (Admin)...';

INSERT INTO dbo.RoleFeature (RoleType, MenuItemId, FeatureType, IsAllowed, CreatedAtUtc)
VALUES
    -- MAIN SIDEBAR category + Dashboard group + submenus
    (2, 100000, 'menu', 1, GETUTCDATE()),
    (2, 101000, 'menu', 1, GETUTCDATE()),
    (2, 101010, 'menu', 1, GETUTCDATE()),
    (2, 101020, 'menu', 1, GETUTCDATE()),
    (2, 101030, 'menu', 1, GETUTCDATE()),
    -- PEOPLE category + Students group + submenus
    (2, 200000, 'menu', 1, GETUTCDATE()),
    (2, 201000, 'menu', 1, GETUTCDATE()),
    (2, 201010, 'menu', 1, GETUTCDATE()),
    (2, 201020, 'menu', 1, GETUTCDATE()),
    (2, 201030, 'menu', 1, GETUTCDATE()),
    -- Staff group + submenus
    (2, 202000, 'menu', 1, GETUTCDATE()),
    (2, 202010, 'menu', 1, GETUTCDATE()),
    (2, 202020, 'menu', 1, GETUTCDATE()),
    (2, 202030, 'menu', 1, GETUTCDATE()),
    -- Parents group (hidden by FIX-12)
    (2, 203000, 'menu', 1, GETUTCDATE()),
    -- ACADEMICS category + Classes group + submenus
    (2, 300000, 'menu', 1, GETUTCDATE()),
    (2, 301000, 'menu', 1, GETUTCDATE()),
    (2, 301010, 'menu', 1, GETUTCDATE()),
    (2, 301020, 'menu', 1, GETUTCDATE()),
    (2, 301030, 'menu', 1, GETUTCDATE()),
    -- ATTENDANCE category + Attendance group + submenu
    (2, 400000, 'menu', 1, GETUTCDATE()),
    (2, 401000, 'menu', 1, GETUTCDATE()),
    (2, 401010, 'menu', 1, GETUTCDATE()),
    -- CONFIGURATION category + Audit Log group + submenu
    (2, 500000, 'menu', 1, GETUTCDATE()),
    (2, 501000, 'menu', 1, GETUTCDATE()),
    (2, 501010, 'menu', 1, GETUTCDATE()),
    -- User Management group + submenus (FIX-04)
    (2, 502000, 'menu', 1, GETUTCDATE()),
    (2, 502010, 'menu', 1, GETUTCDATE()),
    (2, 502020, 'menu', 1, GETUTCDATE()),
    -- SETTINGS category + Settings group + submenu
    (2, 600000, 'menu', 1, GETUTCDATE()),
    (2, 601000, 'menu', 1, GETUTCDATE()),
    (2, 601010, 'menu', 1, GETUTCDATE()),
    -- Diagnostics group + submenu
    (2, 602000, 'menu', 1, GETUTCDATE()),
    (2, 602010, 'menu', 1, GETUTCDATE()),
    -- Site group + submenu
    (2, 603000, 'menu', 1, GETUTCDATE()),
    (2, 603010, 'menu', 1, GETUTCDATE()),
    -- DESIGN SYSTEM category + entries (hidden by FIX-08; permissions kept for consistency)
    (2, 700000, 'menu', 1, GETUTCDATE()),
    (2, 701000, 'menu', 1, GETUTCDATE()),
    (2, 701010, 'menu', 1, GETUTCDATE());

-- ===========================================================================
-- FIX-03  RoleFeature permissions for RoleType 1 (User)
--
-- Policy: Users see operational modules only.
--   Allowed : MAIN SIDEBAR, PEOPLE (Students/Staff), ACADEMICS (Classes),
--             ATTENDANCE.
--   Denied  : CONFIGURATION, SETTINGS, DESIGN SYSTEM.
--   Note    : 201020, 202020, 301020 are included (hidden by FIX-10 anyway).
--             Parents group (203000) included (hidden by FIX-12 anyway).
--             FIX-04 items (502000, 502010, 502020) are excluded by omission.
-- ===========================================================================
PRINT 'FIX-03: Inserting RoleFeature rows for RoleType=1 (User)...';

INSERT INTO dbo.RoleFeature (RoleType, MenuItemId, FeatureType, IsAllowed, CreatedAtUtc)
VALUES
    -- MAIN SIDEBAR category + Dashboard group + submenus
    (1, 100000, 'menu', 1, GETUTCDATE()),
    (1, 101000, 'menu', 1, GETUTCDATE()),
    (1, 101010, 'menu', 1, GETUTCDATE()),
    (1, 101020, 'menu', 1, GETUTCDATE()),
    (1, 101030, 'menu', 1, GETUTCDATE()),
    -- PEOPLE category + Students group + submenus
    (1, 200000, 'menu', 1, GETUTCDATE()),
    (1, 201000, 'menu', 1, GETUTCDATE()),
    (1, 201010, 'menu', 1, GETUTCDATE()),
    (1, 201020, 'menu', 1, GETUTCDATE()),  -- Student Details (hidden by FIX-10)
    (1, 201030, 'menu', 1, GETUTCDATE()),
    -- Staff group + submenus
    (1, 202000, 'menu', 1, GETUTCDATE()),
    (1, 202010, 'menu', 1, GETUTCDATE()),
    (1, 202020, 'menu', 1, GETUTCDATE()),  -- Staff Details (hidden by FIX-10)
    (1, 202030, 'menu', 1, GETUTCDATE()),
    -- Parents group (hidden by FIX-12)
    (1, 203000, 'menu', 1, GETUTCDATE()),
    -- ACADEMICS category + Classes group + submenus
    (1, 300000, 'menu', 1, GETUTCDATE()),
    (1, 301000, 'menu', 1, GETUTCDATE()),
    (1, 301010, 'menu', 1, GETUTCDATE()),
    (1, 301020, 'menu', 1, GETUTCDATE()),  -- Class Details (hidden by FIX-10)
    (1, 301030, 'menu', 1, GETUTCDATE()),
    -- ATTENDANCE category + Attendance group + submenu
    (1, 400000, 'menu', 1, GETUTCDATE()),
    (1, 401000, 'menu', 1, GETUTCDATE()),
    (1, 401010, 'menu', 1, GETUTCDATE());
    -- CONFIGURATION, SETTINGS, DESIGN SYSTEM: denied by omission.
    -- FIX-04 items (502000, 502010, 502020): denied by omission.

COMMIT TRANSACTION;
PRINT 'E19 SQL fixes applied successfully.';

-- ===========================================================================
-- Verification queries (run separately to check results)
-- ===========================================================================
/*
-- Confirm FIX-07: both Settings rows renamed
SELECT Id, Label, GroupName, ItemType FROM dbo.MenuItems WHERE Id IN (601000, 601010);

-- Confirm FIX-08: DESIGN SYSTEM hidden
SELECT Id, Label, IsHidden FROM dbo.MenuItems WHERE Id IN (700000, 701000, 701010);

-- Confirm FIX-10: details routes hidden
SELECT Id, Label, Route, IsHidden FROM dbo.MenuItems WHERE Id IN (201020, 202020, 301020);

-- Confirm FIX-12: Parents group hidden
SELECT Id, Label, IsHidden FROM dbo.MenuItems WHERE Id = 203000;

-- Confirm FIX-04: new User Management rows exist
SELECT Id, Label, Route, ItemType, IsHidden FROM dbo.MenuItems WHERE Id IN (502000, 502010, 502020);

-- Confirm FIX-03: role permission counts
SELECT RoleType, COUNT(*) AS PermissionCount
FROM   dbo.RoleFeature
WHERE  FeatureType = 'menu'
GROUP  BY RoleType
ORDER  BY RoleType;
-- Expected: RoleType 1 = 23, RoleType 2 = 39, RoleType 3 = 36 (pre-FIX-04 count)
*/
