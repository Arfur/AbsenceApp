namespace AbsenceApp.Core.DTOs;

public class DashboardOverviewDto
{
    public int TotalStudents       { get; set; }
    public int TotalStaff          { get; set; }
    public int AbsencesToday       { get; set; }
    public int UnauthorisedToday   { get; set; }
    public int RegistersOpen       { get; set; }
}

public class DashboardStudentActivityDto
{
    public long   StudentId   { get; set; }
    public string FullName    { get; set; } = string.Empty;
    public int    AbsenceCount { get; set; }
    public DateOnly LastAbsence { get; set; }
}

public class DashboardSafeguardingDto
{
    public long   StudentId        { get; set; }
    public string FullName         { get; set; } = string.Empty;
    public bool   SafeguardingFlag { get; set; }
    public string? Notes           { get; set; }
}
