using AbsenceApp.Core.DTOs;

namespace AbsenceApp.Core.Interfaces;

/// <summary>
/// Read-only service for retrieving in-app messages.
/// Phase 3 Header Nav Identity.
/// </summary>
public interface IMessageService
{
    /// <summary>Returns all unread messages for the given user, newest first.</summary>
    Task<List<MessageDto>> GetUnreadMessagesAsync(long userId);
}
