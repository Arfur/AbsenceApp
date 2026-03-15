/*
===============================================================================
 File        : AttendanceMapperTests.cs
 Namespace   : AbsenceApp.Tests.MapperTests
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : xUnit tests for AttendanceMapper — verifies ToDto and ToEntity
               including nullable ClassId handling.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial test suite.
===============================================================================
*/

using AbsenceApp.Core.DTOs;
using AbsenceApp.Data.Mappers;
using AbsenceApp.Data.Models;

namespace AbsenceApp.Tests.MapperTests;

public class AttendanceMapperTests
{
    // =========================================================================
    // Helpers
    // =========================================================================

    private static readonly DateTime _ts = new(2026, 3, 13, 9, 0, 0, DateTimeKind.Utc);

    private static Attendance SampleEntity() => new()
    {
        AttendanceId = 5,
        UserId       = 10,
        ClassId      = 3,
        Status       = "Present",
        Timestamp    = _ts,
        RecordedBy   = 20,
    };

    private static AttendanceDto SampleDto() => new()
    {
        AttendanceId = 5,
        UserId       = 10,
        ClassId      = 3,
        Status       = "Present",
        Timestamp    = _ts,
        RecordedBy   = 20,
    };

    // =========================================================================
    // ToDto tests
    // =========================================================================

    [Fact]
    public void ToDto_ShouldMapAllFields()
    {
        var dto = AttendanceMapper.ToDto(SampleEntity());

        Assert.Equal(5,         dto.AttendanceId);
        Assert.Equal(10,        dto.UserId);
        Assert.Equal(3,         dto.ClassId);
        Assert.Equal("Present", dto.Status);
        Assert.Equal(_ts,       dto.Timestamp);
        Assert.Equal(20,        dto.RecordedBy);
    }

    [Fact]
    public void ToDto_NullClassId_ShouldMapAsNull()
    {
        var entity = SampleEntity();
        entity.ClassId = null;
        var dto = AttendanceMapper.ToDto(entity);

        Assert.Null(dto.ClassId);
    }

    // =========================================================================
    // ToEntity tests
    // =========================================================================

    [Fact]
    public void ToEntity_ShouldMapAllFields()
    {
        var entity = AttendanceMapper.ToEntity(SampleDto());

        Assert.Equal(5,         entity.AttendanceId);
        Assert.Equal(10,        entity.UserId);
        Assert.Equal(3,         entity.ClassId);
        Assert.Equal("Present", entity.Status);
        Assert.Equal(_ts,       entity.Timestamp);
        Assert.Equal(20,        entity.RecordedBy);
    }

    // =========================================================================
    // Round-trip test
    // =========================================================================

    [Fact]
    public void RoundTrip_ShouldPreserveAllValues()
    {
        var original = SampleEntity();
        var restored = AttendanceMapper.ToEntity(AttendanceMapper.ToDto(original));

        Assert.Equal(original.AttendanceId, restored.AttendanceId);
        Assert.Equal(original.UserId,       restored.UserId);
        Assert.Equal(original.ClassId,      restored.ClassId);
        Assert.Equal(original.Status,       restored.Status);
        Assert.Equal(original.Timestamp,    restored.Timestamp);
        Assert.Equal(original.RecordedBy,   restored.RecordedBy);
    }
}
