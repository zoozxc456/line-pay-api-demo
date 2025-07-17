namespace LinePayDemo.Payment.DTOs.LinePay;

public class LinePayRequest
{
    public required decimal Amount { get; set; }
    public required string Currency { get; set; }
    public required Guid OrderId { get; set; }

    public required List<LinePayRequestPackage> Packages { get; set; } = [];
    public required LinePayRequestRedirectUrls RedirectUrls { get; set; }

    public class LinePayRequestPackage
    {
        public required Guid Id { get; set; }
        public required decimal Amount { get; set; }
        public required List<LinePayRequestProduct> Products { get; set; } = [];
    }

    public class LinePayRequestProduct
    {
        public required Guid Id { get; set; }
        public required int Quantity { get; set; }
        public required decimal Price { get; set; }
        public required string Name { get; set; }
        public required string ImageUrl { get; set; }
    }

    public class LinePayRequestRedirectUrls
    {
        public required string ConfirmUrl { get; set; }
        public required string CancelUrl { get; set; }
    }
}