/*
===============================================================================
 File        : UiStateV2.cs
 Namespace   : AbsenceApp.Client.State.V2
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-21
 Updated     : 2026-03-21
-------------------------------------------------------------------------------
 Purpose     : Mutable record holding transient UI state: loading flags, error messages, navigation context (current route, breadcrumb, active module), dark-mode preference, and sidebar collapse state. Owned by AppStateStoreV2.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-21  Initial implementation.
-------------------------------------------------------------------------------
 Notes       :
   - Phase 6 model. No DI registration required; owned by AppStateStoreV2.
===============================================================================
*/

namespace AbsenceApp.Client.State.V2;

/// <summary>
/// Mutable runtime UI state managed by AppStateStoreV2.
/// Components observe changes via the store's OnChange event.
/// </summary>
public sealed class UiStateV2
{
    // -------------------------------------------------------------------------
    // Loading
    // -------------------------------------------------------------------------
    /// <summary>True while a full-page data load is in progress.</summary>
    public bool IsLoading { get; internal set; }

    /// <summary>True while a background save/submit operation is running.</summary>
    public bool IsSaving { get; internal set; }

    // -------------------------------------------------------------------------

    // Error

    // -------------------------------------------------------------------------
    /// <summary>Current page-level error message. Null when no active error.</summary>
    public string? PageError { get; internal set; }

    /// <summary>True when PageError has a value.</summary>
    public bool HasPageError => !string.IsNullOrEmpty(PageError);

    // -------------------------------------------------------------------------

    // Navigation context

    // -------------------------------------------------------------------------
    /// <summary>Current page title displayed in the V2 header/breadcrumb.</summary>
    public string CurrentPageTitle { get; internal set; } = string.Empty;

    /// <summary>Breadcrumb segments for the current V2 route.</summary>
    public IReadOnlyList<string> Breadcrumb { get; internal set; } = [];

    // -------------------------------------------------------------------------

    // Dark mode

    // -------------------------------------------------------------------------
    /// <summary>True when the V2 shell is in dark mode.</summary>
    public bool DarkMode { get; internal set; }

    // -------------------------------------------------------------------------

    // Sidebar

    // -------------------------------------------------------------------------
    /// <summary>True when the V2 sidebar is collapsed to icon-only mode.</summary>
    public bool SidebarCollapsed { get; internal set; }
}
