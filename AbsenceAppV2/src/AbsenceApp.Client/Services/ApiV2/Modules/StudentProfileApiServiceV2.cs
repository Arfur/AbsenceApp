/*
===============================================================================
 File        : StudentProfileApiServiceV2.cs
 Namespace   : AbsenceApp.Client.Services.ApiV2.Modules
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-05-05
 Updated     : 2026-05-05
-------------------------------------------------------------------------------
 Purpose     : Client-side Student Profile API service. Wraps the data layer
               for the Student Profile, Absence Management, and Calendar pages.
               Uses IServiceScopeFactory (MAUI Blazor Hybrid — direct DB access,
               no HTTP layer).
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-05-05  Initial creation.
===============================================================================
*/

using AbsenceApp.Client.Shared;
using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;
using AbsenceApp.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace AbsenceApp.Client.Services.ApiV2.Modules;

public sealed class StudentProfileApiServiceV2
{
    private readonly IServiceScopeFactory _scopeFactory;

    public StudentProfileApiServiceV2(IServiceScopeFactory scopeFactory)
        => _scopeFactory = scopeFactory;

    // =========================================================================
    // Student — profile data
    // =========================================================================

    public async Task<StudentFullViewDto?> GetStudentAsync(int studentId, CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var svc = scope.ServiceProvider.GetRequiredService<IStudentFullViewService>();
            return await svc.GetByIdAsync(studentId, ct);
        }
        catch (Exception ex)
        {
            AppLog.Write("StudentProfileApiServiceV2.cs", "GetStudentAsync", $"ERROR {ex.Message}");
            return null;
        }
    }

    public async Task<IEnumerable<StudentContactDto>> GetContactsAsync(int studentId, CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var svc = scope.ServiceProvider.GetRequiredService<IStudentContactService>();
            return await svc.GetByStudentAsync(studentId);
        }
        catch (Exception ex)
        {
            AppLog.Write("StudentProfileApiServiceV2.cs", "GetContactsAsync", $"ERROR {ex.Message}");
            return [];
        }
    }

    public async Task<IEnumerable<StudentMedicalDto>> GetMedicalAsync(int studentId, CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var repo    = scope.ServiceProvider.GetRequiredService<IStudentMedicalRepository>();
            var records = await repo.GetByStudentAsync(studentId, ct);
            return records.Select(m => new StudentMedicalDto
            {
                Id                  = m.Id,
                StudentId           = m.StudentId,
                MedicalCondition    = m.MedicalCondition,
                IsAllergic          = m.IsAllergic,
                AllergyDetails      = m.AllergyDetails,
                Medication          = m.Medication,
                EmergencyActionPlan = m.EmergencyActionPlan,
                RecordedBy          = m.RecordedBy,
                CreatedAt           = m.CreatedAt,
                UpdatedAt           = m.UpdatedAt,
            });
        }
        catch (Exception ex)
        {
            AppLog.Write("StudentProfileApiServiceV2.cs", "GetMedicalAsync", $"ERROR {ex.Message}");
            return [];
        }
    }

    public async Task<IEnumerable<StudentFlagDto>> GetFlagsAsync(int studentId, CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var repo    = scope.ServiceProvider.GetRequiredService<IStudentFlagRepository>();
            var records = await repo.GetByStudentAsync(studentId, ct);
            return records.Select(f => new StudentFlagDto
            {
                Id         = f.Id,
                StudentId  = f.StudentId,
                FlagCode   = f.FlagCode,
                IsActive   = f.IsActive,
                Notes      = f.Notes,
                AssignedAt = f.AssignedAt,
                AssignedBy = f.AssignedBy,
                CreatedAt  = f.CreatedAt,
                UpdatedAt  = f.UpdatedAt,
            });
        }
        catch (Exception ex)
        {
            AppLog.Write("StudentProfileApiServiceV2.cs", "GetFlagsAsync", $"ERROR {ex.Message}");
            return [];
        }
    }

    // =========================================================================
    // Absences — CRUD
    // =========================================================================

    public async Task<IEnumerable<AbsenceDto>> GetAbsencesAsync(int studentId, CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var svc = scope.ServiceProvider.GetRequiredService<IAbsenceService>();
            return await svc.GetByPersonAsync("Student", studentId);
        }
        catch (Exception ex)
        {
            AppLog.Write("StudentProfileApiServiceV2.cs", "GetAbsencesAsync", $"ERROR {ex.Message}");
            return [];
        }
    }

    public async Task<(bool Success, string? Error, AbsenceDto? Result)> CreateAbsenceAsync(
        CreateAbsenceDto dto, CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var svc    = scope.ServiceProvider.GetRequiredService<IAbsenceService>();
            var result = await svc.CreateAsync(dto);
            return (true, null, result);
        }
        catch (Exception ex)
        {
            AppLog.Write("StudentProfileApiServiceV2.cs", "CreateAbsenceAsync", $"ERROR {ex.Message}");
            return (false, ex.Message, null);
        }
    }

    public async Task<(bool Success, string? Error)> UpdateAbsenceAsync(
        long id, UpdateAbsenceDto dto, CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var svc = scope.ServiceProvider.GetRequiredService<IAbsenceService>();
            await svc.UpdateAsync(id, dto);
            return (true, null);
        }
        catch (Exception ex)
        {
            AppLog.Write("StudentProfileApiServiceV2.cs", "UpdateAbsenceAsync", $"ERROR {ex.Message}");
            return (false, ex.Message);
        }
    }

    public async Task<(bool Success, string? Error)> DeleteAbsenceAsync(long id, CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var svc = scope.ServiceProvider.GetRequiredService<IAbsenceService>();
            await svc.DeleteAsync(id);
            return (true, null);
        }
        catch (Exception ex)
        {
            AppLog.Write("StudentProfileApiServiceV2.cs", "DeleteAbsenceAsync", $"ERROR {ex.Message}");
            return (false, ex.Message);
        }
    }

    // =========================================================================
    // Reference data
    // =========================================================================

    public async Task<IEnumerable<AbsenceTypeDto>> GetAbsenceTypesAsync(CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var svc = scope.ServiceProvider.GetRequiredService<IAbsenceTypeService>();
            return await svc.GetAllAsync();
        }
        catch (Exception ex)
        {
            AppLog.Write("StudentProfileApiServiceV2.cs", "GetAbsenceTypesAsync", $"ERROR {ex.Message}");
            return [];
        }
    }

    public async Task<IEnumerable<AbsenceStatusDto>> GetAbsenceStatusesAsync(CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var svc = scope.ServiceProvider.GetRequiredService<IAbsenceStatusService>();
            return await svc.GetAllAsync();
        }
        catch (Exception ex)
        {
            AppLog.Write("StudentProfileApiServiceV2.cs", "GetAbsenceStatusesAsync", $"ERROR {ex.Message}");
            return [];
        }
    }

    public async Task<IEnumerable<YearGroupDto>> GetYearGroupsAsync(CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var svc = scope.ServiceProvider.GetRequiredService<IYearGroupService>();
            return await svc.GetAllAsync();
        }
        catch (Exception ex)
        {
            AppLog.Write("StudentProfileApiServiceV2.cs", "GetYearGroupsAsync", $"ERROR {ex.Message}");
            return [];
        }
    }

    public async Task<IEnumerable<HouseDto>> GetHousesAsync(CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var svc = scope.ServiceProvider.GetRequiredService<IHouseService>();
            return await svc.GetAllAsync();
        }
        catch (Exception ex)
        {
            AppLog.Write("StudentProfileApiServiceV2.cs", "GetHousesAsync", $"ERROR {ex.Message}");
            return [];
        }
    }

    public async Task<IEnumerable<ClassDto>> GetClassesAsync(CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var svc = scope.ServiceProvider.GetRequiredService<IClassService>();
            return await svc.GetAllAsync();
        }
        catch (Exception ex)
        {
            AppLog.Write("StudentProfileApiServiceV2.cs", "GetClassesAsync", $"ERROR {ex.Message}");
            return [];
        }
    }
}
