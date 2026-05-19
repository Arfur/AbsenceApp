/*
===============================================================================
 File        : MauiProgram.cs
 Namespace   : AbsenceApp.Client
 Author      : Michael
 Version     : 1.3.0
 Created     : 2026-03-13
 Updated     : 2026-05-12
-------------------------------------------------------------------------------
 Purpose     : MAUI application bootstrap. Builds the MauiApp host, registers
               the Blazor web view, loads the embedded appsettings.json, wires
               the full EF data layer via AddDataLayer(), and registers all
               application services and scoped ViewModels into the DI container.
               Wraps the entire build in a try-catch that writes any startup
               exception to C:\DevAbsence2\logs\AbsenceApp_crash.log before
               re-throwing, ensuring DI failures are visible.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial implementation.
   - 1.1.0  2026-03-17  Added SubjectListViewModel, SubjectDetailsViewModel,
                         and SubjectAddViewModel DI registrations.
   - 1.2.0  2026-03-18  Wrapped builder in try-catch crash logger so DI build
                         failures are written to Desktop/AbsenceApp_crash.log.
   - 1.2.1  2026-03-26  Changed AppStateService lifetime from Singleton to
                         Scoped to align with EF Core DbContext lifetime and
                         prevent concurrent DbContext usage during login.
   - 1.2.2  2026-04-04  Phase 3 Stabilisation Issue 1: reverted AppStateService
                         from Scoped back to Singleton. AppStateService holds
                         no DbContext reference; Scoped lifetime caused auth
                         state (IsAuthenticated) to reset on certain navigations,
                         causing redirect-to-login loops.
   - 1.2.3  2026-04-05  Added EntitlementsService as Scoped.
   - 1.2.4  2026-04-06  Option A Phase 3: removed EntitlementsService.
   - 1.2.5  2026-04-07  Crash log path changed from Desktop to
                         C:\DevAbsence2\logs (unified log directory).
   - 1.3.0  2026-05-12  Enabled full Blazor WebView logging (Debug + Console)
                         to surface hidden WebView2 render exceptions.
-------------------------------------------------------------------------------
 Notes       :
   - appsettings.json is embedded as a ManifestResource so MAUI can read it
     at runtime without requiring file-system access to the project directory.
   - All ViewModels are registered Scoped to match the Scoped lifetime of the
     EF Core DbContext and all repository/service dependencies.
===============================================================================
*/

using AbsenceApp.Client.Extensions;
using AbsenceApp.Client.Services;
using AbsenceApp.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AbsenceApp.Client;

public static class MauiProgram
{
    // =========================================================================
    // Bootstrap
    // =========================================================================

    public static MauiApp CreateMauiApp()
    {
        try
        {
            // -----------------------------------------------------------------
            // Host builder — fonts and Blazor web view
            // -----------------------------------------------------------------
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

            // -----------------------------------------------------------------
            // NEW: Enable Blazor WebView logging so hidden exceptions appear
            // -----------------------------------------------------------------
            builder.Logging.SetMinimumLevel(LogLevel.Debug);
            builder.Logging.AddDebug();

            // -----------------------------------------------------------------
            // Configuration — load appsettings.json from embedded resource
            // -----------------------------------------------------------------
            var assembly = typeof(MauiProgram).Assembly;
            using var appSettingsStream = assembly.GetManifestResourceStream("AbsenceApp.Client.appsettings.json");
            if (appSettingsStream is not null)
                builder.Configuration.AddJsonStream(appSettingsStream);

            // -----------------------------------------------------------------
            // Data layer — DbContext, EF repositories, and EF-backed services
            // -----------------------------------------------------------------
            builder.Services.AddDataLayer(
                builder.Configuration["ConnectionStrings:AbsenceAppDatabase"]);

            // -----------------------------------------------------------------
            // Application services
            // -----------------------------------------------------------------
            builder.Services.AddSingleton<AppStateService>();

            // -----------------------------------------------------------------
            // V2 UI Framework — all V2 services, API clients, and ViewModels
            // -----------------------------------------------------------------
            builder.Services.AddAbsenceAppV2Framework(builder.Configuration);

            // -----------------------------------------------------------------
            // Debug tooling
            // -----------------------------------------------------------------
#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
        catch (Exception ex)
        {
            // Write crash details to the unified logs directory.
            var dir  = @"C:\DevAbsence2\logs";
            var path = Path.Combine(dir, "AbsenceApp_crash.log");
            try { Directory.CreateDirectory(dir); } catch { }
            File.WriteAllText(path, ex.ToString());
            throw;
        }
    }
}
