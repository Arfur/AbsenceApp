namespace AbsenceApp.Core.DTOs;

public class AuthResultDto
{
    public bool    Success      { get; init; }
    public string? ErrorMessage { get; init; }
    public long    UserId       { get; init; }
    public string  UserName     { get; init; } = string.Empty;
    public string  Role         { get; init; } = string.Empty;
}
