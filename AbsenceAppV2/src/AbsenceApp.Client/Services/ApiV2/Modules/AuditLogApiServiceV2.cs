/*
===============================================================================
 File        : AuditLogApiServiceV2.cs
 Namespace   : AbsenceApp.Client.Services.ApiV2.Modules
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-21
 Updated     : 2026-03-21
-------------------------------------------------------------------------------
 Purpose     : V2 API service for the AuditLog module. Provides strongly-typed read and filter endpoints for audit log entries, delegating HTTP calls to ApiClientV2 and returning ApiResponseV2 envelopes.
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
/// V2 API service for audit log data. Read-only — no create/update/delete.
/// Register as Scoped in MauiProgram.cs when instructed (Phase 10).
/// </summary>
public sealed class AuditLogApiServiceV2 : ApiServiceBaseV2
{
    protected override string RoutePrefix => "api/auditlog";

    public AuditLogApiServiceV2(ApiClientV2 client) : base(client) { }

    // -------------------------------------------------------------------------

    // Read

    // -------------------------------------------------------------------------
    /// <summary>Returns a paged list of audit log entries.</summary>
    public Task<ApiResponseV2<PagedResultV2<AuditLogDto>>> GetPagedAsync(
        int page = 1,
        int pageSize = 50,
        string? queryString = null,
        CancellationToken ct = default) =>
        GetPagedAsync<AuditLogDto>(page, pageSize, queryString, ct);

    /// <summary>Returns a single audit log entry by ID.</summary>
    public Task<ApiResponseV2<AuditLogDto>> GetByIdAsync(
        long id,
        CancellationToken ct = default) =>
        GetByIdAsync<AuditLogDto>(id, ct);
}
