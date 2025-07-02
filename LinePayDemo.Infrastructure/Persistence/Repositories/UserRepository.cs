using LinePayDemo.Infrastructure.Persistence.Contexts;
using LinePayDemo.User.Repositories;

namespace LinePayDemo.Infrastructure.Persistence.Repositories;

public class UserRepository(ShoppingMallContext context) : GenericRepository<User.Models.User>(context), IUserRepository
{
    public Task<User.Models.User?> GetByAccountAsync(string account)
    {
        return FindAsync(x => x.Account == account);
    }

    public new Task AddAsync(User.Models.User user)
    {
        return base.AddAsync(user);
    }

    public new Task UpdateAsync(User.Models.User user)
    {
        return base.UpdateAsync(user);
    }

    public Task<User.Models.User?> GetByIdAsync(Guid id)
    {
        return FindAsync(x => x.Id == id);
    }

    public Task<List<User.Models.User>> GetAllAsync()
    {
        return FindAllAsync();
    }
}