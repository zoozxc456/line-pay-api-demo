using LinePayDemo.Ledger.Domain.Account;
using LinePayDemo.Ledger.Repositories;

namespace LinePayDemo.Ledger.Services;

public class AccountService(IAccountRepository accountRepository) : IAccountService
{
    private const decimal InitialBalance = 0;

    public async Task<Account> CreateAccountAsync(string accountName)
    {
        var account = new Account(accountName, InitialBalance);
        await accountRepository.AddAsync(account);

        return account;
    }

    public Task<Account?> GetAccountByIdAsync(Guid accountId)
    {
        return accountRepository.GetByIdAsync(accountId);
    }

    public Task<List<Account>> GetAllAccountsAsync()
    {
        return accountRepository.GetAllAsync();
    }
}