namespace AbsenceApp.Data.Models;

public class UserPermissionOverride
{
    public int      Id        { get; set; }
    public int      UserId    { get; set; }
    public int      PageId    { get; set; }
    public bool     CanRead   { get; set; }
    public bool     CanWrite  { get; set; }
    public bool     CanCreate { get; set; }
    public bool     CanDelete { get; set; }
    public bool     CanImport { get; set; }
    public bool     CanExport { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
