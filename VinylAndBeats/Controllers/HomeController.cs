using Microsoft.AspNetCore.Mvc;
using VinylAndBeats.Mappings;
using VinylAndBeats.Services;
using VinylAndBeats.ViewModels;

namespace VinylAndBeats.Controllers;

public class HomeController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;

    public HomeController(IProductService productService, ICategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var products = await _productService.GetAllAsync(cancellationToken);
        var categories = await _categoryService.GetAllAsync(cancellationToken);

        var viewModel = new HomeViewModel
        {
            RecentProducts = products.Where(p => p.Stock > 0).Take(4).ToViewModelList(),
            Categories = categories,
            TotalProducts = products.Count
        };

        return View(viewModel);
    }
}