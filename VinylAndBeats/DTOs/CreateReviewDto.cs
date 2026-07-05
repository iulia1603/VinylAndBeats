using System.ComponentModel.DataAnnotations;

namespace VinylAndBeats.DTOs;

public record CreateReviewDto(
    [Range(1, 5)] int Rating,
    [Required, MinLength(3)] string Comment);