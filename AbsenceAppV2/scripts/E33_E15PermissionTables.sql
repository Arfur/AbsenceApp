-- ===========================================================================
-- E33 E15 Permission System — Missing Tables + Seed Data
-- File    : E33_E15PermissionTables.sql
-- Version : 1.0.0
-- Created : 2026-04-24
-- Updated : 2026-04-24
-- ---------------------------------------------------------------------------
-- Purpose  : Creates the four E15 permission tables that are missing from the
--            absenceapp MySQL database. These tables are actively queried by
--            PermissionServiceV2 and UserManagementService but were never
--            created because the EF Core Baseline + UserProfileConfiguration
--            migrations were not applied to the live database.
--
--            Also renames UserProfiles → user_profiles to align the database
--            with the [Table("user_profiles")] attribute on the EF entity
--            (UserProfile.cs v1.1.0, applied 2026-04-22).
--
-- Tables created (in dependency order):
--   1. app_pages                         — canonical page registry
--   2. role_default_page_permissions     — role-level defaults (per page)
--   3. user_page_permissions             — per-user explicit permissions
--   4. user_page_overrides               — per-user Grant / Deny overrides
--
-- Seed data:
--   • app_pages                    — 14 V2 application pages (exact values
--                                    from UserManagementModelBuilderExtensions)
--   • role_default_page_permissions — sensible defaults for 5 roles:
--                                    super_admin / admin / staff_admin /
--                                    teacher / office_staff
--
-- Idempotent: CREATE TABLE uses IF NOT EXISTS.
--             INSERT seed data uses INSERT IGNORE (skips duplicates).
--             The UserProfiles rename is guarded by a stored procedure.
--
-- Prerequisites: Run against the 'absenceapp' MySQL / MariaDB database.
--               No EF migrations required.
-- ===========================================================================

USE absenceapp;

-- ===========================================================================
-- STEP 0 — Rename UserProfiles → user_profiles (if not already done)
-- ---------------------------------------------------------------------------
-- The EF entity UserProfile maps to 'user_profiles'. If the live DB still
-- holds the old name, this renames it safely. Re-running is harmless.
-- ===========================================================================

DROP PROCEDURE IF EXISTS _e33_rename_user_profiles;

DELIMITER $$
CREATE PROCEDURE _e33_rename_user_profiles()
BEGIN
    DECLARE old_exists INT DEFAULT 0;
    DECLARE new_exists INT DEFAULT 0;

    SELECT COUNT(*) INTO old_exists
    FROM information_schema.TABLES
    WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'UserProfiles';

    SELECT COUNT(*) INTO new_exists
    FROM information_schema.TABLES
    WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'user_profiles';

    IF old_exists > 0 AND new_exists = 0 THEN
        RENAME TABLE UserProfiles TO user_profiles;
        SELECT 'STEP 0: Renamed UserProfiles → user_profiles' AS result;
    ELSEIF new_exists > 0 THEN
        SELECT 'STEP 0: user_profiles already exists — skipped' AS result;
    ELSE
        SELECT 'STEP 0: Neither UserProfiles nor user_profiles found — check schema' AS result;
    END IF;
END$$
DELIMITER ;

CALL _e33_rename_user_profiles();
DROP PROCEDURE IF EXISTS _e33_rename_user_profiles;


-- ===========================================================================
-- STEP 1 — Create app_pages
-- ---------------------------------------------------------------------------
-- Columns derived from AppPage.cs entity + UserManagementModelBuilderExtensions
-- configuration (b.ToTable("app_pages")).
-- Unique indexes on Route and Slug mirror the EF HasIndex definitions.
-- ===========================================================================

CREATE TABLE IF NOT EXISTS app_pages (
    Id              INT          NOT NULL AUTO_INCREMENT,
    Name            VARCHAR(200) NOT NULL,
    Slug            VARCHAR(200) NOT NULL DEFAULT '',
    Route           VARCHAR(500) NOT NULL,
    CategoryKey     VARCHAR(100) NOT NULL DEFAULT '',
    MenuKey         VARCHAR(200) NOT NULL DEFAULT '',
    IconKey         VARCHAR(100)     NULL,
    Description     VARCHAR(1000)    NULL,
    IsActive        TINYINT(1)   NOT NULL DEFAULT 1,
    SortOrder       INT          NOT NULL DEFAULT 0,
    SupportsRead    TINYINT(1)   NOT NULL DEFAULT 0,
    SupportsWrite   TINYINT(1)   NOT NULL DEFAULT 0,
    SupportsCreate  TINYINT(1)   NOT NULL DEFAULT 0,
    SupportsDelete  TINYINT(1)   NOT NULL DEFAULT 0,
    SupportsImport  TINYINT(1)   NOT NULL DEFAULT 0,
    SupportsExport  TINYINT(1)   NOT NULL DEFAULT 0,
    CreatedAt       DATETIME(6)  NOT NULL DEFAULT (UTC_TIMESTAMP(6)),
    UpdatedAt       DATETIME(6)  NOT NULL DEFAULT (UTC_TIMESTAMP(6)),
    PRIMARY KEY (Id),
    UNIQUE INDEX IX_app_pages_Route (Route),
    UNIQUE INDEX IX_app_pages_Slug  (Slug)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;


-- ===========================================================================
-- STEP 2 — Create role_default_page_permissions
-- ---------------------------------------------------------------------------
-- One row per (RoleTypeName, PageId) pair. RoleTypeName is a denormalised
-- string (not a FK to role_types) matching RoleType.Name.
-- ===========================================================================

CREATE TABLE IF NOT EXISTS role_default_page_permissions (
    Id           INT          NOT NULL AUTO_INCREMENT,
    RoleTypeName VARCHAR(100) NOT NULL,
    PageId       INT          NOT NULL,
    CanRead      TINYINT(1)   NOT NULL DEFAULT 0,
    CanWrite     TINYINT(1)   NOT NULL DEFAULT 0,
    CanCreate    TINYINT(1)   NOT NULL DEFAULT 0,
    CanDelete    TINYINT(1)   NOT NULL DEFAULT 0,
    CanImport    TINYINT(1)   NOT NULL DEFAULT 0,
    CanExport    TINYINT(1)   NOT NULL DEFAULT 0,
    PRIMARY KEY (Id),
    UNIQUE INDEX IX_rdpp_Role_Page (RoleTypeName, PageId),
    CONSTRAINT FK_rdpp_PageId
        FOREIGN KEY (PageId) REFERENCES app_pages (Id) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;


-- ===========================================================================
-- STEP 3 — Create user_page_permissions
-- ---------------------------------------------------------------------------
-- Highest-priority override: one row per (UserId, PageId). When present,
-- PermissionServiceV2 uses this row directly instead of role defaults.
-- ===========================================================================

CREATE TABLE IF NOT EXISTS user_page_permissions (
    Id        INT        NOT NULL AUTO_INCREMENT,
    UserId    BIGINT     NOT NULL,
    PageId    INT        NOT NULL,
    CanRead   TINYINT(1) NOT NULL DEFAULT 0,
    CanWrite  TINYINT(1) NOT NULL DEFAULT 0,
    CanCreate TINYINT(1) NOT NULL DEFAULT 0,
    CanDelete TINYINT(1) NOT NULL DEFAULT 0,
    CanImport TINYINT(1) NOT NULL DEFAULT 0,
    CanExport TINYINT(1) NOT NULL DEFAULT 0,
    PRIMARY KEY (Id),
    UNIQUE INDEX IX_upp_User_Page (UserId, PageId),
    CONSTRAINT FK_upp_PageId
        FOREIGN KEY (PageId) REFERENCES app_pages (Id) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;


-- ===========================================================================
-- STEP 4 — Create user_page_overrides
-- ---------------------------------------------------------------------------
-- Explicit Grant / Deny overrides checked before role defaults.
-- OverrideType: 'Grant' or 'Deny' (10-char max per EF config).
-- ===========================================================================

CREATE TABLE IF NOT EXISTS user_page_overrides (
    Id           INT         NOT NULL AUTO_INCREMENT,
    UserId       BIGINT      NOT NULL,
    PageId       INT         NOT NULL,
    OverrideType VARCHAR(10) NOT NULL,
    PRIMARY KEY (Id),
    UNIQUE INDEX IX_upo_User_Page (UserId, PageId),
    CONSTRAINT FK_upo_PageId
        FOREIGN KEY (PageId) REFERENCES app_pages (Id) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;


-- ===========================================================================
-- STEP 5 — Seed app_pages (14 canonical V2 pages)
-- ---------------------------------------------------------------------------
-- Source of truth: UserManagementModelBuilderExtensions.cs v1.2.0 DefaultPages
-- array. INSERT IGNORE skips any row whose Route or Slug already exists.
-- Explicit IDs are used to match EF HasData seed values.
-- Columns: Id, Name, Slug, Route, CategoryKey, MenuKey, IconKey,
--          IsActive, SortOrder,
--          SupportsRead, SupportsWrite, SupportsCreate, SupportsDelete,
--          SupportsImport, SupportsExport,
--          CreatedAt, UpdatedAt
-- ===========================================================================

INSERT IGNORE INTO app_pages
    (Id, Name, Slug, Route, CategoryKey, MenuKey, IconKey,
     IsActive, SortOrder,
     SupportsRead, SupportsWrite, SupportsCreate, SupportsDelete, SupportsImport, SupportsExport,
     CreatedAt, UpdatedAt)
VALUES
    -- ── Core pages (ids 1–9) ───────────────────────────────────────────────
    ( 1, 'Dashboard',   'dashboard',   '/v2/dashboard',   'MAIN SIDEBAR', 'Dashboard',  'bi-speedometer2',
      1, 10,  1, 0, 0, 0, 0, 0,  '2026-04-11 00:00:00', '2026-04-11 00:00:00'),

    ( 2, 'Students',    'students',    '/v2/students',    'PEOPLE',       'Students',   'bi-mortarboard',
      1, 20,  1, 1, 1, 1, 1, 1,  '2026-04-11 00:00:00', '2026-04-11 00:00:00'),

    ( 3, 'Staff',       'staff',       '/v2/staff',       'PEOPLE',       'Staff',      'bi-person-badge',
      1, 30,  1, 1, 1, 1, 1, 1,  '2026-04-11 00:00:00', '2026-04-11 00:00:00'),

    ( 4, 'Classes',     'classes',     '/v2/classes',     'ACADEMICS',    'Classes',    'bi-journal-bookmark',
      1, 40,  1, 1, 1, 1, 0, 1,  '2026-04-11 00:00:00', '2026-04-11 00:00:00'),

    ( 5, 'Attendance',  'attendance',  '/v2/attendance',  'ATTENDANCE',   'Attendance', 'bi-calendar-check',
      1, 50,  1, 1, 1, 0, 0, 1,  '2026-04-11 00:00:00', '2026-04-11 00:00:00'),

    ( 6, 'Audit Log',   'auditlog',    '/v2/auditlog',    'CONFIGURATION','Audit Log',  'bi-journal-text',
      1, 60,  1, 0, 0, 0, 0, 1,  '2026-04-11 00:00:00', '2026-04-11 00:00:00'),

    ( 7, 'Settings',    'settings',    '/v2/settings',    'SETTINGS',     'Settings',   'bi-gear',
      1, 70,  1, 1, 0, 0, 0, 0,  '2026-04-11 00:00:00', '2026-04-11 00:00:00'),

    ( 8, 'Users',       'users',       '/v2/users',       'CONFIGURATION','Users',      'bi-people',
      1, 80,  1, 1, 1, 1, 0, 0,  '2026-04-11 00:00:00', '2026-04-11 00:00:00'),

    ( 9, 'Pages',       'pages',       '/v2/pages',       'CONFIGURATION','Pages',      'bi-file-earmark-text',
      1, 85,  1, 1, 1, 1, 0, 0,  '2026-04-11 00:00:00', '2026-04-11 00:00:00'),

    -- ── Dashboard sub-pages (ids 10–12) ───────────────────────────────────
    (10, 'Dashboard Overview',     'dashboard-overview',     '/v2/dashboard/overview',
      'MAIN SIDEBAR', 'Dashboard', 'bi-bar-chart',
      1, 11,  1, 0, 0, 0, 0, 0,  '2026-04-11 00:00:00', '2026-04-11 00:00:00'),

    (11, 'Dashboard Trends',       'dashboard-trends',       '/v2/dashboard/trends',
      'MAIN SIDEBAR', 'Dashboard', 'bi-graph-up',
      1, 12,  1, 0, 0, 0, 0, 0,  '2026-04-11 00:00:00', '2026-04-11 00:00:00'),

    (12, 'Dashboard Safeguarding', 'dashboard-safeguarding', '/v2/dashboard/safeguarding',
      'MAIN SIDEBAR', 'Dashboard', 'bi-shield-check',
      1, 13,  1, 0, 0, 0, 0, 0,  '2026-04-11 00:00:00', '2026-04-11 00:00:00'),

    -- ── Settings sub-pages (ids 13–14) ────────────────────────────────────
    (13, 'Diagnostics', 'diagnostics', '/v2/diagnostics', 'SETTINGS', 'Diagnostics', 'bi-activity',
      1, 75,  1, 0, 0, 0, 0, 0,  '2026-04-11 00:00:00', '2026-04-11 00:00:00'),

    (14, 'Site',        'site',        '/v2/site',        'SETTINGS', 'Site',        'bi-globe',
      1, 76,  1, 0, 0, 0, 0, 0,  '2026-04-11 00:00:00', '2026-04-11 00:00:00');


-- ===========================================================================
-- STEP 6 — Seed role_default_page_permissions (5 roles × 14 pages)
-- ---------------------------------------------------------------------------
-- Each role receives a curated set of defaults appropriate to its scope.
-- Permission flags are constrained to the page's SupportsXxx capabilities.
--
-- Role scope summary:
--   super_admin  — full access to all 14 pages
--   admin        — full data access; read-only on admin config pages
--   staff_admin  — people + academic management; no config/system pages
--   teacher      — read + attendance write; no admin pages
--   office_staff — read-only people; attendance write; no admin pages
--
-- CanRead=0 for a page removes it from the sidebar for that role.
-- Roles not given a row for a page fall through to Deny (PermissionServiceV2
-- Priority 3: no role default → all-false EffectivePermissionDto).
-- ===========================================================================

INSERT IGNORE INTO role_default_page_permissions
    (RoleTypeName, PageId, CanRead, CanWrite, CanCreate, CanDelete, CanImport, CanExport)
VALUES

    -- ── super_admin — full access to everything ───────────────────────────
    -- Flags match each page's SupportsXxx capabilities exactly.
    ('super_admin',  1, 1, 0, 0, 0, 0, 0),   -- Dashboard       (R)
    ('super_admin',  2, 1, 1, 1, 1, 1, 1),   -- Students        (R,W,C,D,I,E)
    ('super_admin',  3, 1, 1, 1, 1, 1, 1),   -- Staff           (R,W,C,D,I,E)
    ('super_admin',  4, 1, 1, 1, 1, 0, 1),   -- Classes         (R,W,C,D,E)
    ('super_admin',  5, 1, 1, 1, 0, 0, 1),   -- Attendance      (R,W,C,E)
    ('super_admin',  6, 1, 0, 0, 0, 0, 1),   -- Audit Log       (R,E)
    ('super_admin',  7, 1, 1, 0, 0, 0, 0),   -- Settings        (R,W)
    ('super_admin',  8, 1, 1, 1, 1, 0, 0),   -- Users           (R,W,C,D)
    ('super_admin',  9, 1, 1, 1, 1, 0, 0),   -- Pages           (R,W,C,D)
    ('super_admin', 10, 1, 0, 0, 0, 0, 0),   -- Dashboard Overview
    ('super_admin', 11, 1, 0, 0, 0, 0, 0),   -- Dashboard Trends
    ('super_admin', 12, 1, 0, 0, 0, 0, 0),   -- Dashboard Safeguarding
    ('super_admin', 13, 1, 0, 0, 0, 0, 0),   -- Diagnostics
    ('super_admin', 14, 1, 0, 0, 0, 0, 0),   -- Site

    -- ── admin — full data access; read-only on configuration/system pages ──
    ('admin',  1, 1, 0, 0, 0, 0, 0),   -- Dashboard
    ('admin',  2, 1, 1, 1, 1, 1, 1),   -- Students
    ('admin',  3, 1, 1, 1, 1, 1, 1),   -- Staff
    ('admin',  4, 1, 1, 1, 1, 0, 1),   -- Classes
    ('admin',  5, 1, 1, 1, 0, 0, 1),   -- Attendance
    ('admin',  6, 1, 0, 0, 0, 0, 1),   -- Audit Log (read + export only)
    ('admin',  7, 1, 0, 0, 0, 0, 0),   -- Settings  (read-only)
    ('admin',  8, 1, 0, 0, 0, 0, 0),   -- Users     (read-only; writes are super_admin)
    ('admin',  9, 1, 0, 0, 0, 0, 0),   -- Pages     (read-only)
    ('admin', 10, 1, 0, 0, 0, 0, 0),
    ('admin', 11, 1, 0, 0, 0, 0, 0),
    ('admin', 12, 1, 0, 0, 0, 0, 0),
    ('admin', 13, 0, 0, 0, 0, 0, 0),   -- Diagnostics — hidden from admin
    ('admin', 14, 0, 0, 0, 0, 0, 0),   -- Site        — hidden from admin

    -- ── staff_admin — people management; no system config ─────────────────
    ('staff_admin',  1, 1, 0, 0, 0, 0, 0),   -- Dashboard
    ('staff_admin',  2, 1, 1, 1, 0, 0, 0),   -- Students  (R,W,C — no delete/import)
    ('staff_admin',  3, 1, 1, 1, 1, 1, 1),   -- Staff     (full)
    ('staff_admin',  4, 1, 1, 0, 0, 0, 1),   -- Classes   (R,W,E)
    ('staff_admin',  5, 1, 1, 1, 0, 0, 1),   -- Attendance
    ('staff_admin',  6, 0, 0, 0, 0, 0, 0),   -- Audit Log — hidden
    ('staff_admin',  7, 0, 0, 0, 0, 0, 0),   -- Settings  — hidden
    ('staff_admin',  8, 0, 0, 0, 0, 0, 0),   -- Users     — hidden
    ('staff_admin',  9, 0, 0, 0, 0, 0, 0),   -- Pages     — hidden
    ('staff_admin', 10, 1, 0, 0, 0, 0, 0),
    ('staff_admin', 11, 1, 0, 0, 0, 0, 0),
    ('staff_admin', 12, 1, 0, 0, 0, 0, 0),
    ('staff_admin', 13, 0, 0, 0, 0, 0, 0),
    ('staff_admin', 14, 0, 0, 0, 0, 0, 0),

    -- ── teacher — view-only + attendance write ────────────────────────────
    ('teacher',  1, 1, 0, 0, 0, 0, 0),   -- Dashboard
    ('teacher',  2, 1, 0, 0, 0, 0, 0),   -- Students  (read-only)
    ('teacher',  3, 0, 0, 0, 0, 0, 0),   -- Staff     — hidden
    ('teacher',  4, 1, 0, 0, 0, 0, 0),   -- Classes   (read-only)
    ('teacher',  5, 1, 1, 1, 0, 0, 0),   -- Attendance (R,W,C)
    ('teacher',  6, 0, 0, 0, 0, 0, 0),   -- Audit Log — hidden
    ('teacher',  7, 0, 0, 0, 0, 0, 0),   -- Settings  — hidden
    ('teacher',  8, 0, 0, 0, 0, 0, 0),   -- Users     — hidden
    ('teacher',  9, 0, 0, 0, 0, 0, 0),   -- Pages     — hidden
    ('teacher', 10, 1, 0, 0, 0, 0, 0),
    ('teacher', 11, 1, 0, 0, 0, 0, 0),
    ('teacher', 12, 1, 0, 0, 0, 0, 0),
    ('teacher', 13, 0, 0, 0, 0, 0, 0),
    ('teacher', 14, 0, 0, 0, 0, 0, 0),

    -- ── office_staff — administrative view + attendance ───────────────────
    ('office_staff',  1, 1, 0, 0, 0, 0, 0),   -- Dashboard
    ('office_staff',  2, 1, 0, 0, 0, 0, 0),   -- Students (read-only)
    ('office_staff',  3, 1, 0, 0, 0, 0, 0),   -- Staff    (read-only)
    ('office_staff',  4, 1, 0, 0, 0, 0, 0),   -- Classes  (read-only)
    ('office_staff',  5, 1, 1, 1, 0, 0, 1),   -- Attendance (R,W,C,E)
    ('office_staff',  6, 0, 0, 0, 0, 0, 0),   -- Audit Log — hidden
    ('office_staff',  7, 0, 0, 0, 0, 0, 0),   -- Settings  — hidden
    ('office_staff',  8, 0, 0, 0, 0, 0, 0),   -- Users     — hidden
    ('office_staff',  9, 0, 0, 0, 0, 0, 0),   -- Pages     — hidden
    ('office_staff', 10, 1, 0, 0, 0, 0, 0),
    ('office_staff', 11, 1, 0, 0, 0, 0, 0),
    ('office_staff', 12, 1, 0, 0, 0, 0, 0),
    ('office_staff', 13, 0, 0, 0, 0, 0, 0),
    ('office_staff', 14, 0, 0, 0, 0, 0, 0);


-- ===========================================================================
-- STEP 7 — Verification queries
-- ---------------------------------------------------------------------------
-- Run these after the script to confirm correct row counts.
-- ===========================================================================

SELECT 'app_pages'                     AS table_name, COUNT(*) AS row_count FROM app_pages                     UNION ALL
SELECT 'role_default_page_permissions' AS table_name, COUNT(*) AS row_count FROM role_default_page_permissions UNION ALL
SELECT 'user_page_permissions'         AS table_name, COUNT(*) AS row_count FROM user_page_permissions         UNION ALL
SELECT 'user_page_overrides'           AS table_name, COUNT(*) AS row_count FROM user_page_overrides;

-- Expected:
--   app_pages                     : 14
--   role_default_page_permissions : 70  (14 pages × 5 roles)
--   user_page_permissions         :  0  (no user-specific overrides seeded)
--   user_page_overrides           :  0  (no grant/deny overrides seeded)

SELECT 'user_profiles' AS table_name,
       TABLE_ROWS      AS approx_rows
FROM information_schema.TABLES
WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'user_profiles';
