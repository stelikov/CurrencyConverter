namespace CurrencyConverterApp.Server.Models;

public class CurrencyApiResponse
{
    public Dictionary<string, decimal> Rates { get; set; }
    public DateTime Date { get; set; }
}