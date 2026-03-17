/*
===============================================================================
 File        : MauiProgram.cs
 Namespace   : AbsenceApp.Client
 Author      : Michael
 Version     : 1.1.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : Bootstraps the .NET MAUI + Blazor hybrid application.
               Configures fonts, registers all DI services, and builds the
               MauiApp instance consumed by each platform entry point.
-------------------------------------------------------------------------------
 Description :
   Service registrations:
     IStudentService  → StudentService  (core business logic)
     IAbsenceService  → AbsenceService  (core business logic)
     StudentRepository / AbsenceRepository  (in-memory data stores)
     ClientStudentService / ClientAbsenceService  (Blazor-facing wrappers)
     MainViewModel  (shared view-model for page state)

   BlazorWebViewDeveloperTools and debug logging are enabled in DEBUG builds.

   EF Core:
     Registers AppDbContext using the connection string defined in
     appsettings.json (AbsenceAppDatabase).
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial implementation.
   - 1.1.0  2026-03-13  Added EF Core AppDbContext registration.
-------------------------------------------------------------------------------
 Notes       :
   - All services are registered as singletons to share state across pages.
   - DbContext is registered as scoped (default EF Core behaviour).
===============================================================================
*/

using AbsenceApp.Client.Services;
using AbsenceApp.Client.ViewModels;
using AbsenceApp.Core.Interfaces;
using AbsenceApp.Core.ViewModels;
using AbsenceApp.Data;
using AbsenceApp.Data.Services;
using AbsenceApp.Data.Repositories;
using Microsoft.Extensions.Logging;

namespace AbsenceApp.Client;

public static class MauiProgram
{
    // =========================================================================
    // Application bootstrap — creates and configures the MauiApp instance
    // =========================================================================

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();

        // =========================================================================
        // Data layer — registers AppDbContext + all EF repositories via extension
        // =========================================================================
        builder.Services.AddDataLayer(
            builder.Configuration["ConnectionStrings:AbsenceAppDatabase"]);

        // =========================================================================
        // Dependency injection registrations — all singletons; shared across pages
        // Legacy in-memory AbsenceRecord/Student flow: bind repos to interfaces
        // =========================================================================
        builder.Services.AddSingleton<IAbsenceRepository, AbsenceRepository>();
        builder.Services.AddSingleton<IStudentRepository, StudentRepository>();
        builder.Services.AddSingleton<IStudentService, StudentService>();
        builder.Services.AddSingleton<IAbsenceService, AbsenceService>();
        builder.Services.AddSingleton<ClientStudentService>();
        builder.Services.AddSingleton<ClientAbsenceService>();
        builder.Services.AddSingleton<MainViewModel>();
        builder.Services.AddSingleton<AppStateService>();

        // =========================================================================
        // Per-page Core ViewModels — each scoped to a dedicated page
        // =========================================================================
        builder.Services.AddSingleton<StudentsViewModel>();
        builder.Services.AddSingleton<StudentDetailsViewModel>();
        builder.Services.AddSingleton<AbsencesViewModel>();
        builder.Services.AddSingleton<ClassesViewModel>();
        builder.Services.AddSingleton<ClassDetailsViewModel>();
        builder.Services.AddSingleton<AbsenceDetailsViewModel>();
        builder.Services.AddSingleton<AuditLogViewModel>();
        builder.Services.AddSingleton<AttendanceViewModel>();
        builder.Services.AddSingleton<StaffViewModel>();

        builder.Services.AddSingleton<TableSettingsViewModel>();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
