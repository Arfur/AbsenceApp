/*
===============================================================================
 File        : StaffApiServiceV2.cs
 Namespace   : AbsenceApp.Client.Services.ApiV2.Modules
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-21
 Updated     : 2026-03-21
-------------------------------------------------------------------------------
 Purpose     : V2 API service for the Staff module. Provides strongly-typed methods for all staff CRUD and sub-resource endpoints, delegating HTTP calls to ApiClientV2 and returning ApiResponseV2 envelopes.
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
/// V2 API service for staff data. Register as Scoped in MauiProgram.cs
/// when instructed (Phase 10).
/// </summary>
public sealed class StaffApiServiceV2 : ApiServiceBaseV2
{
    protected override string RoutePrefix => "api/staff";

    public StaffApiServiceV2(ApiClientV2 client) : base(client) { }

    // -------------------------------------------------------------------------

    // Read

    // -------------------------------------------------------------------------
    /// <summary>Returns a paged list of staff members.</summary>
    public Task<ApiResponseV2<PagedResultV2<StaffDto>>> GetPagedAsync(
        int page = 1,
        int pageSize = 25,
        string? queryString = null,
        CancellationToken ct = default) =>
        GetPagedAsync<StaffDto>(page, pageSize, queryString, ct);

    /// <summary>Returns the full detail view for a staff member by ID.</summary>
    public Task<ApiResponseV2<StaffFullViewDto>> GetDetailAsync(
        long id,
        CancellationToken ct = default) =>
        Client.GetAsync<StaffFullViewDto>($"{RoutePrefix}/{id}/full", ct);

    // -------------------------------------------------------------------------

    // Write

    // -------------------------------------------------------------------------
    /// <summary>Creates a new staff record.</summary>
    public Task<ApiResponseV2<StaffDto>> CreateAsync(
        StaffDto body,
        CancellationToken ct = default) =>
        CreateAsync<StaffDto, StaffDto>(body, ct);

    /// <summary>Updates an existing staff member by ID.</summary>
    public Task<ApiResponseV2> UpdateAsync(
        long id,
        StaffDto body,
        CancellationToken ct = default) =>
        UpdateAsync<StaffDto>(id, body, ct);

    // -------------------------------------------------------------------------

    // Sub-resources

    // -------------------------------------------------------------------------
    /// <summary>Returns absence records for a staff member.</summary>
    public Task<ApiResponseV2<List<StaffAbsenceDto>>> GetAbsencesAsync(
        long staffId,
        CancellationToken ct = default) =>
        Client.GetAsync<List<StaffAbsenceDto>>($"{RoutePrefix}/{staffId}/absences", ct);
}
