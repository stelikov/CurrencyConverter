using CurrencyConverterApp.Server.Models;
using CurrencyConverterApp.Server.Services;
using Microsoft.Extensions.Options;

namespace CurrencyConverterApp.Server.HostedService;

public class CurrencyRateUpdateService : IHostedService, IDisposable
{
    private readonly ILogger<CurrencyRateUpdateService> _logger;
    private readonly ICurrencyService _currencyService;
    private readonly TimerSettings _timerSettings;
    private Timer _timer;

    public CurrencyRateUpdateService(ILogger<CurrencyRateUpdateService> logger, ICurrencyService currencyService, IOptions<TimerSettings> timerSettings)
    {
        _logger = logger;
        _currencyService = currencyService;
        _timerSettings = timerSettings.Value;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Currency Rate Update Service running.");

        var interval = TimeSpan.FromHours(_timerSettings.IntervalInHours);
        _timer = new Timer(DoWork, null, TimeSpan.Zero, interval);

        return Task.CompletedTask;
    }
    private async void DoWork(object state)
    {
        _logger.LogInformation("Updating currency rates...");
        await _currencyService.UpdateRates();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}