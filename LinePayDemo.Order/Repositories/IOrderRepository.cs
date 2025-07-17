namespace LinePayDemo.Order.Repositories;

public interface IOrderRepository
{
    Task AddAsync(Domain.Order order);
    Task UpdateAsync(Domain.Order order);
    Task<Domain.Order?> GetByOrderIdAsync(Guid orderId);
    Task<Domain.Order?> GetByIdAsync(Guid orderId);
    Task<List<Domain.Order>> GetAllForBuyerAsync(Guid userId);
}