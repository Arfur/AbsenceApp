namespace AbsenceApp.Data.Models;

public class UserDevice
{
    public int       Id           { get; set; }
    public int       UserId       { get; set; }
    public string    DeviceType   { get; set; } = string.Empty;
    public string?   SerialNumber { get; set; }
    public DateOnly  AssignedDate { get; set; }
    public DateOnly? ReturnedDate { get; set; }
    public string?   Notes        { get; set; }
    public DateTime  CreatedAt    { get; set; }
    public DateTime  UpdatedAt    { get; set; }
}
