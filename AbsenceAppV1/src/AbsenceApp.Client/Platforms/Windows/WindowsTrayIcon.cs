/*
===============================================================================
 File        : WindowsTrayIcon.cs
 Namespace   : AbsenceApp.Client.WinUI
 Platform    : Windows
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-17
 Updated     : 2026-03-17
-------------------------------------------------------------------------------
 Purpose     : Manages a Windows system-tray (notification area) icon for the
               AbsenceApp MAUI WinUI 3 application.
-------------------------------------------------------------------------------
 Implementation notes:
   Uses Shell_NotifyIcon (Win32 P/Invoke) directly — no WinForms or WPF
   dependency required, which avoids the MC6000/NETSDK1073 build errors that
   arise when UseWindowsForms=true is added to a MAUI project.

   Message routing:
     Shell_NotifyIcon posts tray-icon events (left-click, right-click …) to
     a caller-supplied HWND using a caller-supplied message ID (WM_TRAYICON).
     We subclass the main WinUI 3 window's HWND with SetWindowSubclass
     (ComCtl32 v6) so WM_TRAYICON messages reach our SubclassProc.
     ComCtl32 v6 is activated automatically by the Windows App SDK that
     WinUI 3 / MAUI loads on Windows 10+.

   Context menu:
     Win32 CreatePopupMenu / TrackPopupMenu — native, DPI-aware, no extra deps.

   Icon:
     ExtractIconW extracts the first icon from the running EXE.  MAUI's build
     pipeline embeds the MauiIcon SVG (blue calendar) as the EXE icon, so the
     tray icon and the app icon are identical.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-17  Initial implementation.
===============================================================================
*/

using Microsoft.UI.Xaml;
using System.Runtime.InteropServices;
using WinRT.Interop;

namespace AbsenceApp.Client.WinUI;

/// <summary>
/// Creates and manages a Windows notification-area (system tray) icon.
/// Dispose to remove the icon and clean up the window subclass.
/// </summary>
internal sealed class WindowsTrayIcon : IDisposable
{
    // =========================================================================
    // Win32 constants
    // =========================================================================

    const uint NIM_ADD    = 0x00;
    const uint NIM_DELETE = 0x02;

    const uint NIF_MESSAGE = 0x01;
    const uint NIF_ICON    = 0x02;
    const uint NIF_TIP     = 0x04;

    // Custom message sent by Shell_NotifyIcon to our HWND
    const uint WM_TRAYICON = 0x8001; // WM_APP + 1

    // Win32 window messages
    const uint WM_CLOSE      = 0x0010;  // Programmatic close / Alt+F4 safety net
    const uint WM_SYSCOMMAND = 0x0112;  // System command — X button sends this first
    const uint SC_CLOSE      = 0xF060;  // wParam for window-close (X button / Alt+F4)

    // ShowWindow nCmdShow values
    const int SW_HIDE    = 0;
    const int SW_RESTORE = 9;

    // Mouse event codes carried in LPARAM of WM_TRAYICON
    const uint WM_LBUTTONDBLCLK = 0x0203;
    const uint WM_RBUTTONUP     = 0x0205;

    // TrackPopupMenu flags
    const uint TPM_RETURNCMD    = 0x0100;

    // Menu item IDs
    const nuint IDM_OPEN = 1000;
    const nuint IDM_EXIT = 1001;

    // Unique ID used with SetWindowSubclass to avoid conflicts
    const nuint SUBCLASS_ID = 0xAB5E;

    // =========================================================================
    // Win32 structures
    // =========================================================================

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    struct NOTIFYICONDATA
    {
        public int    cbSize;
        public nint   hWnd;
        public uint   uID;
        public uint   uFlags;
        public uint   uCallbackMessage;
        public nint   hIcon;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string szTip;
        public uint   dwState;
        public uint   dwStateMask;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string szInfo;
        public uint   uVersion;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string szInfoTitle;
        public uint   dwInfoFlags;
        public Guid   guidItem;
        public nint   hBalloonIcon;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct POINT { public int X; public int Y; }

    // =========================================================================
    // P/Invoke declarations
    // =========================================================================

    [DllImport("Shell32.dll", CharSet = CharSet.Unicode)]
    static extern bool Shell_NotifyIconW(uint msg, ref NOTIFYICONDATA pnid);

    [DllImport("Shell32.dll", CharSet = CharSet.Unicode)]  // ExtractIconW lives in Shell32, not User32
    static extern nint ExtractIconW(nint hInst, string pszExeFileName, uint nIconIndex);

    [DllImport("User32.dll")]
    static extern bool DestroyIcon(nint hIcon);

    [DllImport("User32.dll")]
    static extern nint CreatePopupMenu();

    [DllImport("User32.dll", CharSet = CharSet.Unicode)]
    static extern bool AppendMenuW(nint hMenu, uint uFlags, nuint uIDNewItem, string? lpNewItem);

    [DllImport("User32.dll")]
    static extern bool DestroyMenu(nint hMenu);

    [DllImport("User32.dll")]
    static extern uint TrackPopupMenu(nint hMenu, uint uFlags,
        int x, int y, int nReserved, nint hWnd, nint prcRect);

    [DllImport("User32.dll")]
    static extern bool GetCursorPos(out POINT lpPoint);

    [DllImport("User32.dll")]
    static extern bool SetForegroundWindow(nint hWnd);

    [DllImport("User32.dll")]
    static extern bool ShowWindow(nint hWnd, int nCmdShow);

    // SetWindowSubclass / DefSubclassProc — ComCtl32 v6 (activated by WinUI 3)
    delegate nint SUBCLASSPROC(nint hWnd, uint uMsg,
        nint wParam, nint lParam, nuint uIdSubclass, nuint dwRefData);

    [DllImport("ComCtl32.dll")]
    static extern bool SetWindowSubclass(nint hWnd, SUBCLASSPROC pfnSubclass,
        nuint uIdSubclass, nuint dwRefData);

    [DllImport("ComCtl32.dll")]
    static extern bool RemoveWindowSubclass(nint hWnd, SUBCLASSPROC pfnSubclass,
        nuint uIdSubclass);

    [DllImport("ComCtl32.dll")]
    static extern nint DefSubclassProc(nint hWnd, uint uMsg,
        nint wParam, nint lParam);

    // =========================================================================
    // Fields
    // =========================================================================

    readonly nint         _hWnd;
    readonly nint         _hIcon;
    readonly SUBCLASSPROC _subclassDelegate; // field keeps the delegate alive (prevents GC)
    bool                  _disposed;
    bool                  _allowClose;       // when true, WM_CLOSE passes through to DefSubclassProc

    /// <summary>Called when the user clicks "Open" or double-clicks the tray icon.</summary>
    public Action? OnOpen { get; set; }

    /// <summary>Called when the user clicks X on the window (instead of actually closing).</summary>
    public Action? OnHide { get; set; }

    /// <summary>Called when the user clicks "Exit" in the context menu.</summary>
    public Action? OnExit { get; set; }

    /// <summary>
    /// Call this before calling Application.Exit() so the next WM_CLOSE
    /// message is forwarded to DefSubclassProc (allowing the window to close).
    /// </summary>
    public void AllowClose() => _allowClose = true;

    /// <summary>
    /// Hides the window using raw Win32 ShowWindow(SW_HIDE).
    /// Use this instead of AppWindow.Hide() which does not clear WS_VISIBLE on Win10.
    /// </summary>
    public void HideWindow() => ShowWindow(_hWnd, SW_HIDE);

    /// <summary>
    /// Restores and activates the window using raw Win32.
    /// Symmetric counterpart to HideWindow().
    /// </summary>
    public void RestoreWindow()
    {
        ShowWindow(_hWnd, SW_RESTORE);
        SetForegroundWindow(_hWnd);
    }

    // =========================================================================
    // Constructor
    // =========================================================================

    public WindowsTrayIcon(Microsoft.UI.Xaml.Window nativeWindow, string tooltip)
    {
        _hWnd = WindowNative.GetWindowHandle(nativeWindow);

        Diag($"Created. HWND=0x{_hWnd:X} Window.HashCode={nativeWindow.GetHashCode()}");

        // Extract the app icon from the running EXE (MAUI embeds the MauiIcon there).
        var exe = System.Diagnostics.Process.GetCurrentProcess()
                      .MainModule?.FileName ?? string.Empty;
        Diag($"EXE path: {exe}");
        _hIcon = ExtractIconW(nint.Zero, exe, 0);
        Diag($"ExtractIconW hIcon=0x{_hIcon:X}");

        // Register with Shell_NotifyIcon.
        var nid = new NOTIFYICONDATA
        {
            cbSize          = Marshal.SizeOf<NOTIFYICONDATA>(),
            hWnd            = _hWnd,
            uID             = 1,
            uFlags          = NIF_MESSAGE | NIF_ICON | NIF_TIP,
            uCallbackMessage = WM_TRAYICON,
            hIcon           = _hIcon,
            szTip           = tooltip ?? string.Empty,
            szInfo          = string.Empty,
            szInfoTitle     = string.Empty,
        };
        Diag($"NOTIFYICONDATA built. cbSize={nid.cbSize}");
        bool notifyOk = Shell_NotifyIconW(NIM_ADD, ref nid);
        Diag($"Shell_NotifyIconW returned {notifyOk}");

        // Subclass the WinUI window so WM_TRAYICON messages reach SubclassProc.
        // Storing the delegate in a field prevents the GC from collecting it.
        _subclassDelegate = SubclassProc;
        Diag("About to call SetWindowSubclass...");
        bool ok = SetWindowSubclass(_hWnd, _subclassDelegate, SUBCLASS_ID, 0);
        Diag($"SetWindowSubclass returned {ok}");
    }

    // =========================================================================
    // WndProc subclass
    // =========================================================================

    nint SubclassProc(nint hWnd, uint uMsg, nint wParam, nint lParam,
                      nuint uIdSubclass, nuint dwRefData)
    {
        // ── WM_SYSCOMMAND + SC_CLOSE ───────────────────────────────────────────
        // WinUI 3's X button fires WM_SYSCOMMAND(SC_CLOSE) BEFORE WM_CLOSE and
        // BEFORE AppWindow.Closing. Swallowing it here stops the window closing.
        if (uMsg == WM_SYSCOMMAND && ((uint)wParam.ToInt64() & 0xFFF0) == SC_CLOSE)
        {
            Diag($"WM_SYSCOMMAND SC_CLOSE. _allowClose={_allowClose}");
            if (!_allowClose)
            {
                // Do NOT call ShowWindow here — doing so re-entrantly (while still inside
                // SubclassProc) causes WM_SHOWWINDOW to be delivered synchronously back
                // into the subclass chain, which WinUI 3 misinterprets as shutdown and
                // drops the tray icon. Instead, swallow the message and let OnHide defer
                // the ShowWindow call to the next message-loop frame.
                Diag("SC_CLOSE swallowed — deferring hide via OnHide");
                OnHide?.Invoke();
                return nint.Zero; // swallow — window stays open
            }
            // _allowClose=true → fall through → DefSubclassProc → window closes
        }

        // ── WM_CLOSE ──────────────────────────────────────────────────────────
        // Safety net: Alt+F4, programmatic CloseWindow(), Task Manager.
        if (uMsg == WM_CLOSE)
        {
            Diag($"WM_CLOSE. _allowClose={_allowClose}");
            if (!_allowClose)
            {
                // Same deferred-hide rationale as the WM_SYSCOMMAND branch above.
                Diag("WM_CLOSE swallowed — deferring hide via OnHide");
                OnHide?.Invoke();
                return nint.Zero; // swallow
            }
        }

        // ── WM_TRAYICON ───────────────────────────────────────────────────────
        if (uMsg == WM_TRAYICON)
        {
            var evt = (uint)(lParam.ToInt64() & 0xFFFF);

            if (evt == WM_LBUTTONDBLCLK)
                OnOpen?.Invoke();
            else if (evt == WM_RBUTTONUP)
                ShowContextMenu();

            return nint.Zero;
        }

        return DefSubclassProc(hWnd, uMsg, wParam, lParam);
    }

    // =========================================================================
    // Diag — append a timestamped line to the desktop diagnostic log
    // =========================================================================

    static void Diag(string msg)
    {
        try
        {
            var path = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                "AbsenceApp_diag.log");
            System.IO.File.AppendAllText(path, $"[{DateTime.Now:HH:mm:ss.fff}] TRAY: {msg}\n");
        }
        catch { }
    }

    // =========================================================================
    // ShowContextMenu — Win32 popup at cursor position
    // =========================================================================

    void ShowContextMenu()
    {
        var menu = CreatePopupMenu();
        if (menu == nint.Zero) return;

        try
        {
            AppendMenuW(menu, 0x0000 /* MF_STRING    */, IDM_OPEN, "Open AbsenceApp");
            AppendMenuW(menu, 0x0800 /* MF_SEPARATOR */, 0,        null);
            AppendMenuW(menu, 0x0000 /* MF_STRING    */, IDM_EXIT, "Exit");

            GetCursorPos(out var pt);
            SetForegroundWindow(_hWnd); // required so the menu dismisses on click-away

            var cmd = TrackPopupMenu(menu, TPM_RETURNCMD,
                                     pt.X, pt.Y, 0, _hWnd, nint.Zero);

            if      (cmd == IDM_OPEN) OnOpen?.Invoke();
            else if (cmd == IDM_EXIT) OnExit?.Invoke();
        }
        finally
        {
            DestroyMenu(menu);
        }
    }

    // =========================================================================
    // Dispose — remove tray icon and window subclass
    // =========================================================================

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        RemoveWindowSubclass(_hWnd, _subclassDelegate, SUBCLASS_ID);

        var nid = new NOTIFYICONDATA
        {
            cbSize = Marshal.SizeOf<NOTIFYICONDATA>(),
            hWnd   = _hWnd,
            uID    = 1,
        };
        Shell_NotifyIconW(NIM_DELETE, ref nid);

        if (_hIcon != nint.Zero)
            DestroyIcon(_hIcon);
    }
}
