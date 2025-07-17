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

    public Task AddAsync(Domain.Order order)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Domain.Order order)
    {
        throw new NotImplementedException();
    }

    Task<Domain.Order?> IOrderRepository.GetByOrderIdAsync(Guid orderId)
    {
        throw new NotImplementedException();
    }

    public Task<Models.Order?> GetByOrderIdAsync(Guid orderId)
    {
        Orders.TryGetValue(orderId, out var order);
        return Task.FromResult(order);
    }

    public Task<Domain.Order?> GetByIdAsync(Guid orderId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Domain.Order>> GetAllForBuyerAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Models.Order>> GetAllForUserAsync(Guid userId)
    {
        return Task.FromResult(Orders.Values.Where(o => o.UserId == userId).ToList());
    }
}