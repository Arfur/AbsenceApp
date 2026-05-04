/*
===============================================================================
 File        : IUserManagementService.cs
 Namespace   : AbsenceApp.Core.Interfaces
 Author      : Michael
 Version     : 1.4.0
 Created     : 2026-04-11
 Updated     : 2026-05-04
-------------------------------------------------------------------------------
 Purpose     : Service contract for the E15 User Management module.
               Covers user CRUD, role listing, page listing, per-user
               permission matrix read/write, the three list-page data
               methods added for the Roles, Permissions, and Page Access pages,
               and the full User Profile page data methods (v1.2.0).
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-11  Initial creation (E15 User Management).
   - 1.1.0  2026-04-21  Added GetAllRoleTypesAsync(), GetFeaturesAsync(), and
                         GetPageAccessAsync() to support live data on the Roles,
                         Permissions, and Page Access list pages.
   - 1.2.0  2026-04-21  Added User Profile page methods: GetUserProfileHeaderAsync,
                         GetUserProfileDetailAsync, GetStaffContactAsync,
                         GetStaffClassAssignmentsAsync, GetStaffDevicesAsync,
                         GetStaffExternalAccountsAsync, GetStaffAbsencesAsync,
                         GetUserLoginAuditAsync, SaveUserProfileAsync,
                         ChangePasswordAsync, UpdateProfilePhotoAsync.
  - 1.3.0  2026-04-25  Updated GetUserProfileDetailAsync to return unified
                 UserProfileFullDetailDto for one-call full profile
                 loading.
  - 1.4.0  2026-05-04  Added GetUsersForSelectAsync() — returns all users with
                 accounts as lightweight UserSelectDto list, used for the
                 Edit Mode user-navigation dropdown.
-------------------------------------------------------------------------------
 Notes       :
   - Implemented by UserManagementService in AbsenceApp.Data.Services.
   - Register as Scoped (EF Core DbContext dependency).
===============================================================================
*/
using AbsenceApp.Core.DTOs;

namespace AbsenceApp.Core.Interfaces;

public interface IUserManagementService
{
    // ── User CRUD ─────────────────────────────────────────────────────────────

    Task<IReadOnlyList<UserListItemDto>> GetUsersAsync(CancellationToken ct = default);
    Task<UserUpdateDto?>                 GetUserForEditAsync(int userId, CancellationToken ct = default);
    Task<long>                           CreateUserAsync(UserCreateDto dto, CancellationToken ct = default);
    Task                                 UpdateUserAsync(UserUpdateDto dto, CancellationToken ct = default);
    Task                                 DeleteUserAsync(int userId, CancellationToken ct = default);

    // ── Staff-linked helpers ──────────────────────────────────────────────────

    Task<StaffSelectDto?> GetStaffForUserCreateAsync(int staffId, CancellationToken ct = default);
    Task<bool>            StaffHasUserAsync(int staffId, CancellationToken ct = default);
    Task<IReadOnlyList<StaffSelectDto>> GetStaffWithoutUsersAsync(CancellationToken ct = default);
    Task<IReadOnlyList<UserSelectDto>> GetUsersForSelectAsync(CancellationToken ct = default);

    // ── Reference data ───────────────────────────────────────────────────────

    Task<IReadOnlyList<RoleTypeSelectDto>> GetRoleTypesAsync(CancellationToken ct = default);
    Task<IReadOnlyList<AppPageDto>>        GetPagesAsync(CancellationToken ct = default);
    Task<IReadOnlyList<RoleListItemDto>>   GetAllRoleTypesAsync(CancellationToken ct = default);
    Task<IReadOnlyList<FeatureListItemDto>> GetFeaturesAsync(CancellationToken ct = default);
    Task<IReadOnlyList<PageAccessRowDto>>  GetPageAccessAsync(CancellationToken ct = default);

    // ── Permission matrix ─────────────────────────────────────────────────────

    Task<IReadOnlyList<PagePermissionDto>> GetUserPermissionsAsync(long userId, CancellationToken ct = default);
    Task SaveUserPermissionsAsync(long userId, IEnumerable<PagePermissionDto> permissions, CancellationToken ct = default);
    Task<IReadOnlyList<PagePermissionDto>> GetRoleDefaultsAsync(string roleTypeName, CancellationToken ct = default);

    // ── User Profile page ─────────────────────────────────────────────────────

    /// <summary>Returns the header strip data (name, role, status, photo, last-login) for a user.</summary>
    Task<UserProfileHeaderDto?> GetUserProfileHeaderAsync(int userId, CancellationToken ct = default);

    /// <summary>Returns the unified full profile payload (users + userprofiles + staff + staff-linked collections) for a user.</summary>
    Task<UserProfileFullDetailDto> GetUserProfileDetailAsync(int userId, CancellationToken ct = default);

    /// <summary>Returns the Staff employment and contact fields for the given staffId.</summary>
    Task<StaffContactDto?> GetStaffContactAsync(int staffId, CancellationToken ct = default);

    /// <summary>Returns all StaffAssignment rows for the staff member, joined with class/job-title/department names.</summary>
    Task<IReadOnlyList<StaffClassRowDto>> GetStaffClassAssignmentsAsync(int staffId, CancellationToken ct = default);

    /// <summary>Returns all StaffDevice rows for the staff member, joined with device type name.</summary>
    Task<IReadOnlyList<StaffDeviceRowDto>> GetStaffDevicesAsync(int staffId, CancellationToken ct = default);

    /// <summary>Returns all StaffExternalAccount rows for the staff member, joined with external system name.</summary>
    Task<IReadOnlyList<StaffExternalRowDto>> GetStaffExternalAccountsAsync(int staffId, CancellationToken ct = default);

    /// <summary>Returns Absence rows where PersonType="Staff" and PersonId=staffId, ordered by StartDate desc.</summary>
    Task<IReadOnlyList<StaffAbsenceRowDto>> GetStaffAbsencesAsync(int staffId, CancellationToken ct = default);

    /// <summary>Returns LoginAudit rows for the given userId, ordered by LoginTime desc (max 200 rows).</summary>
    Task<IReadOnlyList<LoginAuditRowDto>> GetUserLoginAuditAsync(int userId, CancellationToken ct = default);

    /// <summary>Saves both the User and UserProfile records from a profile form save.</summary>
    Task SaveUserProfileAsync(UserProfileSaveDto dto, CancellationToken ct = default);

    /// <summary>Verifies oldPassword then changes the stored hash to a hash of newPassword.</summary>
    Task ChangePasswordAsync(ChangePasswordDto dto, CancellationToken ct = default);

    /// <summary>Updates UserProfile.ProfilePictureUrl for the given userId (upserts the row if absent).</summary>
    Task UpdateProfilePhotoAsync(int userId, string photoUrl, CancellationToken ct = default);
}
