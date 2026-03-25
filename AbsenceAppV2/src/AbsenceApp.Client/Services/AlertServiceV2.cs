/*
===============================================================================
 File        : AlertServiceV2.cs
 Namespace   : AbsenceApp.Client.Services
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-21
 Updated     : 2026-03-21
-------------------------------------------------------------------------------
 Purpose     : Programmatic API for showing dismissible alert/toast notifications from anywhere in the app. Maintains a bounded queue of active AlertV2Model entries and raises an event so AlertV2.razor can re-render.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-21  Initial implementation.
-------------------------------------------------------------------------------
 Notes       :
   - Phase 6 service. Register as Singleton in DI.
===============================================================================
*/

namespace AbsenceApp.Client.Services;

/// <summary>
/// Severity level of a V2 alert notification.
/// </summary>
public enum AlertSeverityV2 { Info, Success, Warning, Danger }

/// <summary>
/// An individual alert notification managed by AlertServiceV2.
/// </summary>
public sealed class AlertItemV2
{
    internal AlertItemV2(
        string message,
        AlertSeverityV2 severity,
        int autoDismissMs)
    {
        Id            = Guid.NewGuid();
        Message       = message;
        Severity      = severity;
        AutoDismissMs = autoDismissMs;
        CreatedAt     = DateTime.UtcNow;
    }

    /// <summary>Unique identifier for this alert (used as the dismiss key).</summary>
    public Guid Id { get; }

    /// <summary>The human-readable alert message.</summary>
    public string Message { get; }

    /// <summary>Visual severity (controls icon and colour).</summary>
    public AlertSeverityV2 Severity { get; }

    /// <summary>Auto-dismiss delay in milliseconds. 0 means manual dismiss only.</summary>
    public int AutoDismissMs { get; }

    /// <summary>UTC timestamp when this alert was created.</summary>
    public DateTime CreatedAt { get; }
}

/// <summary>
/// Singleton service for queuing V2 alert notifications. Components subscribe to
/// OnAlertsChanged and re-render when the alert stack changes.
/// Register in MauiProgram.cs as Singleton when instructed (Phase 10).
/// </summary>
public sealed class AlertServiceV2
{
    private readonly List<AlertItemV2> _alerts = [];
    private const int DefaultAutoDismissMs = 5000;

    /// <summary>Raised whenever the alert list changes. Subscribers call InvokeAsync(StateHasChanged).</summary>
    public event Action? OnAlertsChanged;

    /// <summary>Read-only snapshot of current active alerts.</summary>
    public IReadOnlyList<AlertItemV2> Alerts => _alerts;

    // -------------------------------------------------------------------------

    // Show helpers

    // -------------------------------------------------------------------------
    /// <summary>Shows an informational alert.</summary>
    public void ShowInfo(string message, int autoDismissMs = DefaultAutoDismissMs)
        => Add(message, AlertSeverityV2.Info, autoDismissMs);

    /// <summary>Shows a success alert.</summary>
    public void ShowSuccess(string message, int autoDismissMs = DefaultAutoDismissMs)
        => Add(message, AlertSeverityV2.Success, autoDismissMs);

    /// <summary>Shows a warning alert.</summary>
    public void ShowWarning(string message, int autoDismissMs = DefaultAutoDismissMs)
        => Add(message, AlertSeverityV2.Warning, autoDismissMs);

    /// <summary>Shows a danger/error alert. Does not auto-dismiss by default.</summary>
    public void ShowDanger(string message, int autoDismissMs = 0)
        => Add(message, AlertSeverityV2.Danger, autoDismissMs);

    /// <summary>Manually dismisses the alert with the given ID.</summary>
    public void Dismiss(Guid id)
    {
        var found = _alerts.FirstOrDefault(a => a.Id == id);
        if (found is not null)
        {
            _alerts.Remove(found);
            OnAlertsChanged?.Invoke();
        }
    }

    /// <summary>Dismisses all active alerts.</summary>
    public void DismissAll()
    {
        _alerts.Clear();
        OnAlertsChanged?.Invoke();
    }

    // -------------------------------------------------------------------------

    // Internal

    // -------------------------------------------------------------------------
    private void Add(string message, AlertSeverityV2 severity, int autoDismissMs)
    {
        var item = new AlertItemV2(message, severity, autoDismissMs);
        _alerts.Add(item);
        OnAlertsChanged?.Invoke();

        if (autoDismissMs > 0)
        {
            var id = item.Id;
            _ = Task.Delay(autoDismissMs).ContinueWith(_ => Dismiss(id));
        }
    }
}
