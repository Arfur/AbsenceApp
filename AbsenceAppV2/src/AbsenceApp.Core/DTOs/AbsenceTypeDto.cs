namespace AbsenceApp.Core.DTOs;

public class AbsenceTypeDto
{
    public long    Id          { get; set; }
    public string  Name        { get; set; } = string.Empty;
    public string  Code        { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool    IsPaid      { get; set; }
}
