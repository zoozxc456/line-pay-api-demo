using LinePayDemo.Transaction.Enums;

namespace LinePayDemo.Transaction.Domain;

public abstract class DepositTransaction
{
    public Guid Id { get; protected set; }
    public Guid UserId { get; protected set; }
    public decimal Amount { get; protected set; }
    public string Currency { get; protected set; }
    public DepositType Type { get; protected set; }
    public DepositStatus Status { get; protected set; }
    public DateTime? DepositedAt { get; protected set; }

    protected DepositTransaction()
    {
        Id = Guid.NewGuid();
    }

    public void MarkAsCompleted()
    {
        if (Status != DepositStatus.Pending)
        {
            throw new InvalidOperationException($"只有待處理的儲值交易才能標記為已完成。目前狀態: {Status}");
        }

        Status = DepositStatus.Completed;
    }

    public void MarkAsFailed()
    {
        if (Status == DepositStatus.Completed)
        {
            throw new InvalidOperationException("已完成的儲值交易不能標記為失敗。");
        }

        Status = DepositStatus.Failed;
    }

    public void MarkAsCancelled()
    {
        if (Status == DepositStatus.Completed)
        {
            throw new InvalidOperationException("已完成的儲值交易不能標記為取消。");
        }

        Status = DepositStatus.Cancelled;
    }
}