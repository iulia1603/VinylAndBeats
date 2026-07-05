using Microsoft.EntityFrameworkCore;
using VinylAndBeats.Data;
using VinylAndBeats.Models;

namespace VinylAndBeats.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(AppDbContext context) : base(context) { }
}