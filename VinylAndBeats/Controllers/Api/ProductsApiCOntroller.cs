using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VinylAndBeats.Authorization;
using VinylAndBeats.DTOs;
using VinylAndBeats.Mappings;
using VinylAndBeats.Services;

namespace VinylAndBeats.Controllers.Api;

[ApiController]
[Route("api/products")]
public class ProductsApiController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ITagService _tagService;

    public ProductsApiController(IProductService productService, ITagService tagService)
    {
        _productService = productService;
        _tagService = tagService;
    }

    // GET: /api/products
    [HttpGet]
    [ProducesResponseType(typeof(List<ProductDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ProductDto>>> GetAll(CancellationToken cancellationToken)
    {
        var products = await _productService.GetAllAsync(cancellationToken);
        return Ok(products.ToDtoList());
    }

    // GET: /api/products/5
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var product = await _productService.GetByIdAsync(id, cancellationToken);
        if (product == null) return NotFound();
        return Ok(product.ToDto());
    }

    // POST: /api/products
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ProductDto>> Create(CreateProductDto dto, CancellationToken cancellationToken)
    {
        var product = dto.ToEntity();
        product.SellerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (dto.TagIds != null && dto.TagIds.Count > 0)
        {
            var tags = await _tagService.GetByIdsAsync(dto.TagIds, cancellationToken);
            foreach (var tag in tags) product.Tags.Add(tag);
        }

        await _productService.AddAsync(product, cancellationToken);

        var created = await _productService.GetByIdAsync(product.Id, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, created!.ToDto());
    }

    // PUT: /api/products/5
    [HttpPut("{id:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, UpdateProductDto dto, CancellationToken cancellationToken)
    {
        var product = await _productService.GetByIdAsync(id, cancellationToken);
        if (product == null) return NotFound();
        if (!User.CanModifyProduct(product)) return Forbid(JwtBearerDefaults.AuthenticationScheme);

        dto.ApplyTo(product);

        product.Tags.Clear();
        if (dto.TagIds != null && dto.TagIds.Count > 0)
        {
            var tags = await _tagService.GetByIdsAsync(dto.TagIds, cancellationToken);
            foreach (var tag in tags) product.Tags.Add(tag);
        }

        await _productService.UpdateAsync(product, cancellationToken);
        return NoContent();
    }

    // DELETE: /api/products/5
    [HttpDelete("{id:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var product = await _productService.GetByIdAsync(id, cancellationToken);
        if (product == null) return NotFound();
        if (!User.CanModifyProduct(product)) return Forbid(JwtBearerDefaults.AuthenticationScheme);

        await _productService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}