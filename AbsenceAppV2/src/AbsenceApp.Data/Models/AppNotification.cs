/*
===============================================================================
 File        : AppNotification.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-04-05
 Updated     : 2026-04-05
-------------------------------------------------------------------------------
 Purpose     : EF Core entity for the app_notifications table.
               Represents a system notification delivered to a user.
               Named AppNotification to avoid conflict with system.Notification.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-05  Phase 3 Header Nav Identity: initial creation for
                         notification icon dropdown and unread badge count.
-------------------------------------------------------------------------------
 Notes       :
   - UserId is a FK to Users.Id (no navigation property; FK enforced at DB).
===============================================================================
*/
namespace AbsenceApp.Data.Models;

public class AppNotification
{
    public long     Id        { get; set; }
    public long     UserId    { get; set; }
    public string   Title     { get; set; } = default!;
    public string   Body      { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public bool     IsRead    { get; set; }
}
