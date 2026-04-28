namespace AbsenceApp.Core.DTOs;

public class HouseDto
{
    public int      Id          { get; set; }
    public string   Name        { get; set; } = string.Empty;
    public string   Code        { get; set; } = string.Empty;
    public string?  Description { get; set; }
    public DateTime CreatedAt   { get; set; }
    public DateTime UpdatedAt   { get; set; }
}
