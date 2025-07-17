namespace LinePayDemo.Api.Contracts.Request;

public class PurchaseRequest
{
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}