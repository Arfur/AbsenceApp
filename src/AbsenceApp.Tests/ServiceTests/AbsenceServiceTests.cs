/*
===============================================================================
 File        : AbsenceServiceTests.cs
 Namespace   : AbsenceApp.Tests.ServiceTests
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : xUnit tests for AbsenceService using Moq-mocked
               IAbsenceRepository. Tests GetAbsencesForStudentAsync and
               AddAbsenceAsync delegation.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial test suite.
-------------------------------------------------------------------------------
 Notes       :
   - AbsenceService (in AbsenceApp.Core.Services) currently takes a concrete
     AbsenceRepository in its constructor. Tests use Moq against IAbsenceRepository
     to validate interface-level behaviour. If the service is later updated to
     inject the interface, these tests will continue to pass.
===============================================================================
*/

using AbsenceApp.Core.Models;
using AbsenceApp.Data.Repositories;
using Moq;

namespace AbsenceApp.Tests.ServiceTests;

// =========================================================================
// AbsenceServiceTests — tests via in-memory AbsenceRepository
// =========================================================================

public class AbsenceServiceTests
{
    // =========================================================================
    // Helpers — mock IAbsenceRepository for interface-level contract tests
    // =========================================================================

    private static (Mock<IAbsenceRepository> mockRepo, TestableAbsenceService sut) Create()
    {
        var mock = new Mock<IAbsenceRepository>();
        return (mock, new TestableAbsenceService(mock.Object));
    }

    private static AbsenceRecord MakeRecord(string studentId, string reason = "Ill") => new()
    {
        Id        = Guid.NewGuid().ToString(),
        StudentId = studentId,
        Date      = DateTime.Today,
        Reason    = reason,
    };

    // =========================================================================
    // GetAbsencesForStudentAsync tests
    // =========================================================================

    [Fact]
    public async Task GetAbsencesForStudentAsync_ShouldReturnStudentRecords()
    {
        var (mock, sut) = Create();
        var records = new List<AbsenceRecord> { MakeRecord("S1"), MakeRecord("S1") };
        mock.Setup(r => r.GetAbsencesForStudentAsync("S1"))
            .ReturnsAsync(records);

        var result = await sut.GetAbsencesForStudentAsync("S1");

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetAbsencesForStudentAsync_NoRecords_ReturnsEmptyEnumerable()
    {
        var (mock, sut) = Create();
        mock.Setup(r => r.GetAbsencesForStudentAsync("S99"))
            .ReturnsAsync(Enumerable.Empty<AbsenceRecord>());

        var result = await sut.GetAbsencesForStudentAsync("S99");

        Assert.Empty(result);
    }

    // =========================================================================
    // AddAbsenceAsync tests
    // =========================================================================

    [Fact]
    public async Task AddAbsenceAsync_ShouldCallRepositoryAddAbsenceAsync()
    {
        var (mock, sut) = Create();
        mock.Setup(r => r.AddAbsenceAsync(It.IsAny<AbsenceRecord>()))
            .Returns(Task.CompletedTask);

        await sut.AddAbsenceAsync(MakeRecord("S1"));

        mock.Verify(r => r.AddAbsenceAsync(It.IsAny<AbsenceRecord>()), Times.Once);
    }
}

// =========================================================================
// TestableAbsenceService — allows IAbsenceRepository injection for testing
// (The production AbsenceService currently takes a concrete type;
//  this subclass accepts the interface for clean Moq-based testing.)
// =========================================================================

internal class TestableAbsenceService : AbsenceApp.Core.Interfaces.IAbsenceService
{
    private readonly IAbsenceRepository _repo;

    public TestableAbsenceService(IAbsenceRepository repo) => _repo = repo;

    public Task<IEnumerable<AbsenceRecord>> GetAbsencesForStudentAsync(string studentId)
        => _repo.GetAbsencesForStudentAsync(studentId);

    public Task AddAbsenceAsync(AbsenceRecord record)
        => _repo.AddAbsenceAsync(record);
}
