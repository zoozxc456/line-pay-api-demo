using LinePayDemo.Ledger.Domain.Transaction;

namespace LinePayDemo.Ledger.Repositories;

public interface ITransactionRepository
{
    Task<Transaction?> GetByIdAsync(Guid id);
    Task<List<Transaction>> GetAllAsync();
    Task AddAsync(Transaction transaction);
}