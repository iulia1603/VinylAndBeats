using System.ComponentModel.DataAnnotations;

namespace VinylAndBeats.Models;

public class Tag : BaseEntity
{
    [Required]
    [MinLength(2)]
    public string Name { get; set; } = string.Empty;

    public List<Product> Products { get; set; } = new();
}