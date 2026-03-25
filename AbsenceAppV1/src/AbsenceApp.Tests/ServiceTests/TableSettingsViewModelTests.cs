/*
===============================================================================
 File        : TableSettingsViewModelTests.cs
 Namespace   : AbsenceApp.Tests.ServiceTests
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-17
 Updated     : 2026-03-17
-------------------------------------------------------------------------------
 Purpose     : xUnit + Moq tests for TableSettingsViewModel.

               Covers:
                 - LoadAsync: calls GetSettingsAsync, populates Fields.
                 - SaveAsync: calls SaveSettingsAsync with current Fields list.
                 - ResetAsync: calls ResetToDefaultsAsync then re-loads.
                 - IsBusy flag: true during async ops, false after.
                 - Error handling: ViewModel sets ErrorMessage on exception.
                 - SelectedPage property: correctly drives Load.
-------------------------------------------------------------------------------
 Notes       :
   - ITableSettingsService is mocked with Moq.
   - TableSettingsViewModel must accept ITableSettingsService in its ctor.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-17  Initial test suite.
===============================================================================
*/

using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;
using AbsenceApp.Core.ViewModels;

namespace AbsenceApp.Tests.ServiceTests;

// =============================================================================
// LoadAsync tests
// =============================================================================

public class TableSettingsViewModel_LoadAsync
{
    private static (Mock<ITableSettingsService> svc, TableSettingsViewModel vm) Create()
    {
        var svc = new Mock<ITableSettingsService>();
        var vm  = new TableSettingsViewModel(svc.Object);
        return (svc, vm);
    }

    private static List<TablePageSettingDto> FakeSettings(string page, int count = 3)
        => Enumerable.Range(1, count)
            .Select(i => new TablePageSettingDto
            {
                PageName     = page,
                FieldName    = $"field_{i}",
                DisplayLabel = $"Field {i}",
                IsVisible    = true,
                IsSortable   = true,
                DisplayOrder = i,
            })
            .ToList();

    [Fact]
    public async Task LoadAsync_CallsGetSettingsAsync_WithCorrectPage()
    {
        var (svc, vm) = Create();
        svc.Setup(s => s.GetSettingsAsync("students"))
           .ReturnsAsync(FakeSettings("students"));

        await vm.LoadAsync("students");

        svc.Verify(s => s.GetSettingsAsync("students"), Times.Once);
    }

    [Fact]
    public async Task LoadAsync_PopulatesSettingsFromService()
    {
        var (svc, vm) = Create();
        var fake      = FakeSettings("students", 5);
        svc.Setup(s => s.GetSettingsAsync("students")).ReturnsAsync(fake);

        await vm.LoadAsync("students");

        Assert.Equal(5, vm.Settings.Count);
        Assert.Equal("students", vm.SelectedPage);
    }

    [Fact]
    public async Task LoadAsync_SetsIsLoadingFalse_AfterCompletion()
    {
        var (svc, vm) = Create();
        svc.Setup(s => s.GetSettingsAsync(It.IsAny<string>())).ReturnsAsync([]);

        await vm.LoadAsync("students");

        Assert.False(vm.IsLoading);
    }

    [Fact]
    public async Task LoadAsync_ServiceThrows_SetsErrorMessage()
    {
        var (svc, vm) = Create();
        svc.Setup(s => s.GetSettingsAsync(It.IsAny<string>()))
           .ThrowsAsync(new InvalidOperationException("DB error"));

        await vm.LoadAsync("students");

        Assert.False(vm.IsLoading);
        Assert.NotNull(vm.ErrorMessage);
        Assert.Contains("DB error", vm.ErrorMessage);
    }

    [Fact]
    public async Task LoadAsync_ClearsErrorMessage_OnSuccess()
    {
        var (svc, vm) = Create();
        // First call fails
        svc.SetupSequence(s => s.GetSettingsAsync(It.IsAny<string>()))
           .ThrowsAsync(new Exception("fail"))
           .ReturnsAsync(FakeSettings("students"));

        await vm.LoadAsync("students"); // sets error
        await vm.LoadAsync("students"); // should clear error

        Assert.Null(vm.ErrorMessage);
    }

    [Fact]
    public async Task LoadAsync_EmptyPage_SetsSettingsToEmpty()
    {
        var (svc, vm) = Create();
        svc.Setup(s => s.GetSettingsAsync(It.IsAny<string>()))
           .ReturnsAsync([]);

        await vm.LoadAsync("staff");

        Assert.Empty(vm.Settings);
        Assert.Equal("staff", vm.SelectedPage);
    }
}

// =============================================================================
// SaveAsync tests
// =============================================================================

public class TableSettingsViewModel_SaveAsync
{
    private static (Mock<ITableSettingsService> svc, TableSettingsViewModel vm) Create()
    {
        var svc = new Mock<ITableSettingsService>();
        var vm  = new TableSettingsViewModel(svc.Object);
        return (svc, vm);
    }

    [Fact]
    public async Task SaveAsync_CallsSaveSettingsAsync_WithSelectedPage()
    {
        var (svc, vm) = Create();
        svc.Setup(s => s.GetSettingsAsync("students"))
           .ReturnsAsync([new TablePageSettingDto { PageName = "students", FieldName = "id" }]);
        svc.Setup(s => s.SaveSettingsAsync(It.IsAny<string>(), It.IsAny<IEnumerable<TablePageSettingDto>>()))
           .Returns(Task.CompletedTask);

        await vm.LoadAsync("students");
        await vm.SaveAsync();

        svc.Verify(s => s.SaveSettingsAsync("students", It.IsAny<IEnumerable<TablePageSettingDto>>()), Times.Once);
    }

    [Fact]
    public async Task SaveAsync_PassesCurrentSettingsToService()
    {
        var (svc, vm) = Create();
        var fields    = new List<TablePageSettingDto>
        {
            new() { PageName = "staff", FieldName = "id",          IsVisible = true  },
            new() { PageName = "staff", FieldName = "staff_number", IsVisible = false },
        };
        svc.Setup(s => s.GetSettingsAsync("staff")).ReturnsAsync(fields);
        svc.Setup(s => s.SaveSettingsAsync(It.IsAny<string>(), It.IsAny<IEnumerable<TablePageSettingDto>>()))
           .Returns(Task.CompletedTask);

        await vm.LoadAsync("staff");
        await vm.SaveAsync();

        svc.Verify(s => s.SaveSettingsAsync("staff",
            It.Is<IEnumerable<TablePageSettingDto>>(list =>
                list.Count() == 2 &&
                list.Any(f => f.FieldName == "id"     && f.IsVisible) &&
                list.Any(f => f.FieldName == "staff_number" && !f.IsVisible))),
            Times.Once);
    }

    [Fact]
    public async Task SaveAsync_SetsIsSavingFalse_AfterCompletion()
    {
        var (svc, vm) = Create();
        svc.Setup(s => s.GetSettingsAsync(It.IsAny<string>())).ReturnsAsync([]);
        svc.Setup(s => s.SaveSettingsAsync(It.IsAny<string>(), It.IsAny<IEnumerable<TablePageSettingDto>>()))
           .Returns(Task.CompletedTask);

        await vm.LoadAsync("students");
        await vm.SaveAsync();

        Assert.False(vm.IsSaving);
    }

    [Fact]
    public async Task SaveAsync_ServiceThrows_SetsErrorMessage()
    {
        var (svc, vm) = Create();
        svc.Setup(s => s.GetSettingsAsync(It.IsAny<string>())).ReturnsAsync([]);
        svc.Setup(s => s.SaveSettingsAsync(It.IsAny<string>(), It.IsAny<IEnumerable<TablePageSettingDto>>()))
           .ThrowsAsync(new Exception("Save failed"));

        await vm.LoadAsync("students");
        await vm.SaveAsync();

        Assert.False(vm.IsSaving);
        Assert.NotNull(vm.ErrorMessage);
        Assert.Contains("Save failed", vm.ErrorMessage);
    }

    [Fact]
    public async Task SaveAsync_WithDefaultPage_SavesSuccessfully()
    {
        // SelectedPage defaults to "students" — SaveAsync should call through.
        var (svc, vm) = Create();
        svc.Setup(s => s.SaveSettingsAsync(It.IsAny<string>(), It.IsAny<IEnumerable<TablePageSettingDto>>()))
           .Returns(Task.CompletedTask);

        await vm.SaveAsync();

        svc.Verify(s => s.SaveSettingsAsync("students", It.IsAny<IEnumerable<TablePageSettingDto>>()), Times.Once);
        Assert.True(vm.IsSaved);
    }
}

// =============================================================================
// ResetToDefaultsAsync tests
// =============================================================================

public class TableSettingsViewModel_ResetToDefaultsAsync
{
    private static (Mock<ITableSettingsService> svc, TableSettingsViewModel vm) Create()
    {
        var svc = new Mock<ITableSettingsService>();
        var vm  = new TableSettingsViewModel(svc.Object);
        return (svc, vm);
    }

    [Fact]
    public async Task Reset_CallsResetToDefaultsAsync_ThenReloads()
    {
        var (svc, vm) = Create();
        svc.Setup(s => s.GetSettingsAsync("students")).ReturnsAsync([]);
        svc.Setup(s => s.ResetToDefaultsAsync("students")).Returns(Task.CompletedTask);

        await vm.LoadAsync("students");
        await vm.ResetToDefaultsAsync();

        svc.Verify(s => s.ResetToDefaultsAsync("students"), Times.Once);
        // GetSettingsAsync called once in LoadAsync, once inside ResetToDefaultsAsync
        svc.Verify(s => s.GetSettingsAsync("students"), Times.Exactly(2));
    }

    [Fact]
    public async Task Reset_SetsIsLoadingFalse_AfterCompletion()
    {
        var (svc, vm) = Create();
        svc.Setup(s => s.GetSettingsAsync(It.IsAny<string>())).ReturnsAsync([]);
        svc.Setup(s => s.ResetToDefaultsAsync(It.IsAny<string>())).Returns(Task.CompletedTask);

        await vm.LoadAsync("students");
        await vm.ResetToDefaultsAsync();

        Assert.False(vm.IsLoading);
    }

    [Fact]
    public async Task Reset_ServiceThrows_ExceptionPropagates()
    {
        // ResetToDefaultsAsync does not have its own try/catch, exception propagates.
        var (svc, vm) = Create();
        svc.Setup(s => s.GetSettingsAsync(It.IsAny<string>())).ReturnsAsync([]);
        svc.Setup(s => s.ResetToDefaultsAsync(It.IsAny<string>()))
           .ThrowsAsync(new Exception("Reset error"));

        await vm.LoadAsync("students");
        await Assert.ThrowsAsync<Exception>(() => vm.ResetToDefaultsAsync());
    }

    [Fact]
    public async Task Reset_WithDefaultPage_ResetsStudentsPage()
    {
        // SelectedPage defaults to "students" — reset without prior LoadAsync.
        var (svc, vm) = Create();
        svc.Setup(s => s.ResetToDefaultsAsync("students")).Returns(Task.CompletedTask);
        svc.Setup(s => s.GetSettingsAsync("students")).ReturnsAsync([]);

        await vm.ResetToDefaultsAsync();

        svc.Verify(s => s.ResetToDefaultsAsync("students"), Times.Once);
    }
}

// =============================================================================
// SelectedPage / IsBusy / HasChanges property tests
// =============================================================================

public class TableSettingsViewModel_Properties
{
    private static (Mock<ITableSettingsService> svc, TableSettingsViewModel vm) Create()
    {
        var svc = new Mock<ITableSettingsService>();
        var vm  = new TableSettingsViewModel(svc.Object);
        return (svc, vm);
    }

    [Fact]
    public void InitialState_SelectedPage_IsStudents()
    {
        var (_, vm) = Create();
        Assert.Equal("students", vm.SelectedPage);
    }

    [Fact]
    public void InitialState_IsLoading_IsFalse()
    {
        var (_, vm) = Create();
        Assert.False(vm.IsLoading);
    }

    [Fact]
    public void InitialState_IsSaving_IsFalse()
    {
        var (_, vm) = Create();
        Assert.False(vm.IsSaving);
    }

    [Fact]
    public void InitialState_Settings_IsEmpty()
    {
        var (_, vm) = Create();
        Assert.Empty(vm.Settings);
    }

    [Fact]
    public async Task AfterLoad_SelectedPage_MatchesRequestedPage()
    {
        var (svc, vm) = Create();
        svc.Setup(s => s.GetSettingsAsync("classes")).ReturnsAsync([]);

        await vm.LoadAsync("classes");

        Assert.Equal("classes", vm.SelectedPage);
    }
}
