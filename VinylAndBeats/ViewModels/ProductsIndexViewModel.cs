using VinylAndBeats.Models;

namespace VinylAndBeats.ViewModels;

public class ProductsIndexViewModel
{
    public List<ProductViewModel> Products { get; set; } = new();
    public List<Category> Categories { get; set; } = new();
    public int? SelectedCategoryId { get; set; }
    public string? Search { get; set; }
}