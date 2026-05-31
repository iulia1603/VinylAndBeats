namespace VinylAndBeats.ViewModels;

public class HomeViewModel
{
    public List<ProductViewModel> RecentProducts { get; set; } = new();
    public int TotalProducts { get; set; }
    public int TotalCategories { get; set; }
}