/*
===============================================================================
 File        : StaffFormViewModelV2.cs
 Namespace   : AbsenceApp.Client.ViewModels.V2
 Author      : Michael
 Version     : 1.1.0
 Created     : 2026-03-21
 Updated     : 2026-05-10
-------------------------------------------------------------------------------
 Purpose     : ViewModel for StaffFormPageV2 (add and edit). Manages form
               field state and delegates create/update calls to
               StaffProfileApiServiceV2 (EF Core, MAUI-compatible).
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-21  Initial implementation (Phase 7).
   - 1.1.0  2026-05-10  Fix JobGroup mapping: switched from StaffApiServiceV2
                         (HTTP) to StaffProfileApiServiceV2 (EF Core). LoadForEditAsync
                         now calls GetStaffRawAsync so DepartmentId, JobTitleId,
                         and JobGroupId are properly loaded. SaveAsync now calls
                         CreateStaffAsync / UpdateStaffAsync (EF Core).
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
    private readonly StaffProfileApiServiceV2 _api;

    public StaffFormViewModelV2(StaffProfileApiServiceV2 api) => _api = api;

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

    public string StaffNumber { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string WorkEmail { get; set; } = string.Empty;
    public string EmploymentType { get; set; } = "Full-time";
    public string ContractType { get; set; } = "Permanent";
    public string WorkLocation { get; set; } = string.Empty;
    public DateOnly HireDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public DateOnly DateOfBirth { get; set; } = DateOnly.FromDateTime(DateTime.Today.AddYears(-20));
    public string AccountStatus { get; set; } = "Active";
    public int DepartmentId { get; set; }
    public int JobTitleId { get; set; }
    public int JobGroupId { get; set; }

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

        var raw = await _api.GetStaffRawAsync(id, ct);
        if (raw is not null)
        {
            StaffNumber    = raw.StaffNumber;
            Title          = raw.Title;
            FirstName      = raw.FirstName;
            LastName       = raw.LastName;
            WorkEmail      = raw.WorkEmail;
            EmploymentType = raw.EmploymentType;
            ContractType   = raw.ContractType;
            WorkLocation   = raw.WorkLocation;
            HireDate       = raw.HireDate;
            DateOfBirth    = raw.DateOfBirth;
            AccountStatus  = raw.AccountStatus;
            DepartmentId   = raw.DepartmentId;
            JobTitleId     = raw.JobTitleId;
            JobGroupId     = raw.JobGroupId;
        }
        else
        {
            Error = "Failed to load staff member for editing.";
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
            var (success, error, _) = await _api.CreateStaffAsync(dto, ct);
            SaveSuccess = success;
            if (!success) Error = error ?? "Failed to create staff member.";
        }
        else
        {
            var (success, error) = await _api.UpdateStaffAsync(EditId, dto, ct);
            SaveSuccess = success;
            if (!success) Error = error ?? "Failed to update staff member.";
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
        DateOfBirth    = DateOnly.FromDateTime(DateTime.Today.AddYears(-20));
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
        DateOfBirth    = DateOfBirth,
    };
}