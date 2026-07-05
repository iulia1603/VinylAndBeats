namespace VinylAndBeats.DTOs;

public record ReviewDto(
    int Id,
    int ProductId,
    string AuthorName,
    int Rating,
    string Comment,
    DateTime CreatedAt);