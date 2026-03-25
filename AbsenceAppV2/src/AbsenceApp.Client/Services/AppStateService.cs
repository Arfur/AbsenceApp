/* ============================================================================
   File        : AppStateService.cs
   Namespace   : AbsenceApp.Client.Services
   Author      : Michael
   Version     : 2.1.0
   Created     : 2026-03-17
   Updated     : 2026-03-25
   ----------------------------------------------------------------------------
   Purpose     : Singleton UI state container shared across all layout and
                 shell components. Tracks:
                   - Sidebar collapsed/expanded
                   - Dark/light mode
                   - Breadcrumb segments + category
                   - User identity + authentication state
                   - Per-page table UI state (search, filters, show-entries)
   ----------------------------------------------------------------------------
   Changes     :
     - 1.0.0  2026-03-17  Initial implementation.
     - 2.0.0  2026-03-21  Extended for V2 shell: BreadcrumbCategory, V2 menu
                          integration, and expanded table-state persistence.
     - 2.1.0  2026-03-25  Confirmed as shared V2 service after V1/V2 split.
                          Added full header + section comments; no functional
                          changes.
   ============================================================================
*/

using AbsenceApp.Client.Shared;

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
       Section: Backing fields
       ============================================================================ */

    private readonly Dictionary<string, TablePageUiState> _tablePageStates =
        new(StringComparer.OrdinalIgnoreCase);

    /* ============================================================================
       Section: Global UI state
       ============================================================================ */

    public bool   SidebarCollapsed { get; private set; } = false;
    public bool   DarkMode         { get; private set; } = false;
    public int    UnreadCount      { get; set; }         = 4;
    public string UserName         { get; set; }         = string.Empty;
    public string UserRole         { get; set; }         = string.Empty;
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
        DarkMode = !DarkMode;
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
