namespace VinylAndBeats.Models;

public class Cart : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser? User { get; set; }

    public List<CartItem> Items { get; set; } = new();
}