namespace LinePayDemo.Order.Repositories;

public interface IOrderRepository
{
    Task AddAsync(Models.Order order);
    Task UpdateAsync(Models.Order order);
    Task<Models.Order?> GetByOrderIdAsync(Guid orderId);
    Task<List<Models.Order>> GetAllForUserAsync(Guid userId);
}