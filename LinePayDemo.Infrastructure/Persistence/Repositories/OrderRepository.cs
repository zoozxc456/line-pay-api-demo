using LinePayDemo.Infrastructure.Persistence.Contexts;
using LinePayDemo.Order.Repositories;

namespace LinePayDemo.Infrastructure.Persistence.Repositories;

public class OrderRepository(ShoppingMallContext context)
    : GenericRepository<Order.Models.Order>(context), IOrderRepository
{
    public new Task AddAsync(Order.Models.Order order)
    {
        return base.AddAsync(order);
    }

    public new Task UpdateAsync(Order.Models.Order order)
    {
        return base.UpdateAsync(order);
    }

    public Task<Order.Models.Order?> GetByOrderIdAsync(Guid orderId)
    {
        return FindAsync(x => x.OrderId == orderId);
    }

    public Task<List<Order.Models.Order>> GetAllForUserAsync(Guid userId)
    {
        return FindAllAsync(x => x.UserId == userId);
    }
}