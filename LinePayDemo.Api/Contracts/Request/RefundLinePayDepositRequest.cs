namespace LinePayDemo.Api.Contracts.Request;

public class RefundLinePayDepositRequest
{
    public Guid UserId { get; set; }
    public Guid OrderId { get; set; }
    public long TransactionId { get; set; }
    public decimal? RefundAmount { get; set; }
}