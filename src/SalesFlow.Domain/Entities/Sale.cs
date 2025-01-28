namespace SalesFlow.Domain.Entities;
public class Sale
{
    public long Id { get; set; }
    public int Number { get; set; }
    public DateTime Date { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public List<SaleItem?> Items { get; set; } = [];
}