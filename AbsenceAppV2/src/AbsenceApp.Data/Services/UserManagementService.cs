/*
===============================================================================
 File        : UserManagementService.cs
 Namespace   : AbsenceApp.Data.Services
 Author      : Michael
 Version     : 2.0.0
 Created     : 2026-04-11
 Updated     : 2026-05-04
-------------------------------------------------------------------------------
 Purpose     : E15 User Management service. Implements IUserManagementService
               for full user CRUD, role/page reference data, and per-user
               page-level permission matrix read/write.

               Passwords are hashed using PBKDF2 (Rfc2898DeriveBytes / SHA-256,
               310 000 iterations) with a random 32-byte salt. The stored format
               is "<iterations>:<salt_b64>:<hash_b64>" so AuthService can detect
               old plain-text credentials and the new hashed format transparently.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-11  Initial creation (E15 User Management).
   - 1.1.0  2026-04-11  E16 Pages Registry: updated GetPagesAsync() to project
                         new AppPage fields (Slug, CategoryKey, MenuKey, IconKey,
                         Description, SupportsXxx) into AppPageDto.
    - 1.2.0  2026-04-21  Fixed GetUsersAsync() — replaced broken raw string
                         literal SQL (IN clause was emitted as literal text)
                         with correct string concatenation. Added three new
                         methods: GetAllRoleTypesAsync() (Roles page),
                         GetFeaturesAsync() (Permissions page), and
                         GetPageAccessAsync() (Page Access page).
    - 1.3.0  2026-04-21  Fixed DeleteUserAsync() cascade: now deletes userrole
                         (raw SQL), UserPagePermissions, UserPageOverrides,
                         UserProfiles, and LoginAudit child rows before
                         removing the user record to avoid FK constraint
                         violations.
   - 1.4.0  2026-04-21  Added 11 User Profile page methods. Removed dead
                         BuildFullName() private method. Fixed UserRoleIdRow
                         value type (int â†’ long). Added IsAdmin field to
                         UserProfileSaveDto and SaveUserProfileAsync.
   - 1.5.0  2026-04-22  Session 6: Added GetStaffWithoutUsersAsync() â€” returns
                         all Staff rows where no User account has a matching
                         StaffId FK, ordered by LastName then FirstName.
                         (Header update missed in Session 6; corrected here.)
    - 1.6.0  2026-04-24  Session 7 Task E: re-validated all profile loading
                                 queries. Confirmed _db.UserProfiles -> userprofiles
                                 (via [Table] attribute), _db.LoginAudit -> correct
                         column mapping (via [Column] attributes in v1.2.0),
                         _db.AppPages / _db.UserPagePermissions /
                         _db.RoleDefaultPagePermissions -> correct table names
                         via EF config. No code changes required; header
                         version incremented as evidence of validation pass.
   - 1.7.0  2026-04-25  Session 9 final fix: DateOfBirth does not exist on the
                         userprofiles table -- it belongs to Staff. Updated
                         GetUserProfileDetailAsync to load DateOfBirth from
                         Staff (via User.StaffId) instead of from UserProfile.
                         Removed profile.DateOfBirth assignment in
                         SaveUserProfileAsync.
    - 1.8.0  2026-04-25  Added unified one-call profile detail endpoint payload.
                                 GetUserProfileDetailAsync now returns
                                 UserProfileFullDetailDto combining users, userprofiles,
                                 staff, permissions, role types, and staff-linked tab
                                 collections (contacts, classes, devices, external,
                                 absences, login audit) plus staff-related summaries.
   - 1.9.0  2026-05-04  Fix Plan #2 Step 8: changed (ulong)staffId to (long)staffId
                         in GetStaffAbsencesAsync LINQ Where clause. Absence.PersonId
                         is long after Phase 2 type alignment; long == ulong was
                         ambiguous (CS0034). staffId is int; (long)staffId matches.
   - 2.0.0  2026-05-04  Added GetUsersForSelectAsync() — loads all users, joins Staff
                         table for FullName, returns IReadOnlyList<UserSelectDto>
                         ordered by FullName. Used for the Edit Mode user-navigation
                         dropdown in UserFormPageV2 (Amendment C).
-------------------------------------------------------------------------------
 Notes       :
   - Register as Scoped in DataServiceRegistration.cs.
   - UserPagePermission rows where all six flags are false are deleted (not
     inserted as all-false rows) to keep the table lean.
   - feature and rolefeature tables have no EF DbSets; GetFeaturesAsync()
     queries them via raw SQL.
===============================================================================
*/

using System.Security.Cryptography;
using System.Text;
using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;
using AbsenceApp.Data.Context;
using AbsenceApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AbsenceApp.Data.Services;

public sealed class UserManagementService : IUserManagementService
{
    // =========================================================================
    // Construction
    // =========================================================================

    private readonly AppDbContext _db;

    public UserManagementService(AppDbContext db) => _db = db;

    // =========================================================================
    // Constants
    // =========================================================================

    private const int    Pbkdf2Iterations = 310_000;
    private const int    SaltBytes        = 32;
    private const int    HashBytes        = 32;
    private const string ActiveStatus     = "active";

    // =========================================================================
    // User CRUD
    // =========================================================================

    public async Task<IReadOnlyList<UserListItemDto>> GetUsersAsync(CancellationToken ct = default)
    {
        // Load all users.
        var users = await _db.Users
            .AsNoTracking()
            .OrderBy(u => u.Username)
            .ToListAsync(ct);

        // Staff name lookup.
        var staffIds = users
            .Where(u => u.StaffId.HasValue)
            .Select(u => u.StaffId!.Value)
            .Distinct()
            .ToList();

        var staffNames = staffIds.Count > 0
            ? await _db.Staff
                .AsNoTracking()
                .Where(s => staffIds.Contains(s.Id))
                .ToDictionaryAsync(s => s.Id,
                    s => $"{s.FirstName} {s.LastName}".Trim(), ct)
            : new Dictionary<int, string>();

        // Role display-name lookup via raw SQL (userrole table has no DbSet).
        // Returns rows: UserId, DisplayName
        var userIds = users.Select(u => u.Id).ToList();
        var roleRows = userIds.Count > 0
            ? await _db.Database
                .SqlQueryRaw<UserRoleRow>(
                    "SELECT ur.UserId, r.Name AS DisplayName " +
                    "FROM userrole ur " +
                    "JOIN roles r ON r.Id = ur.RoleId " +
                    "WHERE ur.UserId IN (" + string.Join(",", userIds) + ")")
                .ToListAsync(ct)
            : new List<UserRoleRow>();

        var userRoleMap = roleRows
            .GroupBy(r => r.UserId)
            .ToDictionary(g => g.Key, g => g.First().DisplayName);

        return users.Select(u => new UserListItemDto
        {
            Id           = u.Id,
            StaffId      = u.StaffId,
            StaffName    = u.StaffId.HasValue && staffNames.TryGetValue(u.StaffId.Value, out var sn)
                               ? sn : string.Empty,
            FullName     = u.StaffId.HasValue && staffNames.TryGetValue(u.StaffId.Value, out var fn)
                               ? fn : u.Username,
            Username     = u.Username,
            Email        = u.Email,
            RoleName     = userRoleMap.TryGetValue(u.Id, out var rn) ? rn : string.Empty,
            Status       = u.Status,
            CreatedAt    = u.CreatedAt,
        }).ToList();
    }

    // Private projection type for role SQL query.
    private sealed class UserRoleRow
    {
        public int    UserId      { get; set; }
        public string DisplayName { get; set; } = string.Empty;
    }

    private sealed class UserRoleFullRow
    {
        public int    UserId      { get; set; }
        public int    RoleId      { get; set; }
        public string RoleName    { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
    }

    public async Task<UserUpdateDto?> GetUserForEditAsync(int userId, CancellationToken ct = default)
    {
        var u = await _db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == userId, ct);

        if (u is null) return null;

        // Resolve the user's role via raw SQL (userrole table has no DbSet).
        var roleRows = await _db.Database
            .SqlQueryRaw<UserRoleIdRow>(
                "SELECT RoleId AS Value FROM userrole WHERE UserId = {0} LIMIT 1",
                userId)
            .ToListAsync(ct);
        var roleId = roleRows.Count > 0 ? (int)roleRows[0].Value : 0;

        return new UserUpdateDto
        {
            Id          = u.Id,
            StaffId     = u.StaffId,
            Username    = u.Username,
            Email       = u.Email,
            RoleId      = roleId,
            Status      = u.Status,
            NewPassword = null,
        };
    }

    private sealed class UserRoleIdRow { public int Value { get; set; } }

    public async Task<long> CreateUserAsync(UserCreateDto dto, CancellationToken ct = default)
    {
        // Mandatory field checks 
        if (dto.StaffId <= 0)                    throw new ArgumentException("StaffId is required. Users can only be created from a Staff record.");
        if (string.IsNullOrWhiteSpace(dto.Username))  throw new ArgumentException("Username is required.");
        if (string.IsNullOrWhiteSpace(dto.Email))     throw new ArgumentException("Email is required.");
        if (string.IsNullOrWhiteSpace(dto.Password))  throw new ArgumentException("Password is required.");

        // Validate StaffId exists 
        var staffExists = await _db.Staff.AnyAsync(s => s.Id == dto.StaffId, ct);
        if (!staffExists)
            throw new InvalidOperationException($"Staff record {dto.StaffId} does not exist.");

        // Prevent duplicate user for same StaffId 
        var staffAlreadyHasUser = await _db.Users.AnyAsync(u => u.StaffId == dto.StaffId, ct);
        if (staffAlreadyHasUser)
            throw new InvalidOperationException($"A user account already exists for Staff #{dto.StaffId}.");

        // Prevent duplicate username 
        var duplicate = await _db.Users.AnyAsync(u => u.Username == dto.Username, ct);
        if (duplicate)
            throw new InvalidOperationException($"Username '{dto.Username}' is already taken.");

        // Insert user (auto_increment PK) 
        var user = new User
        {
            StaffId   = dto.StaffId,
            Username  = dto.Username.Trim(),
            Email     = dto.Email.Trim(),
            Password  = HashPassword(dto.Password),
            Status    = ActiveStatus,
            IsAdmin   = false,
            LoginCount           = 0,
            IsTwoFactorEnabled   = false,
            Timezone             = "UTC",
            LanguageCode         = "en",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };

        await using var tx = await _db.Database.BeginTransactionAsync(ct);

        _db.Users.Add(user);
        await _db.SaveChangesAsync(ct);

        // Insert UserRole row via raw SQL (userrole table has no DbSet) 
        if (dto.RoleId > 0)
        {
            await _db.Database.ExecuteSqlRawAsync(
                "INSERT INTO userrole (UserId, RoleId, AssignedAt, AssignedBy) VALUES ({0}, {1}, {2}, {0})",
                user.Id, dto.RoleId, DateTime.UtcNow);
        }

        await tx.CommitAsync(ct);
        return user.Id;
    }

    public async Task UpdateUserAsync(UserUpdateDto dto, CancellationToken ct = default)
    {
        var user = await _db.Users.FindAsync([dto.Id], ct)
                   ?? throw new KeyNotFoundException($"User {dto.Id} not found.");

        // StaffId is IMMUTABLE after creation never update it.
        user.Username  = dto.Username.Trim();
        user.Email     = dto.Email.Trim();
        user.Status    = dto.Status;
        user.UpdatedAt = DateTime.UtcNow;

        if (!string.IsNullOrWhiteSpace(dto.NewPassword))
            user.Password = HashPassword(dto.NewPassword);

        // Update role via raw SQL.
        if (dto.RoleId > 0)
        {
            var existsCount = await _db.Database
                .SqlQueryRaw<int>("SELECT COUNT(*) AS Value FROM userrole WHERE UserId = {0}", dto.Id)
                .FirstOrDefaultAsync();
            if (existsCount == 0)
            {
                await _db.Database.ExecuteSqlRawAsync(
                    "INSERT INTO userrole (UserId, RoleId, AssignedAt, AssignedBy) VALUES ({0}, {1}, {2}, {0})",
                    dto.Id, dto.RoleId, DateTime.UtcNow);
            }
            else
            {
                await _db.Database.ExecuteSqlRawAsync(
                    "UPDATE userrole SET RoleId = {0} WHERE UserId = {1}",
                    dto.RoleId, dto.Id);
            }
        }

        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteUserAsync(int userId, CancellationToken ct = default)
    {
        var user = await _db.Users.FindAsync([userId], ct)
                   ?? throw new KeyNotFoundException($"User {userId} not found.");

        // Delete child records to avoid FK constraint violations.
        // userrole has no EF DbSet â€” use raw SQL.
        await _db.Database.ExecuteSqlRawAsync(
            "DELETE FROM userrole WHERE UserId = {0}", userId);

        // EF-tracked child entities
        var permissions = await _db.UserPagePermissions
            .Where(p => p.UserId == userId).ToListAsync(ct);
        _db.UserPagePermissions.RemoveRange(permissions);

        var overrides = await _db.UserPageOverrides
            .Where(o => o.UserId == userId).ToListAsync(ct);
        _db.UserPageOverrides.RemoveRange(overrides);

        var profiles = await _db.UserProfiles
            .Where(p => p.UserId == userId).ToListAsync(ct);
        _db.UserProfiles.RemoveRange(profiles);

        var loginAudits = await _db.LoginAudit
            .Where(a => a.UserId == userId).ToListAsync(ct);
        _db.LoginAudit.RemoveRange(loginAudits);

        _db.Users.Remove(user);
        await _db.SaveChangesAsync(ct);
    }

    // =========================================================================
    // Staff-linked helpers
    // =========================================================================

    public async Task<StaffSelectDto?> GetStaffForUserCreateAsync(
        int staffId, CancellationToken ct = default)
    {
        var s = await _db.Staff
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == staffId, ct);

        if (s is null) return null;

        return new StaffSelectDto
        {
            Id          = s.Id,
            FullName    = $"{s.FirstName} {s.LastName}".Trim(),
            StaffNumber = s.StaffNumber,
            WorkEmail   = s.WorkEmail,
        };
    }

    public async Task<bool> StaffHasUserAsync(
        int staffId, CancellationToken ct = default)
        => await _db.Users.AnyAsync(u => u.StaffId == staffId, ct);

    public async Task<IReadOnlyList<StaffSelectDto>> GetStaffWithoutUsersAsync(
        CancellationToken ct = default)
    {
        var linkedIds = await _db.Users
            .AsNoTracking()
            .Where(u => u.StaffId.HasValue)
            .Select(u => u.StaffId!.Value)
            .ToListAsync(ct);

        var linkedSet = new HashSet<int>(linkedIds);

        return await _db.Staff
            .AsNoTracking()
            .Where(s => !linkedSet.Contains(s.Id))
            .OrderBy(s => s.LastName)
            .ThenBy(s => s.FirstName)
            .Select(s => new StaffSelectDto
            {
                Id          = s.Id,
                FullName    = (s.FirstName + " " + s.LastName).Trim(),
                StaffNumber = s.StaffNumber,
                WorkEmail   = s.WorkEmail,
            })
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<UserSelectDto>> GetUsersForSelectAsync(
        CancellationToken ct = default)
    {
        // Load all user rows with their StaffId and Username.
        var userRows = await _db.Users
            .AsNoTracking()
            .Select(u => new { u.Id, u.StaffId, u.Username })
            .ToListAsync(ct);

        // Collect distinct StaffIds that are present.
        var staffIds = userRows
            .Where(u => u.StaffId.HasValue)
            .Select(u => u.StaffId!.Value)
            .Distinct()
            .ToList();

        // Build a name map from Staff table.
        var nameMap = staffIds.Count > 0
            ? await _db.Staff
                .AsNoTracking()
                .Where(s => staffIds.Contains(s.Id))
                .ToDictionaryAsync(
                    s => s.Id,
                    s => (s.FirstName + " " + s.LastName).Trim(),
                    ct)
            : new Dictionary<int, string>();

        return userRows
            .Select(u => new UserSelectDto
            {
                Id       = u.Id,
                FullName = u.StaffId.HasValue && nameMap.TryGetValue(u.StaffId.Value, out var n)
                               ? n
                               : u.Username,
                Username = u.Username,
            })
            .OrderBy(u => u.FullName)
            .ToList();
    }

    // =========================================================================
    // Reference data
    // =========================================================================

    public async Task<IReadOnlyList<RoleTypeSelectDto>> GetRoleTypesAsync(CancellationToken ct = default)
    {
        return await _db.RoleTypes
            .AsNoTracking()
            .OrderBy(r => r.Priority)
            .Select(r => new RoleTypeSelectDto
            {
                Id          = r.Id,
                Name        = r.Name,
                DisplayName = r.Name,
            })
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<AppPageDto>> GetPagesAsync(CancellationToken ct = default)
    {
        return await _db.AppPages
            .AsNoTracking()
            .Where(p => p.IsActive)
            .OrderBy(p => p.SortOrder)
            .Select(p => new AppPageDto
            {
                Id             = p.Id,
                Name           = p.Name,
                Slug           = p.Slug,
                Route          = p.Route,
                CategoryKey    = p.CategoryKey,
                MenuKey        = p.MenuKey,
                IconKey        = p.IconKey,
                Description    = p.Description,
                IsActive       = p.IsActive,
                SortOrder      = p.SortOrder,
                SupportsRead   = p.SupportsRead,
                SupportsWrite  = p.SupportsWrite,
                SupportsCreate = p.SupportsCreate,
                SupportsDelete = p.SupportsDelete,
                SupportsImport = p.SupportsImport,
                SupportsExport = p.SupportsExport,
            })
            .ToListAsync(ct);
    }

    // =========================================================================
    // Permission matrix
    // =========================================================================

    public async Task<IReadOnlyList<PagePermissionDto>> GetUserPermissionsAsync(
        long userId, CancellationToken ct = default)
    {
        var pages = await _db.AppPages
            .AsNoTracking()
            .Where(p => p.IsActive)
            .OrderBy(p => p.SortOrder)
            .ToListAsync(ct);

        var existingRows = await _db.UserPagePermissions
            .AsNoTracking()
            .Where(p => p.UserId == userId)
            .ToListAsync(ct);

        var overrideMap = existingRows.ToDictionary(p => p.PageId);

        return pages.Select(pg =>
        {
            overrideMap.TryGetValue(pg.Id, out var row);
            return new PagePermissionDto
            {
                PageId      = pg.Id,
                PageName    = pg.Name,
                PageRoute   = pg.Route,
                HasOverride = row is not null,
                CanRead     = row?.CanRead   ?? false,
                CanWrite    = row?.CanWrite   ?? false,
                CanCreate   = row?.CanCreate  ?? false,
                CanDelete   = row?.CanDelete  ?? false,
                CanImport   = row?.CanImport  ?? false,
                CanExport   = row?.CanExport  ?? false,
            };
        }).ToList();
    }

    public async Task SaveUserPermissionsAsync(
        long userId,
        IEnumerable<PagePermissionDto> permissions,
        CancellationToken ct = default)
    {
        var existing = await _db.UserPagePermissions
            .Where(p => p.UserId == userId)
            .ToListAsync(ct);

        var existingMap = existing.ToDictionary(p => p.PageId);
        var permList    = permissions.ToList();

        foreach (var dto in permList)
        {
            var anySet = dto.CanRead || dto.CanWrite || dto.CanCreate
                      || dto.CanDelete || dto.CanImport || dto.CanExport;

            if (existingMap.TryGetValue(dto.PageId, out var row))
            {
                if (!anySet)
                {
                    // All flags cleared — remove the override row.
                    _db.UserPagePermissions.Remove(row);
                }
                else
                {
                    row.CanRead   = dto.CanRead;
                    row.CanWrite  = dto.CanWrite;
                    row.CanCreate = dto.CanCreate;
                    row.CanDelete = dto.CanDelete;
                    row.CanImport = dto.CanImport;
                    row.CanExport = dto.CanExport;
                }
            }
            else if (anySet)
            {
                // New override row for this user × page.
                _db.UserPagePermissions.Add(new UserPagePermission
                {
                    UserId    = userId,
                    PageId    = dto.PageId,
                    CanRead   = dto.CanRead,
                    CanWrite  = dto.CanWrite,
                    CanCreate = dto.CanCreate,
                    CanDelete = dto.CanDelete,
                    CanImport = dto.CanImport,
                    CanExport = dto.CanExport,
                });
            }
        }

        await _db.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<PagePermissionDto>> GetRoleDefaultsAsync(
        string roleCode, CancellationToken ct = default)
    {
        var pages = await _db.AppPages
            .AsNoTracking()
            .Where(p => p.IsActive)
            .OrderBy(p => p.SortOrder)
            .ToListAsync(ct);

        var defaults = await _db.RoleDefaultPagePermissions
            .AsNoTracking()
            .Where(r => r.RoleTypeName == roleCode)
            .ToListAsync(ct);

        var defaultMap = defaults.ToDictionary(d => d.PageId);

        return pages.Select(pg =>
        {
            defaultMap.TryGetValue(pg.Id, out var row);
            return new PagePermissionDto
            {
                PageId      = pg.Id,
                PageName    = pg.Name,
                PageRoute   = pg.Route,
                HasOverride = false,
                CanRead     = row?.CanRead   ?? false,
                CanWrite    = row?.CanWrite   ?? false,
                CanCreate   = row?.CanCreate  ?? false,
                CanDelete   = row?.CanDelete  ?? false,
                CanImport   = row?.CanImport  ?? false,
                CanExport   = row?.CanExport  ?? false,
            };
        }).ToList();
    }

    // =========================================================================
    // Roles list (all role types, with user count)
    // =========================================================================

    public async Task<IReadOnlyList<RoleListItemDto>> GetAllRoleTypesAsync(CancellationToken ct = default)
    {
        var roleTypes = await _db.RoleTypes
            .AsNoTracking()
            .OrderBy(r => r.Priority)
            .ThenBy(r => r.Id)
            .ToListAsync(ct);

        // Count users per role (roles table is now canonical; no roletypes join needed).
        var countRows = await _db.Database
            .SqlQueryRaw<RoleUserCountRow>(
                "SELECT r.Id, COUNT(DISTINCT ur.UserId) AS UserCount " +
                "FROM roles r " +
                "LEFT JOIN userrole ur ON ur.RoleId = r.Id " +
                "GROUP BY r.Id")
            .ToListAsync(ct);

        var countMap = countRows.ToDictionary(r => r.Id, r => r.UserCount);

        return roleTypes.Select(rt => new RoleListItemDto
        {
            Id          = rt.Id,
            Name        = rt.Name,
            DisplayName = rt.DisplayName,
            Description = rt.Description ?? string.Empty,
            UserCount   = countMap.TryGetValue(rt.Id, out var c) ? c : 0,
        }).ToList();
    }

    private sealed class RoleUserCountRow
    {
        public long Id        { get; set; }
        public int  UserCount { get; set; }
    }

    // =========================================================================
    // Permissions list (features + which role types have each feature)
    // =========================================================================

    public async Task<IReadOnlyList<FeatureListItemDto>> GetFeaturesAsync(CancellationToken ct = default)
    {
        // feature and rolefeature have no EF DbSets â€” use raw SQL.
        var featureRows = await _db.Database
            .SqlQueryRaw<FeatureRow>(
                "SELECT Id, Code, COALESCE(Description, '') AS Description " +
                "FROM features ORDER BY Id")
            .ToListAsync(ct);

        var roleFeatureRows = await _db.Database
            .SqlQueryRaw<FeatureRoleRow>(
                "SELECT rf.FeatureCode, r.Name AS DisplayName " +
                "FROM rolefeatures rf " +
                "JOIN roles r ON r.Id = rf.RoleId " +
                "WHERE rf.IsEnabled = 1")
            .ToListAsync(ct);

        var roleMap = roleFeatureRows
            .GroupBy(r => r.FeatureCode, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(
                g => g.Key,
                g => string.Join(", ", g.Select(x => x.DisplayName).Distinct().OrderBy(x => x)),
                StringComparer.OrdinalIgnoreCase);

        return featureRows.Select(f => new FeatureListItemDto
        {
            Id          = f.Id,
            Code        = f.Code,
            Description = f.Description,
            Roles       = roleMap.TryGetValue(f.Code, out var r) ? r : string.Empty,
        }).ToList();
    }

    private sealed class FeatureRow
    {
        public long   Id          { get; set; }
        public string Code        { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    private sealed class FeatureRoleRow
    {
        public string FeatureCode { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
    }

    // =========================================================================
    // Page access list (app_pages + which role types can read each page)
    // =========================================================================

    public async Task<IReadOnlyList<PageAccessRowDto>> GetPageAccessAsync(CancellationToken ct = default)
    {
        var pages = await _db.AppPages
            .AsNoTracking()
            .Where(p => p.IsActive)
            .OrderBy(p => p.SortOrder)
            .ToListAsync(ct);

        var roleDefaults = await _db.RoleDefaultPagePermissions
            .AsNoTracking()
            .Where(r => r.CanRead)
            .Select(r => new { r.PageId, r.RoleTypeName })
            .ToListAsync(ct);

        var roleMap = roleDefaults
            .GroupBy(r => r.PageId)
            .ToDictionary(
                g => g.Key,
                g => string.Join(", ", g.Select(x => x.RoleTypeName).OrderBy(x => x)));

        return pages.Select(p => new PageAccessRowDto
        {
            Id       = p.Id,
            PageName = p.Name,
            Route    = p.Route,
            Category = p.CategoryKey,
            Roles    = roleMap.TryGetValue(p.Id, out var r) ? r : "â€”",
        }).ToList();
    }

    // =========================================================================
    // Password helpers
    // =========================================================================

    /// <summary>
    /// Returns a PBKDF2-SHA256 hash in the format
    /// "&lt;iterations&gt;:&lt;salt_b64&gt;:&lt;hash_b64&gt;".
    /// </summary>
    public static string HashPassword(string plainText)
    {
        using var rng  = RandomNumberGenerator.Create();
        var salt       = new byte[SaltBytes];
        rng.GetBytes(salt);

        var hash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(plainText),
            salt,
            Pbkdf2Iterations,
            HashAlgorithmName.SHA256,
            HashBytes);

        return $"{Pbkdf2Iterations}:{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
    }

    /// <summary>
    /// Verifies a plain-text password against a stored hash produced by
    /// <see cref="HashPassword"/>, a BCrypt hash (legacy seeded accounts),
    /// or falls back to plain-text comparison for legacy accounts (development data).
    /// </summary>
    public static bool VerifyPassword(string plainText, string stored)
    {
        // BCrypt format: starts with $2a$, $2b$, or $2y$
        if (stored.StartsWith("$2", StringComparison.Ordinal))
            return BCrypt.Net.BCrypt.Verify(plainText, stored);

        // PBKDF2 format: "<iterations>:<salt_b64>:<hash_b64>"
        var parts = stored.Split(':');
        if (parts.Length == 3
            && int.TryParse(parts[0], out var iters))
        {
            try
            {
                var salt   = Convert.FromBase64String(parts[1]);
                var stored_ = Convert.FromBase64String(parts[2]);
                var hash   = Rfc2898DeriveBytes.Pbkdf2(
                    Encoding.UTF8.GetBytes(plainText),
                    salt,
                    iters,
                    HashAlgorithmName.SHA256,
                    stored_.Length);
                return CryptographicOperations.FixedTimeEquals(hash, stored_);
            }
            catch
            {
                // Fall through to plain-text comparison.
            }
        }

        // Legacy plain-text comparison (dev-only accounts).
        return stored == plainText;
    }

    // =========================================================================
    // User Profile page methods (v1.4.0)
    // =========================================================================

    public async Task<UserProfileHeaderDto?> GetUserProfileHeaderAsync(
        int userId, CancellationToken ct = default)
    {
        var user = await _db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId, ct);

        if (user is null) return null;

        // Staff name
        var staffName = string.Empty;
        if (user.StaffId.HasValue)
        {
            var staff = await _db.Staff
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == user.StaffId.Value, ct);
            if (staff is not null)
                staffName = $"{staff.FirstName} {staff.LastName}".Trim();
        }

        // Role display name via raw SQL (userrole has no DbSet).
        var roleRows = await _db.Database
            .SqlQueryRaw<UserRoleRow>(
                "SELECT ur.UserId, r.Name AS DisplayName " +
                "FROM userrole ur " +
                "JOIN roles r ON r.Id = ur.RoleId " +
                "WHERE ur.UserId = {0} LIMIT 1",
                userId)
            .ToListAsync(ct);
        var roleName = roleRows.Count > 0 ? roleRows[0].DisplayName : string.Empty;

        // Last login time.
        var lastLogin = await _db.LoginAudit
            .AsNoTracking()
            .Where(a => a.UserId == userId && a.Success)
            .OrderByDescending(a => a.LoginTime)
            .Select(a => (DateTime?)a.LoginTime)
            .FirstOrDefaultAsync(ct);

        // Profile picture from UserProfile.
        var profilePic = await _db.UserProfiles
            .AsNoTracking()
            .Where(p => p.UserId == userId)
            .Select(p => p.ProfilePictureUrl)
            .FirstOrDefaultAsync(ct);

        return new UserProfileHeaderDto
        {
            UserId            = user.Id,
            StaffId           = user.StaffId,
            Username          = user.Username,
            FullName          = !string.IsNullOrWhiteSpace(staffName) ? staffName : user.Username,
            Email             = user.Email,
            RoleName          = roleName,
            Status            = user.Status,
            IsAdmin           = user.IsAdmin,
            ProfilePictureUrl = profilePic,
            LastLoginAt       = lastLogin,
            CreatedAt         = user.CreatedAt,
        };
    }

    public async Task<UserProfileFullDetailDto> GetUserProfileDetailAsync(
        int userId, CancellationToken ct = default)
    {
        var user = await _db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId, ct);

        if (user is null)
        {
            return new UserProfileFullDetailDto
            {
                UserExists = false,
                UserId     = userId,
            };
        }

        var profile = await _db.UserProfiles
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.UserId == userId, ct);

        var staff = user.StaffId.HasValue
            ? await _db.Staff
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == user.StaffId.Value, ct)
            : null;

        // Role display/name/id via raw SQL (userrole has no DbSet).
        var roleRows = await _db.Database
            .SqlQueryRaw<UserRoleFullRow>(
                "SELECT ur.UserId, ur.RoleId, r.Name AS RoleName, r.Name AS DisplayName " +
                "FROM userrole ur " +
                "JOIN roles r ON r.Id = ur.RoleId " +
                "WHERE ur.UserId = {0} LIMIT 1",
                userId)
            .ToListAsync(ct);
        var role = roleRows.Count > 0 ? roleRows[0] : null;

        var lastLogin = await _db.LoginAudit
            .AsNoTracking()
            .Where(a => a.UserId == userId && a.Success)
            .OrderByDescending(a => a.LoginTime)
            .Select(a => (DateTime?)a.LoginTime)
            .FirstOrDefaultAsync(ct);

        var roleTypes = await GetRoleTypesAsync(ct);
        var permissions = await GetUserPermissionsAsync(userId, ct);

        var primaryUserContact = await _db.UserContacts
            .AsNoTracking()
            .Where(c => c.UserId == userId)
            .OrderByDescending(c => c.IsPrimary)
            .ThenBy(c => c.Id)
            .FirstOrDefaultAsync(ct);

        var contact = primaryUserContact is not null
            ? new StaffContactDto
            {
                WorkEmail = user.Email,
                AltEmail = primaryUserContact.Email,
                PhoneHome = primaryUserContact.Phone,
                PhoneMobile = primaryUserContact.Phone,
                PhoneEmergency = primaryUserContact.Phone,
                WorkLocation = staff?.WorkLocation ?? string.Empty,
                EmploymentType = staff?.EmploymentType ?? string.Empty,
                ContractType = staff?.ContractType ?? string.Empty,
                HireDate = staff?.HireDate ?? DateOnly.FromDateTime(DateTime.Today),
                EndDate = staff?.EndDate,
            }
            : (user.StaffId.HasValue
                ? await GetStaffContactAsync(user.StaffId.Value, ct)
                : null);

        var classes = user.StaffId.HasValue
            ? await GetStaffClassAssignmentsAsync(user.StaffId.Value, ct)
            : [];

        var userDevices = await _db.UserDevices
            .AsNoTracking()
            .Where(d => d.UserId == userId)
            .OrderByDescending(d => d.AssignedDate)
            .ToListAsync(ct);

        var devices = userDevices.Count > 0
            ? userDevices.Select(d => new StaffDeviceRowDto
            {
                Id = d.Id,
                DeviceType = d.DeviceType,
                SerialNumber = d.SerialNumber ?? string.Empty,
                AssignedDate = d.AssignedDate,
                ReturnedDate = d.ReturnedDate,
            }).ToList()
            : (user.StaffId.HasValue
                ? await GetStaffDevicesAsync(user.StaffId.Value, ct)
                : []);

        var userExternal = await _db.UserExternalAccounts
            .AsNoTracking()
            .Where(e => e.UserId == userId)
            .OrderBy(e => e.SystemName)
            .ToListAsync(ct);

        var external = userExternal.Count > 0
            ? userExternal.Select(e => new StaffExternalRowDto
            {
                Id = e.Id,
                SystemName = e.SystemName,
                SystemCode = e.SystemCode,
                AccountUsername = e.AccountUsername ?? string.Empty,
                AccountEmail = e.AccountEmail ?? string.Empty,
                Status = e.Status,
            }).ToList()
            : (user.StaffId.HasValue
                ? await GetStaffExternalAccountsAsync(user.StaffId.Value, ct)
                : []);

        var absences = user.StaffId.HasValue
            ? await GetStaffAbsencesAsync(user.StaffId.Value, ct)
            : [];

        var loginAudit = await GetUserLoginAuditAsync(userId, ct);

        var otherStaffRelatedAuditEntries = new List<string>();
        if (user.StaffId.HasValue)
        {
            var staffId = user.StaffId.Value;

            var deviceAudit = await _db.StaffDeviceAudit
                .AsNoTracking()
                .Where(a => a.StaffId == staffId)
                .OrderByDescending(a => a.Id) // ChangeTime removed
                .Take(10)
                .ToListAsync(ct);
            otherStaffRelatedAuditEntries.AddRange(
                deviceAudit.Select(a => $"staff_device_audit:{a.Action}"));

            var externalAudit = await _db.StaffExternalAccountAudit
                .AsNoTracking()
                .Where(a => a.StaffId == staffId)
                .OrderByDescending(a => a.Id) // ChangeTime removed
                .Take(10)
                .ToListAsync(ct);
            otherStaffRelatedAuditEntries.AddRange(
                externalAudit.Select(a => $"staff_external_account_audit:{a.Action}"));
        }

        var staffContacts = contact is null
            ? new List<string>()
            : new List<string>
            {
                contact.WorkEmail,
                contact.AltEmail ?? string.Empty,
                contact.PhoneHome ?? string.Empty,
                contact.PhoneMobile ?? string.Empty,
                contact.PhoneEmergency ?? string.Empty,
                contact.WorkLocation,
            }.Where(v => !string.IsNullOrWhiteSpace(v)).Distinct().ToList();

        var staffEmployment = contact is null
            ? new List<string>()
            : new List<string>
            {
                contact.EmploymentType,
                contact.ContractType,
                contact.HireDate.ToString("yyyy-MM-dd"),
                contact.EndDate?.ToString("yyyy-MM-dd") ?? string.Empty,
            }.Where(v => !string.IsNullOrWhiteSpace(v)).Distinct().ToList();

        if (profile is null)
        {
            return new UserProfileFullDetailDto
            {
                UserExists              = true,
                UserId                  = user.Id,
                StaffId                 = user.StaffId,
                Username                = user.Username,
                Email                   = user.Email,
                Status                  = user.Status,
                IsAdmin                 = user.IsAdmin,
                UserCreatedAt           = user.CreatedAt,
                UserUpdatedAt           = user.UpdatedAt,
                RoleId                  = role?.RoleId ?? 0,
                RoleName                = role?.RoleName ?? string.Empty,
                RoleDisplayName         = role?.DisplayName ?? string.Empty,
                RoleTypes               = roleTypes,
                Permissions             = permissions,
                FullName                = staff is not null
                                            ? $"{staff.FirstName} {staff.LastName}".Trim()
                                            : user.Username,
                LastLoginAt             = lastLogin,
                ProfilePictureUrl       = null,
                ProfileId               = 0,
                ProfileExists           = false,
                FirstName               = staff?.FirstName ?? string.Empty,
                LastName                = staff?.LastName ?? string.Empty,
                PreferredName           = staff?.PreferredName,
                Title                   = staff?.Title ?? string.Empty,
                Bio                     = null,
                Gender                  = staff?.Gender,
                Timezone                = "UTC",
                LanguageCode            = "en",
                DepartmentId            = staff?.DepartmentId ?? 0,
                JobTitleId              = staff?.JobTitleId ?? 0,
                SchoolId                = 0,
                ProfileCreatedAt        = null,
                ProfileUpdatedAt        = null,
                StaffNumber             = staff?.StaffNumber ?? string.Empty,
                StaffFirstName          = staff?.FirstName ?? string.Empty,
                StaffLastName           = staff?.LastName ?? string.Empty,
                StaffPreferredName      = staff?.PreferredName,
                StaffTitle              = staff?.Title ?? string.Empty,
                DateOfBirth             = staff?.DateOfBirth.ToDateTime(TimeOnly.MinValue) ?? default,
                StaffGender             = staff?.Gender,
                WorkEmail               = staff?.WorkEmail ?? string.Empty,
                AltEmail                = staff?.AltEmail,
                PhoneHome               = staff?.PhoneHome,
                PhoneMobile             = staff?.PhoneMobile,
                PhoneEmergency          = staff?.PhoneEmergency,
                EmploymentType          = staff?.EmploymentType ?? string.Empty,
                ContractType            = staff?.ContractType ?? string.Empty,
                HireDate                = staff?.HireDate,
                EndDate                 = staff?.EndDate,
                WorkLocation            = staff?.WorkLocation ?? string.Empty,
                ReportingManagerId      = staff?.ReportingManagerId,
                StaffJobTitleId         = staff?.JobTitleId ?? 0,
                StaffJobGroupId         = staff?.JobGroupId ?? 0,
                StaffDepartmentId       = staff?.DepartmentId ?? 0,
                StaffProfilePhotoUrl    = staff?.ProfilePhotoUrl,
                AccountStatus           = staff?.AccountStatus ?? string.Empty,
                DepartmentName          = string.Empty,
                Contact                 = contact,
                Classes                 = classes,
                StaffDevices            = devices,
                StaffExternalAccounts   = external,
                StaffAbsences           = absences,
                StaffLoginAudit         = loginAudit,
                StaffLocations          = staff is not null && !string.IsNullOrWhiteSpace(staff.WorkLocation) ? new[] { staff.WorkLocation } : [],
                StaffPhases             = [],
                StaffQualifications     = [],
                StaffAttendance         = [],
                StaffMedical            = [],
                StaffContacts           = staffContacts,
                StaffEmployment         = staffEmployment,
                OtherStaffRelatedTables = new[] { "staff_device_audit", "staff_external_account_audit" },
                OtherStaffRelatedAuditEntries = otherStaffRelatedAuditEntries,
            };
        }

        return new UserProfileFullDetailDto
        {
            UserExists              = true,
            UserId                  = user.Id,
            StaffId                 = user.StaffId,
            Username                = user.Username,
            Email                   = user.Email,
            Status                  = user.Status,
            IsAdmin                 = user.IsAdmin,
            UserCreatedAt           = user.CreatedAt,
            UserUpdatedAt           = user.UpdatedAt,
            RoleId                  = role?.RoleId ?? 0,
            RoleName                = role?.RoleName ?? string.Empty,
            RoleDisplayName         = role?.DisplayName ?? string.Empty,
            RoleTypes               = roleTypes,
            Permissions             = permissions,
            FullName                = staff is not null
                                        ? $"{staff.FirstName} {staff.LastName}".Trim()
                                        : user.Username,
            LastLoginAt             = lastLogin,
            ProfilePictureUrl       = profile.ProfilePictureUrl,

            ProfileId               = profile.Id,
            ProfileExists           = true,
            FirstName               = staff?.FirstName ?? string.Empty,
            LastName                = staff?.LastName ?? string.Empty,
            PreferredName           = staff?.PreferredName,
            Title                   = staff?.Title ?? string.Empty,
            Bio                     = profile.Bio,
            Gender                  = staff?.Gender,
            Timezone                = profile.Timezone      ?? "UTC",
            LanguageCode            = profile.LanguageCode  ?? "en",
            DepartmentId            = staff?.DepartmentId ?? 0,
            JobTitleId              = staff?.JobTitleId ?? 0,
            SchoolId                = 0,
            ProfileCreatedAt        = profile.CreatedAt,
            ProfileUpdatedAt        = profile.UpdatedAt,

            StaffNumber             = staff?.StaffNumber ?? string.Empty,
            StaffFirstName          = staff?.FirstName ?? string.Empty,
            StaffLastName           = staff?.LastName ?? string.Empty,
            StaffPreferredName      = staff?.PreferredName,
            StaffTitle              = staff?.Title ?? string.Empty,
            DateOfBirth             = staff == null ? default : staff.DateOfBirth.ToDateTime(TimeOnly.MinValue),
            StaffGender             = staff?.Gender,
            WorkEmail               = staff?.WorkEmail ?? string.Empty,
            AltEmail                = staff?.AltEmail,
            PhoneHome               = staff?.PhoneHome,
            PhoneMobile             = staff?.PhoneMobile,
            PhoneEmergency          = staff?.PhoneEmergency,
            EmploymentType          = staff?.EmploymentType ?? string.Empty,
            ContractType            = staff?.ContractType ?? string.Empty,
            HireDate                = staff?.HireDate,
            EndDate                 = staff?.EndDate,
            WorkLocation            = staff?.WorkLocation ?? string.Empty,
            ReportingManagerId      = staff?.ReportingManagerId,
            StaffJobTitleId         = staff?.JobTitleId ?? 0,
            StaffJobGroupId         = staff?.JobGroupId ?? 0,
            StaffDepartmentId       = staff?.DepartmentId ?? 0,
            StaffProfilePhotoUrl    = staff?.ProfilePhotoUrl,
            AccountStatus           = staff?.AccountStatus ?? string.Empty,

            DepartmentName          = string.Empty,

            Contact                 = contact,
            Classes                 = classes,
            StaffDevices            = devices,
            StaffExternalAccounts   = external,
            StaffAbsences           = absences,
            StaffLoginAudit         = loginAudit,

            StaffLocations          = staff is not null && !string.IsNullOrWhiteSpace(staff.WorkLocation) ? new[] { staff.WorkLocation } : [],
            StaffPhases             = [],
            StaffQualifications     = [],
            StaffAttendance         = [],
            StaffMedical            = [],
            StaffContacts           = staffContacts,
            StaffEmployment         = staffEmployment,
            OtherStaffRelatedTables = new[] { "staff_device_audit", "staff_external_account_audit" },
            OtherStaffRelatedAuditEntries = otherStaffRelatedAuditEntries,
        };
    }

    public async Task<StaffContactDto?> GetStaffContactAsync(
        int staffId, CancellationToken ct = default)
    {
        var s = await _db.Staff
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == staffId, ct);

        if (s is null) return null;

        return new StaffContactDto
        {
            WorkEmail      = s.WorkEmail      ?? string.Empty,
            AltEmail       = s.AltEmail,
            PhoneHome      = s.PhoneHome,
            PhoneMobile    = s.PhoneMobile,
            PhoneEmergency = s.PhoneEmergency,
            WorkLocation   = s.WorkLocation   ?? string.Empty,
            EmploymentType = s.EmploymentType ?? string.Empty,
            ContractType   = s.ContractType   ?? string.Empty,
            HireDate       = s.HireDate,
            EndDate        = s.EndDate,
        };
    }

    public Task<IReadOnlyList<StaffClassRowDto>> GetStaffClassAssignmentsAsync(
        int staffId, CancellationToken ct = default)
    {
        // StaffDuty was removed; no assignment source remains in this schema.
        return Task.FromResult<IReadOnlyList<StaffClassRowDto>>([]);
    }

    public async Task<IReadOnlyList<StaffDeviceRowDto>> GetStaffDevicesAsync(
        int staffId, CancellationToken ct = default)
    {
        var devices = await _db.StaffDevices
            .AsNoTracking()
            .Where(d => d.StaffId == staffId)
            .OrderByDescending(d => d.AssignedDate)
            .ToListAsync(ct);

        if (devices.Count == 0) return [];

        return devices.Select(d => new StaffDeviceRowDto
        {
            Id            = d.Id,
            DeviceType    = d.DeviceType,
            SerialNumber  = d.DeviceIdentifier,
            AssignedDate  = d.AssignedDate,
            ReturnedDate  = d.ReturnedDate,
        }).ToList();
    }

    public async Task<IReadOnlyList<StaffExternalRowDto>> GetStaffExternalAccountsAsync(
        int staffId, CancellationToken ct = default)
    {
        var accounts = await _db.StaffExternalAccounts
            .AsNoTracking()
            .Where(a => a.StaffId == staffId)
            .ToListAsync(ct);

        if (accounts.Count == 0) return [];

        var sysIds = accounts.Select(a => a.ExternalSystemId).Distinct().ToList();
        var sysMap = await _db.ExternalSystems.AsNoTracking()
            .Where(s => sysIds.Contains(s.Id))
            .ToDictionaryAsync(s => s.Id, s => new { s.Name, s.Code }, ct);

        return accounts.Select(a =>
        {
            sysMap.TryGetValue(a.ExternalSystemId, out var sys);
            return new StaffExternalRowDto
            {
                Id              = a.Id,
                SystemName      = sys?.Name ?? string.Empty,
                SystemCode      = sys?.Code ?? string.Empty,
                AccountUsername = a.ExternalUsername,
                AccountEmail    = string.Empty,
                Status          = a.Status,
            };
        }).ToList();
    }

    public async Task<IReadOnlyList<StaffAbsenceRowDto>> GetStaffAbsencesAsync(
        int staffId, CancellationToken ct = default)
    {
        return await _db.Absences
            .AsNoTracking()
            .Include(a => a.AbsenceType)
            .Include(a => a.Status)
            .Where(a => a.PersonType == "Staff" && a.PersonId == (long)staffId)
            .OrderByDescending(a => a.StartDate)
            .Select(a => new StaffAbsenceRowDto
            {
                Id           = (long)a.Id,
                AbsenceType  = a.AbsenceType != null ? a.AbsenceType.Name : string.Empty,
                Status       = a.Status      != null ? a.Status.Name      : string.Empty,
                StartDate    = a.StartDate,
                EndDate      = a.EndDate,
                DurationDays = a.DurationDays,
                ReportedVia  = a.ReportedVia,
                Notes        = a.Notes,
                ApprovedAt   = a.ApprovedAt,
            })
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<LoginAuditRowDto>> GetUserLoginAuditAsync(
        int userId, CancellationToken ct = default)
    {
        return await _db.LoginAudit
            .AsNoTracking()
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.LoginTime)
            .Take(200)
            .Select(a => new LoginAuditRowDto
            {
                Id        = a.Id,
                LoginTime = a.LoginTime,
                IpAddress = a.IpAddress ?? string.Empty,
                UserAgent = a.UserAgent ?? string.Empty,
                Success   = a.Success,
            })
            .ToListAsync(ct);
    }

    public async Task SaveUserProfileAsync(
        UserProfileSaveDto dto, CancellationToken ct = default)
    {
        // Update User record.
        var user = await _db.Users.FindAsync([dto.UserId], ct)
                   ?? throw new KeyNotFoundException($"User {dto.UserId} not found.");

        user.Username  = dto.Username.Trim();
        user.Email     = dto.Email.Trim();
        user.Status    = dto.Status;
        user.IsAdmin   = dto.IsAdmin;
        user.UpdatedAt = DateTime.UtcNow;

        // Update role via raw SQL.
        if (dto.RoleId > 0)
        {
            var existsCount = await _db.Database
                .SqlQueryRaw<int>("SELECT COUNT(*) AS Value FROM userrole WHERE UserId = {0}", dto.UserId)
                .FirstOrDefaultAsync(ct);
            if (existsCount == 0)
            {
                await _db.Database.ExecuteSqlRawAsync(
                    "INSERT INTO userrole (UserId, RoleId, AssignedAt, AssignedBy) VALUES ({0}, {1}, {2}, {0})",
                    dto.UserId, dto.RoleId, DateTime.UtcNow);
            }
            else
            {
                await _db.Database.ExecuteSqlRawAsync(
                    "UPDATE userrole SET RoleId = {0} WHERE UserId = {1}",
                    dto.RoleId, dto.UserId);
            }
        }

        // Upsert UserProfile record.
        var profile = await _db.UserProfiles
            .FirstOrDefaultAsync(p => p.UserId == dto.UserId, ct);

        if (profile is null)
        {
            profile = new UserProfile
            {
                UserId          = dto.UserId,
                DisplayName     = string.Empty,
                ThemePreference = "default",
                CreatedAt       = DateTime.UtcNow,
            };
            _db.UserProfiles.Add(profile);
        }

        profile.Bio          = dto.Bio;
        profile.Timezone     = dto.Timezone;
        profile.LanguageCode = dto.LanguageCode;
        profile.DisplayName  = $"{dto.FirstName} {dto.LastName}".Trim();
        profile.UpdatedAt    = DateTime.UtcNow;

        if (user.StaffId.HasValue)
        {
            var staff = await _db.Staff.FirstOrDefaultAsync(s => s.Id == user.StaffId.Value, ct);
            if (staff is not null)
            {
                staff.FirstName    = dto.FirstName.Trim();
                staff.LastName     = dto.LastName.Trim();
                staff.PreferredName = string.IsNullOrWhiteSpace(dto.PreferredName) ? null : dto.PreferredName.Trim();
                staff.Title        = dto.Title.Trim();
                staff.Gender       = string.IsNullOrWhiteSpace(dto.Gender) ? null : dto.Gender.Trim();
                staff.DateOfBirth  = DateOnly.FromDateTime(dto.DateOfBirth.Date);

                if (dto.DepartmentId > 0) staff.DepartmentId = dto.DepartmentId;
                if (dto.JobTitleId > 0) staff.JobTitleId = dto.JobTitleId;

                staff.UpdatedAt = DateTime.UtcNow;
            }
        }

        await _db.SaveChangesAsync(ct);
    }

    public async Task ChangePasswordAsync(
        ChangePasswordDto dto, CancellationToken ct = default)
    {
        var user = await _db.Users.FindAsync([dto.UserId], ct)
                   ?? throw new KeyNotFoundException($"User {dto.UserId} not found.");

        if (!VerifyPassword(dto.OldPassword, user.Password))
            throw new InvalidOperationException("Current password is incorrect.");

        user.Password  = HashPassword(dto.NewPassword);
        user.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync(ct);
    }

    public async Task UpdateProfilePhotoAsync(
        int userId, string photoUrl, CancellationToken ct = default)
    {
        var profile = await _db.UserProfiles
            .FirstOrDefaultAsync(p => p.UserId == userId, ct);

        if (profile is null)
        {
            _db.UserProfiles.Add(new UserProfile
            {
                UserId            = userId,
                ProfilePictureUrl = photoUrl,
                DisplayName       = string.Empty,
                ThemePreference   = "default",
                Timezone          = "UTC",
                LanguageCode      = "en",
                CreatedAt         = DateTime.UtcNow,
                UpdatedAt         = DateTime.UtcNow,
            });
        }
        else
        {
            profile.ProfilePictureUrl = photoUrl;
            profile.UpdatedAt         = DateTime.UtcNow;
        }

        await _db.SaveChangesAsync(ct);
    }
}