/*
===============================================================================
 File        : AppDbContext.cs
 Namespace   : AbsenceApp.Data.Context
 Author      : Michael
 Version     : 2.0.0
 Created     : Unknown
 Updated     : 2026-04-21
-------------------------------------------------------------------------------
 Purpose     : Primary Entity Framework Core DbContext for the AbsenceApp API.

               This context defines all application entities, their relationships,
               and database-level behavioural overrides. It is the single source
               of truth for schema mapping and cascade behaviour.

               Phase 2 introduces entitlement-based navigation and feature
               control. Entitlement entities are wired via a dedicated extension
               method to preserve modularity and audit safety.
-------------------------------------------------------------------------------
 Changes     :
   - 1.1.0  2026-04-05  Added entitlement entity configuration hook via
                         ConfigureEntitlements().
   - 1.2.0  2026-04-05  Added using AbsenceApp.Data.Configurations so the
                         ConfigureEntitlements() extension method resolves at
                         compile time. Closes pre-existing build error CS1061.
   - 1.3.0  2026-04-11  E15 User Management: added DbSets for AppPage,
                         RoleDefaultPagePermission, UserPageOverride, and
                         UserPagePermission. Added ConfigureUserManagement()
                         hook. Added IDENTITY exclusions for the four new
                         entities in the ValueGeneratedNever loop.
   - 2.0.0  2026-04-21  Absence domain redesign: replaced StaffAbsences,
                         StaffAbsenceAudit, StudentAbsences, StudentAbsenceAudit
                         with unified Absences, AbsenceAudit, AbsenceStatuses.
                         Added ValueGeneratedNever exemptions for the four new
                         AUTO_INCREMENT absence tables.
-------------------------------------------------------------------------------
 Notes       :
   - This file intentionally contains no business logic.
   - All behavioural changes must be explicit and auditable.
===============================================================================
*/

using AbsenceApp.Data.Configurations;
using AbsenceApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AbsenceApp.Data.Context;

public class AppDbContext : DbContext
{
    // =========================================================================
    // Construction
    // =========================================================================
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // =========================================================================
    // Phase 1 — Lookup / reference tables
    // =========================================================================
    public DbSet<School> Schools => Set<School>();
    public DbSet<JobTitle> JobTitles => Set<JobTitle>();
    public DbSet<JobGroup> JobGroups => Set<JobGroup>();
    public DbSet<RoleType> RoleTypes => Set<RoleType>();
    public DbSet<Responsibility> Responsibilities => Set<Responsibility>();
    public DbSet<AbsenceType>   AbsenceTypes   => Set<AbsenceType>();
    public DbSet<AbsenceStatus> AbsenceStatuses => Set<AbsenceStatus>();
    public DbSet<DeviceType> DeviceTypes => Set<DeviceType>();
    public DbSet<ExternalSystem> ExternalSystems => Set<ExternalSystem>();
    public DbSet<SystemEvent> SystemEvents => Set<SystemEvent>();
    public DbSet<Class> Classes => Set<Class>();

    // =========================================================================
    // Phase 2 — School-scoped structure
    // =========================================================================
    public DbSet<Phase> Phases => Set<Phase>();
    public DbSet<YearGroup> YearGroups => Set<YearGroup>();
    public DbSet<House> Houses => Set<House>();

    // =========================================================================
    // Phase 3 — Core people tables
    // =========================================================================
    public DbSet<User> Users => Set<User>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Staff> Staff => Set<Staff>();
    public DbSet<Student> Students => Set<Student>();

    // =========================================================================
    // Phase 4 — Extension / link tables
    // =========================================================================
    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
    public DbSet<ClassYearGroupAssignment> ClassYearGroupAssignments => Set<ClassYearGroupAssignment>();
    public DbSet<StaffAssignment> StaffAssignments => Set<StaffAssignment>();
    public DbSet<Absence> Absences => Set<Absence>();
    public DbSet<StaffDevice> StaffDevices => Set<StaffDevice>();
    public DbSet<StaffExternalAccount> StaffExternalAccounts => Set<StaffExternalAccount>();
    public DbSet<StudentContact> StudentContacts => Set<StudentContact>();
    public DbSet<StudentMedical> StudentMedical => Set<StudentMedical>();
    public DbSet<StudentFlag> StudentFlags => Set<StudentFlag>();
    public DbSet<AttendanceRegister> AttendanceRegisters => Set<AttendanceRegister>();
    public DbSet<AttendanceMark> AttendanceMarks => Set<AttendanceMark>();

    // =========================================================================
    // Phase 5 — Audit tables
    // =========================================================================
    public DbSet<LoginAudit> LoginAudit => Set<LoginAudit>();
    public DbSet<AccountVerificationEvent> AccountVerificationEvents => Set<AccountVerificationEvent>();
    public DbSet<RoleChangeAudit> RoleChangeAudit => Set<RoleChangeAudit>();
    public DbSet<StaffAssignmentAudit> StaffAssignmentAudit => Set<StaffAssignmentAudit>();
    public DbSet<StaffDeviceAudit> StaffDeviceAudit => Set<StaffDeviceAudit>();
    public DbSet<StaffExternalAccountAudit> StaffExternalAccountAudit => Set<StaffExternalAccountAudit>();
    public DbSet<AbsenceAudit> AbsenceAudit => Set<AbsenceAudit>();

    // =========================================================================
    // Application configuration
    // =========================================================================
    public DbSet<TablePageSetting> TablePageSettings => Set<TablePageSetting>();

    // =========================================================================
    // Phase 3 — In-app messaging + notifications (Header Nav Identity)
    // =========================================================================
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<AppNotification> AppNotifications => Set<AppNotification>();

    // =========================================================================
    // E15 — User management + page-level permission system
    // =========================================================================
    public DbSet<AppPage>                   AppPages                   => Set<AppPage>();
    public DbSet<RoleDefaultPagePermission> RoleDefaultPagePermissions => Set<RoleDefaultPagePermission>();
    public DbSet<UserPageOverride>          UserPageOverrides          => Set<UserPageOverride>();
    public DbSet<UserPagePermission>        UserPagePermissions        => Set<UserPagePermission>();

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

        // ---------------------------------------------------------------------
        // Disable IDENTITY on all integer/long primary keys so the CSV import
        // pipeline can insert explicit ID values from the CSV files.
        // ---------------------------------------------------------------------
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            // TablePageSetting uses SQL Server IDENTITY — its configuration
            // explicitly sets UseIdentityColumn() and ValueGeneratedOnAdd().
            if (entityType.ClrType == typeof(TablePageSetting)) continue;

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

        modelBuilder.Entity<AttendanceMark>()
            .HasOne<Student>()
            .WithMany()
            .HasForeignKey(a => a.StudentId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<StaffAssignment>()
            .HasOne<JobTitle>()
            .WithMany()
            .HasForeignKey(sa => sa.JobTitleId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<StaffAssignment>()
            .HasOne<JobGroup>()
            .WithMany()
            .HasForeignKey(sa => sa.JobGroupId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<StaffAssignment>()
            .HasOne<Department>()
            .WithMany()
            .HasForeignKey(sa => sa.DepartmentId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<StaffAssignmentAudit>()
            .HasOne<Staff>()
            .WithMany()
            .HasForeignKey(a => a.StaffId)
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

        modelBuilder.Entity<ClassYearGroupAssignment>()
            .HasOne<School>()
            .WithMany()
            .HasForeignKey(a => a.SchoolId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
