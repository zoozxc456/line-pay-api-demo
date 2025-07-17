namespace LinePayDemo.Payment.DTOs.LinePay;

public class LinePayConfirmResponse
{
    public string ReturnCode { get; set; }
    public string ReturnMessage { get; set; }
    public LinePayConfirmInfo Info { get; set; }
}

public class LinePayConfirmInfo
{
    public Guid OrderId { get; set; }
    public long TransactionId { get; set; }
    public List<LinePayConfirmPayInfo> PayInfo { get; set; } = [];
    public List<LinePayConfirmPackage> Packages { get; set; } = [];

    public class LinePayConfirmPayInfo
    {
        public string Method { get; set; }
        public int Amount { get; set; }
        public string MaskedCreditCardNumber { get; set; }
    }

    public class LinePayConfirmPackage
    {
        public Guid Id { get; set; }
        public int Amount { get; set; }
        public int UserFeeAmount { get; set; }
        public List<LinePayConfirmProduct> Products { get; set; } = [];
    }

    public class LinePayConfirmProduct
    {
        public required Guid Id { get; set; }
        public required int Quantity { get; set; }
        public required int Price { get; set; }
        public required string Name { get; set; }
        public string? ImageUrl { get; set; }
    }
}