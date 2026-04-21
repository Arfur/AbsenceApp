/*
===============================================================================
 File        : UserManagementService.cs
 Namespace   : AbsenceApp.Data.Services
 Author      : Michael
 Version     : 1.3.0
 Created     : 2026-04-11
 Updated     : 2026-04-21
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
            : new Dictionary<long, string>();

        // Role display-name lookup via raw SQL (userrole table has no DbSet).
        // Returns rows: UserId, DisplayName
        var userIds = users.Select(u => u.Id).ToList();
        var roleRows = userIds.Count > 0
            ? await _db.Database
                .SqlQueryRaw<UserRoleRow>(
                    "SELECT ur.UserId, rt.DisplayName " +
                    "FROM userrole ur " +
                    "JOIN roles r ON r.Id = ur.RoleId " +
                    "JOIN roletypes rt ON rt.Id = r.RoleTypeId " +
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
            RoleTypeName = userRoleMap.TryGetValue(u.Id, out var rn) ? rn : string.Empty,
            Status       = u.Status,
            CreatedAt    = u.CreatedAt,
        }).ToList();
    }

    // Private projection type for role SQL query.
    private sealed class UserRoleRow
    {
        public long   UserId      { get; set; }
        public string DisplayName { get; set; } = string.Empty;
    }

    public async Task<UserUpdateDto?> GetUserForEditAsync(long userId, CancellationToken ct = default)
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
        var roleId = roleRows.Count > 0 ? (long)roleRows[0].Value : 0L;

        return new UserUpdateDto
        {
            Id          = u.Id,
            StaffId     = u.StaffId,
            Username    = u.Username,
            Email       = u.Email,
            RoleTypeId  = roleId,
            Status      = u.Status,
            NewPassword = null,
        };
    }

    private sealed class UserRoleIdRow { public int Value { get; set; } }

    public async Task<long> CreateUserAsync(UserCreateDto dto, CancellationToken ct = default)
    {
        // ── Mandatory field checks ────────────────────────────────────────────
        if (dto.StaffId <= 0)                    throw new ArgumentException("StaffId is required. Users can only be created from a Staff record.");
        if (string.IsNullOrWhiteSpace(dto.Username))  throw new ArgumentException("Username is required.");
        if (string.IsNullOrWhiteSpace(dto.Email))     throw new ArgumentException("Email is required.");
        if (string.IsNullOrWhiteSpace(dto.Password))  throw new ArgumentException("Password is required.");

        // ── Validate StaffId exists ───────────────────────────────────────────
        var staffExists = await _db.Staff.AnyAsync(s => s.Id == dto.StaffId, ct);
        if (!staffExists)
            throw new InvalidOperationException($"Staff record {dto.StaffId} does not exist.");

        // ── Prevent duplicate user for same StaffId ───────────────────────────
        var staffAlreadyHasUser = await _db.Users.AnyAsync(u => u.StaffId == dto.StaffId, ct);
        if (staffAlreadyHasUser)
            throw new InvalidOperationException($"A user account already exists for Staff #{dto.StaffId}.");

        // ── Prevent duplicate username ────────────────────────────────────────
        var duplicate = await _db.Users.AnyAsync(u => u.Username == dto.Username, ct);
        if (duplicate)
            throw new InvalidOperationException($"Username '{dto.Username}' is already taken.");

        // ── Insert user (auto_increment PK) ───────────────────────────────────
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

        // ── Insert UserRole row via raw SQL (userrole table has no DbSet) ─────
        if (dto.RoleTypeId > 0)
        {
            await _db.Database.ExecuteSqlRawAsync(
                "INSERT INTO userrole (UserId, RoleId, AssignedAt, AssignedBy) VALUES ({0}, {1}, {2}, {0})",
                user.Id, dto.RoleTypeId, DateTime.UtcNow);
        }

        await tx.CommitAsync(ct);
        return user.Id;
    }

    public async Task UpdateUserAsync(UserUpdateDto dto, CancellationToken ct = default)
    {
        var user = await _db.Users.FindAsync([dto.Id], ct)
                   ?? throw new KeyNotFoundException($"User {dto.Id} not found.");

        // StaffId is IMMUTABLE after creation — never update it.
        user.Username  = dto.Username.Trim();
        user.Email     = dto.Email.Trim();
        user.Status    = dto.Status;
        user.UpdatedAt = DateTime.UtcNow;

        if (!string.IsNullOrWhiteSpace(dto.NewPassword))
            user.Password = HashPassword(dto.NewPassword);

        // Update role via raw SQL.
        if (dto.RoleTypeId > 0)
        {
            var existsCount = await _db.Database
                .SqlQueryRaw<int>("SELECT COUNT(*) AS Value FROM userrole WHERE UserId = {0}", dto.Id)
                .FirstOrDefaultAsync();
            if (existsCount == 0)
            {
                await _db.Database.ExecuteSqlRawAsync(
                    "INSERT INTO userrole (UserId, RoleId, AssignedAt, AssignedBy) VALUES ({0}, {1}, {2}, {0})",
                    dto.Id, dto.RoleTypeId, DateTime.UtcNow);
            }
            else
            {
                await _db.Database.ExecuteSqlRawAsync(
                    "UPDATE userrole SET RoleId = {0} WHERE UserId = {1}",
                    dto.RoleTypeId, dto.Id);
            }
        }

        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteUserAsync(long userId, CancellationToken ct = default)
    {
        var user = await _db.Users.FindAsync([userId], ct)
                   ?? throw new KeyNotFoundException($"User {userId} not found.");

        // Delete child records to avoid FK constraint violations.
        // userrole has no EF DbSet — use raw SQL.
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
        long staffId, CancellationToken ct = default)
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
        long staffId, CancellationToken ct = default)
        => await _db.Users.AnyAsync(u => u.StaffId == staffId, ct);

    // =========================================================================
    // Reference data
    // =========================================================================

    public async Task<IReadOnlyList<RoleTypeSelectDto>> GetRoleTypesAsync(CancellationToken ct = default)
    {
        // E15 spec: only these five roles are available for user assignment.
        var allowed = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "super_admin", "admin", "staff_admin", "teacher", "office_staff",
        };

        return await _db.RoleTypes
            .AsNoTracking()
            .Where(r => allowed.Contains(r.Name))
            .OrderBy(r => r.Priority)
            .Select(r => new RoleTypeSelectDto
            {
                Id          = r.Id,
                Name        = r.Name,
                DisplayName = r.DisplayName,
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
        string roleTypeName, CancellationToken ct = default)
    {
        var pages = await _db.AppPages
            .AsNoTracking()
            .Where(p => p.IsActive)
            .OrderBy(p => p.SortOrder)
            .ToListAsync(ct);

        var defaults = await _db.RoleDefaultPagePermissions
            .AsNoTracking()
            .Where(r => r.RoleTypeName == roleTypeName)
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

        // Count users per role type via raw SQL (userrole → roles → roletypes).
        var countRows = await _db.Database
            .SqlQueryRaw<RoleUserCountRow>(
                "SELECT r.RoleTypeId, COUNT(DISTINCT ur.UserId) AS UserCount " +
                "FROM roles r " +
                "LEFT JOIN userrole ur ON ur.RoleId = r.Id " +
                "GROUP BY r.RoleTypeId")
            .ToListAsync(ct);

        var countMap = countRows.ToDictionary(r => r.RoleTypeId, r => r.UserCount);

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
        public long RoleTypeId { get; set; }
        public int  UserCount  { get; set; }
    }

    // =========================================================================
    // Permissions list (features + which role types have each feature)
    // =========================================================================

    public async Task<IReadOnlyList<FeatureListItemDto>> GetFeaturesAsync(CancellationToken ct = default)
    {
        // feature and rolefeature have no EF DbSets — use raw SQL.
        var featureRows = await _db.Database
            .SqlQueryRaw<FeatureRow>(
                "SELECT Id, Code, COALESCE(Description, '') AS Description " +
                "FROM feature ORDER BY Id")
            .ToListAsync(ct);

        var roleFeatureRows = await _db.Database
            .SqlQueryRaw<FeatureRoleRow>(
                "SELECT rf.FeatureCode, rt.DisplayName " +
                "FROM rolefeature rf " +
                "JOIN roles r ON r.Id = rf.RoleId " +
                "JOIN roletypes rt ON rt.Id = r.RoleTypeId " +
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
            Roles    = roleMap.TryGetValue(p.Id, out var r) ? r : "—",
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
    // Private helpers
    // =========================================================================

    private static string BuildFullName(User u)
    {
        if (!string.IsNullOrWhiteSpace(u.FirstName) || !string.IsNullOrWhiteSpace(u.LastName))
            return $"{u.FirstName} {u.LastName}".Trim();
        if (!string.IsNullOrWhiteSpace(u.Name))
            return u.Name;
        return u.Username;    // fallback when no name data loaded from DB
    }

    // =========================================================================
    // User Profile page methods (v1.4.0)
    // =========================================================================

    public async Task<UserProfileHeaderDto?> GetUserProfileHeaderAsync(
        long userId, CancellationToken ct = default)
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
                "SELECT ur.UserId, rt.DisplayName " +
                "FROM userrole ur " +
                "JOIN roles r ON r.Id = ur.RoleId " +
                "JOIN roletypes rt ON rt.Id = r.RoleTypeId " +
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

    public async Task<UserProfileDetailDto> GetUserProfileDetailAsync(
        long userId, CancellationToken ct = default)
    {
        var profile = await _db.UserProfiles
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.UserId == userId, ct);

        if (profile is null)
        {
            return new UserProfileDetailDto { ProfileExists = false };
        }

        return new UserProfileDetailDto
        {
            ProfileId         = profile.Id,
            ProfileExists     = true,
            FirstName         = profile.FirstName ?? string.Empty,
            LastName          = profile.LastName  ?? string.Empty,
            PreferredName     = profile.PreferredName,
            Title             = profile.Title     ?? string.Empty,
            DateOfBirth       = profile.DateOfBirth,
            Bio               = profile.Bio,
            Gender            = profile.Gender,
            Timezone          = profile.Timezone      ?? "UTC",
            LanguageCode      = profile.LanguageCode  ?? "en",
            DepartmentId      = profile.DepartmentId,
            JobTitleId        = profile.JobTitleId,
            ProfilePictureUrl = profile.ProfilePictureUrl,
        };
    }

    public async Task<StaffContactDto?> GetStaffContactAsync(
        long staffId, CancellationToken ct = default)
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

    public async Task<IReadOnlyList<StaffClassRowDto>> GetStaffClassAssignmentsAsync(
        long staffId, CancellationToken ct = default)
    {
        var assignments = await _db.StaffAssignments
            .AsNoTracking()
            .Where(a => a.StaffId == staffId)
            .OrderByDescending(a => a.StartDate)
            .ToListAsync(ct);

        if (assignments.Count == 0) return [];

        // Build lookup dictionaries.
        var classIds = assignments.Where(a => a.ClassId.HasValue).Select(a => a.ClassId!.Value).Distinct().ToList();
        var jtIds    = assignments.Select(a => a.JobTitleId).Distinct().ToList();
        var deptIds  = assignments.Select(a => a.DepartmentId).Distinct().ToList();

        var classMap = classIds.Count > 0
            ? await _db.Classes.AsNoTracking().Where(c => classIds.Contains(c.Id))
                  .ToDictionaryAsync(c => c.Id, c => c.Name, ct)
            : new Dictionary<long, string>();

        var jtMap = jtIds.Count > 0
            ? await _db.JobTitles.AsNoTracking().Where(j => jtIds.Contains(j.Id))
                  .ToDictionaryAsync(j => j.Id, j => j.Title, ct)
            : new Dictionary<long, string>();

        var deptMap = deptIds.Count > 0
            ? await _db.Departments.AsNoTracking().Where(d => deptIds.Contains(d.Id))
                  .ToDictionaryAsync(d => d.Id, d => d.Name, ct)
            : new Dictionary<long, string>();

        return assignments.Select(a => new StaffClassRowDto
        {
            AssignmentId   = a.Id,
            ClassName      = a.ClassId.HasValue && classMap.TryGetValue(a.ClassId.Value, out var cn) ? cn : "—",
            JobTitle       = jtMap.TryGetValue(a.JobTitleId, out var jt) ? jt : string.Empty,
            Department     = deptMap.TryGetValue(a.DepartmentId, out var dn) ? dn : string.Empty,
            StartDate      = a.StartDate,
            EndDate        = a.EndDate,
            DaysOfWeek     = a.DaysOfWeek,
        }).ToList();
    }

    public async Task<IReadOnlyList<StaffDeviceRowDto>> GetStaffDevicesAsync(
        long staffId, CancellationToken ct = default)
    {
        var devices = await _db.StaffDevices
            .AsNoTracking()
            .Where(d => d.StaffId == staffId)
            .OrderByDescending(d => d.AssignedDate)
            .ToListAsync(ct);

        if (devices.Count == 0) return [];

        var typeIds = devices.Select(d => d.DeviceTypeId).Distinct().ToList();
        var typeMap = await _db.DeviceTypes.AsNoTracking()
            .Where(t => typeIds.Contains(t.Id))
            .ToDictionaryAsync(t => t.Id, t => t.Name, ct);

        return devices.Select(d => new StaffDeviceRowDto
        {
            Id            = d.Id,
            DeviceType    = typeMap.TryGetValue(d.DeviceTypeId, out var tn) ? tn : string.Empty,
            SerialNumber  = d.SerialNumber,
            AssignedDate  = d.AssignedDate,
            ReturnedDate  = d.ReturnedDate,
        }).ToList();
    }

    public async Task<IReadOnlyList<StaffExternalRowDto>> GetStaffExternalAccountsAsync(
        long staffId, CancellationToken ct = default)
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
                AccountUsername = a.AccountUsername,
                AccountEmail    = a.AccountEmail,
                Status          = a.Status,
            };
        }).ToList();
    }

    public async Task<IReadOnlyList<StaffAbsenceRowDto>> GetStaffAbsencesAsync(
        long staffId, CancellationToken ct = default)
    {
        return await _db.Absences
            .AsNoTracking()
            .Include(a => a.AbsenceType)
            .Include(a => a.Status)
            .Where(a => a.PersonType == "Staff" && a.PersonId == staffId)
            .OrderByDescending(a => a.StartDate)
            .Select(a => new StaffAbsenceRowDto
            {
                Id           = a.Id,
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
        long userId, CancellationToken ct = default)
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
                IpAddress = a.IpAddress,
                UserAgent = a.UserAgent,
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
        user.UpdatedAt = DateTime.UtcNow;

        // Update role via raw SQL.
        if (dto.RoleTypeId > 0)
        {
            var existsCount = await _db.Database
                .SqlQueryRaw<int>("SELECT COUNT(*) AS Value FROM userrole WHERE UserId = {0}", dto.UserId)
                .FirstOrDefaultAsync(ct);
            if (existsCount == 0)
            {
                await _db.Database.ExecuteSqlRawAsync(
                    "INSERT INTO userrole (UserId, RoleId, AssignedAt, AssignedBy) VALUES ({0}, {1}, {2}, {0})",
                    dto.UserId, dto.RoleTypeId, DateTime.UtcNow);
            }
            else
            {
                await _db.Database.ExecuteSqlRawAsync(
                    "UPDATE userrole SET RoleId = {0} WHERE UserId = {1}",
                    dto.RoleTypeId, dto.UserId);
            }
        }

        // Upsert UserProfile record.
        var profile = await _db.UserProfiles
            .FirstOrDefaultAsync(p => p.UserId == dto.UserId, ct);

        if (profile is null)
        {
            profile = new UserProfile
            {
                UserId    = dto.UserId,
                CreatedAt = DateTime.UtcNow,
                SchoolId  = 1,   // default school; can be parameterised later
            };
            _db.UserProfiles.Add(profile);
        }

        profile.FirstName     = dto.FirstName;
        profile.LastName      = dto.LastName;
        profile.PreferredName = dto.PreferredName;
        profile.Title         = dto.Title;
        profile.DateOfBirth   = dto.DateOfBirth;
        profile.Bio           = dto.Bio;
        profile.Gender        = dto.Gender;
        profile.Timezone      = dto.Timezone;
        profile.LanguageCode  = dto.LanguageCode;
        profile.DepartmentId  = dto.DepartmentId;
        profile.JobTitleId    = dto.JobTitleId;
        profile.UpdatedAt     = DateTime.UtcNow;

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
        long userId, string photoUrl, CancellationToken ct = default)
    {
        var profile = await _db.UserProfiles
            .FirstOrDefaultAsync(p => p.UserId == userId, ct);

        if (profile is null)
        {
            _db.UserProfiles.Add(new UserProfile
            {
                UserId            = userId,
                ProfilePictureUrl = photoUrl,
                FirstName         = string.Empty,
                LastName          = string.Empty,
                Title             = string.Empty,
                Timezone          = "UTC",
                LanguageCode      = "en",
                SchoolId          = 1,
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
