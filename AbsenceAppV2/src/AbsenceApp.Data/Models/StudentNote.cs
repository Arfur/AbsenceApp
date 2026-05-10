namespace AbsenceApp.Data.Models;

public class StudentNote
{
    public long     Id        { get; set; }
    public int      StudentId { get; set; }
    public string   NoteType  { get; set; } = "General";
    public string   Body      { get; set; } = string.Empty;
    public int      CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
