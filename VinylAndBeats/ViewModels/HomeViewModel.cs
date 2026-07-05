using VinylAndBeats.Models;

namespace VinylAndBeats.ViewModels;

public class HomeViewModel
{
    public List<ProductViewModel> RecentProducts { get; set; } = new();
    public List<Category> Categories { get; set; } = new();
    public int TotalProducts { get; set; }
}