/*
===============================================================================
 File        : V2ServiceCollectionExtensions.cs
 Namespace   : AbsenceApp.Client.Extensions
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-22
 Updated     : 2026-03-22
-------------------------------------------------------------------------------
 Purpose     : IServiceCollection extension that registers every V2 framework
               service, API client, and ViewModel in a single call.
               Designed so any .NET MAUI + Blazor Hybrid app can adopt the V2
               UI framework by calling:

                   builder.Services.AddAbsenceAppV2Framework(configuration);

               V1 services are NOT touched here — V1 and V2 are fully
               independent and can be registered in any order.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-22  Initial extraction from MauiProgram.cs (Phase 11).
-------------------------------------------------------------------------------
 Notes       :
   - HttpClient is registered as a singleton for BrandingServiceV2 and
     ApiClientV2 to load local wwwroot JSON files. MAUI Blazor Hybrid does
     not ship Microsoft.Extensions.Http so AddHttpClient<T> is unavailable;
     a plain HttpClient singleton is the correct pattern here.
   - All ViewModels are Scoped to align with Scoped EF Core DbContext.
   - All core V2 services (Alert, Navigation, Theme, Branding) are Singletons
     because they hold UI state that must survive page navigation.
===============================================================================
*/

using AbsenceApp.Client.Services;
using AbsenceApp.Client.Services.ApiV2;
using AbsenceApp.Client.Services.ApiV2.Modules;
using AbsenceApp.Client.Services.TableV2;
using AbsenceApp.Client.Services.Theming;
using AbsenceApp.Client.ViewModels.V2;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AbsenceApp.Client.Extensions;

/// <summary>
/// Registers all V2 framework services, API clients, and ViewModels.
/// Call from MauiProgram.cs: builder.Services.AddAbsenceAppV2Framework(config);
/// </summary>
public static class V2ServiceCollectionExtensions
{
    // =========================================================================
    // Extension method
    // =========================================================================

    public static IServiceCollection AddAbsenceAppV2Framework(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // -----------------------------------------------------------------
        // HttpClient — used by BrandingServiceV2 and ApiClientV2 to load
        // local wwwroot JSON config files. A singleton is sufficient because
        // BaseAddress never changes in a MAUI Hybrid (no real HTTP server).
        // -----------------------------------------------------------------
        services.AddSingleton(_ => new HttpClient
        {
            BaseAddress = new Uri("http://localhost/")
        });

        // -----------------------------------------------------------------
        // V2 Core services — Singleton: hold cross-page UI state
        // -----------------------------------------------------------------
        services.AddSingleton<DesignSystemConfigService>();
        services.AddSingleton<AlertServiceV2>();
        services.AddSingleton<NavigationServiceV2>();
        services.AddSingleton<ThemeServiceV2>();
        services.AddSingleton<BrandingServiceV2>();

        // -----------------------------------------------------------------
        // V2 Scoped services — new instance per Blazor scope (page lifetime)
        // -----------------------------------------------------------------
        services.AddScoped<ErrorHandlerV2>();
        services.AddScoped<TableConfigService>();

        // -----------------------------------------------------------------
        // V2 API services — Scoped: one per Blazor scope (page lifetime)
        // -----------------------------------------------------------------
        services.AddScoped<ApiClientV2>();
        services.AddScoped<StudentsApiServiceV2>();
        services.AddScoped<StaffApiServiceV2>();
        services.AddScoped<ClassesApiServiceV2>();
        services.AddScoped<AttendanceApiServiceV2>();
        services.AddScoped<AuditLogApiServiceV2>();
        services.AddScoped<SettingsApiServiceV2>();

        // -----------------------------------------------------------------
        // V2 ViewModels — Scoped to align with Scoped EF Core services
        // -----------------------------------------------------------------
        services.AddScoped<StudentsListViewModelV2>();
        services.AddScoped<StudentDetailViewModelV2>();
        services.AddScoped<StudentFormViewModelV2>();
        services.AddScoped<StaffListViewModelV2>();
        services.AddScoped<StaffDetailViewModelV2>();
        services.AddScoped<StaffFormViewModelV2>();
        services.AddScoped<ClassesListViewModelV2>();
        services.AddScoped<ClassDetailViewModelV2>();
        services.AddScoped<ClassFormViewModelV2>();
        services.AddScoped<AttendanceListViewModelV2>();
        services.AddScoped<AttendanceDetailViewModelV2>();
        services.AddScoped<AttendanceFormViewModelV2>();
        services.AddScoped<AuditLogListViewModelV2>();
        services.AddScoped<AuditLogDetailViewModelV2>();
        services.AddScoped<SettingsModuleViewModelV2>();
        services.AddScoped<TableSettingsViewModelV2>();

        return services;
    }
}
