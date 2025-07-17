using LinePayDemo.Transaction.Enums;

namespace LinePayDemo.Transaction.Domain;

public class LinePayDepositTransaction : DepositTransaction
{
    public Guid LinePayOrderId { get; private set; }
    public long LinePayTransactionId { get; private set; }
    public DateTime RequestedAt { get; private set; }
    public DateTime? ConfirmedAt { get; private set; }
    public DateTime? CancelledAt { get; private set; }

    private LinePayDepositTransaction()
    {
    }

    // 內部構造函數，供 TransactionService 或其他工廠方法呼叫
    internal LinePayDepositTransaction(Guid id, Guid userId, decimal amount, Guid linePayOrderId,
        long linePayTransactionId)
    {
        Id = id;
        UserId = userId;
        Amount = amount;
        Type = DepositType.LinePay;
        Status = DepositStatus.Pending;
        LinePayOrderId = linePayOrderId;
        LinePayTransactionId = linePayTransactionId;
        RequestedAt = DateTime.UtcNow;
    }

    public void UpdateOnConfirm(DateTime confirmedAt)
    {
        Status = DepositStatus.Completed;
        ConfirmedAt = confirmedAt;
        DepositedAt = confirmedAt;
    }

    public void UpdateOnCancel(DateTime cancelledAt)
    {
        Status = DepositStatus.Cancelled;
        CancelledAt = cancelledAt;
    }
}