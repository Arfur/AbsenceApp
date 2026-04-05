/*
===============================================================================
 File        : ParentsApiServiceV2.cs
 Namespace   : AbsenceApp.Client.Services.ApiV2.Modules
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-27
 Updated     : 2026-03-27
-------------------------------------------------------------------------------
 Purpose     : V2 API service for the Parents module. Provides strongly-typed
               methods for parent CRUD endpoints, delegating HTTP calls to
               ApiClientV2 and returning ApiResponseV2 envelopes.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-27  Initial implementation (Phase 3 API additions).
-------------------------------------------------------------------------------
 Notes       :
   - Register as Scoped in V2ServiceCollectionExtensions.cs.
===============================================================================
*/

using AbsenceApp.Client.Models.DataV2;
using AbsenceApp.Core.DTOs;

namespace AbsenceApp.Client.Services.ApiV2.Modules;

/// <summary>
/// V2 API service for parent data. Register as Scoped in DI.
/// </summary>
public sealed class ParentsApiServiceV2 : ApiServiceBaseV2
{
    protected override string RoutePrefix => "api/parents";

    public ParentsApiServiceV2(ApiClientV2 client) : base(client) { }

    // -------------------------------------------------------------------------
    // Read
    // -------------------------------------------------------------------------

    /// <summary>Returns a paged list of parents with optional search/filter query string.</summary>
    public Task<ApiResponseV2<PagedResultV2<ParentDto>>> GetPagedAsync(
        int page = 1,
        int pageSize = 25,
        string? queryString = null,
        CancellationToken ct = default) =>
        GetPagedAsync<ParentDto>(page, pageSize, queryString, ct);

    /// <summary>Returns a single parent by ID.</summary>
    public Task<ApiResponseV2<ParentDto>> GetByIdAsync(
        long id,
        CancellationToken ct = default) =>
        GetByIdAsync<ParentDto>(id, ct);

    /// <summary>Full-text search across parent records.</summary>
    public Task<ApiResponseV2<PagedResultV2<ParentDto>>> SearchAsync(
        string term,
        int page = 1,
        int pageSize = 25,
        CancellationToken ct = default) =>
        GetPagedAsync<ParentDto>(page, pageSize, $"search={Uri.EscapeDataString(term)}", ct);

    /// <summary>Filter parents by one or more field values.</summary>
    public Task<ApiResponseV2<PagedResultV2<ParentDto>>> FilterAsync(
        Dictionary<string, string> filters,
        int page = 1,
        int pageSize = 25,
        CancellationToken ct = default) =>
        GetPagedAsync<ParentDto>(
            page,
            pageSize,
            string.Join("&", filters
                .Where(f => !string.IsNullOrWhiteSpace(f.Value))
                .Select(f => $"{f.Key}={Uri.EscapeDataString(f.Value)}")),
            ct);

    // -------------------------------------------------------------------------
    // Write
    // -------------------------------------------------------------------------

    /// <summary>Creates a new parent record.</summary>
    public Task<ApiResponseV2<ParentDto>> CreateAsync(
        ParentDto body,
        CancellationToken ct = default) =>
        CreateAsync<ParentDto, ParentDto>(body, ct);

    /// <summary>Updates an existing parent by ID.</summary>
    public Task<ApiResponseV2> UpdateAsync(
        long id,
        ParentDto body,
        CancellationToken ct = default) =>
        UpdateAsync<ParentDto>(id, body, ct);
}
