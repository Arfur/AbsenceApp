/*
===============================================================================
 File        : AuditLogMapperTests.cs
 Namespace   : AbsenceApp.Tests.MapperTests
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : xUnit tests for AuditLogMapper — verifies ToDto and ToEntity.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial test suite.
===============================================================================
*/

using AbsenceApp.Core.DTOs;
using AbsenceApp.Data.Mappers;
using AbsenceApp.Data.Models;

namespace AbsenceApp.Tests.MapperTests;

public class AuditLogMapperTests
{
    // =========================================================================
    // Helpers
    // =========================================================================

    private static readonly DateTime _ts = new(2026, 3, 13, 8, 30, 0, DateTimeKind.Utc);

    private static AuditLog SampleEntity() => new()
    {
        AuditId   = 7,
        UserId    = 11,
        Action    = "Login",
        Timestamp = _ts,
    };

    private static AuditLogDto SampleDto() => new()
    {
        AuditId   = 7,
        UserId    = 11,
        Action    = "Login",
        Timestamp = _ts,
    };

    // =========================================================================
    // ToDto tests
    // =========================================================================

    [Fact]
    public void ToDto_ShouldMapAllFields()
    {
        var dto = AuditLogMapper.ToDto(SampleEntity());

        Assert.Equal(7,       dto.AuditId);
        Assert.Equal(11,      dto.UserId);
        Assert.Equal("Login", dto.Action);
        Assert.Equal(_ts,     dto.Timestamp);
    }

    // =========================================================================
    // ToEntity tests
    // =========================================================================

    [Fact]
    public void ToEntity_ShouldMapAllFields()
    {
        var entity = AuditLogMapper.ToEntity(SampleDto());

        Assert.Equal(7,       entity.AuditId);
        Assert.Equal(11,      entity.UserId);
        Assert.Equal("Login", entity.Action);
        Assert.Equal(_ts,     entity.Timestamp);
    }

    // =========================================================================
    // Round-trip test
    // =========================================================================

    [Fact]
    public void RoundTrip_ShouldPreserveAllValues()
    {
        var original = SampleEntity();
        var restored = AuditLogMapper.ToEntity(AuditLogMapper.ToDto(original));

        Assert.Equal(original.AuditId,   restored.AuditId);
        Assert.Equal(original.UserId,    restored.UserId);
        Assert.Equal(original.Action,    restored.Action);
        Assert.Equal(original.Timestamp, restored.Timestamp);
    }
}
