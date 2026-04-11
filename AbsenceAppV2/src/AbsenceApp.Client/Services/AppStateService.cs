/* ============================================================================
   File        : AppStateService.cs
   Namespace   : AbsenceApp.Client.Services
   Author      : Michael
 * Version     : 2.12.0
   Created     : 2026-03-17
   Updated     : 2026-04-08
   ----------------------------------------------------------------------------
   Purpose     : Singleton UI state container shared across all layout and
                 shell components. Tracks:
                   - Sidebar collapsed/expanded
                   - Dark/light mode (persisted across cold starts)
                   - Breadcrumb segments + category
                   - User identity + authentication state (in-memory only)
                   - Per-page table UI state (search, filters, show-entries)
                   - (2.11.0) Last main-menu route + open group for sidebar
                               restoration when returning from Global Config.
                   - (2.12.0) RestoreMainMenuRequested flag for deterministic
                               sidebar switching back from Global Config.
   ----------------------------------------------------------------------------
   Changes     :
     - 2.2.0   2026-03-27  DarkMode persisted to MAUI Preferences.
     - 2.3.0   2026-03-29  Authentication state and user identity persisted and
                           restored on app startup.
     - 2.3.1   2026-03-29  Restored TablePageUiState DTO in-file to resolve build
                           errors after auth persistence update.
     - 2.4.0   2026-03-29  Enforced mandatory login on every cold start by
                           removing authentication persistence and restoration.
     - 2.5.0   2026-04-06  Phase 3 V1 Parity Issue 5: added OnTableSettingsChanged
                           event and NotifyTableSettingsChanged() so table pages
                           can reload column settings immediately after a save.
     - 2.6.0   2026-04-05  Phase 3 Header Nav Identity: added UnreadMessages
                           property for the message badge count displayed in
                           the HeaderV2 messages icon.
     - 2.7.0   2026-04-05  Phase 3 Header Nav Identity — messaging enablement:
                           replaced hardcoded UnreadCount with DB-backed
                           UnreadMessages + UnreadNotifications.
     - 2.8.0   2026-04-06  Diagnostic reliability: replaced Debug.WriteLine with
                           Console.WriteLine for auth state transitions.
     - 2.9.0   2026-04-06  Logging migration: replaced all Console.WriteLine calls
                           with AppLog.Write for file-based diagnostic output.
     - 2.10.0  2026-04-07  Debug instrumentation across UI state setters.
     - 2.11.0  2026-04-08  Step 2: Added last-main-menu state storage +
                           SetLastMainMenuState(), GetLastMainMenuState(),
                           RestoreMainMenu() for sidebar switching logic.
     - 2.12.0  2026-04-08  Step 2 (Option A): Added RestoreMainMenuRequested flag
                           and ClearRestoreMainMenuRequest() so SidebarV2 can
                           deterministically react to RestoreMainMenu().
   ============================================================================
*/

using AbsenceApp.Client.Shared;
using AbsenceApp.Core.DTOs;
using Microsoft.Maui.Storage;

namespace AbsenceApp.Client.Services;

/* ============================================================================
   Section: Service definition
   ============================================================================ */

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

    // Step 2 (v2.11.0): Backing fields for last main-menu state
    private string _lastMainMenuRoute     = "/v2/dashboard";
    private string _lastMainMenuOpenGroup = string.Empty;

    // Step 2 (v2.12.0): Flag to signal a requested restore to main menu
    public bool RestoreMainMenuRequested { get; private set; }

    /* ============================================================================
       Section: Constructor — restore persisted state
       ============================================================================ */

    public AppStateService()
    {
        DarkMode = Preferences.Default.Get(DarkModeKey, false);
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

    public IReadOnlyList<MessageDto>         UnreadMessages      { get; private set; } = Array.Empty<MessageDto>();
    public IReadOnlyList<AppNotificationDto> UnreadNotifications { get; private set; } = Array.Empty<AppNotificationDto>();

    public int UnreadMessagesCount      => UnreadMessages.Count;
    public int UnreadNotificationsCount => UnreadNotifications.Count;

    /* ============================================================================
       Section: Breadcrumb + category
       ============================================================================ */

    public List<string> Breadcrumb { get; private set; } = new() { "Home" };
    public string BreadcrumbCategory { get; private set; } = string.Empty;

    /* ============================================================================
       Section: Change notification
       ============================================================================ */

    public event Action? OnChange;

    private void Notify() => OnChange?.Invoke();

    /* ============================================================================
       Section: Table-settings change notification
       ============================================================================ */

    public event Action? OnTableSettingsChanged;
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
       Section: Table page state
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

    public void SetMessagingData(
        List<MessageDto> messages,
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
       Section: Step 2 — Last main-menu state (v2.11.0 / v2.12.0)
       ============================================================================ */

    public void SetLastMainMenuState(string route, string openGroup)
    {
        _lastMainMenuRoute     = route;
        _lastMainMenuOpenGroup = openGroup;

        AppLog.Write("AppStateService.cs", "SetLastMainMenuState",
            $"route='{route}' openGroup='{openGroup}'");
    }

    public (string Route, string OpenGroup) GetLastMainMenuState()
        => (_lastMainMenuRoute, _lastMainMenuOpenGroup);

    /// <summary>
    /// Signals that the UI should restore the main menu. SidebarV2 will
    /// observe RestoreMainMenuRequested, perform the restore, then call
    /// ClearRestoreMainMenuRequest().
    /// </summary>
    public void RestoreMainMenu()
    {
        RestoreMainMenuRequested = true;
        AppLog.Write("AppStateService.cs", "RestoreMainMenu",
            $"Restoring main menu: route='{_lastMainMenuRoute}' openGroup='{_lastMainMenuOpenGroup}' — setting RestoreMainMenuRequested=true and calling Notify()");
        Notify();
    }

    /// <summary>
    /// Clears the RestoreMainMenuRequested flag after SidebarV2 has
    /// processed the restore request.
    /// </summary>
    public void ClearRestoreMainMenuRequest()
    {
        if (!RestoreMainMenuRequested) return;

        RestoreMainMenuRequested = false;
        AppLog.Write("AppStateService.cs", "ClearRestoreMainMenuRequest",
            "RestoreMainMenuRequested reset to false");
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
