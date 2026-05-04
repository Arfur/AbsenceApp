/*
===============================================================================
 File        : DashboardService.cs
 Namespace   : AbsenceApp.Data.Services
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-05-04
 Updated     : 2026-05-04
-------------------------------------------------------------------------------
 Purpose     : Application service implementing IDashboardService.
               Provides aggregated overview and activity data for the dashboard.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-05-04  Fix Plan #2 Step 7: added file header. Fixed
                         studentIds.Contains((ulong)s.Id) →
                         studentIds.Contains((long)s.Id). studentIds is
                         List<long> (projected from Absence.PersonId which is
                         long); Student.Id is int; (long)s.Id produces long,
                         matching the Contains parameter type.
===============================================================================
*/
using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;
using AbsenceApp.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace AbsenceApp.Data.Services;

public class DashboardService : IDashboardService
{
    private readonly AppDbContext _context;

    public DashboardService(AppDbContext context) => _context = context;

    public async Task<DashboardOverviewDto> GetOverviewAsync()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        var totalStudents     = await _context.Students.CountAsync();
        var totalStaff        = await _context.Staff.CountAsync();
        var absencesToday     = await _context.Absences.CountAsync(a =>
            a.PersonType == "Student" && a.StartDate <= today && a.EndDate >= today);
        var unauthorisedToday = await _context.Absences.CountAsync(a =>
            a.PersonType == "Student" && a.StartDate <= today && a.EndDate >= today
            && !a.AbsenceType!.IsAuthorised);
        var registersOpen     = 0; // attendance_registers table not in DB

        return new DashboardOverviewDto
        {
            TotalStudents     = totalStudents,
            TotalStaff        = totalStaff,
            AbsencesToday     = absencesToday,
            UnauthorisedToday = unauthorisedToday,
            RegistersOpen     = registersOpen
        };
    }

    public async Task<IEnumerable<DashboardStudentActivityDto>> GetStudentActivityAsync(int topN = 10)
    {
        var result = await _context.Absences
            .Where(a => a.PersonType == "Student")
            .GroupBy(a => a.PersonId)
            .Select(g => new
            {
                StudentId    = g.Key,
                AbsenceCount = g.Count(),
                LastAbsence  = g.Max(a => a.StartDate)
            })
            .OrderByDescending(x => x.AbsenceCount)
            .Take(topN)
            .ToListAsync();

        var studentIds = result.Select(r => r.StudentId).ToList();
        var students   = await _context.Students
            .Where(s => studentIds.Contains((long)s.Id))
            .ToDictionaryAsync(s => s.Id, s => s.FirstName + " " + s.LastName);

        return result.Select(r => new DashboardStudentActivityDto
        {
            StudentId    = (long)r.StudentId,
            FullName     = students.TryGetValue((int)r.StudentId, out var name) ? name : string.Empty,
            AbsenceCount = r.AbsenceCount,
            LastAbsence  = r.LastAbsence
        });
    }

    public async Task<IEnumerable<DashboardSafeguardingDto>> GetSafeguardingAsync()
    {
        var flaggedContacts = await _context.StudentContacts
            .Where(c => c.IsPrimary)
            .ToListAsync();

        var studentIds = flaggedContacts.Select(c => c.StudentId).Distinct().ToList();
        var students   = await _context.Students
            .Where(s => studentIds.Contains(s.Id))
            .ToDictionaryAsync(s => s.Id, s => s.FirstName + " " + s.LastName);

        return flaggedContacts.Select(c => new DashboardSafeguardingDto
        {
            StudentId        = c.StudentId,
            FullName         = students.TryGetValue(c.StudentId, out var name) ? name : string.Empty,
            SafeguardingFlag = false, // Field removed from model
            Notes            = null    // Field removed from model
        });
    }
}
