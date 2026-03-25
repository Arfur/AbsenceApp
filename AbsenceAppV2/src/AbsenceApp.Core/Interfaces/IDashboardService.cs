using AbsenceApp.Core.DTOs;

namespace AbsenceApp.Core.Interfaces;

public interface IDashboardService
{
    Task<DashboardOverviewDto>                      GetOverviewAsync();
    Task<IEnumerable<DashboardStudentActivityDto>>  GetStudentActivityAsync(int topN = 10);
    Task<IEnumerable<DashboardSafeguardingDto>>     GetSafeguardingAsync();
}
