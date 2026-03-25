using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;

namespace AbsenceApp.Core.ViewModels;

public class DashboardSafeguardingViewModel
{
    private readonly IDashboardService _service;

    public DashboardSafeguardingViewModel(IDashboardService service) => _service = service;

    public IEnumerable<DashboardSafeguardingDto> Alerts { get; private set; } = Enumerable.Empty<DashboardSafeguardingDto>();
    public bool    IsLoading    { get; private set; }
    public string? ErrorMessage { get; private set; }

    public async Task LoadAsync()
    {
        IsLoading = true; ErrorMessage = null;
        try   { Alerts = await _service.GetSafeguardingAsync(); }
        catch (Exception ex) { ErrorMessage = ex.Message; }
        finally { IsLoading = false; }
    }
}
