namespace SalesFlow.Application.AutoMapper.Helpers;
public static class DecimalExtensions
{
    public static decimal RoundToTwo(this decimal value)
    {
        return Math.Round(value, 2);
    }
}