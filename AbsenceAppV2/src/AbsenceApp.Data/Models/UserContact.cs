namespace AbsenceApp.Data.Models;

public class UserContact
{
    public int      Id           { get; set; }
    public int      UserId       { get; set; }
    public string   ContactName  { get; set; } = string.Empty;
    public string   Relationship { get; set; } = "Other";
    public string?  Phone        { get; set; }
    public string?  Email        { get; set; }
    public bool     IsPrimary    { get; set; }
    public DateTime CreatedAt    { get; set; }
    public DateTime UpdatedAt    { get; set; }
}
