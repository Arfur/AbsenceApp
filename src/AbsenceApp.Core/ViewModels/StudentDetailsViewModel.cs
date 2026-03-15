/*
===============================================================================
 File        : StudentDetailsViewModel.cs
 Namespace   : AbsenceApp.Core.ViewModels
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-14
-------------------------------------------------------------------------------
 Purpose     : ViewModel for the Student Details page. Loads a single student
               by ID via IStudentService.
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
// StudentDetailsViewModel — backing state for StudentDetailsPage.razor
// ============================================================================
public class StudentDetailsViewModel
{
    // =========================================================================
    // Dependencies
    // =========================================================================
    private readonly IStudentService _studentService;

    public StudentDetailsViewModel(IStudentService studentService)
    {
        _studentService = studentService;
    }

    // =========================================================================
    // Bindable state
    // =========================================================================
    public Student? Student { get; private set; }
    public bool IsLoading { get; private set; }
    public string? ErrorMessage { get; private set; }

    // =========================================================================
    // Commands
    // =========================================================================

    /// <summary>Loads the student for the given string ID.</summary>
    public async Task LoadAsync(string id)
    {
        IsLoading = true;
        ErrorMessage = null;
        try
        {
            Student = await _studentService.GetStudentByIdAsync(id);
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
