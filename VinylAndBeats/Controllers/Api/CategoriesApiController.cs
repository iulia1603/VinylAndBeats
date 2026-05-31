using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VinylAndBeats.DTOs;
using VinylAndBeats.Mappings;
using VinylAndBeats.Models;
using VinylAndBeats.Services;

namespace VinylAndBeats.Controllers.Api;

[ApiController]
[Route("api/categories")]
public class CategoriesApiController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesApiController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    // GET: /api/categories — public
    [HttpGet]
    [ProducesResponseType(typeof(List<CategoryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<CategoryDto>>> GetAll(CancellationToken cancellationToken)
    {
        var categories = await _categoryService.GetAllAsync(cancellationToken);
        return Ok(categories.ToDtoList());
    }

    // POST: /api/categories — DOAR ADMIN
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<CategoryDto>> Create(CreateCategoryDto dto, CancellationToken cancellationToken)
    {
        var category = new Category { Name = dto.Name };
        await _categoryService.AddAsync(category, cancellationToken);
        return CreatedAtAction(nameof(GetAll), new { id = category.Id }, category.ToDto());
    }
}