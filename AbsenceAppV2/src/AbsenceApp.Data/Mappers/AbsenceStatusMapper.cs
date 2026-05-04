using AbsenceApp.Core.DTOs;
using AbsenceApp.Data.Models;

namespace AbsenceApp.Data.Mappers;

public static class AbsenceStatusMapper
{
    public static AbsenceStatusDto ToDto(AbsenceStatus e) => new()
    {
        Id        = (long)e.Id,
        Code      = e.Code,
        Name      = e.Name,
        IsFinal   = e.IsFinal,
        CreatedAt = e.CreatedAt
    };
}
