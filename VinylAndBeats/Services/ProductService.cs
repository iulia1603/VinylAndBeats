using VinylAndBeats.Models;
using VinylAndBeats.Repositories;

namespace VinylAndBeats.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ProductService> _logger;

    public ProductService(IUnitOfWork unitOfWork, ILogger<ProductService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<List<Product>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _unitOfWork.ProductRepository.GetAllWithDetailsAsync(cancellationToken);

    public async Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _unitOfWork.ProductRepository.GetByIdWithDetailsAsync(id, cancellationToken);

    public async Task AddAsync(Product product, CancellationToken cancellationToken = default)
    {
        await EnsureCategoryExistsAsync(product.CategoryId, cancellationToken);

        _logger.LogInformation("Creare produs {Name} de vânzătorul {SellerId}", product.Name, product.SellerId);
        await _unitOfWork.ProductRepository.AddAsync(product, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Produs creat cu id {ProductId}", product.Id);
    }

    public async Task UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        await EnsureCategoryExistsAsync(product.CategoryId, cancellationToken);

        _logger.LogInformation("Actualizare produs {ProductId}", product.Id);
        _unitOfWork.ProductRepository.Update(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var product = await _unitOfWork.ProductRepository.GetByIdAsync(id, cancellationToken);
        if (product != null)
        {
            _logger.LogInformation("Ștergere produs {ProductId}", id);
            _unitOfWork.ProductRepository.Delete(product);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }

    private async Task EnsureCategoryExistsAsync(int categoryId, CancellationToken cancellationToken)
    {
        var category = await _unitOfWork.CategoryRepository.GetByIdAsync(categoryId, cancellationToken);
        if (category == null)
            throw new ArgumentException("Categoria selectată nu există.");
    }
}