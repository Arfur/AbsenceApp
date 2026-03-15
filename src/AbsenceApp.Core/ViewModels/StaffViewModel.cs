/*
===============================================================================
 File        : StaffViewModel.cs
 Namespace   : AbsenceApp.Core.ViewModels
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-14
 Updated     : 2026-03-14
-------------------------------------------------------------------------------
 Purpose     : ViewModel for the Staff list page. Loads all user (staff)
               records via IUserService and exposes them for data-binding.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-14  Initial implementation.
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
    private readonly IUserService _userService;

    public StaffViewModel(IUserService userService)
    {
        _userService = userService;
    }

    // =========================================================================
    // Bindable state
    // =========================================================================
    public IEnumerable<UserDto> Staff { get; private set; } = Enumerable.Empty<UserDto>();
    public bool IsLoading { get; private set; }
    public string? ErrorMessage { get; private set; }

    // =========================================================================
    // Commands
    // =========================================================================

    /// <summary>Loads all staff (user) records from the service.</summary>
    public async Task LoadAsync()
    {
        IsLoading = true;
        ErrorMessage = null;
        try
        {
            Staff = await _userService.GetAllAsync();
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
