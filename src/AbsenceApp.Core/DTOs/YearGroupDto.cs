namespace AbsenceApp.Core.DTOs;

public class YearGroupDto
{
    public long     Id           { get; set; }
    public string   Name         { get; set; } = string.Empty;
    public string   Code         { get; set; } = string.Empty;
    public int      NumericValue { get; set; }
    public long     PhaseId      { get; set; }
    public long     SchoolId     { get; set; }
    public DateTime CreatedAt    { get; set; }
    public DateTime UpdatedAt    { get; set; }
}
