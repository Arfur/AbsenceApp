/*
===============================================================================
 File        : StaffFormViewModelV2.cs
 Namespace   : AbsenceApp.Client.ViewModels.V2
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-21
 Updated     : 2026-03-21
-------------------------------------------------------------------------------
 Purpose     : ViewModel for StaffFormPageV2 (add and edit). Manages form
               field state and delegates create/update calls to
               StaffApiServiceV2.
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
/// Drives the Staff add/edit form page V2. Register as Scoped (Phase 10).
/// </summary>
public sealed class StaffFormViewModelV2
{
    private readonly StaffApiServiceV2 _api;

    public StaffFormViewModelV2(StaffApiServiceV2 api) => _api = api;

    // -------------------------------------------------------------------------
    // Mode
    // -------------------------------------------------------------------------

    public bool IsNew { get; private set; } = true;
    public long EditId { get; private set; }

    // -------------------------------------------------------------------------
    // UI state
    // -------------------------------------------------------------------------

    public bool IsBusy { get; private set; }
    public string? Error { get; private set; }
    public bool SaveSuccess { get; private set; }

    // -------------------------------------------------------------------------
    // Form fields
    // -------------------------------------------------------------------------

    public string StaffNumber { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string WorkEmail { get; set; } = string.Empty;
    public string EmploymentType { get; set; } = "Full-time";
    public string ContractType { get; set; } = "Permanent";
    public string WorkLocation { get; set; } = string.Empty;
    public DateOnly HireDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public string AccountStatus { get; set; } = "Active";
    public long DepartmentId { get; set; }
    public long JobTitleId { get; set; }
    public long JobGroupId { get; set; }

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
        EditId = id;
        IsBusy = true;
        Error = null;

        var result = await _api.GetDetailAsync(id, ct);
        if (result.Success && result.Data is not null)
        {
            var s = result.Data;
            StaffNumber    = s.StaffNumber;
            Title          = s.Title;
            FirstName      = s.FirstName;
            LastName       = s.LastName;
            WorkEmail      = s.WorkEmail;
            EmploymentType = s.EmploymentType;
            ContractType   = s.ContractType;
            WorkLocation   = s.WorkLocation;
            HireDate       = s.HireDate;
            AccountStatus  = s.AccountStatus;
            // DepartmentId, JobTitleId, JobGroupId: StaffFullViewDto exposes resolved
            // names (DepartmentName etc.) rather than raw IDs. Leave IDs as 0.
        }
        else
        {
            Error = result.ErrorMessage ?? "Failed to load staff member for editing.";
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
            if (!result.Success) Error = result.ErrorMessage ?? "Failed to create staff member.";
        }
        else
        {
            var result = await _api.UpdateAsync(EditId, dto, ct);
            SaveSuccess = result.Success;
            if (!result.Success) Error = result.ErrorMessage ?? "Failed to update staff member.";
        }

        IsBusy = false;
        return SaveSuccess;
    }

    // -------------------------------------------------------------------------
    // Private helpers
    // -------------------------------------------------------------------------

    private void ResetFields()
    {
        StaffNumber    = string.Empty;
        Title          = string.Empty;
        FirstName      = string.Empty;
        LastName       = string.Empty;
        WorkEmail      = string.Empty;
        EmploymentType = "Full-time";
        ContractType   = "Permanent";
        WorkLocation   = string.Empty;
        HireDate       = DateOnly.FromDateTime(DateTime.Today);
        AccountStatus  = "Active";
        DepartmentId   = 0;
        JobTitleId     = 0;
        JobGroupId     = 0;
    }

    private StaffDto BuildDto() => new()
    {
        Id             = IsNew ? 0 : EditId,
        StaffNumber    = StaffNumber,
        Title          = Title,
        FirstName      = FirstName,
        LastName       = LastName,
        WorkEmail      = WorkEmail,
        EmploymentType = EmploymentType,
        ContractType   = ContractType,
        WorkLocation   = WorkLocation,
        HireDate       = HireDate,
        AccountStatus  = AccountStatus,
        DepartmentId   = DepartmentId,
        JobTitleId     = JobTitleId,
        JobGroupId     = JobGroupId,
        DateOfBirth    = DateOnly.MinValue,
    };
}
