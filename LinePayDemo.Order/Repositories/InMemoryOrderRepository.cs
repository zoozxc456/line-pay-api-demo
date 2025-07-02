using System.Collections.Concurrent;

namespace LinePayDemo.Order.Repositories;

public class InMemoryOrderRepository : IOrderRepository
{
    private static readonly ConcurrentDictionary<Guid, Models.Order> Orders = new();

    public Task AddAsync(Models.Order order)
    {
        Orders[order.OrderId] = order;
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Models.Order order)
    {
        Orders[order.OrderId] = order; // 記憶體中簡單替換
        return Task.CompletedTask;
    }

    public Task<Models.Order?> GetByOrderIdAsync(Guid orderId)
    {
        Orders.TryGetValue(orderId, out var order);
        return Task.FromResult(order);
    }

    public Task<IEnumerable<Models.Order>> GetAllForUserAsync(Guid userId)
    {
        return Task.FromResult<IEnumerable<Models.Order>>(Orders.Values.Where(o => o.UserId == userId).ToList());
    }
}