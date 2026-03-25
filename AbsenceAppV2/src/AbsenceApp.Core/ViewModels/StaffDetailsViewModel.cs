using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;

namespace AbsenceApp.Core.ViewModels;

public class StaffDetailsViewModel
{
    private readonly IStaffService _service;

    public StaffDetailsViewModel(IStaffService service) => _service = service;

    public StaffDto? Staff      { get; private set; }
    public bool    IsLoading    { get; private set; }
    public string? ErrorMessage { get; private set; }

    public async Task LoadAsync(string id)
    {
        if (!long.TryParse(id, out var longId)) { ErrorMessage = "Invalid ID."; return; }
        IsLoading = true; ErrorMessage = null;
        try   { Staff = await _service.GetByIdAsync(longId); }
        catch (Exception ex) { ErrorMessage = ex.Message; }
        finally { IsLoading = false; }
    }
}
