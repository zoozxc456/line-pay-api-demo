using LinePayDemo.Ledger.Domain.Account;

namespace LinePayDemo.Ledger.Services;

public interface IAccountService
{
    Task<Account> CreateAccountAsync(string accountName);
    public Task<Account?> GetAccountByIdAsync(Guid accountId);
    public Task<List<Account>> GetAllAccountsAsync();
}