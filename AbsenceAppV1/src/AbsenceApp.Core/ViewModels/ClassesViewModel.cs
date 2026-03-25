/*
===============================================================================
 File        : ClassesViewModel.cs
 Namespace   : AbsenceApp.Core.ViewModels
 Author      : Michael
 Version     : 1.2.0
 Created     : 2026-03-13
 Updated     : 2026-03-18
-------------------------------------------------------------------------------
 Purpose     : ViewModel for the Class list page. Loads all classes via
               IClassFullViewService and exposes ClassFullViewDto rows for
               data-binding in the Full View table.
-------------------------------------------------------------------------------
 Changes     :
    - 1.0.0  2026-03-13  Initial implementation in AbsenceApp.Client.
    - 1.1.0  2026-03-14  Moved to AbsenceApp.Core for testability.
    - 1.2.0  2026-03-18  Switched to ClassFullViewDto and
                                IClassFullViewService for Full View UI binding.
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
    private readonly IClassFullViewService _classService;

    public ClassesViewModel(IClassFullViewService classService)
    {
        _classService = classService;
    }

    // =========================================================================
    // Bindable state
    // =========================================================================
    public IEnumerable<ClassFullViewDto> Classes { get; private set; } = Enumerable.Empty<ClassFullViewDto>();
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
