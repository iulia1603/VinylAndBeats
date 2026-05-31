using System.ComponentModel.DataAnnotations;

namespace VinylAndBeats.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Numele de utilizator este obligatoriu")]
    [Display(Name = "Nume utilizator")]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Emailul este obligatoriu")]
    [EmailAddress(ErrorMessage = "Email invalid")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Numele complet este obligatoriu")]
    [Display(Name = "Nume complet")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Parola este obligatorie")]
    [DataType(DataType.Password)]
    [MinLength(6, ErrorMessage = "Parola trebuie să aibă minim 6 caractere")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirmarea parolei este obligatorie")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Parolele nu coincid")]
    [Display(Name = "Confirmă parola")]
    public string ConfirmPassword { get; set; } = string.Empty;
}