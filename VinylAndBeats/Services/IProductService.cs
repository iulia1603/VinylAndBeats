using VinylAndBeats.Models;

namespace VinylAndBeats.Services;

public interface IProductService
{
    Task<List<Product>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task AddAsync(Product product, CancellationToken cancellationToken = default);
    Task UpdateAsync(Product product, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<List<Product>> GetFilteredAsync(int? categoryId, string? search, CancellationToken cancellationToken = default);
    Task<List<Product>> GetBySellerAsync(string sellerId, CancellationToken cancellationToken = default);
}