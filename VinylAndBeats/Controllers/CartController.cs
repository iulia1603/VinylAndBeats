using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VinylAndBeats.Services;
using VinylAndBeats.ViewModels;

namespace VinylAndBeats.Controllers;

[Authorize]
public class CartController : Controller
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var cart = await _cartService.GetOrCreateCartAsync(UserId, cancellationToken);
        var vm = new CartViewModel
        {
            Items = cart.Items.Select(i => new CartItemViewModel
            {
                Id = i.Id,
                ProductId = i.ProductId,
                ProductName = i.Product?.Name ?? "N/A",
                UnitPrice = i.Product?.Price ?? 0,
                Quantity = i.Quantity
            }).ToList()
        };
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(int productId, int quantity = 1, CancellationToken cancellationToken = default)
    {
        try
        {
            await _cartService.AddToCartAsync(UserId, productId, quantity, cancellationToken);
            TempData["Success"] = "Produs adăugat în coș.";
        }
        catch (Exception ex) when (ex is ArgumentException or KeyNotFoundException)
        {
            TempData["Error"] = ex.Message;
        }
        return RedirectToAction("Index", "Products");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove(int cartItemId, CancellationToken cancellationToken)
    {
        await _cartService.RemoveFromCartAsync(UserId, cartItemId, cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Checkout(CancellationToken cancellationToken)
    {
        try
        {
            var order = await _cartService.CheckoutAsync(UserId, cancellationToken);
            TempData["Success"] = $"Comandă plasată! Total: {order.Total:0.00} lei.";
            return RedirectToAction("Index", "Orders");
        }
        catch (Exception ex) when (ex is ArgumentException or KeyNotFoundException)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}