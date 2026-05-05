using AbsenceApp.Core.DTOs;

namespace AbsenceApp.Core.Interfaces;

public interface IAbsenceService
{
    Task<IEnumerable<AbsenceDto>> GetByPersonAsync(string personType, long personId);
    Task<AbsenceDto?>             GetByIdAsync(long id);
    Task<AbsenceDto>              CreateAsync(CreateAbsenceDto dto);
    Task                          UpdateStatusAsync(long absenceId, UpdateAbsenceStatusDto dto);
    Task                          UpdateAsync(long id, UpdateAbsenceDto dto);
    Task                          DeleteAsync(long id);
}
