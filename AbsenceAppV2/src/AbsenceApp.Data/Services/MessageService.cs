/*
===============================================================================
 File        : MessageService.cs
 Namespace   : AbsenceApp.Data.Services
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-04-05
 Updated     : 2026-04-05
-------------------------------------------------------------------------------
 Purpose     : EF-backed read-only service that retrieves unread in-app
               messages for a given user from the Messages table.
               Phase 3 Header Nav Identity.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-05  Initial implementation.
-------------------------------------------------------------------------------
 Notes       :
   - Service is Scoped (matches EF Core DbContext lifetime).
   - Returns newest-first ordering so the header dropdown shows the most
     recent message at the top.
===============================================================================
*/

using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;
using AbsenceApp.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace AbsenceApp.Data.Services;

public class MessageService : IMessageService
{
    private readonly AppDbContext _db;

    public MessageService(AppDbContext db) => _db = db;

    /// <inheritdoc/>
    public async Task<List<MessageDto>> GetUnreadMessagesAsync(long userId)
    {
        return await _db.Messages
            .AsNoTracking()
            .Where(m => m.UserId == userId && !m.IsRead)
            .OrderByDescending(m => m.CreatedAt)
            .Select(m => new MessageDto
            {
                Id         = m.Id,
                SenderName = m.SenderName,
                Subject    = m.Subject,
                Preview    = m.Preview,
                CreatedAt  = m.CreatedAt,
            })
            .ToListAsync();
    }
}
