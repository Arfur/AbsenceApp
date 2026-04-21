/*
===============================================================================
 File        : UserProfileViewModelV2.cs
 Namespace   : AbsenceApp.Client.ViewModels.V2
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-04-21
 Updated     : 2026-04-21
-------------------------------------------------------------------------------
 Purpose     : ViewModel for the full User Profile / Add User page
               (UserFormPageV2.razor). Handles both Add User and Edit User
               modes, including:
                 - Profile header data (photo, name, role, status)
                 - Basic User Info tab (User + UserProfile fields)
                 - Contacts tab (Staff employment / contact data)
                 - Classes & Year Groups tab (StaffAssignments)
                 - Devices tab (StaffDevices)
                 - External Systems tab (StaffExternalAccounts)
                 - Medical Records tab (placeholder — no staff_medical table)
                 - Absence Records tab (Absences where PersonType=Staff)
                 - Login Audit tab (LoginAudit rows)
                 - Profile photo upload (local MAUI file system)
                 - Change password
                 - Module permissions matrix
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-21  Initial creation.
-------------------------------------------------------------------------------
 Notes       :
   - Register as Scoped in V2ServiceCollectionExtensions.cs.
   - Photo bytes are kept in memory (PhotoBytes) and stored to
     FileSystem.AppDataDirectory/user_photos/user_{id}.ext on save.
   - The ProfilePictureUrl in the DB stores the local file path.
   - Tabs: 0=Basic Info, 1=Contacts, 2=Classes, 3=Devices,
           4=External, 5=Medical, 6=Absences, 7=Login Audit
===============================================================================
*/

using AbsenceApp.Client.Services.ApiV2.Modules;
using AbsenceApp.Core.DTOs;
using Microsoft.AspNetCore.Components.Forms;

namespace AbsenceApp.Client.ViewModels.V2;

public sealed class UserProfileViewModelV2
{
    // =========================================================================
    // Construction
    // =========================================================================

    private readonly UserManagementApiServiceV2 _svc;

    public UserProfileViewModelV2(UserManagementApiServiceV2 svc) => _svc = svc;

    // =========================================================================
    // Mode
    // =========================================================================

    public bool IsNew    { get; private set; } = true;
    public long UserId   { get; private set; }
    public long StaffId  { get; private set; }

    // =========================================================================
    // Active tab (0–7)
    // =========================================================================

    public int  ActiveTab    { get; private set; } = 0;

    public void SetTab(int index) => ActiveTab = index;

    // =========================================================================
    // Profile Header (edit mode)
    // =========================================================================

    public string  HeaderFullName    { get; private set; } = string.Empty;
    public string  HeaderEmail       { get; private set; } = string.Empty;
    public string  HeaderRoleName    { get; private set; } = string.Empty;
    public string  HeaderStatus      { get; private set; } = string.Empty;
    public bool    HeaderIsAdmin     { get; private set; }
    public DateTime? HeaderLastLogin { get; private set; }
    public DateTime  HeaderCreatedAt { get; private set; }

    // =========================================================================
    // Profile photo
    // =========================================================================

    /// <summary>
    /// Bytes of the currently displayed photo. Set either from loading the
    /// saved file or from a newly picked file before upload.
    /// </summary>
    public byte[]? PhotoBytes { get; private set; }

    /// <summary>
    /// Local file path stored in UserProfile.ProfilePictureUrl.
    /// </summary>
    public string? PhotoStoredPath { get; private set; }

    /// <summary>
    /// Base64 data-URI for use in an &lt;img src="..."&gt; tag.
    /// Returns null when no photo is available.
    /// </summary>
    public string? PhotoDataUri => PhotoBytes is { Length: > 0 } b
        ? $"data:image/jpeg;base64,{Convert.ToBase64String(b)}"
        : null;

    public bool IsUploadingPhoto { get; private set; }
    public string? PhotoError    { get; private set; }

    // =========================================================================
    // Tab 0 — Basic User Info (User fields)
    // =========================================================================

    public string Username   { get; set; } = string.Empty;
    public string Email      { get; set; } = string.Empty;
    public long   RoleTypeId { get; set; }
    public string Status     { get; set; } = "active";
    public bool   IsAdmin    { get; set; }
    public DateTime UserCreatedAt { get; private set; }

    // =========================================================================
    // Tab 0 — Basic User Info (UserProfile fields)
    // =========================================================================

    public string  FirstName     { get; set; } = string.Empty;
    public string  LastName      { get; set; } = string.Empty;
    public string? PreferredName { get; set; }
    public string  Title         { get; set; } = string.Empty;
    public DateOnly DateOfBirth  { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public string?  Bio          { get; set; }
    public string?  Gender       { get; set; }
    public string   Timezone     { get; set; } = "UTC";
    public string   LanguageCode { get; set; } = "en";
    public long     DepartmentId { get; set; }
    public long     JobTitleId   { get; set; }

    // =========================================================================
    // Add User — password (create mode only)
    // =========================================================================

    public string Password { get; set; } = string.Empty;

    // =========================================================================
    // Change Password panel (edit mode)
    // =========================================================================

    public string OldPassword      { get; set; } = string.Empty;
    public string NewPassword      { get; set; } = string.Empty;
    public bool   IsChangingPw     { get; private set; }
    public string? PwError         { get; private set; }
    public string? PwSuccess       { get; private set; }

    // =========================================================================
    // Tab 1 — Contacts
    // =========================================================================

    public StaffContactDto? Contact { get; private set; }

    // =========================================================================
    // Tab 2 — Classes & Year Groups
    // =========================================================================

    public IReadOnlyList<StaffClassRowDto> Classes { get; private set; } = [];

    // =========================================================================
    // Tab 3 — Devices
    // =========================================================================

    public IReadOnlyList<StaffDeviceRowDto> Devices { get; private set; } = [];

    // =========================================================================
    // Tab 4 — External Systems
    // =========================================================================

    public IReadOnlyList<StaffExternalRowDto> ExternalAccounts { get; private set; } = [];

    // =========================================================================
    // Tab 6 — Absence Records
    // =========================================================================

    public IReadOnlyList<StaffAbsenceRowDto> Absences { get; private set; } = [];

    // =========================================================================
    // Tab 7 — Login Audit
    // =========================================================================

    public IReadOnlyList<LoginAuditRowDto> LoginAudit { get; private set; } = [];

    // =========================================================================
    // Reference data
    // =========================================================================

    public IReadOnlyList<RoleTypeSelectDto> RoleTypes   { get; private set; } = [];
    public List<PagePermissionDto>          Permissions { get; private set; } = [];

    // =========================================================================
    // Add-User linked staff
    // =========================================================================

    public StaffSelectDto? LinkedStaff { get; private set; }

    // =========================================================================
    // State
    // =========================================================================

    public bool    IsLoading        { get; private set; }
    public bool    IsSaving         { get; private set; }
    public string? Error            { get; private set; }
    public string? Success          { get; private set; }

    public void SetError(string message) => Error = message;

    // =========================================================================
    // Initialise — Add User
    // =========================================================================

    public async Task InitNewAsync(long staffId, CancellationToken ct = default)
    {
        IsNew    = true;
        StaffId  = staffId;
        IsLoading = true;
        Error    = null;
        try
        {
            RoleTypes   = await _svc.GetRoleTypesAsync(ct);
            LinkedStaff = await _svc.GetStaffForUserCreateAsync(staffId, ct);
            if (LinkedStaff is null)
            {
                Error = $"Staff record {staffId} not found.";
                return;
            }

            // Pre-fill from staff.
            Email       = LinkedStaff.WorkEmail;
            FirstName   = string.Empty;
            LastName    = string.Empty;
            HeaderFullName = LinkedStaff.FullName;

            Permissions = (await _svc.GetUserPermissionsAsync(0, ct)).ToList();
        }
        catch (Exception ex) { Error = ex.Message; }
        finally { IsLoading = false; }
    }

    // =========================================================================
    // Initialise — Edit User (loads all tabs in parallel)
    // =========================================================================

    public async Task InitEditAsync(long userId, CancellationToken ct = default)
    {
        IsNew     = false;
        UserId    = userId;
        IsLoading = true;
        Error     = null;
        try
        {
            // Load reference data and header in parallel.
            var roleTypesTask = _svc.GetRoleTypesAsync(ct);
            var headerTask    = _svc.GetUserProfileHeaderAsync(userId, ct);
            var detailTask    = _svc.GetUserProfileDetailAsync(userId, ct);
            var permsTask     = _svc.GetUserPermissionsAsync(userId, ct);

            await Task.WhenAll(roleTypesTask, headerTask, detailTask, permsTask);

            RoleTypes   = await roleTypesTask;
            var header  = await headerTask;
            var detail  = await detailTask;
            Permissions = (await permsTask).ToList();

            if (header is null)
            {
                Error = "User not found.";
                return;
            }

            // Populate header.
            StaffId           = header.StaffId ?? 0;
            HeaderFullName    = header.FullName;
            HeaderEmail       = header.Email;
            HeaderRoleName    = header.RoleName;
            HeaderStatus      = header.Status;
            HeaderIsAdmin     = header.IsAdmin;
            HeaderLastLogin   = header.LastLoginAt;
            HeaderCreatedAt   = header.CreatedAt;
            PhotoStoredPath   = header.ProfilePictureUrl;

            // Load stored photo bytes.
            if (!string.IsNullOrWhiteSpace(PhotoStoredPath) && File.Exists(PhotoStoredPath))
                PhotoBytes = await File.ReadAllBytesAsync(PhotoStoredPath, ct);

            // Populate Basic User Info fields.
            Username     = header.Username;
            Email        = header.Email;
            Status       = header.Status;
            IsAdmin      = header.IsAdmin;
            UserCreatedAt = header.CreatedAt;

            // Role — find matching RoleTypeId by DisplayName
            var matchRole = RoleTypes.FirstOrDefault(r => r.DisplayName == header.RoleName
                                                       || r.Name        == header.RoleName);
            if (matchRole is not null) RoleTypeId = matchRole.Id;

            // Populate UserProfile fields.
            if (detail.ProfileExists)
            {
                FirstName     = detail.FirstName;
                LastName      = detail.LastName;
                PreferredName = detail.PreferredName;
                Title         = detail.Title;
                DateOfBirth   = detail.DateOfBirth;
                Bio           = detail.Bio;
                Gender        = detail.Gender;
                Timezone      = detail.Timezone;
                LanguageCode  = detail.LanguageCode;
                DepartmentId  = detail.DepartmentId;
                JobTitleId    = detail.JobTitleId;
            }

            // Load tab data (Staff-linked tabs).
            if (StaffId > 0)
            {
                var contactTask   = _svc.GetStaffContactAsync(StaffId, ct);
                var classTask     = _svc.GetStaffClassAssignmentsAsync(StaffId, ct);
                var deviceTask    = _svc.GetStaffDevicesAsync(StaffId, ct);
                var externalTask  = _svc.GetStaffExternalAccountsAsync(StaffId, ct);
                var absenceTask   = _svc.GetStaffAbsencesAsync(StaffId, ct);
                var auditTask     = _svc.GetUserLoginAuditAsync(userId, ct);

                await Task.WhenAll(contactTask, classTask, deviceTask, externalTask, absenceTask, auditTask);

                Contact          = await contactTask;
                Classes          = await classTask;
                Devices          = await deviceTask;
                ExternalAccounts = await externalTask;
                Absences         = await absenceTask;
                LoginAudit       = await auditTask;
            }
            else
            {
                LoginAudit = await _svc.GetUserLoginAuditAsync(userId, ct);
            }
        }
        catch (Exception ex) { Error = ex.Message; }
        finally { IsLoading = false; }
    }

    // =========================================================================
    // Save — Add User
    // =========================================================================

    public async Task<(bool Ok, long NewUserId)> CreateUserAsync(CancellationToken ct = default)
    {
        Error   = null;
        Success = null;
        IsSaving = true;
        try
        {
            var (ok, err, id) = await _svc.CreateUserAsync(new UserCreateDto
            {
                StaffId    = StaffId,
                Username   = Username.Trim(),
                Email      = Email.Trim(),
                Password   = Password,
                RoleTypeId = RoleTypeId,
            }, ct);

            if (!ok) { Error = err ?? "Unknown error."; return (false, 0); }

            // Save permission matrix.
            var (permOk, permErr) = await _svc.SaveUserPermissionsAsync(id, Permissions, ct);
            if (!permOk) { Error = permErr ?? "Permissions save failed."; return (false, 0); }

            Success = "User account created successfully.";
            return (true, id);
        }
        catch (Exception ex) { Error = ex.Message; return (false, 0); }
        finally { IsSaving = false; }
    }

    // =========================================================================
    // Save — Edit User Profile (Tab 0 save)
    // =========================================================================

    public async Task<bool> SaveProfileAsync(CancellationToken ct = default)
    {
        Error   = null;
        Success = null;
        IsSaving = true;
        try
        {
            var (ok, err) = await _svc.SaveUserProfileAsync(new UserProfileSaveDto
            {
                UserId        = UserId,
                Username      = Username.Trim(),
                Email         = Email.Trim(),
                RoleTypeId    = RoleTypeId,
                Status        = Status,
                FirstName     = FirstName,
                LastName      = LastName,
                PreferredName = PreferredName,
                Title         = Title,
                DateOfBirth   = DateOfBirth,
                Bio           = Bio,
                Gender        = Gender,
                Timezone      = Timezone,
                LanguageCode  = LanguageCode,
                DepartmentId  = DepartmentId,
                JobTitleId    = JobTitleId,
            }, ct);

            if (!ok) { Error = err ?? "Unknown error."; return false; }

            // Save permission matrix.
            var (permOk, permErr) = await _svc.SaveUserPermissionsAsync(UserId, Permissions, ct);
            if (!permOk) { Error = permErr ?? "Permissions save failed."; return false; }

            // Refresh header display values.
            HeaderEmail    = Email;
            HeaderStatus   = Status;
            HeaderFullName = $"{FirstName} {LastName}".Trim();
            var matchRole  = RoleTypes.FirstOrDefault(r => r.Id == RoleTypeId);
            if (matchRole is not null) HeaderRoleName = matchRole.DisplayName;

            Success = "Profile saved successfully.";
            return true;
        }
        catch (Exception ex) { Error = ex.Message; return false; }
        finally { IsSaving = false; }
    }

    // =========================================================================
    // Change Password
    // =========================================================================

    public async Task ChangePasswordAsync(CancellationToken ct = default)
    {
        PwError   = null;
        PwSuccess = null;

        if (string.IsNullOrWhiteSpace(OldPassword) || string.IsNullOrWhiteSpace(NewPassword))
        {
            PwError = "Both current and new password are required.";
            return;
        }
        if (NewPassword.Length < 8)
        {
            PwError = "New password must be at least 8 characters.";
            return;
        }

        IsChangingPw = true;
        try
        {
            var (ok, err) = await _svc.ChangePasswordAsync(new ChangePasswordDto
            {
                UserId      = UserId,
                OldPassword = OldPassword,
                NewPassword = NewPassword,
            }, ct);

            if (!ok) { PwError = err ?? "Password change failed."; return; }

            OldPassword = string.Empty;
            NewPassword = string.Empty;
            PwSuccess   = "Password changed successfully.";
        }
        catch (Exception ex) { PwError = ex.Message; }
        finally { IsChangingPw = false; }
    }

    // =========================================================================
    // Photo upload
    // =========================================================================

    /// <summary>
    /// Reads the picked file, stores bytes in memory, saves to the local MAUI
    /// app-data folder, and persists the path to the DB.
    /// Max size: 5 MB.
    /// </summary>
    public async Task UploadPhotoAsync(IBrowserFile file, CancellationToken ct = default)
    {
        PhotoError       = null;
        IsUploadingPhoto = true;

        try
        {
            const long MaxSize = 5 * 1024 * 1024; // 5 MB

            if (file.Size > MaxSize)
            {
                PhotoError = "Photo must be under 5 MB.";
                return;
            }

            // Read bytes into memory.
            await using var ms = new MemoryStream();
            await using var stream = file.OpenReadStream(MaxSize, ct);
            await stream.CopyToAsync(ms, ct);
            var bytes = ms.ToArray();

            // Determine extension (default to jpg).
            var ext = Path.GetExtension(file.Name).ToLowerInvariant() switch
            {
                ".png"  => ".png",
                ".gif"  => ".gif",
                ".webp" => ".webp",
                _       => ".jpg",
            };

            // Save to local MAUI app-data folder.
            var dir  = Path.Combine(FileSystem.AppDataDirectory, "user_photos");
            Directory.CreateDirectory(dir);
            var path = Path.Combine(dir, $"user_{UserId}{ext}");
            await File.WriteAllBytesAsync(path, bytes, ct);

            // Update in-memory state.
            PhotoBytes      = bytes;
            PhotoStoredPath = path;

            // Persist path to DB (non-blocking — best-effort).
            if (UserId > 0)
            {
                var (ok, err) = await _svc.UpdateProfilePhotoAsync(UserId, path, ct);
                if (!ok) PhotoError = $"Photo saved locally but DB update failed: {err}";
            }
        }
        catch (Exception ex) { PhotoError = ex.Message; }
        finally { IsUploadingPhoto = false; }
    }
}
