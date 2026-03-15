// ============================================================
// File:    AuditLogViewModelTests.cs
// Project: AbsenceApp.Client.Tests
// Purpose: Unit tests for AuditLogViewModel (no bUnit — pure .NET).
// ============================================================

using AbsenceApp.Core.Interfaces;

namespace AbsenceApp.Client.Tests.ViewModelTests;

public sealed class AuditLogViewModelTests
{
    [Fact]
    public async Task LoadAllAsync_PopulatesEntries()
    {
        var entries = new List<AuditLogDto>
        {
            new() { AuditId = 1, UserId = 10, Action = "Login",  Timestamp = DateTime.UtcNow },
            new() { AuditId = 2, UserId = 11, Action = "Logout", Timestamp = DateTime.UtcNow }
        };

        var svc = new Mock<IAuditLogService>();
        svc.Setup(s => s.GetAllAsync()).ReturnsAsync(entries);

        var vm = new AuditLogViewModel(svc.Object);
        await vm.LoadAllAsync();

        Assert.Equal(2, vm.Entries.Count());
        svc.Verify(s => s.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task LoadByUserAsync_FiltersEntries()
    {
        const int userId = 10;
        var entries = new List<AuditLogDto>
        {
            new() { AuditId = 1, UserId = userId, Action = "Update", Timestamp = DateTime.UtcNow }
        };

        var svc = new Mock<IAuditLogService>();
        svc.Setup(s => s.GetByUserAsync(userId)).ReturnsAsync(entries);

        var vm = new AuditLogViewModel(svc.Object);
        await vm.LoadByUserAsync(userId);

        Assert.Single(vm.Entries);
        Assert.Equal("Update", vm.Entries.First().Action);
        svc.Verify(s => s.GetByUserAsync(userId), Times.Once);
    }
}
