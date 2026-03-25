/*
===============================================================================
 File        : AuthServiceTests.cs
 Namespace   : AbsenceApp.Tests.ServiceTests
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-20
 Updated     : 2026-03-20
-------------------------------------------------------------------------------
 Purpose     : xUnit tests for AuthService using EF Core InMemory provider.
               Covers LoginAsync and RegisterAsync happy/sad paths.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-20  Initial creation.
-------------------------------------------------------------------------------
===============================================================================
*/

using AbsenceApp.Data.Context;
using AbsenceApp.Data.Models;
using AbsenceApp.Data.Services;
using Microsoft.EntityFrameworkCore;

namespace AbsenceApp.Tests.ServiceTests;

public class AuthServiceTests
{
    // =========================================================================
    // Helpers
    // =========================================================================

    private static AppDbContext CreateContext()
    {
        var opts = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(opts);
    }

    private static User SeedUser(AppDbContext ctx,
        string username = "testuser", string password = "pass123")
    {
        var now  = DateTime.UtcNow;
        var user = new User
        {
            Name       = username,
            Username   = username,
            Email      = $"{username}@example.com",
            Password   = password,
            RoleTypeId = 2,
            Status     = "Active",
            CreatedAt  = now,
            UpdatedAt  = now,
        };
        ctx.Users.Add(user);
        ctx.SaveChanges();
        return user;
    }

    // =========================================================================
    // LoginAsync tests
    // =========================================================================

    [Fact]
    public async Task LoginAsync_ValidCredentials_ReturnsSuccess()
    {
        using var ctx = CreateContext();
        SeedUser(ctx, "alice", "secret");
        var sut = new AuthService(ctx);

        var result = await sut.LoginAsync("alice", "secret");

        Assert.True(result.Success);
        Assert.Equal("alice", result.UserName);
        Assert.Null(result.ErrorMessage);
    }

    [Fact]
    public async Task LoginAsync_WrongPassword_ReturnsFailure()
    {
        using var ctx = CreateContext();
        SeedUser(ctx, "alice", "secret");
        var sut = new AuthService(ctx);

        var result = await sut.LoginAsync("alice", "wrong");

        Assert.False(result.Success);
        Assert.NotNull(result.ErrorMessage);
    }

    [Fact]
    public async Task LoginAsync_UnknownUsername_ReturnsFailure()
    {
        using var ctx = CreateContext();
        var sut = new AuthService(ctx);

        var result = await sut.LoginAsync("nobody", "pass");

        Assert.False(result.Success);
        Assert.NotNull(result.ErrorMessage);
    }

    [Theory]
    [InlineData("", "pass")]
    [InlineData("user", "")]
    [InlineData("", "")]
    public async Task LoginAsync_EmptyCredentials_ReturnsFailure(string username, string password)
    {
        using var ctx = CreateContext();
        var sut = new AuthService(ctx);

        var result = await sut.LoginAsync(username, password);

        Assert.False(result.Success);
    }

    // =========================================================================
    // RegisterAsync tests
    // =========================================================================

    [Fact]
    public async Task RegisterAsync_NewEmail_CreatesUserAndReturnsSuccess()
    {
        using var ctx = CreateContext();
        var sut = new AuthService(ctx);

        var result = await sut.RegisterAsync("newuser@school.edu", "p@ss1");

        Assert.True(result.Success);
        Assert.Equal("newuser", result.UserName);
        Assert.True(await ctx.Users.AnyAsync(u => u.Username == "newuser"));
    }

    [Fact]
    public async Task RegisterAsync_DuplicateEmail_ReturnsFailure()
    {
        using var ctx = CreateContext();
        SeedUser(ctx, "bob", "pass");
        var sut = new AuthService(ctx);

        var result = await sut.RegisterAsync("bob@example.com", "newpass");

        // Email matches existing user's email
        Assert.False(result.Success);
    }

    [Fact]
    public async Task RegisterAsync_EmptyEmail_ReturnsFailure()
    {
        using var ctx = CreateContext();
        var sut = new AuthService(ctx);

        var result = await sut.RegisterAsync("", "pass");

        Assert.False(result.Success);
    }

    // =========================================================================
    // LogoutAsync test
    // =========================================================================

    [Fact]
    public async Task LogoutAsync_CompletesWithoutException()
    {
        using var ctx = CreateContext();
        var sut = new AuthService(ctx);

        // Should be a no-op — just verify it doesn't throw
        await sut.LogoutAsync();
    }
}
