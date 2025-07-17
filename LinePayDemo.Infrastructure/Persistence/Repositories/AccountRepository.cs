using LinePayDemo.Infrastructure.Persistence.Contexts;
using LinePayDemo.Ledger.Domain.Account;
using LinePayDemo.Ledger.Repositories;

namespace LinePayDemo.Infrastructure.Persistence.Repositories;

public class AccountRepository(ShoppingMallContext context) : GenericRepository<Account>(context), IAccountRepository
{
    public Task<Account?> GetByIdAsync(Guid id)
    {
        return FindAsync(x => x.Id == id);
    }

    public Task<Account?> GetByNameAsync(string name)
    {
        return FindAsync(x => x.Name == name);
    }

    public Task<List<Account>> GetAllAsync()
    {
        return FindAllAsync();
    }

    public new Task AddAsync(Account account)
    {
        return base.AddAsync(account);
    }

    public new Task UpdateAsync(Account account)
    {
        return base.UpdateAsync(account);
    }
}