/*
===============================================================================
 File        : AppDbContext.cs
 Namespace   : AbsenceApp.Data.Context
 Author      : Michael
 Version     : 2.5.0
 Created     : Unknown
 Updated     : 2026-05-12
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
                                 attribute-driven UserProfile mapping behaviour.
    - 2.0.3  2026-04-25  Interim StaffDepartment support (later removed during
                                 deleted-entity cleanup).
   - 2.1.0  2026-04-28  Full explicit table mapping for all entities across
                         Phases 1–6 and E15. Removed duplicate OnModelCreating
                         block, corrected structural braces, and finalised
                         deterministic schema mapping.
   - 2.2.0  2026-05-04  Phase 1 fixes: renamed DbSet<Class> to DbSet<TeachingGroup>;
                         renamed DbSet<ClassMember> to DbSet<ClassMembers>;
                         renamed Entity<ClassMember>().ToTable to Entity<ClassMembers>();
                         renamed Entity<Class>() block to Entity<TeachingGroup>().
                         Phase 2 fixes: removed all HasColumnType("bigint unsigned")
                         overrides from AbsenceType, AbsenceStatus, Absence, and
                         AbsenceAudit entity blocks. EF default long→bigint mapping
                         is correct; explicit unsigned overrides caused type mismatches.
   - 2.5.0  2026-05-12  Phase A Design Token System: added DbSet<DesignToken>,
                         called modelBuilder.ConfigureDesignTokens(), added
                         DesignTokens table mapping, and excluded DesignToken
                         from the ValueGeneratedNever loop.
   - 2.4.0  2026-05-05  Schema lock: removed DbSets and ToTable mappings for
                         deleted tables (systemevents, attendance, globalconfigs).
                         Table rolemenuitem was renamed to rolemenuitems — code
                         already mapped to rolemenuitems; no change needed here.
   - 2.3.0  2026-05-04  Fix Plan #2 Step 2: added DbSet<JobTitle> JobTitles and
                         DbSet<StaffDepartment> StaffDepartments. Both model types
                         exist; repositories (JobTitleRepository, DepartmentRepository)
                         reference these DbSet names which were missing from the context.
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
    // DbSets - Phase 1 Lookup / reference tables
    // =========================================================================
    public DbSet<School> Schools { get; set; }
    public DbSet<RoleType> RoleTypes { get; set; }
    public DbSet<ResponsibilityType> ResponsibilityTypes { get; set; }
    public DbSet<AbsenceType> AbsenceTypes { get; set; }
    public DbSet<AbsenceStatus> AbsenceStatuses { get; set; }
    public DbSet<DeviceType> DeviceTypes { get; set; }
    public DbSet<ExternalSystem> ExternalSystems { get; set; }
    public DbSet<AuditLog> AuditLog { get; set; }
    public DbSet<Phase> Phases { get; set; }

    // =========================================================================
    // DbSets - Phase 2 School-scoped structure
    // =========================================================================
    public DbSet<YearGroup> YearGroups { get; set; }
    public DbSet<House> Houses { get; set; }

    // =========================================================================
    // DbSets - Phase 3 Core people tables
    // =========================================================================
    public DbSet<User> Users { get; set; }
    public DbSet<Staff> Staff { get; set; }
    public DbSet<Student> Students { get; set; }

    // =========================================================================
    // DbSets - Phase 4 Extension / link tables
    // =========================================================================
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<ClassYearGroup> ClassYearGroups { get; set; }
    public DbSet<Absence> Absences { get; set; }
    public DbSet<StaffDevice> StaffDevices { get; set; }
    public DbSet<StaffExternalAccount> StaffExternalAccounts { get; set; }
    public DbSet<StudentContact> StudentContacts { get; set; }
    public DbSet<StudentMedical> StudentMedical { get; set; }
    public DbSet<StudentFlag> StudentFlags { get; set; }
    public DbSet<StaffAttendance> StaffAttendance { get; set; }
    public DbSet<StaffContact> StaffContacts { get; set; }
    public DbSet<StaffWorkLocation> StaffWorkLocations { get; set; }
    public DbSet<StaffQualification> StaffQualifications { get; set; }
    public DbSet<StaffResponsibility> StaffResponsibilities { get; set; }
    public DbSet<StaffWorkingPattern> StaffWorkingPatterns { get; set; }
    public DbSet<ClassMembers> ClassMembers { get; set; }
    public DbSet<StaffPhase> StaffPhases { get; set; }

    // =========================================================================
    // DbSets - Phase 5 Audit tables
    // =========================================================================
    public DbSet<LoginAudit> LoginAudit { get; set; }
    public DbSet<AccountVerificationEvent> AccountVerificationEvents { get; set; }
    public DbSet<RoleChangeAudit> RoleChangeAudit { get; set; }
    public DbSet<StaffDeviceAudit> StaffDeviceAudit { get; set; }
    public DbSet<StaffExternalAccountAudit> StaffExternalAccountAudit { get; set; }
    public DbSet<AbsenceAudit> AbsenceAudit { get; set; }

    // =========================================================================
    // DbSets - Phase 6 Messaging + notifications
    // =========================================================================
    public DbSet<Message> Messages { get; set; }
    public DbSet<AppNotification> AppNotifications { get; set; }

    // =========================================================================
    // DbSets - E15 User management + permissions
    // =========================================================================
    public DbSet<AppPage> AppPages { get; set; }
    public DbSet<RoleDefaultPagePermission> RoleDefaultPagePermissions { get; set; }
    public DbSet<UserPageOverride> UserPageOverrides { get; set; }
    public DbSet<UserPagePermission> UserPagePermissions { get; set; }
    public DbSet<RoleFeature> RoleFeature { get; set; }
    public DbSet<RoleMenuItem> RoleMenuItem { get; set; }
    public DbSet<MenuItem> MenuItems { get; set; }
    public DbSet<MenuItemsGlobalConfig> MenuItemsGlobalConfig { get; set; }
    public DbSet<UserFeatureOverride> UserFeatureOverride { get; set; }
    public DbSet<UserRole> UserRole { get; set; }

    // =========================================================================
    // DbSets - Additional Academic / Staff entities you added
    // =========================================================================
    public DbSet<TeachingGroup> Classes { get; set; }
    public DbSet<StaffAssignment> StaffAssignments { get; set; }
    public DbSet<StaffAttribute> StaffAttributes { get; set; }
    public DbSet<StaffAttributeType> StaffAttributeTypes { get; set; }
    public DbSet<JobTitle> JobTitles { get; set; }
    public DbSet<StaffDepartment> StaffDepartments { get; set; }
    public DbSet<JobGroup> JobGroups { get; set; }

    // =========================================================================
    // DbSets - Profile Notes
    // =========================================================================
    public DbSet<StudentNote> StudentNotes { get; set; }
    public DbSet<StaffNote> StaffNotes { get; set; }
    public DbSet<UserNote> UserNotes { get; set; }

    // =========================================================================
    // DbSets - Design System
    // =========================================================================
    public DbSet<DesignToken> DesignTokens { get; set; }

    // =========================================================================
    // DbSets - User-scoped link tables
    // =========================================================================
    public DbSet<UserContact> UserContacts { get; set; }
    public DbSet<UserDevice> UserDevices { get; set; }
    public DbSet<UserExternalAccount> UserExternalAccounts { get; set; }
    public DbSet<UserPermissionOverride> UserPermissionOverrides { get; set; }

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
        // Design System — token configuration and seed data
        // ---------------------------------------------------------------------
        modelBuilder.ConfigureDesignTokens();

        // ---------------------------------------------------------------------
        // E15 — User management + page-level permission configuration
        // ---------------------------------------------------------------------
        modelBuilder.ConfigureUserManagement();

        // =========================================================================
        // Phase 1 — Lookup / reference tables
        // =========================================================================
        modelBuilder.Entity<School>().ToTable("schools");
        modelBuilder.Entity<RoleType>().ToTable("roles");
        modelBuilder.Entity<ResponsibilityType>().ToTable("responsibilitytypes");
        modelBuilder.Entity<AbsenceType>(entity =>
        {
            entity.ToTable("absencetypes");
            entity.HasKey(a => a.Id);
        });
        modelBuilder.Entity<AbsenceStatus>(entity =>
        {
            entity.ToTable("absencestatuses");
            entity.HasKey(a => a.Id);
        });
        modelBuilder.Entity<DeviceType>().ToTable("devicetypes");
        modelBuilder.Entity<ExternalSystem>().ToTable("externalsystems");
        modelBuilder.Entity<AuditLog>().ToTable("auditlog");
        modelBuilder.Entity<Phase>().ToTable("phases");

        // =========================================================================
        // Phase 2 — School-scoped structure
        // =========================================================================
        modelBuilder.Entity<YearGroup>().ToTable("yeargroups");
        modelBuilder.Entity<House>().ToTable("houses");

        // =========================================================================
        // Phase 3 — Core people tables
        // =========================================================================
        modelBuilder.Entity<User>().ToTable("users");
        modelBuilder.Entity<User>()
            .Property(u => u.Id)
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<Staff>().ToTable("staff");
        modelBuilder.Entity<Student>().ToTable("students");

        // =========================================================================
        // Phase 4 — Extension / link tables
        // =========================================================================
        modelBuilder.Entity<UserProfile>().ToTable("userprofiles");
        modelBuilder.Entity<UserProfile>()
            .Property(p => p.Id)
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<ClassYearGroup>().ToTable("classyeargroups");
        modelBuilder.Entity<Absence>(entity =>
        {
            entity.ToTable("absences");
            entity.HasKey(a => a.Id);
        });
        modelBuilder.Entity<StaffDevice>().ToTable("staffdevices");
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
        modelBuilder.Entity<LoginAudit>()
            .Property(a => a.Id)
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<AccountVerificationEvent>(entity =>
        {
            entity.ToTable("accountverificationevents");
            entity.HasKey(a => a.Id);

            entity.Property(a => a.Id)
                .HasColumnType("bigint")
                .IsRequired()
                .ValueGeneratedOnAdd();

            entity.Property(a => a.UserId)
                .HasColumnType("bigint")
                .IsRequired();
        });
        modelBuilder.Entity<RoleChangeAudit>().ToTable("rolechangeaudits");
        modelBuilder.Entity<StaffDeviceAudit>().ToTable("staffdeviceaudits");
        modelBuilder.Entity<StaffExternalAccountAudit>().ToTable("staffexternalaccountsaudits");
        modelBuilder.Entity<AbsenceAudit>(entity =>
        {
            entity.ToTable("absenceaudits");
            entity.HasKey(a => a.Id);
        });

        // =========================================================================
        // Phase 6 — Messaging + notifications
        // =========================================================================
        modelBuilder.Entity<Message>().ToTable("messages");
        modelBuilder.Entity<Message>()
            .Property(m => m.Id)
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<AppNotification>().ToTable("appnotifications");
        modelBuilder.Entity<AppNotification>()
            .Property(n => n.Id)
            .ValueGeneratedOnAdd();

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

        // =========================================================================
        // DbSets - Additional Academic / Staff entities you added
        // =========================================================================
        modelBuilder.Entity<TeachingGroup>(entity =>
        {
            entity.ToTable("classes");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id)
                .HasColumnType("int")
                .IsRequired();
        });
        modelBuilder.Entity<StaffAssignment>(entity =>
        {
            entity.ToTable("staffassignments");
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Id)
                .HasColumnType("int")
                .IsRequired();
            entity.Property(a => a.StaffId)
                .HasColumnType("int")
                .IsRequired();
            entity.Property(a => a.LocationId)
                .HasColumnType("int")
                .IsRequired();
        });
        modelBuilder.Entity<StaffAttribute>(entity =>
        {
            entity.ToTable("staffattributes");
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Id)
                .HasColumnType("int")
                .IsRequired();
            entity.Property(a => a.AttributeTypeId)
                .HasColumnType("int unsigned")
                .IsRequired();
        });
        modelBuilder.Entity<StaffAttributeType>(entity =>
        {
            entity.ToTable("staffattributetypes");
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Id)
                .HasColumnType("int unsigned")
                .IsRequired();
        });
        modelBuilder.Entity<StaffPhase>(entity =>
        {
            entity.ToTable("staffphases");
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Id)
                .HasColumnType("int")
                .IsRequired();
        });

        // Lookup table mappings
        modelBuilder.Entity<JobTitle>().ToTable("jobtitles");
        modelBuilder.Entity<StaffDepartment>().ToTable("staffdepartments");
        modelBuilder.Entity<JobGroup>().ToTable("jobgroups");

        // Profile notes
        modelBuilder.Entity<StudentNote>().ToTable("studentnotes");
        modelBuilder.Entity<StaffNote>().ToTable("staffnotes");
        modelBuilder.Entity<UserNote>().ToTable("usernotes");

        // User-scoped link tables
        modelBuilder.Entity<UserContact>().ToTable("usercontacts");
        modelBuilder.Entity<UserDevice>().ToTable("userdevices");
        modelBuilder.Entity<UserExternalAccount>().ToTable("userexternalaccounts");
        modelBuilder.Entity<UserPermissionOverride>().ToTable("userpermissionoverrides");

        // Design System
        modelBuilder.Entity<DesignToken>().ToTable("DesignTokens");

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

            // Runtime-created entities use AUTO_INCREMENT — exempt from the
            // ValueGeneratedNever blanket so EF reads generated keys back.
            if (entityType.ClrType == typeof(User))                     continue;
            if (entityType.ClrType == typeof(UserProfile))              continue;
            if (entityType.ClrType == typeof(LoginAudit))               continue;
            if (entityType.ClrType == typeof(AccountVerificationEvent)) continue;
            if (entityType.ClrType == typeof(Message))                  continue;
            if (entityType.ClrType == typeof(AppNotification))          continue;
            if (entityType.ClrType == typeof(Role))                     continue;

            // New Absence domain tables use AUTO_INCREMENT — exempt from the
            // ValueGeneratedNever blanket so EF reads the generated Id back.
            if (entityType.ClrType == typeof(AbsenceType))   continue;
            if (entityType.ClrType == typeof(AbsenceStatus)) continue;
            if (entityType.ClrType == typeof(Absence))       continue;
            if (entityType.ClrType == typeof(AbsenceAudit))  continue;

            // Profile Notes + User link tables use AUTO_INCREMENT
            if (entityType.ClrType == typeof(StudentNote))           continue;
            if (entityType.ClrType == typeof(StaffNote))             continue;
            if (entityType.ClrType == typeof(UserNote))              continue;
            if (entityType.ClrType == typeof(UserContact))           continue;
            if (entityType.ClrType == typeof(UserDevice))            continue;
            if (entityType.ClrType == typeof(UserExternalAccount))   continue;
            if (entityType.ClrType == typeof(UserPermissionOverride)) continue;
            if (entityType.ClrType == typeof(JobGroup))              continue;
            if (entityType.ClrType == typeof(JobTitle))              continue;
            if (entityType.ClrType == typeof(StaffDepartment))       continue;

            // Design System tokens use AUTO_INCREMENT
            if (entityType.ClrType == typeof(DesignToken))           continue;

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