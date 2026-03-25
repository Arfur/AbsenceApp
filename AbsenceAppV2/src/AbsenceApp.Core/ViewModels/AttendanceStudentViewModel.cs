using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;

namespace AbsenceApp.Core.ViewModels;

public class AttendanceStudentViewModel
{
    private readonly IAttendanceRegisterService _service;

    public AttendanceStudentViewModel(IAttendanceRegisterService service) => _service = service;

    public IEnumerable<AttendanceMarkDto> Marks { get; private set; } = Enumerable.Empty<AttendanceMarkDto>();
    public bool    IsLoading    { get; private set; }
    public string? ErrorMessage { get; private set; }

    public async Task LoadAsync(long registerId)
    {
        IsLoading = true; ErrorMessage = null;
        try   { Marks = await _service.GetMarksAsync(registerId); }
        catch (Exception ex) { ErrorMessage = ex.Message; }
        finally { IsLoading = false; }
    }
}
