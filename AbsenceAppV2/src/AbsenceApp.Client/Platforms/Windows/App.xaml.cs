/*
===============================================================================
 File        : App.xaml.cs
 Namespace   : AbsenceApp.Client.WinUI
 Platform    : Windows (WinUI 3)
 Author      : Michael
 Version     : 2.1.1
 Created     : 2026-03-13
 Updated     : 2026-05-08
-------------------------------------------------------------------------------
 Purpose     : Windows (WinUI 3) application code-behind.
               Initialises the WinUI application, delegates MauiApp creation
               to MauiProgram.CreateMauiApp(), and configures system-tray
               behaviour via WindowsTrayIcon.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial implementation.
   - 2.0.0  2026-03-17  Added system-tray support via WindowsTrayIcon.
   - 2.1.0  2026-03-18  Tray setup robustness improvements.
   - 2.1.1  2026-05-08  Restored WinUI namespace AbsenceApp.Client.WinUI.
-------------------------------------------------------------------------------
 Notes       :
   None.
===============================================================================
*/

using Microsoft.UI.Dispatching;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;

namespace AbsenceApp.Client.WinUI;

public partial class App : MauiWinUIApplication
{
    private WindowsTrayIcon?           _tray;
    private Microsoft.UI.Xaml.Window?  _nativeWindow;
    private bool                       _isQuitting;
    private const string               AppAumid = "AbsenceApp.Client";

    public App() => InitializeComponent();

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);

        DispatcherQueue.GetForCurrentThread().TryEnqueue(() =>
        {
            RegisterAumid();
            try
            {
                _nativeWindow =
                    Microsoft.Maui.Controls.Application.Current?
                        .Windows.FirstOrDefault()?
                        .Handler?.PlatformView
                    as Microsoft.UI.Xaml.Window;

                if (_nativeWindow is null)
                {
                    AppDiag("_nativeWindow is NULL — tray cannot be created");
                    return;
                }

                AppDiag($"_nativeWindow resolved. HashCode={_nativeWindow.GetHashCode()}");

                _tray = new WindowsTrayIcon(_nativeWindow, "AbsenceApp")
                {
                    OnOpen = ShowApp,
                    OnHide = HideApp,
                    OnExit = QuitApp,
                };
            }
            catch (Exception ex)
            {
                AppDiag($"EXCEPTION in tray setup: {ex.GetType().Name}: {ex.Message}");
                AppDiag($"  StackTrace: {ex.StackTrace?.Split('\n')[0]}");
            }
        });
    }

    private void HideApp()
    {
        AppDiag("HideApp() called — scheduling deferred hide");
        DispatcherQueue.GetForCurrentThread().TryEnqueue(
            DispatcherQueuePriority.Low,
            () =>
            {
                _tray?.HideWindow();
                AppDiag("HideWindow() executed — window hidden to tray");
                ShowMinimisedToast();
            });
    }

    private void ShowApp()
    {
        if (_tray is null) return;
        AppDiag("ShowApp() called — restoring window via Win32");
        _tray.RestoreWindow();
    }

    private void QuitApp()
    {
        if (_isQuitting) return;
        _isQuitting = true;

        _tray?.AllowClose();
        _tray?.Dispose();
        _tray = null;

        DispatcherQueue.GetForCurrentThread().TryEnqueue(() =>
        {
            try { Microsoft.UI.Xaml.Application.Current.Exit(); } catch { }
        });
    }

    private static void RegisterAumid()
    {
        try
        {
            const string keyPath = @"SOFTWARE\Classes\AppUserModelId\AbsenceApp.Client";
            using var key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(keyPath, writable: true);
            key?.SetValue("DisplayName", "AbsenceApp");
            AppDiag("RegisterAumid() — HKCU AUMID key written");
        }
        catch (Exception ex)
        {
            AppDiag($"RegisterAumid failed: {ex.Message}");
        }
    }

    private static void ShowMinimisedToast()
    {
        try
        {
            const string xml = """
                <toast duration="short">
                  <visual>
                    <binding template="ToastGeneric">
                      <text>AbsenceApp</text>
                      <text>AbsenceApp has been minimised to the System Tray. Right-click the tray icon to exit or launch it again.</text>
                    </binding>
                  </visual>
                </toast>
                """;
            var doc = new Windows.Data.Xml.Dom.XmlDocument();
            doc.LoadXml(xml);
            var notifier = Windows.UI.Notifications.ToastNotificationManager
                .CreateToastNotifier(AppAumid);
            notifier.Show(new Windows.UI.Notifications.ToastNotification(doc));
            AppDiag("ShowMinimisedToast() — toast dispatched");
        }
        catch (Exception ex)
        {
            AppDiag($"Toast failed: {ex.Message}");
        }
    }

    static void AppDiag(string msg)
    {
        try
        {
            var path = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                "AbsenceApp_diag.log");
            System.IO.File.AppendAllText(path, $"[{DateTime.Now:HH:mm:ss.fff}] APP: {msg}\n");
        }
        catch { }
    }
}
