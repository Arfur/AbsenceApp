/*
===============================================================================
 File        : UserManagementService.cs
 Namespace   : AbsenceApp.Data.Services
 Author      : Michael
 Version     : 1.1.0
 Created     : 2026-04-11
 Updated     : 2026-04-11
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
-------------------------------------------------------------------------------
 Notes       :
   - Register as Scoped in DataServiceRegistration.cs.
   - UserPagePermission rows where all six flags are false are deleted (not
     inserted as all-false rows) to keep the table lean.
   - RoleTypeName check in GetAllowedRoleNames ensures only the five
     E15-approved role slugs can be assigned: super_admin, admin, staff_admin,
     teacher, office_staff.
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
        var users = await _db.Users
            .AsNoTracking()
            .OrderBy(u => u.LastName).ThenBy(u => u.FirstName)
            .ToListAsync(ct);

        var roleIds   = users.Select(u => u.RoleTypeId).Distinct().ToList();
        var roleNames = await _db.RoleTypes
            .AsNoTracking()
            .Where(r => roleIds.Contains(r.Id))
            .ToDictionaryAsync(r => r.Id, r => r.DisplayName, ct);

        return users.Select(u => new UserListItemDto
        {
            Id           = u.Id,
            FullName     = BuildFullName(u),
            Username     = u.Username,
            Email        = u.Email,
            PhoneNumber  = u.PhoneNumber,
            RoleTypeName = roleNames.TryGetValue(u.RoleTypeId, out var rn) ? rn : string.Empty,
            Status       = u.Status,
            CreatedAt    = u.CreatedAt,
        }).ToList();
    }

    public async Task<UserUpdateDto?> GetUserForEditAsync(long userId, CancellationToken ct = default)
    {
        var u = await _db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == userId, ct);

        if (u is null) return null;

        return new UserUpdateDto
        {
            Id          = u.Id,
            FirstName   = u.FirstName,
            LastName    = u.LastName,
            Username    = u.Username,
            Email       = u.Email,
            PhoneNumber = u.PhoneNumber,
            RoleTypeId  = u.RoleTypeId,
            Status      = u.Status,
            NewPassword = null,      // never return stored hash
        };
    }

    public async Task<long> CreateUserAsync(UserCreateDto dto, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(dto.FirstName))    throw new ArgumentException("First name is required.");
        if (string.IsNullOrWhiteSpace(dto.LastName))     throw new ArgumentException("Last name is required.");
        if (string.IsNullOrWhiteSpace(dto.Username))     throw new ArgumentException("Username is required.");
        if (string.IsNullOrWhiteSpace(dto.Email))        throw new ArgumentException("Email is required.");
        if (string.IsNullOrWhiteSpace(dto.Password))     throw new ArgumentException("Password is required.");

        var duplicate = await _db.Users
            .AnyAsync(u => u.Username == dto.Username, ct);
        if (duplicate)
            throw new InvalidOperationException($"Username '{dto.Username}' is already taken.");

        // Assign the next available ID (table does not use IDENTITY)
        // Users table uses long PK with ValueGeneratedNever (CSV import pipeline).
        var maxId = await _db.Users.MaxAsync(u => (long?)u.Id, ct) ?? 0L;

        var user = new User
        {
            Id          = maxId + 1,
            FirstName   = dto.FirstName.Trim(),
            LastName    = dto.LastName.Trim(),
            Name        = $"{dto.FirstName.Trim()} {dto.LastName.Trim()}",
            Username    = dto.Username.Trim(),
            Email       = dto.Email.Trim(),
            PhoneNumber = dto.PhoneNumber?.Trim(),
            Password    = HashPassword(dto.Password),
            RoleTypeId  = dto.RoleTypeId,
            Status      = ActiveStatus,
            CreatedAt   = DateTime.UtcNow,
            UpdatedAt   = DateTime.UtcNow,
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync(ct);
        return user.Id;
    }

    public async Task UpdateUserAsync(UserUpdateDto dto, CancellationToken ct = default)
    {
        var user = await _db.Users.FindAsync([dto.Id], ct)
                   ?? throw new KeyNotFoundException($"User {dto.Id} not found.");

        user.FirstName   = dto.FirstName.Trim();
        user.LastName    = dto.LastName.Trim();
        user.Name        = $"{dto.FirstName.Trim()} {dto.LastName.Trim()}";
        user.Username    = dto.Username.Trim();
        user.Email       = dto.Email.Trim();
        user.PhoneNumber = dto.PhoneNumber?.Trim();
        user.RoleTypeId  = dto.RoleTypeId;
        user.Status      = dto.Status;
        user.UpdatedAt   = DateTime.UtcNow;

        if (!string.IsNullOrWhiteSpace(dto.NewPassword))
            user.Password = HashPassword(dto.NewPassword);

        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteUserAsync(long userId, CancellationToken ct = default)
    {
        var user = await _db.Users.FindAsync([userId], ct)
                   ?? throw new KeyNotFoundException($"User {userId} not found.");

        _db.Users.Remove(user);
        await _db.SaveChangesAsync(ct);
    }

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
    /// <see cref="HashPassword"/> or falls back to plain-text comparison
    /// for legacy accounts (development data).
    /// </summary>
    public static bool VerifyPassword(string plainText, string stored)
    {
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
        return u.Name;    // fallback to legacy combined Name field
    }
}
