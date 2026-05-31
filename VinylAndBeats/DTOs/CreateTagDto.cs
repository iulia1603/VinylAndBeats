using System.ComponentModel.DataAnnotations;
namespace VinylAndBeats.DTOs;

public record CreateTagDto([Required, MinLength(2)] string Name);