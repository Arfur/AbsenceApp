/*
===============================================================================
 File        : RoleServiceTests.cs
 Namespace   : AbsenceApp.Tests.ServiceTests
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : xUnit tests for RoleService using Moq-mocked IRoleRepository.
               Tests all public methods and business-rule validations.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial test suite.
===============================================================================
*/

using AbsenceApp.Core.DTOs;
using AbsenceApp.Data.Models;
using AbsenceApp.Data.Repositories;
using AbsenceApp.Data.Services;
using Moq;

namespace AbsenceApp.Tests.ServiceTests;

// =========================================================================
// RoleServiceTests — Moq-based service tests for RoleService
// =========================================================================

public class RoleServiceTests
{
    // =========================================================================
    // Helpers
    // =========================================================================

    private static (Mock<IRoleRepository> mockRepo, RoleService sut) Create()
    {
        var mock = new Mock<IRoleRepository>();
        return (mock, new RoleService(mock.Object));
    }

    private static Role SampleRole(int id = 1) =>
        new() { RoleId = id, RoleName = "Admin" };

    private static RoleDto SampleDto(int id = 1) =>
        new() { RoleId = id, RoleName = "Admin" };

    // =========================================================================
    // GetAllAsync tests
    // =========================================================================

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllMappedRoles()
    {
        var (mock, sut) = Create();
        mock.Setup(r => r.ListAsync())
            .ReturnsAsync(new List<Role> { SampleRole(1), SampleRole(2) });

        var result = (await sut.GetAllAsync()).ToList();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetAllAsync_Empty_ShouldReturnEmptyEnumerable()
    {
        var (mock, sut) = Create();
        mock.Setup(r => r.ListAsync()).ReturnsAsync(new List<Role>());

        Assert.Empty(await sut.GetAllAsync());
    }

    // =========================================================================
    // GetByIdAsync tests
    // =========================================================================

    [Fact]
    public async Task GetByIdAsync_ExistingId_ShouldReturnDto()
    {
        var (mock, sut) = Create();
        mock.Setup(r => r.FindByIdAsync(1)).ReturnsAsync(SampleRole(1));

        var result = await sut.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("Admin", result.RoleName);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistentId_ShouldReturnNull()
    {
        var (mock, sut) = Create();
        mock.Setup(r => r.FindByIdAsync(It.IsAny<int>())).ReturnsAsync((Role?)null);

        Assert.Null(await sut.GetByIdAsync(999));
    }

    // =========================================================================
    // CreateAsync tests
    // =========================================================================

    [Fact]
    public async Task CreateAsync_ValidDto_ShouldCallAddAsync()
    {
        var (mock, sut) = Create();
        mock.Setup(r => r.AddAsync(It.IsAny<Role>())).Returns(Task.CompletedTask);

        await sut.CreateAsync(SampleDto(0));

        mock.Verify(r => r.AddAsync(It.IsAny<Role>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_EmptyRoleName_ShouldThrowArgumentException()
    {
        var (_, sut) = Create();

        await Assert.ThrowsAsync<ArgumentException>(
            () => sut.CreateAsync(new RoleDto { RoleName = "" }));
    }

    [Fact]
    public async Task CreateAsync_ShouldResetRoleIdToZero()
    {
        var (mock, sut) = Create();
        Role? captured = null;
        mock.Setup(r => r.AddAsync(It.IsAny<Role>()))
            .Callback<Role>(r => captured = r)
            .Returns(Task.CompletedTask);

        await sut.CreateAsync(SampleDto(99));

        Assert.Equal(0, captured!.RoleId);
    }

    // =========================================================================
    // UpdateAsync tests
    // =========================================================================

    [Fact]
    public async Task UpdateAsync_ValidDto_ShouldCallUpdateAsync()
    {
        var (mock, sut) = Create();
        mock.Setup(r => r.UpdateAsync(It.IsAny<Role>())).Returns(Task.CompletedTask);

        await sut.UpdateAsync(SampleDto(1));

        mock.Verify(r => r.UpdateAsync(It.IsAny<Role>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WhitespaceRoleName_ShouldThrowArgumentException()
    {
        var (_, sut) = Create();

        await Assert.ThrowsAsync<ArgumentException>(
            () => sut.UpdateAsync(new RoleDto { RoleId = 1, RoleName = "   " }));
    }

    // =========================================================================
    // DeleteAsync tests
    // =========================================================================

    [Fact]
    public async Task DeleteAsync_ShouldDelegateToRepository()
    {
        var (mock, sut) = Create();
        mock.Setup(r => r.DeleteAsync(5)).Returns(Task.CompletedTask);

        await sut.DeleteAsync(5);

        mock.Verify(r => r.DeleteAsync(5), Times.Once);
    }
}
