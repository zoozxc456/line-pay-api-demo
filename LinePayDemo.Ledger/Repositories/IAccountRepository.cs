using LinePayDemo.Ledger.Domain.Account;

namespace LinePayDemo.Ledger.Repositories;

public interface IAccountRepository
{
    Task<Account?> GetByIdAsync(Guid id);
    Task<Account?> GetByNameAsync(string name);
    Task<List<Account>> GetAllAsync();
    Task AddAsync(Account account);
    Task UpdateAsync(Account account);
}