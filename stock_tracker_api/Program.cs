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
app.UseAuthorization();
app.MapControllers();


app.Run();

