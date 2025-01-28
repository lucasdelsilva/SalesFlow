namespace SalesFlow.Communication.Response.Sales;

public class ResponseSalesJson
{
    public List<ResponseSaleShortJson> Sales { get; set; } = [];
    public decimal Total { get; set; }
}

public class ResponseSaleShortJson
{
    public int Number { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public string Date { get; set; } = string.Empty;
}