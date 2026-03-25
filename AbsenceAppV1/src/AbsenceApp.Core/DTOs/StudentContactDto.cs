namespace AbsenceApp.Core.DTOs;

public class StudentContactDto
{
    public long    Id                       { get; set; }
    public long    StudentId                { get; set; }
    public string  ContactName              { get; set; } = string.Empty;
    public string  Relationship             { get; set; } = string.Empty;
    public string? PhoneMobile              { get; set; }
    public string? PhoneHome                { get; set; }
    public string? Email                    { get; set; }
    public int     Priority                 { get; set; }
    public bool?   LivesWithStudent         { get; set; }
    public bool    HasParentalResponsibility { get; set; }
    public bool?   SafeguardingFlag         { get; set; }
    public string? Notes                    { get; set; }
    public DateTime CreatedAt               { get; set; }
    public DateTime UpdatedAt               { get; set; }
}
