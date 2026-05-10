/*
===============================================================================
 File        : StudentProfileViewModelV2.cs
 Namespace   : AbsenceApp.Client.ViewModels.V2
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-05-05
 Updated     : 2026-05-05
-------------------------------------------------------------------------------
 Purpose     : ViewModel for StudentProfilePageV2. Provides view-only student
               profile data across 7 tabs, avatar upload capability, and full
               CRUD for the student's absence records.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-05-05  Initial creation.
-------------------------------------------------------------------------------
 Notes       :
   - Register as Scoped in V2ServiceCollectionExtensions.cs.
   - Student profile fields are view-only; no form editing on this page.
   - Tabs: 0=Profile, 1=Contacts, 2=Medical, 3=Flags, 4=Additional, 5=Notes,
           6=Absences (CRUD)
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
    // Active tab (0–6)
    // =========================================================================

    public int ActiveTab { get; private set; } = 0;
    public void SetTab(int index) => ActiveTab = index;

    // =========================================================================
    // Banner header
    // =========================================================================

    public string HeaderFullName       { get; private set; } = string.Empty;
    public string HeaderAdmissionNumber { get; private set; } = string.Empty;
    public string HeaderStatus         { get; private set; } = string.Empty;
    public string HeaderYearGroup      { get; private set; } = string.Empty;
    public string HeaderClass          { get; private set; } = string.Empty;

    // =========================================================================
    // Profile photo
    // =========================================================================

    public byte[]? PhotoBytes { get; private set; }

    public string? PhotoDataUri => PhotoBytes is { Length: > 0 } b
        ? $"data:image/jpeg;base64,{Convert.ToBase64String(b)}"
        : null;

    public bool   IsUploadingPhoto { get; private set; }
    public string? PhotoError      { get; private set; }

    // =========================================================================
    // Tab 0 — Profile (view-only)
    // =========================================================================

    public StudentFullViewDto? Student { get; private set; }

    // =========================================================================
    // Tab 1 — Contacts
    // =========================================================================

    public IReadOnlyList<StudentContactDto> Contacts { get; private set; } = [];

    // =========================================================================
    // Tab 2 — Medical
    // =========================================================================

    public IReadOnlyList<StudentMedicalDto> Medical { get; private set; } = [];

    // =========================================================================
    // Tab 3 — Flags
    // =========================================================================

    public IReadOnlyList<StudentFlagDto> Flags { get; private set; } = [];

    // =========================================================================
    // Tab 6 — Absences (CRUD)
    // =========================================================================

    public IReadOnlyList<AbsenceDto>        Absences        { get; private set; } = [];
    public IReadOnlyList<AbsenceTypeDto>    AbsenceTypes    { get; private set; } = [];
    public IReadOnlyList<AbsenceStatusDto>  AbsenceStatuses { get; private set; } = [];

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
            // Load all data in parallel
            var studentTask   = _api.GetStudentAsync(studentId, ct);
            var contactsTask  = _api.GetContactsAsync(studentId, ct);
            var medicalTask   = _api.GetMedicalAsync(studentId, ct);
            var flagsTask     = _api.GetFlagsAsync(studentId, ct);
            var absencesTask  = _api.GetAbsencesAsync(studentId, ct);
            var typesTask     = _api.GetAbsenceTypesAsync(ct);
            var statusesTask  = _api.GetAbsenceStatusesAsync(ct);

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
                HeaderFullName        = $"{Student.FirstName} {Student.LastName}".Trim();
                HeaderAdmissionNumber = Student.AdmissionNumber ?? string.Empty;
                HeaderStatus          = Student.Status ?? string.Empty;
                HeaderYearGroup       = Student.YearGroupName ?? string.Empty;
                HeaderClass           = Student.ClassName ?? string.Empty;

                // Load stored photo from well-known local path convention
                var photoPath = Path.Combine(FileSystem.AppDataDirectory, "student_photos",
                                              $"student_{StudentId}.jpg");
                if (File.Exists(photoPath))
                    PhotoBytes = await File.ReadAllBytesAsync(photoPath, ct);
            }
            else
            {
                Error = "Student not found.";
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
            using var ms     = new MemoryStream();
            await stream.CopyToAsync(ms, ct);
            PhotoBytes = ms.ToArray();

            // Persist to local file system
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
    // Absence CRUD helpers (used by Tab 6 action buttons)
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
        // Refresh absence list
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

    public bool IsNew      { get; private set; }
    public bool IsEditing  { get; private set; }

    // Reference lookup lists (populated once on first edit request)
    public IReadOnlyList<Core.DTOs.YearGroupDto> YearGroups { get; private set; } = [];
    public IReadOnlyList<Core.DTOs.ClassDto>     Classes    { get; private set; } = [];
    public IReadOnlyList<Core.DTOs.HouseDto>     Houses     { get; private set; } = [];

    // Editable form fields
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

    /// <summary>Initialise for Add (new student) mode.</summary>
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
        ActiveTab             = 0;

        // Reset form to defaults
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

    /// <summary>Start editing an existing (already loaded) student.</summary>
    public async Task BeginEditAsync(CancellationToken ct = default)
    {
        if (IsNew) return; // already in add mode

        // Load raw FK ids
        var raw = await _api.GetStudentRawAsync(StudentId, ct);
        if (raw is not null)
        {
            EditAdmissionNumber = raw.AdmissionNumber;
            EditFirstName       = raw.FirstName;
            EditMiddleNames     = raw.MiddleNames  ?? string.Empty;
            EditLastName        = raw.LastName;
            EditLegalFirstName  = raw.LegalFirstName ?? string.Empty;
            EditLegalLastName   = raw.LegalLastName  ?? string.Empty;
            EditPreferredName   = raw.PreferredName  ?? string.Empty;
            EditGender          = raw.Gender         ?? string.Empty;
            EditDateOfBirth     = raw.DateOfBirth.ToDateTime(TimeOnly.MinValue);
            EditAdmissionDate   = raw.AdmissionDate;
            EditYearGroupId     = raw.YearGroupId;
            EditClassId         = raw.ClassId;
            EditHouseId         = raw.HouseId;
            EditUsername        = raw.Username ?? string.Empty;
            EditUpn             = raw.Upn      ?? string.Empty;
            EditStatus          = raw.Status   ?? "Active";
        }

        await LoadLookupsAsync(ct);
        IsEditing = true;
    }

    /// <summary>Cancel edit — revert to view mode.</summary>
    public void CancelEdit()
    {
        IsEditing = false;
        SaveError = null;
    }

    /// <summary>Save the current form fields (create or update).</summary>
    public async Task<bool> SaveAsync(CancellationToken ct = default)
    {
        IsSaving  = true;
        SaveError = null;

        var dto = new Core.DTOs.StudentDto
        {
            Id              = IsNew ? 0 : StudentId,
            AdmissionNumber = EditAdmissionNumber,
            FirstName       = EditFirstName,
            MiddleNames     = string.IsNullOrWhiteSpace(EditMiddleNames)    ? null : EditMiddleNames,
            LastName        = EditLastName,
            LegalFirstName  = string.IsNullOrWhiteSpace(EditLegalFirstName) ? null : EditLegalFirstName,
            LegalLastName   = string.IsNullOrWhiteSpace(EditLegalLastName)  ? null : EditLegalLastName,
            PreferredName   = string.IsNullOrWhiteSpace(EditPreferredName)  ? null : EditPreferredName,
            Gender          = string.IsNullOrWhiteSpace(EditGender)         ? null : EditGender,
            DateOfBirth     = DateOnly.FromDateTime(EditDateOfBirth),
            AdmissionDate   = EditAdmissionDate,
            YearGroupId     = EditYearGroupId,
            ClassId         = EditClassId,
            HouseId         = EditHouseId,
            Username        = string.IsNullOrWhiteSpace(EditUsername) ? null : EditUsername,
            Upn             = string.IsNullOrWhiteSpace(EditUpn)      ? null : EditUpn,
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
        if (YearGroups.Count > 0) return; // already loaded
        var yg = await _api.GetYearGroupsAsync(ct);
        var cl = await _api.GetClassesAsync(ct);
        var ho = await _api.GetHousesAsync(ct);
        YearGroups = yg.ToList().AsReadOnly();
        Classes    = cl.ToList().AsReadOnly();
        Houses     = ho.ToList().AsReadOnly();
    }
}
