using LinePayDemo.Infrastructure.Persistence.Contexts;
using LinePayDemo.Transaction.Models;
using LinePayDemo.Transaction.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LinePayDemo.Infrastructure.Persistence.Repositories;

public class LinePayTransactionRepository(ShoppingMallContext context)
    : GenericRepository<LinePayTransaction>(context), ILinePayTransactionRepository
{
    protected override IQueryable<LinePayTransaction> ApplyQuery(IQueryable<LinePayTransaction> query)
    {
        return query.Include(x => x.RefundTransactions);
    }

    public new Task AddAsync(LinePayTransaction transaction)
    {
        return base.AddAsync(transaction);
    }

    public new Task UpdateAsync(LinePayTransaction transaction)
    {
        return base.UpdateAsync(transaction);
    }

    public Task<LinePayTransaction?> GetByIdAsync(Guid id)
    {
        return FindAsync(x => x.Id == id);
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