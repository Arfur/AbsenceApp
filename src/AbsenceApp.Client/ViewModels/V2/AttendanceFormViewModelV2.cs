/*
===============================================================================
 File        : AttendanceFormViewModelV2.cs
 Namespace   : AbsenceApp.Client.ViewModels.V2
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-21
 Updated     : 2026-03-21
-------------------------------------------------------------------------------
 Purpose     : ViewModel for AttendanceFormPageV2. Manages the mark-attendance
               workflow for a class session, building the AttendanceMarkDto
               and delegating to AttendanceApiServiceV2.
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
/// Drives the Attendance mark/register form page V2. Register as Scoped (Phase 10).
/// </summary>
public sealed class AttendanceFormViewModelV2
{
    private readonly AttendanceApiServiceV2 _api;

    public AttendanceFormViewModelV2(AttendanceApiServiceV2 api) => _api = api;

    // -------------------------------------------------------------------------
    // State
    // -------------------------------------------------------------------------

    public long ClassId { get; private set; }
    public bool IsBusy { get; private set; }
    public string? Error { get; private set; }
    public bool SaveSuccess { get; private set; }

    // -------------------------------------------------------------------------
    // Form fields
    // -------------------------------------------------------------------------

    /// <summary>The student ID to record attendance for.</summary>
    public long StudentId { get; set; }

    /// <summary>Attendance mark code (e.g. "P" = Present, "A" = Absent, "L" = Late).</summary>
    public string MarkCode { get; set; } = "P";

    public bool? IsLate { get; set; }
    public int? MinutesLate { get; set; }
    public string Notes { get; set; } = string.Empty;
    public long RecordedBy { get; set; }

    // -------------------------------------------------------------------------
    // Initialise
    // -------------------------------------------------------------------------

    public void LoadForClass(long classId)
    {
        ClassId = classId;
        StudentId = 0;
        MarkCode = "P";
        IsLate = null;
        MinutesLate = null;
        Notes = string.Empty;
        RecordedBy = 0;
    }

    // -------------------------------------------------------------------------
    // Actions
    // -------------------------------------------------------------------------

    public async Task<bool> MarkAsync(CancellationToken ct = default)
    {
        IsBusy = true;
        Error = null;
        SaveSuccess = false;

        var dto = new AttendanceMarkDto
        {
            StudentId   = StudentId,
            MarkCode    = MarkCode,
            IsLate      = IsLate,
            MinutesLate = MinutesLate,
            Notes       = string.IsNullOrWhiteSpace(Notes) ? null : Notes,
            RecordedBy  = RecordedBy,
        };

        var result = await _api.MarkAttendanceAsync(ClassId, dto, ct);
        SaveSuccess = result.Success;
        if (!result.Success)
            Error = result.ErrorMessage ?? "Failed to mark attendance.";

        IsBusy = false;
        return SaveSuccess;
    }
}
