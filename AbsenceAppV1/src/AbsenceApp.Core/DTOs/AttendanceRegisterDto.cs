namespace AbsenceApp.Core.DTOs;

public class AttendanceRegisterDto
{
    public long     Id        { get; set; }
    public long     ClassId   { get; set; }
    public DateOnly Date      { get; set; }
    public string   Session   { get; set; } = string.Empty;
    public long     OpenedBy  { get; set; }
    public long?    ClosedBy  { get; set; }
    public string   Status    { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
