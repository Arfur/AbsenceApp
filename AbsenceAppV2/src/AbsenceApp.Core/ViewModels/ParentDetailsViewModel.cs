using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;

namespace AbsenceApp.Core.ViewModels;

public class ParentDetailsViewModel
{
    private readonly IStudentContactService _service;

    public ParentDetailsViewModel(IStudentContactService service) => _service = service;

    public StudentContactDto? Contact { get; private set; }
    public bool    IsLoading    { get; private set; }
    public string? ErrorMessage { get; private set; }

    public async Task LoadAsync(long id)
    {
        IsLoading = true; ErrorMessage = null;
        try   { Contact = await _service.GetByIdAsync(id); }
        catch (Exception ex) { ErrorMessage = ex.Message; }
        finally { IsLoading = false; }
    }
}
