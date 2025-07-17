namespace LinePayDemo.Payment.Enums;

public enum LinePayTransactionStatus
{
    Created = 0,
    Pending,
    Confirmed,
    Cancelled,
    Failed,
    Refunded
}