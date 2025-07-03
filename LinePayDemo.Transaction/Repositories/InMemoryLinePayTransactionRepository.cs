using System.Collections.Concurrent;
using LinePayDemo.Transaction.Models;

namespace LinePayDemo.Transaction.Repositories;

public class InMemoryLinePayTransactionRepository : ILinePayTransactionRepository
{
    private static readonly ConcurrentDictionary<Guid, LinePayTransaction> Transactions = new();
    private static readonly object Locker = new();

    public Task AddAsync(LinePayTransaction transaction)
    {
        transaction.Id = Guid.NewGuid();
        Transactions[transaction.OrderId] = transaction;
        return Task.CompletedTask;
    }

    public Task UpdateAsync(LinePayTransaction transaction)
    {
        lock (Locker)
        {
            if (Transactions.ContainsKey(transaction.OrderId))
            {
                Transactions[transaction.OrderId] = transaction;
            }
            else
            {
                throw new InvalidOperationException($"找不到 OrderId 為 {transaction.OrderId} 的交易，無法更新。");
            }
        }

        return Task.CompletedTask;
    }

    public Task<LinePayTransaction?> GetByIdAsync(Guid id)
    {
        var transaction = Transactions.Values.FirstOrDefault(x=>x.Id == id);
        return Task.FromResult(transaction);
    }

    public Task<LinePayTransaction?> GetByOrderIdAsync(Guid orderId)
    {
        Transactions.TryGetValue(orderId, out var transaction);
        return Task.FromResult(transaction);
    }

    public Task<LinePayTransaction?> GetByLinePayTransactionIdAsync(long linePayTransactionId)
    {
        var transaction = Transactions.Values.FirstOrDefault(t => t.LinePayTransactionId == linePayTransactionId);
        return Task.FromResult(transaction);
    }

    public Task<List<LinePayTransaction>> GetAllAsync()
    {
        return Task.FromResult(Transactions.Values.ToList());
    }
}