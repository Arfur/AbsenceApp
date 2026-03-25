namespace AbsenceApp.Core.DTOs;

public class HouseDto
{
    public long     Id        { get; set; }
    public string   Name      { get; set; } = string.Empty;
    public string   Colour    { get; set; } = string.Empty;
    public string   Code      { get; set; } = string.Empty;
    public long     SchoolId  { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
