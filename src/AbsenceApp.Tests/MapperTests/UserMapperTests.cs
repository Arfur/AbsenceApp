/*
===============================================================================
 File        : UserMapperTests.cs
 Namespace   : AbsenceApp.Tests.MapperTests
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-15
 Updated     : 2026-03-15
-------------------------------------------------------------------------------
 Purpose     : xUnit tests for UserMapper using the TABLE1 User entity schema.
               Verifies name combination, status-to-bool, and round-trip mappings.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-15  Initial creation.
-------------------------------------------------------------------------------
===============================================================================
*/

using AbsenceApp.Core.DTOs;
using AbsenceApp.Data.Mappers;
using AbsenceApp.Data.Models;

namespace AbsenceApp.Tests.MapperTests;

public class UserMapperTests
{
    // =========================================================================
    // Helpers
    // =========================================================================

    private static User SampleUser() => new()
    {
        Id        = 99,
        Name      = "Jane Doe",
        Username  = "jane.doe",
        Email     = "jane@example.com",
        Status    = "Active",
        Password  = "hashed",
        RoleTypeId = 1,
        CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
        UpdatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
    };

    private static UserDto SampleDto() => new()
    {
        UserId    = 99,
        Username  = "jane.doe",
        FirstName = "Jane Doe",
        LastName  = string.Empty,
        Email     = "jane@example.com",
        IsActive  = true,
        CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
    };

    // =========================================================================
    // ToDto tests
    // =========================================================================

    [Fact]
    public void ToDto_ShouldMapIdUsernameEmailStatus()
    {
        var entity = SampleUser();
        var dto    = UserMapper.ToDto(entity);

        Assert.Equal(99,            dto.UserId);
        Assert.Equal("jane.doe",    dto.Username);
        Assert.Equal("jane@example.com", dto.Email);
        Assert.True(dto.IsActive);
    }

    [Fact]
    public void ToDto_NameMapsToFirstName()
    {
        var dto = UserMapper.ToDto(SampleUser());

        Assert.Equal("Jane Doe", dto.FirstName);
    }

    [Fact]
    public void ToDto_FullName_ShouldEqualName()
    {
        var dto = UserMapper.ToDto(SampleUser());

        Assert.Equal("Jane Doe", dto.FullName);
    }

    // =========================================================================
    // ToEntity tests
    // =========================================================================

    [Fact]
    public void ToEntity_ShouldMapUsernameEmailStatus()
    {
        var dto    = SampleDto();
        var entity = UserMapper.ToEntity(dto);

        Assert.Equal("jane.doe",        entity.Username);
        Assert.Equal("jane@example.com", entity.Email);
        Assert.Equal("Active",           entity.Status);
    }
}
