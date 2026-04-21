using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;

namespace AbsenceApp.Core.ViewModels;

public class AttendanceLogViewModel
{
    private readonly IAbsenceService _service;

    public AttendanceLogViewModel(IAbsenceService service) => _service = service;

    public IEnumerable<AbsenceDto> Absences { get; private set; } = Enumerable.Empty<AbsenceDto>();
    public bool    IsLoading    { get; private set; }
    public string? ErrorMessage { get; private set; }

    public async Task LoadAsync(long studentId)
    {
        IsLoading = true; ErrorMessage = null;
        try   { Absences = await _service.GetByPersonAsync("Student", studentId); }
        catch (Exception ex) { ErrorMessage = ex.Message; }
        finally { IsLoading = false; }
    }
}
