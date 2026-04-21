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

        entity.StatusId  = dto.NewStatusId;
        entity.UpdatedAt = DateTime.UtcNow;

        if (dto.NewStatusId == entity.StatusId) return;

        // Approval metadata
        var newStatus = await _statusRepo.GetByIdAsync(dto.NewStatusId);
        if (newStatus?.Code == "APPROVED")
        {
            entity.ApprovedBy = dto.ChangedBy;
            entity.ApprovedAt = DateTime.UtcNow;
        }

        await _repo.SaveChangesAsync();
    }
}
