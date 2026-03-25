using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;

namespace AbsenceApp.Core.ViewModels;

public class ParentListViewModel
{
    private readonly IStudentContactService _service;

    public ParentListViewModel(IStudentContactService service) => _service = service;

    public IEnumerable<StudentContactDto> Contacts { get; private set; } = Enumerable.Empty<StudentContactDto>();
    public bool    IsLoading    { get; private set; }
    public string? ErrorMessage { get; private set; }

    public async Task LoadAsync(long studentId)
    {
        IsLoading = true; ErrorMessage = null;
        try   { Contacts = await _service.GetByStudentAsync(studentId); }
        catch (Exception ex) { ErrorMessage = ex.Message; }
        finally { IsLoading = false; }
    }
}
