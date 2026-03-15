using AbsenceApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AbsenceApp.Data.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // -------------------------------------------------------------------------
    // Phase 1 — Lookup / reference tables
    // -------------------------------------------------------------------------
    public DbSet<School> Schools => Set<School>();
    public DbSet<JobTitle> JobTitles => Set<JobTitle>();
    public DbSet<JobGroup> JobGroups => Set<JobGroup>();
    public DbSet<RoleType> RoleTypes => Set<RoleType>();
    public DbSet<Responsibility> Responsibilities => Set<Responsibility>();
    public DbSet<AbsenceType> AbsenceTypes => Set<AbsenceType>();
    public DbSet<DeviceType> DeviceTypes => Set<DeviceType>();
    public DbSet<ExternalSystem> ExternalSystems => Set<ExternalSystem>();
    public DbSet<SystemEvent> SystemEvents => Set<SystemEvent>();
    public DbSet<Class> Classes => Set<Class>();

    // -------------------------------------------------------------------------
    // Phase 2 — School-scoped structure
    // -------------------------------------------------------------------------
    public DbSet<Phase> Phases => Set<Phase>();
    public DbSet<YearGroup> YearGroups => Set<YearGroup>();
    public DbSet<House> Houses => Set<House>();

    // -------------------------------------------------------------------------
    // Phase 3 — Core people tables
    // -------------------------------------------------------------------------
    public DbSet<User> Users => Set<User>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Staff> Staff => Set<Staff>();
    public DbSet<Student> Students => Set<Student>();

    // -------------------------------------------------------------------------
    // Phase 4 — Extension / link tables
    // -------------------------------------------------------------------------
    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
    public DbSet<ClassYearGroupAssignment> ClassYearGroupAssignments => Set<ClassYearGroupAssignment>();
    public DbSet<StaffAssignment> StaffAssignments => Set<StaffAssignment>();
    public DbSet<StaffAbsence> StaffAbsences => Set<StaffAbsence>();
    public DbSet<StaffDevice> StaffDevices => Set<StaffDevice>();
    public DbSet<StaffExternalAccount> StaffExternalAccounts => Set<StaffExternalAccount>();
    public DbSet<StudentAbsence> StudentAbsences => Set<StudentAbsence>();
    public DbSet<StudentContact> StudentContacts => Set<StudentContact>();
    public DbSet<StudentMedical> StudentMedical => Set<StudentMedical>();
    public DbSet<StudentFlag> StudentFlags => Set<StudentFlag>();
    public DbSet<AttendanceRegister> AttendanceRegisters => Set<AttendanceRegister>();
    public DbSet<AttendanceMark> AttendanceMarks => Set<AttendanceMark>();

    // -------------------------------------------------------------------------
    // Phase 5 — Audit tables
    // -------------------------------------------------------------------------
    public DbSet<LoginAudit> LoginAudit => Set<LoginAudit>();
    public DbSet<AccountVerificationEvent> AccountVerificationEvents => Set<AccountVerificationEvent>();
    public DbSet<RoleChangeAudit> RoleChangeAudit => Set<RoleChangeAudit>();
    public DbSet<StaffAssignmentAudit> StaffAssignmentAudit => Set<StaffAssignmentAudit>();
    public DbSet<StaffAbsenceAudit> StaffAbsenceAudit => Set<StaffAbsenceAudit>();
    public DbSet<StaffDeviceAudit> StaffDeviceAudit => Set<StaffDeviceAudit>();
    public DbSet<StaffExternalAccountAudit> StaffExternalAccountAudit => Set<StaffExternalAccountAudit>();
    public DbSet<StudentAbsenceAudit> StudentAbsenceAudit => Set<StudentAbsenceAudit>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        // -------------------------------------------------------------------------
        // Disable IDENTITY on all integer/long primary keys so the CSV import
        // pipeline can insert explicit ID values from the CSV files.
        // -------------------------------------------------------------------------
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
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

        // -------------------------------------------------------------------------
        // SQL Server cascade-path overrides
        // EF Core convention generates ON DELETE CASCADE for all required FKs.
        // SQL Server rejects any schema where multiple cascade paths reach the same
        // table.  The relationships below are set to NO ACTION to break those cycles;
        // the primary ownership cascade for each entity is left as CASCADE.
        // -------------------------------------------------------------------------

        // Attendance nav properties (User, Recorder, Class) are ignored in
        // AttendanceConfiguration because UserId/RecordedBy are int but User.Id is long.

        // School→YearGroup→Student AND School→Student = two paths.  Suppress direct.
        modelBuilder.Entity<Student>()
            .HasOne<School>()
            .WithMany()
            .HasForeignKey(s => s.SchoolId)
            .OnDelete(DeleteBehavior.NoAction);

        // Class→AttendanceRegister→AttendanceMark AND Class→Student→AttendanceMark.
        modelBuilder.Entity<AttendanceMark>()
            .HasOne<Student>()
            .WithMany()
            .HasForeignKey(a => a.StudentId)
            .OnDelete(DeleteBehavior.NoAction);

        // JobTitle/JobGroup/Department each reach StaffAssignment via Staff AND directly.
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

        // Each audit table has a direct FK to Staff/Student AND a FK to the parent
        // record (which itself has a FK to Staff/Student), creating two cascade paths.
        modelBuilder.Entity<StaffAbsenceAudit>()
            .HasOne<Staff>()
            .WithMany()
            .HasForeignKey(a => a.StaffId)
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
        modelBuilder.Entity<StudentAbsenceAudit>()
            .HasOne<Student>()
            .WithMany()
            .HasForeignKey(a => a.StudentId)
            .OnDelete(DeleteBehavior.NoAction);

        // School→YearGroup→ClassYearGroupAssignment AND School→ClassYearGroupAssignment direct.
        modelBuilder.Entity<ClassYearGroupAssignment>()
            .HasOne<School>()
            .WithMany()
            .HasForeignKey(a => a.SchoolId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
