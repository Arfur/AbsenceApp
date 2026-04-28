/*
===============================================================================
 File        : StudentFormViewModelV2.cs
 Namespace   : AbsenceApp.Client.ViewModels.V2
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-21
 Updated     : 2026-03-21
-------------------------------------------------------------------------------
 Purpose     : ViewModel for StudentFormPageV2 (add and edit). Manages form
               field state and delegates create/update calls to
               StudentsApiServiceV2.
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
/// Drives the Student add/edit form page V2. Register as Scoped (Phase 10).
/// </summary>
public sealed class StudentFormViewModelV2
{
    private readonly StudentsApiServiceV2 _api;

    public StudentFormViewModelV2(StudentsApiServiceV2 api) => _api = api;

    // -------------------------------------------------------------------------
    // Mode
    // -------------------------------------------------------------------------

    public bool IsNew { get; private set; } = true;
    public int EditId { get; private set; }

    // -------------------------------------------------------------------------
    // UI state
    // -------------------------------------------------------------------------

    public bool IsBusy { get; private set; }
    public string? Error { get; private set; }
    public bool SaveSuccess { get; private set; }

    // -------------------------------------------------------------------------
    // Form fields
    // -------------------------------------------------------------------------

    public string AdmissionNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string MiddleNames { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string LegalFirstName { get; set; } = string.Empty;
    public string LegalLastName { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; } = DateTime.Today.AddYears(-10);
    public int YearGroupId { get; set; }
    public int ClassId { get; set; }
    public string Status { get; set; } = "Active";

    // -------------------------------------------------------------------------
    // Initialise
    // -------------------------------------------------------------------------

    public void LoadForCreate()
    {
        IsNew = true;
        EditId = 0;
        ResetFields();
    }

    public async Task LoadForEditAsync(long id, CancellationToken ct = default)
    {
        IsNew = false;
        EditId = (int)id;
        IsBusy = true;
        Error = null;

        var result = await _api.GetDetailAsync(id, ct);
        if (result.Success && result.Data is not null)
        {
            var s = result.Data;
            AdmissionNumber = s.AdmissionNumber;
            FirstName       = s.FirstName;
            LastName        = s.LastName;
            Gender          = s.Gender ?? string.Empty;
            Status          = s.Status;
        }
        else
        {
            Error = result.ErrorMessage ?? "Failed to load student for editing.";
        }

        IsBusy = false;
    }

    // -------------------------------------------------------------------------
    // Actions
    // -------------------------------------------------------------------------

    public async Task<bool> SaveAsync(CancellationToken ct = default)
    {
        IsBusy = true;
        Error = null;
        SaveSuccess = false;

        var dto = BuildDto();

        if (IsNew)
        {
            var result = await _api.CreateAsync(dto, ct);
            SaveSuccess = result.Success;
            if (!result.Success) Error = result.ErrorMessage ?? "Failed to create student.";
        }
        else
        {
            var result = await _api.UpdateAsync(EditId, dto, ct);
            SaveSuccess = result.Success;
            if (!result.Success) Error = result.ErrorMessage ?? "Failed to update student.";
        }

        IsBusy = false;
        return SaveSuccess;
    }

    // -------------------------------------------------------------------------
    // Private helpers
    // -------------------------------------------------------------------------

    private void ResetFields()
    {
        AdmissionNumber = string.Empty;
        FirstName       = string.Empty;
        MiddleNames     = string.Empty;
        LastName        = string.Empty;
        LegalFirstName  = string.Empty;
        LegalLastName   = string.Empty;
        Gender          = string.Empty;
        DateOfBirth     = DateTime.Today.AddYears(-10);
        YearGroupId     = 0;
        ClassId         = 0;
        Status          = "Active";
    }

    private StudentDto BuildDto() => new()
    {
        Id              = IsNew ? 0 : EditId,
        AdmissionNumber = AdmissionNumber,
        FirstName       = FirstName,
        MiddleNames     = string.IsNullOrWhiteSpace(MiddleNames) ? null : MiddleNames,
        LastName        = LastName,
        LegalFirstName  = LegalFirstName,
        LegalLastName   = LegalLastName,
        Gender          = Gender,
        DateOfBirth     = DateOnly.FromDateTime(DateOfBirth),
        YearGroupId     = YearGroupId,
        ClassId         = ClassId,
        Status          = Status,
    };
}
