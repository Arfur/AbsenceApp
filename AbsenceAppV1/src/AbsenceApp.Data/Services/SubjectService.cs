/*
===============================================================================
 File        : SubjectService.cs
 Namespace   : AbsenceApp.Data.Services
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-17
 Updated     : 2026-03-17
-------------------------------------------------------------------------------
 Purpose     : EF Core implementation of ISubjectService.
               Subjects are stored in the classes table; this service provides
               a clean domain boundary until a dedicated subjects table exists.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-17  Initial creation.
===============================================================================
*/

using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;
using AbsenceApp.Data.Mappers;
using AbsenceApp.Data.Repositories;

namespace AbsenceApp.Data.Services;

public class SubjectService : ISubjectService
{
    private readonly IClassRepository _repository;

    public SubjectService(IClassRepository repository) => _repository = repository;

    public async Task<IEnumerable<SubjectDto>> GetAllAsync()
    {
        var entities = await _repository.ListAsync();
        return entities.Select(SubjectMapper.ToDto);
    }

    public async Task<SubjectDto?> GetByIdAsync(int id)
    {
        var entity = await _repository.FindByIdAsync(id);
        return entity is null ? null : SubjectMapper.ToDto(entity);
    }

    public async Task<SubjectDto> CreateAsync(SubjectDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ArgumentException("Name is required.", nameof(dto));

        var entity = SubjectMapper.ToEntity(dto);
        entity.Id = 0;
        await _repository.AddAsync(entity);
        return SubjectMapper.ToDto(entity);
    }

    public async Task UpdateAsync(SubjectDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ArgumentException("Name is required.", nameof(dto));

        var entity = SubjectMapper.ToEntity(dto);
        await _repository.UpdateAsync(entity);
    }

    public async Task DeleteAsync(int id) => await _repository.DeleteAsync(id);
}
