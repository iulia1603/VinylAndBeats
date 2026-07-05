using VinylAndBeats.Models;

namespace VinylAndBeats.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<List<Product>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default);
    Task<Product?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default);
    Task<List<Product>> GetFilteredAsync(int? categoryId, string? search, CancellationToken cancellationToken = default);
}