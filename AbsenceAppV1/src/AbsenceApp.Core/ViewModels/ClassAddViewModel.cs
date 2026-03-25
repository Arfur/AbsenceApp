using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;

namespace AbsenceApp.Core.ViewModels;

public class ClassAddViewModel
{
    private readonly IClassService _service;

    public ClassAddViewModel(IClassService service) => _service = service;

    public ClassDto NewClass    { get; set; } = new();
    public bool    IsSaving     { get; private set; }
    public bool    IsSaved      { get; private set; }
    public string? ErrorMessage { get; private set; }

    public async Task SaveAsync()
    {
        IsSaving = true; IsSaved = false; ErrorMessage = null;
        try
        {
            NewClass = await _service.CreateAsync(NewClass);
            IsSaved = true;
        }
        catch (Exception ex) { ErrorMessage = ex.Message; }
        finally { IsSaving = false; }
    }
}
