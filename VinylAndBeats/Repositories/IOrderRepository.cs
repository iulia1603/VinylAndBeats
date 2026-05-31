using VinylAndBeats.Models;

namespace VinylAndBeats.Repositories;

public interface IOrderRepository : IRepository<Order>
{
    Task<List<Order>> GetByBuyerIdWithItemsAsync(string buyerId, CancellationToken cancellationToken = default);
}