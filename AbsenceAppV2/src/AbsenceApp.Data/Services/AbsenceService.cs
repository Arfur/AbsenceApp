/*
===============================================================================
 File        : AbsenceService.cs
 Namespace   : AbsenceApp.Data.Services
 Author      : Michael
 Version     : 1.2.0
 Created     : 2026-05-04
 Updated     : 2026-05-05
-------------------------------------------------------------------------------
 Purpose     : Application service implementing IAbsenceService. Handles
               CRUD operations and status transitions for Absence records.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-05-04  Initial header added (Phase 3.11). Fixed
                         UpdateStatusAsync logic bug: guard was placed AFTER
                         the assignment it guarded, making the guard always true
                         and preventing SaveChangesAsync from ever being called.
                         Moved guard before assignment; removed erroneous
                         (ulong) and (ulong?) casts now that Absence model
                         uses long.
   - 1.1.0  2026-05-04  Fix Plan #2 Step 4: removed three (ulong) casts from
                         GetByPersonAsync, GetByIdAsync, and UpdateStatusAsync
                         calls to _repo.GetByPersonAsync / _repo.GetByIdAsync.
                         IAbsenceRepository parameters are now long (Step 3);
                         the casts produced ulong arguments and caused CS1503.
   - 1.2.0  2026-05-05  Student Absence Management: added UpdateAsync and
                         DeleteAsync implementations.
===============================================================================
*/
using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;
using AbsenceApp.Data.Mappers;
using AbsenceApp.Data.Models;
using AbsenceApp.Data.Repositories;

namespace AbsenceApp.Data.Services;

public class AbsenceService : IAbsenceService
{
    private readonly IAbsenceRepository       _repo;
    private readonly IAbsenceStatusRepository _statusRepo;

    public AbsenceService(IAbsenceRepository repo, IAbsenceStatusRepository statusRepo)
    {
        _repo       = repo;
        _statusRepo = statusRepo;
    }

    public async Task<IEnumerable<AbsenceDto>> GetByPersonAsync(string personType, long personId)
    {
        var entities = await _repo.GetByPersonAsync(personType, personId);
        return entities.Select(AbsenceMapper.ToDto);
    }

    public async Task<AbsenceDto?> GetByIdAsync(long id)
    {
        var entity = await _repo.GetByIdAsync(id);
        return entity is null ? null : AbsenceMapper.ToDto(entity);
    }

    public async Task<AbsenceDto> CreateAsync(CreateAbsenceDto dto)
    {
        // Resolve PENDING status ID
        var statuses  = await _statusRepo.GetAllAsync();
        var pending   = statuses.FirstOrDefault(s => s.Code == "PENDING")
                        ?? statuses.First();

        var entity  = AbsenceMapper.ToEntity(dto, pending.Id);
        var created = await _repo.AddAsync(entity);
        return AbsenceMapper.ToDto(created);
    }

    public async Task UpdateStatusAsync(long absenceId, UpdateAbsenceStatusDto dto)
    {
        var entity = await _repo.GetByIdAsync(absenceId)
                     ?? throw new KeyNotFoundException($"Absence {absenceId} not found.");

        if (dto.NewStatusId == entity.StatusId) return;

        entity.StatusId  = dto.NewStatusId;
        entity.UpdatedAt = DateTime.UtcNow;

        // Approval metadata
        var newStatus = await _statusRepo.GetByIdAsync(dto.NewStatusId);
        if (newStatus?.Code == "APPROVED")
        {
            entity.ApprovedBy = dto.ChangedBy;
            entity.ApprovedAt = DateTime.UtcNow;
        }

        await _repo.SaveChangesAsync();
    }

    public async Task UpdateAsync(long id, UpdateAbsenceDto dto)
    {
        var entity = await _repo.GetByIdAsync(id)
                     ?? throw new KeyNotFoundException($"Absence {id} not found.");

        entity.AbsenceTypeId = dto.AbsenceTypeId;
        entity.StatusId      = dto.StatusId;
        entity.StartDate     = dto.StartDate;
        entity.EndDate       = dto.EndDate;
        entity.ReportedVia   = dto.ReportedVia;
        entity.Notes         = dto.Notes;
        entity.ApprovedBy    = dto.ApprovedBy;
        entity.UpdatedAt     = DateTime.UtcNow;
        entity.DurationDays  = (int)(dto.EndDate.ToDateTime(TimeOnly.MinValue) -
                                     dto.StartDate.ToDateTime(TimeOnly.MinValue)).TotalDays + 1;

        await _repo.UpdateAsync(entity);
    }

    public async Task DeleteAsync(long id)
    {
        await _repo.DeleteAsync(id);
    }
}
