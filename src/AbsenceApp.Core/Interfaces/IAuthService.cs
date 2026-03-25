using AbsenceApp.Core.DTOs;

namespace AbsenceApp.Core.Interfaces;

public interface IAuthService
{
    Task<AuthResultDto> LoginAsync(string username, string password);
    Task<AuthResultDto> RegisterAsync(string email, string password);
    Task LogoutAsync();
}
