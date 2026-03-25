using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;

namespace AbsenceApp.Core.ViewModels;

public class SubjectListViewModel
{
    private readonly ISubjectService _service;

    public SubjectListViewModel(ISubjectService service) => _service = service;

    public IEnumerable<SubjectDto> Subjects { get; private set; } = Enumerable.Empty<SubjectDto>();
    public bool    IsLoading    { get; private set; }
    public string? ErrorMessage { get; private set; }

    public async Task LoadAsync()
    {
        IsLoading = true; ErrorMessage = null;
        try   { Subjects = await _service.GetAllAsync(); }
        catch (Exception ex) { ErrorMessage = ex.Message; }
        finally { IsLoading = false; }
    }
}
