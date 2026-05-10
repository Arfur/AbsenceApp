/*
===============================================================================
 File        : StaffProfileApiServiceV2.cs
 Namespace   : AbsenceApp.Client.Services.ApiV2.Modules
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-05-09
 Updated     : 2026-05-09
-------------------------------------------------------------------------------
 Purpose     : Client-side Staff Profile API service. Wraps the data layer
               for the Staff Profile page. Uses IServiceScopeFactory
               (MAUI Blazor Hybrid — direct DB access, no HTTP layer).
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-05-09  Initial creation (Phase 5).
===============================================================================
*/

using AbsenceApp.Client.Shared;
using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;
using AbsenceApp.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AbsenceApp.Client.Services.ApiV2.Modules;

public sealed class StaffProfileApiServiceV2
{
    private readonly IServiceScopeFactory _scopeFactory;

    public StaffProfileApiServiceV2(IServiceScopeFactory scopeFactory)
        => _scopeFactory = scopeFactory;

    // =========================================================================
    // Staff — profile data
    // =========================================================================

    public async Task<StaffFullViewDto?> GetStaffAsync(long id, CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var svc  = scope.ServiceProvider.GetRequiredService<IStaffFullViewService>();
            var all  = await svc.GetAllAsync();
            return all.FirstOrDefault(s => s.Id == (int)id);
        }
        catch (Exception ex)
        {
            AppLog.Write("StaffProfileApiServiceV2.cs", "GetStaffAsync", $"ERROR {ex.Message}");
            return null;
        }
    }

    /// <summary>Returns the raw Staff record including FK IDs for editing.</summary>
    public async Task<StaffDto?> GetStaffRawAsync(long id, CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var e = await db.Staff.AsNoTracking()
                            .FirstOrDefaultAsync(s => s.Id == (int)id, ct);
            if (e is null) return null;
            return new StaffDto
            {
                Id             = e.Id,
                StaffNumber    = e.StaffNumber,
                FirstName      = e.FirstName,
                LastName       = e.LastName,
                PreferredName  = e.PreferredName,
                Title          = e.Title,
                DateOfBirth    = e.DateOfBirth,
                Gender         = e.Gender,
                WorkEmail      = e.WorkEmail,
                AltEmail       = e.AltEmail,
                PhoneHome      = e.PhoneHome,
                PhoneMobile    = e.PhoneMobile,
                PhoneEmergency = e.PhoneEmergency,
                EmploymentType = e.EmploymentType,
                ContractType   = e.ContractType,
                HireDate       = e.HireDate,
                EndDate        = e.EndDate,
                WorkLocation   = e.WorkLocation,
                JobTitleId     = e.JobTitleId,
                JobGroupId     = e.JobGroupId,
                DepartmentId   = e.DepartmentId,
                ReportingManagerId = e.ReportingManagerId,
                ProfilePhotoUrl = e.ProfilePhotoUrl,
                AccountStatus  = e.AccountStatus,
                CreatedAt      = e.CreatedAt,
                UpdatedAt      = e.UpdatedAt,
            };
        }
        catch (Exception ex)
        {
            AppLog.Write("StaffProfileApiServiceV2.cs", "GetStaffRawAsync", $"ERROR {ex.Message}");
            return null;
        }
    }

    // =========================================================================
    // Staff — create / update
    // =========================================================================

    public async Task<(bool Success, string? Error, int NewId)> CreateStaffAsync(
        StaffDto dto, CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var entity = new Data.Models.Staff
            {
                StaffNumber    = dto.StaffNumber,
                FirstName      = dto.FirstName,
                LastName       = dto.LastName,
                PreferredName  = dto.PreferredName,
                Title          = dto.Title,
                DateOfBirth    = dto.DateOfBirth,
                Gender         = dto.Gender,
                WorkEmail      = dto.WorkEmail,
                AltEmail       = dto.AltEmail,
                PhoneHome      = dto.PhoneHome,
                PhoneMobile    = dto.PhoneMobile,
                PhoneEmergency = dto.PhoneEmergency,
                EmploymentType = dto.EmploymentType,
                ContractType   = dto.ContractType,
                HireDate       = dto.HireDate,
                EndDate        = dto.EndDate,
                WorkLocation   = dto.WorkLocation,
                JobTitleId     = dto.JobTitleId,
                JobGroupId     = dto.JobGroupId,
                DepartmentId   = dto.DepartmentId,
                ReportingManagerId = dto.ReportingManagerId,
                ProfilePhotoUrl = dto.ProfilePhotoUrl,
                AccountStatus  = dto.AccountStatus,
                CreatedAt      = DateTime.UtcNow,
                UpdatedAt      = DateTime.UtcNow,
            };
            db.Staff.Add(entity);
            await db.SaveChangesAsync(ct);
            return (true, null, entity.Id);
        }
        catch (Exception ex)
        {
            AppLog.Write("StaffProfileApiServiceV2.cs", "CreateStaffAsync", $"ERROR {ex.Message}");
            return (false, ex.Message, 0);
        }
    }

    public async Task<(bool Success, string? Error)> UpdateStaffAsync(
        long id, StaffDto dto, CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var entity = await db.Staff.FindAsync([(int)id], ct);
            if (entity is null)
                return (false, "Staff member not found.");

            entity.StaffNumber    = dto.StaffNumber;
            entity.FirstName      = dto.FirstName;
            entity.LastName       = dto.LastName;
            entity.PreferredName  = dto.PreferredName;
            entity.Title          = dto.Title;
            entity.DateOfBirth    = dto.DateOfBirth;
            entity.Gender         = dto.Gender;
            entity.WorkEmail      = dto.WorkEmail;
            entity.AltEmail       = dto.AltEmail;
            entity.PhoneHome      = dto.PhoneHome;
            entity.PhoneMobile    = dto.PhoneMobile;
            entity.PhoneEmergency = dto.PhoneEmergency;
            entity.EmploymentType = dto.EmploymentType;
            entity.ContractType   = dto.ContractType;
            entity.HireDate       = dto.HireDate;
            entity.EndDate        = dto.EndDate;
            entity.WorkLocation   = dto.WorkLocation;
            entity.JobTitleId     = dto.JobTitleId;
            entity.JobGroupId     = dto.JobGroupId;
            entity.DepartmentId   = dto.DepartmentId;
            entity.ReportingManagerId = dto.ReportingManagerId;
            entity.ProfilePhotoUrl = dto.ProfilePhotoUrl;
            entity.AccountStatus  = dto.AccountStatus;
            entity.UpdatedAt      = DateTime.UtcNow;

            await db.SaveChangesAsync(ct);
            return (true, null);
        }
        catch (Exception ex)
        {
            AppLog.Write("StaffProfileApiServiceV2.cs", "UpdateStaffAsync", $"ERROR {ex.Message}");
            return (false, ex.Message);
        }
    }

    // =========================================================================
    // Absences
    // =========================================================================

    public async Task<IEnumerable<AbsenceDto>> GetAbsencesAsync(long staffId, CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var svc = scope.ServiceProvider.GetRequiredService<IAbsenceService>();
            return await svc.GetByPersonAsync("Staff", (int)staffId);
        }
        catch (Exception ex)
        {
            AppLog.Write("StaffProfileApiServiceV2.cs", "GetAbsencesAsync", $"ERROR {ex.Message}");
            return [];
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
            AppLog.Write("StaffProfileApiServiceV2.cs", "DeleteAbsenceAsync", $"ERROR {ex.Message}");
            return (false, ex.Message);
        }
    }

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
            AppLog.Write("StaffProfileApiServiceV2.cs", "GetAbsenceTypesAsync", $"ERROR {ex.Message}");
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
            AppLog.Write("StaffProfileApiServiceV2.cs", "GetAbsenceStatusesAsync", $"ERROR {ex.Message}");
            return [];
        }
    }

    // =========================================================================
    // Reference data
    // =========================================================================

    public async Task<IEnumerable<DepartmentDto>> GetDepartmentsAsync(CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var svc = scope.ServiceProvider.GetRequiredService<IDepartmentService>();
            return await svc.GetAllAsync();
        }
        catch (Exception ex)
        {
            AppLog.Write("StaffProfileApiServiceV2.cs", "GetDepartmentsAsync", $"ERROR {ex.Message}");
            return [];
        }
    }

    public async Task<IEnumerable<JobTitleDto>> GetJobTitlesAsync(CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var svc = scope.ServiceProvider.GetRequiredService<IJobTitleService>();
            return await svc.GetAllAsync();
        }
        catch (Exception ex)
        {
            AppLog.Write("StaffProfileApiServiceV2.cs", "GetJobTitlesAsync", $"ERROR {ex.Message}");
            return [];
        }
    }

    // =========================================================================
    // Staff Notes — CRUD
    // =========================================================================

    public async Task<IReadOnlyList<StaffNoteDto>> GetNotesAsync(long staffId, CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            return await db.StaffNotes
                .AsNoTracking()
                .Where(n => n.StaffId == staffId)
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => new StaffNoteDto
                {
                    Id = n.Id,
                    StaffId = n.StaffId,
                    NoteType = n.NoteType,
                    Body = n.Body,
                    CreatedBy = n.CreatedBy,
                    CreatedAt = n.CreatedAt,
                    UpdatedAt = n.UpdatedAt,
                })
                .ToListAsync(ct);
        }
        catch (Exception ex)
        {
            AppLog.Write("StaffProfileApiServiceV2.cs", "GetNotesAsync", $"ERROR {ex.Message}");
            return [];
        }
    }

    public async Task<(bool Success, string? Error, long NewId)> CreateNoteAsync(StaffNoteDto dto, CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var row = new Data.Models.StaffNote
            {
                StaffId = dto.StaffId,
                NoteType = string.IsNullOrWhiteSpace(dto.NoteType) ? "General" : dto.NoteType,
                Body = dto.Body,
                CreatedBy = dto.CreatedBy,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            db.StaffNotes.Add(row);
            await db.SaveChangesAsync(ct);
            return (true, null, row.Id);
        }
        catch (Exception ex)
        {
            AppLog.Write("StaffProfileApiServiceV2.cs", "CreateNoteAsync", $"ERROR {ex.Message}");
            return (false, ex.Message, 0);
        }
    }

    public async Task<(bool Success, string? Error)> DeleteNoteAsync(long noteId, CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var row = await db.StaffNotes.FindAsync([noteId], ct);
            if (row is null) return (false, "Staff note not found.");
            db.StaffNotes.Remove(row);
            await db.SaveChangesAsync(ct);
            return (true, null);
        }
        catch (Exception ex)
        {
            AppLog.Write("StaffProfileApiServiceV2.cs", "DeleteNoteAsync", $"ERROR {ex.Message}");
            return (false, ex.Message);
        }
    }
}
