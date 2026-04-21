namespace AbsenceApp.Core.DTOs;

public class AbsenceStatusDto
{
    public long     Id        { get; set; }
    public string   Code      { get; set; } = string.Empty;
    public string   Name      { get; set; } = string.Empty;
    public bool     IsFinal   { get; set; }
    public DateTime CreatedAt { get; set; }
}
