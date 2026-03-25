namespace AbsenceApp.Core.DTOs;

public class StudentAbsenceDto
{
    public long       Id            { get; set; }
    public long       StudentId     { get; set; }
    public long       AbsenceTypeId { get; set; }
    public DateOnly   Date          { get; set; }
    public TimeOnly?  StartTime     { get; set; }
    public TimeOnly?  EndTime       { get; set; }
    public bool       IsAuthorised  { get; set; }
    public string?    Notes         { get; set; }
    public long       RecordedBy    { get; set; }
    public DateTime   CreatedAt     { get; set; }
    public DateTime   UpdatedAt     { get; set; }
}
