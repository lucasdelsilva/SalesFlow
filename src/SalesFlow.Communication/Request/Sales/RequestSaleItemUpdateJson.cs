﻿namespace SalesFlow.Communication.Request.Sales;
public class RequestSaleItemUpdateJson
{
    public long Id { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}