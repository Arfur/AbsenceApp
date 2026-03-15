/*
===============================================================================
 File        : AttendanceViewModel.cs
 Namespace   : AbsenceApp.Core.ViewModels
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-14
 Updated     : 2026-03-14
-------------------------------------------------------------------------------
 Purpose     : ViewModel for the Attendance page. Loads all attendance records
               via IAttendanceService and exposes them for data-binding.
               Supports optional filtering by user ID.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-14  Initial implementation.
===============================================================================
*/

using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;

namespace AbsenceApp.Core.ViewModels;

// ============================================================================
// AttendanceViewModel — backing state for AttendancePage.razor
// ============================================================================
public class AttendanceViewModel
{
    // =========================================================================
    // Dependencies
    // =========================================================================
    private readonly IAttendanceService _attendanceService;

    public AttendanceViewModel(IAttendanceService attendanceService)
    {
        _attendanceService = attendanceService;
    }

    // =========================================================================
    // Bindable state
    // =========================================================================
    public IEnumerable<AttendanceDto> Records { get; private set; } = Enumerable.Empty<AttendanceDto>();
    public bool IsLoading { get; private set; }
    public string? ErrorMessage { get; private set; }

    // =========================================================================
    // Commands
    // =========================================================================

    /// <summary>Loads all attendance records from the service.</summary>
    public async Task LoadAsync()
    {
        IsLoading = true;
        ErrorMessage = null;
        try
        {
            Records = await _attendanceService.GetAllAsync();
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

    /// <summary>Loads attendance records filtered to the specified user ID.</summary>
    public async Task LoadByUserAsync(int userId)
    {
        IsLoading = true;
        ErrorMessage = null;
        try
        {
            Records = await _attendanceService.GetByUserAsync(userId);
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
