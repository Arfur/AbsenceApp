/*
===============================================================================
 File        : App.xaml.cs
 Namespace   : AbsenceApp.Client.WinUI
 Platform    : Windows (WinUI 3)
 Author      : Michael
 Version     : 2.1.0
 Created     : 2026-03-13
 Updated     : 2026-03-18
-------------------------------------------------------------------------------
 Purpose     : Windows (WinUI 3) application code-behind.
               Initialises the WinUI application, delegates MauiApp creation
               to MauiProgram.CreateMauiApp(), and configures system-tray
               behaviour via WindowsTrayIcon (pure Win32 P/Invoke):
                 - App launches directly into the system tray (no visible window).
                 - Tray icon is created on launch (uses EXE-embedded icon).
                 - Minimising hides the window from the taskbar (goes to tray).
                 - Double-clicking the tray icon restores the window.
                 - Right-clicking shows Open / Exit context menu.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial implementation.
   - 2.0.0  2026-03-17  Added system-tray support via WindowsTrayIcon.
   - 2.1.0  2026-03-18  Wrapped OnLaunched tray setup in try-catch to prevent
                         WinRT STATUS_STOWED_EXCEPTION crash on AppWindow.Hide().
                         Removed MoveAndResize(-32000) pre-hide workaround.
                         Deferred Hide() to a Low-priority dispatcher frame.
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
    // =========================================================================
    // Fields
    // =========================================================================

    private WindowsTrayIcon?           _tray;
    private Microsoft.UI.Xaml.Window?  _nativeWindow;
    private bool                       _isQuitting;
    private const string               AppAumid = "AbsenceApp.Client";

    // =========================================================================
    // Constructor
    // =========================================================================

    public App() => InitializeComponent();

    // =========================================================================
    // MAUI bootstrap
    // =========================================================================

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    // =========================================================================
    // OnLaunched — set up tray and window behaviour after MAUI has initialised
    // =========================================================================

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);

        // Defer one frame so the MAUI window handler is fully attached.
        DispatcherQueue.GetForCurrentThread().TryEnqueue(() =>
        {
            RegisterAumid();
            try
            {
                // Resolve the underlying WinUI 3 window through the MAUI handler.
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

                // Create the system-tray icon (blue calendar icon from EXE).
                _tray = new WindowsTrayIcon(_nativeWindow, "AbsenceApp")
                {
                    OnOpen = ShowApp,
                    OnHide = HideApp,  // X button → hide to tray (Win32 WM_SYSCOMMAND intercept)
                    OnExit = QuitApp,
                };

                // App starts visible — the window is NOT hidden on launch.
                // Hiding only occurs when the user clicks X (via SubclassProc → HideApp).
            }
            catch (Exception ex)
            {
                // Log the actual exception so we can diagnose tray setup failures.
                AppDiag($"EXCEPTION in tray setup: {ex.GetType().Name}: {ex.Message}");
                AppDiag($"  StackTrace: {ex.StackTrace?.Split('\n')[0]}");
            }
        });
    }

    // =========================================================================
    // Window event — close button (X) → hide to tray (called by WM_CLOSE intercept)
    // =========================================================================

    private void HideApp()
    {
        // SubclassProc has swallowed WM_SYSCOMMAND/WM_CLOSE so the window won't close.
        // We defer ShowWindow(SW_HIDE) to the next message-loop frame so it fires
        // AFTER SubclassProc has fully returned — this prevents re-entrant message
        // delivery (WM_SHOWWINDOW) from confusing WinUI 3's compositor and dropping
        // the tray icon registration.
        AppDiag("HideApp() called — scheduling deferred hide");
        DispatcherQueue.GetForCurrentThread().TryEnqueue(
            Microsoft.UI.Dispatching.DispatcherQueuePriority.Low,
            () =>
            {
                _tray?.HideWindow();
                AppDiag("HideWindow() executed — window hidden to tray");
                ShowMinimisedToast();
            });
    }

    // =========================================================================
    // ShowApp — restore the window from tray
    // =========================================================================

    private void ShowApp()
    {
        if (_tray is null) return;
        AppDiag("ShowApp() called — restoring window via Win32");
        _tray.RestoreWindow();
    }

    // =========================================================================
    // QuitApp — tear down cleanly and exit
    // =========================================================================

    private void QuitApp()
    {
        if (_isQuitting) return;
        _isQuitting = true;

        // AllowClose() lets the next WM_CLOSE message pass through SubclassProc
        // to DefSubclassProc, so the window actually closes when Exit() is called.
        _tray?.AllowClose();
        _tray?.Dispose();
        _tray = null;

        // Use the WinUI 3 Exit() — more reliable than MAUI Quit() for process termination.
        DispatcherQueue.GetForCurrentThread().TryEnqueue(() =>
        {
            try { Microsoft.UI.Xaml.Application.Current.Exit(); } catch { }
        });
    }

    // =========================================================================
    // RegisterAumid — idempotent HKCU registry entry so WinRT toast can find the app
    // =========================================================================

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

    // =========================================================================
    // ShowMinimisedToast — Windows Action Centre notification: "minimised to tray"
    // =========================================================================

    private static void ShowMinimisedToast()
    {
        try
        {
            const string xml = """
                <toast duration="short">
                  <visual>
                    <binding template="ToastGeneric">
                      <text>AbsenceApp</text>
                      <text>AbsenceApp has been minimised to the System Tray. Right-click the tray icon to exit the app or launch it again.</text>
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

    // =========================================================================
    // AppDiag — write a timestamped line to the desktop diagnostic log
    // =========================================================================

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

