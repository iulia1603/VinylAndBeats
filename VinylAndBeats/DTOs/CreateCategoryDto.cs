using System.ComponentModel.DataAnnotations;
namespace VinylAndBeats.DTOs;

public record CreateCategoryDto([Required, MinLength(2)] string Name);