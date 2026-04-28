namespace AbsenceApp.Core.DTOs;

public class StudentContactDto
{
    public int     Id           { get; set; }
    public int     StudentId    { get; set; }
    public string  ContactName  { get; set; } = string.Empty;
    public string  Relationship { get; set; } = string.Empty;
    public string? Phone        { get; set; }
    public string? Email        { get; set; }
    public bool    IsPrimary    { get; set; }
    public DateTime CreatedAt   { get; set; }
    public DateTime UpdatedAt   { get; set; }
}
