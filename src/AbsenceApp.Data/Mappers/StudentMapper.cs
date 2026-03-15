/*
===============================================================================
 File        : StudentMapper.cs
 Namespace   : AbsenceApp.Data.Mappers
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-15
 Updated     : 2026-03-15
-------------------------------------------------------------------------------
 Purpose     : Maps between the Student EF entity (TABLE29) and the Student
               domain model (AbsenceApp.Core.Models.Student).
               Resolves the naming collision between the two Student classes
               using explicit type aliases.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-15  Initial creation.
-------------------------------------------------------------------------------
 Notes       :
   - Uses CoreStudent / DataStudent aliases to avoid CS0104 ambiguity.
   - YearGroup on the domain model carries the YearGroupId string for now;
===============================================================================
*/

using AbsenceApp.Core.Models;
using Microsoft.EntityFrameworkCore;

using CoreStudent  = AbsenceApp.Core.Models.Student;
using DataStudent  = AbsenceApp.Data.Models.Student;

namespace AbsenceApp.Data.Mappers;

// =========================================================================
// StudentMapper -- Data.Models.Student <-> Core.Models.Student mapping
// =========================================================================

/// <summary>
/// Maps between the Student EF entity (AbsenceApp.Data.Models.Student) and
/// the Student domain model (AbsenceApp.Core.Models.Student).
/// Updated to reflect the TABLE29 schema — students are no longer stored in the
/// users table.
/// </summary>
public static class StudentMapper
{
    /// <summary>
    /// Projects a <see cref="DataStudent"/> EF entity to the lightweight
    /// <see cref="CoreStudent"/> domain model used by the client layer.
    /// YearGroup is set to the YearGroupId string representation; callers can
    /// enrich this with a resolved name if required.
    /// </summary>
    // =========================================================================
    // Data entity to domain model
    // =========================================================================

    public static CoreStudent ToDomain(DataStudent entity) => new()
    {
        Id        = entity.Id.ToString(),
        FirstName = entity.FirstName,
        LastName  = entity.LastName,
        YearGroup = entity.YearGroupId.ToString(),
    };

    /// <summary>
    /// Projects a <see cref="CoreStudent"/> domain model to a minimal
    /// <see cref="DataStudent"/> EF entity suitable for in-memory or test use.
    /// </summary>
    // =========================================================================
    // Domain model to data entity
    // =========================================================================

    public static DataStudent ToEntity(CoreStudent domain) => new()
    {
        FirstName      = domain.FirstName,
        LastName       = domain.LastName,
        LegalFirstName = domain.FirstName,
        LegalLastName  = domain.LastName,
        Gender         = string.Empty,
        AdmissionNumber = string.Empty,
        Status         = "Active",
        CreatedAt      = DateTime.UtcNow,
        UpdatedAt      = DateTime.UtcNow,
    };
}
