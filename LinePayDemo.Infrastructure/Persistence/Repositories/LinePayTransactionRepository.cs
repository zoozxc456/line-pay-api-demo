using LinePayDemo.Infrastructure.Persistence.Contexts;
using LinePayDemo.Transaction.Models;
using LinePayDemo.Transaction.Repositories;

namespace LinePayDemo.Infrastructure.Persistence.Repositories;

public class LinePayTransactionRepository(ShoppingMallContext context)
    : GenericRepository<LinePayTransaction>(context), ILinePayTransactionRepository
{
    public new Task AddAsync(LinePayTransaction transaction)
    {
        return base.AddAsync(transaction);
    }

    public new Task UpdateAsync(LinePayTransaction transaction)
    {
        return base.UpdateAsync(transaction);
    }

    public Task<LinePayTransaction?> GetByOrderIdAsync(Guid orderId)
    {
        return FindAsync(x => x.OrderId == orderId);
    }

    public Task<LinePayTransaction?> GetByLinePayTransactionIdAsync(long linePayTransactionId)
    {
        return FindAsync(x => x.LinePayTransactionId == linePayTransactionId);
    }

    public Task<List<LinePayTransaction>> GetAllAsync()
    {
        return FindAllAsync();
    }
}