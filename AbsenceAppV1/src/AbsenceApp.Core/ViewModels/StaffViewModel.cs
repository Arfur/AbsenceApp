/*
===============================================================================
 File        : StaffViewModel.cs
 Namespace   : AbsenceApp.Core.ViewModels
 Author      : Michael
 Version     : 1.1.0
 Created     : 2026-03-14
 Updated     : 2026-03-18
-------------------------------------------------------------------------------
 Purpose     : ViewModel for the Staff list page. Loads all staff Full View
               rows via IStaffFullViewService and exposes them for data-binding.
-------------------------------------------------------------------------------
 Changes     :
    - 1.0.0  2026-03-14  Initial implementation.
    - 1.1.0  2026-03-18  Switched to StaffFullViewDto and
                                IStaffFullViewService for Full View UI binding.
===============================================================================
*/

using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;

namespace AbsenceApp.Core.ViewModels;

// ============================================================================
// StaffViewModel — backing state for StaffListPage.razor
// ============================================================================
public class StaffViewModel
{
    // =========================================================================
    // Dependencies
    // =========================================================================
    private readonly IStaffFullViewService _staffService;

    public StaffViewModel(IStaffFullViewService staffService)
    {
        _staffService = staffService;
    }

    // =========================================================================
    // Bindable state
    // =========================================================================
    public IEnumerable<StaffFullViewDto> Staff { get; private set; } = Enumerable.Empty<StaffFullViewDto>();
    public bool IsLoading { get; private set; }
    public string? ErrorMessage { get; private set; }

    // =========================================================================
    // Commands
    // =========================================================================

    /// <summary>Loads all staff records from the service.</summary>
    public async Task LoadAsync()
    {
        IsLoading = true;
        ErrorMessage = null;
        try
        {
            Staff = await _staffService.GetAllAsync();
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
