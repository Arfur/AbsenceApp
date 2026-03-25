using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;

namespace AbsenceApp.Core.ViewModels;

public class StudentAddViewModel
{
    private readonly IStudentService _service;

    public StudentAddViewModel(IStudentService service) => _service = service;

    public StudentDto NewStudent { get; set; } = new();
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
