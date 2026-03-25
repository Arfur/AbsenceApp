/*
===============================================================================
 File        : RoleMapperTests.cs
 Namespace   : AbsenceApp.Tests.MapperTests
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : xUnit tests for RoleMapper — verifies ToDto and ToEntity.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial test suite.
===============================================================================
*/

using AbsenceApp.Core.DTOs;
using AbsenceApp.Data.Mappers;
using AbsenceApp.Data.Models;

namespace AbsenceApp.Tests.MapperTests;

public class RoleMapperTests
{
    // =========================================================================
    // ToDto tests
    // =========================================================================

    [Fact]
    public void ToDto_ShouldMapRoleIdAndRoleName()
    {
        var entity = new Role { RoleId = 3, RoleName = "Teacher" };
        var dto    = RoleMapper.ToDto(entity);

        Assert.Equal(3,         dto.RoleId);
        Assert.Equal("Teacher", dto.RoleName);
    }

    // =========================================================================
    // ToEntity tests
    // =========================================================================

    [Fact]
    public void ToEntity_ShouldMapRoleIdAndRoleName()
    {
        var dto    = new RoleDto { RoleId = 5, RoleName = "Student" };
        var entity = RoleMapper.ToEntity(dto);

        Assert.Equal(5,         entity.RoleId);
        Assert.Equal("Student", entity.RoleName);
    }

    // =========================================================================
    // Round-trip test
    // =========================================================================

    [Fact]
    public void RoundTrip_ShouldPreserveValues()
    {
        var original = new Role { RoleId = 7, RoleName = "Admin" };
        var restored = RoleMapper.ToEntity(RoleMapper.ToDto(original));

        Assert.Equal(original.RoleId,   restored.RoleId);
        Assert.Equal(original.RoleName, restored.RoleName);
    }
}
