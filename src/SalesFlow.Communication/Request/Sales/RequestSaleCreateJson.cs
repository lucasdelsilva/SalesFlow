namespace SalesFlow.Communication.Request.Sales;

public class RequestSaleCreateJson
{
    public string CustomerName { get; set; } = string.Empty;

    public List<RequestSaleItemCreateJson> Items { get; set; } = [];
}