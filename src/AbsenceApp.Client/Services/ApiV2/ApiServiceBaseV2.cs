/*
===============================================================================
 File        : ApiServiceBaseV2.cs
 Namespace   : AbsenceApp.Client.Services.ApiV2
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-21
 Updated     : 2026-03-21
-------------------------------------------------------------------------------
 Purpose     : Base class for all V2 API module services. Provides shared helper methods (GetAsync, PostAsync, PutAsync, DeleteAsync) that delegate to ApiClientV2 and unwrap ApiResponseV2 results, reducing boilerplate in module services.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-21  Initial implementation.
-------------------------------------------------------------------------------
 Notes       :
   - Phase 6 service. Inherit; do not register directly.
===============================================================================
*/

using AbsenceApp.Client.Models.DataV2;

namespace AbsenceApp.Client.Services.ApiV2;

/// <summary>
/// Abstract base for V2 module API services. Injected classes must supply
/// the route prefix (e.g. "api/students") and the scoped ApiClientV2.
/// </summary>
public abstract class ApiServiceBaseV2
{
    private protected readonly ApiClientV2 Client;

    /// <summary>
    /// API route prefix for this module (without leading or trailing slash).
    /// Example: "api/students"
    /// </summary>
    protected abstract string RoutePrefix { get; }

    protected ApiServiceBaseV2(ApiClientV2 client)
    {
        Client = client;
    }

    // -------------------------------------------------------------------------

    // Generic CRUD helpers

    // -------------------------------------------------------------------------
    /// <summary>
    /// GET {RoutePrefix}/{id} — Fetch a single entity by ID.
    /// </summary>
    protected Task<ApiResponseV2<T>> GetByIdAsync<T>(
        long id,
        CancellationToken ct = default) =>
        Client.GetAsync<T>($"{RoutePrefix}/{id}", ct);

    /// <summary>
    /// GET {RoutePrefix}?page={page}&amp;pageSize={pageSize} — Fetch a paged list.
    /// </summary>
    protected Task<ApiResponseV2<PagedResultV2<T>>> GetPagedAsync<T>(
        int page = 1,
        int pageSize = 25,
        string? queryString = null,
        CancellationToken ct = default)
    {
        var url = $"{RoutePrefix}?page={page}&pageSize={pageSize}";
        if (!string.IsNullOrEmpty(queryString)) url += "&" + queryString.TrimStart('&', '?');
        return Client.GetAsync<PagedResultV2<T>>(url, ct);
    }

    /// <summary>
    /// GET {RoutePrefix} — Fetch the full list (non-paged).
    /// </summary>
    protected Task<ApiResponseV2<List<T>>> GetListAsync<T>(
        string? queryString = null,
        CancellationToken ct = default)
    {
        var url = RoutePrefix;
        if (!string.IsNullOrEmpty(queryString)) url += "?" + queryString.TrimStart('&', '?');
        return Client.GetAsync<List<T>>(url, ct);
    }

    /// <summary>
    /// POST {RoutePrefix} — Create a new entity.
    /// </summary>
    protected Task<ApiResponseV2<TResult>> CreateAsync<TBody, TResult>(
        TBody body,
        CancellationToken ct = default) =>
        Client.PostAsync<TBody, TResult>(RoutePrefix, body, ct);

    /// <summary>
    /// PUT {RoutePrefix}/{id} — Full update of an entity.
    /// </summary>
    protected Task<ApiResponseV2> UpdateAsync<TBody>(
        long id,
        TBody body,
        CancellationToken ct = default) =>
        Client.PutAsync($"{RoutePrefix}/{id}", body, ct);

    /// <summary>
    /// DELETE {RoutePrefix}/{id} — Delete an entity.
    /// </summary>
    protected Task<ApiResponseV2> DeleteAsync(
        long id,
        CancellationToken ct = default) =>
        Client.DeleteAsync($"{RoutePrefix}/{id}", ct);
}
