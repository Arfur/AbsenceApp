/*
===============================================================================
 File        : StudentServiceTests.cs
 Namespace   : AbsenceApp.Tests.ServiceTests
 Author      : Michael
 Version     : 2.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-17
-------------------------------------------------------------------------------
 Purpose     : xUnit tests for the EF-backed StudentService.
               Tests GetAllAsync and GetByIdAsync via Moq-mocked IStudentRepository.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial test suite (in-memory).
   - 2.0.0  2026-03-17  Migrated to EF-backed service returning StudentDto.
===============================================================================
*/

using AbsenceApp.Core.DTOs;
using AbsenceApp.Data.Repositories;
using AbsenceApp.Data.Services;
using Moq;
using DataStudent = AbsenceApp.Data.Models.Student;

namespace AbsenceApp.Tests.ServiceTests;

public class StudentServiceTests
{
    // =========================================================================
    // Helpers
    // =========================================================================

    private static (Mock<IStudentRepository> mockRepo, StudentService sut) Create()
    {
        var mock = new Mock<IStudentRepository>();
        return (mock, new StudentService(mock.Object));
    }

    private static DataStudent MakeEntity(long id, string firstName = "Test", string lastName = "Student", long yearGroupId = 7) => new()
    {
        Id              = id,
        FirstName       = firstName,
        LastName        = lastName,
        LegalFirstName  = firstName,
        LegalLastName   = lastName,
        Gender          = "M",
        AdmissionNumber = $"ADM{id:D3}",
        Status          = "Active",
        YearGroupId     = yearGroupId,
        ClassId         = 1,
        SchoolId        = 1,
        CreatedAt       = DateTime.UtcNow,
        UpdatedAt       = DateTime.UtcNow,
    };

    // =========================================================================
    // GetAllAsync tests
    // =========================================================================

    [Fact]
    public async Task GetAllAsync_ShouldReturnMappedDtos()
    {
        var (mock, sut) = Create();
        mock.Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<DataStudent> { MakeEntity(1), MakeEntity(2) });

        var result = (await sut.GetAllAsync()).ToList();

        Assert.Equal(2, result.Count);
        Assert.All(result, d => Assert.Equal("Test", d.FirstName));
    }

    [Fact]
    public async Task GetAllAsync_EmptyRepository_ShouldReturnEmpty()
    {
        var (mock, sut) = Create();
        mock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<DataStudent>());

        Assert.Empty(await sut.GetAllAsync());
    }

    // =========================================================================
    // GetByIdAsync tests
    // =========================================================================

    [Fact]
    public async Task GetByIdAsync_ExistingId_ShouldReturnDto()
    {
        var (mock, sut) = Create();
        mock.Setup(r => r.GetByIdAsync(1L)).ReturnsAsync(MakeEntity(1));

        var result = await sut.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1L,    result.Id);
        Assert.Equal("Test", result.FirstName);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistentId_ShouldReturnNull()
    {
        var (mock, sut) = Create();
        mock.Setup(r => r.GetByIdAsync(It.IsAny<long>())).ReturnsAsync((DataStudent?)null);

        Assert.Null(await sut.GetByIdAsync(999));
    }
}
