namespace AbsenceApp.Core.DTOs;

public class AttendanceMarkDto
{
    public long    Id                   { get; set; }
    public long    AttendanceRegisterId { get; set; }
    public long    StudentId            { get; set; }
    public string  MarkCode             { get; set; } = string.Empty;
    public bool?   IsLate               { get; set; }
    public int?    MinutesLate          { get; set; }
    public string? Notes                { get; set; }
    public long    RecordedBy           { get; set; }
    public DateTime CreatedAt           { get; set; }
    public DateTime UpdatedAt           { get; set; }
}
