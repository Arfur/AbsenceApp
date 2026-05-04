/*
===============================================================================
 File        : UserProfileViewModelV2.cs
 Namespace   : AbsenceApp.Client.ViewModels.V2
 Author      : Michael
 Version     : 1.3.0
   Created     : 2026-04-21
     Updated     : 2026-05-04
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
   - 1.1.0  2026-04-22  Session 6: Added StaffWithoutAccounts, SelectLinkedStaffAsync.
                         Session 7 Phase 3: Added RemovePhotoAsync.
    - 1.2.0  2026-04-25  Edit-mode profile loading now uses one unified DTO and
                                 one API call: UserProfileFullDetailDto from
                                 GetUserProfileDetailAsync. All tab sections are
                                 populated from this single payload.
    - 1.3.0  2026-05-04  Amendment B/C: Added UsersWithAccounts property;
                         changed InitNewAsync signature to long? preselectedStaffId;
                         InitEditAsync now loads UsersWithAccounts via
                         GetUsersForSelectAsync; added RefreshDropdownsAsync().
                         Amendment A: HeaderLastLogin already exists and is set
                         in InitEditAsync — now surfaced in banner far-right section.
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
    public int  UserId   { get; private set; }
    public int  StaffId  { get; private set; }

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
    public int   RoleTypeId { get; set; }
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
    public DateTime DateOfBirth { get; set; } = DateTime.Today;

    public string?  Bio          { get; set; }
    public string?  Gender       { get; set; }
    public string   Timezone     { get; set; } = "UTC";
    public string   LanguageCode { get; set; } = "en";
    public int     DepartmentId { get; set; }
    public int     JobTitleId   { get; set; }

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
    // Add-User staff list (populated when no staff is pre-selected)
    // =========================================================================

    public IReadOnlyList<StaffSelectDto> StaffWithoutAccounts { get; private set; } = [];

    // =========================================================================
    // Edit-User — users with accounts (for banner navigation dropdown)
    // =========================================================================

    public IReadOnlyList<UserSelectDto> UsersWithAccounts { get; private set; } = [];

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

    public async Task InitNewAsync(long? preselectedStaffId = null, CancellationToken ct = default)
    {
        IsNew    = true;
        StaffId  = (int)(preselectedStaffId ?? 0L);
        IsLoading = true;
        Error    = null;
        try
        {
            RoleTypes   = await _svc.GetRoleTypesAsync(ct);
            Permissions = (await _svc.GetUserPermissionsAsync(0, ct)).ToList();

            if (preselectedStaffId.HasValue && preselectedStaffId.Value > 0)
            {
                // Pre-selected staff (navigated from StaffDetailPage).
                LinkedStaff = await _svc.GetStaffForUserCreateAsync((int)preselectedStaffId.Value, ct);
                if (LinkedStaff is null)
                {
                    Error = $"Staff record {preselectedStaffId.Value} not found.";
                    return;
                }
                Email          = LinkedStaff.WorkEmail;
                HeaderFullName = LinkedStaff.FullName;
            }
            else
            {
                // No pre-selection — load the staff-without-accounts dropdown.
                StaffWithoutAccounts = await _svc.GetStaffWithoutUsersAsync(ct);
            }
        }
        catch (Exception ex) { Error = ex.Message; }
        finally { IsLoading = false; }
    }

    // =========================================================================
    // Select linked staff from dropdown (Add User mode, no pre-selection)
    // =========================================================================

    public async Task SelectLinkedStaffAsync(int staffId, CancellationToken ct = default)
    {
        if (staffId <= 0)
        {
            LinkedStaff = null;
            StaffId     = 0;
            Email       = string.Empty;
            return;
        }

        Error = null;
        try
        {
            StaffId     = staffId;
            LinkedStaff = await _svc.GetStaffForUserCreateAsync(staffId, ct);
            if (LinkedStaff is not null)
                Email = LinkedStaff.WorkEmail;
        }
        catch (Exception ex) { Error = ex.Message; }
    }

    // =========================================================================
    // Initialise — Edit User (single unified API call)
    // =========================================================================

    public async Task InitEditAsync(long userId, CancellationToken ct = default)
    {
        IsNew     = false;
        UserId    = (int)userId;
        IsLoading = true;
        Error     = null;
        try
        {
            var full = await _svc.GetUserProfileDetailAsync(UserId, ct);

            if (!full.UserExists)
            {
                Error = "User not found.";
                return;
            }

            RoleTypes   = full.RoleTypes;
            Permissions = full.Permissions.ToList();

            // Load users-with-accounts for Edit Mode navigation dropdown.
            UsersWithAccounts = await _svc.GetUsersForSelectAsync(ct);

            // Populate header.
            StaffId           = full.StaffId ?? 0;
            HeaderFullName    = full.FullName;
            HeaderEmail       = full.Email;
            HeaderRoleName    = full.RoleDisplayName;
            HeaderStatus      = full.Status;
            HeaderIsAdmin     = full.IsAdmin;
            HeaderLastLogin   = full.LastLoginAt;
            HeaderCreatedAt   = full.UserCreatedAt;
            PhotoStoredPath   = full.ProfilePictureUrl;

            // Load stored photo bytes.
            if (!string.IsNullOrWhiteSpace(PhotoStoredPath) && File.Exists(PhotoStoredPath))
                PhotoBytes = await File.ReadAllBytesAsync(PhotoStoredPath, ct);

            // Populate Basic User Info fields.
            Username      = full.Username;
            Email         = full.Email;
            Status        = full.Status;
            IsAdmin       = full.IsAdmin;
            UserCreatedAt = full.UserCreatedAt;
            RoleTypeId    = full.RoleTypeId;

            // Populate UserProfile fields.
            if (full.ProfileExists)
            {
                FirstName     = full.FirstName;
                LastName      = full.LastName;
                PreferredName = full.PreferredName;
                Title         = full.Title;
                Bio           = full.Bio;
                Gender        = full.Gender;
                Timezone      = full.Timezone;
                LanguageCode  = full.LanguageCode;
                DepartmentId  = full.DepartmentId;
                JobTitleId    = full.JobTitleId;
            }

            DateOfBirth     = full.DateOfBirth;
            Contact         = full.Contact;
            Classes         = full.Classes;
            Devices         = full.StaffDevices;
            ExternalAccounts = full.StaffExternalAccounts;
            Absences        = full.StaffAbsences;
            LoginAudit      = full.StaffLoginAudit;
        }
        catch (Exception ex) { Error = ex.Message; }
        finally { IsLoading = false; }
    }

    // =========================================================================
    // Refresh both dropdown lists (called after CreateUserAsync succeeds)
    // =========================================================================

    public async Task RefreshDropdownsAsync(CancellationToken ct = default)
    {
        try
        {
            StaffWithoutAccounts = await _svc.GetStaffWithoutUsersAsync(ct);
            UsersWithAccounts    = await _svc.GetUsersForSelectAsync(ct);
        }
        catch (Exception ex) { Error = ex.Message; }
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
                IsAdmin       = IsAdmin,
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

    // =========================================================================
    // Photo removal
    // =========================================================================

    public async Task RemovePhotoAsync(CancellationToken ct = default)
    {
        PhotoError       = null;
        IsUploadingPhoto = true;
        try
        {
            // Delete local file if it exists.
            if (!string.IsNullOrWhiteSpace(PhotoStoredPath) && File.Exists(PhotoStoredPath))
            {
                try { File.Delete(PhotoStoredPath); } catch { /* best-effort */ }
            }

            // Clear in-memory state.
            PhotoBytes      = null;
            PhotoStoredPath = null;

            // Clear path in DB.
            if (UserId > 0)
            {
                var (ok, err) = await _svc.UpdateProfilePhotoAsync(UserId, string.Empty, ct);
                if (!ok) PhotoError = $"Photo removed locally but DB update failed: {err}";
            }
        }
        catch (Exception ex) { PhotoError = ex.Message; }
        finally { IsUploadingPhoto = false; }
    }
}
