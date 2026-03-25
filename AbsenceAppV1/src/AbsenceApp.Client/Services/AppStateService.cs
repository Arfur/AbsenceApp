using AbsenceApp.Client.Shared;

namespace AbsenceApp.Client.Services;

/// <summary>
/// Singleton service that holds UI-level state shared across all layout components:
/// sidebar collapsed/expanded, dark/light mode, unread notification count,
/// and the current breadcrumb trail.
/// </summary>
public class AppStateService
{
    private readonly Dictionary<string, TablePageUiState> _tablePageStates = new(StringComparer.OrdinalIgnoreCase);

    public bool   SidebarCollapsed { get; private set; } = false;
    public bool   DarkMode          { get; private set; } = false;
    public int    UnreadCount       { get; set; }         = 4;
    public string UserName          { get; set; }         = string.Empty;
    public string UserRole          { get; set; }         = string.Empty;
    public bool   IsAuthenticated   { get; private set; } = false;
    public long   CurrentUserId     { get; private set; }

    /// <summary>Breadcrumb segments shown in the main area, e.g. ["Home","Students","All Students"]</summary>
    public List<string> Breadcrumb { get; private set; } = new() { "Home" };

    /// <summary>
    /// Section category label for the current page, derived from menu.json by SidebarV2.
    /// Examples: "PEOPLE", "ACADEMICS", "CONFIGURATION", "Dashboard".
    /// </summary>
    public string BreadcrumbCategory { get; private set; } = string.Empty;

    public event Action? OnChange;

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

    public TablePageUiState? GetTablePageState(string key)
        => _tablePageStates.TryGetValue(key, out var state) ? state.Clone() : null;

    public void SetTablePageState(string key, string searchValue, int showEntries, IEnumerable<FilterChip> activeFilters)
    {
        _tablePageStates[key] = new TablePageUiState
        {
            SearchValue   = searchValue,
            ShowEntries   = showEntries,
            ActiveFilters = activeFilters.Select(c => new FilterChip(c.Name, c.Value)).ToList(),
        };
    }

    private void Notify() => OnChange?.Invoke();

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

    public void QuitApp()
    {
        Microsoft.Maui.Controls.Application.Current?.Quit();
    }
}

public sealed class TablePageUiState
{
    public string SearchValue { get; init; } = string.Empty;
    public int ShowEntries { get; init; } = 10;
    public List<FilterChip> ActiveFilters { get; init; } = [];

    public TablePageUiState Clone() => new()
    {
        SearchValue   = SearchValue,
        ShowEntries   = ShowEntries,
        ActiveFilters = ActiveFilters.Select(c => new FilterChip(c.Name, c.Value)).ToList(),
    };
}
