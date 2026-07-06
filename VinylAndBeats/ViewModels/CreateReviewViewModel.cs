using System.ComponentModel.DataAnnotations;

namespace VinylAndBeats.ViewModels;

public class CreateReviewViewModel
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;

    [Range(1, 5, ErrorMessage = "Ratingul trebuie să fie între 1 și 5")]
    public int Rating { get; set; } = 5;

    [Required(ErrorMessage = "Comentariul este obligatoriu")]
    [MinLength(3, ErrorMessage = "Comentariul trebuie să aibă minim 3 caractere")]
    public string Comment { get; set; } = string.Empty;
}