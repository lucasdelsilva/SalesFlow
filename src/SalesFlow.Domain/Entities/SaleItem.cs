namespace SalesFlow.Domain.Entities;
public class SaleItem
{
    public long Id { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public long SaleId { get; set; }

    // Método para calcular o valor total do item
    // Pode ficar no response
    public void CalculateTotalPrice()
    {
        TotalPrice = Quantity * UnitPrice;
    }
}