using VinylAndBeats.Models;
using VinylAndBeats.ViewModels;

namespace VinylAndBeats.Mappings;

public static class ProductViewModelMappings
{
    public static ProductViewModel ToViewModel(this Product product) => new()
    {
        Id = product.Id,
        Name = product.Name,
        Description = product.Description,
        Price = product.Price,
        Stock = product.Stock,
        ImageUrl = product.ImageUrl,
        CategoryName = product.Category?.Name ?? "N/A",
        SellerName = product.Seller?.FullName ?? "N/A",
        Tags = product.Tags?.Select(t => t.Name).ToList() ?? new()
    };

    public static List<ProductViewModel> ToViewModelList(this IEnumerable<Product> products)
        => products.Select(p => p.ToViewModel()).ToList();
}