using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;

namespace AbsenceApp.Core.ViewModels;

public class SubjectAddViewModel
{
    private readonly ISubjectService _service;

    public SubjectAddViewModel(ISubjectService service) => _service = service;

    public SubjectDto NewSubject  { get; set; } = new();
    public bool    IsSaving     { get; private set; }
    public bool    IsSaved      { get; private set; }
    public string? ErrorMessage { get; private set; }

    public async Task SaveAsync()
    {
        IsSaving = true; IsSaved = false; ErrorMessage = null;
        try
        {
            NewSubject = await _service.CreateAsync(NewSubject);
            IsSaved = true;
        }
        catch (Exception ex) { ErrorMessage = ex.Message; }
        finally { IsSaving = false; }
    }
}
