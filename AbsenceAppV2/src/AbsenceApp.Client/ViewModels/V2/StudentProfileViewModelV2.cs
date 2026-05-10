/*
===============================================================================
 File        : StudentProfileViewModelV2.cs
 Namespace   : AbsenceApp.Client.ViewModels.V2
 Author      : Michael
 Version     : 1.2.0
 Created     : 2026-05-05
 Updated     : 2026-05-10
-------------------------------------------------------------------------------
 Purpose     : ViewModel for StudentProfilePageV2. Provides student profile
               data across 6 tabs, avatar upload capability, full CRUD for the
               student's absence records, and shared banner/tab/selector state
               for the unified V2 profile chrome.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-05-05  Initial creation.
   - 1.1.0  2026-05-10  Added shared profile banner fields, tab metadata, and
                         searchable selector state for the unified profile UI.
     - 1.2.0  2026-05-10  Updated runtime tab contract to Basic Info, Contacts,
                                                 Devices, External, Medical, Absences.
-------------------------------------------------------------------------------
 Notes       :
     - Register as Scoped in V2ServiceCollectionExtensions.cs.
     - Tabs: 0=Basic Info, 1=Contacts, 2=Devices, 3=External,
                     4=Medical, 5=Absences (CRUD)
   - Avatar upload is allowed (stored to local AppDataDirectory).
===============================================================================
*/

using AbsenceApp.Client.Services.ApiV2.Modules;
using AbsenceApp.Core.DTOs;
using Microsoft.AspNetCore.Components.Forms;

namespace AbsenceApp.Client.ViewModels.V2;

public sealed class StudentProfileViewModelV2
{
    private readonly StudentProfileApiServiceV2 _api;

    public StudentProfileViewModelV2(StudentProfileApiServiceV2 api) => _api = api;

    // =========================================================================
    // Identity
    // =========================================================================

    public int StudentId { get; private set; }

    // =========================================================================
    // Active tab (0–5)
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
        new() { Index = 2, Label = "Devices",    Icon = "bi-laptop",              Enabled = !IsNew },
        new() { Index = 3, Label = "External",   Icon = "bi-box-arrow-up-right",  Enabled = !IsNew },
        new() { Index = 4, Label = "Medical",    Icon = "bi-heart-pulse",         Enabled = !IsNew },
        new() { Index = 5, Label = "Absences",   Icon = "bi-calendar-x",          Enabled = !IsNew },
    ];

    // =========================================================================
    // Banner header
    // =========================================================================

    public string HeaderFullName        { get; private set; } = string.Empty;
    public string HeaderAdmissionNumber { get; private set; } = string.Empty;
    public string HeaderStatus          { get; private set; } = string.Empty;
    public string HeaderYearGroup       { get; private set; } = string.Empty;
    public string HeaderClass           { get; private set; } = string.Empty;

    public IReadOnlyList<ProfileBannerFieldDto> BannerFields =>
    [
        new() { Label = "Admission No:", Value = HeaderAdmissionNumber },
        new() { Label = "Year Group:",   Value = HeaderYearGroup },
        new() { Label = "Class:",        Value = HeaderClass },
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
            ProfileSelectorItems = await _api.SearchStudentProfileSelectorAsync(null, ct: ct);
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
            ProfileSelectorItems = await _api.SearchStudentProfileSelectorAsync(term, ct: ct);
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
    // Tab 0 — Profile (view-only)
    // =========================================================================

    public StudentFullViewDto? Student { get; private set; }

    // =========================================================================
    // Tab 1 — Contacts
    // =========================================================================

    public IReadOnlyList<StudentContactDto> Contacts { get; private set; } = [];

    // =========================================================================
    // Tab 4 — Medical
    // =========================================================================

    public IReadOnlyList<StudentMedicalDto> Medical { get; private set; } = [];

    // =========================================================================
    // Optional flags data (not surfaced as a dedicated tab in the current
    // runtime contract)
    // =========================================================================

    public IReadOnlyList<StudentFlagDto> Flags { get; private set; } = [];

    // =========================================================================
    // Tab 5 — Absences (CRUD)
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

    public async Task InitAsync(int studentId, CancellationToken ct = default)
    {
        StudentId = studentId;
        IsLoading = true;
        Error     = null;
        Student   = null;

        try
        {
            var studentTask  = _api.GetStudentAsync(studentId, ct);
            var contactsTask = _api.GetContactsAsync(studentId, ct);
            var medicalTask  = _api.GetMedicalAsync(studentId, ct);
            var flagsTask    = _api.GetFlagsAsync(studentId, ct);
            var absencesTask = _api.GetAbsencesAsync(studentId, ct);
            var typesTask    = _api.GetAbsenceTypesAsync(ct);
            var statusesTask = _api.GetAbsenceStatusesAsync(ct);

            await Task.WhenAll(studentTask, contactsTask, medicalTask,
                               flagsTask, absencesTask, typesTask, statusesTask);

            Student         = await studentTask;
            Contacts        = (await contactsTask).ToList().AsReadOnly();
            Medical         = (await medicalTask).ToList().AsReadOnly();
            Flags           = (await flagsTask).ToList().AsReadOnly();
            Absences        = (await absencesTask).ToList().AsReadOnly();
            AbsenceTypes    = (await typesTask).ToList().AsReadOnly();
            AbsenceStatuses = (await statusesTask).ToList().AsReadOnly();

            if (Student is not null)
            {
                HeaderFullName        = Student.FullName;
                HeaderAdmissionNumber = Student.AdmissionNumber ?? string.Empty;
                HeaderStatus          = Student.Status ?? string.Empty;
                HeaderYearGroup       = Student.YearGroupName ?? string.Empty;
                HeaderClass           = Student.ClassName ?? string.Empty;
                SelectorSearchText    = HeaderFullName;

                var photoPath = Path.Combine(FileSystem.AppDataDirectory, "student_photos",
                                             $"student_{StudentId}.jpg");
                PhotoBytes = File.Exists(photoPath)
                    ? await File.ReadAllBytesAsync(photoPath, ct)
                    : null;

                await RefreshProfileSelectorAsync(ct);
            }
            else
            {
                Error = "Student not found.";
                ProfileSelectorItems = [];
                SelectorSearchText = string.Empty;
            }
        }
        catch (Exception ex)
        {
            Error = $"Failed to load student profile: {ex.Message}";
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

            var dir  = Path.Combine(FileSystem.AppDataDirectory, "student_photos");
            Directory.CreateDirectory(dir);
            var ext  = Path.GetExtension(file.Name).ToLowerInvariant();
            var path = Path.Combine(dir, $"student_{StudentId}{ext}");
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
    // Absence CRUD helpers (used by Tab 5 action buttons)
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

        var fresh = await _api.GetAbsencesAsync(StudentId, ct);
        Absences = fresh.ToList().AsReadOnly();
    }

    public async Task RefreshAbsencesAsync(CancellationToken ct = default)
    {
        var fresh = await _api.GetAbsencesAsync(StudentId, ct);
        Absences = fresh.ToList().AsReadOnly();
    }

    // =========================================================================
    // Edit / Add mode
    // =========================================================================

    public bool IsNew     { get; private set; }
    public bool IsEditing { get; private set; }

    public IReadOnlyList<YearGroupDto> YearGroups { get; private set; } = [];
    public IReadOnlyList<ClassDto>     Classes    { get; private set; } = [];
    public IReadOnlyList<HouseDto>     Houses     { get; private set; } = [];

    public string   EditAdmissionNumber { get; set; } = string.Empty;
    public string   EditFirstName       { get; set; } = string.Empty;
    public string   EditMiddleNames     { get; set; } = string.Empty;
    public string   EditLastName        { get; set; } = string.Empty;
    public string   EditLegalFirstName  { get; set; } = string.Empty;
    public string   EditLegalLastName   { get; set; } = string.Empty;
    public string   EditPreferredName   { get; set; } = string.Empty;
    public string   EditGender          { get; set; } = string.Empty;
    public DateTime EditDateOfBirth     { get; set; } = DateTime.Today.AddYears(-10);
    public DateOnly EditAdmissionDate   { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public int      EditYearGroupId     { get; set; }
    public int?     EditClassId         { get; set; }
    public int?     EditHouseId         { get; set; }
    public string   EditUsername        { get; set; } = string.Empty;
    public string   EditUpn             { get; set; } = string.Empty;
    public string   EditStatus          { get; set; } = "Active";

    public bool    IsSaving  { get; private set; }
    public string? SaveError { get; private set; }

    public async Task InitNewAsync(CancellationToken ct = default)
    {
        IsNew     = true;
        IsEditing = true;
        StudentId = 0;
        Student   = null;
        Contacts  = [];
        Medical   = [];
        Flags     = [];
        Absences  = [];
        HeaderFullName        = "New Student";
        HeaderAdmissionNumber = string.Empty;
        HeaderStatus          = string.Empty;
        HeaderYearGroup       = string.Empty;
        HeaderClass           = string.Empty;
        PhotoBytes            = null;
        PhotoError            = null;
        ActiveTab             = 0;
        SelectorSearchText    = string.Empty;
        ProfileSelectorItems  = [];

        EditAdmissionNumber = string.Empty;
        EditFirstName       = string.Empty;
        EditMiddleNames     = string.Empty;
        EditLastName        = string.Empty;
        EditLegalFirstName  = string.Empty;
        EditLegalLastName   = string.Empty;
        EditPreferredName   = string.Empty;
        EditGender          = string.Empty;
        EditDateOfBirth     = DateTime.Today.AddYears(-10);
        EditAdmissionDate   = DateOnly.FromDateTime(DateTime.Today);
        EditYearGroupId     = 0;
        EditClassId         = null;
        EditHouseId         = null;
        EditUsername        = string.Empty;
        EditUpn             = string.Empty;
        EditStatus          = "Active";

        await LoadLookupsAsync(ct);
    }

    public async Task BeginEditAsync(CancellationToken ct = default)
    {
        if (IsNew)
            return;

        var raw = await _api.GetStudentRawAsync(StudentId, ct);
        if (raw is not null)
        {
            EditAdmissionNumber = raw.AdmissionNumber;
            EditFirstName       = raw.FirstName;
            EditMiddleNames     = raw.MiddleNames ?? string.Empty;
            EditLastName        = raw.LastName;
            EditLegalFirstName  = raw.LegalFirstName ?? string.Empty;
            EditLegalLastName   = raw.LegalLastName ?? string.Empty;
            EditPreferredName   = raw.PreferredName ?? string.Empty;
            EditGender          = raw.Gender ?? string.Empty;
            EditDateOfBirth     = raw.DateOfBirth.ToDateTime(TimeOnly.MinValue);
            EditAdmissionDate   = raw.AdmissionDate;
            EditYearGroupId     = raw.YearGroupId;
            EditClassId         = raw.ClassId;
            EditHouseId         = raw.HouseId;
            EditUsername        = raw.Username ?? string.Empty;
            EditUpn             = raw.Upn ?? string.Empty;
            EditStatus          = raw.Status ?? "Active";
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

        var dto = new StudentDto
        {
            Id              = IsNew ? 0 : StudentId,
            AdmissionNumber = EditAdmissionNumber,
            FirstName       = EditFirstName,
            MiddleNames     = string.IsNullOrWhiteSpace(EditMiddleNames) ? null : EditMiddleNames,
            LastName        = EditLastName,
            LegalFirstName  = string.IsNullOrWhiteSpace(EditLegalFirstName) ? null : EditLegalFirstName,
            LegalLastName   = string.IsNullOrWhiteSpace(EditLegalLastName) ? null : EditLegalLastName,
            PreferredName   = string.IsNullOrWhiteSpace(EditPreferredName) ? null : EditPreferredName,
            Gender          = string.IsNullOrWhiteSpace(EditGender) ? null : EditGender,
            DateOfBirth     = DateOnly.FromDateTime(EditDateOfBirth),
            AdmissionDate   = EditAdmissionDate,
            YearGroupId     = EditYearGroupId,
            ClassId         = EditClassId,
            HouseId         = EditHouseId,
            Username        = string.IsNullOrWhiteSpace(EditUsername) ? null : EditUsername,
            Upn             = string.IsNullOrWhiteSpace(EditUpn) ? null : EditUpn,
            Status          = EditStatus,
        };

        bool ok;
        if (IsNew)
        {
            var (success, err, newId) = await _api.CreateStudentAsync(dto, ct);
            ok = success;
            if (ok)
            {
                StudentId = newId;
                IsNew     = false;
                IsEditing = false;
                await InitAsync(StudentId, ct);
            }
            else
            {
                SaveError = err;
            }
        }
        else
        {
            var (success, err) = await _api.UpdateStudentAsync(StudentId, dto, ct);
            ok = success;
            if (ok)
            {
                IsEditing = false;
                await InitAsync(StudentId, ct);
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
        if (YearGroups.Count > 0)
            return;

        var yg = await _api.GetYearGroupsAsync(ct);
        var cl = await _api.GetClassesAsync(ct);
        var ho = await _api.GetHousesAsync(ct);
        YearGroups = yg.ToList().AsReadOnly();
        Classes    = cl.ToList().AsReadOnly();
        Houses     = ho.ToList().AsReadOnly();
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
