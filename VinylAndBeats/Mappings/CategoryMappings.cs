using VinylAndBeats.DTOs;
using VinylAndBeats.Models;

namespace VinylAndBeats.Mappings;

public static class CategoryMappings
{
    public static CategoryDto ToDto(this Category category) => new(
        Id: category.Id,
        Name: category.Name);

    public static List<CategoryDto> ToDtoList(this IEnumerable<Category> categories)
        => categories.Select(c => c.ToDto()).ToList();
}