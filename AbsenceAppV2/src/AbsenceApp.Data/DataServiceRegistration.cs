/*
===============================================================================
 File        : DataServiceRegistration.cs
 Namespace   : AbsenceApp.Data
 Author      : Michael
 Version     : 1.2.0
 Created     : 2026-03-13
 Updated     : 2026-03-17
-------------------------------------------------------------------------------
 Purpose     : IServiceCollection extension that registers the complete data
               layer — DbContext, EF repositories, and EF-backed services —
               into the application's DI container in a single call.
-------------------------------------------------------------------------------
 Description :
   AddDataLayer(connectionString) registers:
     - AppDbContext              (Scoped — EF Core default)
     - IUserRepository           → UserRepository
     - IRoleRepository           → RoleRepository
     - IClassRepository          → ClassRepository
     - IAttendanceRepository     → AttendanceRepository
     - IAuditLogRepository       → AuditLogRepository
     - IClassService             → ClassService
     - IRoleService              → RoleService
     - IAuditLogService          → AuditLogService
   - IStudentFullViewService   → StudentFullViewService
     - IStaffFullViewService    → StaffFullViewService
     - IUserFullViewService     → UserFullViewService
     - IClassFullViewService    → ClassFullViewService
   Called from MauiProgram.CreateMauiApp() during application bootstrap.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial creation with repositories only.
   - 1.1.0  2026-03-13  Added IClassService, IRoleService, IAuditLogService.
   - 1.2.0  2026-03-17  Added 4 FullView projection services.
-------------------------------------------------------------------------------
 Notes       :
   - Legacy in-memory services (IStudentService, IAbsenceService) are
     registered separately in MauiProgram.cs as singletons.
===============================================================================
*/

using AbsenceApp.Core.Interfaces;
using AbsenceApp.Data.Context;
using AbsenceApp.Data.Repositories;
using AbsenceApp.Data.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AbsenceApp.Data;

public static class DataServiceRegistration
{
    // =========================================================================
    // AddDataLayer — registers full EF data layer into the DI container
    // =========================================================================

    public static IServiceCollection AddDataLayer(
        this IServiceCollection services,
        string? connectionString)
    {
        // DbContext — scoped lifetime (EF Core default)
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));

        // EF Core repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IClassRepository, ClassRepository>();
        services.AddScoped<IAttendanceRepository, AttendanceRepository>();
        services.AddScoped<IAuditLogRepository, AuditLogRepository>();
        services.AddScoped<IStudentRepository, StudentRepository>();
        services.AddScoped<IStaffRepository, StaffRepository>();
        services.AddScoped<IStaffAbsenceRepository, StaffAbsenceRepository>();
        services.AddScoped<IStudentAbsenceRepository, StudentAbsenceRepository>();
        services.AddScoped<IStudentContactRepository, StudentContactRepository>();
        services.AddScoped<IAttendanceRegisterRepository, AttendanceRegisterRepository>();
        services.AddScoped<IYearGroupRepository, YearGroupRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IJobTitleRepository, JobTitleRepository>();
        services.AddScoped<IHouseRepository, HouseRepository>();
        services.AddScoped<IAbsenceTypeRepository, AbsenceTypeRepository>();

        // EF-backed services
        services.AddScoped<IClassService, ClassService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IAuditLogService, AuditLogService>();
        services.AddScoped<IAttendanceService, AttendanceService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITableSettingsService, TableSettingsService>();
        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<IStaffService, StaffService>();
        services.AddScoped<IStaffAbsenceService, StaffAbsenceService>();
        services.AddScoped<IStudentAbsenceService, StudentAbsenceService>();
        services.AddScoped<IStudentContactService, StudentContactService>();
        services.AddScoped<IAttendanceRegisterService, AttendanceRegisterService>();
        services.AddScoped<IYearGroupService, YearGroupService>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<IJobTitleService, JobTitleService>();
        services.AddScoped<IHouseService, HouseService>();
        services.AddScoped<IAbsenceTypeService, AbsenceTypeService>();
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ISubjectService, SubjectService>();

        // Full-view projection services (cross-table FK → name resolution)
        services.AddScoped<IStudentFullViewService, StudentFullViewService>();
        services.AddScoped<IStaffFullViewService, StaffFullViewService>();
        services.AddScoped<IUserFullViewService, UserFullViewService>();
        services.AddScoped<IClassFullViewService, ClassFullViewService>();

        return services;
    }
}
