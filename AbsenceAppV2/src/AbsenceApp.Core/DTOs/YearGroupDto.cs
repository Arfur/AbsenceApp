namespace AbsenceApp.Core.DTOs;

public class YearGroupDto
{
    public int      Id           { get; set; }
    public string   Name         { get; set; } = string.Empty;
    public string   Code         { get; set; } = string.Empty;
    public int?     NumericValue { get; set; }
    public int?     PhaseId      { get; set; }
    public string?  Description  { get; set; }
    public int      DisplayOrder { get; set; }
    public DateTime CreatedAt    { get; set; }
    public DateTime UpdatedAt    { get; set; }
}
