using VinylAndBeats.Models;

namespace VinylAndBeats.Repositories;

public interface IReviewRepository : IRepository<Review>
{
    Task<List<Review>> GetByProductIdAsync(int productId, CancellationToken cancellationToken = default);
    Task<bool> HasUserPurchasedProductAsync(string userId, int productId, CancellationToken cancellationToken = default);
}