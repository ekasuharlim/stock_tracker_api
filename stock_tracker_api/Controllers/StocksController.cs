namespace StockTrackingApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using StockTrackingApi.Data;
using StockTrackingApi.Models;

[ApiController]
[Route("api/[controller]")]
public class StocksController : ControllerBase
{
    private readonly StockTrackingDbContext _context;
    public StocksController(StockTrackingDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public IActionResult GetStocks()
    {
        return Ok(_context.Stocks.ToList());
    }
    
    [HttpPost]
    public IActionResult CreateStock(Stock stock)
    {
        _context.Stocks.Add(stock);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetStocks), new { id = stock.StockID }, stock);
    }
}
