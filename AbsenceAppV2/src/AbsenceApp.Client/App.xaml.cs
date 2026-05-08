/*
===============================================================================
 File        : App.xaml.cs
 Namespace   : AbsenceApp.Client
 Platform    : MAUI (Cross‑platform)
 Author      : Michael
 Version     : 1.4.2
 Created     : 2026-03-13
 Updated     : 2026-05-08
-------------------------------------------------------------------------------
 Purpose     : Root MAUI Application class.
               - Hosts the BlazorWebView via MainPage.xaml.
               - Registers global crash handlers for both synchronous and
                 asynchronous unhandled exceptions.
               - Writes crash logs to the unified C:\DevAbsence1\logs directory.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial implementation.
   - 1.1.0  2026-03-18  Added AppDomain.UnhandledException handler.
   - 1.2.0  2026-04-05  Injected EntitlementsService — superseded.
   - 1.3.0  2026-04-05  Removed EntitlementsService injection.
   - 1.4.0  2026-04-07  Crash log path changed to C:\DevAbsence1\logs.
   - 1.4.2  2026-05-08  Restored as MAUI Application; no logic changes.
-------------------------------------------------------------------------------
 Notes       :
   - MainPage hosts the BlazorWebView declared in MainPage.xaml.
   - Crash logs are written to C:\DevAbsence1\logs.
===============================================================================
*/

namespace AbsenceApp.Client;

public partial class App : Application
{
    public App()
    {
        static void WriteLog(string text)
        {
            var dir  = @"C:\DevAbsence1\logs";
            var path = Path.Combine(dir, "AbsenceApp_crash.log");
            try { Directory.CreateDirectory(dir); } catch { }
            File.AppendAllText(path, $"[{DateTime.Now:HH:mm:ss}] {text}{Environment.NewLine}");
        }

        AppDomain.CurrentDomain.UnhandledException += (_, e) =>
            WriteLog($"AppDomain: {e.ExceptionObject}");

        TaskScheduler.UnobservedTaskException += (_, e) =>
        {
            WriteLog($"UnobservedTask: {e.Exception}");
            e.SetObserved();
        };

        InitializeComponent();
        MainPage = new MainPage();
    }
}
