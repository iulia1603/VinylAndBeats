using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VinylAndBeats.DTOs;
using VinylAndBeats.Mappings;
using VinylAndBeats.Models;
using VinylAndBeats.Services;

namespace VinylAndBeats.Controllers.Api;

[ApiController]
[Route("api/tags")]
public class TagsApiController : ControllerBase
{
    private readonly ITagService _tagService;

    public TagsApiController(ITagService tagService)
    {
        _tagService = tagService;
    }

    // GET: /api/tags — public
    [HttpGet]
    [ProducesResponseType(typeof(List<TagDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<TagDto>>> GetAll(CancellationToken cancellationToken)
    {
        var tags = await _tagService.GetAllAsync(cancellationToken);
        return Ok(tags.ToDtoList());
    }

    // POST: /api/tags — DOAR ADMIN
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    [ProducesResponseType(typeof(TagDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<TagDto>> Create(CreateTagDto dto, CancellationToken cancellationToken)
    {
        var tag = new Tag { Name = dto.Name };
        await _tagService.AddAsync(tag, cancellationToken);
        return CreatedAtAction(nameof(GetAll), new { id = tag.Id }, tag.ToDto());
    }
}