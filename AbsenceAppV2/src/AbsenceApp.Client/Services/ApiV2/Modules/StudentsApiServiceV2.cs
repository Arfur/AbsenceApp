/*
===============================================================================
 File        : StudentsApiServiceV2.cs
 Namespace   : AbsenceApp.Client.Services.ApiV2.Modules
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-21
 Updated     : 2026-03-21
-------------------------------------------------------------------------------
 Purpose     : V2 API service for the Students module. Provides strongly-typed methods for all student CRUD and sub-resource endpoints, delegating HTTP calls to ApiClientV2 and returning ApiResponseV2 envelopes.
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
/// V2 API service for student data. Register as Scoped in MauiProgram.cs
/// when instructed (Phase 10).
/// </summary>
public sealed class StudentsApiServiceV2 : ApiServiceBaseV2
{
    protected override string RoutePrefix => "api/students";

    public StudentsApiServiceV2(ApiClientV2 client) : base(client) { }

    // -------------------------------------------------------------------------

    // Read

    // -------------------------------------------------------------------------
    /// <summary>Returns a paged list of students with optional search/filter query string.</summary>
    public Task<ApiResponseV2<PagedResultV2<StudentDto>>> GetPagedAsync(
        int page = 1,
        int pageSize = 25,
        string? queryString = null,
        CancellationToken ct = default) =>
        GetPagedAsync<StudentDto>(page, pageSize, queryString, ct);

    /// <summary>Returns the full detail view for a student by ID.</summary>
    public Task<ApiResponseV2<StudentFullViewDto>> GetDetailAsync(
        long id,
        CancellationToken ct = default) =>
        Client.GetAsync<StudentFullViewDto>($"{RoutePrefix}/{id}/full", ct);

    // -------------------------------------------------------------------------

    // Write

    // -------------------------------------------------------------------------
    /// <summary>Creates a new student record.</summary>
    public Task<ApiResponseV2<StudentDto>> CreateAsync(
        StudentDto body,
        CancellationToken ct = default) =>
        CreateAsync<StudentDto, StudentDto>(body, ct);

    /// <summary>Updates an existing student by ID.</summary>
    public Task<ApiResponseV2> UpdateAsync(
        long id,
        StudentDto body,
        CancellationToken ct = default) =>
        UpdateAsync<StudentDto>(id, body, ct);

    // -------------------------------------------------------------------------

    // Sub-resources

    // -------------------------------------------------------------------------
    /// <summary>Returns absence history for a student.</summary>
    public Task<ApiResponseV2<List<StudentAbsenceDto>>> GetAbsencesAsync(
        long studentId,
        CancellationToken ct = default) =>
        Client.GetAsync<List<StudentAbsenceDto>>($"{RoutePrefix}/{studentId}/absences", ct);

    // -------------------------------------------------------------------------
    // Search / Filter
    // -------------------------------------------------------------------------

    /// <summary>Full-text search across student records.</summary>
    public Task<ApiResponseV2<PagedResultV2<StudentDto>>> SearchAsync(
        string term,
        int page = 1,
        int pageSize = 25,
        CancellationToken ct = default) =>
        GetPagedAsync<StudentDto>(page, pageSize, $"search={Uri.EscapeDataString(term)}", ct);

    /// <summary>Filter students by one or more field values.</summary>
    public Task<ApiResponseV2<PagedResultV2<StudentDto>>> FilterAsync(
        Dictionary<string, string> filters,
        int page = 1,
        int pageSize = 25,
        CancellationToken ct = default) =>
        GetPagedAsync<StudentDto>(
            page,
            pageSize,
            string.Join("&", filters
                .Where(f => !string.IsNullOrWhiteSpace(f.Value))
                .Select(f => $"{f.Key}={Uri.EscapeDataString(f.Value)}")),
            ct);
}
