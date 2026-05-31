using VinylAndBeats.Models;
using VinylAndBeats.Repositories;

namespace VinylAndBeats.Services;

public class TagService : ITagService
{
    private readonly IUnitOfWork _unitOfWork;

    public TagService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<Tag>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _unitOfWork.TagRepository.GetAllAsync(cancellationToken);

    public async Task<Tag?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _unitOfWork.TagRepository.GetByIdAsync(id, cancellationToken);
}