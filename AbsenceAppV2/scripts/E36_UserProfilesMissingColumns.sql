-- =============================================================================
-- Script      : E36_UserProfilesMissingColumns.sql
-- Author      : Michael
-- Version     : 1.0.0
-- Created     : 2026-04-25
-- Updated     : 2026-04-25
-- -----------------------------------------------------------------------------
-- Purpose     : Adds the two columns that were present in the EF entity
--               (UserProfile v1.1.0+) but were never applied to the DB:
--                 • DateOfBirth       DATE NULL
--                 • ProfilePictureUrl VARCHAR(500) NULL
--
--               The DB table is `userprofiles` — MySQL on Windows stores all
--               table names in lowercase. E33 Step 0 attempted to RENAME
--               UserProfiles TO user_profiles but the information_schema check
--               used case-sensitive matching which returned 0 rows on Windows
--               MySQL, so the rename never executed. The table remains
--               `userprofiles`.
--               After the code fix (AppDbContext v2.0.2 removes the
--               ToTable("UserProfiles") override; UserProfile v1.4.0 uses
--               [Table("userprofiles")]) EF correctly targets `userprofiles`.
--               This script brings the DB schema in line with the EF model.
-- -----------------------------------------------------------------------------
-- Prerequisites:
--   • The `userprofiles` table must exist (created during initial DB setup).
--     E33 Step 0 rename is a no-op on Windows MySQL — table is already named
--     `userprofiles`.
-- -----------------------------------------------------------------------------
-- Idempotent  : Yes — uses ADD COLUMN IF NOT EXISTS; safe to re-run.
-- =============================================================================

USE absenceapp;

-- -----------------------------------------------------------------------------
-- Step 1 — Add DateOfBirth if not already present
-- -----------------------------------------------------------------------------
ALTER TABLE `userprofiles`
    ADD COLUMN IF NOT EXISTS `DateOfBirth` DATE NULL DEFAULT NULL;

-- -----------------------------------------------------------------------------
-- Step 2 — Add ProfilePictureUrl if not already present
-- -----------------------------------------------------------------------------
ALTER TABLE `userprofiles`
    ADD COLUMN IF NOT EXISTS `ProfilePictureUrl` VARCHAR(500) NULL DEFAULT NULL;
