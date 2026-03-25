using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;

namespace AbsenceApp.Core.ViewModels;

public class DashboardOverviewViewModel
{
    private readonly IDashboardService _service;

    public DashboardOverviewViewModel(IDashboardService service) => _service = service;

    public DashboardOverviewDto? Overview  { get; private set; }
    public bool    IsLoading    { get; private set; }
    public string? ErrorMessage { get; private set; }

    public async Task LoadAsync()
    {
        IsLoading = true; ErrorMessage = null;
        try   { Overview = await _service.GetOverviewAsync(); }
        catch (Exception ex) { ErrorMessage = ex.Message; }
        finally { IsLoading = false; }
    }
}
