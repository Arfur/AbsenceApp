/*
===============================================================================
 File        : NotificationService.cs
 Namespace   : AbsenceApp.Data.Services
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-04-05
 Updated     : 2026-04-05
-------------------------------------------------------------------------------
 Purpose     : EF-backed read-only service that retrieves unread in-app
               notifications for a given user from the AppNotifications table.
               Phase 3 Header Nav Identity.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-05  Initial implementation.
-------------------------------------------------------------------------------
 Notes       :
   - Service is Scoped (matches EF Core DbContext lifetime).
   - Returns newest-first ordering so the header dropdown shows the most
     recent notification at the top.
===============================================================================
*/

using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;
using AbsenceApp.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace AbsenceApp.Data.Services;

public class NotificationService : INotificationService
{
    private readonly AppDbContext _db;

    public NotificationService(AppDbContext db) => _db = db;

    /// <inheritdoc/>
    public async Task<List<AppNotificationDto>> GetUnreadNotificationsAsync(long userId)
    {
        return await _db.AppNotifications
            .AsNoTracking()
            .Where(n => n.UserId == userId && !n.IsRead)
            .OrderByDescending(n => n.CreatedAt)
            .Select(n => new AppNotificationDto
            {
                Id        = n.Id,
                Title     = n.Title,
                Body      = n.Body,
                CreatedAt = n.CreatedAt,
            })
            .ToListAsync();
    }
}
