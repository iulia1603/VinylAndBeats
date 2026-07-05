using VinylAndBeats.Data;

namespace VinylAndBeats.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    private IProductRepository? _productRepository;
    private ICategoryRepository? _categoryRepository;
    private ITagRepository? _tagRepository;
    private ICartRepository? _cartRepository;
    private IOrderRepository? _orderRepository;
    private IReviewRepository? _reviewRepository;

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
    public ICartRepository CartRepository => _cartRepository ??= new CartRepository(_context);
    public IOrderRepository OrderRepository => _orderRepository ??= new OrderRepository(_context);

    public IReviewRepository ReviewRepository => _reviewRepository ??= new ReviewRepository(_context);
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);
}