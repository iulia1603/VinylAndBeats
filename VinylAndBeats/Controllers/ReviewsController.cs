using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VinylAndBeats.Services;
using VinylAndBeats.ViewModels;

namespace VinylAndBeats.Controllers;

[Authorize]
public class ReviewsController : Controller
{
    private readonly IReviewService _reviewService;
    private readonly IProductService _productService;

    public ReviewsController(IReviewService reviewService, IProductService productService)
    {
        _reviewService = reviewService;
        _productService = productService;
    }

    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    // GET: /Reviews/Create?productId=5
    [HttpGet]
    public async Task<IActionResult> Create(int productId, CancellationToken cancellationToken)
    {
        var product = await _productService.GetByIdAsync(productId, cancellationToken);
        if (product == null) return NotFound();

        var vm = new CreateReviewViewModel
        {
            ProductId = productId,
            ProductName = product.Name
        };
        return View(vm);
    }

    // POST: /Reviews/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateReviewViewModel vm, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(vm);

        try
        {
            await _reviewService.AddAsync(vm.ProductId, UserId, vm.Rating, vm.Comment, cancellationToken);
            TempData["Success"] = "Recenzie adăugată!";
            return RedirectToAction("Index", "Orders");
        }
        catch (Exception ex) when (ex is ArgumentException or KeyNotFoundException)
        {
            // ex. "doar cine a cumpărat"
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(vm);
        }
    }
}