/*
===============================================================================
 File        : RoleRepositoryTests.cs
 Namespace   : AbsenceApp.Tests.RepositoryTests
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : xUnit tests for RoleRepository using EF Core InMemory provider.
               Verifies full CRUD contract and IQueryable composition.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial test suite.
===============================================================================
*/

using AbsenceApp.Data.Context;
using AbsenceApp.Data.Models;
using AbsenceApp.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AbsenceApp.Tests.RepositoryTests;

// =========================================================================
// RoleRepositoryTests — EF Core InMemory CRUD + IQueryable tests
// =========================================================================

public class RoleRepositoryTests
{
    // =========================================================================
    // Helpers — fresh InMemory context factory
    // =========================================================================

    private static AppDbContext CreateContext()
    {
        var opts = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(opts);
    }

    // =========================================================================
    // AddAsync + ListAsync tests
    // =========================================================================

    [Fact]
    public async Task AddAsync_ShouldPersistRole()
    {
        using var ctx = CreateContext();
        var repo = new RoleRepository(ctx);

        await repo.AddAsync(new Role { RoleName = "Admin" });
        var list = await repo.ListAsync();

        Assert.Single(list);
        Assert.Equal("Admin", list.First().RoleName);
    }

    [Fact]
    public async Task ListAsync_MultipleRoles_ShouldReturnAll()
    {
        using var ctx = CreateContext();
        var repo = new RoleRepository(ctx);
        await repo.AddAsync(new Role { RoleName = "Admin" });
        await repo.AddAsync(new Role { RoleName = "Teacher" });

        var list = await repo.ListAsync();

        Assert.Equal(2, list.Count());
    }

    // =========================================================================
    // FindByIdAsync tests
    // =========================================================================

    [Fact]
    public async Task FindByIdAsync_ExistingId_ShouldReturnRole()
    {
        using var ctx = CreateContext();
        var repo = new RoleRepository(ctx);
        await repo.AddAsync(new Role { RoleName = "Student" });
        var added = (await repo.ListAsync()).First();

        var found = await repo.FindByIdAsync(added.RoleId);

        Assert.NotNull(found);
        Assert.Equal("Student", found.RoleName);
    }

    [Fact]
    public async Task FindByIdAsync_NonExistentId_ShouldReturnNull()
    {
        using var ctx = CreateContext();
        var repo = new RoleRepository(ctx);

        Assert.Null(await repo.FindByIdAsync(404));
    }

    // =========================================================================
    // UpdateAsync tests
    // =========================================================================

    [Fact]
    public async Task UpdateAsync_ShouldModifyRoleName()
    {
        using var ctx = CreateContext();
        var repo = new RoleRepository(ctx);
        await repo.AddAsync(new Role { RoleName = "Old" });
        var role = (await repo.ListAsync()).First();

        role.RoleName = "New";
        await repo.UpdateAsync(role);

        var updated = await repo.FindByIdAsync(role.RoleId);
        Assert.Equal("New", updated!.RoleName);
    }

    // =========================================================================
    // DeleteAsync tests
    // =========================================================================

    [Fact]
    public async Task DeleteAsync_ExistingId_ShouldRemoveRole()
    {
        using var ctx = CreateContext();
        var repo = new RoleRepository(ctx);
        await repo.AddAsync(new Role { RoleName = "ToDelete" });
        var role = (await repo.ListAsync()).First();

        await repo.DeleteAsync(role.RoleId);

        Assert.Empty(await repo.ListAsync());
    }

    [Fact]
    public async Task DeleteAsync_NonExistentId_ShouldNotThrow()
    {
        using var ctx = CreateContext();
        var repo = new RoleRepository(ctx);

        await repo.DeleteAsync(999);
    }

    // =========================================================================
    // IQueryable tests
    // =========================================================================

    [Fact]
    public async Task Query_FilterByName_ShouldReturnMatchingRoles()
    {
        using var ctx = CreateContext();
        var repo = new RoleRepository(ctx);
        await repo.AddAsync(new Role { RoleName = "Admin" });
        await repo.AddAsync(new Role { RoleName = "Teacher" });

        var result = await repo.Query()
            .Where(r => r.RoleName.StartsWith("A"))
            .ToListAsync();

        Assert.Single(result);
        Assert.Equal("Admin", result[0].RoleName);
    }
}
