/*
===============================================================================
 File        : StudentsViewModel.cs
 Namespace   : AbsenceApp.Core.ViewModels
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-14
-------------------------------------------------------------------------------
 Purpose     : ViewModel for the Students list page. Loads all students via
               IStudentService and exposes them for data-binding.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial implementation in AbsenceApp.Client.
   - 1.1.0  2026-03-14  Moved to AbsenceApp.Core for testability.
===============================================================================
*/

using AbsenceApp.Core.Interfaces;
using AbsenceApp.Core.Models;

namespace AbsenceApp.Core.ViewModels;

// ============================================================================
// StudentsViewModel — backing state for StudentsPage.razor
// ============================================================================
public class StudentsViewModel
{
    // =========================================================================
    // Dependencies
    // =========================================================================
    private readonly IStudentService _studentService;

    public StudentsViewModel(IStudentService studentService)
    {
        _studentService = studentService;
    }

    // =========================================================================
    // Bindable state
    // =========================================================================
    public IEnumerable<Student> Students { get; private set; } = Enumerable.Empty<Student>();
    public bool IsLoading { get; private set; }
    public string? ErrorMessage { get; private set; }

    // =========================================================================
    // Commands
    // =========================================================================

    /// <summary>Loads all students from the service.</summary>
    public async Task LoadAsync()
    {
        IsLoading = true;
        ErrorMessage = null;
        try
        {
            Students = await _studentService.GetAllStudentsAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsLoading = false;
        }
    }
}
