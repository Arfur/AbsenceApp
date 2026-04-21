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

    /// <summary>Mandatory. Set from the originating Staff record; immutable after creation.</summary>
    public long    StaffId      { get; set; }
    public string  Username     { get; set; } = string.Empty;
    public string  Email        { get; set; } = string.Empty;
    public string  Password     { get; set; } = string.Empty;
    public long    RoleTypeId   { get; set; }
    public string  Status       { get; set; } = "active";

    // =========================================================================
    // Linked staff (read-only display)
    // =========================================================================

    public StaffSelectDto? LinkedStaff { get; private set; }

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

    public long   UserId   { get; private set; }
    public bool   IsLoading { get; private set; }
    public bool   IsSaving  { get; private set; }
    public string? Error    { get; private set; }
    public string? Success  { get; private set; }

    /// <summary>Called by the page when the route is invalid before any async init.</summary>
    public void SetError(string message) => Error = message;

    // =========================================================================
    // Initialise for Add (must be called with the originating StaffId)
    // =========================================================================

    public async Task InitNewAsync(long staffId, CancellationToken ct = default)
    {
        IsNew     = true;
        StaffId   = staffId;
        IsLoading = true;
        Error     = null;
        try
        {
            RoleTypes   = await _svc.GetRoleTypesAsync(ct);
            LinkedStaff = await _svc.GetStaffForUserCreateAsync(staffId, ct);
            if (LinkedStaff is null)
            {
                Error = $"Staff record {staffId} not found.";
                return;
            }
            // Pre-fill email from staff work email.
            Email = LinkedStaff.WorkEmail;
            Permissions = (await _svc.GetUserPermissionsAsync(0, ct)).ToList();
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

            UserId    = dto.Id;
            StaffId   = dto.StaffId ?? 0;
            Username  = dto.Username;
            Email     = dto.Email;
            RoleTypeId = dto.RoleTypeId;
            Status    = dto.Status;
            Password  = string.Empty;   // never pre-fill password

            if (StaffId > 0)
                LinkedStaff = await _svc.GetStaffForUserCreateAsync(StaffId, ct);

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
                    StaffId    = StaffId,
                    Username   = Username.Trim(),
                    Email      = Email.Trim(),
                    Password   = Password,
                    RoleTypeId = RoleTypeId,
                }, ct);

                if (!ok) { Error = err ?? "Unknown error."; return false; }
                savedId = id;
            }
            else
            {
                var (ok, err) = await _svc.UpdateUserAsync(new UserUpdateDto
                {
                    Id          = UserId,
                    StaffId     = StaffId,    // passed through but NOT modified by service
                    Username    = Username.Trim(),
                    Email       = Email.Trim(),
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
