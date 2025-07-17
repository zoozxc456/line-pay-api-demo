using LinePayDemo.Payment.Enums;
using LinePayDemo.Ledger.Domain.Transaction;

namespace LinePayDemo.Payment.Domain.UserPoint;

public class UserPointTransaction
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public UserPointTransactionType Type { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime TransactionDate { get; private set; }
    public string Description { get; private set; }
    public Guid? LedgerTransactionId { get; private set; }
    public Transaction? LedgerTransaction { get; private set; }

    private UserPointTransaction()
    {
    }

    public UserPointTransaction(Guid userId, UserPointTransactionType type, decimal amount, string description = "")
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Type = type;
        Amount = amount;
        TransactionDate = DateTime.UtcNow;
        Description = description;
    }

    public void AttachLedgerTransaction(Guid ledgerTransactionId)
    {
        LedgerTransactionId = ledgerTransactionId;
    }
}