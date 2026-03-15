/*
===============================================================================
 File        : ClassRepositoryTests.cs
 Namespace   : AbsenceApp.Tests.RepositoryTests
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-15
 Updated     : 2026-03-15
-------------------------------------------------------------------------------
 Purpose     : xUnit tests for ClassRepository using EF Core InMemory provider.
               Verifies full CRUD contract and IQueryable composition against the
               TABLE9 Class entity schema.
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

public class ClassRepositoryTests
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

    // =========================================================================
    // Tests
    // =========================================================================

    [Fact]
    public async Task AddAsync_ShouldPersistClass()
    {
        using var ctx = CreateContext();
        var repo = new ClassRepository(ctx);

        await repo.AddAsync(new Class { Name = "7A", Code = "7A", Description = "Year 7 Set A" });
        var list = await repo.ListAsync();

        Assert.Single(list);
        Assert.Equal("7A", list.First().Name);
    }

    [Fact]
    public async Task ListAsync_WhenEmpty_ShouldReturnEmptyCollection()
    {
        using var ctx = CreateContext();
        var repo = new ClassRepository(ctx);

        Assert.Empty(await repo.ListAsync());
    }

    [Fact]
    public async Task FindByIdAsync_ExistingId_ReturnsClass()
    {
        using var ctx = CreateContext();
        var repo = new ClassRepository(ctx);
        await repo.AddAsync(new Class { Name = "8B", Code = "8B" });
        var cls = (await repo.ListAsync()).First();

        var found = await repo.FindByIdAsync((int)cls.Id);

        Assert.NotNull(found);
        Assert.Equal("8B", found.Name);
    }

    [Fact]
    public async Task FindByIdAsync_NonExistentId_ReturnsNull()
    {
        using var ctx = CreateContext();
        var repo = new ClassRepository(ctx);

        Assert.Null(await repo.FindByIdAsync(999));
    }

    [Fact]
    public async Task UpdateAsync_ShouldModifyName()
    {
        using var ctx = CreateContext();
        var repo = new ClassRepository(ctx);
        await repo.AddAsync(new Class { Name = "Old", Code = "OLD" });
        var cls = (await repo.ListAsync()).First();

        cls.Name = "New";
        await repo.UpdateAsync(cls);

        Assert.Equal("New", (await repo.FindByIdAsync((int)cls.Id))!.Name);
    }

    [Fact]
    public async Task DeleteAsync_ExistingId_ShouldRemoveClass()
    {
        using var ctx = CreateContext();
        var repo = new ClassRepository(ctx);
        await repo.AddAsync(new Class { Name = "ToDelete", Code = "DEL" });
        var cls = (await repo.ListAsync()).First();

        await repo.DeleteAsync((int)cls.Id);

        Assert.Empty(await repo.ListAsync());
    }

    [Fact]
    public async Task DeleteAsync_NonExistentId_ShouldNotThrow()
    {
        using var ctx = CreateContext();
        var repo = new ClassRepository(ctx);

        await repo.DeleteAsync(999);
    }

    [Fact]
    public async Task AddAsync_WithNullDescription_ShouldSucceed()
    {
        using var ctx = CreateContext();
        var repo = new ClassRepository(ctx);

        await repo.AddAsync(new Class { Name = "NoDesc", Code = "ND", Description = null });
        var cls = (await repo.ListAsync()).First();

        Assert.Null(cls.Description);
    }

    [Fact]
    public async Task Query_FilterByName_ShouldReturnMatchingClasses()
    {
        using var ctx = CreateContext();
        var repo = new ClassRepository(ctx);
        await repo.AddAsync(new Class { Name = "7A", Code = "7A" });
        await repo.AddAsync(new Class { Name = "8B", Code = "8B" });

        var result = await repo.Query()
            .Where(c => c.Name.StartsWith("7"))
            .ToListAsync();

        Assert.Single(result);
        Assert.Equal("7A", result[0].Name);
    }
}
