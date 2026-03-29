/* ============================================================================
   File        : AppStateService.cs
   Namespace   : AbsenceApp.Client.Services
   Author      : Michael
   Version     : 2.4.0
   Created     : 2026-03-17
   Updated     : 2026-03-29
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
   ============================================================================
*/

using AbsenceApp.Client.Shared;
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
    public int    UnreadCount      { get; set; }         = 4;
    public string UserName         { get; private set; } = string.Empty;
    public string UserRole         { get; private set; } = string.Empty;
    public bool   IsAuthenticated  { get; private set; } = false;
    public long   CurrentUserId    { get; private set; }

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
       Section: Sidebar + theme toggles
       ============================================================================ */

    public void ToggleSidebar()
    {
        SidebarCollapsed = !SidebarCollapsed;
        Notify();
    }

    public void ToggleDarkMode()
    {
        SetDarkMode(!DarkMode);
    }

    public void SetDarkMode(bool value)
    {
        DarkMode = value;
        Preferences.Default.Set(DarkModeKey, value);
        Notify();
    }

    /* ============================================================================
       Section: Breadcrumb setters
       ============================================================================ */

    public void SetBreadcrumb(params string[] segments)
    {
        Breadcrumb = new List<string>(segments);
        Notify();
    }

    public void SetBreadcrumbCategory(string category)
    {
        BreadcrumbCategory = category;
        Notify();
    }

    /* ============================================================================
       Section: Table page state (search, filters, show-entries)
       ============================================================================ */

    public TablePageUiState? GetTablePageState(string key)
        => _tablePageStates.TryGetValue(key, out var state) ? state.Clone() : null;

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
    }

    /* ============================================================================
       Section: Authentication + user identity
       ============================================================================ */

    public void SetUser(long id, string name, string role)
    {
        CurrentUserId   = id;
        UserName        = name;
        UserRole        = role;
        IsAuthenticated = true;

        Notify();
    }

    public void AuthLogout()
    {
        IsAuthenticated = false;
        CurrentUserId   = 0;
        UserName        = string.Empty;
        UserRole        = string.Empty;

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
