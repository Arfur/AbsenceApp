/*
===============================================================================
 File        : ISubjectService.cs
 Namespace   : AbsenceApp.Core.Interfaces
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-17
 Updated     : 2026-03-17
-------------------------------------------------------------------------------
 Purpose     : Defines the async CRUD contract for subject operations.
               Implemented by SubjectService in AbsenceApp.Data.Services.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-17  Initial interface defined.
===============================================================================
*/

using AbsenceApp.Core.DTOs;

namespace AbsenceApp.Core.Interfaces;

public interface ISubjectService
{
    Task<IEnumerable<SubjectDto>> GetAllAsync();
    Task<SubjectDto?> GetByIdAsync(int id);
    Task<SubjectDto> CreateAsync(SubjectDto dto);
    Task UpdateAsync(SubjectDto dto);
    Task DeleteAsync(int id);
}
