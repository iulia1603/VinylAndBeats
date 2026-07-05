using VinylAndBeats.DTOs;
using VinylAndBeats.Models;

namespace VinylAndBeats.Mappings;

public static class ReviewMappings
{
    public static ReviewDto ToDto(this Review review) => new(
        Id: review.Id,
        ProductId: review.ProductId,
        AuthorName: review.Author?.FullName ?? "Anonim",
        Rating: review.Rating,
        Comment: review.Comment,
        CreatedAt: review.CreatedAt);

    public static List<ReviewDto> ToDtoList(this IEnumerable<Review> reviews)
        => reviews.Select(r => r.ToDto()).ToList();
}