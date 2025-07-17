using LinePayDemo.Payment.Domain.LinePay;

namespace LinePayDemo.Payment.Interfaces.Repositories;

public interface ILinePayTransactionRepository
{
    Task AddAsync(LinePayTransaction transaction);
    Task UpdateAsync(LinePayTransaction transaction);
    Task<LinePayTransaction?> GetByIdAsync(Guid id);
    Task<LinePayTransaction?> GetByOrderIdAsync(Guid orderId);
    Task<LinePayTransaction?> GetByLinePayTransactionIdAsync(long linePayTransactionId);
    Task<List<LinePayTransaction>> GetAllAsync();
}