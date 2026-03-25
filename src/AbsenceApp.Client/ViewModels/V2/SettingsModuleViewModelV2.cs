/*
===============================================================================
 File        : SettingsModuleViewModelV2.cs
 Namespace   : AbsenceApp.Client.ViewModels.V2
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-21
 Updated     : 2026-03-21
-------------------------------------------------------------------------------
 Purpose     : ViewModel for the Settings module V2 pages. Manages table page
               settings: load by page name, track changes, and save via
               SettingsApiServiceV2. Also exposes the diagnostics report.
               Drives SettingsListPageV2, SettingsDetailPageV2, and
               SettingsFormPageV2.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-21  Initial implementation (Phase 7).
-------------------------------------------------------------------------------
 Notes       :
   - Register as Scoped in DI (Phase 10).
===============================================================================
*/

using AbsenceApp.Client.Services.ApiV2.Modules;
using AbsenceApp.Core.DTOs;

namespace AbsenceApp.Client.ViewModels.V2;

/// <summary>
/// Drives the Settings module V2 pages. Register as Scoped (Phase 10).
/// </summary>
public sealed class SettingsModuleViewModelV2
{
    private readonly SettingsApiServiceV2 _api;

    public SettingsModuleViewModelV2(SettingsApiServiceV2 api) => _api = api;

    // -------------------------------------------------------------------------
    // Table settings state
    // -------------------------------------------------------------------------

    public string CurrentPageName { get; private set; } = string.Empty;
    public List<TablePageSettingDto> Settings { get; private set; } = [];
    public bool IsLoading { get; private set; }
    public bool IsBusy { get; private set; }
    public string? Error { get; private set; }
    public bool SaveSuccess { get; private set; }

    // -------------------------------------------------------------------------
    // Diagnostics state
    // -------------------------------------------------------------------------

    public TableSettingsDiagnosticReport? DiagnosticsReport { get; private set; }
    public bool IsDiagnosticsLoading { get; private set; }
    public string? DiagnosticsError { get; private set; }

    // -------------------------------------------------------------------------
    // Data loading
    // -------------------------------------------------------------------------

    public async Task LoadSettingsAsync(string pageName, CancellationToken ct = default)
    {
        CurrentPageName = pageName;
        IsLoading = true;
        Error = null;

        var result = await _api.GetTableSettingsAsync(pageName, ct);
        if (result.Success && result.Data is not null)
            Settings = result.Data;
        else
            Error = result.ErrorMessage ?? $"Failed to load settings for '{pageName}'.";

        IsLoading = false;
    }

    public async Task LoadDiagnosticsAsync(CancellationToken ct = default)
    {
        IsDiagnosticsLoading = true;
        DiagnosticsError = null;

        var result = await _api.GetDiagnosticsAsync(ct);
        if (result.Success)
            DiagnosticsReport = result.Data;
        else
            DiagnosticsError = result.ErrorMessage ?? "Failed to load diagnostics.";

        IsDiagnosticsLoading = false;
    }

    // -------------------------------------------------------------------------
    // Actions
    // -------------------------------------------------------------------------

    public async Task<bool> SaveSettingsAsync(CancellationToken ct = default)
    {
        IsBusy = true;
        Error = null;
        SaveSuccess = false;

        var result = await _api.SaveTableSettingsAsync(CurrentPageName, Settings, ct);
        SaveSuccess = result.Success;
        if (!result.Success)
            Error = result.ErrorMessage ?? "Failed to save settings.";

        IsBusy = false;
        return SaveSuccess;
    }
}
