/*
===============================================================================
 File        : StaffProfileViewModelV2.cs
 Namespace   : AbsenceApp.Client.ViewModels.V2
 Author      : Michael
 Version     : 1.2.0
 Created     : 2026-05-09
 Updated     : 2026-05-10
-------------------------------------------------------------------------------
 Purpose     : ViewModel for StaffProfilePageV2. Provides staff profile data
               across 7 tabs with full view / edit / add mode support,
               avatar upload and absence CRUD, plus shared banner/tab/selector
               state for the unified V2 profile chrome.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-05-09  Initial creation (Phase 5).
   - 1.1.0  2026-05-10  Added shared profile banner fields, tab metadata, and
                         searchable selector state for the unified profile UI.
     - 1.2.0  2026-05-10  Updated runtime tab contract to Basic Info, Contacts,
                                                 Classes, Devices, External, Medical, Absences.
-------------------------------------------------------------------------------
 Notes       :
     - Register as Scoped in V2ServiceCollectionExtensions.cs.
     - Tabs: 0=Basic Info, 1=Contacts, 2=Classes, 3=Devices,
                     4=External, 5=Medical, 6=Absences
===============================================================================
*/

using AbsenceApp.Client.Services.ApiV2.Modules;
using AbsenceApp.Core.DTOs;
using Microsoft.AspNetCore.Components.Forms;

namespace AbsenceApp.Client.ViewModels.V2;

public sealed class StaffProfileViewModelV2
{
    private readonly StaffProfileApiServiceV2 _api;

    public StaffProfileViewModelV2(StaffProfileApiServiceV2 api) => _api = api;

    // =========================================================================
    // Identity
    // =========================================================================

    public long StaffId { get; private set; }

    // =========================================================================
    // Active tab (0–6)
    // =========================================================================

    public int ActiveTab { get; private set; } = 0;
    public void SetTab(int index)
    {
        if (!IsNew || index == 0)
            ActiveTab = index;
    }

    public IReadOnlyList<ProfileTabItemDto> ProfileTabs =>
    [
        new() { Index = 0, Label = "Basic Info", Icon = "bi-person",              Enabled = true },
        new() { Index = 1, Label = "Contacts",   Icon = "bi-telephone",           Enabled = !IsNew },
        new() { Index = 2, Label = "Classes",    Icon = "bi-mortarboard",         Enabled = !IsNew },
        new() { Index = 3, Label = "Devices",    Icon = "bi-laptop",              Enabled = !IsNew },
        new() { Index = 4, Label = "External",   Icon = "bi-box-arrow-up-right",  Enabled = !IsNew },
        new() { Index = 5, Label = "Medical",    Icon = "bi-heart-pulse",         Enabled = !IsNew },
        new() { Index = 6, Label = "Absences",   Icon = "bi-calendar-x",          Enabled = !IsNew },
    ];

    // =========================================================================
    // Banner header
    // =========================================================================

    public string HeaderFullName    { get; private set; } = string.Empty;
    public string HeaderStaffNumber { get; private set; } = string.Empty;
    public string HeaderDepartment  { get; private set; } = string.Empty;
    public string HeaderStatus      { get; private set; } = string.Empty;

    public IReadOnlyList<ProfileBannerFieldDto> BannerFields =>
    [
        new() { Label = "Staff No:",   Value = HeaderStaffNumber },
        new() { Label = "Department:", Value = HeaderDepartment },
        new()
        {
            Label = "Status:",
            Value = string.IsNullOrWhiteSpace(HeaderStatus) ? "—" : HeaderStatus,
            IsBadge = true,
            CssClass = BuildStatusBadgeClass(HeaderStatus),
        },
    ];

    // =========================================================================
    // Shared selector
    // =========================================================================

    public string SelectorSearchText { get; private set; } = string.Empty;
    public bool IsSelectorLoading { get; private set; }
    public IReadOnlyList<ProfileNameSelectorItemDto> ProfileSelectorItems { get; private set; } = [];

    public async Task RefreshProfileSelectorAsync(CancellationToken ct = default)
    {
        if (IsNew)
        {
            ProfileSelectorItems = [];
            return;
        }

        IsSelectorLoading = true;
        try
        {
            ProfileSelectorItems = await _api.SearchStaffProfileSelectorAsync(null, ct: ct);
        }
        finally
        {
            IsSelectorLoading = false;
        }
    }

    public async Task SearchProfileSelectorAsync(string term, CancellationToken ct = default)
    {
        SelectorSearchText = term;

        if (IsNew)
        {
            ProfileSelectorItems = [];
            return;
        }

        IsSelectorLoading = true;
        try
        {
            ProfileSelectorItems = await _api.SearchStaffProfileSelectorAsync(term, ct: ct);
        }
        finally
        {
            IsSelectorLoading = false;
        }
    }

    // =========================================================================
    // Profile photo
    // =========================================================================

    public byte[]? PhotoBytes { get; private set; }

    public string? PhotoDataUri => PhotoBytes is { Length: > 0 } b
        ? $"data:image/jpeg;base64,{Convert.ToBase64String(b)}"
        : null;

    public bool    IsUploadingPhoto { get; private set; }
    public string? PhotoError       { get; private set; }

    // =========================================================================
    // Tab 0 — Profile (view)
    // =========================================================================

    public StaffFullViewDto? Staff { get; private set; }

    // =========================================================================
    // Tab 3 — Absences (CRUD)
    // =========================================================================

    public IReadOnlyList<AbsenceDto>       Absences        { get; private set; } = [];
    public IReadOnlyList<AbsenceTypeDto>   AbsenceTypes    { get; private set; } = [];
    public IReadOnlyList<AbsenceStatusDto> AbsenceStatuses { get; private set; } = [];

    // =========================================================================
    // State
    // =========================================================================

    public bool    IsLoading { get; private set; }
    public string? Error     { get; private set; }

    // =========================================================================
    // Initialisation
    // =========================================================================

    public async Task InitAsync(long staffId, CancellationToken ct = default)
    {
        StaffId   = staffId;
        IsLoading = true;
        Error     = null;
        Staff     = null;

        try
        {
            var staffTask    = _api.GetStaffAsync(staffId, ct);
            var absencesTask = _api.GetAbsencesAsync(staffId, ct);
            var typesTask    = _api.GetAbsenceTypesAsync(ct);
            var statusesTask = _api.GetAbsenceStatusesAsync(ct);

            await Task.WhenAll(staffTask, absencesTask, typesTask, statusesTask);

            Staff           = await staffTask;
            Absences        = (await absencesTask).ToList().AsReadOnly();
            AbsenceTypes    = (await typesTask).ToList().AsReadOnly();
            AbsenceStatuses = (await statusesTask).ToList().AsReadOnly();

            if (Staff is not null)
            {
                HeaderFullName    = Staff.FullName;
                HeaderStaffNumber = Staff.StaffNumber;
                HeaderDepartment  = Staff.DepartmentName;
                HeaderStatus      = Staff.AccountStatus;
                SelectorSearchText = HeaderFullName;

                var photoPath = Path.Combine(FileSystem.AppDataDirectory, "staff_photos",
                                             $"staff_{StaffId}.jpg");
                PhotoBytes = File.Exists(photoPath)
                    ? await File.ReadAllBytesAsync(photoPath, ct)
                    : null;

                await RefreshProfileSelectorAsync(ct);
            }
            else
            {
                Error = "Staff member not found.";
                ProfileSelectorItems = [];
                SelectorSearchText = string.Empty;
            }
        }
        catch (Exception ex)
        {
            Error = $"Failed to load staff profile: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    // =========================================================================
    // Photo upload / removal
    // =========================================================================

    public async Task UploadPhotoAsync(IBrowserFile file, CancellationToken ct = default)
    {
        IsUploadingPhoto = true;
        PhotoError       = null;

        try
        {
            const long maxSize = 4 * 1024 * 1024;
            using var stream = file.OpenReadStream(maxSize, ct);
            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms, ct);
            PhotoBytes = ms.ToArray();

            var dir  = Path.Combine(FileSystem.AppDataDirectory, "staff_photos");
            Directory.CreateDirectory(dir);
            var ext  = Path.GetExtension(file.Name).ToLowerInvariant();
            var path = Path.Combine(dir, $"staff_{StaffId}{ext}");
            await File.WriteAllBytesAsync(path, PhotoBytes, ct);
        }
        catch (Exception ex)
        {
            PhotoError = $"Photo upload failed: {ex.Message}";
        }
        finally
        {
            IsUploadingPhoto = false;
        }
    }

    public void RemovePhoto()
    {
        PhotoBytes = null;
        PhotoError = null;
    }

    // =========================================================================
    // Absence CRUD helpers
    // =========================================================================

    public async Task DeleteAbsenceAsync(long id, CancellationToken ct = default)
    {
        Error = null;
        var (ok, err) = await _api.DeleteAbsenceAsync(id, ct);
        if (!ok)
        {
            Error = err;
            return;
        }

        var fresh = await _api.GetAbsencesAsync(StaffId, ct);
        Absences = fresh.ToList().AsReadOnly();
    }

    // =========================================================================
    // Edit / Add mode
    // =========================================================================

    public bool IsNew     { get; private set; }
    public bool IsEditing { get; private set; }

    public IReadOnlyList<DepartmentDto> Departments { get; private set; } = [];
    public IReadOnlyList<JobTitleDto>   JobTitles   { get; private set; } = [];

    public string   EditStaffNumber        { get; set; } = string.Empty;
    public string   EditTitle              { get; set; } = string.Empty;
    public string   EditFirstName          { get; set; } = string.Empty;
    public string   EditLastName           { get; set; } = string.Empty;
    public string   EditPreferredName      { get; set; } = string.Empty;
    public string   EditGender             { get; set; } = string.Empty;
    public DateOnly EditDateOfBirth        { get; set; } = DateOnly.FromDateTime(DateTime.Today.AddYears(-20));
    public string   EditWorkEmail          { get; set; } = string.Empty;
    public string   EditAltEmail           { get; set; } = string.Empty;
    public string   EditPhoneHome          { get; set; } = string.Empty;
    public string   EditPhoneMobile        { get; set; } = string.Empty;
    public string   EditPhoneEmergency     { get; set; } = string.Empty;
    public string   EditEmploymentType     { get; set; } = "Full-time";
    public string   EditContractType       { get; set; } = "Permanent";
    public DateOnly EditHireDate           { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public DateOnly? EditEndDate           { get; set; }
    public string   EditWorkLocation       { get; set; } = string.Empty;
    public int      EditJobTitleId         { get; set; }
    public int      EditJobGroupId         { get; set; }
    public int      EditDepartmentId       { get; set; }
    public int?     EditReportingManagerId { get; set; }
    public string   EditAccountStatus      { get; set; } = "Active";

    public bool    IsSaving  { get; private set; }
    public string? SaveError { get; private set; }

    public async Task InitNewAsync(CancellationToken ct = default)
    {
        IsNew     = true;
        IsEditing = true;
        StaffId   = 0;
        Staff     = null;
        Absences  = [];
        HeaderFullName     = "New Staff Member";
        HeaderStaffNumber  = string.Empty;
        HeaderDepartment   = string.Empty;
        HeaderStatus       = string.Empty;
        PhotoBytes         = null;
        PhotoError         = null;
        ActiveTab          = 0;
        SelectorSearchText = string.Empty;
        ProfileSelectorItems = [];

        EditStaffNumber        = string.Empty;
        EditTitle              = string.Empty;
        EditFirstName          = string.Empty;
        EditLastName           = string.Empty;
        EditPreferredName      = string.Empty;
        EditGender             = string.Empty;
        EditDateOfBirth        = DateOnly.FromDateTime(DateTime.Today.AddYears(-20));
        EditWorkEmail          = string.Empty;
        EditAltEmail           = string.Empty;
        EditPhoneHome          = string.Empty;
        EditPhoneMobile        = string.Empty;
        EditPhoneEmergency     = string.Empty;
        EditEmploymentType     = "Full-time";
        EditContractType       = "Permanent";
        EditHireDate           = DateOnly.FromDateTime(DateTime.Today);
        EditEndDate            = null;
        EditWorkLocation       = string.Empty;
        EditJobTitleId         = 0;
        EditJobGroupId         = 0;
        EditDepartmentId       = 0;
        EditReportingManagerId = null;
        EditAccountStatus      = "Active";

        await LoadLookupsAsync(ct);
    }

    public async Task BeginEditAsync(CancellationToken ct = default)
    {
        if (IsNew)
            return;

        var raw = await _api.GetStaffRawAsync(StaffId, ct);
        if (raw is not null)
        {
            EditStaffNumber        = raw.StaffNumber;
            EditTitle              = raw.Title;
            EditFirstName          = raw.FirstName;
            EditLastName           = raw.LastName;
            EditPreferredName      = raw.PreferredName ?? string.Empty;
            EditGender             = raw.Gender ?? string.Empty;
            EditDateOfBirth        = raw.DateOfBirth;
            EditWorkEmail          = raw.WorkEmail;
            EditAltEmail           = raw.AltEmail ?? string.Empty;
            EditPhoneHome          = raw.PhoneHome ?? string.Empty;
            EditPhoneMobile        = raw.PhoneMobile ?? string.Empty;
            EditPhoneEmergency     = raw.PhoneEmergency ?? string.Empty;
            EditEmploymentType     = raw.EmploymentType;
            EditContractType       = raw.ContractType;
            EditHireDate           = raw.HireDate;
            EditEndDate            = raw.EndDate;
            EditWorkLocation       = raw.WorkLocation;
            EditJobTitleId         = raw.JobTitleId;
            EditJobGroupId         = raw.JobGroupId;
            EditDepartmentId       = raw.DepartmentId;
            EditReportingManagerId = raw.ReportingManagerId;
            EditAccountStatus      = raw.AccountStatus;
        }

        await LoadLookupsAsync(ct);
        IsEditing = true;
    }

    public void CancelEdit()
    {
        IsEditing = false;
        SaveError = null;
    }

    public async Task<bool> SaveAsync(CancellationToken ct = default)
    {
        IsSaving  = true;
        SaveError = null;

        var dto = new StaffDto
        {
            Id                 = IsNew ? 0 : (int)StaffId,
            StaffNumber        = EditStaffNumber,
            Title              = EditTitle,
            FirstName          = EditFirstName,
            LastName           = EditLastName,
            PreferredName      = string.IsNullOrWhiteSpace(EditPreferredName) ? null : EditPreferredName,
            Gender             = string.IsNullOrWhiteSpace(EditGender) ? null : EditGender,
            DateOfBirth        = EditDateOfBirth,
            WorkEmail          = EditWorkEmail,
            AltEmail           = string.IsNullOrWhiteSpace(EditAltEmail) ? null : EditAltEmail,
            PhoneHome          = string.IsNullOrWhiteSpace(EditPhoneHome) ? null : EditPhoneHome,
            PhoneMobile        = string.IsNullOrWhiteSpace(EditPhoneMobile) ? null : EditPhoneMobile,
            PhoneEmergency     = string.IsNullOrWhiteSpace(EditPhoneEmergency) ? null : EditPhoneEmergency,
            EmploymentType     = EditEmploymentType,
            ContractType       = EditContractType,
            HireDate           = EditHireDate,
            EndDate            = EditEndDate,
            WorkLocation       = EditWorkLocation,
            JobTitleId         = EditJobTitleId,
            JobGroupId         = EditJobGroupId,
            DepartmentId       = EditDepartmentId,
            ReportingManagerId = EditReportingManagerId,
            AccountStatus      = EditAccountStatus,
        };

        bool ok;
        if (IsNew)
        {
            var (success, err, newId) = await _api.CreateStaffAsync(dto, ct);
            ok = success;
            if (ok)
            {
                StaffId   = newId;
                IsNew     = false;
                IsEditing = false;
                await InitAsync(StaffId, ct);
            }
            else
            {
                SaveError = err;
            }
        }
        else
        {
            var (success, err) = await _api.UpdateStaffAsync(StaffId, dto, ct);
            ok = success;
            if (ok)
            {
                IsEditing = false;
                await InitAsync(StaffId, ct);
            }
            else
            {
                SaveError = err;
            }
        }

        IsSaving = false;
        return ok;
    }

    private async Task LoadLookupsAsync(CancellationToken ct = default)
    {
        if (Departments.Count > 0)
            return;

        var deps = await _api.GetDepartmentsAsync(ct);
        var jts  = await _api.GetJobTitlesAsync(ct);
        Departments = deps.ToList().AsReadOnly();
        JobTitles   = jts.ToList().AsReadOnly();
    }

    private static string BuildStatusBadgeClass(string? status) =>
        status?.Trim().ToLowerInvariant() switch
        {
            "active"    => "upv2-badge--status-active",
            "inactive"  => "upv2-badge--status-inactive",
            "suspended" => "upv2-badge--status-suspended",
            "left"      => "upv2-badge--status-left",
            _            => "upv2-badge--status-default",
        };
}
