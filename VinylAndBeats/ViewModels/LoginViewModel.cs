using System.ComponentModel.DataAnnotations;

namespace VinylAndBeats.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Emailul este obligatoriu")]
    [EmailAddress(ErrorMessage = "Email invalid")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Parola este obligatorie")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Ține-mă minte")]
    public bool RememberMe { get; set; }
}