using VinylAndBeats.Models;

namespace VinylAndBeats.Repositories;

public interface ICartRepository : IRepository<Cart>
{
    Task<Cart?> GetByUserIdWithItemsAsync(string userId, CancellationToken cancellationToken = default);
}