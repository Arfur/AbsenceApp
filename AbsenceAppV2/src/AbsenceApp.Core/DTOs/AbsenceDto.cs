namespace AbsenceApp.Core.DTOs;

public class AbsenceDto
{
    public long      Id            { get; set; }
    public string    PersonType    { get; set; } = string.Empty;
    public long      PersonId      { get; set; }
    public long      AbsenceTypeId { get; set; }
    public string    AbsenceTypeName { get; set; } = string.Empty;
    public long      StatusId      { get; set; }
    public string    StatusName    { get; set; } = string.Empty;
    public DateOnly  StartDate     { get; set; }
    public DateOnly  EndDate       { get; set; }
    public int       DurationDays  { get; set; }
    public string    ReportedVia   { get; set; } = "Manual";
    public string?   Notes         { get; set; }
    public long?     RecordedBy    { get; set; }
    public long?     ApprovedBy    { get; set; }
    public DateTime? ApprovedAt    { get; set; }
    public DateTime  CreatedAt     { get; set; }
    public DateTime  UpdatedAt     { get; set; }
}

public class CreateAbsenceDto
{
    public string   PersonType    { get; set; } = string.Empty;
    public long     PersonId      { get; set; }
    public long     AbsenceTypeId { get; set; }
    public DateOnly StartDate     { get; set; }
    public DateOnly EndDate       { get; set; }
    public string   ReportedVia   { get; set; } = "Manual";
    public string?  Notes         { get; set; }
    public long?    RecordedBy    { get; set; }
}

public class UpdateAbsenceStatusDto
{
    public long    NewStatusId { get; set; }
    public long    ChangedBy   { get; set; }
    public string? Notes       { get; set; }
}
