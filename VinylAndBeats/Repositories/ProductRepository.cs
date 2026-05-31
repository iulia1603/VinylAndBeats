using Microsoft.EntityFrameworkCore;
using VinylAndBeats.Data;
using VinylAndBeats.Models;

namespace VinylAndBeats.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context) { }

    public async Task<List<Product>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Seller)
            .Include(p => p.Tags)
            .OrderByDescending(p => p.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<Product?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Seller)
            .Include(p => p.Tags)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<List<Product>> GetByCategoryAsync(int categoryId, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .Where(p => p.CategoryId == categoryId)
            .Include(p => p.Category)
            .Include(p => p.Seller)
            .Include(p => p.Tags)
            .OrderByDescending(p => p.Id)
            .ToListAsync(cancellationToken);
    }
}