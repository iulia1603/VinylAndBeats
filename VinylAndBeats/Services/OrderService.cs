using VinylAndBeats.Models;
using VinylAndBeats.Repositories;

namespace VinylAndBeats.Services;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<Order>> GetMyOrdersAsync(string buyerId, CancellationToken cancellationToken = default)
        => await _unitOfWork.OrderRepository.GetByBuyerIdWithItemsAsync(buyerId, cancellationToken);
}