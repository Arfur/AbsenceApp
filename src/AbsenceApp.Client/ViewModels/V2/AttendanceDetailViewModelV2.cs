/*
===============================================================================
 File        : AttendanceDetailViewModelV2.cs
 Namespace   : AbsenceApp.Client.ViewModels.V2
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-21
 Updated     : 2026-03-21
-------------------------------------------------------------------------------
 Purpose     : ViewModel for AttendanceDetailPageV2. Loads the attendance
               register for a specific class session via
               AttendanceApiServiceV2. Drives AttendanceDetailPageV2.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-21  Initial implementation (Phase 7).
-------------------------------------------------------------------------------
 Notes       :
   - Register as Scoped in DI (Phase 10).
===============================================================================
*/

using AbsenceApp.Client.Services.ApiV2.Modules;
using AbsenceApp.Core.DTOs;

namespace AbsenceApp.Client.ViewModels.V2;

/// <summary>
/// Drives the Attendance detail page V2 (class register view).
/// Register as Scoped (Phase 10).
/// </summary>
public sealed class AttendanceDetailViewModelV2
{
    private readonly AttendanceApiServiceV2 _api;

    public AttendanceDetailViewModelV2(AttendanceApiServiceV2 api) => _api = api;

    // -------------------------------------------------------------------------
    // State
    // -------------------------------------------------------------------------

    public AttendanceRegisterDto? Register { get; private set; }
    public bool IsLoading { get; private set; }
    public string? Error { get; private set; }

    // -------------------------------------------------------------------------
    // Data loading
    // -------------------------------------------------------------------------

    public async Task LoadAsync(long classId, CancellationToken ct = default)
    {
        IsLoading = true;
        Error = null;
        Register = null;

        var result = await _api.GetRegisterAsync(classId, ct);
        if (result.Success)
            Register = result.Data;
        else
            Error = result.ErrorMessage ?? "Failed to load attendance register.";

        IsLoading = false;
    }
}
