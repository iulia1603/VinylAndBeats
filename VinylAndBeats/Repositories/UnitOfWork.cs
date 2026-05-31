using VinylAndBeats.Data;

namespace VinylAndBeats.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    private IProductRepository? _productRepository;
    private ICategoryRepository? _categoryRepository;
    private ITagRepository? _tagRepository;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IProductRepository ProductRepository
        => _productRepository ??= new ProductRepository(_context);

    public ICategoryRepository CategoryRepository
        => _categoryRepository ??= new CategoryRepository(_context);

    public ITagRepository TagRepository
        => _tagRepository ??= new TagRepository(_context);

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);
}