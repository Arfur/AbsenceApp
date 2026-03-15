/*
===============================================================================
 File        : AuditLogRepositoryTests.cs
 Namespace   : AbsenceApp.Tests.RepositoryTests
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-15
 Updated     : 2026-03-15
-------------------------------------------------------------------------------
 Purpose     : xUnit tests for AuditLogRepository (legacy stub) using EF Core InMemory.
               Verifies the stub satisfies the IAuditLogRepository contract with
               the retained AuditLog entity; seeds users via the new TABLE1 schema.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-15  Initial creation.
-------------------------------------------------------------------------------
===============================================================================
*/

using AbsenceApp.Data.Context;
using AbsenceApp.Data.Models;
using AbsenceApp.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AbsenceApp.Tests.RepositoryTests;

/// <summary>
/// Tests for AuditLogRepository (uses legacy AuditLog stub entity).
/// Users are seeded using the new User entity schema.
/// </summary>
public class AuditLogRepositoryTests
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

    private static async Task<int> SeedUserAsync(AppDbContext ctx)
    {
        var user = new User
        {
            Name       = "Admin User",
            Username   = "admin",
            Email      = "admin@test.com",
            Password   = "hashed",
            RoleTypeId = 1,
            Status     = "Active",
            CreatedAt  = DateTime.UtcNow,
            UpdatedAt  = DateTime.UtcNow,
        };
        ctx.Users.Add(user);
        await ctx.SaveChangesAsync();
        return (int)user.Id;
    }

    // =========================================================================
    // Tests
    // =========================================================================

    [Fact]
    public async Task AddAsync_ShouldPersistAuditLog()
    {
        using var ctx = CreateContext();
        var userId = await SeedUserAsync(ctx);
        var repo = new AuditLogRepository(ctx);

        await repo.AddAsync(new AuditLog
        {
            UserId    = userId,
            Action    = "Login",
            Timestamp = DateTime.UtcNow,
        });

        Assert.Single(await repo.ListAsync());
    }

    [Fact]
    public async Task ListAsync_WhenEmpty_ReturnsEmptyCollection()
    {
        using var ctx = CreateContext();
        var repo = new AuditLogRepository(ctx);

        Assert.Empty(await repo.ListAsync());
    }

    [Fact]
    public async Task FindByIdAsync_ExistingId_ReturnsAuditLog()
    {
        using var ctx = CreateContext();
        var userId = await SeedUserAsync(ctx);
        var repo = new AuditLogRepository(ctx);
        await repo.AddAsync(new AuditLog { UserId = userId, Action = "Login", Timestamp = DateTime.UtcNow });
        var entry = (await repo.ListAsync()).First();

        var found = await repo.FindByIdAsync(entry.AuditId);

        Assert.NotNull(found);
        Assert.Equal("Login", found.Action);
    }

    [Fact]
    public async Task FindByIdAsync_NonExistentId_ReturnsNull()
    {
        using var ctx = CreateContext();
        var repo = new AuditLogRepository(ctx);

        Assert.Null(await repo.FindByIdAsync(999));
    }

    [Fact]
    public async Task DeleteAsync_ExistingId_ShouldRemoveAuditLog()
    {
        using var ctx = CreateContext();
        var userId = await SeedUserAsync(ctx);
        var repo = new AuditLogRepository(ctx);
        await repo.AddAsync(new AuditLog { UserId = userId, Action = "Logout", Timestamp = DateTime.UtcNow });
        var entry = (await repo.ListAsync()).First();

        await repo.DeleteAsync(entry.AuditId);

        Assert.Empty(await repo.ListAsync());
    }

    [Fact]
    public async Task DeleteAsync_NonExistentId_ShouldNotThrow()
    {
        using var ctx = CreateContext();
        var repo = new AuditLogRepository(ctx);

        await repo.DeleteAsync(999);
    }

    [Fact]
    public async Task Query_FilterByUserId_ShouldReturnMatchingLogs()
    {
        using var ctx = CreateContext();
        var userId1 = await SeedUserAsync(ctx);

        var u2 = new User
        {
            Name = "B User", Username = "u2", Email = "u2@test.com",
            Password = "h", RoleTypeId = 1, Status = "Active",
            CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow,
        };
        ctx.Users.Add(u2);
        await ctx.SaveChangesAsync();

        var repo = new AuditLogRepository(ctx);
        await repo.AddAsync(new AuditLog { UserId = userId1, Action = "A1", Timestamp = DateTime.UtcNow });
        await repo.AddAsync(new AuditLog { UserId = userId1, Action = "A2", Timestamp = DateTime.UtcNow });
        await repo.AddAsync(new AuditLog { UserId = (int)u2.Id, Action = "B1", Timestamp = DateTime.UtcNow });

        var result = await repo.Query()
            .Where(a => a.UserId == userId1)
            .ToListAsync();

        Assert.Equal(2, result.Count);
        Assert.All(result, r => Assert.Equal(userId1, r.UserId));
    }
}
