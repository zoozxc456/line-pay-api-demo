using LinePayDemo.Infrastructure.Persistence.Contexts;
using LinePayDemo.Payment.Domain.UserPoint;
using LinePayDemo.Payment.Interfaces.Repositories;

namespace LinePayDemo.Infrastructure.Persistence.Repositories;

public class UserPointTransactionRepository(ShoppingMallContext context)
    : GenericRepository<UserPointTransaction>(context), IUserPointTransactionRepository
{
    public new Task AddAsync(UserPointTransaction transaction)
    {
        return base.AddAsync(transaction);
    }

    public Task<List<UserPointTransaction>> GetAllByUserIdAsync(Guid userId)
    {
        return FindAllAsync(x => x.UserId == userId);
    }
}