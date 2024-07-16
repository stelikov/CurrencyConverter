using CurrencyConverterApp.Server.HostedService;
using CurrencyConverterApp.Server.Models;
using CurrencyConverterApp.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();
builder.Services.AddSingleton<ICurrencyService, CurrencyService>();
builder.Services.AddHostedService<CurrencyRateUpdateService>();
builder.Services.Configure<TimerSettings>(builder.Configuration.GetSection("TimerSettings"));


var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
  );

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
