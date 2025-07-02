using LinePayDemo.Transaction.Enums;
using LinePayDemo.Transaction.Models;
using LinePayDemo.Transaction.Repositories;
using Microsoft.Extensions.Logging;

namespace LinePayDemo.Transaction.Services;

public class TransactionService(
    ILinePayTransactionRepository linePayTransactionRepository,
    IUserBalanceRepository userBalanceRepository,
    ILogger<TransactionService> logger)
    : ITransactionService
{
    public async Task<LinePayTransaction> CreateLinePayTransactionAsync(
        Guid userId,
        decimal amount,
        TransactionStatus status)
    {
        var orderId = Guid.NewGuid();
        var transaction = new LinePayTransaction
        {
            OrderId = orderId,
            UserId = userId,
            Amount = amount,
            Currency = "TWD",
            Status = status,
            RequestDateTime = DateTime.Now
        };

        await linePayTransactionRepository.AddAsync(transaction);
        logger.LogInformation($"LinePayTransaction {orderId} 以狀態 {status} 建立。");
        return transaction;
    }

    public async Task UpdateLinePayTransactionDetailsAsync(Guid orderId, long linePayTransactionId)
    {
        var transaction = await linePayTransactionRepository.GetByOrderIdAsync(orderId);
        if (transaction != null)
        {
            transaction.LinePayTransactionId = linePayTransactionId;
            await linePayTransactionRepository.UpdateAsync(transaction);
            logger.LogInformation($"LinePayTransaction {orderId} 詳細資訊已更新。");
        }
    }

    public async Task UpdateLinePayTransactionStatusAsync(Guid orderId, TransactionStatus status)
    {
        var transaction = await linePayTransactionRepository.GetByOrderIdAsync(orderId);
        if (transaction != null)
        {
            transaction.Status = status;

            await linePayTransactionRepository.UpdateAsync(transaction);
            logger.LogInformation($"LinePayTransaction {orderId} 狀態已更新為 {status}。");
        }
    }

    public Task<LinePayTransaction?> GetLinePayTransactionByOrderIdAsync(Guid orderId)
    {
        return linePayTransactionRepository.GetByOrderIdAsync(orderId);
    }

    public async Task<decimal> GetUserBalanceAsync(Guid userId)
    {
        var userBalance = await userBalanceRepository.GetByUserIdAsync(userId);
        return userBalance.Balance;
    }

    public async Task AddPointsToUserBalanceAsync(Guid userId, decimal amount)
    {
        var userBalance = await userBalanceRepository.GetByUserIdAsync(userId);
        userBalance.Balance += amount;
        await userBalanceRepository.AddOrUpdateAsync(userBalance);
        logger.LogInformation($"使用者 {userId} 餘額增加 {amount}。新餘額：{userBalance.Balance}");
    }

    public async Task<DeductionResult> DeductPointsAsync(Guid userId, decimal amount)
    {
        var userBalance = await userBalanceRepository.GetByUserIdAsync(userId);
        if (userBalance.Balance >= amount)
        {
            userBalance.Balance -= amount;
            await userBalanceRepository.AddOrUpdateAsync(userBalance);
            logger.LogInformation($"使用者 {userId} 餘額減少 {amount}。新餘額：{userBalance.Balance}");
            return DeductionResult.Success(userBalance.Balance);
        }

        logger.LogWarning($"使用者 {userId} 嘗試扣除 {amount} 但只有 {userBalance.Balance}。");
        return DeductionResult.Fail("點數不足。", userBalance.Balance);
    }
}