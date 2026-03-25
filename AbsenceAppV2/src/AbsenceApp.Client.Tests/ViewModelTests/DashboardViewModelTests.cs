// ============================================================
// File:    DashboardViewModelTests.cs
// Project: AbsenceApp.Client.Tests
// Purpose: Unit tests for DashboardOverviewViewModel,
//          DashboardStudentActivityViewModel, and
//          DashboardSafeguardingViewModel.
// ============================================================

using AbsenceApp.Core.Interfaces;

namespace AbsenceApp.Client.Tests.ViewModelTests;

public sealed class DashboardViewModelTests
{
    // =========================================================================
    // DashboardOverviewViewModel
    // =========================================================================

    [Fact]
    public async Task Overview_LoadAsync_PopulatesOverview()
    {
        var dto = new DashboardOverviewDto
        {
            TotalStudents     = 120,
            TotalStaff        = 15,
            AbsencesToday     = 8,
            UnauthorisedToday = 3,
            RegistersOpen     = 2
        };

        var svc = new Mock<IDashboardService>();
        svc.Setup(s => s.GetOverviewAsync()).ReturnsAsync(dto);

        var vm = new DashboardOverviewViewModel(svc.Object);
        await vm.LoadAsync();

        Assert.NotNull(vm.Overview);
        Assert.Equal(8, vm.Overview!.AbsencesToday);
        Assert.Equal(3, vm.Overview!.UnauthorisedToday);
        Assert.Equal(2, vm.Overview!.RegistersOpen);
        svc.Verify(s => s.GetOverviewAsync(), Times.Once);
    }

    [Fact]
    public async Task Overview_LoadAsync_SetsErrorOnException()
    {
        var svc = new Mock<IDashboardService>();
        svc.Setup(s => s.GetOverviewAsync()).ThrowsAsync(new InvalidOperationException("DB error"));

        var vm = new DashboardOverviewViewModel(svc.Object);
        await vm.LoadAsync();

        Assert.Null(vm.Overview);
        Assert.Equal("DB error", vm.ErrorMessage);
        Assert.False(vm.IsLoading);
    }

    // =========================================================================
    // DashboardStudentActivityViewModel
    // =========================================================================

    [Fact]
    public async Task StudentActivity_LoadAsync_PopulatesActivity()
    {
        var data = new List<DashboardStudentActivityDto>
        {
            new() { StudentId = 1, FullName = "Alice", AbsenceCount = 3, LastAbsence = DateOnly.FromDateTime(DateTime.Today) },
            new() { StudentId = 2, FullName = "Bob",   AbsenceCount = 1, LastAbsence = DateOnly.FromDateTime(DateTime.Today) }
        };

        var svc = new Mock<IDashboardService>();
        svc.Setup(s => s.GetStudentActivityAsync(It.IsAny<int>())).ReturnsAsync(data);

        var vm = new DashboardStudentActivityViewModel(svc.Object);
        await vm.LoadAsync();

        Assert.Equal(2, vm.Activity.Count());
        Assert.False(vm.IsLoading);
        svc.Verify(s => s.GetStudentActivityAsync(10), Times.Once);
    }

    [Fact]
    public async Task StudentActivity_LoadAsync_SetsErrorOnException()
    {
        var svc = new Mock<IDashboardService>();
        svc.Setup(s => s.GetStudentActivityAsync(It.IsAny<int>())).ThrowsAsync(new Exception("fail"));

        var vm = new DashboardStudentActivityViewModel(svc.Object);
        await vm.LoadAsync();

        Assert.Empty(vm.Activity);
        Assert.NotNull(vm.ErrorMessage);
    }

    // =========================================================================
    // DashboardSafeguardingViewModel
    // =========================================================================

    [Fact]
    public async Task Safeguarding_LoadAsync_PopulatesAlerts()
    {
        var data = new List<DashboardSafeguardingDto>
        {
            new() { StudentId = 5, FullName = "Cara", SafeguardingFlag = true,  Notes = "Review due" },
            new() { StudentId = 6, FullName = "Dan",  SafeguardingFlag = false, Notes = null }
        };

        var svc = new Mock<IDashboardService>();
        svc.Setup(s => s.GetSafeguardingAsync()).ReturnsAsync(data);

        var vm = new DashboardSafeguardingViewModel(svc.Object);
        await vm.LoadAsync();

        Assert.Equal(2, vm.Alerts.Count());
        Assert.True(vm.Alerts.First().SafeguardingFlag);
        svc.Verify(s => s.GetSafeguardingAsync(), Times.Once);
    }

    [Fact]
    public async Task Safeguarding_LoadAsync_SetsErrorOnException()
    {
        var svc = new Mock<IDashboardService>();
        svc.Setup(s => s.GetSafeguardingAsync()).ThrowsAsync(new Exception("timeout"));

        var vm = new DashboardSafeguardingViewModel(svc.Object);
        await vm.LoadAsync();

        Assert.Empty(vm.Alerts);
        Assert.Equal("timeout", vm.ErrorMessage);
    }
}
