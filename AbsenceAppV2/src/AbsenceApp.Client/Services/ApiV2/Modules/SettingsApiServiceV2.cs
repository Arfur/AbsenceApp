/*
===============================================================================
 File        : SettingsApiServiceV2.cs
 Namespace   : AbsenceApp.Client.Services.ApiV2.Modules
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-21
 Updated     : 2026-03-21
-------------------------------------------------------------------------------
 Purpose     : V2 API service for the Settings module. Provides strongly-typed methods for reading and persisting application settings, delegating HTTP calls to ApiClientV2 and returning ApiResponseV2 envelopes.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-21  Initial implementation.
-------------------------------------------------------------------------------
 Notes       :
   - Phase 6 service. Register as Scoped in DI.
===============================================================================
*/

using AbsenceApp.Client.Models.DataV2;
using AbsenceApp.Core.DTOs;

namespace AbsenceApp.Client.Services.ApiV2.Modules;

/// <summary>
/// V2 API service for application settings. Register as Scoped in
/// MauiProgram.cs when instructed (Phase 10).
/// </summary>
public sealed class SettingsApiServiceV2 : ApiServiceBaseV2
{
    protected override string RoutePrefix => "api/settings";

    public SettingsApiServiceV2(ApiClientV2 client) : base(client) { }

    // -------------------------------------------------------------------------

    // Table Page Settings

    // -------------------------------------------------------------------------
    /// <summary>
    /// Returns all table page settings for a given page name.
    /// Maps to GET api/settings/table/{pageName}.
    /// </summary>
    public Task<ApiResponseV2<List<TablePageSettingDto>>> GetTableSettingsAsync(
        string pageName,
        CancellationToken ct = default) =>
        Client.GetAsync<List<TablePageSettingDto>>($"{RoutePrefix}/table/{pageName}", ct);

    /// <summary>
    /// Saves (upserts) the table page settings for a given page name.
    /// Maps to POST api/settings/table/{pageName}.
    /// </summary>
    public Task<ApiResponseV2> SaveTableSettingsAsync(
        string pageName,
        List<TablePageSettingDto> body,
        CancellationToken ct = default) =>
        Client.PostAsync<List<TablePageSettingDto>>($"{RoutePrefix}/table/{pageName}", body, ct);

    // -------------------------------------------------------------------------

    // Diagnostics

    // -------------------------------------------------------------------------
    /// <summary>
    /// Returns the admin table-settings diagnostic report.
    /// Maps to GET api/settings/table/diagnostics.
    /// </summary>
    public Task<ApiResponseV2<TableSettingsDiagnosticReport>> GetDiagnosticsAsync(
        CancellationToken ct = default) =>
        Client.GetAsync<TableSettingsDiagnosticReport>($"{RoutePrefix}/table/diagnostics", ct);
}
