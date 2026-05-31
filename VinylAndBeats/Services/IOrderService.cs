using VinylAndBeats.Models;

namespace VinylAndBeats.Services;

public interface IOrderService
{
    Task<List<Order>> GetMyOrdersAsync(string buyerId, CancellationToken cancellationToken = default);
}