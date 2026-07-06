using System.ComponentModel.DataAnnotations;

namespace VinylAndBeats.Models;

public class Product : BaseEntity
{
    [Required]
    [MinLength(2)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Range(0.01, 1000000)]
    public decimal Price { get; set; }

    [Range(1, 100000)]
    public int Stock { get; set; }

    public string? ImageUrl { get; set; }

    public int CategoryId { get; set; }
    public Category? Category { get; set; }

    public string? SellerId { get; set; }
    public ApplicationUser? Seller { get; set; }

    public List<Tag> Tags { get; set; } = new();
}