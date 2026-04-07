/* ============================================================================
   File        : AppStateService.cs
   Namespace   : AbsenceApp.Client.Services
   Author      : Michael
 * Version     : 2.10.0
   Created     : 2026-03-17
   Updated     : 2026-04-07
   ----------------------------------------------------------------------------
   Purpose     : Singleton UI state container shared across all layout and
                 shell components. Tracks:
                   - Sidebar collapsed/expanded
                   - Dark/light mode (persisted across cold starts)
                   - Breadcrumb segments + category
                   - User identity + authentication state (in-memory only)
                   - Per-page table UI state (search, filters, show-entries)
   ----------------------------------------------------------------------------
   Changes     :
     - 2.2.0  2026-03-27  DarkMode persisted to MAUI Preferences.
     - 2.3.0  2026-03-29  Authentication state and user identity persisted and
                          restored on app startup.
     - 2.3.1  2026-03-29  Restored TablePageUiState DTO in-file to resolve build
                          errors after auth persistence update.
     - 2.4.0  2026-03-29  Enforced mandatory login on every cold start by
                          removing authentication persistence and restoration.
     - 2.5.0  2026-04-06  Phase 3 V1 Parity Issue 5: added OnTableSettingsChanged
                          event and NotifyTableSettingsChanged() so table pages
                          can reload column settings immediately after a save.
     - 2.6.0  2026-04-05  Phase 3 Header Nav Identity: added UnreadMessages
                          property for the message badge count displayed in
                          the HeaderV2 messages icon.
     - 2.7.0  2026-04-05  Phase 3 Header Nav Identity — messaging enablement:
                          replaced hardcoded UnreadCount (int=4) and
                          UnreadMessages (int=2) with database-backed
                          UnreadMessages (IReadOnlyList<MessageDto>) and
                          UnreadNotifications (IReadOnlyList<AppNotificationDto>).
                          Added UnreadMessagesCount and UnreadNotificationsCount
                          computed properties. Added SetMessagingData() method
                          called from Login.razor after successful authentication.
                          Cleared messaging state on AuthLogout().
     - 2.8.0  2026-04-06  Diagnostic reliability: replaced Debug.WriteLine with
                          Console.WriteLine for auth state transitions.
     - 2.9.0  2026-04-06  Logging migration: replaced all Console.WriteLine calls
                          with AppLog.Write for file-based diagnostic output.
     - 2.10.0 2026-04-07  Debug instrumentation: added AppLog.Write calls to
                          ToggleSidebar, ToggleDarkMode, SetDarkMode,
                          SetBreadcrumb, SetBreadcrumbCategory, SetMessagingData,
                          GetTablePageState, SetTablePageState, and enhanced
                          AuthLogout with subscriber-count awareness.
   ============================================================================
*/

using AbsenceApp.Client.Shared;
using AbsenceApp.Core.DTOs;
using Microsoft.Maui.Storage;

namespace AbsenceApp.Client.Services;

/* ============================================================================
   Section: Service definition
   ============================================================================ */

/// <summary>
/// Singleton service that holds UI-level state shared across all layout
/// components: sidebar collapsed/expanded, dark/light mode, unread notification
/// count, user identity, authentication state, breadcrumb trail, and per-page
/// table UI state.
/// </summary>
public class AppStateService
{
    /* ============================================================================
       Section: Persistence keys
       ============================================================================ */

    private const string DarkModeKey = "app.dark_mode";

    /* ============================================================================
       Section: Backing fields
       ============================================================================ */

    private readonly Dictionary<string, TablePageUiState> _tablePageStates =
        new(StringComparer.OrdinalIgnoreCase);

    /* ============================================================================
       Section: Constructor — restore persisted state
       ============================================================================ */

    public AppStateService()
    {
        // Restore the last selected theme so cold starts honour the user's
        // preference without defaulting back to Light mode every time.
        DarkMode = Preferences.Default.Get(DarkModeKey, false);

        // Authentication is intentionally NOT restored.
        // Users must log in on every cold start for security.
    }

    /* ============================================================================
       Section: Global UI state
       ============================================================================ */

    public bool   SidebarCollapsed { get; private set; } = false;
    public bool   DarkMode         { get; private set; }
    public string UserName         { get; private set; } = string.Empty;
    public string UserRole         { get; private set; } = string.Empty;
    public bool   IsAuthenticated  { get; private set; } = false;
    public long   CurrentUserId    { get; private set; }

    // ── Messaging state (populated from DB at login; cleared on logout) ────
    public IReadOnlyList<MessageDto>         UnreadMessages      { get; private set; } = Array.Empty<MessageDto>();
    public IReadOnlyList<AppNotificationDto> UnreadNotifications { get; private set; } = Array.Empty<AppNotificationDto>();

    /// <summary>Count of unread messages — drives the header badge.</summary>
    public int UnreadMessagesCount      => UnreadMessages.Count;

    /// <summary>Count of unread notifications — drives the header badge.</summary>
    public int UnreadNotificationsCount => UnreadNotifications.Count;

    /* ============================================================================
       Section: Breadcrumb + category
       ============================================================================ */

    /// <summary>Breadcrumb segments shown in the main area.</summary>
    public List<string> Breadcrumb { get; private set; } = new() { "Home" };

    /// <summary>Category label for the current page (from V2 menu.json).</summary>
    public string BreadcrumbCategory { get; private set; } = string.Empty;

    /* ============================================================================
       Section: Change notification
       ============================================================================ */

    public event Action? OnChange;

    private void Notify() => OnChange?.Invoke();

    /* ============================================================================
       Section: Table-settings change notification (Issue 5 — V1 parity)
       ============================================================================ */

    /// <summary>
    /// Raised whenever a table's column settings are saved via SettingsListPageV2.
    /// Table pages subscribe to reload settings and call StateHasChanged().
    /// </summary>
    public event Action? OnTableSettingsChanged;

    /// <summary>Fires <see cref="OnTableSettingsChanged"/>.</summary>
    public void NotifyTableSettingsChanged() => OnTableSettingsChanged?.Invoke();

    /* ============================================================================
       Section: Sidebar + theme toggles
       ============================================================================ */

    public void ToggleSidebar()
    {
        SidebarCollapsed = !SidebarCollapsed;
        AppLog.Write("AppStateService.cs", "ToggleSidebar",
            $"SidebarCollapsed={SidebarCollapsed} — calling Notify()");
        Notify();
    }

    public void ToggleDarkMode()
    {
        AppLog.Write("AppStateService.cs", "ToggleDarkMode",
            $"Toggling DarkMode from {DarkMode} to {!DarkMode}");
        SetDarkMode(!DarkMode);
    }

    public void SetDarkMode(bool value)
    {
        DarkMode = value;
        AppLog.Write("AppStateService.cs", "SetDarkMode",
            $"DarkMode={DarkMode} — persisting to Preferences — calling Notify()");
        Preferences.Default.Set(DarkModeKey, value);
        Notify();
    }

    /* ============================================================================
       Section: Breadcrumb setters
       ============================================================================ */

    public void SetBreadcrumb(params string[] segments)
    {
        Breadcrumb = new List<string>(segments);
        AppLog.Write("AppStateService.cs", "SetBreadcrumb",
            $"Breadcrumb=[{string.Join(" > ", segments)}] — calling Notify()");
        Notify();
    }

    public void SetBreadcrumbCategory(string category)
    {
        BreadcrumbCategory = category;
        AppLog.Write("AppStateService.cs", "SetBreadcrumbCategory",
            $"BreadcrumbCategory='{category}' — calling Notify()");
        Notify();
    }

    /* ============================================================================
       Section: Table page state (search, filters, show-entries)
       ============================================================================ */

    public TablePageUiState? GetTablePageState(string key)
    {
        var found = _tablePageStates.TryGetValue(key, out var state);
        AppLog.Write("AppStateService.cs", "GetTablePageState",
            $"key='{key}' found={found}");
        return found ? state?.Clone() : null;
    }

    public void SetTablePageState(
        string key,
        string searchValue,
        int showEntries,
        IEnumerable<FilterChip> activeFilters)
    {
        _tablePageStates[key] = new TablePageUiState
        {
            SearchValue   = searchValue,
            ShowEntries   = showEntries,
            ActiveFilters = activeFilters
                .Select(c => new FilterChip(c.Name, c.Value))
                .ToList(),
        };
        AppLog.Write("AppStateService.cs", "SetTablePageState",
            $"key='{key}' searchValue='{searchValue}' showEntries={showEntries} activeFilters={_tablePageStates[key].ActiveFilters.Count}");
    }

    /* ============================================================================
       Section: Authentication + user identity
       ============================================================================ */

    public void SetUser(long id, string name, string role)
    {
        CurrentUserId   = id;
        AppLog.Write("AppStateService.cs", "SetUser",
            $"CurrentUserId={id} UserName='{name}' Role='{role}' — calling Notify()");
        UserName        = name;
        UserRole        = role;
        IsAuthenticated = true;

        Notify();
    }

    /// <summary>
    /// Stores the authenticated user's unread messages and notifications.
    /// Called by Login.razor immediately after <see cref="SetUser"/>.
    /// </summary>
    public void SetMessagingData(
        List<MessageDto>         messages,
        List<AppNotificationDto> notifications)
    {
        UnreadMessages      = messages;
        UnreadNotifications = notifications;
        AppLog.Write("AppStateService.cs", "SetMessagingData",
            $"UnreadMessages={messages.Count} UnreadNotifications={notifications.Count} — calling Notify()");
        Notify();
    }

    public void AuthLogout()
    {
        IsAuthenticated     = false;
        CurrentUserId       = 0;
        AppLog.Write("AppStateService.cs", "AuthLogout",
            "CurrentUserId reset to 0 — user logged out. Calling Notify()");
        UserName            = string.Empty;
        UserRole            = string.Empty;
        UnreadMessages      = Array.Empty<MessageDto>();
        UnreadNotifications = Array.Empty<AppNotificationDto>();

        Notify();
    }

    /* ============================================================================
       Section: Application control
       ============================================================================ */

    public void QuitApp()
    {
        Microsoft.Maui.Controls.Application.Current?.Quit();
    }
}

/* ============================================================================
   Section: TablePageUiState DTO
   ============================================================================ */

public sealed class TablePageUiState
{
    public string SearchValue { get; init; } = string.Empty;
    public int ShowEntries { get; init; } = 10;
    public List<FilterChip> ActiveFilters { get; init; } = [];

    public TablePageUiState Clone() => new()
    {
        SearchValue   = SearchValue,
        ShowEntries   = ShowEntries,
        ActiveFilters = ActiveFilters
            .Select(c => new FilterChip(c.Name, c.Value))
            .ToList(),
    };
}
