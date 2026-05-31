using VinylAndBeats.Models;

namespace VinylAndBeats.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<List<Product>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default);
    Task<Product?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default);
    Task<List<Product>> GetByCategoryAsync(int categoryId, CancellationToken cancellationToken = default);
}