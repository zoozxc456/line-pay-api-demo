using LinePayDemo.Infrastructure.Persistence.Contexts;
using LinePayDemo.User.Repositories;

namespace LinePayDemo.Infrastructure.Persistence.Repositories;

public class UserRepository(ShoppingMallContext context) : GenericRepository<User.Domain.User>(context), IUserRepository
{
    public Task<User.Domain.User?> GetByAccountAsync(string account)
    {
        return FindAsync(x => x.Account == account);
    }

    public new Task AddAsync(User.Domain.User user)
    {
        return base.AddAsync(user);
    }

    public new Task UpdateAsync(User.Domain.User user)
    {
        return base.UpdateAsync(user);
    }

    public Task<User.Domain.User?> GetByIdAsync(Guid id)
    {
        return FindAsync(x => x.Id == id);
    }

    public Task<List<User.Domain.User>> GetAllAsync()
    {
        return FindAllAsync();
    }
}