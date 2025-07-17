using LinePayDemo.Ledger.Domain.Transaction;
using LinePayDemo.Ledger.DTOs;

namespace LinePayDemo.Ledger.Services;

public interface ITransactionService
{
    public Task<Transaction> PostTransactionAsync(string description, List<TransactionEntryDto> entriesData);
    public Task<Transaction?> GetTransactionByIdAsync(Guid transactionId);
    public Task<List<Transaction>> GetAllTransactionsAsync();
}