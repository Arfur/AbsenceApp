using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;

namespace AbsenceApp.Core.ViewModels;

public class StaffAddViewModel
{
    private readonly IStaffService _service;

    public StaffAddViewModel(IStaffService service) => _service = service;

    public StaffDto NewStaff    { get; set; } = new();
    public bool    IsSaving     { get; private set; }
    public bool    IsSaved      { get; private set; }
    public string? ErrorMessage { get; private set; }

    public async Task SaveAsync()
    {
        IsSaving = true; IsSaved = false; ErrorMessage = null;
        try   { await Task.CompletedTask; IsSaved = true; }
        catch (Exception ex) { ErrorMessage = ex.Message; }
        finally { IsSaving = false; }
    }
}
