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
}
