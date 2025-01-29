namespace SalesFlow.Communication.Request.Sales;
public class RequestSaleItemCreateJson
{
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}