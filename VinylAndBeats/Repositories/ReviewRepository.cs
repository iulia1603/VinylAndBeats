using Microsoft.EntityFrameworkCore;
using VinylAndBeats.Data;
using VinylAndBeats.Models;

namespace VinylAndBeats.Repositories;

public class ReviewRepository : Repository<Review>, IReviewRepository
{
    public ReviewRepository(AppDbContext context) : base(context) { }

    public async Task<List<Review>> GetByProductIdAsync(int productId, CancellationToken cancellationToken = default)
    {
        return await _context.Reviews
            .Where(r => r.ProductId == productId)
            .Include(r => r.Author)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> HasUserPurchasedProductAsync(string userId, int productId, CancellationToken cancellationToken = default)
    {
        return await _context.Orders
            .Where(o => o.BuyerId == userId)
            .SelectMany(o => o.Items)
            .AnyAsync(i => i.ProductId == productId, cancellationToken);
    }
}