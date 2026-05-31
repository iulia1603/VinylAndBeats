using Microsoft.AspNetCore.Mvc;
using VinylAndBeats.Mappings;
using VinylAndBeats.Services;

namespace VinylAndBeats.Controllers;

public class ProductsController : Controller
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    // GET: /Products
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var products = await _productService.GetAllAsync(cancellationToken);
        return View(products.ToViewModelList());
    }

    // GET: /Products/Details/5
    public async Task<IActionResult> Details(int? id, CancellationToken cancellationToken)
    {
        if (id == null)
            return NotFound();

        var product = await _productService.GetByIdAsync(id.Value, cancellationToken);
        if (product == null)
            return NotFound();

        return View(product.ToViewModel());
    }
}