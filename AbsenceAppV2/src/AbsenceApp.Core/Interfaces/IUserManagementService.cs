/*
===============================================================================
 File        : IUserManagementService.cs
 Namespace   : AbsenceApp.Core.Interfaces
 Author      : Michael
 Version     : 1.2.0
 Created     : 2026-04-11
 Updated     : 2026-04-21
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
    Task<UserUpdateDto?>                 GetUserForEditAsync(long userId, CancellationToken ct = default);
    Task<long>                           CreateUserAsync(UserCreateDto dto, CancellationToken ct = default);
    Task                                 UpdateUserAsync(UserUpdateDto dto, CancellationToken ct = default);
    Task                                 DeleteUserAsync(long userId, CancellationToken ct = default);

    // ── Staff-linked helpers ──────────────────────────────────────────────────

    Task<StaffSelectDto?> GetStaffForUserCreateAsync(long staffId, CancellationToken ct = default);
    Task<bool>            StaffHasUserAsync(long staffId, CancellationToken ct = default);

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
    Task<UserProfileHeaderDto?> GetUserProfileHeaderAsync(long userId, CancellationToken ct = default);

    /// <summary>Returns the extended profile fields (user_profiles row) for a user. Returns a blank DTO if no row exists yet.</summary>
    Task<UserProfileDetailDto> GetUserProfileDetailAsync(long userId, CancellationToken ct = default);

    /// <summary>Returns the Staff employment and contact fields for the given staffId.</summary>
    Task<StaffContactDto?> GetStaffContactAsync(long staffId, CancellationToken ct = default);

    /// <summary>Returns all StaffAssignment rows for the staff member, joined with class/job-title/department names.</summary>
    Task<IReadOnlyList<StaffClassRowDto>> GetStaffClassAssignmentsAsync(long staffId, CancellationToken ct = default);

    /// <summary>Returns all StaffDevice rows for the staff member, joined with device type name.</summary>
    Task<IReadOnlyList<StaffDeviceRowDto>> GetStaffDevicesAsync(long staffId, CancellationToken ct = default);

    /// <summary>Returns all StaffExternalAccount rows for the staff member, joined with external system name.</summary>
    Task<IReadOnlyList<StaffExternalRowDto>> GetStaffExternalAccountsAsync(long staffId, CancellationToken ct = default);

    /// <summary>Returns Absence rows where PersonType="Staff" and PersonId=staffId, ordered by StartDate desc.</summary>
    Task<IReadOnlyList<StaffAbsenceRowDto>> GetStaffAbsencesAsync(long staffId, CancellationToken ct = default);

    /// <summary>Returns LoginAudit rows for the given userId, ordered by LoginTime desc (max 200 rows).</summary>
    Task<IReadOnlyList<LoginAuditRowDto>> GetUserLoginAuditAsync(long userId, CancellationToken ct = default);

    /// <summary>Saves both the User and UserProfile records from a profile form save.</summary>
    Task SaveUserProfileAsync(UserProfileSaveDto dto, CancellationToken ct = default);

    /// <summary>Verifies oldPassword then changes the stored hash to a hash of newPassword.</summary>
    Task ChangePasswordAsync(ChangePasswordDto dto, CancellationToken ct = default);

    /// <summary>Updates UserProfile.ProfilePictureUrl for the given userId (upserts the row if absent).</summary>
    Task UpdateProfilePhotoAsync(long userId, string photoUrl, CancellationToken ct = default);
}
