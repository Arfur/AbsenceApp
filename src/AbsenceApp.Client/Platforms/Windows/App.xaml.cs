/*
===============================================================================
 File        : App.xaml.cs
 Namespace   : AbsenceApp.Client.WinUI
 Platform    : Windows (WinUI 3)
 Author      : Michael
 Version     : 2.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-17
-------------------------------------------------------------------------------
 Purpose     : Windows (WinUI 3) application code-behind.
               Initialises the WinUI application, delegates MauiApp creation
               to MauiProgram.CreateMauiApp(), and configures system-tray
               behaviour via WindowsTrayIcon (pure Win32 P/Invoke):
                 - Tray icon is created on launch (uses EXE-embedded icon).
                 - Minimising hides the window from the taskbar (goes to tray).
                 - Double-clicking the tray icon restores the window.
                 - Right-clicking shows Open / Exit context menu.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial implementation.
   - 2.0.0  2026-03-17  Added system-tray support via WindowsTrayIcon.
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
            // Resolve the underlying WinUI 3 window through the MAUI handler.
            _nativeWindow =
                Microsoft.Maui.Controls.Application.Current?
                    .Windows.FirstOrDefault()?
                    .Handler?.PlatformView
                    as Microsoft.UI.Xaml.Window;

            if (_nativeWindow is null) return;

            // Create the system-tray icon (blue calendar icon from EXE).
            _tray = new WindowsTrayIcon(_nativeWindow, "AbsenceApp")
            {
                OnOpen = ShowApp,
                OnExit = QuitApp,
            };

            // Intercept minimize: hide the window to tray instead.
            _nativeWindow.AppWindow.Changed += OnAppWindowChanged;
        });
    }

    // =========================================================================
    // Window event — minimise → hide to tray
    // =========================================================================

    private void OnAppWindowChanged(AppWindow sender, AppWindowChangedEventArgs args)
    {
        if (!args.DidPresenterChange) return;

        if (sender.Presenter is OverlappedPresenter
            { State: OverlappedPresenterState.Minimized })
        {
            // Defer so we don't call Hide() re-entrantly inside the Changed event.
            DispatcherQueue.GetForCurrentThread().TryEnqueue(() => sender.Hide());
        }
    }

    // =========================================================================
    // ShowApp — restore the window from tray
    // =========================================================================

    private void ShowApp()
    {
        if (_nativeWindow is null) return;

        _nativeWindow.AppWindow.Show();

        if (_nativeWindow.AppWindow.Presenter is OverlappedPresenter op)
            op.Restore(true);

        _nativeWindow.Activate();
    }

    // =========================================================================
    // QuitApp — tear down cleanly and exit
    // =========================================================================

    private void QuitApp()
    {
        if (_isQuitting) return;
        _isQuitting = true;

        _tray?.Dispose();
        _tray = null;

        Microsoft.Maui.Controls.Application.Current?.Quit();
    }
}

