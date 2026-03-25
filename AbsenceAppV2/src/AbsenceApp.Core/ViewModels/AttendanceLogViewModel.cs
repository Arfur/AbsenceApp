using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;

namespace AbsenceApp.Core.ViewModels;

public class AttendanceLogViewModel
{
    private readonly IStudentAbsenceService _service;

    public AttendanceLogViewModel(IStudentAbsenceService service) => _service = service;

    public IEnumerable<StudentAbsenceDto> Absences { get; private set; } = Enumerable.Empty<StudentAbsenceDto>();
    public bool    IsLoading    { get; private set; }
    public string? ErrorMessage { get; private set; }

    public async Task LoadAsync(long studentId)
    {
        IsLoading = true; ErrorMessage = null;
        try   { Absences = await _service.GetByStudentAsync(studentId); }
        catch (Exception ex) { ErrorMessage = ex.Message; }
        finally { IsLoading = false; }
    }
}
