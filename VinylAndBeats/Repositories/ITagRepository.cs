using VinylAndBeats.Models;

namespace VinylAndBeats.Repositories;

public interface ITagRepository : IRepository<Tag>
{
    Task<List<Tag>> GetByIdsAsync(List<int> ids, CancellationToken cancellationToken = default);
}