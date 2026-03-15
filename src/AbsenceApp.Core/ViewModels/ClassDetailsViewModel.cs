/*
===============================================================================
 File        : ClassDetailsViewModel.cs
 Namespace   : AbsenceApp.Core.ViewModels
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-14
-------------------------------------------------------------------------------
 Purpose     : ViewModel for the Class Details page. Loads a single class by
               integer ID via IClassService.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial implementation in AbsenceApp.Client.
   - 1.1.0  2026-03-14  Moved to AbsenceApp.Core for testability.
===============================================================================
*/

using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;

namespace AbsenceApp.Core.ViewModels;

// ============================================================================
// ClassDetailsViewModel — backing state for ClassDetailsPage.razor
// ============================================================================
public class ClassDetailsViewModel
{
    // =========================================================================
    // Dependencies
    // =========================================================================
    private readonly IClassService _classService;

    public ClassDetailsViewModel(IClassService classService)
    {
        _classService = classService;
    }

    // =========================================================================
    // Bindable state
    // =========================================================================
    public ClassDto? Class { get; private set; }
    public bool IsLoading { get; private set; }
    public string? ErrorMessage { get; private set; }

    // =========================================================================
    // Commands
    // =========================================================================

    /// <summary>
    /// Loads the class for the given route-parameter ID string.
    /// Handles invalid/non-integer IDs gracefully.
    /// </summary>
    public async Task LoadAsync(string id)
    {
        if (!int.TryParse(id, out var classId))
        {
            ErrorMessage = $"Invalid class ID: '{id}'.";
            return;
        }

        IsLoading = true;
        ErrorMessage = null;
        try
        {
            Class = await _classService.GetByIdAsync(classId);
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
