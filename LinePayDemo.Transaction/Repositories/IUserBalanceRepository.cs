using LinePayDemo.Transaction.Models;

namespace LinePayDemo.Transaction.Repositories;

public interface IUserBalanceRepository
{
    Task<UserBalance?> GetByUserIdAsync(Guid userId);
    Task AddOrUpdateAsync(UserBalance userBalance);
}