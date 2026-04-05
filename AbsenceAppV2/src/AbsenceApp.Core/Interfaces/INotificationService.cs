using AbsenceApp.Core.DTOs;

namespace AbsenceApp.Core.Interfaces;

/// <summary>
/// Read-only service for retrieving in-app notifications.
/// Phase 3 Header Nav Identity.
/// </summary>
public interface INotificationService
{
    /// <summary>Returns all unread notifications for the given user, newest first.</summary>
    Task<List<AppNotificationDto>> GetUnreadNotificationsAsync(long userId);
}
