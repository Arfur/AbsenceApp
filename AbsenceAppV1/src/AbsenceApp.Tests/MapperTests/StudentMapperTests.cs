/*
===============================================================================
 File        : StudentMapperTests.cs
 Namespace   : AbsenceApp.Tests.MapperTests
 Author      : Michael
 Version     : 2.0.0
 Updated     : 2026-03-17
-------------------------------------------------------------------------------
 Purpose     : xUnit tests for StudentMapper verifying Data.Models.Student ↔ StudentDto.
===============================================================================
*/

using AbsenceApp.Core.DTOs;
using AbsenceApp.Data.Mappers;
using DataStudent = AbsenceApp.Data.Models.Student;

namespace AbsenceApp.Tests.MapperTests;

public class StudentMapperTests
{
    // =========================================================================
    // Helpers
    // =========================================================================

    private static DataStudent SampleEntity() => new()
    {
        Id              = 5,
        FirstName       = "Jane",
        LastName        = "Smith",
        LegalFirstName  = "Jane",
        LegalLastName   = "Smith",
        Gender          = "Female",
        AdmissionNumber = "ADM001",
        Status          = "Active",
        YearGroupId     = 3,
        ClassId         = 1,
        SchoolId        = 1,
        CreatedAt       = DateTime.UtcNow,
        UpdatedAt       = DateTime.UtcNow,
    };

    private static StudentDto SampleDto() => new()
    {
        Id              = 5,
        FirstName       = "Jane",
        LastName        = "Smith",
        LegalFirstName  = "Jane",
        LegalLastName   = "Smith",
        Gender          = "Female",
        AdmissionNumber = "ADM001",
        Status          = "Active",
        YearGroupId     = 3,
        ClassId         = 1,
        SchoolId        = 1,
    };

    // =========================================================================
    // ToDto tests
    // =========================================================================

    [Fact]
    public void ToDto_ShouldMapIdFirstNameLastName()
    {
        var dto = StudentMapper.ToDto(SampleEntity());

        Assert.Equal(5L,      dto.Id);
        Assert.Equal("Jane",  dto.FirstName);
        Assert.Equal("Smith", dto.LastName);
    }

    [Fact]
    public void ToDto_ShouldMapYearGroupId()
    {
        var dto = StudentMapper.ToDto(SampleEntity());

        Assert.Equal(3L, dto.YearGroupId);
    }

    [Fact]
    public void ToDto_ShouldMapStatus()
    {
        var dto = StudentMapper.ToDto(SampleEntity());

        Assert.Equal("Active", dto.Status);
    }

    // =========================================================================
    // ToEntity tests
    // =========================================================================

    [Fact]
    public void ToEntity_ShouldMapFirstNameAndLastName()
    {
        var entity = StudentMapper.ToEntity(SampleDto());

        Assert.Equal("Jane",  entity.FirstName);
        Assert.Equal("Smith", entity.LastName);
    }

    [Fact]
    public void ToEntity_ShouldMapStatusToActive()
    {
        var entity = StudentMapper.ToEntity(SampleDto());

        Assert.Equal("Active", entity.Status);
    }

    [Fact]
    public void RoundTrip_ShouldPreserveId()
    {
        var original = SampleEntity();
        var dto      = StudentMapper.ToDto(original);
        var restored = StudentMapper.ToEntity(dto);

        Assert.Equal(original.Id,        restored.Id);
        Assert.Equal(original.FirstName, restored.FirstName);
        Assert.Equal(original.LastName,  restored.LastName);
    }
}
