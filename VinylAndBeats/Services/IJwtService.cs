using VinylAndBeats.Models;

namespace VinylAndBeats.Services;

public interface IJwtService
{
    Task<string> GenerateTokenAsync(ApplicationUser user);
    int ExpiresInSeconds { get; }
}