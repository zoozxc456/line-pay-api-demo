using LinePayDemo.Transaction.Enums;
using LinePayDemo.Transaction.Models;

namespace LinePayDemo.Transaction.Services;

public interface ITransactionService
{
    Task<LinePayTransaction> CreateLinePayTransactionAsync(Guid userId, decimal amount, TransactionStatus status);

    Task UpdateLinePayTransactionDetailsAsync(Guid orderId, long linePayTransactionId);

    Task UpdateLinePayTransactionStatusAsync(Guid orderId, TransactionStatus status);

    Task<LinePayTransaction?> GetLinePayTransactionByOrderIdAsync(Guid orderId);

    // 用於使用者點數管理
    Task<decimal> GetUserBalanceAsync(Guid userId);
    Task AddPointsToUserBalanceAsync(Guid userId, decimal amount);
    Task<DeductionResult> DeductPointsAsync(Guid userId, decimal amount);
}

public class DeductionResult
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public decimal RemainingBalance { get; set; }

    public static DeductionResult Success(decimal remainingBalance) => new DeductionResult
        { IsSuccess = true, RemainingBalance = remainingBalance };

    public static DeductionResult Fail(string message, decimal remainingBalance = 0) => new DeductionResult
        { IsSuccess = false, Message = message, RemainingBalance = remainingBalance };
}