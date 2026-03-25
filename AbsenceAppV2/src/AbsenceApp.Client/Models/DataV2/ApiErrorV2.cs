/*
===============================================================================
 File        : ApiErrorV2.cs
 Namespace   : AbsenceApp.Client.Models.DataV2
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-21
 Updated     : 2026-03-21
-------------------------------------------------------------------------------
 Purpose     : Structured error detail returned inside ApiResponseV2 on failure. Carries Code, Message, and an optional list of field-level validation errors.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-21  Initial implementation.
-------------------------------------------------------------------------------
 Notes       :
   - Phase 6 model. No DI registration required.
===============================================================================
*/

using System.Text.Json.Serialization;

namespace AbsenceApp.Client.Models.DataV2;

/// <summary>
/// Represents the JSON error body returned by the AbsenceApp API on failure.
/// </summary>
public sealed class ApiErrorV2
{
    /// <summary>Short machine-readable error type (e.g. "not_found", "validation_failed").</summary>
    [JsonPropertyName("type")]
    public string Type { get; init; } = string.Empty;

    /// <summary>Human-readable error summary.</summary>
    [JsonPropertyName("title")]
    public string Title { get; init; } = string.Empty;

    /// <summary>HTTP status code echoed in the body.</summary>
    [JsonPropertyName("status")]
    public int Status { get; init; }

    /// <summary>Optional longer description of the error.</summary>
    [JsonPropertyName("detail")]
    public string? Detail { get; init; }

    /// <summary>Field-level validation errors keyed by field name (RFC 7807 "errors" extension).</summary>
    [JsonPropertyName("errors")]
    public Dictionary<string, string[]> Errors { get; init; } = [];

    /// <summary>Returns the most descriptive available message.</summary>
    public string GetMessage() => Detail ?? Title;
}
