using LinePayDemo.Payment.Domain.UserPoint;

namespace LinePayDemo.Payment.Interfaces.Repositories;

public interface IUserPointTransactionRepository
{
    Task AddAsync(UserPointTransaction transaction);
    Task<List<UserPointTransaction>> GetAllByUserIdAsync(Guid userId);
}