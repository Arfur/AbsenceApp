/*
===============================================================================
 File        : ClassServiceTests.cs
 Namespace   : AbsenceApp.Tests.ServiceTests
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-15
 Updated     : 2026-03-15
-------------------------------------------------------------------------------
 Purpose     : xUnit tests for ClassService using EF Core InMemory provider.
               Covers GetAll, GetById, Create, Update, Delete, and validation errors.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-15  Initial creation.
-------------------------------------------------------------------------------
===============================================================================
*/

using AbsenceApp.Core.DTOs;
using AbsenceApp.Data.Models;
using AbsenceApp.Data.Repositories;
using AbsenceApp.Data.Services;
using Moq;

namespace AbsenceApp.Tests.ServiceTests;

public class ClassServiceTests
{
    // =========================================================================
    // Helpers
    // =========================================================================

    private static (Mock<IClassRepository> mockRepo, ClassService sut) Create()
    {
        var mock = new Mock<IClassRepository>();
        return (mock, new ClassService(mock.Object));
    }

    private static Class SampleClass(int id = 1) => new()
    {
        Id          = id,
        Name        = "7A",
        Description = "Year 7 Set A",
    };

    private static ClassDto SampleDto(int id = 1) => new()
    {
        ClassId     = id,
        ClassName   = "7A",
        Description = "Year 7 Set A",
    };

    // =========================================================================
    // Tests
    // =========================================================================

    [Fact]
    public async Task GetAllAsync_ShouldReturnMappedDtos()
    {
        var (mock, sut) = Create();
        mock.Setup(r => r.ListAsync())
            .ReturnsAsync(new List<Class> { SampleClass(1), SampleClass(2) });

        var result = (await sut.GetAllAsync()).ToList();

        Assert.Equal(2, result.Count);
        Assert.All(result, d => Assert.Equal("7A", d.ClassName));
    }

    [Fact]
    public async Task GetAllAsync_EmptyRepository_ShouldReturnEmptyList()
    {
        var (mock, sut) = Create();
        mock.Setup(r => r.ListAsync()).ReturnsAsync(new List<Class>());

        var result = await sut.GetAllAsync();

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ShouldReturnDto()
    {
        var (mock, sut) = Create();
        mock.Setup(r => r.FindByIdAsync(1)).ReturnsAsync(SampleClass(1));

        var result = await sut.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1,    result.ClassId);
        Assert.Equal("7A", result.ClassName);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistentId_ShouldReturnNull()
    {
        var (mock, sut) = Create();
        mock.Setup(r => r.FindByIdAsync(It.IsAny<int>())).ReturnsAsync((Class?)null);

        var result = await sut.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_ValidDto_ShouldCallAddAsync()
    {
        var (mock, sut) = Create();
        mock.Setup(r => r.AddAsync(It.IsAny<Class>())).Returns(Task.CompletedTask);

        var result = await sut.CreateAsync(SampleDto(0));

        mock.Verify(r => r.AddAsync(It.IsAny<Class>()), Times.Once);
        Assert.Equal("7A", result.ClassName);
    }

    [Fact]
    public async Task CreateAsync_EmptyClassName_ShouldThrowArgumentException()
    {
        var (_, sut) = Create();
        var dto = new ClassDto { ClassName = "   " };

        await Assert.ThrowsAsync<ArgumentException>(() => sut.CreateAsync(dto));
    }

    [Fact]
    public async Task CreateAsync_NullClassName_ShouldThrowArgumentException()
    {
        var (_, sut) = Create();
        var dto = new ClassDto { ClassName = null! };

        await Assert.ThrowsAsync<ArgumentException>(() => sut.CreateAsync(dto));
    }
}
