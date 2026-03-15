/*
===============================================================================
 File        : AbsencesViewModel.cs
 Namespace   : AbsenceApp.Core.ViewModels
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-14
-------------------------------------------------------------------------------
 Purpose     : ViewModel for the Absences list page. Loads all absence records
               for a given student ID via IAbsenceService.
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
// AbsencesViewModel — backing state for AbsencesPage.razor
// ============================================================================
public class AbsencesViewModel
{
    // =========================================================================
    // Dependencies
    // =========================================================================
    private readonly IAbsenceService _absenceService;

    public AbsencesViewModel(IAbsenceService absenceService)
    {
        _absenceService = absenceService;
    }

    // =========================================================================
    // Bindable state
    // =========================================================================
    public IEnumerable<AbsenceRecord> Absences { get; private set; } = Enumerable.Empty<AbsenceRecord>();
    public bool IsLoading { get; private set; }
    public string? ErrorMessage { get; private set; }

    // =========================================================================
    // Commands
    // =========================================================================

    /// <summary>Loads all absence records for the specified student.</summary>
    public async Task LoadAsync(string studentId)
    {
        IsLoading = true;
        ErrorMessage = null;
        try
        {
            Absences = await _absenceService.GetAbsencesForStudentAsync(studentId);
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

    /// <summary>Adds a new absence record and refreshes the list.</summary>
    public async Task AddAbsenceAsync(AbsenceRecord record)
    {
        await _absenceService.AddAbsenceAsync(record);
        await LoadAsync(record.StudentId);
    }
}
