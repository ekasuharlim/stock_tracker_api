namespace StockTrackingApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using StockTrackingApi.Data;
using StockTrackingApi.Models;

[ApiController]
[Route("api/[controller]")]
public class DividendsController : ControllerBase
{
    private readonly StockTrackingDbContext _context;
    public DividendsController(StockTrackingDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public IActionResult GetDividends()
    {
        return Ok(_context.Dividends.ToList());
    }
    
    [HttpPost]
    public IActionResult CreateDividend(Dividend dividend)
    {
        _context.Dividends.Add(dividend);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetDividends), new { id = dividend.DividendID }, dividend);
    }
}
