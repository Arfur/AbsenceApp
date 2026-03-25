using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;

namespace AbsenceApp.Core.ViewModels;

public class DashboardStudentActivityViewModel
{
    private readonly IDashboardService _service;

    public DashboardStudentActivityViewModel(IDashboardService service) => _service = service;

    public IEnumerable<DashboardStudentActivityDto> Activity { get; private set; } = Enumerable.Empty<DashboardStudentActivityDto>();
    public bool    IsLoading    { get; private set; }
    public string? ErrorMessage { get; private set; }

    public async Task LoadAsync(int topN = 10)
    {
        IsLoading = true; ErrorMessage = null;
        try   { Activity = await _service.GetStudentActivityAsync(topN); }
        catch (Exception ex) { ErrorMessage = ex.Message; }
        finally { IsLoading = false; }
    }
}
