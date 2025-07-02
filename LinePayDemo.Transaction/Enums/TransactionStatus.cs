namespace LinePayDemo.Transaction.Enums;

public enum TransactionStatus
{
    Pending = 0,
    Completed,
    Confirmed,
    Cancelled,
    Expired,
    Failed,
}