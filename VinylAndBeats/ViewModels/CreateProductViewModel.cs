using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using VinylAndBeats.Models;

namespace VinylAndBeats.ViewModels;

public class CreateProductViewModel
{
    [Required(ErrorMessage = "Numele este obligatoriu")]
    [MinLength(2, ErrorMessage = "Numele trebuie să aibă minim 2 caractere")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Descrierea este obligatorie")]
    public string Description { get; set; } = string.Empty;

    [Range(0, 1000000, ErrorMessage = "Prețul trebuie să fie pozitiv")]
    [Display(Name = "Preț")]
    public decimal Price { get; set; }

    [Range(1, 100000, ErrorMessage = "Stocul trebuie să fie cel puțin 1")]
    public int Stock { get; set; } = 1;

    [Display(Name = "Poză produs")]
    public IFormFile? ImageFile { get; set; }

    [Required(ErrorMessage = "Categoria este obligatorie")]
    [Display(Name = "Categorie")]
    public int CategoryId { get; set; }

    public List<SelectListItem> Categories { get; set; } = new();
    public List<int> SelectedTagIds { get; set; } = new();
    public List<Tag> AvailableTags { get; set; } = new();
}