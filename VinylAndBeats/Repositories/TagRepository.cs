using Microsoft.EntityFrameworkCore;
using VinylAndBeats.Data;
using VinylAndBeats.Models;

namespace VinylAndBeats.Repositories;

public class TagRepository : Repository<Tag>, ITagRepository
{
    public TagRepository(AppDbContext context) : base(context) { }

    public async Task<List<Tag>> GetByIdsAsync(List<int> ids, CancellationToken cancellationToken = default)
        => await _dbSet.Where(t => ids.Contains(t.Id)).ToListAsync(cancellationToken);
}