namespace AbsenceApp.Core.DTOs;

public class DepartmentDto
{
    public int     Id          { get; set; }
    public string  Name        { get; set; } = string.Empty;
    public string  Code        { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int?    HeadUserId  { get; set; }
    public string  Status      { get; set; } = string.Empty;
    public DateTime CreatedAt  { get; set; }
    public DateTime UpdatedAt  { get; set; }
}
