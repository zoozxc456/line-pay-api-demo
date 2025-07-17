using LinePayDemo.Infrastructure.Persistence.Contexts;
using LinePayDemo.Ledger.Domain.Transaction;
using LinePayDemo.Ledger.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LinePayDemo.Infrastructure.Persistence.Repositories;

public class TransactionRepository(ShoppingMallContext context)
    : GenericRepository<Transaction>(context), ITransactionRepository
{
    protected override IQueryable<Transaction> ApplyQuery(IQueryable<Transaction> query)
    {
        return query.Include(x => x.Entries);
    }

    public Task<Transaction?> GetByIdAsync(Guid id)
    {
        return FindAsync(x => x.Id == id);
    }

    public Task<List<Transaction>> GetAllAsync()
    {
        return FindAllAsync();
    }

    public new Task AddAsync(Transaction transaction)
    {
        return base.AddAsync(transaction);
    }
}