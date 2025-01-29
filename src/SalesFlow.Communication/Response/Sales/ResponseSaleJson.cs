namespace SalesFlow.Communication.Response.Sales;
public class ResponseSaleJson
{
    public long Id { get; set; }
    public int Number { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public int QuantityItems { get; set; }
    public decimal TotalAmount { get; set; }
    public string Date { get; set; } = string.Empty;
    public List<ResponseSaleItemJson> Items { get; set; } = [];
}

public class ResponseSaleItemJson
{
    public long Id { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
}