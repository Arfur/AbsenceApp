namespace AbsenceApp.Core.DTOs;

public sealed class StudentNoteDto
{
    public long     Id        { get; set; }
    public int      StudentId { get; set; }
    public string   NoteType  { get; set; } = "General";
    public string   Body      { get; set; } = string.Empty;
    public int      CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public sealed class StaffNoteDto
{
    public long     Id        { get; set; }
    public int      StaffId   { get; set; }
    public string   NoteType  { get; set; } = "General";
    public string   Body      { get; set; } = string.Empty;
    public int      CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public sealed class UserNoteDto
{
    public long     Id        { get; set; }
    public int      UserId    { get; set; }
    public string   NoteType  { get; set; } = "General";
    public string   Body      { get; set; } = string.Empty;
    public int      CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public sealed class UserContactDto
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

public sealed class UserDeviceDto
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

public sealed class UserExternalAccountDto
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

public sealed class UserPermissionOverrideDto
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
