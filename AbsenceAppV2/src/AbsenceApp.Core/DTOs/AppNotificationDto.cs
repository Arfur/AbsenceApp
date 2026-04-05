namespace AbsenceApp.Core.DTOs;

/// <summary>
/// Read-only projection of an unread AppNotification for display in the header
/// notifications dropdown.  Phase 3 Header Nav Identity.
/// </summary>
public class AppNotificationDto
{
    public long     Id        { get; init; }
    public string   Title     { get; init; } = string.Empty;
    public string   Body      { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}
