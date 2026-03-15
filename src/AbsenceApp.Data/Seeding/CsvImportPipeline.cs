/*
===============================================================================
 File        : CsvImportPipeline.cs
 Namespace   : AbsenceApp.Data.Seeding
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-15
 Updated     : 2026-03-15
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
        // â”€â”€ Phase 1: standalone lookup tables (no FK dependencies) â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
        await ImportSchoolsAsync(csvDirectory);
        await ImportPhasesAsync(csvDirectory);
        await ImportJobTitlesAsync(csvDirectory);
        await ImportJobGroupsAsync(csvDirectory);
        await ImportRoleTypesAsync(csvDirectory);
        await ImportResponsibilitiesAsync(csvDirectory);
        await ImportAbsenceTypesAsync(csvDirectory);
        await ImportDeviceTypesAsync(csvDirectory);
        await ImportExternalSystemsAsync(csvDirectory);
        await ImportSystemEventsAsync(csvDirectory);

        // â”€â”€ Phase 2: school-scoped structure â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
        await ImportYearGroupsAsync(csvDirectory);
        await ImportHousesAsync(csvDirectory);
        await ImportDepartmentsAsync(csvDirectory);
        await ImportClassesAsync(csvDirectory);

        // â”€â”€ Phase 3: core people tables â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
        await ImportUsersAsync(csvDirectory);
        await ImportUserProfilesAsync(csvDirectory);
        await ImportStaffAsync(csvDirectory);
        await ImportStudentsAsync(csvDirectory);

        // â”€â”€ Phase 4: extension / link tables â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
        await ImportClassYearGroupAssignmentsAsync(csvDirectory);
        await ImportStaffAssignmentsAsync(csvDirectory);
        await ImportStaffAbsencesAsync(csvDirectory);
        await ImportStaffDevicesAsync(csvDirectory);
        await ImportStaffExternalAccountsAsync(csvDirectory);
        await ImportStudentAbsencesAsync(csvDirectory);
        await ImportStudentContactsAsync(csvDirectory);
        await ImportStudentMedicalAsync(csvDirectory);
        await ImportStudentFlagsAsync(csvDirectory);
        await ImportAttendanceRegistersAsync(csvDirectory);
        await ImportAttendanceMarksAsync(csvDirectory);

        // â”€â”€ Phase 5: audit tables â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
        await ImportLoginAuditAsync(csvDirectory);
        await ImportAccountVerificationEventsAsync(csvDirectory);
        await ImportRoleChangeAuditAsync(csvDirectory);
        await ImportStaffAssignmentAuditAsync(csvDirectory);
        await ImportStaffAbsenceAuditAsync(csvDirectory);
        await ImportStaffDeviceAuditAsync(csvDirectory);
        await ImportStaffExternalAccountAuditAsync(csvDirectory);
        await ImportStudentAbsenceAuditAsync(csvDirectory);
    }

    // =========================================================================
    // Phase 1 â€” lookup tables
    // =========================================================================

    // CSV columns: id, name, code, school_ref, created_at, updated_at
    // Entity column notes:
    //   status     â†’ not in CSV; defaulted to "active"
    //   description â†’ not in CSV; nullable â€” left null
    private async Task ImportSchoolsAsync(string dir)
    {
        var rows = await ReadCsvAsync(Path.Combine(dir, "schools.csv"));
        var entities = rows.Select(r => new School
        {
            Id        = ToLong(r, "id"),
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
    //   code         â†’ not in CSV; set to name value
    //   numeric_order â†’ not in CSV; defaulted to 0
    //   school_id    â†’ not in CSV; defaulted to 1
    private async Task ImportPhasesAsync(string dir)
    {
        var rows = await ReadCsvAsync(Path.Combine(dir, "phases.csv"));
        var entities = rows.Select(r => new Phase
        {
            Id           = ToLong(r, "id"),
            Name         = Col(r, "name"),
            Code         = Col(r, "name"),
            NumericOrder = 0,
            SchoolId     = 1,
            CreatedAt    = ToDateTime(r, "created_at"),
            UpdatedAt    = ToDateTime(r, "updated_at"),
        }).ToList();
        await UpsertAsync(_db.Phases, p => p.Id, entities);
    }

    // CSV columns: id, title, code, description, created_at, updated_at
    private async Task ImportJobTitlesAsync(string dir)
    {
        var rows = await ReadCsvAsync(Path.Combine(dir, "job_titles.csv"));
        var entities = rows.Select(r => new JobTitle
        {
            Id          = ToLong(r, "id"),
            Title       = Col(r, "title"),
            Code        = Col(r, "code"),
            Description = NullableCol(r, "description"),
            CreatedAt   = ToDateTime(r, "created_at"),
            UpdatedAt   = ToDateTime(r, "updated_at"),
        }).ToList();
        await UpsertAsync(_db.JobTitles, j => j.Id, entities);
    }

    // CSV columns: id, name, description, typical_members, created_at, updated_at
    private async Task ImportJobGroupsAsync(string dir)
    {
        var rows = await ReadCsvAsync(Path.Combine(dir, "job_groups.csv"));
        var entities = rows.Select(r => new JobGroup
        {
            Id             = ToLong(r, "id"),
            Name           = Col(r, "name"),
            Description    = NullableCol(r, "description"),
            TypicalMembers = NullableCol(r, "typical_members"),
            CreatedAt      = ToDateTime(r, "created_at"),
            UpdatedAt      = ToDateTime(r, "updated_at"),
        }).ToList();
        await UpsertAsync(_db.JobGroups, j => j.Id, entities);
    }

    // CSV columns: id, name, display_name, description, is_system_role, is_default,
    //              priority, created_at, updated_at
    private async Task ImportRoleTypesAsync(string dir)
    {
        var rows = await ReadCsvAsync(Path.Combine(dir, "role_types.csv"));
        var entities = rows.Select(r => new RoleType
        {
            Id           = ToLong(r, "id"),
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
        var entities = rows.Select(r => new Responsibility
        {
            Id          = ToLong(r, "id"),
            Name        = Col(r, "name"),
            Code        = Col(r, "code"),
            Description = NullableCol(r, "description"),
        }).ToList();
        await UpsertAsync(_db.Responsibilities, r => r.Id, entities);
    }

    // CSV columns: id, name, code, description, is_paid
    private async Task ImportAbsenceTypesAsync(string dir)
    {
        var rows = await ReadCsvAsync(Path.Combine(dir, "absence_types.csv"));
        var entities = rows.Select(r => new AbsenceType
        {
            Id          = ToLong(r, "id"),
            Name        = Col(r, "name"),
            Code        = Col(r, "code"),
            Description = NullableCol(r, "description"),
            IsPaid      = ToBool(r, "is_paid"),
        }).ToList();
        await UpsertAsync(_db.AbsenceTypes, at => at.Id, entities);
    }

    // CSV columns: id, name, code, description, created_at, updated_at
    private async Task ImportDeviceTypesAsync(string dir)
    {
        var rows = await ReadCsvAsync(Path.Combine(dir, "device_types.csv"));
        var entities = rows.Select(r => new DeviceType
        {
            Id          = ToLong(r, "id"),
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
            Id          = ToLong(r, "id"),
            Name        = Col(r, "name"),
            Code        = Col(r, "code"),
            Description = NullableCol(r, "description"),
            CreatedAt   = ToDateTime(r, "created_at"),
            UpdatedAt   = ToDateTime(r, "updated_at"),
        }).ToList();
        await UpsertAsync(_db.ExternalSystems, e => e.Id, entities);
    }

    // CSV columns: id, event_type, event_time, triggered_by, description, metadata
    private async Task ImportSystemEventsAsync(string dir)
    {
        var rows = await ReadCsvAsync(Path.Combine(dir, "system_events.csv"));
        var entities = rows.Select(r => new SystemEvent
        {
            Id          = ToLong(r, "id"),
            EventType   = Col(r, "event_type"),
            EventTime   = ToDateTime(r, "event_time"),
            TriggeredBy = Col(r, "triggered_by"),
            Description = NullableCol(r, "description"),
            Metadata    = NullableCol(r, "metadata"),
        }).ToList();
        await UpsertAsync(_db.SystemEvents, e => e.Id, entities);
    }

    // =========================================================================
    // Phase 2 â€” school-scoped structure
    // =========================================================================

    // CSV columns: id, name, created_at, updated_at
    // Entity column notes:
    //   code         â†’ not in CSV; set to name value
    //   numeric_value â†’ not in CSV; defaulted to 0
    //   phase_id     â†’ not in CSV; defaulted to 1
    //   school_id    â†’ not in CSV; defaulted to 1
    private async Task ImportYearGroupsAsync(string dir)
    {
        var rows = await ReadCsvAsync(Path.Combine(dir, "year_groups.csv"));
        var entities = rows.Select(r => new YearGroup
        {
            Id           = ToLong(r, "id"),
            Name         = Col(r, "name"),
            Code         = Col(r, "name"),
            NumericValue = 0,
            PhaseId      = 1,
            SchoolId     = 1,
            CreatedAt    = ToDateTime(r, "created_at"),
            UpdatedAt    = ToDateTime(r, "updated_at"),
        }).ToList();
        await UpsertAsync(_db.YearGroups, y => y.Id, entities);
    }

    // CSV columns: id, name, created_at, updated_at
    // Entity column notes:
    //   colour   â†’ not in CSV; defaulted to empty string
    //   code     â†’ not in CSV; set to name value
    //   school_id â†’ not in CSV; defaulted to 1
    private async Task ImportHousesAsync(string dir)
    {
        var rows = await ReadCsvAsync(Path.Combine(dir, "houses.csv"));
        var entities = rows.Select(r => new House
        {
            Id        = ToLong(r, "id"),
            Name      = Col(r, "name"),
            Colour    = "",
            Code      = Col(r, "name"),
            SchoolId  = 1,
            CreatedAt = ToDateTime(r, "created_at"),
            UpdatedAt = ToDateTime(r, "updated_at"),
        }).ToList();
        await UpsertAsync(_db.Houses, h => h.Id, entities);
    }

    // CSV columns: id, name, code, created_at, updated_at
    // Entity column notes:
    //   description â†’ not in CSV; nullable â€” left null
    //   head_user_id â†’ not in CSV; nullable â€” left null
    //   status      â†’ not in CSV; defaulted to "active"
    private async Task ImportDepartmentsAsync(string dir)
    {
        var rows = await ReadCsvAsync(Path.Combine(dir, "departments.csv"));
        var entities = rows.Select(r => new Department
        {
            Id          = ToLong(r, "id"),
            Name        = Col(r, "name"),
            Code        = Col(r, "code"),
            Description = NullableCol(r, "description"),
            HeadUserId  = null,
            Status      = "active",
            CreatedAt   = ToDateTime(r, "created_at"),
            UpdatedAt   = ToDateTime(r, "updated_at"),
        }).ToList();
        await UpsertAsync(_db.Departments, d => d.Id, entities);
    }

    // CSV columns: id, name, created_at, updated_at
    // Entity column notes:
    //   code        â†’ not in CSV; set to name value
    //   description â†’ not in CSV; nullable â€” left null
    private async Task ImportClassesAsync(string dir)
    {
        var rows = await ReadCsvAsync(Path.Combine(dir, "classes.csv"));
        var entities = rows.Select(r => new Class
        {
            Id          = ToLong(r, "id"),
            Name        = Col(r, "name"),
            Code        = Col(r, "name"),
            Description = null,
            CreatedAt   = ToDateTime(r, "created_at"),
            UpdatedAt   = ToDateTime(r, "updated_at"),
        }).ToList();
        await UpsertAsync(_db.Classes, c => c.Id, entities);
    }

    // =========================================================================
    // Phase 3 â€” core people tables
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
            Id                 = ToLong(r, "id"),
            Name               = Col(r, "name"),
            Username           = Col(r, "username"),
            Email              = Col(r, "email"),
            EmailVerifiedAt    = NullableDateTime(r, "email_verified_at"),
            Password           = Col(r, "password"),
            RoleTypeId         = ToLong(r, "role_type_id"),
            Status             = Col(r, "status"),
            IsAdmin            = ToBool(r, "is_admin"),
            ProfilePhotoPath   = NullableCol(r, "profile_photo_path"),
            PhoneNumber        = NullableCol(r, "phone_number"),
            LastLoginAt        = NullableDateTime(r, "last_login_at"),
            LastLoginIp        = NullableCol(r, "last_login_ip"),
            RememberToken      = NullableCol(r, "remember_token"),
            CreatedAt          = ToDateTime(r, "created_at"),
            UpdatedAt          = ToDateTime(r, "updated_at"),
            DepartmentId       = NullableLong(r, "department_id"),
            Designation        = NullableCol(r, "designation"),
            LoginCount         = ToInt(r, "login_count"),
            IsTwoFactorEnabled = ToBool(r, "is_two_factor_enabled"),
            TwoFactorSecret    = NullableCol(r, "two_factor_secret"),
            BackupCodes        = NullableCol(r, "backup_codes"),
            Timezone           = NullableCol(r, "timezone"),
            LanguageCode       = NullableCol(r, "language_code"),
            Bio                = NullableCol(r, "bio"),
            DateOfBirth        = NullableDate(r, "date_of_birth"),
            Gender             = NullableCol(r, "gender"),
            Address            = NullableCol(r, "address"),
            City               = NullableCol(r, "city"),
            Country            = NullableCol(r, "country"),
            PostalCode         = NullableCol(r, "postal_code"),
        }).ToList();
        await UpsertAsync(_db.Users, u => u.Id, entities);
    }

    // CSV columns: id, user_id, first_name, last_name, preferred_name, title,
    //              date_of_birth, profile_picture_url, bio, gender, timezone,
    //              language_code, department_id, job_title_id, school_id,
    //              created_at, updated_at
    private async Task ImportUserProfilesAsync(string dir)
    {
        var rows = await ReadCsvAsync(Path.Combine(dir, "user_profiles.csv"));
        var entities = rows.Select(r => new UserProfile
        {
            Id                = ToLong(r, "id"),
            UserId            = ToLong(r, "user_id"),
            FirstName         = Col(r, "first_name"),
            LastName          = Col(r, "last_name"),
            PreferredName     = NullableCol(r, "preferred_name"),
            Title             = Col(r, "title"),
            DateOfBirth       = ToDate(r, "date_of_birth"),
            ProfilePictureUrl = NullableCol(r, "profile_picture_url"),
            Bio               = NullableCol(r, "bio"),
            Gender            = NullableCol(r, "gender"),
            Timezone          = Col(r, "timezone"),
            LanguageCode      = Col(r, "language_code"),
            DepartmentId      = ToLong(r, "department_id"),
            JobTitleId        = ToLong(r, "job_title_id"),
            SchoolId          = ToLong(r, "school_id"),
            CreatedAt         = ToDateTime(r, "created_at"),
            UpdatedAt         = ToDateTime(r, "updated_at"),
        }).ToList();
        await UpsertAsync(_db.UserProfiles, u => u.Id, entities);
    }

    // CSV columns: id, first_name, last_name, job_title (textâ€”not an ID),
    //              email, school_id (text ref codeâ€”not numeric), created_at, updated_at
    // Entity column notes:
    //   staff_number      â†’ not in CSV; set to id string as placeholder
    //   title             â†’ not in CSV; defaulted to empty string
    //   date_of_birth     â†’ not in CSV; defaulted to DateOnly.MinValue
    //   employment_type   â†’ not in CSV; defaulted to "Unknown"
    //   contract_type     â†’ not in CSV; defaulted to "Unknown"
    //   hire_date         â†’ not in CSV; defaulted to DateOnly.MinValue
    //   work_location     â†’ not in CSV; defaulted to empty string
    //   job_title_id      â†’ not in CSV (only text name present); defaulted to 1
    //   job_group_id      â†’ not in CSV; defaulted to 1
    //   department_id     â†’ not in CSV; defaulted to 1
    //   account_status    â†’ not in CSV; defaulted to "active"
    private async Task ImportStaffAsync(string dir)
    {
        var rows = await ReadCsvAsync(Path.Combine(dir, "staff.csv"));
        var entities = rows.Select(r => new Staff
        {
            Id               = ToLong(r, "id"),
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
            JobTitleId       = 1,
            JobGroupId       = 1,
            DepartmentId     = 1,
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
    //   middle_names    â†’ not in CSV; nullable â€” left null
    //   legal_first_name â†’ not in CSV; mirrored from first_name
    //   legal_last_name  â†’ not in CSV; mirrored from last_name
    //   gender           â†’ not in CSV; defaulted to empty string
    //   date_of_birth    â†’ not in CSV; defaulted to DateOnly.MinValue
    //   admission_date   â†’ not in CSV; defaulted to DateOnly.MinValue
    //   username         â†’ not in CSV; nullable â€” left null
    //   upn              â†’ not in CSV; nullable â€” left null
    private async Task ImportStudentsAsync(string dir)
    {
        var rows = await ReadCsvAsync(Path.Combine(dir, "students.csv"));
        var entities = rows.Select(r => new Student
        {
            Id              = ToLong(r, "id"),
            AdmissionNumber = Col(r, "admission_number"),
            FirstName       = Col(r, "first_name"),
            LastName        = Col(r, "last_name"),
            MiddleNames     = null,
            LegalFirstName  = Col(r, "first_name"),
            LegalLastName   = Col(r, "last_name"),
            Gender          = "",
            DateOfBirth     = DateOnly.MinValue,
            AdmissionDate   = DateOnly.MinValue,
            YearGroupId     = ToLong(r, "year_group_id"),
            ClassId         = ToLong(r, "class_id"),
            HouseId         = NullableLong(r, "house_id"),
            Username        = null,
            Upn             = null,
            SchoolId        = NullableLong(r, "school_id") ?? 1,
            Status          = Col(r, "status"),
            CreatedAt       = ToDateTime(r, "created_at"),
            UpdatedAt       = ToDateTime(r, "updated_at"),
        }).ToList();
        await UpsertAsync(_db.Students, s => s.Id, entities);
    }

    // =========================================================================
    // Phase 4 â€” link / extension tables
    // =========================================================================

    // CSV columns: id, name, code, description, school_id, created_at, updated_at
    // Note: the CSV does not contain class_id or year_group_id columns.
    //   class_id     â†’ not in CSV; defaulted to 1
    //   year_group_id â†’ not in CSV; defaulted to 1
    private async Task ImportClassYearGroupAssignmentsAsync(string dir)
    {
        var rows = await ReadCsvAsync(Path.Combine(dir, "class_year_group_assignments.csv"));
        var entities = rows.Select(r => new ClassYearGroupAssignment
        {
            Id          = ToLong(r, "id"),
            ClassId     = 1,
            YearGroupId = 1,
            SchoolId    = ToLong(r, "school_id"),
            CreatedAt   = ToDateTime(r, "created_at"),
            UpdatedAt   = ToDateTime(r, "updated_at"),
        }).ToList();
        await UpsertAsync(_db.ClassYearGroupAssignments, c => c.Id, entities);
    }

    // CSV columns: id, staff_id, class_id, year_group_id, phase_id,
    //              created_at, updated_at
    // Entity column notes:
    //   job_title_id    â†’ not in CSV; defaulted to 1
    //   job_group_id    â†’ not in CSV; defaulted to 1
    //   department_id   â†’ not in CSV; defaulted to 1
    //   responsibility_id â†’ not in CSV; nullable â€” left null
    //   start_date      â†’ not in CSV; defaulted to DateOnly.MinValue
    //   end_date        â†’ not in CSV; nullable â€” left null
    //   days_of_week    â†’ not in CSV; nullable â€” left null
    private async Task ImportStaffAssignmentsAsync(string dir)
    {
        var rows = await ReadCsvAsync(Path.Combine(dir, "staff_assignments.csv"));
        var entities = rows.Select(r => new StaffAssignment
        {
            Id               = ToLong(r, "id"),
            StaffId          = ToLong(r, "staff_id"),
            ClassId          = NullableLong(r, "class_id"),
            JobTitleId       = 1,
            JobGroupId       = 1,
            DepartmentId     = 1,
            ResponsibilityId = null,
            StartDate        = DateOnly.MinValue,
            EndDate          = null,
            DaysOfWeek       = null,
            CreatedAt        = ToDateTime(r, "created_at"),
            UpdatedAt        = ToDateTime(r, "updated_at"),
        }).ToList();
        await UpsertAsync(_db.StaffAssignments, sa => sa.Id, entities);
    }

    // CSV columns: id, staff_id, absence_type_id, start_date, end_date,
    //              notes, created_at, updated_at
    private async Task ImportStaffAbsencesAsync(string dir)
    {
        var rows = await ReadCsvAsync(Path.Combine(dir, "staff_absences.csv"));
        var entities = rows.Select(r => new StaffAbsence
        {
            Id            = ToLong(r, "id"),
            StaffId       = ToLong(r, "staff_id"),
            AbsenceTypeId = ToLong(r, "absence_type_id"),
            StartDate     = ToDate(r, "start_date"),
            EndDate       = NullableDate(r, "end_date"),
            Notes         = NullableCol(r, "notes"),
            CreatedAt     = ToDateTime(r, "created_at"),
            UpdatedAt     = ToDateTime(r, "updated_at"),
        }).ToList();
        await UpsertAsync(_db.StaffAbsences, sa => sa.Id, entities);
    }

    // CSV columns: id, staff_id, device_type_id, serial_number, asset_tag,
    //              issued_date, return_date, condition, notes, created_at, updated_at
    // Entity column notes:
    //   assigned_date â†’ mapped from issued_date
    //   returned_date â†’ mapped from return_date
    private async Task ImportStaffDevicesAsync(string dir)
    {
        var rows = await ReadCsvAsync(Path.Combine(dir, "staff_devices.csv"));
        var entities = rows.Select(r => new StaffDevice
        {
            Id           = ToLong(r, "id"),
            StaffId      = ToLong(r, "staff_id"),
            DeviceTypeId = ToLong(r, "device_type_id"),
            SerialNumber = Col(r, "serial_number"),
            AssignedDate = ToDate(r, "issued_date"),
            ReturnedDate = NullableDate(r, "return_date"),
            CreatedAt    = ToDateTime(r, "created_at"),
            UpdatedAt    = ToDateTime(r, "updated_at"),
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
            Id               = ToLong(r, "id"),
            StaffId          = ToLong(r, "staff_id"),
            ExternalSystemId = ToLong(r, "external_system_id"),
            AccountUsername  = Col(r, "account_username"),
            AccountEmail     = Col(r, "account_email"),
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
    //   absence_type_id â†’ not in CSV; defaulted to 1
    //   date            â†’ not in CSV; defaulted to DateOnly.MinValue
    //   recorded_by     â†’ not in CSV; defaulted to 1
    private async Task ImportStudentAbsencesAsync(string dir)
    {
        var rows = await ReadCsvAsync(Path.Combine(dir, "student_absences.csv"));
        var entities = rows.Select(r => new StudentAbsence
        {
            Id            = ToLong(r, "id"),
            StudentId     = ToLong(r, "student_id"),
            AbsenceTypeId = 1,
            Date          = DateOnly.MinValue,
            StartTime     = null,
            EndTime       = null,
            IsAuthorised  = false,
            Notes         = null,
            RecordedBy    = 1,
            CreatedAt     = ToDateTime(r, "created_at"),
            UpdatedAt     = ToDateTime(r, "updated_at"),
        }).ToList();
        await UpsertAsync(_db.StudentAbsences, sa => sa.Id, entities);
    }

    // CSV columns: id, student_id, contact_name, relationship, email, phone,
    //              created_at, updated_at
    // Entity column notes:
    //   phone_mobile             â†’ mapped from phone
    //   phone_home               â†’ not in CSV; nullable â€” left null
    //   priority                 â†’ not in CSV; defaulted to 1
    //   lives_with_student       â†’ not in CSV; nullable â€” left null
    //   has_parental_responsibility â†’ not in CSV; defaulted to false
    //   safeguarding_flag        â†’ not in CSV; nullable â€” left null
    //   notes                    â†’ not in CSV; nullable â€” left null
    private async Task ImportStudentContactsAsync(string dir)
    {
        var rows = await ReadCsvAsync(Path.Combine(dir, "student_contacts.csv"));
        var entities = rows.Select(r => new StudentContact
        {
            Id                       = ToLong(r, "id"),
            StudentId                = ToLong(r, "student_id"),
            ContactName              = Col(r, "contact_name"),
            Relationship             = Col(r, "relationship"),
            Email                    = NullableCol(r, "email"),
            PhoneMobile              = NullableCol(r, "phone"),
            PhoneHome                = null,
            Priority                 = 1,
            LivesWithStudent         = null,
            HasParentalResponsibility = false,
            SafeguardingFlag         = null,
            Notes                    = null,
            CreatedAt                = ToDateTime(r, "created_at"),
            UpdatedAt                = ToDateTime(r, "updated_at"),
        }).ToList();
        await UpsertAsync(_db.StudentContacts, sc => sc.Id, entities);
    }

    // CSV columns: id, student_id, condition_name, severity, medication,
    //              care_plan_file, notes, created_at, updated_at
    // Entity column notes:
    //   medication, care_plan_file, notes â†’ not on entity; ignored
    private async Task ImportStudentMedicalAsync(string dir)
    {
        var rows = await ReadCsvAsync(Path.Combine(dir, "student_medical.csv"));
        var entities = rows.Select(r => new StudentMedical
        {
            Id            = ToLong(r, "id"),
            StudentId     = ToLong(r, "student_id"),
            ConditionName = Col(r, "condition_name"),
            Severity      = NullableCol(r, "severity"),
            CreatedAt     = ToDateTime(r, "created_at"),
            UpdatedAt     = ToDateTime(r, "updated_at"),
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
            Id        = ToLong(r, "id"),
            StudentId = ToLong(r, "student_id"),
            FlagType  = Col(r, "flag_type"),
            FlagValue = NullableCol(r, "flag_value"),
            StartDate = ToDate(r, "start_date"),
            EndDate   = NullableDate(r, "end_date"),
            Notes     = NullableCol(r, "notes"),
            CreatedAt = ToDateTime(r, "created_at"),
            UpdatedAt = ToDateTime(r, "updated_at"),
        }).ToList();
        await UpsertAsync(_db.StudentFlags, sf => sf.Id, entities);
    }

    // CSV columns: id, class_id, date, session, opened_by, closed_by,
    //              status, created_at, updated_at
    // (Source CSV is currently empty; importer is ready for future data.)
    private async Task ImportAttendanceRegistersAsync(string dir)
    {
        var rows = await ReadCsvAsync(Path.Combine(dir, "attendance_registers.csv"));
        var entities = rows.Select(r => new AttendanceRegister
        {
            Id        = ToLong(r, "id"),
            ClassId   = ToLong(r, "class_id"),
            Date      = ToDate(r, "date"),
            Session   = Col(r, "session"),
            OpenedBy  = ToLong(r, "opened_by"),
            ClosedBy  = NullableLong(r, "closed_by"),
            Status    = Col(r, "status"),
            CreatedAt = ToDateTime(r, "created_at"),
            UpdatedAt = ToDateTime(r, "updated_at"),
        }).ToList();
        await UpsertAsync(_db.AttendanceRegisters, ar => ar.Id, entities);
    }

    // CSV columns: id, attendance_register_id, student_id, mark_code,
    //              is_late, minutes_late, notes, recorded_by,
    //              created_at, updated_at
    // (Source CSV is currently empty; importer is ready for future data.)
    private async Task ImportAttendanceMarksAsync(string dir)
    {
        var rows = await ReadCsvAsync(Path.Combine(dir, "attendance_marks.csv"));
        var entities = rows.Select(r => new AttendanceMark
        {
            Id                   = ToLong(r, "id"),
            AttendanceRegisterId = ToLong(r, "attendance_register_id"),
            StudentId            = ToLong(r, "student_id"),
            MarkCode             = Col(r, "mark_code"),
            IsLate               = NullableBool(r, "is_late"),
            MinutesLate          = NullableInt(r, "minutes_late"),
            Notes                = NullableCol(r, "notes"),
            RecordedBy           = ToLong(r, "recorded_by"),
            CreatedAt            = ToDateTime(r, "created_at"),
            UpdatedAt            = ToDateTime(r, "updated_at"),
        }).ToList();
        await UpsertAsync(_db.AttendanceMarks, am => am.Id, entities);
    }

    // =========================================================================
    // Phase 5 â€” audit tables
    // =========================================================================

    // CSV columns: id, user_id, login_time, ip_address, user_agent,
    //              success, created_at
    private async Task ImportLoginAuditAsync(string dir)
    {
        var rows = await ReadCsvAsync(Path.Combine(dir, "login_audit.csv"));
        var entities = rows.Select(r => new LoginAudit
        {
            Id        = ToLong(r, "id"),
            UserId    = ToLong(r, "user_id"),
            LoginTime = ToDateTime(r, "login_time"),
            IpAddress = Col(r, "ip_address"),
            UserAgent = Col(r, "user_agent"),
            Success   = ToBool(r, "success"),
            CreatedAt = ToDateTime(r, "created_at"),
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
            Id        = ToLong(r, "id"),
            UserId    = ToLong(r, "user_id"),
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
            Id         = ToLong(r, "id"),
            UserId     = ToLong(r, "user_id"),
            OldRoleId  = ToLong(r, "old_role_id"),
            NewRoleId  = ToLong(r, "new_role_id"),
            ChangedBy  = ToLong(r, "changed_by"),
            ChangeTime = ToDateTime(r, "change_time"),
            Reason     = NullableCol(r, "reason"),
            Metadata   = NullableCol(r, "metadata"),
        }).ToList();
        await UpsertAsync(_db.RoleChangeAudit, rca => rca.Id, entities);
    }

    // CSV columns: id, staff_assignment_id, staff_id, action, changed_by,
    //              change_time, old_values, new_values
    private async Task ImportStaffAssignmentAuditAsync(string dir)
    {
        var rows = await ReadCsvAsync(Path.Combine(dir, "staff_assignment_audit.csv"));
        var entities = rows.Select(r => new StaffAssignmentAudit
        {
            Id                = ToLong(r, "id"),
            StaffAssignmentId = ToLong(r, "staff_assignment_id"),
            StaffId           = ToLong(r, "staff_id"),
            Action            = Col(r, "action"),
            ChangedBy         = ToLong(r, "changed_by"),
            ChangeTime        = ToDateTime(r, "change_time"),
            OldValues         = NullableCol(r, "old_values"),
            NewValues         = NullableCol(r, "new_values"),
        }).ToList();
        await UpsertAsync(_db.StaffAssignmentAudit, saa => saa.Id, entities);
    }

    // CSV columns: id, staff_absence_id, staff_id, action, changed_by,
    //              change_time, old_values, new_values
    private async Task ImportStaffAbsenceAuditAsync(string dir)
    {
        var rows = await ReadCsvAsync(Path.Combine(dir, "staff_absence_audit.csv"));
        var entities = rows.Select(r => new StaffAbsenceAudit
        {
            Id             = ToLong(r, "id"),
            StaffAbsenceId = ToLong(r, "staff_absence_id"),
            StaffId        = ToLong(r, "staff_id"),
            Action         = Col(r, "action"),
            ChangedBy      = ToLong(r, "changed_by"),
            ChangeTime     = ToDateTime(r, "change_time"),
            OldValues      = NullableCol(r, "old_values"),
            NewValues      = NullableCol(r, "new_values"),
        }).ToList();
        await UpsertAsync(_db.StaffAbsenceAudit, saa => saa.Id, entities);
    }

    // CSV columns: id, staff_device_id, staff_id, action, changed_by,
    //              change_time, old_values, new_values
    private async Task ImportStaffDeviceAuditAsync(string dir)
    {
        var rows = await ReadCsvAsync(Path.Combine(dir, "staff_device_audit.csv"));
        var entities = rows.Select(r => new StaffDeviceAudit
        {
            Id            = ToLong(r, "id"),
            StaffDeviceId = ToLong(r, "staff_device_id"),
            StaffId       = ToLong(r, "staff_id"),
            Action        = Col(r, "action"),
            ChangedBy     = ToLong(r, "changed_by"),
            ChangeTime    = ToDateTime(r, "change_time"),
            OldValues     = NullableCol(r, "old_values"),
            NewValues     = NullableCol(r, "new_values"),
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
            Id                     = ToLong(r, "id"),
            StaffExternalAccountId = ToLong(r, "staff_external_account_id"),
            StaffId                = ToLong(r, "staff_id"),
            Action                 = Col(r, "action"),
            ChangedBy              = ToLong(r, "changed_by"),
            ChangeTime             = ToDateTime(r, "change_time"),
            OldValues              = NullableCol(r, "old_values"),
            NewValues              = NullableCol(r, "new_values"),
        }).ToList();
        await UpsertAsync(_db.StaffExternalAccountAudit, sea => sea.Id, entities);
    }

    // CSV columns: id, student_absence_id, student_id, action, changed_by,
    //              changed_at, field_name, old_value, new_value, notes
    private async Task ImportStudentAbsenceAuditAsync(string dir)
    {
        var rows = await ReadCsvAsync(Path.Combine(dir, "student_absence_audit.csv"));
        var entities = rows.Select(r => new StudentAbsenceAudit
        {
            Id               = ToLong(r, "id"),
            StudentAbsenceId = ToLong(r, "student_absence_id"),
            StudentId        = ToLong(r, "student_id"),
            Action           = Col(r, "action"),
            ChangedBy        = ToLong(r, "changed_by"),
            ChangedAt        = ToDateTime(r, "changed_at"),
            FieldName        = NullableCol(r, "field_name"),
            OldValue         = NullableCol(r, "old_value"),
            NewValue         = NullableCol(r, "new_value"),
            Notes            = NullableCol(r, "notes"),
        }).ToList();
        await UpsertAsync(_db.StudentAbsenceAudit, saa => saa.Id, entities);
    }

    // =========================================================================
    // Generic upsert
    // =========================================================================

    /// <summary>
    /// Upserts a batch of entities using their Id.
    /// - Rows whose Id already exists in the database are updated (all columns).
    /// - Rows whose Id is not yet in the database are inserted.
    /// The idExpr is an Expression so EF Core can translate the SELECT to SQL.
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
                ? EntityState.Modified  // â†’ SQL UPDATE
                : EntityState.Added;    // â†’ SQL INSERT (explicit Id)
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
    /// Each row is returned as a case-insensitive column-name â†’ value dictionary.
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

    // â”€â”€ Type-safe value extractors â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

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
