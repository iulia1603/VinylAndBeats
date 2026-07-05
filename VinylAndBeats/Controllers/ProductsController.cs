using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VinylAndBeats.Authorization;
using VinylAndBeats.Mappings;
using VinylAndBeats.Models;
using VinylAndBeats.Services;
using VinylAndBeats.ViewModels;

namespace VinylAndBeats.Controllers;

public class ProductsController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    private readonly ITagService _tagService;
    private readonly IWebHostEnvironment _env;

    public ProductsController(
        IProductService productService,
        ICategoryService categoryService,
        ITagService tagService,
        IWebHostEnvironment env)
    {
        _productService = productService;
        _categoryService = categoryService;
        _tagService = tagService;
        _env = env;
    }

    // GET: /Products — public
    public async Task<IActionResult> Index(int? categoryId, string? search, CancellationToken cancellationToken)
    {
        var products = await _productService.GetFilteredAsync(categoryId, search, cancellationToken);
        var categories = await _categoryService.GetAllAsync(cancellationToken);

        var vm = new ProductsIndexViewModel
        {
            Products = products.Where(p => p.Stock > 0).ToViewModelList(),
            Categories = categories,
            SelectedCategoryId = categoryId,
            Search = search
        };
        return View(vm);
    }

    // GET: /Products/Details/5 — public
    public async Task<IActionResult> Details(int? id, CancellationToken cancellationToken)
    {
        if (id == null) return NotFound();
        var product = await _productService.GetByIdAsync(id.Value, cancellationToken);
        if (product == null) return NotFound();
        return View(product.ToViewModel());
    }

    // GET: /Products/Create — necesita login
    [Authorize]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        var viewModel = new CreateProductViewModel();
        await LoadDropdownsAsync(viewModel, cancellationToken);
        return View(viewModel);
    }

    // POST: /Products/Create
    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateProductViewModel viewModel, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            await LoadDropdownsAsync(viewModel, cancellationToken);
            return View(viewModel);
        }

        if (viewModel.ImageFile is { Length: > 0 })
        {
            var ext = Path.GetExtension(viewModel.ImageFile.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(ext))
            {
                ModelState.AddModelError("ImageFile", "Tip de fișier neacceptat (jpg, png, webp, gif).");
                await LoadDropdownsAsync(viewModel, cancellationToken);
                return View(viewModel);
            }
        }

        var product = new Product
        {
            Name = viewModel.Name,
            Description = viewModel.Description,
            Price = viewModel.Price,
            Stock = viewModel.Stock,
            ImageUrl = await SaveImageAsync(viewModel.ImageFile),
            CategoryId = viewModel.CategoryId,
            SellerId = User.FindFirstValue(ClaimTypes.NameIdentifier)
        };

        await _productService.AddAsync(product, cancellationToken);

        if (viewModel.SelectedTagIds.Any())
        {
            var tags = await _tagService.GetByIdsAsync(viewModel.SelectedTagIds, cancellationToken);
            product.Tags = tags;
            await _productService.UpdateAsync(product, cancellationToken);
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: /Products/Edit/5 — doar proprietar sau admin
    [Authorize]
    public async Task<IActionResult> Edit(int? id, CancellationToken cancellationToken)
    {
        if (id == null) return NotFound();
        var product = await _productService.GetByIdAsync(id.Value, cancellationToken);
        if (product == null) return NotFound();
        if (!User.CanModifyProduct(product)) return Forbid();

        var viewModel = new EditProductViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            ExistingImageUrl = product.ImageUrl,
            CategoryId = product.CategoryId,
            SelectedTagIds = product.Tags?.Select(t => t.Id).ToList() ?? new()
        };
        await LoadDropdownsAsync(viewModel, cancellationToken);
        return View(viewModel);
    }

    // POST: /Products/Edit/5
    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EditProductViewModel viewModel, CancellationToken cancellationToken)
    {
        if (id != viewModel.Id) return NotFound();

        var product = await _productService.GetByIdAsync(id, cancellationToken);
        if (product == null) return NotFound();
        if (!User.CanModifyProduct(product)) return Forbid();

        if (!ModelState.IsValid)
        {
            viewModel.ExistingImageUrl = product.ImageUrl;
            await LoadDropdownsAsync(viewModel, cancellationToken);
            return View(viewModel);
        }

        if (viewModel.ImageFile is { Length: > 0 })
        {
            var ext = Path.GetExtension(viewModel.ImageFile.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(ext))
            {
                ModelState.AddModelError("ImageFile", "Tip de fișier neacceptat (jpg, png, webp, gif).");
                viewModel.ExistingImageUrl = product.ImageUrl;
                await LoadDropdownsAsync(viewModel, cancellationToken);
                return View(viewModel);
            }
            product.ImageUrl = await SaveImageAsync(viewModel.ImageFile);
        }

        product.Name = viewModel.Name;
        product.Description = viewModel.Description;
        product.Price = viewModel.Price;
        product.Stock = viewModel.Stock;
        product.CategoryId = viewModel.CategoryId;

        product.Tags ??= new();
        product.Tags.Clear();
        if (viewModel.SelectedTagIds.Any())
        {
            var tags = await _tagService.GetByIdsAsync(viewModel.SelectedTagIds, cancellationToken);
            foreach (var tag in tags)
                product.Tags.Add(tag);
        }

        await _productService.UpdateAsync(product, cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    // GET: /Products/Delete/5 — doar proprietar sau admin
    [Authorize]
    public async Task<IActionResult> Delete(int? id, CancellationToken cancellationToken)
    {
        if (id == null) return NotFound();
        var product = await _productService.GetByIdAsync(id.Value, cancellationToken);
        if (product == null) return NotFound();
        if (!User.CanModifyProduct(product)) return Forbid();
        return View(product.ToViewModel());
    }

    // POST: /Products/Delete/5
    [Authorize]
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
    {
        var product = await _productService.GetByIdAsync(id, cancellationToken);
        if (product == null) return NotFound();
        if (!User.CanModifyProduct(product)) return Forbid();
        await _productService.DeleteAsync(id, cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".webp", ".gif" };

    private async Task<string?> SaveImageAsync(IFormFile? file)
    {
        if (file == null || file.Length == 0) return null;
        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        var imagesDir = Path.Combine(_env.WebRootPath, "images");
        Directory.CreateDirectory(imagesDir);
        var fileName = $"{Guid.NewGuid():N}{ext}";
        var savePath = Path.Combine(imagesDir, fileName);
        await using var stream = System.IO.File.Create(savePath);
        await file.CopyToAsync(stream);
        return $"/images/{fileName}";
    }
    private async Task LoadDropdownsAsync(CreateProductViewModel viewModel, CancellationToken cancellationToken)
    {
        var categories = await _categoryService.GetAllAsync(cancellationToken);
        viewModel.Categories = categories
            .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
            .ToList();
        viewModel.AvailableTags = await _tagService.GetAllAsync(cancellationToken);
    }
}