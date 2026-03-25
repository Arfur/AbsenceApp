/*
===============================================================================
 File        : App.xaml.cs
 Namespace   : AbsenceApp.Client
 Author      : Michael
 Version     : 1.1.0
 Created     : 2026-03-13
 Updated     : 2026-03-18
-------------------------------------------------------------------------------
 Purpose     : Code-behind for the root MAUI Application class.
               Sets MainPage to the Blazor host ContentPage on startup.
               Registers a global unhandled-exception handler that writes
               crash details to the Desktop before WinUI can swallow them.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial implementation.
   - 1.1.0  2026-03-18  Added AppDomain.UnhandledException handler to log
                         startup crashes to Desktop/AbsenceApp_crash.log
                         before WinUI swallows them in CoreMessagingXP.dll.
-------------------------------------------------------------------------------
 Notes       :
   - The root ContentPage hosts the BlazorWebView declared in MainPage.xaml.
   - The crash log is written to the current user's Desktop for easy discovery.
===============================================================================
*/

namespace AbsenceApp.Client;

public partial class App : Application
{
    // =========================================================================
    // Constructor
    // =========================================================================

    public App()
    {
        static void WriteLog(string text)
        {
            var path = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                "AbsenceApp_crash.log");
            File.AppendAllText(path, $"[{DateTime.Now:HH:mm:ss}] {text}{Environment.NewLine}");
        }

        // Catch unhandled .NET domain exceptions (DI build failures, etc.)
        AppDomain.CurrentDomain.UnhandledException += (_, e) =>
            WriteLog($"AppDomain: {e.ExceptionObject}");

        // Catch async task exceptions that were never observed
        TaskScheduler.UnobservedTaskException += (_, e) =>
        {
            WriteLog($"UnobservedTask: {e.Exception}");
            e.SetObserved();
        };

        InitializeComponent();
        MainPage = new MainPage();
    }
}
