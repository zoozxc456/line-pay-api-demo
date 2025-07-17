using LinePayDemo.Transaction.Enums;

namespace LinePayDemo.Transaction.Domain;

public class OnPremiseDepositTransaction : DepositTransaction
{
    public string Cashier { get; private set; }
    public string ReceiptNumber { get; private set; }

    private OnPremiseDepositTransaction()
    {
    }

    public OnPremiseDepositTransaction(Guid userId, decimal amount, string cashier, string receiptNumber)
    {
        UserId = userId;
        Amount = amount;
        Type = DepositType.OnPremise;
        Status = DepositStatus.Completed;
        DepositedAt = DateTime.UtcNow;
        Cashier = cashier;
        ReceiptNumber = receiptNumber;
    }
}