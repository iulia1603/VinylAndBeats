using System.ComponentModel.DataAnnotations;

namespace VinylAndBeats.Models;

public class CartItem : BaseEntity
{
    public int CartId { get; set; }
    public Cart? Cart { get; set; }

    public int ProductId { get; set; }
    public Product? Product { get; set; }

    [Range(1, 100000)]
    public int Quantity { get; set; }
}