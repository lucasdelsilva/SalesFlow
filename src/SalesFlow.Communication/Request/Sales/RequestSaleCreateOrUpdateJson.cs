namespace SalesFlow.Communication.Request.Sales;

public class RequestSaleCreateOrUpdateJson
{
    public string CustomerName { get; set; } = string.Empty;

    public List<SaleItemRequest> Items { get; set; } = [];
}

public class SaleItemRequest
{
    public string ProductName { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }
}