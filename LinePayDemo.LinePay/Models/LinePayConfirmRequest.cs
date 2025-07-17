namespace LinePayDemo.Payment.DTOs.LinePay;

public class LinePayConfirmRequest
{
    public required int Amount { get; init; }
    public required string Currency { get; init; } = "TWD";
}