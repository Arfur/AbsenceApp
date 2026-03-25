/*
===============================================================================
 File        : ClassesApiServiceV2.cs
 Namespace   : AbsenceApp.Client.Services.ApiV2.Modules
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-21
 Updated     : 2026-03-21
-------------------------------------------------------------------------------
 Purpose     : V2 API service for the Classes module. Provides strongly-typed methods for all class CRUD and sub-resource endpoints, delegating HTTP calls to ApiClientV2 and returning ApiResponseV2 envelopes.
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
/// V2 API service for class data. Register as Scoped in MauiProgram.cs
/// when instructed (Phase 10).
/// </summary>
public sealed class ClassesApiServiceV2 : ApiServiceBaseV2
{
    protected override string RoutePrefix => "api/classes";

    public ClassesApiServiceV2(ApiClientV2 client) : base(client) { }

    // -------------------------------------------------------------------------

    // Read

    // -------------------------------------------------------------------------
    /// <summary>Returns a paged list of classes.</summary>
    public Task<ApiResponseV2<PagedResultV2<ClassDto>>> GetPagedAsync(
        int page = 1,
        int pageSize = 25,
        string? queryString = null,
        CancellationToken ct = default) =>
        GetPagedAsync<ClassDto>(page, pageSize, queryString, ct);

    /// <summary>Returns the full detail view for a class by ID.</summary>
    public Task<ApiResponseV2<ClassFullViewDto>> GetDetailAsync(
        long id,
        CancellationToken ct = default) =>
        Client.GetAsync<ClassFullViewDto>($"{RoutePrefix}/{id}/full", ct);

    // -------------------------------------------------------------------------

    // Write

    // -------------------------------------------------------------------------
    /// <summary>Creates a new class record.</summary>
    public Task<ApiResponseV2<ClassDto>> CreateAsync(
        ClassDto body,
        CancellationToken ct = default) =>
        CreateAsync<ClassDto, ClassDto>(body, ct);

    /// <summary>Updates an existing class by ID.</summary>
    public Task<ApiResponseV2> UpdateAsync(
        long id,
        ClassDto body,
        CancellationToken ct = default) =>
        UpdateAsync<ClassDto>(id, body, ct);

}
