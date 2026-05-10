namespace AbsenceApp.Data.Models;

public class StaffNote
{
    public long     Id        { get; set; }
    public int      StaffId   { get; set; }
    public string   NoteType  { get; set; } = "General";
    public string   Body      { get; set; } = string.Empty;
    public int      CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
