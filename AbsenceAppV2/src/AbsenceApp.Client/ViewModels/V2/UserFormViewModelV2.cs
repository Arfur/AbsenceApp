/*
===============================================================================
 File        : UserFormViewModelV2.cs
 Namespace   : AbsenceApp.Client.ViewModels.V2
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-04-11
 Updated     : 2026-04-11
-------------------------------------------------------------------------------
 Purpose     : ViewModel shared by AddUserPageV2 and EditUserPageV2.
               Manages form field binding, role type dropdown data, and
               the complete per-page permission matrix for the user.
               Delegates persistence to UserManagementApiServiceV2.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-11  Initial creation (E15 User Management).
-------------------------------------------------------------------------------
 Notes       :
   - Register as Scoped in V2ServiceCollectionExtensions.cs.
   - IsNew=true → CreateUserAsync; IsNew=false → UpdateUserAsync.
   - Permissions are saved via SaveUserPermissionsAsync after the user
     record save.
===============================================================================
*/

using AbsenceApp.Client.Services.ApiV2.Modules;
using AbsenceApp.Core.DTOs;

namespace AbsenceApp.Client.ViewModels.V2;

public sealed class UserFormViewModelV2
{
    // =========================================================================
    // Construction
    // =========================================================================

    private readonly UserManagementApiServiceV2 _svc;

    public UserFormViewModelV2(UserManagementApiServiceV2 svc) => _svc = svc;

    // =========================================================================
    // Mode
    // =========================================================================

    public bool IsNew { get; private set; } = true;

    // =========================================================================
    // Form fields
    // =========================================================================

    public long    UserId       { get; set; }
    public string  FirstName    { get; set; } = string.Empty;
    public string  LastName     { get; set; } = string.Empty;
    public string  Username     { get; set; } = string.Empty;
    public string  Email        { get; set; } = string.Empty;
    public string? PhoneNumber  { get; set; }
    public string  Password     { get; set; } = string.Empty;
    public long    RoleTypeId   { get; set; }
    public string  Status       { get; set; } = "active";

    // =========================================================================
    // Reference data
    // =========================================================================

    public IReadOnlyList<RoleTypeSelectDto> RoleTypes  { get; private set; } = [];

    // =========================================================================
    // Permission matrix rows (one per active AppPage)
    // =========================================================================

    public List<PagePermissionDto> Permissions { get; private set; } = [];

    // =========================================================================
    // State
    // =========================================================================

    public bool   IsLoading { get; private set; }
    public bool   IsSaving  { get; private set; }
    public string? Error    { get; private set; }
    public string? Success  { get; private set; }

    // =========================================================================
    // Initialise for Add
    // =========================================================================

    public async Task InitNewAsync(CancellationToken ct = default)
    {
        IsNew    = true;
        IsLoading = true;
        Error    = null;
        try
        {
            RoleTypes   = await _svc.GetRoleTypesAsync(ct);
            Permissions = (await _svc.GetUserPermissionsAsync(0, ct)).ToList(); // all-false defaults
        }
        catch (Exception ex) { Error = ex.Message; }
        finally { IsLoading = false; }
    }

    // =========================================================================
    // Initialise for Edit
    // =========================================================================

    public async Task InitEditAsync(long userId, CancellationToken ct = default)
    {
        IsNew     = false;
        IsLoading = true;
        Error     = null;
        try
        {
            RoleTypes = await _svc.GetRoleTypesAsync(ct);

            var dto = await _svc.GetUserForEditAsync(userId, ct);
            if (dto is null)
            {
                Error = "User not found.";
                return;
            }

            UserId      = dto.Id;
            FirstName   = dto.FirstName;
            LastName    = dto.LastName;
            Username    = dto.Username;
            Email       = dto.Email;
            PhoneNumber = dto.PhoneNumber;
            RoleTypeId  = dto.RoleTypeId;
            Status      = dto.Status;
            Password    = string.Empty;   // never pre-fill password

            Permissions = (await _svc.GetUserPermissionsAsync(userId, ct)).ToList();
        }
        catch (Exception ex) { Error = ex.Message; }
        finally { IsLoading = false; }
    }

    // =========================================================================
    // Save
    // =========================================================================

    public async Task<bool> SaveAsync(CancellationToken ct = default)
    {
        Error   = null;
        Success = null;
        IsSaving = true;

        try
        {
            long savedId;

            if (IsNew)
            {
                var (ok, err, id) = await _svc.CreateUserAsync(new UserCreateDto
                {
                    FirstName   = FirstName.Trim(),
                    LastName    = LastName.Trim(),
                    Username    = Username.Trim(),
                    Email       = Email.Trim(),
                    PhoneNumber = PhoneNumber?.Trim(),
                    Password    = Password,
                    RoleTypeId  = RoleTypeId,
                }, ct);

                if (!ok) { Error = err ?? "Unknown error."; return false; }
                savedId = id;
            }
            else
            {
                var (ok, err) = await _svc.UpdateUserAsync(new UserUpdateDto
                {
                    Id          = UserId,
                    FirstName   = FirstName.Trim(),
                    LastName    = LastName.Trim(),
                    Username    = Username.Trim(),
                    Email       = Email.Trim(),
                    PhoneNumber = PhoneNumber?.Trim(),
                    NewPassword = string.IsNullOrWhiteSpace(Password) ? null : Password,
                    RoleTypeId  = RoleTypeId,
                    Status      = Status,
                }, ct);

                if (!ok) { Error = err ?? "Unknown error."; return false; }
                savedId = UserId;
            }

            // Save permission matrix
            var (permOk, permErr) = await _svc.SaveUserPermissionsAsync(savedId, Permissions, ct);
            if (!permOk) { Error = permErr ?? "Permissions could not be saved."; return false; }

            Success = IsNew ? "User created successfully." : "User updated successfully.";
            return true;
        }
        catch (Exception ex)
        {
            Error = ex.Message;
            return false;
        }
        finally
        {
            IsSaving = false;
        }
    }
}
