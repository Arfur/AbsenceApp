using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;
using AbsenceApp.Data.Context;
using AbsenceApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AbsenceApp.Data.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _db;

    public AuthService(AppDbContext db) => _db = db;

    /// <summary>
    /// Validates username and password against the Users table.
    /// NOTE: Passwords are currently stored and compared as plain text for
    /// development purposes. In production, replace with a proper hashing
    /// algorithm such as BCrypt or PBKDF2.
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

        // TODO: replace plain-text comparison with BCrypt.Verify or equivalent
        if (user.Password != password)
            return new AuthResultDto { Success = false, ErrorMessage = "Invalid username or password." };

        var roleType = await _db.RoleTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == user.RoleTypeId);

        return new AuthResultDto
        {
            Success  = true,
            UserId   = user.Id,
            UserName = user.Username,
            Role     = roleType?.DisplayName ?? (user.IsAdmin ? "Admin" : "Staff")
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
            Name      = username,
            Username  = username,
            Email     = email,
            // TODO: replace plain-text with hashed password before production deployment
            Password  = password,
            RoleTypeId = 2,   // default to Staff role
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
