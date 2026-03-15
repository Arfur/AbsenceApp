/*
===============================================================================
 File        : ClassMapperTests.cs
 Namespace   : AbsenceApp.Tests.MapperTests
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-15
 Updated     : 2026-03-15
-------------------------------------------------------------------------------
 Purpose     : xUnit tests for ClassMapper using the TABLE9 Class entity schema.
               Verifies round-trip and field-level mappings between Class and ClassDto.
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

public class ClassMapperTests
{
    // =========================================================================
    // Tests — ToDto
    // =========================================================================

    [Fact]
    public void ToDto_ShouldMapAllFields()
    {
        var entity = new Class { Id = 1, Name = "8A", Description = "Year 8 Set A" };
        var dto    = ClassMapper.ToDto(entity);

        Assert.Equal(1,              dto.ClassId);
        Assert.Equal("8A",           dto.ClassName);
        Assert.Equal("Year 8 Set A", dto.Description);
    }

    [Fact]
    public void ToDto_NullDescription_ShouldMapAsNull()
    {
        var entity = new Class { Id = 2, Name = "9B", Description = null };
        var dto    = ClassMapper.ToDto(entity);

        Assert.Null(dto.Description);
    }

    // =========================================================================
    // Tests — ToEntity / round-trip
    // =========================================================================

    [Fact]
    public void ToEntity_ShouldMapAllFields()
    {
        var dto    = new ClassDto { ClassId = 3, ClassName = "10C", Description = "Top Set" };
        var entity = ClassMapper.ToEntity(dto);

        Assert.Equal(3,         entity.Id);
        Assert.Equal("10C",     entity.Name);
        Assert.Equal("Top Set", entity.Description);
    }

    [Fact]
    public void RoundTrip_ShouldPreserveValues()
    {
        var original = new Class { Id = 4, Name = "11D", Description = "Bottom Set" };
        var restored = ClassMapper.ToEntity(ClassMapper.ToDto(original));

        Assert.Equal(original.Id,          (long)restored.Id);
        Assert.Equal(original.Name,        restored.Name);
        Assert.Equal(original.Description, restored.Description);
    }
}
