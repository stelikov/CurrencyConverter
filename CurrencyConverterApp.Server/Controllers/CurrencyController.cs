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

    [HttpGet("update-rates")]
    public async Task<IActionResult> UpdateRates()
    {
        await _currencyService.UpdateRates();
        return Ok();
    }

    [HttpGet("convert")]
    public async Task<IActionResult> Convert(string fromCurrency, string toCurrency, decimal amount, DateTime date)
    {
        try
        {
            await _currencyService.UpdateRates();

            var rates = _currencyService.GetRatesByDate(date);
            var fromRate = rates[fromCurrency];
            var toRate = rates[toCurrency];

            var convertedAmount = amount * (toRate / fromRate);
            return Ok(convertedAmount.ToString("#.####"));
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}
