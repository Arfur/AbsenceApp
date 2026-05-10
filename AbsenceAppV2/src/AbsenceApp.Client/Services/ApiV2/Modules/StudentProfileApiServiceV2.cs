/*
===============================================================================
 File        : StudentProfileApiServiceV2.cs
 Namespace   : AbsenceApp.Client.Services.ApiV2.Modules
 Author      : Michael
 Version     : 1.1.0
 Created     : 2026-05-05
 Updated     : 2026-05-10
-------------------------------------------------------------------------------
 Purpose     : Client-side Student Profile API service. Wraps the data layer
               for the Student Profile, Absence Management, and Calendar pages.
               Uses IServiceScopeFactory (MAUI Blazor Hybrid — direct DB access,
               no HTTP layer).
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-05-05  Initial creation.
   - 1.1.0  2026-05-10  Added SearchStudentProfileSelectorAsync() for the shared
                         profile-name selector.
===============================================================================
*/

using AbsenceApp.Client.Shared;
using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;
using AbsenceApp.Data.Context;
using AbsenceApp.Data.Repositories;
using Microsoft.EntityFrameworkCore;
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

    public async Task<IReadOnlyList<ProfileNameSelectorItemDto>> SearchStudentProfileSelectorAsync(
        string? term,
        int maxResults = 12,
        CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var svc = scope.ServiceProvider.GetRequiredService<IStudentFullViewService>();
            var students = await svc.GetAllAsync();
            var query = students.AsEnumerable();
            var search = term?.Trim();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(s =>
                    s.FullName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    (s.AdmissionNumber ?? string.Empty).Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    (s.YearGroupName ?? string.Empty).Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    (s.ClassName ?? string.Empty).Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            return query
                .OrderBy(s => s.FullName)
                .Take(maxResults)
                .Select(s => new ProfileNameSelectorItemDto
                {
                    Id = s.Id,
                    DisplayName = s.FullName,
                    SecondaryText = string.Join(" • ", new[]
                    {
                        string.IsNullOrWhiteSpace(s.AdmissionNumber) ? null : s.AdmissionNumber,
                        string.IsNullOrWhiteSpace(s.YearGroupName) ? null : s.YearGroupName,
                        string.IsNullOrWhiteSpace(s.ClassName) ? null : s.ClassName,
                    }.Where(value => !string.IsNullOrWhiteSpace(value))),
                    Route = $"/v2/students/{s.Id}",
                    EntityType = "Student",
                    Status = s.Status,
                })
                .ToList()
                .AsReadOnly();
        }
        catch (Exception ex)
        {
            AppLog.Write("StudentProfileApiServiceV2.cs", "SearchStudentProfileSelectorAsync", $"ERROR {ex.Message}");
            return [];
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

    // =========================================================================
    // Student — create / update
    // =========================================================================

    public async Task<(bool Success, string? Error, int NewId)> CreateStudentAsync(
        StudentDto dto, CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var entity = new Data.Models.Student
            {
                AdmissionNumber = dto.AdmissionNumber,
                FirstName       = dto.FirstName,
                MiddleNames     = dto.MiddleNames,
                LastName        = dto.LastName,
                LegalFirstName  = dto.LegalFirstName,
                LegalLastName   = dto.LegalLastName,
                PreferredName   = dto.PreferredName,
                Gender          = dto.Gender,
                DateOfBirth     = dto.DateOfBirth,
                YearGroupId     = dto.YearGroupId,
                ClassId         = dto.ClassId,
                HouseId         = dto.HouseId,
                Username        = dto.Username,
                Upn             = dto.Upn,
                SchoolId        = dto.SchoolId,
                AdmissionDate   = dto.AdmissionDate,
                Status          = dto.Status,
                CreatedAt       = DateTime.UtcNow,
                UpdatedAt       = DateTime.UtcNow,
            };
            db.Students.Add(entity);
            await db.SaveChangesAsync(ct);
            return (true, null, entity.Id);
        }
        catch (Exception ex)
        {
            AppLog.Write("StudentProfileApiServiceV2.cs", "CreateStudentAsync", $"ERROR {ex.Message}");
            return (false, ex.Message, 0);
        }
    }

    public async Task<(bool Success, string? Error)> UpdateStudentAsync(
        int id, StudentDto dto, CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var entity = await db.Students.FindAsync([id], ct);
            if (entity is null)
                return (false, "Student not found.");

            entity.AdmissionNumber = dto.AdmissionNumber;
            entity.FirstName       = dto.FirstName;
            entity.MiddleNames     = dto.MiddleNames;
            entity.LastName        = dto.LastName;
            entity.LegalFirstName  = dto.LegalFirstName;
            entity.LegalLastName   = dto.LegalLastName;
            entity.PreferredName   = dto.PreferredName;
            entity.Gender          = dto.Gender;
            entity.DateOfBirth     = dto.DateOfBirth;
            entity.YearGroupId     = dto.YearGroupId;
            entity.ClassId         = dto.ClassId;
            entity.HouseId         = dto.HouseId;
            entity.Username        = dto.Username;
            entity.Upn             = dto.Upn;
            entity.SchoolId        = dto.SchoolId;
            entity.AdmissionDate   = dto.AdmissionDate;
            entity.Status          = dto.Status;
            entity.UpdatedAt       = DateTime.UtcNow;

            await db.SaveChangesAsync(ct);
            return (true, null);
        }
        catch (Exception ex)
        {
            AppLog.Write("StudentProfileApiServiceV2.cs", "UpdateStudentAsync", $"ERROR {ex.Message}");
            return (false, ex.Message);
        }
    }

    public async Task<StudentDto?> GetStudentRawAsync(int studentId, CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var e = await db.Students.AsNoTracking()
                            .FirstOrDefaultAsync(s => s.Id == studentId, ct);
            if (e is null) return null;
            return new StudentDto
            {
                Id              = e.Id,
                AdmissionNumber = e.AdmissionNumber,
                FirstName       = e.FirstName,
                MiddleNames     = e.MiddleNames,
                LastName        = e.LastName,
                LegalFirstName  = e.LegalFirstName,
                LegalLastName   = e.LegalLastName,
                PreferredName   = e.PreferredName,
                Gender          = e.Gender,
                DateOfBirth     = e.DateOfBirth,
                YearGroupId     = e.YearGroupId,
                ClassId         = e.ClassId,
                HouseId         = e.HouseId,
                Username        = e.Username,
                Upn             = e.Upn,
                SchoolId        = e.SchoolId,
                AdmissionDate   = e.AdmissionDate,
                Status          = e.Status,
                CreatedAt       = e.CreatedAt,
                UpdatedAt       = e.UpdatedAt,
            };
        }
        catch (Exception ex)
        {
            AppLog.Write("StudentProfileApiServiceV2.cs", "GetStudentRawAsync", $"ERROR {ex.Message}");
            return null;
        }
    }

    // =========================================================================
    // Student Notes — CRUD
    // =========================================================================

    public async Task<IReadOnlyList<StudentNoteDto>> GetNotesAsync(int studentId, CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            return await db.StudentNotes
                .AsNoTracking()
                .Where(n => n.StudentId == studentId)
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => new StudentNoteDto
                {
                    Id = n.Id,
                    StudentId = n.StudentId,
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
            AppLog.Write("StudentProfileApiServiceV2.cs", "GetNotesAsync", $"ERROR {ex.Message}");
            return [];
        }
    }

    public async Task<(bool Success, string? Error, long NewId)> CreateNoteAsync(StudentNoteDto dto, CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var row = new Data.Models.StudentNote
            {
                StudentId = dto.StudentId,
                NoteType = string.IsNullOrWhiteSpace(dto.NoteType) ? "General" : dto.NoteType,
                Body = dto.Body,
                CreatedBy = dto.CreatedBy,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            db.StudentNotes.Add(row);
            await db.SaveChangesAsync(ct);
            return (true, null, row.Id);
        }
        catch (Exception ex)
        {
            AppLog.Write("StudentProfileApiServiceV2.cs", "CreateNoteAsync", $"ERROR {ex.Message}");
            return (false, ex.Message, 0);
        }
    }

    public async Task<(bool Success, string? Error)> DeleteNoteAsync(long noteId, CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var row = await db.StudentNotes.FindAsync([noteId], ct);
            if (row is null) return (false, "Student note not found.");
            db.StudentNotes.Remove(row);
            await db.SaveChangesAsync(ct);
            return (true, null);
        }
        catch (Exception ex)
        {
            AppLog.Write("StudentProfileApiServiceV2.cs", "DeleteNoteAsync", $"ERROR {ex.Message}");
            return (false, ex.Message);
        }
    }
}
