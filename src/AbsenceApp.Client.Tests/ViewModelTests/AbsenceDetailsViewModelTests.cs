// ============================================================
// File:    AbsenceDetailsViewModelTests.cs
// Project: AbsenceApp.Client.Tests
// Purpose: Unit tests for AbsenceDetailsViewModel (pure .NET — no bUnit).
// ============================================================

namespace AbsenceApp.Client.Tests.ViewModelTests;

public sealed class AbsenceDetailsViewModelTests
{
    [Fact]
    public async Task SaveAsync_CallsAddAbsenceAsync()
    {
        var svc = new Mock<IAbsenceService>();
        svc.Setup(s => s.AddAbsenceAsync(It.IsAny<AbsenceRecord>()))
           .Returns(Task.CompletedTask);

        var vm = new AbsenceDetailsViewModel(svc.Object);
        vm.CurrentRecord = new AbsenceRecord
        {
            StudentId = "42",
            Date      = DateTime.Today,
            Reason    = "Sick"
        };

        await vm.SaveAsync();

        svc.Verify(s => s.AddAbsenceAsync(It.Is<AbsenceRecord>(r =>
            r.StudentId == "42" && r.Reason == "Sick")), Times.Once);
    }

    [Fact]
    public async Task SaveAsync_SetsIsSavedTrue()
    {
        var svc = new Mock<IAbsenceService>();
        svc.Setup(s => s.AddAbsenceAsync(It.IsAny<AbsenceRecord>()))
           .Returns(Task.CompletedTask);

        var vm = new AbsenceDetailsViewModel(svc.Object);
        vm.CurrentRecord = new AbsenceRecord
        {
            StudentId = "7",
            Date      = DateTime.Today,
            Reason    = "Holiday"
        };

        await vm.SaveAsync();

        Assert.True(vm.IsSaved);
    }
}
