using VinylAndBeats.DTOs;
using VinylAndBeats.Models;

namespace VinylAndBeats.Mappings;

public static class ProductMappings
{
    public static ProductDto ToDto(this Product product) => new(
        Id: product.Id,
        Name: product.Name,
        Description: product.Description,
        Price: product.Price,
        Stock: product.Stock,
        ImageUrl: product.ImageUrl,
        CategoryId: product.CategoryId,
        CategoryName: product.Category?.Name ?? "N/A",
        SellerId: product.SellerId,
        SellerName: product.Seller?.FullName ?? "N/A",
        Tags: product.Tags?.Select(t => new TagDto(t.Id, t.Name)).ToList() ?? new List<TagDto>());

    public static List<ProductDto> ToDtoList(this IEnumerable<Product> products)
        => products.Select(p => p.ToDto()).ToList();

    public static Product ToEntity(this CreateProductDto dto) => new()
    {
        Name = dto.Name,
        Description = dto.Description,
        Price = dto.Price,
        Stock = dto.Stock,
        ImageUrl = dto.ImageUrl,
        CategoryId = dto.CategoryId
    };

    public static void ApplyTo(this UpdateProductDto dto, Product product)
    {
        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Price = dto.Price;
        product.Stock = dto.Stock;
        product.ImageUrl = dto.ImageUrl;
        product.CategoryId = dto.CategoryId;
    }
}