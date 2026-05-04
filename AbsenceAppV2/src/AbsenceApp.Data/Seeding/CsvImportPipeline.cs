/*
===============================================================================
 File        : CsvImportPipeline.cs
 Namespace   : AbsenceApp.Data.Seeding
 Author      : Michael
 Version     : 1.1.0
 Created     : 2026-03-15
 Updated     : 2026-05-04
-------------------------------------------------------------------------------
 Purpose     : Deterministic CSV-to-EF import pipeline for all 40 SQL tables.
               Reads CSV files from a directory and upserts rows via AppDbContext
               using explicit, fixed column-to-property mappings per table.
               Importers run in FK-dependency order (Phase 1 through 5).
-------------------------------------------------------------------------------
 Description :
   RunAllAsync(dir) iterates all 34 importers in dependency order.
   Each private ImportXxxAsync method encodes one table's column mapping.
   UpsertAsync<T> uses EF EntityState to INSERT new rows and UPDATE existing
   rows identified by their Id, then calls SaveChangesAsync per batch.
   ReadCsvAsync parses RFC 4180 CSV (quoted fields, embedded commas).
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-15  Initial creation.
   - 1.1.0  2026-05-04  Fix Plan #2 Step 6: removed (ulong) cast from
                         AbsenceType seed row: Id = (ulong)ToInt(...) →
                         Id = ToInt(...). ToInt returns int; AbsenceType.Id
                         is long. int implicitly converts to long; no cast needed.
-------------------------------------------------------------------------------
 Notes       :
   - SQL Server: configure ValueGeneratedNever() for identity columns, or
     enable IDENTITY_INSERT, before running against a real SQL Server instance.
   - Missing CSV files are silently skipped; empty files produce no rows.
===============================================================================
*/

using System.Globalization;
using System.Linq.Expressions;
using AbsenceApp.Data.Context;
using AbsenceApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AbsenceApp.Data.Seeding;

/// <summary>
/// Reads CSV files from a directory and upserts rows into the database via
/// AppDbContext.  Column-to-property mappings are explicit and fixed.
///
/// Notes on SQL Server identity columns:
///   The pipeline inserts rows with explicit IDs from the CSV files.  For SQL
///   Server tables with IDENTITY columns, configure each entity with
///   .HasKey(...).ValueGeneratedNever() (or enable IDENTITY_INSERT per table)
///   before running this pipeline against a real SQL Server database.
///   When used with the EF Core InMemory provider (e.g. in tests) this is a
///   non-issue.
/// </summary>
public sealed class CsvImportPipeline
{
    private readonly AppDbContext _db;

    public CsvImportPipeline(AppDbContext db) => _db = db;

    // =========================================================================
    // Public entry point
    // =========================================================================

    /// <summary>
    /// Imports every CSV that has a matching entity, in FK-dependency order.
    /// </summary>
    public async Task RunAllAsync(string csvDirectory)
    {
        // ── Phase 1: standalone lookup tables (no FK dependencies) ────────────
        await ImportSchoolsAsync(csvDirectory);
        await ImportPhasesAsync(csvDirectory);
        await ImportRoleTypesAsync(csvDirectory);
        await ImportResponsibilitiesAsync(csvDirectory);
        await ImportAbsenceTypesAsync(csvDirectory);
        await ImportDeviceTypesAsync(csvDirectory);
        await ImportExternalSystemsAsync(csvDirectory);
        await ImportSystemEventsAsync(csvDirectory);

        // ── Phase 2: school-scoped structure ──────────────────────────────────
        await ImportYearGroupsAsync(csvDirectory);
        await ImportHousesAsync(csvDirectory);

        // ── Phase 3: core people tables ───────────────────────────────────────
        await ImportUsersAsync(csvDirectory);
        await ImportUserProfilesAsync(csvDirectory);
        await ImportStaffAsync(csvDirectory);
        await ImportStudentsAsync(csvDirectory);

        // ── Phase 4: extension / link tables ─────────────────────────────────
        await ImportClassYearGroupAsync(csvDirectory);
        await ImportStaffAbsencesAsync(csvDirectory);
        await ImportStaffDevicesAsync(csvDirectory);
        await ImportStaffExternalAccountsAsync(csvDirectory);
        await ImportStudentAbsencesAsync(csvDirectory);
        await ImportStudentContactsAsync(csvDirectory);
        await ImportStudentMedicalAsync(csvDirectory);
        await ImportStudentFlagsAsync(csvDirectory);

        // ── Phase 5: audit tables ─────────────────────────────────────────────
        await ImportLoginAuditAsync(csvDirectory);
        await ImportAccountVerificationEventsAsync(csvDirectory);
        await ImportRoleChangeAuditAsync(csvDirectory);
        await ImportStaffAbsenceAuditAsync(csvDirectory);
        await ImportStaffDeviceAuditAsync(csvDirectory);
        await ImportStaffExternalAccountAuditAsync(csvDirectory);
        await ImportStudentAbsenceAuditAsync(csvDirectory);
    }

    // =========================================================================
    // Phase 1 — lookup tables
    // =========================================================================

    // CSV columns: id, name, code, school_ref, created_at, updated_at
    // Entity column notes:
    //   status     → not in CSV; defaulted to "active"
    //   description → not in CSV; nullable — left null
    private async Task ImportSchoolsAsync(string dir)
    {
        var rows = await ReadCsvAsync(Path.Combine(dir, "schools.csv"));
        var entities = rows.Select(r => new School
        {
            Id        = ToInt(r, "id"),
            Name      = Col(r, "name"),
            Code      = Col(r, "code"),
            SchoolRef = Col(r, "school_ref"),
            Status    = "active",
            CreatedAt = ToDateTime(r, "created_at"),
            UpdatedAt = ToDateTime(r, "updated_at"),
        }).ToList();
        await UpsertAsync(_db.Schools, s => s.Id, entities);
    }

    // CSV columns: id, name, created_at, updated_at
    // Entity column notes:
    //   code         → not in CSV; set to name value
    //   numeric_order → not in CSV; defaulted to 0
    //   school_id    → not in CSV; defaulted to 1
    private async Task ImportPhasesAsync(string dir)
    {
        var rows = await ReadCsvAsync(Path.Combine(dir, "phases.csv"));
        var entities = rows.Select(r => new Phase
        {
            Id           = ToInt(r, "id"),
            Name         = Col(r, "name"),
            Code         = Col(r, "name"),
            NumericOrder = 0,
            SchoolId     = 1,
            CreatedAt    = ToDateTime(r, "created_at"),
            UpdatedAt    = ToDateTime(r, "updated_at"),
        }).ToList();
        await UpsertAsync(_db.Phases, p => p.Id, entities);
    }

    // CSV columns: id, name, display_name, description, is_system_role, is_default,
    //              priority, created_at, updated_at
    private async Task ImportRoleTypesAsync(string dir)
    {
        var rows = await ReadCsvAsync(Path.Combine(dir, "role_types.csv"));
        var entities = rows.Select(r => new RoleType
        {
            Id           = ToInt(r, "id"),
            Name         = Col(r, "name"),
            DisplayName  = Col(r, "display_name"),
            Description  = NullableCol(r, "description"),
            IsSystemRole = ToBool(r, "is_system_role"),
            IsDefault    = ToBool(r, "is_default"),
            Priority     = ToInt(r, "priority"),
            CreatedAt    = ToDateTime(r, "created_at"),
            UpdatedAt    = ToDateTime(r, "updated_at"),
        }).ToList();
        await UpsertAsync(_db.RoleTypes, rt => rt.Id, entities);
    }

    // CSV columns: id, name, code, description
    private async Task ImportResponsibilitiesAsync(string dir)
    {
        var rows = await ReadCsvAsync(Path.Combine(dir, "responsibilities.csv"));
        var entities = rows.Select(r => new ResponsibilityType
        {
            Id          = ToInt(r, "id"),
            Name        = Col(r, "name"),
            Code        = Col(r, "code"),
            Description = NullableCol(r, "description"),
        }).ToList();
        await UpsertAsync(_db.ResponsibilityTypes, r => r.Id, entities);
    }

    // CSV columns (new schema): id, code, name, category, is_authorised, created_at
    private async Task ImportAbsenceTypesAsync(string dir)
    {
        var rows = await ReadCsvAsync(Path.Combine(dir, "absence_types.csv"));
        var entities = rows.Select(r => new AbsenceType
        {
            Id           = ToInt(r, "id"),
            Code         = Col(r, "code"),
            Name         = Col(r, "name"),
            Category     = Col(r, "category"),
            IsAuthorised = ToBool(r, "is_authorised"),
        }).ToList();
        await UpsertAsync(_db.AbsenceTypes, at => at.Id, entities);
    }

    // CSV columns: id, name, code, description, created_at, updated_at
    private async Task ImportDeviceTypesAsync(string dir)
    {
        var rows = await ReadCsvAsync(Path.Combine(dir, "device_types.csv"));
        var entities = rows.Select(r => new DeviceType
        {
            Id          = ToInt(r, "id"),
            Name        = Col(r, "name"),
            Code        = Col(r, "code"),
            Description = NullableCol(r, "description"),
            CreatedAt   = ToDateTime(r, "created_at"),
            UpdatedAt   = ToDateTime(r, "updated_at"),
        }).ToList();
        await UpsertAsync(_db.DeviceTypes, d => d.Id, entities);
    }

    // CSV columns: id, name, code, description, created_at, updated_at
    private async Task ImportExternalSystemsAsync(string dir)
    {
        var rows = await ReadCsvAsync(Path.Combine(dir, "external_systems.csv"));
        var entities = rows.Select(r => new ExternalSystem
        {
            Id          = ToInt(r, "id"),
            Name        = Col(r, "name"),
            Code        = Col(r, "code"),
            Description = NullableCol(r, "description"),
        }).ToList();
        await UpsertAsync(_db.ExternalSystems, e => e.Id, entities);
    }

    // CSV columns: id, event_type, event_time, triggered_by, description, metadata
    private async Task ImportSystemEventsAsync(string dir)
    {
        var rows = await ReadCsvAsync(Path.Combine(dir, "system_events.csv"));
        var entities = rows.Select(r => new SystemEvent
        {
            Id          = ToInt(r, "id"),
            EventType   = Col(r, "event_type"),
            EventTime   = ToDateTime(r, "event_time"),
            TriggeredBy = Col(r, "triggered_by"),
            Description = NullableCol(r, "description"),
            Metadata    = NullableCol(r, "metadata"),
        }).ToList();
        await UpsertAsync(_db.SystemEvents, e => e.Id, entities);
    }

    // =========================================================================
    // Phase 2 — school-scoped structure
    // =========================================================================

    // CSV columns: id, name, created_at, updated_at
    // Entity column notes:
    //   code         → not in CSV; set to name value
    //   numeric_value → not in CSV; defaulted to 0
    //   phase_id     → not in CSV; defaulted to 1
    //   school_id    → not in CSV; defaulted to 1
    private async Task ImportYearGroupsAsync(string dir)
    {
        var rows = await ReadCsvAsync(Path.Combine(dir, "year_groups.csv"));
        var entities = rows.Select(r => new YearGroup
        {
            Id           = ToInt(r, "id"),
            Name         = Col(r, "name"),
            Code         = Col(r, "name"),
            NumericValue = null,
            PhaseId      = 1,
            DisplayOrder = 0,
            CreatedAt    = ToDateTime(r, "created_at"),
            UpdatedAt    = ToDateTime(r, "updated_at"),
        }).ToList();
        await UpsertAsync(_db.YearGroups, y => y.Id, entities);
    }

    // CSV columns: id, name, created_at, updated_at
    // Entity column notes:
    //   colour   → not in CSV; defaulted to empty string
    //   code     → not in CSV; set to name value
    //   school_id → not in CSV; defaulted to 1
    private async Task ImportHousesAsync(string dir)
    {
        var rows = await ReadCsvAsync(Path.Combine(dir, "houses.csv"));
        var entities = rows.Select(r => new House
        {
            Id        = ToInt(r, "id"),
            Name      = Col(r, "name"),
            Code      = Col(r, "name"),
            CreatedAt = ToDateTime(r, "created_at"),
            UpdatedAt = ToDateTime(r, "updated_at"),
        }).ToList();
        await UpsertAsync(_db.Houses, h => h.Id, entities);
    }

    // =========================================================================
    // Phase 3 — core people tables
    // =========================================================================

    // CSV columns: id, name, username, email, email_verified_at, password,
    //              role_type_id, status, is_admin, profile_photo_path,
    //              phone_number, last_login_at, last_login_ip, remember_token,
    //              created_at, updated_at, department_id, designation,
    //              login_count, is_two_factor_enabled, two_factor_secret,
    //              backup_codes, timezone, language_code, bio, date_of_birth,
    //              gender, address, city, country, postal_code
    // (The source users.csv may be empty; this importer handles that gracefully.)
    private async Task ImportUsersAsync(string dir)
    {
        var rows = await ReadCsvAsync(Path.Combine(dir, "users.csv"));
        var entities = rows.Select(r => new User
        {
            Id                 = ToInt(r, "id"),
            Username           = Col(r, "username"),
            Email              = Col(r, "email"),
            EmailVerifiedAt    = NullableDateTime(r, "email_verified_at"),
            Password           = Col(r, "password"),
            Status             = Col(r, "status"),
            IsAdmin            = ToBool(r, "is_admin"),
            LastLoginAt        = NullableDateTime(r, "last_login_at"),
            LastLoginIp        = NullableCol(r, "last_login_ip"),
            RememberToken      = NullableCol(r, "remember_token"),
            CreatedAt          = ToDateTime(r, "created_at"),
            UpdatedAt          = ToDateTime(r, "updated_at"),
            LoginCount         = ToInt(r, "login_count"),
            IsTwoFactorEnabled = ToBool(r, "is_two_factor_enabled"),
            TwoFactorSecret    = NullableCol(r, "two_factor_secret"),
            BackupCodes        = NullableCol(r, "backup_codes"),
            Timezone           = NullableCol(r, "timezone"),
            LanguageCode       = NullableCol(r, "language_code"),
        }).ToList();
        await UpsertAsync(_db.Users, u => u.Id, entities);
    }

    // CSV columns: id, user_id, first_name, last_name, preferred_name, title,
    //              date_of_birth, profile_picture_url, bio, gender, timezone,
    //              language_code, department_id, job_title_id, school_id,
    //              created_at, updated_at
    private async Task ImportUserProfilesAsync(string dir)
    {
        var rows = await ReadCsvAsync(Path.Combine(dir, "userprofiles.csv"));
        var entities = rows.Select(r => new UserProfile
        {
            Id                = ToInt(r, "id"),
            UserId            = ToInt(r, "user_id"),
            DisplayName       = Col(r, "first_name") + " " + Col(r, "last_name"),
            ThemePreference   = "default",
            ProfilePictureUrl = NullableCol(r, "profile_picture_url"),
            Bio               = NullableCol(r, "bio"),
            Timezone          = Col(r, "timezone"),
            LanguageCode      = Col(r, "language_code"),
            CreatedAt         = ToDateTime(r, "created_at"),
            UpdatedAt         = ToDateTime(r, "updated_at"),
        }).ToList();
        await UpsertAsync(_db.UserProfiles, u => u.Id, entities);
    }

    // CSV columns: id, first_name, last_name, job_title (text—not an ID),
    //              email, school_id (text ref code—not numeric), created_at, updated_at
    // Entity column notes:
    //   staff_number      → not in CSV; set to id string as placeholder
    //   title             → not in CSV; defaulted to empty string
    //   date_of_birth     → not in CSV; defaulted to DateOnly.MinValue
    //   employment_type   → not in CSV; defaulted to "Unknown"
    //   contract_type     → not in CSV; defaulted to "Unknown"
    //   hire_date         → not in CSV; defaulted to DateOnly.MinValue
    //   work_location     → not in CSV; defaulted to empty string
    //   job_title_id      → not in CSV (only text name present); defaulted to 1
    //   job_group_id      → not in CSV; defaulted to 1
    //   department_id     → not in CSV; defaulted to 1
    //   account_status    → not in CSV; defaulted to "active"
    private async Task ImportStaffAsync(string dir)
    {
        var rows = await ReadCsvAsync(Path.Combine(dir, "staff.csv"));
        var entities = rows.Select(r => new Staff
        {
            Id               = ToInt(r, "id"),
            FirstName        = Col(r, "first_name"),
            LastName         = Col(r, "last_name"),
            WorkEmail        = Col(r, "email"),
            StaffNumber      = Col(r, "id"),
            PreferredName    = null,
            Title            = "",
            DateOfBirth      = DateOnly.MinValue,
            Gender           = null,
            AltEmail         = null,
            PhoneHome        = null,
            PhoneMobile      = null,
            PhoneEmergency   = null,
            EmploymentType   = "Unknown",
            ContractType     = "Unknown",
            HireDate         = DateOnly.MinValue,
            EndDate          = null,
            WorkLocation     = "",
            ReportingManagerId = null,
            JobTitleId       = NullableInt(r, "job_title_id") ?? 0,
            JobGroupId       = NullableInt(r, "job_group_id") ?? 0,
            DepartmentId     = NullableInt(r, "department_id") ?? 0,
            ProfilePhotoUrl  = null,
            AccountStatus    = "active",
            CreatedAt        = ToDateTime(r, "created_at"),
            UpdatedAt        = ToDateTime(r, "updated_at"),
        }).ToList();
        await UpsertAsync(_db.Staff, s => s.Id, entities);
    }

    // CSV columns: id, admission_number, first_name, last_name, year_group_id,
    //              class_id, house_id, school_id, status, created_at, updated_at
    // Entity column notes:
    //   middle_names    → not in CSV; nullable — left null
    //   legal_first_name → not in CSV; mirrored from first_name
    //   legal_last_name  → not in CSV; mirrored from last_name
    //   gender           → not in CSV; defaulted to empty string
    //   date_of_birth    → not in CSV; defaulted to DateOnly.MinValue
    //   admission_date   → not in CSV; defaulted to DateOnly.MinValue
    //   username         → not in CSV; nullable — left null
    //   upn              → not in CSV; nullable — left null
    private async Task ImportStudentsAsync(string dir)
    {
        var rows = await ReadCsvAsync(Path.Combine(dir, "students.csv"));
        var entities = rows.Select(r => new Student
        {
            Id              = ToInt(r, "id"),
            AdmissionNumber = Col(r, "admission_number"),
            FirstName       = Col(r, "first_name"),
            LastName        = Col(r, "last_name"),
            MiddleNames     = null,
            LegalFirstName  = NullableCol(r, "first_name"),
            LegalLastName   = NullableCol(r, "last_name"),
            Gender          = null,
            DateOfBirth     = DateOnly.MinValue,
            AdmissionDate   = DateOnly.MinValue,
            YearGroupId     = ToInt(r, "year_group_id"),
            ClassId         = NullableInt(r, "class_id"),
            HouseId         = NullableInt(r, "house_id"),
            Username        = null,
            Upn             = null,
            SchoolId        = NullableInt(r, "school_id"),
            Status          = Col(r, "status"),
            CreatedAt       = ToDateTime(r, "created_at"),
            UpdatedAt       = ToDateTime(r, "updated_at"),
        }).ToList();
        await UpsertAsync(_db.Students, s => s.Id, entities);
    }

    // =========================================================================
    // Phase 4 — link / extension tables
    // =========================================================================

    // CSV columns: id, name, code, description, school_id, created_at, updated_at
    // Note: the CSV does not contain class_id or year_group_id columns.
    //   class_id     → not in CSV; defaulted to 1
    //   year_group_id → not in CSV; defaulted to 1
    private async Task ImportClassYearGroupAsync(string dir)
    {
        var rows = await ReadCsvAsync(Path.Combine(dir, "class_year_group_assignments.csv"));
        var entities = rows.Select(r => new ClassYearGroup
        {
            Id          = ToInt(r, "id"),
            ClassId     = 1,
            YearGroupId = 1,
            CreatedAt   = ToDateTime(r, "created_at"),
            UpdatedAt   = ToDateTime(r, "updated_at"),
        }).ToList();
        await UpsertAsync(_db.ClassYearGroups, c => c.Id, entities);
    }

    // StaffAbsences table archived - import no longer applicable.
    private Task ImportStaffAbsencesAsync(string dir) => Task.CompletedTask;

    // CSV columns: id, staff_id, device_type_id, serial_number, asset_tag,
    //              issued_date, return_date, condition, notes, created_at, updated_at
    // Entity column notes:
    //   assigned_date → mapped from issued_date
    //   returned_date → mapped from return_date
    private async Task ImportStaffDevicesAsync(string dir)
    {
        var rows = await ReadCsvAsync(Path.Combine(dir, "staff_devices.csv"));
        var entities = rows.Select(r => new StaffDevice
        {
            Id             = ToInt(r, "id"),
            StaffId        = ToInt(r, "staff_id"),
            DeviceType     = NullableCol(r, "device_type") ?? "Unknown",
            DeviceIdentifier = NullableCol(r, "serial_number") ?? string.Empty,
            AssignedDate   = ToDate(r, "issued_date"),
            ReturnedDate   = NullableDate(r, "return_date"),
            CreatedAt      = ToDateTime(r, "created_at"),
            UpdatedAt      = ToDateTime(r, "updated_at"),
        }).ToList();
        await UpsertAsync(_db.StaffDevices, sd => sd.Id, entities);
    }

    // CSV columns: id, staff_id, external_system_id, account_username,
    //              account_email, status, created_at, updated_at
    private async Task ImportStaffExternalAccountsAsync(string dir)
    {
        var rows = await ReadCsvAsync(Path.Combine(dir, "staff_external_accounts.csv"));
        var entities = rows.Select(r => new StaffExternalAccount
        {
            Id               = ToInt(r, "id"),
            StaffId          = ToInt(r, "staff_id"),
            ExternalSystemId = ToInt(r, "external_system_id"),
            ExternalUsername = NullableCol(r, "account_username") ?? string.Empty,
            AccountType      = "user",
            Status           = Col(r, "status"),
            CreatedAt        = ToDateTime(r, "created_at"),
            UpdatedAt        = ToDateTime(r, "updated_at"),
        }).ToList();
        await UpsertAsync(_db.StaffExternalAccounts, sea => sea.Id, entities);
    }

    // CSV columns: id, student_id, school_id, absence_count, last_absence_date,
    //              created_at, updated_at
    // Note: the CSV represents a summary record; it does not match the StudentAbsence
    // entity structure.  Only id and student_id are directly mappable; all other
    // required entity fields are defaulted below.
    //   absence_type_id → not in CSV; defaulted to 1
    //   date            → not in CSV; defaulted to DateOnly.MinValue
    //   recorded_by     → not in CSV; defaulted to 1
    // StudentAbsences table archived - import no longer applicable.
    private Task ImportStudentAbsencesAsync(string dir) => Task.CompletedTask;

    // CSV columns: id, student_id, contact_name, relationship, email, phone,
    //              created_at, updated_at
    // Entity column notes:
    //   phone_mobile             → mapped from phone
    //   phone_home               → not in CSV; nullable — left null
    //   priority                 → not in CSV; defaulted to 1
    //   lives_with_student       → not in CSV; nullable — left null
    //   has_parental_responsibility → not in CSV; defaulted to false
    //   safeguarding_flag        → not in CSV; nullable — left null
    //   notes                    → not in CSV; nullable — left null
    private async Task ImportStudentContactsAsync(string dir)
    {
        var rows = await ReadCsvAsync(Path.Combine(dir, "student_contacts.csv"));
        var entities = rows.Select(r => new StudentContact
        {
            Id           = ToInt(r, "id"),
            StudentId    = ToInt(r, "student_id"),
            ContactName  = Col(r, "contact_name"),
            Relationship = Col(r, "relationship"),
            Email        = NullableCol(r, "email"),
            Phone        = NullableCol(r, "phone"),
            IsPrimary    = false,
            CreatedAt    = ToDateTime(r, "created_at"),
            UpdatedAt    = ToDateTime(r, "updated_at"),
        }).ToList();
        await UpsertAsync(_db.StudentContacts, sc => sc.Id, entities);
    }

    // CSV columns: id, student_id, condition_name, severity, medication,
    //              care_plan_file, notes, created_at, updated_at
    // Entity column notes:
    //   medication, care_plan_file, notes → not on entity; ignored
    private async Task ImportStudentMedicalAsync(string dir)
    {
        var rows = await ReadCsvAsync(Path.Combine(dir, "student_medical.csv"));
        var entities = rows.Select(r => new StudentMedical
        {
            Id               = ToInt(r, "id"),
            StudentId        = ToInt(r, "student_id"),
            MedicalCondition = Col(r, "condition_name"),
            IsAllergic       = false,
            RecordedBy       = 1,
            CreatedAt        = ToDateTime(r, "created_at"),
            UpdatedAt        = ToDateTime(r, "updated_at"),
        }).ToList();
        await UpsertAsync(_db.StudentMedical, sm => sm.Id, entities);
    }

    // CSV columns: id, student_id, flag_type, flag_value, start_date, end_date,
    //              notes, created_at, updated_at
    private async Task ImportStudentFlagsAsync(string dir)
    {
        var rows = await ReadCsvAsync(Path.Combine(dir, "student_flags.csv"));
        var entities = rows.Select(r => new StudentFlag
        {
            Id         = ToInt(r, "id"),
            StudentId  = ToInt(r, "student_id"),
            FlagCode   = Col(r, "flag_type"),
            IsActive   = true,
            AssignedAt = ToDateTime(r, "start_date"),
            AssignedBy = 1,
            Notes      = NullableCol(r, "notes"),
            CreatedAt  = ToDateTime(r, "created_at"),
            UpdatedAt  = ToDateTime(r, "updated_at"),
        }).ToList();
        await UpsertAsync(_db.StudentFlags, sf => sf.Id, entities);
    }

    // =========================================================================
    // Phase 5 — audit tables
    // =========================================================================

    // CSV columns: id, user_id, login_time, ip_address, user_agent,
    //              success, created_at
    private async Task ImportLoginAuditAsync(string dir)
    {
        var rows = await ReadCsvAsync(Path.Combine(dir, "login_audit.csv"));
        var entities = rows.Select(r => new LoginAudit
        {
            Id        = ToInt(r, "id"),
            UserId    = ToInt(r, "user_id"),
            LoginTime = ToDateTime(r, "login_time"),
            IpAddress = NullableCol(r, "ip_address"),
            UserAgent = NullableCol(r, "user_agent"),
            Success   = ToBool(r, "success"),
        }).ToList();
        await UpsertAsync(_db.LoginAudit, la => la.Id, entities);
    }

    // CSV columns: id, user_id, event_type, event_time, ip_address,
    //              metadata, created_at
    private async Task ImportAccountVerificationEventsAsync(string dir)
    {
        var rows = await ReadCsvAsync(Path.Combine(dir, "account_verification_events.csv"));
        var entities = rows.Select(r => new AccountVerificationEvent
        {
            Id        = ToInt(r, "id"),
            UserId    = ToInt(r, "user_id"),
            EventType = Col(r, "event_type"),
            EventTime = ToDateTime(r, "event_time"),
            IpAddress = Col(r, "ip_address"),
            Metadata  = NullableCol(r, "metadata"),
            CreatedAt = ToDateTime(r, "created_at"),
        }).ToList();
        await UpsertAsync(_db.AccountVerificationEvents, av => av.Id, entities);
    }

    // CSV columns: id, user_id, old_role_id, new_role_id, changed_by,
    //              change_time, reason, metadata
    private async Task ImportRoleChangeAuditAsync(string dir)
    {
        var rows = await ReadCsvAsync(Path.Combine(dir, "role_change_audit.csv"));
        var entities = rows.Select(r => new RoleChangeAudit
        {
            Id         = ToInt(r, "id"),
            UserId     = ToInt(r, "user_id"),
            OldRoleId  = NullableInt(r, "old_role_id"),
            NewRoleId  = NullableInt(r, "new_role_id"),
            ChangedBy  = ToInt(r, "changed_by"),
            ChangedAt  = ToDateTime(r, "change_time"),
            ChangeReason = NullableCol(r, "reason"),
        }).ToList();
        await UpsertAsync(_db.RoleChangeAudit, rca => rca.Id, entities);
    }

    // StaffAbsenceAudit table archived � import no longer applicable.
    private Task ImportStaffAbsenceAuditAsync(string dir) => Task.CompletedTask;

    // CSV columns: id, staff_device_id, staff_id, action, changed_by,
    //              change_time, old_values, new_values
    private async Task ImportStaffDeviceAuditAsync(string dir)
    {
        var rows = await ReadCsvAsync(Path.Combine(dir, "staff_device_audit.csv"));
        var entities = rows.Select(r => new StaffDeviceAudit
        {
            Id            = ToInt(r, "id"),
            StaffDeviceId = ToInt(r, "staff_device_id"),
            StaffId       = ToInt(r, "staff_id"),
            Action        = Col(r, "action"),
            CreatedAt     = ToDateTime(r, "change_time"),
        }).ToList();
        await UpsertAsync(_db.StaffDeviceAudit, sda => sda.Id, entities);
    }

    // CSV columns: id, staff_external_account_id, staff_id, action, changed_by,
    //              change_time, old_values, new_values
    private async Task ImportStaffExternalAccountAuditAsync(string dir)
    {
        var rows = await ReadCsvAsync(Path.Combine(dir, "staff_external_account_audit.csv"));
        var entities = rows.Select(r => new StaffExternalAccountAudit
        {
            Id                     = ToInt(r, "id"),
            StaffExternalAccountId = ToInt(r, "staff_external_account_id"),
            StaffId                = ToInt(r, "staff_id"),
            Action                 = Col(r, "action"),
            CreatedAt              = ToDateTime(r, "change_time"),
        }).ToList();
        await UpsertAsync(_db.StaffExternalAccountAudit, sea => sea.Id, entities);
    }

    // StudentAbsenceAudit table archived � import no longer applicable.
    private Task ImportStudentAbsenceAuditAsync(string dir) => Task.CompletedTask;

    // =========================================================================
    // Generic upsert
    // =========================================================================

    /// <summary>
    /// Upserts a batch of entities using their int Id.
    /// </summary>
    private async Task UpsertAsync<T>(
        DbSet<T> set,
        Expression<Func<T, int>> idExpr,
        IReadOnlyList<T> entities)
        where T : class
    {
        if (entities.Count == 0) return;

        var idFn        = idExpr.Compile();
        var existingIds = (await set.Select(idExpr).ToListAsync()).ToHashSet();

        foreach (var entity in entities)
        {
            _db.Entry(entity).State = existingIds.Contains(idFn(entity))
                ? EntityState.Modified
                : EntityState.Added;
        }

        await _db.SaveChangesAsync();
        _db.ChangeTracker.Clear();
    }

    /// <summary>
    /// Upserts a batch of entities using their long Id (for legacy long-keyed entities).
    /// </summary>
    private async Task UpsertAsync<T>(
        DbSet<T> set,
        Expression<Func<T, long>> idExpr,
        IReadOnlyList<T> entities)
        where T : class
    {
        if (entities.Count == 0) return;

        var idFn        = idExpr.Compile();
        var existingIds = (await set.Select(idExpr).ToListAsync()).ToHashSet();

        foreach (var entity in entities)
        {
            _db.Entry(entity).State = existingIds.Contains(idFn(entity))
                ? EntityState.Modified
                : EntityState.Added;
        }

        await _db.SaveChangesAsync();
        _db.ChangeTracker.Clear();
    }

    private async Task UpsertAsync<T>(
        DbSet<T> set,
        Expression<Func<T, ulong>> idExpr,
        IReadOnlyList<T> entities)
        where T : class
    {
        if (entities.Count == 0) return;

        var idFn        = idExpr.Compile();
        var existingIds = (await set.Select(idExpr).ToListAsync()).ToHashSet();

        foreach (var entity in entities)
        {
            _db.Entry(entity).State = existingIds.Contains(idFn(entity))
                ? EntityState.Modified
                : EntityState.Added;
        }

        await _db.SaveChangesAsync();
        _db.ChangeTracker.Clear();
    }

    // =========================================================================
    // CSV parsing helpers
    // =========================================================================

    /// <summary>
    /// Reads all data rows from a CSV file.  Returns an empty list if the file
    /// does not exist or contains no data rows.
    /// Each row is returned as a case-insensitive column-name → value dictionary.
    /// </summary>
    private static async Task<List<Dictionary<string, string>>> ReadCsvAsync(string path)
    {
        if (!File.Exists(path))
            return [];

        var lines  = await File.ReadAllLinesAsync(path);
        var result = new List<Dictionary<string, string>>();
        string[]? headers = null;

        foreach (var raw in lines)
        {
            var line = raw.Trim();
            if (string.IsNullOrEmpty(line)) continue;

            if (headers is null)
            {
                headers = ParseCsvLine(line)
                    .Select(h => h.Trim().ToLowerInvariant())
                    .ToArray();
                continue;
            }

            var values = ParseCsvLine(line);
            var dict   = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            for (var i = 0; i < headers.Length; i++)
                dict[headers[i]] = i < values.Length ? values[i] : string.Empty;

            result.Add(dict);
        }

        return result;
    }

    /// <summary>
    /// Parses one RFC 4180-compliant CSV line.
    /// Handles double-quoted fields that contain commas or escaped quotes ("").
    /// </summary>
    private static string[] ParseCsvLine(string line)
    {
        var fields  = new List<string>();
        var current = new System.Text.StringBuilder();
        var inQuotes = false;
        var i = 0;

        while (i < line.Length)
        {
            var c = line[i];
            if (inQuotes)
            {
                if (c == '"' && i + 1 < line.Length && line[i + 1] == '"')
                {
                    current.Append('"');
                    i += 2;
                }
                else if (c == '"')
                {
                    inQuotes = false;
                    i++;
                }
                else
                {
                    current.Append(c);
                    i++;
                }
            }
            else
            {
                if (c == '"') { inQuotes = true; i++; }
                else if (c == ',') { fields.Add(current.ToString()); current.Clear(); i++; }
                else { current.Append(c); i++; }
            }
        }

        fields.Add(current.ToString());
        return [.. fields];
    }

    // ── Type-safe value extractors ────────────────────────────────────────────

    private static string Col(IReadOnlyDictionary<string, string> row, string col) =>
        row.TryGetValue(col, out var v) ? v.Trim() : string.Empty;

    private static string? NullableCol(IReadOnlyDictionary<string, string> row, string col)
    {
        var v = Col(row, col);
        return string.IsNullOrEmpty(v) ? null : v;
    }

    private static long ToLong(IReadOnlyDictionary<string, string> row, string col) =>
        long.TryParse(Col(row, col), out var v) ? v : 0L;

    private static long? NullableLong(IReadOnlyDictionary<string, string> row, string col)
    {
        var s = Col(row, col);
        return long.TryParse(s, out var v) ? v : (long?)null;
    }

    private static int ToInt(IReadOnlyDictionary<string, string> row, string col) =>
        int.TryParse(Col(row, col), out var v) ? v : 0;

    private static int? NullableInt(IReadOnlyDictionary<string, string> row, string col)
    {
        var s = Col(row, col);
        return int.TryParse(s, out var v) ? v : (int?)null;
    }

    private static bool ToBool(IReadOnlyDictionary<string, string> row, string col)
    {
        var s = Col(row, col);
        return s == "1" || s.Equals("true", StringComparison.OrdinalIgnoreCase);
    }

    private static bool? NullableBool(IReadOnlyDictionary<string, string> row, string col)
    {
        var s = Col(row, col);
        if (string.IsNullOrEmpty(s)) return null;
        return s == "1" || s.Equals("true", StringComparison.OrdinalIgnoreCase);
    }

    private static DateTime ToDateTime(IReadOnlyDictionary<string, string> row, string col)
    {
        var s = Col(row, col);
        return DateTime.TryParse(s, CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out var v)
            ? v : DateTime.MinValue;
    }

    private static DateTime? NullableDateTime(IReadOnlyDictionary<string, string> row, string col)
    {
        var s = Col(row, col);
        if (string.IsNullOrEmpty(s)) return null;
        return DateTime.TryParse(s, CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out var v)
            ? v : (DateTime?)null;
    }

    private static DateOnly ToDate(IReadOnlyDictionary<string, string> row, string col)
    {
        var s = Col(row, col);
        return DateOnly.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.None, out var v)
            ? v : DateOnly.MinValue;
    }

    private static DateOnly? NullableDate(IReadOnlyDictionary<string, string> row, string col)
    {
        var s = Col(row, col);
        if (string.IsNullOrEmpty(s)) return null;
        return DateOnly.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.None, out var v)
            ? v : (DateOnly?)null;
    }
}

