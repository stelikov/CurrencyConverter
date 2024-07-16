using CurrencyConverterApp.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyConverterApp.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CurrencyController : ControllerBase
{
    private readonly ICurrencyService _currencyService;

    public CurrencyController(ICurrencyService currencyService)
    {
        _currencyService = currencyService;
    }

    [HttpGet("convert")]
    public async Task<IActionResult> Convert(string fromCurrency, string toCurrency, decimal amount, DateTime date)
    {
        try
        {
            var rates = _currencyService.GetRatesByDate(date);
            var fromRate = rates[fromCurrency];
            var toRate = rates[toCurrency];

            var convertedAmount = amount * (toRate / fromRate);
            var roundedAmount = Math.Round(convertedAmount, 4);
            return Ok(roundedAmount);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("rates")]
    public async Task<IActionResult> GetRates(DateTime date)
    {
        try
        {
            var rates = _currencyService.GetRatesListByDate(date);
            return Ok(rates);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}
