using LinePayDemo.Infrastructure.Persistence.Contexts;
using LinePayDemo.Order.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LinePayDemo.Infrastructure.Persistence.Repositories;

public class OrderRepository(ShoppingMallContext context)
    : GenericRepository<Order.Domain.Order>(context), IOrderRepository
{
    protected override IQueryable<Order.Domain.Order> ApplyQuery(IQueryable<Order.Domain.Order> query)
    {
        return query
            .Include(x => x.OrderDetails)
            .Include(x => x.LedgerTransaction);
    }

    public new Task AddAsync(Order.Domain.Order order)
    {
        return base.AddAsync(order);
    }

    public new Task UpdateAsync(Order.Domain.Order order)
    {
        return base.UpdateAsync(order);
    }

    public Task<Order.Domain.Order?> GetByOrderIdAsync(Guid orderId)
    {
        return FindAsync(x => x.OrderId == orderId);
    }

    public Task<Order.Domain.Order?> GetByIdAsync(Guid orderId)
    {
        return FindAsync(x => x.OrderId == orderId);
    }

    public Task<List<Order.Domain.Order>> GetAllForBuyerAsync(Guid userId)
    {
        return FindAllAsync(x => x.BuyerId == userId);
    }
}