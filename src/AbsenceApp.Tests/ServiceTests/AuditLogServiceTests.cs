/*
===============================================================================
 File        : AuditLogServiceTests.cs
 Namespace   : AbsenceApp.Tests.ServiceTests
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : xUnit tests for AuditLogService using Moq-mocked
               IAuditLogRepository. Tests all public methods including the
               append-only LogAsync business rule and GetByUserAsync
               IQueryable filtering.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial test suite.
===============================================================================
*/

using AbsenceApp.Data.Models;
using AbsenceApp.Data.Repositories;
using AbsenceApp.Data.Services;
using AbsenceApp.Tests.Helpers;
using Moq;

namespace AbsenceApp.Tests.ServiceTests;

// =========================================================================
// AuditLogServiceTests — Moq-based service tests for AuditLogService
// =========================================================================

public class AuditLogServiceTests
{
    // =========================================================================
    // Helpers
    // =========================================================================

    private static (Mock<IAuditLogRepository> mockRepo, AuditLogService sut) Create()
    {
        var mock = new Mock<IAuditLogRepository>();
        return (mock, new AuditLogService(mock.Object));
    }

    private static AuditLog MakeLog(int id, int userId, string action) => new()
    {
        AuditId   = id,
        UserId    = userId,
        Action    = action,
        Timestamp = DateTime.UtcNow,
    };

    // =========================================================================
    // GetAllAsync tests
    // =========================================================================

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllMappedDtos()
    {
        var (mock, sut) = Create();
        mock.Setup(r => r.ListAsync())
            .ReturnsAsync(new List<AuditLog>
            {
                MakeLog(1, 10, "Login"),
                MakeLog(2, 11, "Logout"),
            });

        var result = (await sut.GetAllAsync()).ToList();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetAllAsync_EmptyRepository_ShouldReturnEmpty()
    {
        var (mock, sut) = Create();
        mock.Setup(r => r.ListAsync()).ReturnsAsync(new List<AuditLog>());

        Assert.Empty(await sut.GetAllAsync());
    }

    // =========================================================================
    // GetByIdAsync tests
    // =========================================================================

    [Fact]
    public async Task GetByIdAsync_ExistingId_ShouldReturnDto()
    {
        var (mock, sut) = Create();
        mock.Setup(r => r.FindByIdAsync(1)).ReturnsAsync(MakeLog(1, 10, "Login"));

        var result = await sut.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("Login", result.Action);
        Assert.Equal(10, result.UserId);
    }

    [Fact]
    public async Task GetByIdAsync_NotFound_ShouldReturnNull()
    {
        var (mock, sut) = Create();
        mock.Setup(r => r.FindByIdAsync(It.IsAny<int>())).ReturnsAsync((AuditLog?)null);

        Assert.Null(await sut.GetByIdAsync(999));
    }

    // =========================================================================
    // GetByUserAsync tests — uses IQueryable.Where filtering
    // =========================================================================

    [Fact]
    public async Task GetByUserAsync_ShouldReturnOnlyMatchingUserLogs()
    {
        var logs = new List<AuditLog>
        {
            MakeLog(1, 10, "Login"),
            MakeLog(2, 10, "ViewReport"),
            MakeLog(3, 20, "Login"),
        };

        var (mock, sut) = Create();
        mock.Setup(r => r.Query()).Returns(logs.AsAsyncQueryable());

        var result = (await sut.GetByUserAsync(10)).ToList();

        Assert.Equal(2, result.Count);
        Assert.All(result, d => Assert.Equal(10, d.UserId));
    }

    [Fact]
    public async Task GetByUserAsync_NoMatchingUser_ShouldReturnEmpty()
    {
        var logs = new List<AuditLog> { MakeLog(1, 5, "Login") };

        var (mock, sut) = Create();
        mock.Setup(r => r.Query()).Returns(logs.AsAsyncQueryable());

        var result = await sut.GetByUserAsync(99);

        Assert.Empty(result);
    }

    // =========================================================================
    // LogAsync tests — append-only business rule
    // =========================================================================

    [Fact]
    public async Task LogAsync_ValidAction_ShouldCallAddAsyncOnce()
    {
        var (mock, sut) = Create();
        mock.Setup(r => r.AddAsync(It.IsAny<AuditLog>())).Returns(Task.CompletedTask);

        var result = await sut.LogAsync(10, "UserCreated");

        mock.Verify(r => r.AddAsync(It.IsAny<AuditLog>()), Times.Once);
        Assert.Equal("UserCreated", result.Action);
        Assert.Equal(10, result.UserId);
    }

    [Fact]
    public async Task LogAsync_EmptyAction_ShouldThrowArgumentException()
    {
        var (_, sut) = Create();

        await Assert.ThrowsAsync<ArgumentException>(
            () => sut.LogAsync(10, ""));
    }

    [Fact]
    public async Task LogAsync_WhitespaceAction_ShouldThrowArgumentException()
    {
        var (_, sut) = Create();

        await Assert.ThrowsAsync<ArgumentException>(
            () => sut.LogAsync(10, "   "));
    }

    [Fact]
    public async Task LogAsync_ShouldSetTimestampToNow()
    {
        var (mock, sut) = Create();
        AuditLog? captured = null;
        mock.Setup(r => r.AddAsync(It.IsAny<AuditLog>()))
            .Callback<AuditLog>(l => captured = l)
            .Returns(Task.CompletedTask);

        await sut.LogAsync(1, "Action");

        Assert.NotNull(captured);
        Assert.True((DateTime.UtcNow - captured.Timestamp).TotalSeconds < 5);
    }
}
