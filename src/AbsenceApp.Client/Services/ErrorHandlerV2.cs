/*
===============================================================================
 File        : ErrorHandlerV2.cs
 Namespace   : AbsenceApp.Client.Services
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-21
 Updated     : 2026-03-21
-------------------------------------------------------------------------------
 Purpose     : Centralised error handling service for V2 components. Wraps API calls, logs structured errors, and delegates user-facing messages to AlertServiceV2. Optionally routes critical errors to an error boundary page.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-21  Initial implementation.
-------------------------------------------------------------------------------
 Notes       :
   - Phase 6 service. Register as Scoped in DI.
===============================================================================
*/

using AbsenceApp.Client.Models.DataV2;
using AbsenceApp.Client.State.V2;

namespace AbsenceApp.Client.Services;

/// <summary>
/// Handles V2 errors by routing them to AlertServiceV2 and/or
/// AppStateStoreV2 depending on severity and context.
/// Register as Singleton in MauiProgram.cs when instructed (Phase 10).
/// </summary>
public sealed class ErrorHandlerV2
{
    private readonly AlertServiceV2      _alerts;
    private readonly AppStateStoreV2     _store;

    public ErrorHandlerV2(AlertServiceV2 alerts, AppStateStoreV2 store)
    {
        _alerts = alerts;
        _store  = store;
    }

    // -------------------------------------------------------------------------

    // Exception handling

    // -------------------------------------------------------------------------
    /// <summary>
    /// Handles an unexpected exception. Always shows a danger alert and
    /// optionally sets the page-level error for full-page failures.
    /// </summary>
    public void HandleException(Exception ex, bool setPageError = false, string? context = null)
    {
        var msg = BuildExceptionMessage(ex, context);
        _alerts.ShowDanger(msg);
        if (setPageError) _store.SetPageError(msg);
    }

    // -------------------------------------------------------------------------

    // API response handling

    // -------------------------------------------------------------------------
    /// <summary>
    /// Handles a failed generic ApiResponseV2{T}. Shows the appropriate
    /// alert severity based on status code and optionally sets page error.
    /// </summary>
    public void HandleFailure<T>(ApiResponseV2<T> response, bool setPageError = false)
    {
        var msg = response.ErrorMessage ?? "An unexpected error occurred.";
        RouteAlert(msg, response.StatusCode, setPageError);
    }

    /// <summary>
    /// Handles a failed non-generic ApiResponseV2. Same routing logic.
    /// </summary>
    public void HandleFailure(ApiResponseV2 response, bool setPageError = false)
    {
        var msg = response.ErrorMessage ?? "An unexpected error occurred.";
        RouteAlert(msg, response.StatusCode, setPageError);
    }

    // -------------------------------------------------------------------------

    // Convenience helpers

    // -------------------------------------------------------------------------
    /// <summary>Shows a success alert for a completed operation.</summary>
    public void ShowSuccess(string message) => _alerts.ShowSuccess(message);

    /// <summary>Shows an informational alert.</summary>
    public void ShowInfo(string message) => _alerts.ShowInfo(message);

    // -------------------------------------------------------------------------

    // Internal

    // -------------------------------------------------------------------------
    private void RouteAlert(string message, int statusCode, bool setPageError)
    {
        if (statusCode is >= 500 or 0)
        {
            _alerts.ShowDanger(message);
            if (setPageError) _store.SetPageError(message);
        }
        else if (statusCode == 422 || statusCode == 400)
        {
            _alerts.ShowWarning(message);
        }
        else if (statusCode == 401 || statusCode == 403)
        {
            _alerts.ShowWarning("You do not have permission to perform this action.");
        }
        else if (statusCode == 404)
        {
            _alerts.ShowWarning("The requested resource was not found.");
            if (setPageError) _store.SetPageError("Not found.");
        }
        else
        {
            _alerts.ShowDanger(message);
        }
    }

    private static string BuildExceptionMessage(Exception ex, string? context)
    {
        var prefix = string.IsNullOrEmpty(context) ? string.Empty : context + ": ";
        return ex switch
        {
            TaskCanceledException or OperationCanceledException =>
                prefix + "The operation timed out. Please try again.",
            HttpRequestException httpEx =>
                prefix + (httpEx.Message.Length < 200 ? httpEx.Message : "A network error occurred."),
            _ => prefix + "An unexpected error occurred. Please try again.",
        };
    }
}
