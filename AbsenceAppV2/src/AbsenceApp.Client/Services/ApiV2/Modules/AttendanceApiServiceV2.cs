/*
===============================================================================
 File        : AttendanceApiServiceV2.cs
 Namespace   : AbsenceApp.Client.Services.ApiV2.Modules
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-21
 Updated     : 2026-03-21
-------------------------------------------------------------------------------
 Purpose     : V2 API service for the Attendance module. Provides strongly-typed methods for all attendance CRUD and sub-resource endpoints, delegating HTTP calls to ApiClientV2 and returning ApiResponseV2 envelopes.
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
/// V2 API service for attendance data. Register as Scoped in MauiProgram.cs
/// when instructed (Phase 10).
/// </summary>
public sealed class AttendanceApiServiceV2 : ApiServiceBaseV2
{
    protected override string RoutePrefix => "api/attendance";

    public AttendanceApiServiceV2(ApiClientV2 client) : base(client) { }

    // -------------------------------------------------------------------------

    // Read

    // -------------------------------------------------------------------------
    /// <summary>Returns a paged list of attendance records.</summary>
    public Task<ApiResponseV2<PagedResultV2<AttendanceDto>>> GetPagedAsync(
        int page = 1,
        int pageSize = 25,
        string? queryString = null,
        CancellationToken ct = default) =>
        GetPagedAsync<AttendanceDto>(page, pageSize, queryString, ct);

    /// <summary>
    /// Returns the attendance register for a specific class session.
    /// Maps to GET api/attendance/{classId}/register.
    /// </summary>
    public Task<ApiResponseV2<AttendanceRegisterDto>> GetRegisterAsync(
        long classId,
        CancellationToken ct = default) =>
        Client.GetAsync<AttendanceRegisterDto>($"{RoutePrefix}/{classId}/register", ct);

    // -------------------------------------------------------------------------

    // Write

    // -------------------------------------------------------------------------
    /// <summary>
    /// Marks attendance for a class session.
    /// Maps to POST api/attendance/{classId}/mark.
    /// </summary>
    public Task<ApiResponseV2> MarkAttendanceAsync(
        long classId,
        AttendanceMarkDto body,
        CancellationToken ct = default) =>
        Client.PostAsync<AttendanceMarkDto>($"{RoutePrefix}/{classId}/mark", body, ct);
}
