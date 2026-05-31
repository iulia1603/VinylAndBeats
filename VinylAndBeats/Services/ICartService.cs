using VinylAndBeats.Models;

namespace VinylAndBeats.Services;

public interface ICartService
{
    Task<Cart> GetOrCreateCartAsync(string userId, CancellationToken cancellationToken = default);
    Task AddToCartAsync(string userId, int productId, int quantity, CancellationToken cancellationToken = default);
    Task RemoveFromCartAsync(string userId, int cartItemId, CancellationToken cancellationToken = default);
    Task<Order> CheckoutAsync(string userId, CancellationToken cancellationToken = default);
}