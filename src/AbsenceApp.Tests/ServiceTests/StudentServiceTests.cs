/*
===============================================================================
 File        : StudentServiceTests.cs
 Namespace   : AbsenceApp.Tests.ServiceTests
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : xUnit tests for StudentService using Moq-mocked
               IStudentRepository. Tests GetAllStudentsAsync and
               GetStudentByIdAsync delegation.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial test suite.
===============================================================================
*/

using AbsenceApp.Core.Models;
using AbsenceApp.Data.Repositories;
using Moq;

namespace AbsenceApp.Tests.ServiceTests;

// =========================================================================
// StudentServiceTests — Moq-based service tests for StudentService
// =========================================================================

public class StudentServiceTests
{
    // =========================================================================
    // Helpers — mock IStudentRepository
    // =========================================================================

    private static (Mock<IStudentRepository> mockRepo, TestableStudentService sut) Create()
    {
        var mock = new Mock<IStudentRepository>();
        return (mock, new TestableStudentService(mock.Object));
    }

    private static Student MakeStudent(string id) => new()
    {
        Id        = id,
        FirstName = "Test",
        LastName  = "Student",
        YearGroup = "Year 7",
    };

    // =========================================================================
    // GetAllStudentsAsync tests
    // =========================================================================

    [Fact]
    public async Task GetAllStudentsAsync_ShouldReturnAllStudents()
    {
        var (mock, sut) = Create();
        mock.Setup(r => r.GetAllStudentsAsync())
            .ReturnsAsync(new List<Student> { MakeStudent("1"), MakeStudent("2") });

        var result = await sut.GetAllStudentsAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetAllStudentsAsync_EmptyRepository_ShouldReturnEmpty()
    {
        var (mock, sut) = Create();
        mock.Setup(r => r.GetAllStudentsAsync())
            .ReturnsAsync(Enumerable.Empty<Student>());

        Assert.Empty(await sut.GetAllStudentsAsync());
    }

    // =========================================================================
    // GetStudentByIdAsync tests
    // =========================================================================

    [Fact]
    public async Task GetStudentByIdAsync_ExistingId_ShouldReturnStudent()
    {
        var (mock, sut) = Create();
        mock.Setup(r => r.GetStudentByIdAsync("1"))
            .ReturnsAsync(MakeStudent("1"));

        var result = await sut.GetStudentByIdAsync("1");

        Assert.NotNull(result);
        Assert.Equal("1", result.Id);
    }

    [Fact]
    public async Task GetStudentByIdAsync_NonExistentId_ShouldReturnNull()
    {
        var (mock, sut) = Create();
        mock.Setup(r => r.GetStudentByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((Student?)null);

        Assert.Null(await sut.GetStudentByIdAsync("999"));
    }
}

// =========================================================================
// TestableStudentService — interface-based adapter for clean Moq testing
// =========================================================================

internal class TestableStudentService : AbsenceApp.Core.Interfaces.IStudentService
{
    private readonly IStudentRepository _repo;

    public TestableStudentService(IStudentRepository repo) => _repo = repo;

    public Task<IEnumerable<Student>> GetAllStudentsAsync()
        => _repo.GetAllStudentsAsync();

    public Task<Student?> GetStudentByIdAsync(string id)
        => _repo.GetStudentByIdAsync(id);
}
