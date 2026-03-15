/*
===============================================================================
 File        : DataServiceRegistration.cs
 Namespace   : AbsenceApp.Data
 Author      : Michael
 Version     : 1.1.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
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
   Called from MauiProgram.CreateMauiApp() during application bootstrap.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial creation with repositories only.
   - 1.1.0  2026-03-13  Added IClassService, IRoleService, IAuditLogService.
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

        // EF-backed services
        services.AddScoped<IClassService, ClassService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IAuditLogService, AuditLogService>();
        services.AddScoped<IAttendanceService, AttendanceService>();
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}
