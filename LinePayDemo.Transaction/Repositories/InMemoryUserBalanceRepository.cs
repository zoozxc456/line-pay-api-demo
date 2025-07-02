using System.Collections.Concurrent;
using LinePayDemo.Transaction.Models;

namespace LinePayDemo.Transaction.Repositories;

public class InMemoryUserBalanceRepository : IUserBalanceRepository
{
    private static readonly ConcurrentDictionary<Guid, UserBalance> UserBalances = new();

    public Task<UserBalance?> GetByUserIdAsync(Guid userId)
    {
        UserBalances.TryGetValue(userId, out var balance);
        // 如果找不到餘額則初始化 (適用於新使用者)
        if (balance == null)
        {
            balance = new UserBalance { UserId = userId, Balance = 0 };
            UserBalances.TryAdd(userId, balance);
        }

        return Task.FromResult<UserBalance?>(balance);
    }

    public Task AddOrUpdateAsync(UserBalance userBalance)
    {
        UserBalances[userBalance.UserId] = userBalance; // 覆寫或新增
        return Task.CompletedTask;
    }
}