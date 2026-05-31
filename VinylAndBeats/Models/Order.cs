namespace VinylAndBeats.Models;

public class Order : BaseEntity
{
    public string BuyerId { get; set; } = string.Empty;
    public ApplicationUser? Buyer { get; set; }

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    public decimal Total { get; set; }

    public List<OrderItem> Items { get; set; } = new();
}