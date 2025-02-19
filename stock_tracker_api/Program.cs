using Microsoft.EntityFrameworkCore;
using StockTrackingApi.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

string? connectionString  = builder.Configuration.GetConnectionString("stockApiDb");
var logger = LoggerFactory.Create(
    logging => {
        logging.AddConsole();
        logging.AddDebug();
        logging.SetMinimumLevel(LogLevel.Debug);
    }).CreateLogger("Program");

logger.LogDebug("DEBUG: Application is starting...");
logger.LogDebug($"Using Connection String: {connectionString}");

builder.Services.AddDbContext<StockTrackingDbContext>(
    options => options.UseSqlServer(connectionString)
);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var appLogger = app.Logger;
app.Use(async (context, next) =>
{
    appLogger.LogInformation($"Handling request: {context.Request.Method} {context.Request.Path}");
    await next();
    appLogger.LogInformation($"Response status: {context.Response.StatusCode}");
});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
