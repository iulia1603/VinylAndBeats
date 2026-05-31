using System.ComponentModel.DataAnnotations;
namespace VinylAndBeats.DTOs;

public record LoginDto(
    [Required, EmailAddress] string Email,
    [Required] string Password);