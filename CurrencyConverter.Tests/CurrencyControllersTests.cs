using CurrencyConverterApp.Server.Controllers;
using CurrencyConverterApp.Server.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CurrencyConverter.Tests;

public class CurrencyControllerTests
{
    private readonly Mock<CurrencyService> _currencyServiceMock;
    private readonly CurrencyController _currencyController;

    public CurrencyControllerTests()
    {
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        var webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
        webHostEnvironmentMock.Setup(env => env.ContentRootPath).Returns(Directory.GetCurrentDirectory());

        _currencyServiceMock = new Mock<CurrencyService>(httpClientFactoryMock.Object, webHostEnvironmentMock.Object);

        _currencyController = new CurrencyController(_currencyServiceMock.Object);
    }

    [Fact]
    public async void Convert_ShouldReturnConvertedAmount()
    {
        // Arrange
        var fromCurrency = "USD";
        var toCurrency = "EUR";
        var amount = 100m;
        var date = new DateTime(2023, 1, 1);
        var expectedConvertedAmount = 91.8274m;

        var service = new Mock<ICurrencyService>();
        service.Setup(s => s.GetRatesByDate(date)).Returns(new Dictionary<string, decimal>
        {
            { "USD", 1.0890m },
            { "EUR", 1.0000m }
        });

        var controller = new CurrencyController(service.Object);

        // Act
        var result = await controller.Convert(fromCurrency, toCurrency, amount, date) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedConvertedAmount, result.Value);
    }

    [Fact(Skip = "Not Finished")]
    public async Task Convert_ShouldReturnNotFound_WhenRatesNotFound()
    {
        // Arrange
        _currencyServiceMock.Setup(service => service.GetRatesByDate(It.IsAny<DateTime>())).Throws<FileNotFoundException>();

        // Act
        var result = await _currencyController.Convert("AUD", "USD", 100, DateTime.UtcNow);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }
}