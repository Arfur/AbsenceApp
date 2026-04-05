namespace AbsenceApp.Core.DTOs;

/// <summary>
/// Read-only projection of an unread Message for display in the header
/// messages dropdown.  Phase 3 Header Nav Identity.
/// </summary>
public class MessageDto
{
    public long     Id         { get; init; }
    public string   SenderName { get; init; } = string.Empty;
    public string   Subject    { get; init; } = string.Empty;
    public string   Preview    { get; init; } = string.Empty;
    public DateTime CreatedAt  { get; init; }
}
