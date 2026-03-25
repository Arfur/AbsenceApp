/*
===============================================================================
 File        : StudentDetailViewModelV2.cs
 Namespace   : AbsenceApp.Client.ViewModels.V2
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-21
 Updated     : 2026-03-21
-------------------------------------------------------------------------------
 Purpose     : ViewModel for StudentDetailPageV2. Loads the full student
               detail record and associated absence history via
               StudentsApiServiceV2. Drives StudentDetailPageV2.
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
/// Drives the Student detail page V2. Register as Scoped (Phase 10).
/// </summary>
public sealed class StudentDetailViewModelV2
{
    private readonly StudentsApiServiceV2 _api;

    public StudentDetailViewModelV2(StudentsApiServiceV2 api) => _api = api;

    // -------------------------------------------------------------------------
    // State
    // -------------------------------------------------------------------------

    public StudentFullViewDto? Item { get; private set; }
    public List<StudentAbsenceDto> Absences { get; private set; } = [];
    public bool IsLoading { get; private set; }
    public string? Error { get; private set; }

    // -------------------------------------------------------------------------
    // Data loading
    // -------------------------------------------------------------------------

    public async Task LoadAsync(long id, CancellationToken ct = default)
    {
        IsLoading = true;
        Error = null;
        Item = null;
        Absences = [];

        var detailResult = await _api.GetDetailAsync(id, ct);
        if (detailResult.Success)
        {
            Item = detailResult.Data;
        }
        else
        {
            Error = detailResult.ErrorMessage ?? "Failed to load student.";
            IsLoading = false;
            return;
        }

        var absenceResult = await _api.GetAbsencesAsync(id, ct);
        if (absenceResult.Success && absenceResult.Data is not null)
            Absences = absenceResult.Data;

        IsLoading = false;
    }
}
