using VinylAndBeats.Models;
using VinylAndBeats.Repositories;

namespace VinylAndBeats.Services;

public class ReviewService : IReviewService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ReviewService> _logger;

    public ReviewService(IUnitOfWork unitOfWork, ILogger<ReviewService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<List<Review>> GetForProductAsync(int productId, CancellationToken cancellationToken = default)
        => await _unitOfWork.ReviewRepository.GetByProductIdAsync(productId, cancellationToken);

    public async Task<Review> AddAsync(int productId, string userId, int rating, string comment, CancellationToken cancellationToken = default)
    {
        var product = await _unitOfWork.ProductRepository.GetByIdAsync(productId, cancellationToken);
        if (product == null)
            throw new KeyNotFoundException("Produsul nu există.");

        var hasPurchased = await _unitOfWork.ReviewRepository.HasUserPurchasedProductAsync(userId, productId, cancellationToken);
        if (!hasPurchased)
            throw new ArgumentException("Poți lăsa o recenzie doar pentru un produs pe care l-ai cumpărat.");

        var review = new Review
        {
            ProductId = productId,
            AuthorId = userId,
            Rating = rating,
            Comment = comment,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.ReviewRepository.AddAsync(review, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Recenzie adăugată la produsul {ProductId} de {UserId}", productId, userId);

        return review;
    }
}