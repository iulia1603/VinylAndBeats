using VinylAndBeats.Data;
using VinylAndBeats.Models;

namespace VinylAndBeats.Repositories;

public class TagRepository : Repository<Tag>, ITagRepository
{
    public TagRepository(AppDbContext context) : base(context) { }
}