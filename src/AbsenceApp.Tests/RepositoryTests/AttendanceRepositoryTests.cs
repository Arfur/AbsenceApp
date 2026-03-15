/*
===============================================================================
 File        : AttendanceRepositoryTests.cs
 Namespace   : AbsenceApp.Tests.RepositoryTests
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-15
 Updated     : 2026-03-15
-------------------------------------------------------------------------------
 Purpose     : xUnit tests for AttendanceRepository (legacy stub) using EF Core InMemory.
               Verifies the stub satisfies the IAttendanceRepository contract with
               the retained Attendance entity; seeds users via the new TABLE1 schema.
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
/// Tests for AttendanceRepository (uses legacy Attendance stub entity).
/// Users are seeded using the new User entity schema (Id long, Name, Status).
/// </summary>
public class AttendanceRepositoryTests
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

    private static Attendance MakeAttendance(int userId, int recBy) => new()
    {
        UserId     = userId,
        ClassId    = null,
        Status     = "Present",
        Timestamp  = DateTime.UtcNow,
        RecordedBy = recBy,
    };

    private static User MakeUser(string username) => new()
    {
        Name       = username,
        Username   = username,
        Email      = $"{username}@test.com",
        Password   = "hashed",
        RoleTypeId = 1,
        Status     = "Active",
        CreatedAt  = DateTime.UtcNow,
        UpdatedAt  = DateTime.UtcNow,
    };

    private static async Task<(long userId1, long userId2)> SeedUsers(AppDbContext ctx)
    {
        ctx.Users.AddRange(MakeUser("student.one"), MakeUser("recorder.one"));
        await ctx.SaveChangesAsync();
        var ids = await ctx.Users.Select(u => u.Id).ToListAsync();
        return (ids[0], ids[1]);
    }

    // =========================================================================
    // Tests
    // =========================================================================

    [Fact]
    public async Task AddAsync_ShouldPersistAttendanceRecord()
    {
        using var ctx = CreateContext();
        var (uid1, uid2) = await SeedUsers(ctx);
        var repo = new AttendanceRepository(ctx);

        await repo.AddAsync(MakeAttendance((int)uid1, (int)uid2));
        var list = await repo.ListAsync();

        Assert.Single(list);
        Assert.Equal("Present", list.First().Status);
    }

    [Fact]
    public async Task ListAsync_WhenEmpty_ReturnsEmptyCollection()
    {
        using var ctx = CreateContext();
        var repo = new AttendanceRepository(ctx);

        Assert.Empty(await repo.ListAsync());
    }

    [Fact]
    public async Task FindByIdAsync_NonExistentId_ReturnsNull()
    {
        using var ctx = CreateContext();
        var repo = new AttendanceRepository(ctx);

        Assert.Null(await repo.FindByIdAsync(999));
    }

    [Fact]
    public async Task DeleteAsync_NonExistentId_ShouldNotThrow()
    {
        using var ctx = CreateContext();
        var repo = new AttendanceRepository(ctx);

        await repo.DeleteAsync(999);
    }

    [Fact]
    public async Task AddAsync_WithNullClassId_ShouldSucceed()
    {
        using var ctx = CreateContext();
        var (uid1, uid2) = await SeedUsers(ctx);
        var repo = new AttendanceRepository(ctx);

        var rec = MakeAttendance((int)uid1, (int)uid2);
        rec.ClassId = null;
        await repo.AddAsync(rec);

        Assert.Single(await repo.ListAsync());
    }
}
