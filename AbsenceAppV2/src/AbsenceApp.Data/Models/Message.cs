/*
===============================================================================
 File        : Message.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-04-05
 Updated     : 2026-04-05
-------------------------------------------------------------------------------
 Purpose     : EF Core entity for the messages table.
               Represents an in-app message sent to a user.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-05  Phase 3 Header Nav Identity: initial creation for
                         message icon dropdown and unread badge count.
-------------------------------------------------------------------------------
 Notes       :
   - UserId is a FK to Users.Id (no navigation property; FK enforced at DB).
   - SenderName is stored denormalised for display without join.
===============================================================================
*/
namespace AbsenceApp.Data.Models;

public class Message
{
    public long     Id         { get; set; }
    public long     UserId     { get; set; }
    public string   SenderName { get; set; } = default!;
    public string   Subject    { get; set; } = default!;
    public string   Preview    { get; set; } = default!;
    public DateTime CreatedAt  { get; set; }
    public bool     IsRead     { get; set; }
}
