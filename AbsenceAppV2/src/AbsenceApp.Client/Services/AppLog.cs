/*
===============================================================================
 File        : AppLog.cs
 Namespace   : AbsenceApp.Client.Services
 Author      : Michael
 Version     : 2.0.0
 Created     : 2026-04-06
 Updated     : 2026-04-07
-------------------------------------------------------------------------------
 Purpose     : Central, file-based, thread-safe diagnostic logger for the
               AbsenceApp MAUI Blazor Hybrid application.

               Writes structured log entries to a single append-only file:
                 C:\DevAbsence2\logs\AbsenceApp.log

               The directory is created automatically if it does not exist.

               Every entry includes a millisecond timestamp, source file
               name, method name, and message text:
                 [2026-04-06 22:31:12.347] Login.razor::DoLogin — msg

               The logger is completely synchronous (no async/await), is
               guarded by a lock for thread safety, and wraps all I/O in
               try/catch so logging can NEVER throw or crash the application.

               Usage:
                 AppLog.Write("Login.razor", "DoLogin", "IsAuthenticated=false");

               Startup validation:
                 AppLog.TestWrite()   — called once from Routes.razor OnInitialized
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-06  Initial implementation.
   - 2.0.0  2026-04-07  Changed log directory from %LocalApplicationData% to
                         C:\DevAbsence2\logs (fixed, project-local path).
                         Directory is now auto-created on first write.
-------------------------------------------------------------------------------
 Notes       :
   - Registered as nothing — static class, no DI required.
   - Uses a single static lock(). Safe across Blazor re-renders.
   - File path is fixed: C:\DevAbsence2\logs\AbsenceApp.log.
===============================================================================
*/

using System.IO;

namespace AbsenceApp.Client.Services;

// ===========================================================================
// AppLog
// ===========================================================================

/// <summary>
/// Thread-safe, synchronous, file-backed diagnostic logger.
/// All public members are intentionally static — no DI registration needed.
/// </summary>
public static class AppLog
{
    // ── Log directory and file path ──────────────────────────────────────────

    private static readonly string LogDirectory = @"C:\DevAbsence2\logs";

    private static readonly string LogPath = Path.Combine(LogDirectory, "AbsenceApp.log");

    // ── Ensure directory exists (once, at class initialisation) ─────────────

    static AppLog()
    {
        try { Directory.CreateDirectory(LogDirectory); } catch { }
    }

    // ── Thread-safety lock ───────────────────────────────────────────────────

    private static readonly object _fileLock = new();

    // =========================================================================
    // Write
    // Primary log method. Called from any file/method combination.
    // =========================================================================

    /// <summary>
    /// Appends a single formatted log entry to AbsenceApp.log.
    /// Never throws — all I/O errors are silently swallowed.
    /// </summary>
    /// <param name="source">
    ///   File or component name (e.g. "Login.razor", "AppStateService.cs").
    /// </param>
    /// <param name="method">
    ///   Method or lifecycle name (e.g. "DoLogin", "OnAfterRenderAsync").
    /// </param>
    /// <param name="message">
    ///   Human-readable message, including key variable values where relevant.
    /// </param>
    public static void Write(string source, string method, string message)
    {
        var entry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] {source}::{method} — {message}{Environment.NewLine}";
        lock (_fileLock)
        {
            try
            {
                File.AppendAllText(LogPath, entry);
            }
            catch
            {
                // Intentionally silent — logging must never crash the app.
            }
        }
    }

    // =========================================================================
    // TestWrite
    // Startup validation. Writes a test entry, confirms the file exists,
    // then writes a confirmation entry. Called once from Routes.razor.
    // =========================================================================

    /// <summary>
    /// Writes a test entry to the log file and confirms it was created.
    /// Never throws. Failures are written to the log itself (best-effort).
    /// </summary>
    public static void TestWrite()
    {
        Write("AppLog.cs", "TestWrite", $"Log file initialised. Path={LogPath}");

        lock (_fileLock)
        {
            try
            {
                bool exists = File.Exists(LogPath);
                var status  = exists ? "FILE EXISTS — write verified OK" : "WARNING — file not found after write";
                var entry   = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] AppLog.cs::TestWrite — {status}{Environment.NewLine}";
                File.AppendAllText(LogPath, entry);
            }
            catch (Exception ex)
            {
                // Best-effort: if this also fails, there is nothing we can do.
                try
                {
                    var failure = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] AppLog.cs::TestWrite — EXCEPTION during verification: {ex.GetType().Name}: {ex.Message}{Environment.NewLine}";
                    File.AppendAllText(LogPath, failure);
                }
                catch { }
            }
        }
    }
}
