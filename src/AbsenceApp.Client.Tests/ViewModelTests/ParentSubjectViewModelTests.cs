// ============================================================
// File:    ParentSubjectViewModelTests.cs
// Project: AbsenceApp.Client.Tests
// Purpose: Unit tests for ParentListViewModel, ParentDetailsViewModel,
//          SubjectListViewModel, and SubjectDetailsViewModel.
// ============================================================

using AbsenceApp.Core.Interfaces;

namespace AbsenceApp.Client.Tests.ViewModelTests;

public sealed class ParentSubjectViewModelTests
{
    // =========================================================================
    // ParentListViewModel
    // =========================================================================

    [Fact]
    public async Task ParentList_LoadAsync_PopulatesContacts()
    {
        var contacts = new List<StudentContactDto>
        {
            new() { Id = 1, StudentId = 10, ContactName = "Naomi Rose", Relationship = "Mother", PhoneMobile = "07000111", Email = "naomi@test.com", Priority = 1, HasParentalResponsibility = true },
            new() { Id = 2, StudentId = 10, ContactName = "Steve Jones", Relationship = "Father", PhoneMobile = "07000222", Email = "steve@test.com", Priority = 2, HasParentalResponsibility = true }
        };

        var svc = new Mock<IStudentContactService>();
        svc.Setup(s => s.GetByStudentAsync(10)).ReturnsAsync(contacts);

        var vm = new ParentListViewModel(svc.Object);
        await vm.LoadAsync(10);

        Assert.Equal(2, vm.Contacts.Count());
        Assert.False(vm.IsLoading);
        svc.Verify(s => s.GetByStudentAsync(10), Times.Once);
    }

    [Fact]
    public async Task ParentList_LoadAsync_SetsErrorOnException()
    {
        var svc = new Mock<IStudentContactService>();
        svc.Setup(s => s.GetByStudentAsync(It.IsAny<long>())).ThrowsAsync(new Exception("not found"));

        var vm = new ParentListViewModel(svc.Object);
        await vm.LoadAsync(99);

        Assert.Empty(vm.Contacts);
        Assert.Equal("not found", vm.ErrorMessage);
    }

    // =========================================================================
    // ParentDetailsViewModel
    // =========================================================================

    [Fact]
    public async Task ParentDetails_LoadAsync_PopulatesContact()
    {
        var contact = new StudentContactDto
        {
            Id = 1, StudentId = 10, ContactName = "Naomi Rose",
            Relationship = "Mother", PhoneMobile = "07000111", Email = "naomi@test.com",
            Priority = 1, HasParentalResponsibility = true
        };

        var svc = new Mock<IStudentContactService>();
        svc.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(contact);

        var vm = new ParentDetailsViewModel(svc.Object);
        await vm.LoadAsync(1);

        Assert.NotNull(vm.Contact);
        Assert.Equal("Naomi Rose", vm.Contact!.ContactName);
        Assert.False(vm.IsLoading);
    }

    [Fact]
    public async Task ParentDetails_LoadAsync_SetsErrorOnException()
    {
        var svc = new Mock<IStudentContactService>();
        svc.Setup(s => s.GetByIdAsync(It.IsAny<long>())).ThrowsAsync(new Exception("missing"));

        var vm = new ParentDetailsViewModel(svc.Object);
        await vm.LoadAsync(99);

        Assert.Null(vm.Contact);
        Assert.Equal("missing", vm.ErrorMessage);
    }

    // =========================================================================
    // SubjectListViewModel
    // =========================================================================

    [Fact]
    public async Task SubjectList_LoadAsync_PopulatesSubjects()
    {
        var subjects = new List<SubjectDto>
        {
            new() { Id = 1, Name = "English",     Description = "English Language" },
            new() { Id = 2, Name = "Mathematics",  Description = "Pure Maths" }
        };

        var svc = new Mock<ISubjectService>();
        svc.Setup(s => s.GetAllAsync()).ReturnsAsync(subjects);

        var vm = new SubjectListViewModel(svc.Object);
        await vm.LoadAsync();

        Assert.Equal(2, vm.Subjects.Count());
        Assert.False(vm.IsLoading);
        svc.Verify(s => s.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task SubjectList_LoadAsync_SetsErrorOnException()
    {
        var svc = new Mock<ISubjectService>();
        svc.Setup(s => s.GetAllAsync()).ThrowsAsync(new Exception("db error"));

        var vm = new SubjectListViewModel(svc.Object);
        await vm.LoadAsync();

        Assert.Empty(vm.Subjects);
        Assert.Equal("db error", vm.ErrorMessage);
    }

    // =========================================================================
    // SubjectDetailsViewModel
    // =========================================================================

    [Fact]
    public async Task SubjectDetails_LoadAsync_PopulatesSubject()
    {
        var subject = new SubjectDto { Id = 1, Name = "English", Description = "English Language" };

        var svc = new Mock<ISubjectService>();
        svc.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(subject);

        var vm = new SubjectDetailsViewModel(svc.Object);
        await vm.LoadAsync(1);

        Assert.NotNull(vm.Subject);
        Assert.Equal("English", vm.Subject!.Name);
        Assert.False(vm.IsLoading);
    }

    [Fact]
    public async Task SubjectDetails_LoadAsync_SetsErrorOnException()
    {
        var svc = new Mock<ISubjectService>();
        svc.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ThrowsAsync(new Exception("not found"));

        var vm = new SubjectDetailsViewModel(svc.Object);
        await vm.LoadAsync(99);

        Assert.Null(vm.Subject);
        Assert.NotNull(vm.ErrorMessage);
    }

    // =========================================================================
    // SubjectAddViewModel
    // =========================================================================

    [Fact]
    public async Task SubjectAdd_SaveAsync_SetsSavedOnSuccess()
    {
        var input  = new SubjectDto { Name = "Biology", Description = "Life Sciences" };
        var result = new SubjectDto { Id = 5, Name = "Biology", Description = "Life Sciences" };

        var svc = new Mock<ISubjectService>();
        svc.Setup(s => s.CreateAsync(It.IsAny<SubjectDto>())).ReturnsAsync(result);

        var vm = new SubjectAddViewModel(svc.Object);
        vm.NewSubject = input;
        await vm.SaveAsync();

        Assert.True(vm.IsSaved);
        Assert.False(vm.IsSaving);
        Assert.Null(vm.ErrorMessage);
        Assert.Equal(5, vm.NewSubject.Id);
        svc.Verify(s => s.CreateAsync(It.IsAny<SubjectDto>()), Times.Once);
    }

    [Fact]
    public async Task SubjectAdd_SaveAsync_SetsErrorOnException()
    {
        var svc = new Mock<ISubjectService>();
        svc.Setup(s => s.CreateAsync(It.IsAny<SubjectDto>())).ThrowsAsync(new Exception("save failed"));

        var vm = new SubjectAddViewModel(svc.Object);
        vm.NewSubject = new SubjectDto { Name = "Chemistry" };
        await vm.SaveAsync();

        Assert.False(vm.IsSaved);
        Assert.False(vm.IsSaving);
        Assert.Equal("save failed", vm.ErrorMessage);
    }
}
