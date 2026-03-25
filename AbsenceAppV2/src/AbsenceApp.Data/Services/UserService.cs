/*
===============================================================================
 File        : UserService.cs
 Namespace   : AbsenceApp.Data.Services
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-14
 Updated     : 2026-03-14
-------------------------------------------------------------------------------
 Purpose     : Implements IUserService using IUserRepository.
               Provides read operations for user (staff) data, converting
               User EF entities to UserDto via UserMapper.
-------------------------------------------------------------------------------
 Description :
   Users represent staff members in the AbsenceApp system.  This service
   exposes only the read path consumed by the Staff UI page.
   UserDto intentionally excludes PasswordHash; see UserMapper for details.
   All entity-to-DTO conversion is delegated to UserMapper.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-14  Initial creation.
-------------------------------------------------------------------------------
 Notes       :
   - Does not reference AppDbContext directly; all data access is via the
     injected IUserRepository.
===============================================================================
*/

using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;
using AbsenceApp.Data.Mappers;
using AbsenceApp.Data.Repositories;

namespace AbsenceApp.Data.Services;

public class UserService : IUserService
{
    // =========================================================================
    // Dependencies — constructor-injected repository
    // =========================================================================

    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository) => _repository = repository;

    // =========================================================================
    // IUserService implementation — read operations
    // =========================================================================

    /// <inheritdoc/>
    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var entities = await _repository.ListAsync();
        return entities.Select(UserMapper.ToDto);
    }

    /// <inheritdoc/>
    public async Task<UserDto?> GetByIdAsync(int id)
    {
        var entity = await _repository.FindByIdAsync(id);
        return entity is null ? null : UserMapper.ToDto(entity);
    }
}
