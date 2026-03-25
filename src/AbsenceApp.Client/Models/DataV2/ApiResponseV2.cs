/*
===============================================================================
 File        : ApiResponseV2.cs
 Namespace   : AbsenceApp.Client.Models.DataV2
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-21
 Updated     : 2026-03-21
-------------------------------------------------------------------------------
 Purpose     : Generic API response envelope for all V2 API calls. Wraps a typed Data payload with Success flag, StatusCode, and an optional ApiErrorV2 for failure details.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-21  Initial implementation.
-------------------------------------------------------------------------------
 Notes       :
   - Phase 6 model. No DI registration required.
===============================================================================
*/

namespace AbsenceApp.Client.Models.DataV2;

/// <summary>
/// Standard response envelope returned by ApiServiceBaseV2-derived services.
/// </summary>
/// <typeparam name="T">The payload type. May be null on failure.</typeparam>
public sealed class ApiResponseV2<T>
{
    /// <summary>True when the request completed successfully (HTTP 2xx).</summary>
    public bool Success { get; init; }

    /// <summary>HTTP status code returned by the API. 0 if no response was received.</summary>
    public int StatusCode { get; init; }

    /// <summary>The response payload. Null when Success is false.</summary>
    public T? Data { get; init; }

    /// <summary>User-facing error or validation message (non-null when Success is false).</summary>
    public string? ErrorMessage { get; init; }

    /// <summary>Structured field-level validation errors keyed by field name.</summary>
    public Dictionary<string, string[]> ValidationErrors { get; init; } = [];

    // -------------------------------------------------------------------------

    // Factory helpers

    // -------------------------------------------------------------------------
    /// <summary>Creates a successful response wrapping the given payload.</summary>
    public static ApiResponseV2<T> Ok(T data, int statusCode = 200) =>
        new() { Success = true, StatusCode = statusCode, Data = data };

    /// <summary>Creates a failure response with the given error message and status code.</summary>
    public static ApiResponseV2<T> Fail(string message, int statusCode = 0) =>
        new() { Success = false, StatusCode = statusCode, ErrorMessage = message };

    /// <summary>Creates a failure response with structured validation errors.</summary>
    public static ApiResponseV2<T> FailValidation(
        string message,
        Dictionary<string, string[]> errors,
        int statusCode = 422) =>
        new() { Success = false, StatusCode = statusCode, ErrorMessage = message, ValidationErrors = errors };
}

/// <summary>
/// Non-generic response envelope for void/no-content API operations.
/// </summary>
public sealed class ApiResponseV2
{
    /// <summary>True when the request completed successfully.</summary>
    public bool Success { get; init; }

    /// <summary>HTTP status code returned by the API.</summary>
    public int StatusCode { get; init; }

    /// <summary>User-facing error message (non-null when Success is false).</summary>
    public string? ErrorMessage { get; init; }

    /// <summary>Structured field-level validation errors keyed by field name.</summary>
    public Dictionary<string, string[]> ValidationErrors { get; init; } = [];

    // -------------------------------------------------------------------------

    // Factory helpers

    // -------------------------------------------------------------------------
    /// <summary>Creates a successful no-content response.</summary>
    public static ApiResponseV2 Ok(int statusCode = 200) =>
        new() { Success = true, StatusCode = statusCode };

    /// <summary>Creates a failure response.</summary>
    public static ApiResponseV2 Fail(string message, int statusCode = 0) =>
        new() { Success = false, StatusCode = statusCode, ErrorMessage = message };
}
