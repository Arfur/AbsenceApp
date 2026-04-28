namespace AbsenceApp.Core.DTOs;

public class JobTitleDto
{
    public int     Id          { get; set; }
    public string? Title       { get; set; }
    public string? Code        { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt  { get; set; }
    public DateTime UpdatedAt  { get; set; }
}
