/*
===============================================================================
 File        : MauiProgram.cs
 Namespace   : AbsenceApp.Client
 Author      : Michael
 Version     : 1.5.3
 Created     : 2026-03-13
 Updated     : 2026-07-01
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
                         Scoped to align with EF Core DbContext lifetime.
   - 1.2.2  2026-04-04  Phase 3 Stabilisation Issue 1: reverted AppStateService
                         from Scoped back to Singleton.
   - 1.2.3  2026-04-05  Added EntitlementsService as Scoped.
   - 1.2.4  2026-04-06  Option A Phase 3: removed EntitlementsService.
   - 1.2.5  2026-04-07  Crash log path changed to C:\DevAbsence2\logs.
   - 1.3.0  2026-05-12  Enabled full Blazor WebView logging.
   - 1.4.0  2026-06-02  Added startup validation call to GetComponentsAsync().
   - 1.5.0  2026-06-02  Phase 2: Added RegenerateDesignSystemAsync() call so
                         components.json is dynamically rebuilt at startup.
   - 1.5.1  2026-06-02  Added full debug tracing + awaited regeneration to
                         surface exceptions and ensure components.json updates.
   - 1.5.2  2026-06-02  FIX: Restored non-async signature and wrapped
                         RegenerateDesignSystemAsync() in Task.Run with
                         full debug tracing.
   - 1.5.3  2026-07-01  Added AddDbContextFactory<AppDbContext> for safe EF Core
                         usage in Blazor/Maui Hybrid to prevent DbContext
                         concurrency exceptions.
-------------------------------------------------------------------------------
 Notes       :
   - appsettings.json is embedded as a ManifestResource.
   - All ViewModels are Scoped to match EF Core DbContext lifetime.
===============================================================================
*/

using AbsenceApp.Client.Extensions;
using AbsenceApp.Client.Services;
using AbsenceApp.Data;
using AbsenceApp.Data.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AbsenceApp.Client;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        try
        {
            Console.WriteLine("MAUI: Entering CreateMauiApp()");
            Debug.WriteLine("MAUI: Entering CreateMauiApp()");

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

            builder.Logging.SetMinimumLevel(LogLevel.Debug);
            builder.Logging.AddDebug();

            Console.WriteLine("MAUI: Loading embedded appsettings.json");
            Debug.WriteLine("MAUI: Loading embedded appsettings.json");

            var assembly = typeof(MauiProgram).Assembly;
            using var appSettingsStream =
                assembly.GetManifestResourceStream("AbsenceApp.Client.appsettings.json");
            if (appSettingsStream is not null)
                builder.Configuration.AddJsonStream(appSettingsStream);

            Console.WriteLine("MAUI: Registering Data Layer");
            Debug.WriteLine("MAUI: Registering Data Layer");

            builder.Services.AddDataLayer(
                builder.Configuration["ConnectionStrings:AbsenceAppDatabase"]);

            // -----------------------------------------------------------------
            // NEW: Add DbContextFactory for safe EF Core usage in Blazor/Maui
            // -----------------------------------------------------------------
            builder.Services.AddDbContextFactory<AppDbContext>(options =>
            {
                options.UseMySql(
                    builder.Configuration["ConnectionStrings:AbsenceAppDatabase"],
                    new MariaDbServerVersion(new Version(10, 4, 32)));
            });

            builder.Services.AddSingleton<AppStateService>();
            builder.Services.AddAbsenceAppV2Framework(builder.Configuration);

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            Console.WriteLine("MAUI: Builder.Build() reached");
            Debug.WriteLine("MAUI: Builder.Build() reached");

            var app = builder.Build();

            Console.WriteLine("MAUI: App created");
            Debug.WriteLine("MAUI: App created");

            // -----------------------------------------------------------------
            // Phase 2 — Dynamic components.json regeneration (safe async)
            // -----------------------------------------------------------------
            Console.WriteLine("MAUI: Resolving DesignSystemConfigService");
            Debug.WriteLine("MAUI: Resolving DesignSystemConfigService");

            var designSystem = app.Services.GetRequiredService<DesignSystemConfigService>();

            Console.WriteLine("MAUI: Scheduling RegenerateDesignSystemAsync()");
            Debug.WriteLine("MAUI: Scheduling RegenerateDesignSystemAsync()");

            Task.Run(async () =>
            {
                try
                {
                    Console.WriteLine("MAUI: RegenerateDesignSystemAsync() starting");
                    Debug.WriteLine("MAUI: RegenerateDesignSystemAsync() starting");

                    await designSystem.RegenerateDesignSystemAsync();

                    Console.WriteLine("MAUI: RegenerateDesignSystemAsync() completed");
                    Debug.WriteLine("MAUI: RegenerateDesignSystemAsync() completed");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("MAUI: ERROR in RegenerateDesignSystemAsync()");
                    Console.WriteLine(ex.ToString());

                    Debug.WriteLine("MAUI: ERROR in RegenerateDesignSystemAsync()");
                    Debug.WriteLine(ex.ToString());
                }
            });

            return app;
        }
        catch (Exception ex)
        {
            var dir = @"C:\DevAbsence2\logs";
            var path = Path.Combine(dir, "AbsenceApp_crash.log");
            try { Directory.CreateDirectory(dir); } catch { }
            File.WriteAllText(path, ex.ToString());

            Console.WriteLine("MAUI: Startup exception written to crash log");
            Debug.WriteLine("MAUI: Startup exception written to crash log");

            throw;
        }
    }
}
