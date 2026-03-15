/*
===============================================================================
 File        : AbsenceMapperTests.cs
 Namespace   : AbsenceApp.Tests.MapperTests
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : xUnit tests for AbsenceMapper — verifies ToDomain and ToEntity
               including null Reason handling and non-parseable StudentId.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial test suite.
===============================================================================
*/

using AbsenceApp.Core.Models;
using AbsenceApp.Data.Mappers;
using AbsenceApp.Data.Models;

namespace AbsenceApp.Tests.MapperTests;

public class AbsenceMapperTests
{
    // =========================================================================
    // Helpers
    // =========================================================================

    private static readonly DateTime _date = new(2026, 3, 13, 0, 0, 0, DateTimeKind.Utc);

    private static Attendance SampleAttendance() => new()
    {
        AttendanceId = 42,
        UserId       = 7,
        Status       = "Absent",
        Timestamp    = _date.AddHours(9), // has a time component that should be stripped
        RecordedBy   = 1,
    };

    private static AbsenceRecord SampleDomain() => new()
    {
        Id        = "42",
        StudentId = "7",
        Date      = _date,
        Reason    = "Absent",
    };

    // =========================================================================
    // ToDomain tests
    // =========================================================================

    [Fact]
    public void ToDomain_ShouldMapAllFields()
    {
        var domain = AbsenceMapper.ToDomain(SampleAttendance());

        Assert.Equal("42",      domain.Id);
        Assert.Equal("7",       domain.StudentId);
        Assert.Equal(_date,     domain.Date);          // time component stripped
        Assert.Equal("Absent",  domain.Reason);
    }

    [Fact]
    public void ToDomain_ShouldStripTimeFromTimestamp()
    {
        var entity = SampleAttendance();
        entity.Timestamp = _date.AddHours(14).AddMinutes(35);

        var domain = AbsenceMapper.ToDomain(entity);

        Assert.Equal(_date, domain.Date);
        Assert.Equal(TimeSpan.Zero, domain.Date.TimeOfDay);
    }

    // =========================================================================
    // ToEntity tests
    // =========================================================================

    [Fact]
    public void ToEntity_ShouldMapAllFields()
    {
        const int recordedBy = 99;
        var entity = AbsenceMapper.ToEntity(SampleDomain(), recordedBy);

        Assert.Equal(7,        entity.UserId);
        Assert.Equal("Absent", entity.Status);
        Assert.Equal(_date,    entity.Timestamp);
        Assert.Equal(99,       entity.RecordedBy);
    }

    [Fact]
    public void ToEntity_NonParsableStudentId_ShouldDefaultUserIdToZero()
    {
        var domain = new AbsenceRecord { Id = "x", StudentId = "not-an-int", Date = _date, Reason = "Late" };
        var entity = AbsenceMapper.ToEntity(domain, recordedBy: 1);

        Assert.Equal(0, entity.UserId);
    }

    [Fact]
    public void ToEntity_NullReason_ShouldMapToEmptyString()
    {
        var domain = new AbsenceRecord { Id = "1", StudentId = "2", Date = _date, Reason = null! };
        var entity = AbsenceMapper.ToEntity(domain, recordedBy: 1);

        Assert.Equal(string.Empty, entity.Status);
    }
}
