namespace AbsenceApp.Data.Models;

public class UserExternalAccount
{
    public int      Id              { get; set; }
    public int      UserId          { get; set; }
    public int      SystemId        { get; set; }
    public string   SystemName      { get; set; } = string.Empty;
    public string   SystemCode      { get; set; } = string.Empty;
    public string?  AccountUsername { get; set; }
    public string?  AccountEmail    { get; set; }
    public string   Status          { get; set; } = "active";
    public DateTime CreatedAt       { get; set; }
    public DateTime UpdatedAt       { get; set; }
}
