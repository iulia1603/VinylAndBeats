namespace VinylAndBeats.Models;

public class Review : BaseEntity
{
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public string AuthorId { get; set; } = string.Empty;
    public ApplicationUser Author { get; set; } = null!;

    public int Rating { get; set; }           
    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}