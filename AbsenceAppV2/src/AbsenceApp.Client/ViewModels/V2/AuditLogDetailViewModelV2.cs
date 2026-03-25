/*
===============================================================================
 File        : AuditLogDetailViewModelV2.cs
 Namespace   : AbsenceApp.Client.ViewModels.V2
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-21
 Updated     : 2026-03-21
-------------------------------------------------------------------------------
 Purpose     : ViewModel for AuditLogDetailPageV2 and AuditLogFormPageV2.
               Loads a single audit log entry by ID (read-only) via
               AuditLogApiServiceV2.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-21  Initial implementation (Phase 7).
-------------------------------------------------------------------------------
 Notes       :
   - Register as Scoped in DI (Phase 10).
   - Audit log entries are read-only; no create/update/delete operations.
===============================================================================
*/

using AbsenceApp.Client.Services.ApiV2.Modules;
using AbsenceApp.Core.DTOs;

namespace AbsenceApp.Client.ViewModels.V2;

/// <summary>
/// Drives the Audit Log detail page V2. Read-only. Register as Scoped (Phase 10).
/// </summary>
public sealed class AuditLogDetailViewModelV2
{
    private readonly AuditLogApiServiceV2 _api;

    public AuditLogDetailViewModelV2(AuditLogApiServiceV2 api) => _api = api;

    // -------------------------------------------------------------------------
    // State
    // -------------------------------------------------------------------------

    public AuditLogDto? Item { get; private set; }
    public bool IsLoading { get; private set; }
    public string? Error { get; private set; }

    // -------------------------------------------------------------------------
    // Data loading
    // -------------------------------------------------------------------------

    public async Task LoadAsync(long id, CancellationToken ct = default)
    {
        IsLoading = true;
        Error = null;
        Item = null;

        var result = await _api.GetByIdAsync(id, ct);
        if (result.Success)
            Item = result.Data;
        else
            Error = result.ErrorMessage ?? "Failed to load audit log entry.";

        IsLoading = false;
    }
}
