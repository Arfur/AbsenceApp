namespace AbsenceApp.Core.DTOs;

public class AbsenceTypeDto
{
    public long     Id           { get; set; }
    public string   Code         { get; set; } = string.Empty;
    public string   Name         { get; set; } = string.Empty;
    public string   Category     { get; set; } = string.Empty;
    public bool     IsAuthorised { get; set; } = true;
    public DateTime CreatedAt    { get; set; }
}
