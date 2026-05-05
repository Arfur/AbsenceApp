/*
===============================================================================
 File        : StudentCalendarViewModelV2.cs
 Namespace   : AbsenceApp.Client.ViewModels.V2
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-05-05
 Updated     : 2026-05-05
-------------------------------------------------------------------------------
 Purpose     : ViewModel for StudentCalendarPageV2.
               Provides a monthly calendar grid showing A (Absent) / P (Present)
               marks for each day, derived from the student's absence date ranges.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-05-05  Initial creation.
-------------------------------------------------------------------------------
 Notes       :
   - Register as Scoped in V2ServiceCollectionExtensions.cs.
   - Calendar grid logic: a day is "Absent" if it falls within any absence's
     [StartDate, EndDate] range (inclusive). Weekends/holidays are not filtered.
   - Navigation: PreviousMonth / NextMonth cycle through months.
===============================================================================
*/

using AbsenceApp.Client.Services.ApiV2.Modules;
using AbsenceApp.Core.DTOs;

namespace AbsenceApp.Client.ViewModels.V2;

public sealed class StudentCalendarViewModelV2
{
    private readonly StudentProfileApiServiceV2 _api;

    public StudentCalendarViewModelV2(StudentProfileApiServiceV2 api) => _api = api;

    // =========================================================================
    // Identity
    // =========================================================================

    public int StudentId { get; private set; }

    // =========================================================================
    // Student header
    // =========================================================================

    public StudentFullViewDto? Student { get; private set; }

    // =========================================================================
    // All absence records
    // =========================================================================

    public IReadOnlyList<AbsenceDto> Absences { get; private set; } = [];

    // =========================================================================
    // Calendar navigation
    // =========================================================================

    public int Year  { get; private set; } = DateTime.Today.Year;
    public int Month { get; private set; } = DateTime.Today.Month;

    public string MonthLabel => new DateTime(Year, Month, 1).ToString("MMMM yyyy");

    /// <summary>
    /// Returns an ordered list of (Date, isAbsent) pairs for every day in the
    /// current Month/Year.  Days that fall within at least one absence's
    /// [StartDate, EndDate] range are marked absent.
    /// </summary>
    public IReadOnlyList<(DateOnly Date, bool IsAbsent)> CalendarDays
    {
        get
        {
            var days  = DateTime.DaysInMonth(Year, Month);
            var result = new List<(DateOnly, bool)>(days);
            for (var d = 1; d <= days; d++)
            {
                var date    = new DateOnly(Year, Month, d);
                var absent  = Absences.Any(a => date >= a.StartDate && date <= a.EndDate);
                result.Add((date, absent));
            }
            return result.AsReadOnly();
        }
    }

    // =========================================================================
    // State
    // =========================================================================

    public bool    IsLoading { get; private set; }
    public string? Error     { get; private set; }

    // =========================================================================
    // Initialisation
    // =========================================================================

    public async Task InitAsync(int studentId, CancellationToken ct = default)
    {
        StudentId = studentId;
        IsLoading = true;
        Error     = null;

        try
        {
            var studentTask  = _api.GetStudentAsync(studentId, ct);
            var absencesTask = _api.GetAbsencesAsync(studentId, ct);
            await Task.WhenAll(studentTask, absencesTask);

            Student  = await studentTask;
            Absences = (await absencesTask).ToList().AsReadOnly();

            if (Student is null)
                Error = "Student not found.";
        }
        catch (Exception ex)
        {
            Error = $"Failed to load calendar data: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    // =========================================================================
    // Month navigation
    // =========================================================================

    public void PreviousMonth()
    {
        var dt = new DateTime(Year, Month, 1).AddMonths(-1);
        Year  = dt.Year;
        Month = dt.Month;
    }

    public void NextMonth()
    {
        var dt = new DateTime(Year, Month, 1).AddMonths(1);
        Year  = dt.Year;
        Month = dt.Month;
    }

    public void GoToToday()
    {
        Year  = DateTime.Today.Year;
        Month = DateTime.Today.Month;
    }
}
