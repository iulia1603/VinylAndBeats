namespace VinylAndBeats.Repositories;

public interface IUnitOfWork
{
    IProductRepository ProductRepository { get; }
    ICategoryRepository CategoryRepository { get; }
    ITagRepository TagRepository { get; }
    ICartRepository CartRepository { get; }
    IOrderRepository OrderRepository { get; }
    IReviewRepository ReviewRepository { get; }
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}