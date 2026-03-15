/*
===============================================================================
 File        : UserRepositoryTests.cs
 Namespace   : AbsenceApp.Tests.RepositoryTests
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-15
 Updated     : 2026-03-15
-------------------------------------------------------------------------------
 Purpose     : xUnit tests for UserRepository using EF Core InMemory provider.
               Verifies full CRUD contract and IQueryable composition against the
               TABLE1 User entity schema.
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

public class UserRepositoryTests
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

    private static User SampleUser(string username = "john.doe") => new()
    {
        Name       = "John Doe",
        Username   = username,
        Email      = "john@example.com",
        Password   = "hashed",
        RoleTypeId = 1,
        Status     = "Active",
        CreatedAt  = DateTime.UtcNow,
        UpdatedAt  = DateTime.UtcNow,
    };

    // =========================================================================
    // Tests
    // =========================================================================

    [Fact]
    public async Task AddAsync_ShouldPersistUser()
    {
        using var ctx = CreateContext();
        var repo = new UserRepository(ctx);

        await repo.AddAsync(SampleUser());
        var list = await repo.ListAsync();

        Assert.Single(list);
    }

    [Fact]
    public async Task ListAsync_WhenEmpty_ShouldReturnEmptyCollection()
    {
        using var ctx = CreateContext();
        var repo = new UserRepository(ctx);

        var list = await repo.ListAsync();

        Assert.Empty(list);
    }

    [Fact]
    public async Task FindByIdAsync_ExistingId_ShouldReturnUser()
    {
        using var ctx = CreateContext();
        var repo = new UserRepository(ctx);
        await repo.AddAsync(SampleUser());
        var added = (await repo.ListAsync()).First();

        var found = await repo.FindByIdAsync((int)added.Id);

        Assert.NotNull(found);
        Assert.Equal("john.doe", found.Username);
    }

    [Fact]
    public async Task FindByIdAsync_NonExistentId_ShouldReturnNull()
    {
        using var ctx = CreateContext();
        var repo = new UserRepository(ctx);

        var found = await repo.FindByIdAsync(999);

        Assert.Null(found);
    }

    [Fact]
    public async Task UpdateAsync_ShouldModifyExistingUser()
    {
        using var ctx = CreateContext();
        var repo = new UserRepository(ctx);
        await repo.AddAsync(SampleUser());
        var user = (await repo.ListAsync()).First();

        user.Email = "updated@example.com";
        await repo.UpdateAsync(user);

        var updated = await repo.FindByIdAsync((int)user.Id);
        Assert.Equal("updated@example.com", updated!.Email);
    }

    [Fact]
    public async Task DeleteAsync_ExistingId_ShouldRemoveUser()
    {
        using var ctx = CreateContext();
        var repo = new UserRepository(ctx);
        await repo.AddAsync(SampleUser());
        var user = (await repo.ListAsync()).First();

        await repo.DeleteAsync((int)user.Id);
        var list = await repo.ListAsync();

        Assert.Empty(list);
    }

    [Fact]
    public async Task DeleteAsync_NonExistentId_ShouldNotThrow()
    {
        using var ctx = CreateContext();
        var repo = new UserRepository(ctx);

        await repo.DeleteAsync(999);
    }

    [Fact]
    public async Task Query_ShouldSupportWhereFilter()
    {
        using var ctx = CreateContext();
        var repo = new UserRepository(ctx);
        await repo.AddAsync(SampleUser("alice.smith"));
        await repo.AddAsync(SampleUser("bob.jones"));

        var result = await repo.Query()
            .Where(u => u.Username == "alice.smith")
            .ToListAsync();

        Assert.Single(result);
        Assert.Equal("alice.smith", result[0].Username);
    }
}
