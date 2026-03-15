/*
===============================================================================
 File        : AuditLogService.cs
 Namespace   : AbsenceApp.Data.Services
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : Implements IAuditLogService using IAuditLogRepository.
               Provides append-only audit trail management with user-scoped
               lookup and IQueryable-based filtering.
-------------------------------------------------------------------------------
 Description :
   Business rules enforced:
     - Audit logs are append-only. No UpdateAsync or DeleteAsync is exposed.
     - The 'action' argument must be a non-empty string; ArgumentException is
       thrown if violated.
     - Timestamps are set to DateTime.UtcNow at the point of logging.
   GetByUserAsync uses IQueryable<AuditLog> to push the WHERE clause to SQL
   rather than loading all records into memory before filtering.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial creation.
-------------------------------------------------------------------------------
 Notes       :
   - Does not reference AppDbContext directly; all data access is via the
     injected IAuditLogRepository.
   - AuditLogMapper converts between AuditLog entities and AuditLogDto.
===============================================================================
*/

using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;
using AbsenceApp.Data.Mappers;
using AbsenceApp.Data.Models;
using AbsenceApp.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AbsenceApp.Data.Services;

public class AuditLogService : IAuditLogService
{
    // =========================================================================
    // Dependencies — constructor-injected repository
    // =========================================================================

    private readonly IAuditLogRepository _repository;

    public AuditLogService(IAuditLogRepository repository) => _repository = repository;

    // =========================================================================
    // IAuditLogService implementation — read operations
    // =========================================================================

    public async Task<IEnumerable<AuditLogDto>> GetAllAsync()
    {
        var entities = await _repository.ListAsync();
        return entities.Select(AuditLogMapper.ToDto);
    }

    public async Task<AuditLogDto?> GetByIdAsync(int id)
    {
        var entity = await _repository.FindByIdAsync(id);
        return entity is null ? null : AuditLogMapper.ToDto(entity);
    }

    public async Task<IEnumerable<AuditLogDto>> GetByUserAsync(int userId)
    {
        var entries = await _repository.Query()
            .Where(a => a.UserId == userId)
            .ToListAsync();
        return entries.Select(AuditLogMapper.ToDto);
    }

    // =========================================================================
    // IAuditLogService implementation — append-only write operation
    // =========================================================================

    // Business rule: audit logs are append-only.
    // No UpdateAsync or DeleteAsync is exposed on this service.
    public async Task<AuditLogDto> LogAsync(int userId, string action)
    {
        if (string.IsNullOrWhiteSpace(action))
            throw new ArgumentException("Action is required.", nameof(action));

        var entity = new AuditLog
        {
            UserId    = userId,
            Action    = action,
            Timestamp = DateTime.UtcNow,
        };
        await _repository.AddAsync(entity);
        return AuditLogMapper.ToDto(entity);
    }
}
