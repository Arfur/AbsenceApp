/*
===============================================================================
 File        : StudentMapperTests.cs
 Namespace   : AbsenceApp.Tests.MapperTests
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-15
 Updated     : 2026-03-15
-------------------------------------------------------------------------------
 Purpose     : xUnit tests for StudentMapper verifying correct field projection
               between the Data-layer Student entity (TABLE29) and the Core domain model.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-15  Initial creation.
-------------------------------------------------------------------------------
===============================================================================
*/

using AbsenceApp.Data.Mappers;

using CoreStudent = AbsenceApp.Core.Models.Student;
using DataStudent = AbsenceApp.Data.Models.Student;

namespace AbsenceApp.Tests.MapperTests;

public class StudentMapperTests
{
    // =========================================================================
    // Helpers
    // =========================================================================

    private static DataStudent SampleDataStudent() => new()
    {
        Id             = 5,
        FirstName      = "Jane",
        LastName       = "Smith",
        LegalFirstName = "Jane",
        LegalLastName  = "Smith",
        Gender         = "Female",
        AdmissionNumber = "ADM001",
        Status         = "Active",
        YearGroupId    = 3,
        ClassId        = 1,
        SchoolId       = 1,
        CreatedAt      = DateTime.UtcNow,
        UpdatedAt      = DateTime.UtcNow,
    };

    private static CoreStudent SampleCoreStudent() => new()
    {
        Id        = "5",
        FirstName = "Jane",
        LastName  = "Smith",
        YearGroup = "Year 9",
    };

    // =========================================================================
    // ToDomain tests
    // =========================================================================

    [Fact]
    public void ToDomain_ShouldMapIdFirstNameLastName()
    {
        var domain = StudentMapper.ToDomain(SampleDataStudent());

        Assert.Equal("5",     domain.Id);
        Assert.Equal("Jane",  domain.FirstName);
        Assert.Equal("Smith", domain.LastName);
    }

    [Fact]
    public void ToDomain_YearGroup_ShouldBeYearGroupIdString()
    {
        var domain = StudentMapper.ToDomain(SampleDataStudent());

        Assert.Equal("3", domain.YearGroup);
    }

    // =========================================================================
    // ToEntity tests
    // =========================================================================

    [Fact]
    public void ToEntity_ShouldMapFirstNameAndLastName()
    {
        var entity = StudentMapper.ToEntity(SampleCoreStudent());

        Assert.Equal("Jane",  entity.FirstName);
        Assert.Equal("Smith", entity.LastName);
    }

    [Fact]
    public void ToEntity_ShouldSetStatusToActive()
    {
        var entity = StudentMapper.ToEntity(SampleCoreStudent());

        Assert.Equal("Active", entity.Status);
    }
}
