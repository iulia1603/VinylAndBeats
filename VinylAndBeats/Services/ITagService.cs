using VinylAndBeats.Models;

namespace VinylAndBeats.Services;

public interface ITagService
{
    Task<List<Tag>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Tag?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}