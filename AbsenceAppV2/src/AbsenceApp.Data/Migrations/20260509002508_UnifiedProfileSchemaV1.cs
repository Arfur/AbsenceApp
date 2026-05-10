using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AbsenceApp.Data.Migrations
{
    /// <summary>
    /// Deterministic, non-destructive schema sync migration for unified profile pages.
    /// This migration uses idempotent SQL (IF [NOT] EXISTS) and does not drop/rename
    /// existing tables or columns.
    /// </summary>
    public partial class UnifiedProfileSchemaV1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // -----------------------------------------------------------------
            // Phase A verification/safety: ensure required staff columns exist.
            // -----------------------------------------------------------------
            migrationBuilder.Sql("ALTER TABLE staff ADD COLUMN IF NOT EXISTS Title VARCHAR(20) NOT NULL DEFAULT ''; ");
            migrationBuilder.Sql("ALTER TABLE staff ADD COLUMN IF NOT EXISTS PreferredName VARCHAR(50) NULL; ");
            migrationBuilder.Sql("ALTER TABLE staff ADD COLUMN IF NOT EXISTS Gender VARCHAR(20) NULL; ");
            migrationBuilder.Sql("ALTER TABLE staff ADD COLUMN IF NOT EXISTS PhoneHome VARCHAR(20) NULL; ");
            migrationBuilder.Sql("ALTER TABLE staff ADD COLUMN IF NOT EXISTS PhoneEmergency VARCHAR(20) NULL; ");
            migrationBuilder.Sql("ALTER TABLE staff ADD COLUMN IF NOT EXISTS AltEmail VARCHAR(100) NULL; ");
            migrationBuilder.Sql("ALTER TABLE staff ADD COLUMN IF NOT EXISTS EndDate DATE NULL; ");
            migrationBuilder.Sql("ALTER TABLE staff ADD COLUMN IF NOT EXISTS ReportingManagerId INT NULL; ");

            // Optional dual-link support for installations that still use student_id-only absences.
            migrationBuilder.Sql("ALTER TABLE absences ADD COLUMN IF NOT EXISTS StaffId BIGINT NULL; ");

            // -----------------------------------------------------------------
            // Phase B: lookup + notes + user supplementary tables.
            // -----------------------------------------------------------------
            migrationBuilder.Sql(@"
CREATE TABLE IF NOT EXISTS jobgroups (
    Id          INT            NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Name        VARCHAR(100)   NOT NULL,
    Code        VARCHAR(20)    NULL,
    Description TEXT           NULL,
    CreatedAt   DATETIME(6)    NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    UpdatedAt   DATETIME(6)    NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6)
);");

            migrationBuilder.Sql(@"
CREATE TABLE IF NOT EXISTS jobtitles (
    Id          INT            NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Title       VARCHAR(150)   NULL,
    Code        VARCHAR(20)    NULL,
    Description TEXT           NULL,
    CreatedAt   DATETIME(6)    NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    UpdatedAt   DATETIME(6)    NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6)
);");

            migrationBuilder.Sql(@"
CREATE TABLE IF NOT EXISTS staffdepartments (
    Id          INT            NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Name        VARCHAR(150)   NOT NULL,
    Code        VARCHAR(20)    NOT NULL,
    Description TEXT           NOT NULL DEFAULT '',
    HeadUserId  INT            NULL,
    Status      VARCHAR(20)    NOT NULL DEFAULT 'active',
    CreatedAt   DATETIME(6)    NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    UpdatedAt   DATETIME(6)    NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6)
);");

            migrationBuilder.Sql(@"
CREATE TABLE IF NOT EXISTS studentnotes (
    Id         BIGINT        NOT NULL AUTO_INCREMENT PRIMARY KEY,
    StudentId  INT           NOT NULL,
    NoteType   VARCHAR(50)   NOT NULL DEFAULT 'General',
    Body       TEXT          NOT NULL,
    CreatedBy  INT           NOT NULL DEFAULT 0,
    CreatedAt  DATETIME(6)   NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    UpdatedAt  DATETIME(6)   NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    INDEX idx_studentnotes_student (StudentId)
);");

            migrationBuilder.Sql(@"
CREATE TABLE IF NOT EXISTS staffnotes (
    Id        BIGINT        NOT NULL AUTO_INCREMENT PRIMARY KEY,
    StaffId   INT           NOT NULL,
    NoteType  VARCHAR(50)   NOT NULL DEFAULT 'General',
    Body      TEXT          NOT NULL,
    CreatedBy INT           NOT NULL DEFAULT 0,
    CreatedAt DATETIME(6)   NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    UpdatedAt DATETIME(6)   NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    INDEX idx_staffnotes_staff (StaffId)
);");

            migrationBuilder.Sql(@"
CREATE TABLE IF NOT EXISTS usercontacts (
    Id             INT           NOT NULL AUTO_INCREMENT PRIMARY KEY,
    UserId         INT           NOT NULL,
    ContactName    VARCHAR(150)  NOT NULL,
    Relationship   VARCHAR(100)  NOT NULL DEFAULT 'Other',
    Phone          VARCHAR(50)   NULL,
    Email          VARCHAR(150)  NULL,
    IsPrimary      TINYINT(1)    NOT NULL DEFAULT 0,
    CreatedAt      DATETIME(6)   NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    UpdatedAt      DATETIME(6)   NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    INDEX idx_usercontacts_user (UserId)
);");

            migrationBuilder.Sql(@"
CREATE TABLE IF NOT EXISTS userdevices (
    Id           INT           NOT NULL AUTO_INCREMENT PRIMARY KEY,
    UserId       INT           NOT NULL,
    DeviceType   VARCHAR(100)  NOT NULL,
    SerialNumber VARCHAR(100)  NULL,
    AssignedDate DATE          NOT NULL,
    ReturnedDate DATE          NULL,
    Notes        TEXT          NULL,
    CreatedAt    DATETIME(6)   NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    UpdatedAt    DATETIME(6)   NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    INDEX idx_userdevices_user (UserId)
);");

            migrationBuilder.Sql(@"
CREATE TABLE IF NOT EXISTS userexternalaccounts (
    Id              INT           NOT NULL AUTO_INCREMENT PRIMARY KEY,
    UserId          INT           NOT NULL,
    SystemId        INT           NOT NULL DEFAULT 0,
    SystemName      VARCHAR(150)  NOT NULL DEFAULT '',
    SystemCode      VARCHAR(50)   NOT NULL DEFAULT '',
    AccountUsername VARCHAR(150)  NULL,
    AccountEmail    VARCHAR(150)  NULL,
    Status          VARCHAR(20)   NOT NULL DEFAULT 'active',
    CreatedAt       DATETIME(6)   NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    UpdatedAt       DATETIME(6)   NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    INDEX idx_userexternal_user (UserId)
);");

            migrationBuilder.Sql(@"
CREATE TABLE IF NOT EXISTS userpermissionoverrides (
    Id         INT           NOT NULL AUTO_INCREMENT PRIMARY KEY,
    UserId     INT           NOT NULL,
    PageId     INT           NOT NULL,
    CanRead    TINYINT(1)    NOT NULL DEFAULT 0,
    CanWrite   TINYINT(1)    NOT NULL DEFAULT 0,
    CanCreate  TINYINT(1)    NOT NULL DEFAULT 0,
    CanDelete  TINYINT(1)    NOT NULL DEFAULT 0,
    CanImport  TINYINT(1)    NOT NULL DEFAULT 0,
    CanExport  TINYINT(1)    NOT NULL DEFAULT 0,
    CreatedAt  DATETIME(6)   NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    UpdatedAt  DATETIME(6)   NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    UNIQUE KEY uk_userperm_user_page (UserId, PageId)
);");

            migrationBuilder.Sql(@"
CREATE TABLE IF NOT EXISTS usernotes (
    Id        BIGINT        NOT NULL AUTO_INCREMENT PRIMARY KEY,
    UserId    INT           NOT NULL,
    NoteType  VARCHAR(50)   NOT NULL DEFAULT 'General',
    Body      TEXT          NOT NULL,
    CreatedBy INT           NOT NULL DEFAULT 0,
    CreatedAt DATETIME(6)   NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    UpdatedAt DATETIME(6)   NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    INDEX idx_usernotes_user (UserId)
);");

            migrationBuilder.Sql(@"
CREATE TABLE IF NOT EXISTS loginaudits (
    Id            INT            NOT NULL AUTO_INCREMENT PRIMARY KEY,
    UserId        INT            NULL,
    LoginAt       DATETIME       NOT NULL,
    LoginIp       VARCHAR(45)    NULL,
    UserAgent     TEXT           NULL,
    WasSuccessful TINYINT(1)     NOT NULL DEFAULT 0,
    FailureReason TEXT           NULL
);");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Keep Down non-destructive intentionally for safety in shared environments.
        }
    }
}
