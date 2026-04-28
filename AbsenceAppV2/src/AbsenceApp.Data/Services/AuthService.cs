/*
===============================================================================
 File        : AuthService.cs
 Namespace   : AbsenceApp.Data.Services
 Author      : Michael
 Version     : 1.2.0
 Created     : 2026-03-22
 Updated     : 2026-04-19
-------------------------------------------------------------------------------
 Purpose     : Provides authentication services for the AbsenceApp platform,
               including user login, registration, and logout operations.

               This service validates credentials against the Users table in
               the application database and returns a structured AuthResultDto
               describing the outcome of the authentication attempt.

               The service is designed to be consumed by UI layers (MAUI /
               Blazor Hybrid) and must be registered with a Scoped lifetime to
               ensure safe usage of the underlying EF Core DbContext.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-22  Initial implementation of login and registration logic
                         using AppDbContext (Phase 1).
   - 1.2.0  2026-04-19  RoleId resolution fix: replaced _db.RoleTypes lookup
                         via user.RoleTypeId with raw SQL through the
                         userrole → roles → roletypes chain. Eliminates the
                         "Unknown column 'u.RoleTypeId'" startup crash.
   - 1.3.0  2026-04-19  Authentication fix: VerifyPassword now detects BCrypt
                         hashes ($2a$/$2b$) from seeded accounts and verifies
                         them via BCrypt.Net-Next. PBKDF2 and plain-text
                         fallback paths retained for forward compatibility.
-------------------------------------------------------------------------------
 Notes       :
   - Seeded accounts have BCrypt ($2a$12$) hashed passwords.
   - New users created via UserManagementService have PBKDF2-SHA256 hashes.
   - Legacy dev accounts with plain-text passwords also authenticate via
     the VerifyPassword fallback.
   - No server-side session state is maintained; authentication state is
     managed by the client application.
   - AppDbContext MUST be registered as Scoped. AuthService MUST NOT be
     registered as Singleton.
===============================================================================
*/

using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;
using AbsenceApp.Data.Context;
using AbsenceApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace AbsenceApp.Data.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _db;

    public AuthService(AppDbContext db) => _db = db;

    /// <summary>
    /// Validates username and password against the Users table.
    /// Supports both PBKDF2-hashed passwords (new users created via
    /// UserManagementService) and legacy plain-text passwords (dev-only accounts).
    /// </summary>
    public async Task<AuthResultDto> LoginAsync(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            return new AuthResultDto { Success = false, ErrorMessage = "Username and password are required." };

        var user = await _db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Username == username);

        if (user is null)
            return new AuthResultDto { Success = false, ErrorMessage = "Invalid username or password." };

        // Verify using PBKDF2-aware comparison (falls back to plain-text for
        // dev accounts whose passwords have not yet been re-hashed).
        if (!UserManagementService.VerifyPassword(password, user.Password))
            return new AuthResultDto { Success = false, ErrorMessage = "Invalid username or password." };

        // Resolve role display name via userrole → roles → roletypes
        var roleDisplayNames = await _db.Database
            .SqlQueryRaw<string>(
                "SELECT rt.DisplayName " +
                "FROM userrole ur " +
                "INNER JOIN roles r  ON r.Id  = ur.RoleId " +
                "INNER JOIN roletypes rt ON rt.Id = r.RoleTypeId " +
                "WHERE ur.UserId = @UserId LIMIT 1",
                new MySqlParameter("@UserId", user.Id))
            .ToListAsync();
        var roleDisplayName = roleDisplayNames.FirstOrDefault()
                              ?? (user.IsAdmin ? "Admin" : "Staff");

        return new AuthResultDto
        {
            Success  = true,
            UserId   = user.Id,
            UserName = user.Username,
            Role     = roleDisplayName
        };
    }

    /// <summary>
    /// Creates a new user account. Username is derived from the email prefix.
    /// NOTE: Password stored as plain text — replace with hashed value in production.
    /// </summary>
    public async Task<AuthResultDto> RegisterAsync(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            return new AuthResultDto { Success = false, ErrorMessage = "Email and password are required." };

        var atIndex = email.IndexOf('@');
        var username = atIndex > 0 ? email[..atIndex] : email;

        var existing = await _db.Users
            .AsNoTracking()
            .AnyAsync(u => u.Username == username || u.Email == email);

        if (existing)
            return new AuthResultDto { Success = false, ErrorMessage = "An account with that email already exists." };

        var now  = DateTime.UtcNow;
        var user = new User
        {
            Username  = username,
            Email     = email,
            // TODO: replace plain-text with hashed password before production deployment
            Password  = password,
            Status    = "Active",
            IsAdmin   = false,
            CreatedAt = now,
            UpdatedAt = now
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return new AuthResultDto
        {
            Success  = true,
            UserId   = user.Id,
            UserName = user.Username,
            Role     = "Staff"
        };
    }

    /// <summary>No server-side session to clear in a MAUI local app.</summary>
    public Task LogoutAsync() => Task.CompletedTask;
}
