using System.Security.Claims;
using VinylAndBeats.Models;

namespace VinylAndBeats.Authorization;

public static class ClaimsPrincipalExtensions
{
    public static bool CanModifyProduct(this ClaimsPrincipal user, Product product)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        return product.SellerId == userId || user.IsInRole("Admin");
    }
}