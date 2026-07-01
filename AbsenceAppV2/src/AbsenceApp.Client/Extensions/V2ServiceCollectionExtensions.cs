/*
===============================================================================
 File        : V2ServiceCollectionExtensions.cs
 Namespace   : AbsenceApp.Client.Extensions
 Author      : Michael
 Version     : 2.0.0
 Created     : 2026-03-22
 Updated     : 2026-06-02
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
   - 1.1.0  2026-03-26  Added IDesignSystemService / DesignSystemService
                         registration (Phase 1 placeholder, Singleton).
   - 1.2.0  2026-04-06  Option A: registered NavigationApiServiceV2 (Singleton);
                         NavigationServiceV2 constructor updated to take
                         NavigationApiServiceV2 (JSON loading removed).
   - 1.3.0  2026-04-06  Option A Phase 3: registered FeaturePermissionApiServiceV2
                         (Singleton). All feature permission checks now flow
                         through the API.
   - 1.4.0  2026-04-11  E15 additions: UserManagementApiServiceV2, ViewModels.
   - 1.5.0  2026-04-11  E16 Pages Registry: PagesApiServiceV2, ViewModels.
   - 1.6.0  2026-04-21  Added UserProfileViewModelV2 registration.
   - 1.7.0  2026-05-05  Student Absence Management: added StudentProfileApiServiceV2,
                         StudentProfileViewModelV2, StudentAbsenceFormViewModelV2,
                         StudentCalendarViewModelV2. Removed StudentDetailViewModelV2.
   - 1.8.0  2026-05-12  Phase A Design Token System: added
                         DesignTokenApiServiceV2 as Singleton.
   - 2.0.0  2026-06-02  Phase 2 Dynamic Token System Integration:
                         - Added MySQL IDbConnection registration (Scoped)
                         - Updated DesignSystemConfigService registration to use
                           factory injection for IDbConnection
                         - Fixed silent MAUI startup exit caused by missing DI
                           dependency resolution
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

using System.Data;
using MySqlConnector;
using AbsenceApp.Client.Services;
using AbsenceApp.Client.Services.ApiV2;
using AbsenceApp.Client.Services.ApiV2.Modules;
using AbsenceApp.Client.Services.DesignSystem;
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
    public static IServiceCollection AddAbsenceAppV2Framework(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // -----------------------------------------------------------------
        // HttpClient — used by BrandingServiceV2 and ApiClientV2 to load
        // local wwwroot JSON config files.
        // -----------------------------------------------------------------
        services.AddSingleton(_ => new HttpClient
        {
            BaseAddress = new Uri("http://localhost/")
        });

        // -----------------------------------------------------------------
        // Database connection for DesignSystemConfigService (Scoped)
        // -----------------------------------------------------------------
        services.AddScoped<IDbConnection>(sp =>
        {
            var connString = configuration["ConnectionStrings:AbsenceAppDatabase"];
            return new MySqlConnection(connString);
        });

        // -----------------------------------------------------------------
        // V2 Core services — Singleton: hold cross-page UI state
        // -----------------------------------------------------------------
        services.AddSingleton<DesignSystemConfigService>(sp =>
        {
            var db = sp.GetRequiredService<IDbConnection>();
            return new DesignSystemConfigService(db);
        });

        services.AddSingleton<AlertServiceV2>();
        services.AddSingleton<NavigationApiServiceV2>();
        services.AddSingleton<FeaturePermissionApiServiceV2>();
        services.AddSingleton<PermissionServiceV2>();
        services.AddSingleton<NavigationServiceV2>();
        services.AddSingleton<ThemeServiceV2>();
        services.AddSingleton<BrandingServiceV2>();
        services.AddSingleton<IDesignSystemService, DesignSystemService>();
        services.AddSingleton<DesignTokenApiServiceV2>();

        // -----------------------------------------------------------------
        // V2 Scoped services — new instance per Blazor scope (page lifetime)
        // -----------------------------------------------------------------
        services.AddScoped<ErrorHandlerV2>();
        services.AddScoped<TableConfigService>();

        // -----------------------------------------------------------------
        // V2 API services — Scoped
        // -----------------------------------------------------------------
        services.AddScoped<ApiClientV2>();
        services.AddScoped<StudentsApiServiceV2>();
        services.AddScoped<StaffApiServiceV2>();
        services.AddScoped<ClassesApiServiceV2>();
        services.AddScoped<AttendanceApiServiceV2>();
        services.AddScoped<AuditLogApiServiceV2>();
        services.AddScoped<SettingsApiServiceV2>();
        services.AddScoped<ParentsApiServiceV2>();
        services.AddScoped<UserManagementApiServiceV2>();
        services.AddScoped<PagesApiServiceV2>();
        services.AddScoped<StudentProfileApiServiceV2>();
        services.AddScoped<StaffProfileApiServiceV2>();

        // -----------------------------------------------------------------
        // V2 Table settings — Singleton
        // -----------------------------------------------------------------
        services.AddSingleton<TableSettingsFileService>();

        // -----------------------------------------------------------------
        // V2 ViewModels — Scoped
        // -----------------------------------------------------------------
        services.AddScoped<StudentsListViewModelV2>();
        services.AddScoped<StudentFormViewModelV2>();
        services.AddScoped<StudentProfileViewModelV2>();
        services.AddScoped<StudentAbsenceFormViewModelV2>();
        services.AddScoped<StudentCalendarViewModelV2>();
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
        services.AddScoped<UserListViewModelV2>();
        services.AddScoped<UserFormViewModelV2>();
        services.AddScoped<UserProfileViewModelV2>();
        services.AddScoped<PagesListViewModelV2>();
        services.AddScoped<PageFormViewModelV2>();
        services.AddScoped<StaffProfileViewModelV2>();
        services.AddScoped<ButtonsEditorViewModelV2>();

        return services;
    }
}
