-- =============================================================================
-- E32 System Management — RoleFeature Seed Script
-- File    : E32_SystemManagementRoleFeature.sql
-- Version : 1.0.0
-- Created : 2026-05-xx
-- -----------------------------------------------------------------------------
-- Purpose : Grants RoleType 3 (super_admin) visibility of the 10 System
--           Management MenuItems (IDs 800000–802030) that were inserted in E31.
--
--           The sidebar reads from dbo.fn_GetVisibleMenuItems(@RoleType), which
--           requires a matching dbo.RoleFeature row (FeatureType='menu',
--           IsAllowed=1) for each item when EnableRoleBasedNavigation=true.
--           Without these rows the TVF WHERE clause excluded all 10 new items.
--
-- Items covered:
--   800000   category  SYSTEM MANAGEMENT
--   801000   menu      User Management
--   801010   submenu   Users
--   801020   submenu   Roles
--   801030   submenu   Permissions
--   801040   submenu   Page Access
--   802000   menu      Pages
--   802010   submenu   All Pages
--   802020   submenu   Page Layouts
--   802030   submenu   Page Metadata
--
-- Role scope: RoleType 3 (super_admin) only.
--   Admin (2) and User (1) should NOT see System Management items.
--
-- Idempotent: guarded by NOT EXISTS so re-running is safe.
-- =============================================================================

SET NOCOUNT ON;
BEGIN TRANSACTION;

DECLARE @now DATETIME2 = GETUTCDATE();
DECLARE @roleType INT   = 3;   -- super_admin

PRINT 'E32: Inserting RoleFeature rows for System Management (RoleType=3)...';

INSERT INTO dbo.RoleFeature (RoleType, MenuItemId, FeatureType, IsAllowed, CreatedAtUtc)
SELECT @roleType, mi.Id, 'menu', 1, @now
FROM   (VALUES
    (800000),
    (801000), (801010), (801020), (801030), (801040),
    (802000), (802010), (802020), (802030)
) AS mi(Id)
WHERE NOT EXISTS (
    SELECT 1
    FROM   dbo.RoleFeature rf
    WHERE  rf.MenuItemId   = mi.Id
      AND  rf.RoleType     = @roleType
      AND  rf.FeatureType  = 'menu'
);

DECLARE @inserted INT = @@ROWCOUNT;
PRINT 'E32: Rows inserted: ' + CAST(@inserted AS NVARCHAR(10));

-- Verify
SELECT MenuItemId, RoleType, FeatureType, IsAllowed
FROM   dbo.RoleFeature
WHERE  FeatureType = 'menu'
  AND  MenuItemId >= 800000
ORDER  BY MenuItemId;

COMMIT TRANSACTION;
PRINT 'E32: Done.';
