/*
===============================================================================
 File        : StudentAbsenceFormViewModelV2.cs
 Namespace   : AbsenceApp.Client.ViewModels.V2
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-05-05
 Updated     : 2026-05-05
-------------------------------------------------------------------------------
 Purpose     : ViewModel for StudentAbsenceFormPageV2.
               Handles both Create and Edit modes for a student absence record.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-05-05  Initial creation.
-------------------------------------------------------------------------------
 Notes       :
   - IsNew=true  → InitNewAsync(studentId)   → CreateAbsenceAsync on save
   - IsNew=false → InitEditAsync(studentId, absenceId) → UpdateAbsenceAsync on save
   - Register as Scoped in V2ServiceCollectionExtensions.cs.
===============================================================================
*/

using AbsenceApp.Client.Services.ApiV2.Modules;
using AbsenceApp.Core.DTOs;

namespace AbsenceApp.Client.ViewModels.V2;

public sealed class StudentAbsenceFormViewModelV2
{
    private readonly StudentProfileApiServiceV2 _api;

    public StudentAbsenceFormViewModelV2(StudentProfileApiServiceV2 api) => _api = api;

    // =========================================================================
    // Mode
    // =========================================================================

    public int  StudentId  { get; private set; }
    public long AbsenceId  { get; private set; }
    public bool IsNew      { get; private set; } = true;

    // =========================================================================
    // Form fields
    // =========================================================================

    public long     AbsenceTypeId { get; set; }
    public long     StatusId      { get; set; }
    public DateOnly StartDate     { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public DateOnly EndDate       { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public string   ReportedVia   { get; set; } = "Manual";
    public string?  Notes         { get; set; }

    // =========================================================================
    // Reference data
    // =========================================================================

    public IReadOnlyList<AbsenceTypeDto>   AbsenceTypes    { get; private set; } = [];
    public IReadOnlyList<AbsenceStatusDto> AbsenceStatuses { get; private set; } = [];

    // =========================================================================
    // State
    // =========================================================================

    public bool    IsLoading { get; private set; }
    public bool    IsSaving  { get; private set; }
    public string? Error     { get; private set; }
    public string? Success   { get; private set; }

    // =========================================================================
    // Initialisation — New
    // =========================================================================

    public async Task InitNewAsync(int studentId, CancellationToken ct = default)
    {
        StudentId = studentId;
        IsNew     = true;
        AbsenceId = 0;
        Error     = null;
        Success   = null;
        IsLoading = true;

        // Reset form fields
        AbsenceTypeId = 0;
        StatusId      = 0;
        StartDate     = DateOnly.FromDateTime(DateTime.Today);
        EndDate       = DateOnly.FromDateTime(DateTime.Today);
        ReportedVia   = "Manual";
        Notes         = null;

        try
        {
            var typesTask    = _api.GetAbsenceTypesAsync(ct);
            var statusesTask = _api.GetAbsenceStatusesAsync(ct);
            await Task.WhenAll(typesTask, statusesTask);
            AbsenceTypes    = (await typesTask).ToList().AsReadOnly();
            AbsenceStatuses = (await statusesTask).ToList().AsReadOnly();

            // Default to first type/status if available
            if (AbsenceTypes.Count > 0)    AbsenceTypeId = AbsenceTypes[0].Id;
            if (AbsenceStatuses.Count > 0) StatusId      = AbsenceStatuses
                .FirstOrDefault(s => s.Code == "PENDING")?.Id
                ?? AbsenceStatuses[0].Id;
        }
        catch (Exception ex)
        {
            Error = $"Failed to load reference data: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    // =========================================================================
    // Initialisation — Edit
    // =========================================================================

    public async Task InitEditAsync(int studentId, long absenceId, CancellationToken ct = default)
    {
        StudentId = studentId;
        AbsenceId = absenceId;
        IsNew     = false;
        Error     = null;
        Success   = null;
        IsLoading = true;

        try
        {
            var typesTask    = _api.GetAbsenceTypesAsync(ct);
            var statusesTask = _api.GetAbsenceStatusesAsync(ct);
            var absTask      = _api.GetAbsencesAsync(studentId, ct);

            await Task.WhenAll(typesTask, statusesTask, absTask);

            AbsenceTypes    = (await typesTask).ToList().AsReadOnly();
            AbsenceStatuses = (await statusesTask).ToList().AsReadOnly();

            var absence = (await absTask).FirstOrDefault(a => a.Id == absenceId);
            if (absence is null)
            {
                Error = "Absence record not found.";
                return;
            }

            AbsenceTypeId = absence.AbsenceTypeId;
            StatusId      = absence.StatusId;
            StartDate     = absence.StartDate;
            EndDate       = absence.EndDate;
            ReportedVia   = absence.ReportedVia;
            Notes         = absence.Notes;
        }
        catch (Exception ex)
        {
            Error = $"Failed to load absence: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    // =========================================================================
    // Save
    // =========================================================================

    public async Task<bool> SaveAsync(CancellationToken ct = default)
    {
        if (StartDate > EndDate)
        {
            Error = "Start date cannot be after end date.";
            return false;
        }

        IsSaving = true;
        Error    = null;
        Success  = null;

        try
        {
            if (IsNew)
            {
                var dto = new CreateAbsenceDto
                {
                    PersonType    = "Student",
                    PersonId      = StudentId,
                    AbsenceTypeId = AbsenceTypeId,
                    StartDate     = StartDate,
                    EndDate       = EndDate,
                    ReportedVia   = ReportedVia,
                    Notes         = Notes,
                };
                var (ok, err, _) = await _api.CreateAbsenceAsync(dto, ct);
                if (!ok) { Error = err; return false; }
            }
            else
            {
                var dto = new UpdateAbsenceDto
                {
                    AbsenceTypeId = AbsenceTypeId,
                    StatusId      = StatusId,
                    StartDate     = StartDate,
                    EndDate       = EndDate,
                    ReportedVia   = ReportedVia,
                    Notes         = Notes,
                };
                var (ok, err) = await _api.UpdateAbsenceAsync(AbsenceId, dto, ct);
                if (!ok) { Error = err; return false; }
            }

            Success = "Absence saved successfully.";
            return true;
        }
        catch (Exception ex)
        {
            Error = $"Save failed: {ex.Message}";
            return false;
        }
        finally
        {
            IsSaving = false;
        }
    }
}
