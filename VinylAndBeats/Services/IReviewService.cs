using VinylAndBeats.Models;

namespace VinylAndBeats.Services;

public interface IReviewService
{
    Task<List<Review>> GetForProductAsync(int productId, CancellationToken cancellationToken = default);
    Task<Review> AddAsync(int productId, string userId, int rating, string comment, CancellationToken cancellationToken = default);
}