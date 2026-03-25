/*
===============================================================================
 File        : RoleService.cs
 Namespace   : AbsenceApp.Data.Services
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : Implements IRoleService using IRoleRepository.
               Provides full CRUD operations for system roles, with input
               validation enforced before persistence.
-------------------------------------------------------------------------------
 Description :
   Business rules enforced:
     - RoleName must be a non-null, non-whitespace string on Create/Update.
     - RoleId is reset to 0 on Create so the database auto-generates the key.
   All entity-to-DTO conversion is delegated to RoleMapper.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial creation.
-------------------------------------------------------------------------------
 Notes       :
   - Does not reference AppDbContext directly; all data access is via the
     injected IRoleRepository.
   - RoleMapper converts between Role entities and RoleDto.
===============================================================================
*/

using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;
using AbsenceApp.Data.Mappers;
using AbsenceApp.Data.Repositories;

namespace AbsenceApp.Data.Services;

public class RoleService : IRoleService
{
    // =========================================================================
    // Dependencies — constructor-injected repository
    // =========================================================================

    private readonly IRoleRepository _repository;

    public RoleService(IRoleRepository repository) => _repository = repository;

    // =========================================================================
    // IRoleService implementation — read operations
    // =========================================================================

    public async Task<IEnumerable<RoleDto>> GetAllAsync()
    {
        var entities = await _repository.ListAsync();
        return entities.Select(RoleMapper.ToDto);
    }

    public async Task<RoleDto?> GetByIdAsync(int id)
    {
        var entity = await _repository.FindByIdAsync(id);
        return entity is null ? null : RoleMapper.ToDto(entity);
    }

    // =========================================================================
    // IRoleService implementation — write operations
    // =========================================================================

    public async Task<RoleDto> CreateAsync(RoleDto dto)
    {
        // Business rule: role name is required and must be unique by convention.
        if (string.IsNullOrWhiteSpace(dto.RoleName))
            throw new ArgumentException("RoleName is required.", nameof(dto));

        var entity = RoleMapper.ToEntity(dto);
        entity.RoleId = 0; // let DB generate
        await _repository.AddAsync(entity);
        return RoleMapper.ToDto(entity);
    }

    public async Task UpdateAsync(RoleDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.RoleName))
            throw new ArgumentException("RoleName is required.", nameof(dto));

        await _repository.UpdateAsync(RoleMapper.ToEntity(dto));
    }

    public async Task DeleteAsync(int id) => await _repository.DeleteAsync(id);
}
