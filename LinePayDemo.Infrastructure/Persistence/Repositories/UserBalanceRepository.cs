using System.Collections.Concurrent;
using LinePayDemo.Infrastructure.Persistence.Contexts;
using LinePayDemo.Transaction.Models;
using LinePayDemo.Transaction.Repositories;

namespace LinePayDemo.Infrastructure.Persistence.Repositories;

public class UserBalanceRepository(ShoppingMallContext context)
    : GenericRepository<UserBalance>(context), IUserBalanceRepository
{
    public Task<UserBalance?> GetByUserIdAsync(Guid userId)
    {
        return FindAsync(x => x.UserId == userId);
    }

    public async Task AddOrUpdateAsync(UserBalance userBalance)
    {
        var balance = await FindAsync(x => x.UserId == userBalance.UserId);

        if (balance is null)
        {
            await AddAsync(userBalance);
        }
        else
        {
            await UpdateAsync(userBalance);
        }
    }
}