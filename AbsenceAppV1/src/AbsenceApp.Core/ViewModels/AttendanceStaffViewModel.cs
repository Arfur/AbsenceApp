using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;

namespace AbsenceApp.Core.ViewModels;

public class AttendanceStaffViewModel
{
    private readonly IAttendanceRegisterService _service;

    public AttendanceStaffViewModel(IAttendanceRegisterService service) => _service = service;

    public IEnumerable<AttendanceRegisterDto> Registers { get; private set; } = Enumerable.Empty<AttendanceRegisterDto>();
    public bool    IsLoading    { get; private set; }
    public string? ErrorMessage { get; private set; }

    public async Task LoadAsync(long classId)
    {
        IsLoading = true; ErrorMessage = null;
        try   { Registers = await _service.GetByClassAsync(classId); }
        catch (Exception ex) { ErrorMessage = ex.Message; }
        finally { IsLoading = false; }
    }
}
