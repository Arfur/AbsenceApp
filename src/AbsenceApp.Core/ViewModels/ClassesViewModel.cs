/*
===============================================================================
 File        : ClassesViewModel.cs
 Namespace   : AbsenceApp.Core.ViewModels
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-14
-------------------------------------------------------------------------------
 Purpose     : ViewModel for the Class list page. Loads all classes via
               IClassService and exposes them for data-binding.
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
// ClassesViewModel — backing state for ClassListPage.razor
// ============================================================================
public class ClassesViewModel
{
    // =========================================================================
    // Dependencies
    // =========================================================================
    private readonly IClassService _classService;

    public ClassesViewModel(IClassService classService)
    {
        _classService = classService;
    }

    // =========================================================================
    // Bindable state
    // =========================================================================
    public IEnumerable<ClassDto> Classes { get; private set; } = Enumerable.Empty<ClassDto>();
    public bool IsLoading { get; private set; }
    public string? ErrorMessage { get; private set; }

    // =========================================================================
    // Commands
    // =========================================================================

    /// <summary>Loads all classes from the service.</summary>
    public async Task LoadAsync()
    {
        IsLoading = true;
        ErrorMessage = null;
        try
        {
            Classes = await _classService.GetAllAsync();
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
