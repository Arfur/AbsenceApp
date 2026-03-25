/*
===============================================================================
 File        : AppStateStoreV2.cs
 Namespace   : AbsenceApp.Client.State.V2
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-21
 Updated     : 2026-03-21
-------------------------------------------------------------------------------
 Purpose     : Central Flux-style state store for the V2 UI. Owns immutable UserStateV2 and mutable UiStateV2 snapshots, exposes typed mutation methods, and raises OnChange events so subscribed components can call StateHasChanged.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-21  Initial implementation.
-------------------------------------------------------------------------------
 Notes       :
   - Phase 6 service. Register as Singleton in DI.
===============================================================================
*/

using AbsenceApp.Client.Services;

namespace AbsenceApp.Client.State.V2;

/// <summary>
/// Singleton state store for V2 components. All state mutations go through
/// the methods on this class so that OnChange fires consistently.
/// </summary>
public sealed class AppStateStoreV2
{
    // -------------------------------------------------------------------------
    // State
    // -------------------------------------------------------------------------
    /// <summary>Current authenticated user state.</summary>
    public UserStateV2 User { get; private set; } = UserStateV2.Anonymous();

    /// <summary>Transient UI state (loading, errors, breadcrumb, dark mode, sidebar).</summary>
    public UiStateV2 Ui { get; } = new();

    // -------------------------------------------------------------------------

    // Change notification

    // -------------------------------------------------------------------------
    /// <summary>
    /// Raised whenever any state mutates. V2 components subscribe with
    /// <c>Store.OnChange += () => InvokeAsync(StateHasChanged);</c>
    /// and must unsubscribe in Dispose.
    /// </summary>
    public event Action? OnChange;

    // -------------------------------------------------------------------------

    // Constructor

    // -------------------------------------------------------------------------
    public AppStateStoreV2(AppStateService v1AppState)
    {
        // Seed dark-mode and sidebar from V1 state so V2 components start
        // in sync with the running application on first load.
        Ui.DarkMode         = v1AppState.DarkMode;
        Ui.SidebarCollapsed = v1AppState.SidebarCollapsed;

        // Mirror V1 auth into V2 user state if already authenticated
        if (v1AppState.IsAuthenticated)
        {
            User = UserStateV2.Authenticated(
                v1AppState.CurrentUserId,
                v1AppState.UserName,
                v1AppState.UserRole);
        }

        // Forward V1 state changes to V2 subscribers
        v1AppState.OnChange += () =>
        {
            Ui.DarkMode         = v1AppState.DarkMode;
            Ui.SidebarCollapsed = v1AppState.SidebarCollapsed;
            Notify();
        };
    }

    // -------------------------------------------------------------------------

    // User mutations

    // -------------------------------------------------------------------------
    /// <summary>Sets the authenticated user after a successful login.</summary>
    public void SetUser(long id, string name, string role, string email = "")
    {
        User = UserStateV2.Authenticated(id, name, role, email);
        Notify();
    }

    /// <summary>Clears user state on logout.</summary>
    public void ClearUser()
    {
        User = UserStateV2.Anonymous();
        Notify();
    }

    // -------------------------------------------------------------------------

    // UI mutations

    // -------------------------------------------------------------------------
    /// <summary>Sets the global page-loading flag.</summary>
    public void SetLoading(bool loading)
    {
        Ui.IsLoading = loading;
        Notify();
    }

    /// <summary>Sets the global saving flag.</summary>
    public void SetSaving(bool saving)
    {
        Ui.IsSaving = saving;
        Notify();
    }

    /// <summary>Sets or clears the page-level error message.</summary>
    public void SetPageError(string? message)
    {
        Ui.PageError = message;
        Notify();
    }

    /// <summary>Clears the page-level error message.</summary>
    public void ClearPageError() => SetPageError(null);

    /// <summary>Updates the breadcrumb and page title for the current route.</summary>
    public void SetBreadcrumb(string pageTitle, params string[] segments)
    {
        Ui.CurrentPageTitle = pageTitle;
        Ui.Breadcrumb       = segments;
        Notify();
    }

    /// <summary>Toggles dark mode and notifies subscribers.</summary>
    public void ToggleDarkMode()
    {
        Ui.DarkMode = !Ui.DarkMode;
        Notify();
    }

    /// <summary>Toggles sidebar collapsed state.</summary>
    public void ToggleSidebar()
    {
        Ui.SidebarCollapsed = !Ui.SidebarCollapsed;
        Notify();
    }

    // -------------------------------------------------------------------------

    // Internal

    // -------------------------------------------------------------------------
    private void Notify() => OnChange?.Invoke();
}
