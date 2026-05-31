using System.ComponentModel.DataAnnotations;

namespace VinylAndBeats.DTOs;

public record UpdateProductDto(
    [Required, MinLength(2)] string Name,
    [Required] string Description,
    [Range(0, 1000000)] decimal Price,
    [Range(0, 100000)] int Stock,
    string? ImageUrl,
    [Required] int CategoryId,
    List<int>? TagIds = null);