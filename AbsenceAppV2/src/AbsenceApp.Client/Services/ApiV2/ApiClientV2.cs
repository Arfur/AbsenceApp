/*
===============================================================================
 File        : ApiClientV2.cs
 Namespace   : AbsenceApp.Client.Services.ApiV2
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-21
 Updated     : 2026-03-21
-------------------------------------------------------------------------------
 Purpose     : Low-level HTTP client wrapper for all V2 API calls. Handles serialisation/deserialisation, base URL configuration, auth header injection, and converts HTTP errors into ApiErrorV2 instances wrapped in ApiResponseV2.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-21  Initial implementation.
-------------------------------------------------------------------------------
 Notes       :
   - Phase 6 service. Register as Singleton in DI.
===============================================================================
*/

using System.Net.Http.Json;
using System.Text.Json;
using AbsenceApp.Client.Models.DataV2;

namespace AbsenceApp.Client.Services.ApiV2;

/// <summary>
/// HTTP client wrapper used by all V2 API module services.
/// Abstracts HttpClient lifecycle and centralises error parsing.
/// Register via AddHttpClient&lt;ApiClientV2&gt; in MauiProgram.cs (Phase 10).
/// </summary>
public sealed class ApiClientV2
{
    private readonly HttpClient _http;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    public ApiClientV2(HttpClient http)
    {
        _http = http;
    }

    // -------------------------------------------------------------------------

    // GET

    // -------------------------------------------------------------------------
    /// <summary>
    /// Sends a GET request and deserializes the response body as T.
    /// </summary>
    public async Task<ApiResponseV2<T>> GetAsync<T>(
        string path,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _http.GetAsync(path, cancellationToken);
            return await ParseResponseAsync<T>(response);
        }
        catch (Exception ex)
        {
            return ApiResponseV2<T>.Fail(BuildNetworkError(ex));
        }
    }

    // -------------------------------------------------------------------------

    // POST

    // -------------------------------------------------------------------------
    /// <summary>
    /// Sends a POST request with a JSON body and deserializes the response as T.
    /// </summary>
    public async Task<ApiResponseV2<T>> PostAsync<TBody, T>(
        string path,
        TBody body,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _http.PostAsJsonAsync(path, body, JsonOptions, cancellationToken);
            return await ParseResponseAsync<T>(response);
        }
        catch (Exception ex)
        {
            return ApiResponseV2<T>.Fail(BuildNetworkError(ex));
        }
    }

    /// <summary>
    /// Sends a POST request and returns a non-generic result (for void/204 endpoints).
    /// </summary>
    public async Task<ApiResponseV2> PostAsync<TBody>(
        string path,
        TBody body,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _http.PostAsJsonAsync(path, body, JsonOptions, cancellationToken);
            return await ParseVoidResponseAsync(response);
        }
        catch (Exception ex)
        {
            return ApiResponseV2.Fail(BuildNetworkError(ex));
        }
    }

    // -------------------------------------------------------------------------

    // PUT

    // -------------------------------------------------------------------------
    /// <summary>
    /// Sends a PUT request with a JSON body and deserializes the response as T.
    /// </summary>
    public async Task<ApiResponseV2<T>> PutAsync<TBody, T>(
        string path,
        TBody body,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _http.PutAsJsonAsync(path, body, JsonOptions, cancellationToken);
            return await ParseResponseAsync<T>(response);
        }
        catch (Exception ex)
        {
            return ApiResponseV2<T>.Fail(BuildNetworkError(ex));
        }
    }

    /// <summary>
    /// Sends a PUT request and returns a non-generic result.
    /// </summary>
    public async Task<ApiResponseV2> PutAsync<TBody>(
        string path,
        TBody body,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _http.PutAsJsonAsync(path, body, JsonOptions, cancellationToken);
            return await ParseVoidResponseAsync(response);
        }
        catch (Exception ex)
        {
            return ApiResponseV2.Fail(BuildNetworkError(ex));
        }
    }

    // -------------------------------------------------------------------------

    // DELETE

    // -------------------------------------------------------------------------
    /// <summary>
    /// Sends a DELETE request and returns a non-generic result.
    /// </summary>
    public async Task<ApiResponseV2> DeleteAsync(
        string path,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _http.DeleteAsync(path, cancellationToken);
            return await ParseVoidResponseAsync(response);
        }
        catch (Exception ex)
        {
            return ApiResponseV2.Fail(BuildNetworkError(ex));
        }
    }

    // -------------------------------------------------------------------------

    // Private helpers

    // -------------------------------------------------------------------------
    private static async Task<ApiResponseV2<T>> ParseResponseAsync<T>(HttpResponseMessage response)
    {
        var statusCode = (int)response.StatusCode;

        if (response.IsSuccessStatusCode)
        {
            // Handle 204 No Content
            if (statusCode == 204) return ApiResponseV2<T>.Ok(default!, statusCode);

            try
            {
                var data = await response.Content.ReadFromJsonAsync<T>(JsonOptions);
                return data is not null
                    ? ApiResponseV2<T>.Ok(data, statusCode)
                    : ApiResponseV2<T>.Fail("Empty response body.", statusCode);
            }
            catch (JsonException)
            {
                return ApiResponseV2<T>.Fail("Invalid response format.", statusCode);
            }
        }

        return ApiResponseV2<T>.Fail(await ReadErrorMessageAsync(response), statusCode);
    }

    private static async Task<ApiResponseV2> ParseVoidResponseAsync(HttpResponseMessage response)
    {
        var statusCode = (int)response.StatusCode;

        if (response.IsSuccessStatusCode) return ApiResponseV2.Ok(statusCode);

        return ApiResponseV2.Fail(await ReadErrorMessageAsync(response), statusCode);
    }

    private static async Task<string> ReadErrorMessageAsync(HttpResponseMessage response)
    {
        try
        {
            var error = await response.Content.ReadFromJsonAsync<ApiErrorV2>(JsonOptions);
            return error?.GetMessage() ?? response.ReasonPhrase ?? "Request failed.";
        }
        catch
        {
            return response.ReasonPhrase ?? $"HTTP {(int)response.StatusCode}";
        }
    }

    private static string BuildNetworkError(Exception ex) => ex switch
    {
        TaskCanceledException or OperationCanceledException => "The request timed out.",
        HttpRequestException e => e.Message.Length < 200 ? e.Message : "A network error occurred.",
        _ => "An unexpected error occurred.",
    };
}
