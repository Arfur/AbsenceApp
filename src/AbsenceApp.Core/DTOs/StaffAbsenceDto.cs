namespace AbsenceApp.Core.DTOs;

public class StaffAbsenceDto
{
    public long     Id            { get; set; }
    public long     StaffId       { get; set; }
    public long     AbsenceTypeId { get; set; }
    public DateOnly StartDate     { get; set; }
    public DateOnly? EndDate      { get; set; }
    public string?  Notes         { get; set; }
    public DateTime CreatedAt     { get; set; }
    public DateTime UpdatedAt     { get; set; }
}
