namespace AbsenceApp.Core.DTOs;

public class JobTitleDto
{
    public long    Id          { get; set; }
    public string  Title       { get; set; } = string.Empty;
    public string  Code        { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt  { get; set; }
    public DateTime UpdatedAt  { get; set; }
}
