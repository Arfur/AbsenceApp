/*
===============================================================================
 File        : ClassService.cs
 Namespace   : AbsenceApp.Data.Services
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-15
 Updated     : 2026-03-15
-------------------------------------------------------------------------------
 Purpose     : EF Core implementation of IClassService.
               Provides full CRUD operations for Class entities, mapping between
               the Class EF entity and ClassDto via ClassMapper.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-15  Initial creation.
-------------------------------------------------------------------------------
 Notes       :
   - Each mutating operation delegates SaveChanges to the repository.
   - ClassName validation throws ArgumentException on blank value.
===============================================================================
*/

using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;
using AbsenceApp.Data.Mappers;
using AbsenceApp.Data.Repositories;

namespace AbsenceApp.Data.Services;

public class ClassService : IClassService
{
    // =========================================================================
    // Dependencies -- constructor-injected repository
    // =========================================================================

    private readonly IClassRepository _repository;

    public ClassService(IClassRepository repository) => _repository = repository;

    // =========================================================================
    // IClassService implementation
    // =========================================================================

    public async Task<IEnumerable<ClassDto>> GetAllAsync()
    {
        var entities = await _repository.ListAsync();
        return entities.Select(ClassMapper.ToDto);
    }

    public async Task<ClassDto?> GetByIdAsync(int id)
    {
        var entity = await _repository.FindByIdAsync(id);
        return entity is null ? null : ClassMapper.ToDto(entity);
    }

    public async Task<ClassDto> CreateAsync(ClassDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.ClassName))
            throw new ArgumentException("ClassName is required.", nameof(dto));

        var entity = ClassMapper.ToEntity(dto);
        entity.Id = 0;
        await _repository.AddAsync(entity);
        return ClassMapper.ToDto(entity);
    }

    public async Task UpdateAsync(ClassDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.ClassName))
            throw new ArgumentException("ClassName is required.", nameof(dto));

        var entity = ClassMapper.ToEntity(dto);
        await _repository.UpdateAsync(entity);
    }

    public async Task DeleteAsync(int id) => await _repository.DeleteAsync(id);
}
