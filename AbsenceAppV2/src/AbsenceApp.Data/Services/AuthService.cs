/*
===============================================================================
 File        : AuthService.cs
 Namespace   : AbsenceApp.Data.Services
 Author      : Michael
 Version     : 1.3.1
 Created     : 2026-03-22
 Updated     : 2026-07-01
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
   - 1.3.1  2026-07-01  FIX: Replaced injected AppDbContext with
                         IDbContextFactory<AppDbContext> and updated all
                         methods to use short-lived DbContext instances to
                         prevent EF Core concurrency exceptions in Blazor/Maui.
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
    private readonly IDbContextFactory<AppDbContext> _factory;

    public AuthService(IDbContextFactory<AppDbContext> factory)
    {
        _factory = factory;
    }

    /// <summary>
    /// Validates username and password against the Users table.
    /// Supports both PBKDF2-hashed passwords (new users created via
    /// UserManagementService) and legacy plain-text passwords (dev-only accounts).
    /// </summary>
    public async Task<AuthResultDto> LoginAsync(string username, string password)
    {
        using var db = _factory.CreateDbContext();

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            return new AuthResultDto { Success = false, ErrorMessage = "Username and password are required." };

        var user = await db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Username == username);

        if (user is null)
        {
            db.LoginAudit.Add(new LoginAudit
            {
                UserId        = null,
                LoginTime     = DateTime.UtcNow,
                IpAddress     = null,
                UserAgent     = null,
                Success       = false,
                FailureReason = "Unknown username",
            });
            await db.SaveChangesAsync();
            return new AuthResultDto { Success = false, ErrorMessage = "Invalid username or password." };
        }

        if (!UserManagementService.VerifyPassword(password, user.Password))
        {
            db.LoginAudit.Add(new LoginAudit
            {
                UserId        = user.Id,
                LoginTime     = DateTime.UtcNow,
                IpAddress     = null,
                UserAgent     = null,
                Success       = false,
                FailureReason = "Invalid password",
            });
            await db.SaveChangesAsync();
            return new AuthResultDto { Success = false, ErrorMessage = "Invalid username or password." };
        }

        var roleDisplayNames = await db.Database
            .SqlQueryRaw<string>(
                "SELECT r.Name " +
                "FROM userrole ur " +
                "INNER JOIN roles r ON r.Id = ur.RoleId " +
                "WHERE ur.UserId = @UserId LIMIT 1",
                new MySqlParameter("@UserId", user.Id))
            .ToListAsync();

        var roleDisplayName = roleDisplayNames.FirstOrDefault()
                              ?? (user.IsAdmin ? "Admin" : "Staff");

        db.LoginAudit.Add(new LoginAudit
        {
            UserId    = user.Id,
            LoginTime = DateTime.UtcNow,
            IpAddress = null,
            UserAgent = null,
            Success   = true,
        });
        await db.SaveChangesAsync();

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
        using var db = _factory.CreateDbContext();

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            return new AuthResultDto { Success = false, ErrorMessage = "Email and password are required." };

        var atIndex = email.IndexOf('@');
        var username = atIndex > 0 ? email[..atIndex] : email;

        var existing = await db.Users
            .AsNoTracking()
            .AnyAsync(u => u.Username == username || u.Email == email);

        if (existing)
            return new AuthResultDto { Success = false, ErrorMessage = "An account with that email already exists." };

        var now  = DateTime.UtcNow;
        var user = new User
        {
            Username  = username,
            Email     = email,
            Password  = password,
            Status    = "Active",
            IsAdmin   = false,
            CreatedAt = now,
            UpdatedAt = now
        };

        db.Users.Add(user);
        await db.SaveChangesAsync();

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
