using VinylAndBeats.Models;
using VinylAndBeats.Repositories;

namespace VinylAndBeats.Services;

public class CartService : ICartService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CartService> _logger;

    public CartService(IUnitOfWork unitOfWork, ILogger<CartService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Cart> GetOrCreateCartAsync(string userId, CancellationToken cancellationToken = default)
    {
        var cart = await _unitOfWork.CartRepository.GetByUserIdWithItemsAsync(userId, cancellationToken);
        if (cart == null)
        {
            cart = new Cart { UserId = userId };
            await _unitOfWork.CartRepository.AddAsync(cart, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        return cart;
    }

    public async Task AddToCartAsync(string userId, int productId, int quantity, CancellationToken cancellationToken = default)
    {
        if (quantity < 1)
            throw new ArgumentException("Cantitatea trebuie să fie cel puțin 1.");

        var product = await _unitOfWork.ProductRepository.GetByIdAsync(productId, cancellationToken);
        if (product == null)
            throw new KeyNotFoundException("Produsul nu există.");

        if (product.SellerId == userId)
            throw new ArgumentException("Nu îți poți adăuga în coș propriul produs.");

        var cart = await GetOrCreateCartAsync(userId, cancellationToken);
        var existing = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        var alreadyInCart = existing?.Quantity ?? 0;

        if (product.Stock < alreadyInCart + quantity)
            throw new ArgumentException($"Stoc insuficient. Disponibil: {product.Stock}, deja în coș: {alreadyInCart}.");

        if (existing != null)
            existing.Quantity += quantity;
        else
            cart.Items.Add(new CartItem { ProductId = productId, Quantity = quantity });

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveFromCartAsync(string userId, int cartItemId, CancellationToken cancellationToken = default)
    {
        var cart = await _unitOfWork.CartRepository.GetByUserIdWithItemsAsync(userId, cancellationToken);
        if (cart == null) return;

        var item = cart.Items.FirstOrDefault(i => i.Id == cartItemId);
        if (item != null)
        {
            cart.Items.Remove(item);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<Order> CheckoutAsync(string userId, CancellationToken cancellationToken = default)
    {
        var cart = await _unitOfWork.CartRepository.GetByUserIdWithItemsAsync(userId, cancellationToken);
        if (cart == null || cart.Items.Count == 0)
            throw new ArgumentException("Coșul este gol.");

        var order = new Order { BuyerId = userId, OrderDate = DateTime.UtcNow };
        decimal total = 0;

        foreach (var item in cart.Items)
        {
            var product = item.Product!;
            if (product.Stock < item.Quantity)
                throw new ArgumentException($"Stoc insuficient pentru \"{product.Name}\". Disponibil: {product.Stock}.");

            product.Stock -= item.Quantity;

            order.Items.Add(new OrderItem
            {
                ProductId = product.Id,
                Quantity = item.Quantity,
                UnitPrice = product.Price 
            });

            total += product.Price * item.Quantity;
        }

        order.Total = total;
        await _unitOfWork.OrderRepository.AddAsync(order, cancellationToken);

        cart.Items.Clear();
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Comandă {OrderId} plasată de {UserId}, total {Total}", order.Id, userId, total);
        return order;
    }

    public async Task UpdateQuantityAsync(string userId, int cartItemId, int quantity, CancellationToken cancellationToken = default)
    {
        if (quantity < 1)
            throw new ArgumentException("Cantitatea trebuie să fie cel puțin 1.");

        var cart = await _unitOfWork.CartRepository.GetByUserIdWithItemsAsync(userId, cancellationToken);
        var item = cart?.Items.FirstOrDefault(i => i.Id == cartItemId);
        if (item == null)
            throw new KeyNotFoundException("Produsul nu e în coș.");

        if (item.Product!.Stock < quantity)
            throw new ArgumentException($"Stoc insuficient. Disponibil: {item.Product.Stock}.");

        item.Quantity = quantity;
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}