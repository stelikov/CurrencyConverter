using System;
using CurrencyConverterApp.Server.Models;
using Newtonsoft.Json;

namespace CurrencyConverterApp.Server.Services;

public interface ICurrencyService
{
    Task UpdateRates();
    Dictionary<string, decimal> GetRatesByDate(DateTime date);
    public List<string> GetRatesListByDate(DateTime date);
}

public class CurrencyService : ICurrencyService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly string _dataPath;

    public CurrencyService(IHttpClientFactory clientFactory, IWebHostEnvironment env)
    {
        _clientFactory = clientFactory;
        _dataPath = Path.Combine(env.ContentRootPath, "Data");
        Directory.CreateDirectory(_dataPath);
    }

    public virtual async Task UpdateRates()
    {
        try
        {
            var currentDate = DateTime.Now;
            var now = currentDate.ToString("yyyyMMdd");

            var client = _clientFactory.CreateClient();
            var response = await client.GetStringAsync("https://www.bank.lv/vk/ecb.csv?date=" + now);

            var filePath = Path.Combine(_dataPath, $"{currentDate:yyyy-MM-dd}.txt");
            var rates = ProcessRates(response);

            rates.Insert(0, $"EUR\t1.00000000"); // Assuming the base currency is EUR
            await File.WriteAllLinesAsync(filePath, rates);

        }
        catch (Exception ex)
        {
            // TODO LOG EXCEPTION
        }
    }

    private List<string> ProcessRates(string csvData)
    {
        var lines = csvData.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var processedLines = new List<string>();
        foreach (var line in lines)
        {
            var parts = line.Split('\t');
            if (parts.Length == 2)
            {
                processedLines.Add($"{parts[0]}\t{parts[1].Trim('\r')}");
            }
        }
        return processedLines;
    }

    public virtual Dictionary<string, decimal> GetRatesByDate(DateTime date)
    {
        var filePath = Path.Combine(_dataPath, $"{date:yyyy-MM-dd}.txt");

        if (!File.Exists(filePath))
            throw new FileNotFoundException("Currency rates file not found for the given date");

        var lines = File.ReadAllLines(filePath);
        var rates = lines
                    .Select(line => line.Split('\t'))
                    .ToDictionary(parts => parts[0], parts => decimal.Parse(parts[1]));

        return rates;
    }
    public List<string> GetRatesListByDate(DateTime date)
    {
        var filePath = Path.Combine(_dataPath, $"{date:yyyy-MM-dd}.txt");

        if (!File.Exists(filePath))
            throw new FileNotFoundException("Currency rates file not found for the given date");

        return File.ReadAllLines(filePath).ToList();
    }
}
