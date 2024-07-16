namespace CurrencyConverterApp.Server.Models;

public class CurrencyRate
{
    public string Currency { get; set; }
    public decimal Rate { get; set; }
    public DateTime Date { get; set; }
}