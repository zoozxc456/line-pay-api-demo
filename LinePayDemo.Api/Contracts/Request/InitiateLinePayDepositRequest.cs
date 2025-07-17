namespace LinePayDemo.Api.Contracts.Request;

public class InitiateLinePayDepositRequest
{
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    public string Platform { get; set; } = "app";
}