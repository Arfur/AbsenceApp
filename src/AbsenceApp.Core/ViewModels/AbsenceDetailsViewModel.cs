/*
===============================================================================
 File        : AbsenceDetailsViewModel.cs
 Namespace   : AbsenceApp.Core.ViewModels
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-14
-------------------------------------------------------------------------------
 Purpose     : ViewModel for an individual Absence details / add-record page.
               Manages the form state for adding a new absence.
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
// AbsenceDetailsViewModel — backing state for absence add/details form
// ============================================================================
public class AbsenceDetailsViewModel
{
    // =========================================================================
    // Dependencies
    // =========================================================================
    private readonly IAbsenceService _absenceService;

    public AbsenceDetailsViewModel(IAbsenceService absenceService)
    {
        _absenceService = absenceService;
    }

    // =========================================================================
    // Bindable state
    // =========================================================================
    public AbsenceRecord CurrentRecord { get; set; } = new();
    public bool IsSaving { get; private set; }
    public bool IsSaved { get; private set; }
    public string? ErrorMessage { get; private set; }

    // =========================================================================
    // Commands
    // =========================================================================

    /// <summary>
    /// Saves the current absence record.  Sets IsSaved on success.
    /// </summary>
    public async Task SaveAsync()
    {
        IsSaving = true;
        IsSaved  = false;
        ErrorMessage = null;
        try
        {
            CurrentRecord.Id = Guid.NewGuid().ToString();
            await _absenceService.AddAbsenceAsync(CurrentRecord);
            IsSaved = true;
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsSaving = false;
        }
    }
}
