using CurrencyConverterApp.Server.Services;
using Microsoft.AspNetCore.Hosting;
using Moq;
using Moq.Protected;

namespace CurrencyConverter.Tests;
public class CurrencyServiceTests
{
    private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
    private readonly Mock<IWebHostEnvironment> _webHostEnvironmentMock;
    private readonly CurrencyService _currencyService;

    public CurrencyServiceTests()
    {
        _httpClientFactoryMock = new Mock<IHttpClientFactory>();
        _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
        _webHostEnvironmentMock.Setup(env => env.ContentRootPath).Returns(Directory.GetCurrentDirectory());
        _currencyService = new CurrencyService(_httpClientFactoryMock.Object, _webHostEnvironmentMock.Object);
    }

    [Fact]
    public async Task UpdateRates_ShouldWriteRatesToFile()
    {
        // Arrange
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent("AUD\t1.60880000\nBGN\t1.95580000\n")
            });

        var client = new HttpClient(handlerMock.Object);
        _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

        // Act
        await _currencyService.UpdateRates();

        // Assert
        var date = DateTime.UtcNow.ToString("yyyy-MM-dd");
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", $"{date}.txt");
        Assert.True(File.Exists(filePath));

        var lines = await File.ReadAllLinesAsync(filePath);
        Assert.Contains("AUD\t1.60880000", lines);
        Assert.Contains("BGN\t1.95580000", lines);
    }

    [Fact]
    public void GetRatesByDate_ShouldReturnRates()
    {
        // Arrange
        var date = DateTime.UtcNow.ToString("yyyy-MM-dd");
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", $"{date}.txt");
        Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Data"));
        File.WriteAllLines(filePath, new[] { "AUD\t1.60880000", "BGN\t1.95580000" });

        // Act
        var rates = _currencyService.GetRatesByDate(DateTime.UtcNow);

        // Assert
        Assert.Equal(1.60880000m, rates["AUD"]);
        Assert.Equal(1.95580000m, rates["BGN"]);
    }
}
