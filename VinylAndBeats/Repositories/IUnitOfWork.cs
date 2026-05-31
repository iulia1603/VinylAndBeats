namespace VinylAndBeats.Repositories;

public interface IUnitOfWork
{
    IProductRepository ProductRepository { get; }
    ICategoryRepository CategoryRepository { get; }
    ITagRepository TagRepository { get; }
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}