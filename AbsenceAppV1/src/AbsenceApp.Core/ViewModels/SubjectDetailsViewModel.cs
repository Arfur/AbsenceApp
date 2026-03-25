using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;

namespace AbsenceApp.Core.ViewModels;

public class SubjectDetailsViewModel
{
    private readonly ISubjectService _service;

    public SubjectDetailsViewModel(ISubjectService service) => _service = service;

    public SubjectDto? Subject     { get; private set; }
    public bool    IsLoading    { get; private set; }
    public string? ErrorMessage { get; private set; }

    public async Task LoadAsync(int id)
    {
        IsLoading = true; ErrorMessage = null;
        try   { Subject = await _service.GetByIdAsync(id); }
        catch (Exception ex) { ErrorMessage = ex.Message; }
        finally { IsLoading = false; }
    }
}
