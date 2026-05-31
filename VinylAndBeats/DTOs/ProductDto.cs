namespace VinylAndBeats.DTOs;

public record ProductDto(
    int Id,
    string Name,
    string Description,
    decimal Price,
    int Stock,
    string? ImageUrl,
    int CategoryId,
    string CategoryName,
    string? SellerId,
    string SellerName,
    List<TagDto> Tags);