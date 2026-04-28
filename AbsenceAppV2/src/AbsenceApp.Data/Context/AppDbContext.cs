/*
===============================================================================
 File        : AppDbContext.cs
 Namespace   : AbsenceApp.Data.Context
 Author      : Michael
 Version     : 2.1.0
 Created     : Unknown
 Updated     : 2026-04-28
-------------------------------------------------------------------------------
 Purpose     : Primary Entity Framework Core DbContext for the AbsenceApp API.

               This context defines all application entities, their relationships,
               and database-level behavioural overrides. It is the single source
               of truth for schema mapping, table naming, and cascade behaviour.

               Phase-based grouping ensures deterministic schema organisation.
               E15 user-management entities are configured via a dedicated
               extension method to preserve modularity and audit safety.
-------------------------------------------------------------------------------
 Changes     :
   - 1.1.0  2026-04-05  Added entitlement entity configuration hook via
                         ConfigureEntitlements().
   - 1.2.0  2026-04-05  Added using AbsenceApp.Data.Configurations so the
                         ConfigureEntitlements() extension method resolves at
                         compile time. Closed build error CS1061.
   - 1.3.0  2026-04-11  E15 User Management: added DbSets for AppPage,
                         RoleDefaultPagePermission, UserPageOverride, and
                         UserPagePermission. Added ConfigureUserManagement()
                         hook and IDENTITY exemptions.
   - 2.0.0  2026-04-21  Absence domain redesign: replaced legacy absence tables
                         with unified Absences, AbsenceAudit, AbsenceStatuses.
                         Added AUTO_INCREMENT exemptions.
   - 2.0.1  2026-04-24  E15 alignment: confirmed PascalCase table usage and
                         DbSet exposure for all E15 entities. Ensured
                         ValueGeneratedNever exemptions remain correct.
   - 2.0.2  2026-04-25  Removed incorrect UserProfile table override to restore
                         [Table("user_profiles")] attribute behaviour.
   - 2.0.3  2026-04-25  Added StaffDepartment entity support and explicit table
                         mapping under Phase 3.
   - 2.1.0  2026-04-28  Full explicit table mapping for all entities across
                         Phases 1–6 and E15. Removed duplicate OnModelCreating
                         block, corrected structural braces, and finalised
                         deterministic schema mapping.
-------------------------------------------------------------------------------
 Notes       :
   - This file intentionally contains no business logic.
   - All table mappings are now explicit and deterministic.
   - All behavioural changes must remain explicit and auditable.
===============================================================================
*/

using AbsenceApp.Data.Configurations;
using AbsenceApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AbsenceApp.Data.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // =========================================================================
    // DbSets (unchanged)
    // =========================================================================
    public DbSet<School> Schools => Set<School>();
    public DbSet<JobTitle> JobTitles => Set<JobTitle>();
    public DbSet<JobGroup> JobGroups => Set<JobGroup>();
    public DbSet<RoleType> RoleTypes => Set<RoleType>();
    public DbSet<ResponsibilityType> ResponsibilityTypes => Set<ResponsibilityType>();
    public DbSet<AbsenceType> AbsenceTypes => Set<AbsenceType>();
    public DbSet<AbsenceStatus> AbsenceStatuses => Set<AbsenceStatus>();
    public DbSet<DeviceType> DeviceTypes => Set<DeviceType>();
    public DbSet<ExternalSystem> ExternalSystems => Set<ExternalSystem>();
    public DbSet<SystemEvent> SystemEvents => Set<SystemEvent>();
    public DbSet<TeachingGroup> TeachingGroups => Set<TeachingGroup>();
    public DbSet<Attendance> Attendance => Set<Attendance>();
    public DbSet<AuditLog> AuditLog => Set<AuditLog>();
    public DbSet<Feature> Feature => Set<Feature>();
    public DbSet<GlobalConfig> GlobalConfig => Set<GlobalConfig>();

    public DbSet<Phase> Phases => Set<Phase>();
    public DbSet<YearGroup> YearGroups => Set<YearGroup>();
    public DbSet<House> Houses => Set<House>();

    public DbSet<User> Users => Set<User>();
    public DbSet<StaffDepartment> StaffDepartments => Set<StaffDepartment>();
    public DbSet<Staff> Staff => Set<Staff>();
    public DbSet<Student> Students => Set<Student>();

    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
    public DbSet<ClassYearGroup> ClassYearGroups => Set<ClassYearGroup>();
    public DbSet<StaffDuty> StaffDuties => Set<StaffDuty>();
    public DbSet<Absence> Absences => Set<Absence>();
    public DbSet<StaffDevice> StaffDevices => Set<StaffDevice>();
    public DbSet<StaffExternalAccount> StaffExternalAccounts => Set<StaffExternalAccount>();
    public DbSet<StudentContact> StudentContacts => Set<StudentContact>();
    public DbSet<StudentMedical> StudentMedical => Set<StudentMedical>();
    public DbSet<StudentFlag> StudentFlags => Set<StudentFlag>();
    public DbSet<StaffAttendance> StaffAttendance => Set<StaffAttendance>();
    public DbSet<StaffContact> StaffContacts => Set<StaffContact>();
    public DbSet<StaffWorkLocation> StaffWorkLocations => Set<StaffWorkLocation>();
    public DbSet<StaffQualification> StaffQualifications => Set<StaffQualification>();
    public DbSet<StaffResponsibility> StaffResponsibilities => Set<StaffResponsibility>();
    public DbSet<StaffWorkingPattern> StaffWorkingPatterns => Set<StaffWorkingPattern>();
    public DbSet<ClassMembers> ClassMembers => Set<ClassMembers>();

    public DbSet<LoginAudit> LoginAudit => Set<LoginAudit>();
    public DbSet<AccountVerificationEvent> AccountVerificationEvents => Set<AccountVerificationEvent>();
    public DbSet<RoleChangeAudit> RoleChangeAudit => Set<RoleChangeAudit>();
    public DbSet<StaffDeviceAudit> StaffDeviceAudit => Set<StaffDeviceAudit>();
    public DbSet<StaffExternalAccountAudit> StaffExternalAccountAudit => Set<StaffExternalAccountAudit>();
    public DbSet<AbsenceAudit> AbsenceAudit => Set<AbsenceAudit>();

    public DbSet<Message> Messages => Set<Message>();
    public DbSet<AppNotification> AppNotifications => Set<AppNotification>();

    public DbSet<AppPage> AppPages => Set<AppPage>();
    public DbSet<RoleDefaultPagePermission> RoleDefaultPagePermissions => Set<RoleDefaultPagePermission>();
    public DbSet<UserPageOverride> UserPageOverrides => Set<UserPageOverride>();
    public DbSet<UserPagePermission> UserPagePermissions => Set<UserPagePermission>();
    public DbSet<RoleFeature> RoleFeature => Set<RoleFeature>();
    public DbSet<RoleMenuItem> RoleMenuItem => Set<RoleMenuItem>();
    public DbSet<MenuItem> MenuItems => Set<MenuItem>();
    public DbSet<MenuItemsGlobalConfig> MenuItemsGlobalConfig => Set<MenuItemsGlobalConfig>();
    public DbSet<UserFeatureOverride> UserFeatureOverride => Set<UserFeatureOverride>();
    public DbSet<UserRole> UserRole => Set<UserRole>();


// =========================================================================
// Model configuration
// =========================================================================
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    // ---------------------------------------------------------------------
    // Apply IEntityTypeConfiguration<T> mappings from this assembly
    // ---------------------------------------------------------------------
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

    // ---------------------------------------------------------------------
    // Phase 2 — Entitlement entity configuration
    // ---------------------------------------------------------------------
    modelBuilder.ConfigureEntitlements();

    // ---------------------------------------------------------------------
    // E15 — User management + page-level permission configuration
    // ---------------------------------------------------------------------
    modelBuilder.ConfigureUserManagement();

    // =========================================================================
    // Phase 1 — Lookup / reference tables
    // =========================================================================
    modelBuilder.Entity<School>().ToTable("schools");
    modelBuilder.Entity<JobTitle>().ToTable("staffjobtitles");
    modelBuilder.Entity<JobGroup>().ToTable("staffjobgroups");
    modelBuilder.Entity<RoleType>().ToTable("roletypes");
    modelBuilder.Entity<ResponsibilityType>().ToTable("responsibilitytypes");
    modelBuilder.Entity<AbsenceType>().ToTable("absencetypes");
    modelBuilder.Entity<AbsenceStatus>().ToTable("absencestatuses");
    modelBuilder.Entity<DeviceType>().ToTable("devicetypes");
    modelBuilder.Entity<ExternalSystem>().ToTable("externalsystems");
    modelBuilder.Entity<SystemEvent>().ToTable("systemevents");
    modelBuilder.Entity<TeachingGroup>().ToTable("teachinggroups");
    modelBuilder.Entity<Attendance>().ToTable("attendance");
    modelBuilder.Entity<AuditLog>().ToTable("auditlog");
    modelBuilder.Entity<GlobalConfig>().ToTable("globalconfigs");

    // =========================================================================
    // Phase 2 — School-scoped structure
    // =========================================================================
    modelBuilder.Entity<Phase>().ToTable("staffphases");
    modelBuilder.Entity<YearGroup>().ToTable("yeargroups");
    modelBuilder.Entity<House>().ToTable("houses");

    // =========================================================================
    // Phase 3 — Core people tables
    // =========================================================================
    modelBuilder.Entity<User>().ToTable("users");
    modelBuilder.Entity<StaffDepartment>().ToTable("staffdepartments");
    modelBuilder.Entity<Staff>().ToTable("staff");
    modelBuilder.Entity<Student>().ToTable("students");

    // =========================================================================
    // Phase 4 — Extension / link tables
    // =========================================================================
    modelBuilder.Entity<UserProfile>().ToTable("userprofiles");
    modelBuilder.Entity<ClassYearGroup>().ToTable("classyeargroups");
    modelBuilder.Entity<StaffDuty>().ToTable("staffduties");
    modelBuilder.Entity<Absence>().ToTable("absences");
    modelBuilder.Entity<StaffDevice>().ToTable("staffdevices");
    modelBuilder.Entity<StaffExternalAccount>().ToTable("staffexternalaccounts");
    modelBuilder.Entity<StudentContact>().ToTable("studentcontacts");
    modelBuilder.Entity<StudentMedical>().ToTable("studentmedicals");
    modelBuilder.Entity<StudentFlag>().ToTable("studentflags");
    modelBuilder.Entity<StaffAttendance>().ToTable("staffattendances");
    modelBuilder.Entity<StaffContact>().ToTable("staffcontacts");
    modelBuilder.Entity<StaffWorkLocation>().ToTable("staffworklocations");
    modelBuilder.Entity<StaffQualification>().ToTable("staffqualifications");
    modelBuilder.Entity<StaffResponsibility>().ToTable("staffresponsibilities");
    modelBuilder.Entity<StaffWorkingPattern>().ToTable("staffworkingpatterns");
    modelBuilder.Entity<ClassMembers>().ToTable("classmembers");

    // =========================================================================
    // Phase 5 — Audit tables
    // =========================================================================
    modelBuilder.Entity<LoginAudit>().ToTable("loginaudits");
    modelBuilder.Entity<AccountVerificationEvent>().ToTable("accountverificationevents");
    modelBuilder.Entity<RoleChangeAudit>().ToTable("rolechangeaudits");
    modelBuilder.Entity<StaffDeviceAudit>().ToTable("staffdeviceaudits");
    modelBuilder.Entity<StaffExternalAccountAudit>().ToTable("staffexternalaccountaudits");
    modelBuilder.Entity<AbsenceAudit>().ToTable("absenceaudits");

    // =========================================================================
    // Phase 6 — Messaging + notifications
    // =========================================================================
    modelBuilder.Entity<Message>().ToTable("messages");
    modelBuilder.Entity<AppNotification>().ToTable("appnotifications");

    // =========================================================================
    // E15 — User management + permissions
    // =========================================================================
    modelBuilder.Entity<AppPage>().ToTable("apppages");
    modelBuilder.Entity<RoleDefaultPagePermission>().ToTable("roledefaultpagepermissions");
    modelBuilder.Entity<UserPageOverride>().ToTable("userpageoverrides");
    modelBuilder.Entity<UserPagePermission>().ToTable("userpagepermissions");
    modelBuilder.Entity<RoleFeature>().ToTable("rolefeatures");
    modelBuilder.Entity<RoleMenuItem>().ToTable("rolemenuitems");
    modelBuilder.Entity<MenuItem>().ToTable("menuitems");
    modelBuilder.Entity<MenuItemsGlobalConfig>().ToTable("menuitemglobalconfigs");
    modelBuilder.Entity<UserFeatureOverride>().ToTable("userfeatureoverrides");
    modelBuilder.Entity<UserRole>().ToTable("userrole");

    // ---------------------------------------------------------------------
    // Prevent EF from inferring shadow Department FK on StaffAssignment
    // ---------------------------------------------------------------------
    modelBuilder.Entity<StaffDuty>().Ignore("DepartmentId");
    modelBuilder.Entity<StaffDuty>().Ignore("Department");

    // ---------------------------------------------------------------------
    // Disable IDENTITY on all integer/long primary keys so the CSV import
    // pipeline can insert explicit ID values from the CSV files.
    // ---------------------------------------------------------------------
    foreach (var entityType in modelBuilder.Model.GetEntityTypes())
    {
        // E15 entities use SQL Server IDENTITY — exempt from the blanket
        // ValueGeneratedNever override used by the CSV import pipeline.
        if (entityType.ClrType == typeof(AppPage))                   continue;
        if (entityType.ClrType == typeof(RoleDefaultPagePermission)) continue;
        if (entityType.ClrType == typeof(UserPageOverride))          continue;
        if (entityType.ClrType == typeof(UserPagePermission))        continue;

        // New Absence domain tables use AUTO_INCREMENT — exempt from the
        // ValueGeneratedNever blanket so EF reads the generated Id back.
        if (entityType.ClrType == typeof(AbsenceType))   continue;
        if (entityType.ClrType == typeof(AbsenceStatus)) continue;
        if (entityType.ClrType == typeof(Absence))       continue;
        if (entityType.ClrType == typeof(AbsenceAudit))  continue;

        var pk = entityType.FindPrimaryKey();
        if (pk == null) continue;

        foreach (var prop in pk.Properties)
        {
            if (prop.ClrType == typeof(long) || prop.ClrType == typeof(int))
            {
                modelBuilder.Entity(entityType.ClrType)
                    .Property(prop.Name)
                    .ValueGeneratedNever();
            }
        }
    }

    // ---------------------------------------------------------------------
    // SQL Server cascade-path overrides
    // ---------------------------------------------------------------------
    modelBuilder.Entity<Student>()
        .HasOne<School>()
        .WithMany()
        .HasForeignKey(s => s.SchoolId)
        .OnDelete(DeleteBehavior.NoAction);

    modelBuilder.Entity<StaffDeviceAudit>()
        .HasOne<Staff>()
        .WithMany()
        .HasForeignKey(a => a.StaffId)
        .OnDelete(DeleteBehavior.NoAction);

    modelBuilder.Entity<StaffExternalAccountAudit>()
        .HasOne<Staff>()
        .WithMany()
        .HasForeignKey(a => a.StaffId)
        .OnDelete(DeleteBehavior.NoAction);

    modelBuilder.Entity<AbsenceAudit>()
        .HasOne(a => a.OldStatus)
        .WithMany()
        .HasForeignKey(a => a.OldStatusId)
        .OnDelete(DeleteBehavior.NoAction);

    modelBuilder.Entity<AbsenceAudit>()
        .HasOne(a => a.NewStatus)
        .WithMany()
        .HasForeignKey(a => a.NewStatusId)
        .OnDelete(DeleteBehavior.NoAction);
    }
}