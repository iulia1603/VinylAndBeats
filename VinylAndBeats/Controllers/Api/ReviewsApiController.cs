using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VinylAndBeats.DTOs;
using VinylAndBeats.Mappings;
using VinylAndBeats.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace VinylAndBeats.Controllers.Api;

[ApiController]
[Route("api/products/{productId:int}/reviews")]
public class ReviewsApiController : ControllerBase
{
    private readonly IReviewService _reviewService;

    public ReviewsApiController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    // GET: /api/products/5/reviews — public
    [HttpGet]
    [ProducesResponseType(typeof(List<ReviewDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ReviewDto>>> GetForProduct(int productId, CancellationToken cancellationToken)
    {
        var reviews = await _reviewService.GetForProductAsync(productId, cancellationToken);
        return Ok(reviews.ToDtoList());
    }

    // POST: /api/products/5/reviews 
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(ReviewDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReviewDto>> Create(int productId, CreateReviewDto dto, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var review = await _reviewService.AddAsync(productId, userId, dto.Rating, dto.Comment, cancellationToken);
        var saved = (await _reviewService.GetForProductAsync(productId, cancellationToken))
            .First(r => r.Id == review.Id);

        return CreatedAtAction(nameof(GetForProduct), new { productId }, saved.ToDto());
    }
}