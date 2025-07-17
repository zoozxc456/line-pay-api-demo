using LinePayDemo.Ledger.Domain.Transaction;
using LinePayDemo.Ledger.DTOs;
using LinePayDemo.Ledger.Repositories;

namespace LinePayDemo.Ledger.Services;

public class TransactionService(ITransactionRepository transactionRepository, IAccountRepository accountRepository)
    : ITransactionService
{
    public async Task<Transaction> PostTransactionAsync(string description, List<TransactionEntryDto> entriesData)
    {
        if (!CheckIsBalance(entriesData)) throw new Exception("");
        var amount = entriesData.Sum(e => e.Debit);

        var transaction = new Transaction(amount, description);
        foreach (var entryData in entriesData)
        {
            var account = await accountRepository.GetByIdAsync(entryData.AccountId);
            if (account == null)
            {
                Console.WriteLine($"錯誤：找不到帳戶 ID: {entryData.AccountId}。此交易將無法完成。");
                throw new Exception(); // 或者拋出特定例外
            }

            // 更新帳戶餘額
            account.Balance += entryData.Credit;
            account.Balance -= entryData.Debit;
            await accountRepository.UpdateAsync(account); // 標記帳戶為待更新狀態

            // 為交易新增分錄
            transaction.Entries.Add(new TransactionEntry(transaction.Id,
                entryData.AccountId,
                entryData.Debit,
                entryData.Credit)
            );
        }

        // 4. 將新的交易加入倉儲
        await transactionRepository.AddAsync(transaction);
        Console.WriteLine($"成功記錄交易 (ID: {transaction.Id})：'{transaction.Description}'，金額：{transaction.Amount:C}");
        return transaction;
    }

    private static bool CheckIsBalance(List<TransactionEntryDto> entriesData)
    {
        var totalDebit = entriesData.Sum(e => e.Debit);
        var totalCredit = entriesData.Sum(e => e.Credit);
        if (totalDebit == totalCredit) return true;

        Console.WriteLine($"錯誤：交易不平衡！借方總額 {totalDebit} 不等於貸方總額 {totalCredit}。");
        return false;
    }

    public Task<Transaction?> GetTransactionByIdAsync(Guid transactionId)
    {
        return transactionRepository.GetByIdAsync(transactionId);
    }

    public Task<List<Transaction>> GetAllTransactionsAsync()
    {
        return transactionRepository.GetAllAsync();
    }
}