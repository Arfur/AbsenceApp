namespace AbsenceApp.Client.Services;

/// <summary>
/// Singleton service that holds UI-level state shared across all layout components:
/// sidebar collapsed/expanded, dark/light mode, unread notification count,
/// and the current breadcrumb trail.
/// </summary>
public class AppStateService
{
    public bool   SidebarCollapsed { get; private set; } = false;
    public bool   DarkMode          { get; private set; } = false;
    public int    UnreadCount       { get; set; }         = 4;
    public string UserName          { get; set; }         = "mbattle";
    public string UserRole          { get; set; }         = "Super Admin";

    /// <summary>Breadcrumb segments shown in the main area, e.g. ["Home","Students","All Students"]</summary>
    public List<string> Breadcrumb { get; private set; } = new() { "Home" };

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

    private void Notify() => OnChange?.Invoke();

    public void QuitApp()
    {
        Microsoft.Maui.Controls.Application.Current?.Quit();
    }
}
