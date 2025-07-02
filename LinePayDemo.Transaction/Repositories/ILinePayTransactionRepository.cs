using LinePayDemo.Transaction.Models;

namespace LinePayDemo.Transaction.Repositories;

public interface ILinePayTransactionRepository
{
    Task AddAsync(LinePayTransaction transaction);
    Task UpdateAsync(LinePayTransaction transaction);
    Task<LinePayTransaction?> GetByOrderIdAsync(Guid orderId);
    Task<LinePayTransaction?> GetByLinePayTransactionIdAsync(long linePayTransactionId);
    Task<IEnumerable<LinePayTransaction>> GetAllAsync();
}